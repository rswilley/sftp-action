using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace ssh_action.Tests;

public class WorkerTests
{
    [Fact]
    public void ShouldRunIntegrationTest()
    {
        var privateKeyString = File.ReadAllText("rsa.key");
        var secrets = JsonConvert.DeserializeObject<Secrets>(File.ReadAllText("secrets.json"));
        Worker.Execute(new ActionInputs
        {
            Host = secrets.hostname,
            Username = secrets.username,
            Command = "whoami"
        }, privateKeyString);
    }
}

public class Secrets
{
    public string hostname { get; set; } = null!;
    public string username { get; set; } = null!;
}