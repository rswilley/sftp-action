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

    [Option('c', "commands",
        Required = true,
        HelpText = "Commands to run via SSH.")]
    public IEnumerable<string> Commands { get; set; } = null!;
}