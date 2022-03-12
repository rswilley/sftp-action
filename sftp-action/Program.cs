using CommandLine;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;

namespace sftp_action
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<ActionInputs>(args)
                   .WithParsed(input =>
                   {
                       Execute(input);
                   });
        }

        private static void Execute(ActionInputs input)
        {
            var privateKey = new MemoryStream();
            using (var writer = new StreamWriter(privateKey))
            {
                writer.Write(Environment.GetEnvironmentVariable("PRIVATEKEY"));
                writer.Flush();
                privateKey.Seek(0, SeekOrigin.Begin);

                using (var client = new SshClient(input.Host, input.Username, new PrivateKeyFile(privateKey)))
                {
                    client.HostKeyReceived += (sender, e) =>
                    {
                        e.CanTrust = true;
                    };
                    client.Connect();

                    foreach (var (commandName, commandText) in GetCommands(input))
                    {
                        using (var cmd = client.CreateCommand(commandText))
                        {
                            var runningCommandName = commandText;
                            if (!string.IsNullOrEmpty(commandName))
                                runningCommandName = commandName;

                            Console.WriteLine($"Running: {runningCommandName}");
                            var result = cmd.Execute();

                            if (!string.IsNullOrEmpty(result))
                                Console.WriteLine(result);
                        }
                    }

                    client.Disconnect();
                }
            }

            Environment.Exit(0);
        }

        private static List<(string name, string command)> GetCommands(ActionInputs input)
        {
            return new List<(string, string)>
            {
                ("git clone", $"git clone https:/{input.Username}:{input.Githubtoken}@github.com/{input.Username}/{input.Repo}.git"),
                ("", $"cd {input.Repo} && dotnet publish -c Release -o deploy/"),
                ("", $"sudo systemctl stop {input.Repo}.service"),
                ("", $"sudo rsync -a ~/{input.Repo}/deploy/ /var/www/{input.Repo}.com"),
                ("", $"sudo systemctl start {input.Repo}.service"),
                ("", $"rm -rf ~/{input.Repo}")
            };
        }
    }
}
