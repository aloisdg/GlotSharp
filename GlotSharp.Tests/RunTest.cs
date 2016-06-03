using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using GlotSharp.Run;

namespace GlotSharp.Tests {
    [TestFixture]
    public class RunTest {
        private readonly Client _client = new Client();

        // todo: rewrite this one
        [Test (Description = "Test count languages")]
        public async Task TestLanguagesCount() {
            var versions = await _client.GetLanguagesAsync ().ConfigureAwait (false);
            var count = versions.Count ();

            Assert.AreEqual (31, count);
        }

        [TestCase (LanguageType.Python, ExpectedResult = "2,latest", Description = "Test Python")]
        [TestCase (LanguageType.Assembly, ExpectedResult = "latest", Description = "Test Assembly")]
        public async Task<string> TestVersionsAsync(LanguageType language) {
            var versions = await _client.GetVersionsAsync (language).ConfigureAwait (false);
            return string.Join (",", versions.Select (x => x.Version));
        }

        [TestCase (LanguageType.Python, "main.py", "print(42)", ExpectedResult = "42\n", Description = "Test Python")]
        [TestCase (LanguageType.Fsharp, "main.fs", "printfn \"42\"", ExpectedResult = "42\n", Description = "Test Fsharp")]
        public async Task<string> TestRunAsync(LanguageType language, string name, string content) {
            var response = await _client.RunAsync (new Request {
                Files = new[] {
                        new File {
                            Name = name,
                            Content = content
                        }
                    }
            }, language).ConfigureAwait (false);
            return response.StandardOutput;
        }
    }
}