using CommandLine;
using System.Collections.Generic;

namespace ssh_action;

public class ActionInputs
{
    [Option('h', "hostname",
        Required = true,
        HelpText = "The hostname of the remote machine.")]
    public string Host { get; set; } = null!;

    [Option('u', "username",
        Required = true,
        HelpText = "The username to login.")]
    public string Username { get; set; } = null!;

    [Option('c', "command",
        Required = true,
        HelpText = "Command to run via SSH.")]
    public string Command { get; set; } = null!;
}