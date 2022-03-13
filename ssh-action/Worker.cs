using Renci.SshNet;
using System;
using System.IO;
using System.Linq;

namespace ssh_action;

public class Worker
{
    public static void Execute(ActionInputs input, string privateKeyString)
    {
        var privateKeyStream = new MemoryStream();
        using var writer = new StreamWriter(privateKeyStream);
        writer.Write(privateKeyString);
        writer.Flush();
        privateKeyStream.Seek(0, SeekOrigin.Begin);

        using var client = new SshClient(input.Host, input.Username, new PrivateKeyFile(privateKeyStream));
        client.HostKeyReceived += (sender, e) =>
        {
            e.CanTrust = true;
        };
        client.Connect();

        foreach (var command in input.Command.Split(Environment.NewLine)
            .Where(c => !string.IsNullOrEmpty(c)))
        {
            using var cmd = client.CreateCommand(command);
            Console.WriteLine($"Running: {command}");
            var result = cmd.Execute();
            if (cmd.ExitStatus == 0 && !string.IsNullOrEmpty(result))
            {
                Console.WriteLine(result);
            }
            else if (cmd.ExitStatus != 0)
            {
                throw new Exception($"Error: {cmd.Error}");
            }
        }

        client.Disconnect();
    }
}