using CommandLine;
using System;

namespace sftp_action
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<ActionInputs>(args)
                   .WithParsed(input =>
                   {
                       try
                       {
                           new Worker().Execute(input, Environment.GetEnvironmentVariable("PRIVATEKEY"));
                           Environment.Exit(0);
                       }
                       catch (Exception ex)
                       {
                           Console.WriteLine(ex.Message);
                           Environment.Exit(1);
                       }
                   });
        }
    }
}
