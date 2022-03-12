using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;

namespace sftp_action
{
    public class Worker
    {
        public void Execute(ActionInputs input, string privateKeyString)
        {
            var privateKeyStream = new MemoryStream();
            using (var writer = new StreamWriter(privateKeyStream))
            {
                writer.Write(privateKeyString);
                writer.Flush();
                privateKeyStream.Seek(0, SeekOrigin.Begin);

                using (var client = new SshClient(input.Host, input.Username, new PrivateKeyFile(privateKeyStream)))
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
                            if (cmd.ExitStatus == 0 && !string.IsNullOrEmpty(result))
                            {
                                Console.WriteLine(result);
                            } else if (cmd.ExitStatus != 0)
                            {
                                Console.WriteLine(cmd.Error);
                            }
                        }
                    }

                    client.Disconnect();
                }
            }
        }

        private static List<(string name, string command)> GetCommands(ActionInputs input)
        {
            return new List<(string, string)>
            {
                ("", $"rm -rf {input.Repo}"),
                ("git clone", $"git clone https://{input.Username}:{input.Githubtoken}@github.com/{input.Username}/{input.Repo}.git"),
                ("", $"cd {input.Repo} && dotnet publish -c Release -o deploy/"),
                ("", $"sudo systemctl stop {input.Repo}.service"),
                ("", $"sudo rsync -a ~/{input.Repo}/deploy/ /var/www/{input.Repo}.com"),
                ("", $"sudo systemctl start {input.Repo}.service")
            };
        }
    }
}
