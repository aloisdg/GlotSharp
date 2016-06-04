using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
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

        public Task<IEnumerable<Language>> GetLanguagesAsync() {
            return GetAsync<Language> (string.Empty);
        }

        public Task<IEnumerable<LanguageVersion>> GetVersionsAsync(LanguageType language) {
            return GetVersionsAsync (language.ToString ().ToLowerInvariant ());
        }

        public Task<IEnumerable<LanguageVersion>> GetVersionsAsync(string language) {
            return GetAsync<LanguageVersion> (language);
        }

        public Task<Response> RunAsync(Request request, LanguageType language, string version = "latest") {
            return RunAsync (request, language.ToString ().ToLowerInvariant (), version);
        }

        // todo: handle command https://github.com/prasmussen/glot-run/blob/master/api_docs/run.md#custom-run-command
        public async Task<Response> RunAsync(Request request, string language, string version = "latest") {
            var content = JsonConvert.SerializeObject (request, new JsonSerializerSettings {
                ContractResolver = new CamelCasePropertyNamesContractResolver ()
            });
            using (var httpRequest = new HttpRequestMessage (HttpMethod.Post, $"{language}/{version}") {
                Headers = {
                    Authorization = new AuthenticationHeaderValue("Token", Token),
                    Accept = { new MediaTypeWithQualityHeaderValue("application/json")} // useful?
                },
                Content = new StringContent (content, Encoding.UTF8, "application/json")
            }) {
                var httpResponse = await HttpClient.SendAsync (httpRequest).ConfigureAwait (false);
                var json = await httpResponse.Content.ReadAsStringAsync ().ConfigureAwait (false);
                var response = JsonConvert.DeserializeObject<Response> (json);
                return response;
            }
        }

        public void Dispose() {
            HttpClient.Dispose ();
        }

        private static async Task<IEnumerable<T>> GetAsync<T>(string uri) {
            var json = await HttpClient.GetStringAsync (uri).ConfigureAwait (false);
            return JsonConvert.DeserializeObject<IEnumerable<T>> (json);
        }
    }
}
