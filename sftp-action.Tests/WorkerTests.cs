using Newtonsoft.Json;
using System.IO;
using Xunit;

namespace sftp_action.Tests
{
    public class WorkerTests
    {
        [Fact]
        public void ShouldRunIntegrationTest()
        {
            var privateKeyString = File.ReadAllText("rsa.key");
            var secrets = JsonConvert.DeserializeObject<Secrets>(File.ReadAllText("secrets.json"));
            new Worker().Execute(new ActionInputs
            {
                Host = secrets.host,
                Username = secrets.username,
                Githubtoken = secrets.githubtoken,
                Repo = secrets.repo
            }, privateKeyString);
        }
    }

    public class Secrets
    {
        public string host { get; set; } = null!;
        public string username { get; set; } = null!;
        public string githubtoken { get; set; } = null!;
        public string repo { get; set; } = null!;
    }
}