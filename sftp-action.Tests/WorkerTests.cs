using System.IO;
using Xunit;

namespace sftp_action.Tests
{
    public class WorkerTests
    {
        [Fact]
        public void Test1()
        {
            var privateKeyString = File.ReadAllText("rsa.key");
            new Worker().Execute(new ActionInputs
            {
                Host = "test.d2grail.com",
                Username = "rswilley",
            }, privateKeyString);
        }
    }
}