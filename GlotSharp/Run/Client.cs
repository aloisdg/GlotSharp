using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace GlotSharp.Run {
    public partial class Client : IDisposable {
        private static readonly HttpClient HttpClient = new HttpClient {
            BaseAddress = new Uri("https://run.glot.io/languages/")
            //, DefaultRequestHeaders = { Accept = { new MediaTypeWithQualityHeaderValue("application/json") }} // useful?
        };

        public async Task<IEnumerable<Language>> GetLanguagesAsync() {
            var json = await HttpClient.GetStringAsync (string.Empty).ConfigureAwait (false);
            return JsonConvert.DeserializeObject<IEnumerable<Language>> (json);
        }

        public Task<IEnumerable<LanguageVersion>> GetVersionsAsync(LanguageType languageType) {
            return GetVersionsAsync (languageType.ToString ().ToLowerInvariant ());
        }

        public async Task<IEnumerable<LanguageVersion>> GetVersionsAsync(string language) {
            var json = await HttpClient.GetStringAsync (language).ConfigureAwait (false);
            return JsonConvert.DeserializeObject<IEnumerable<LanguageVersion>> (json);
        }

        // todo: handle command https://github.com/prasmussen/glot-run/blob/master/api_docs/run.md#custom-run-command
        public async Task<Response> RunAsync(string language, Request request) {
            var content = JsonConvert.SerializeObject (request, new JsonSerializerSettings {
                ContractResolver = new CamelCasePropertyNamesContractResolver ()
            });
            var httpRequest = new HttpRequestMessage (HttpMethod.Post, $"{language}/{version}") {
                Headers = {
                    Authorization = new AuthenticationHeaderValue("Token", Token),
                    Accept = { new MediaTypeWithQualityHeaderValue("application/json")} // useful?
                },
                Content = new StringContent (content, Encoding.UTF8, "application/json")
            };

            var httpResponse = await HttpClient.SendAsync (httpRequest).ConfigureAwait (false);
            var json = await httpResponse.Content.ReadAsStringAsync ().ConfigureAwait (false);
            var response = JsonConvert.DeserializeObject<Response> (json);
            return response;
        }

        public void Dispose() {
            HttpClient.Dispose ();
        }
    }
}
