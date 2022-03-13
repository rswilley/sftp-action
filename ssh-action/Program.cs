using CommandLine;
using ssh_action;
using System;

Parser.Default.ParseArguments<ActionInputs>(args)
    .WithParsed(input =>
    {
        try
        {
            Console.WriteLine("Commands to run: " + string.Join(',', input.Commands));
            Worker.Execute(input, Environment.GetEnvironmentVariable("PRIVATEKEY"));
            Environment.Exit(0);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Environment.Exit(1);
        }
    });