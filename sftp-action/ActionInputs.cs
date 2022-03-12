using CommandLine;

namespace sftp_action
{
    public class ActionInputs
    {
        [Option('h', "host",
            Required = true,
            HelpText = "The hostname of the remote machine.")]
        public string Host { get; set; } = null!;

        [Option('u', "username",
            Required = true,
            HelpText = "The username to login.")]
        public string Username { get; set; } = null!;

        [Option('t', "githubtoken",
            Required = true,
            HelpText = "The access token for the Github repository.")]
        public string Githubtoken { get; set; } = null!;

        [Option('r', "repo",
            Required = true,
            HelpText = "The Github repo.")]
        public string Repo { get; set; } = null!;
    }
}
