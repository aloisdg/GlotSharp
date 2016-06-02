using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlotSharp.Run;

namespace GlotSharp.Sample {
    internal class Program {
        private static void Main() {
            var t = MainAsync ();
            t.Wait ();
        }

        private static async Task MainAsync() {

            using (var client = new Client ())
            {
                var languages = await client.GetLanguagesAsync().ConfigureAwait(false);
                var language = languages.First();
                Console.WriteLine(language);

                var versions = await client.GetVersionsAsync(language.Name).ConfigureAwait(false);
                var version = versions.First();
                Console.WriteLine(version.Version);

                var pythonVersions = await client.GetVersionsAsync (LanguageType.Python).ConfigureAwait (false);
                var pythonVersion = pythonVersions.First();
                Console.WriteLine(pythonVersion.Version);
            }
            Console.ReadLine();
        }
    }
}
