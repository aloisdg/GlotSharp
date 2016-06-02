using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GlotSharp.Run {
    public class Client : IDisposable {
        private static readonly HttpClient HttpClient = new HttpClient {
            BaseAddress = new Uri("https://run.glot.io/")
            //, DefaultRequestHeaders = { Accept = { new MediaTypeWithQualityHeaderValue("application/json") }}
        };

        private const string Token = "";

        public async Task<IEnumerable<Language>> GetLanguagesAsync() {
            var json = await HttpClient.GetStringAsync ("languages").ConfigureAwait (false);
            return JsonConvert.DeserializeObject<IEnumerable<Language>> (json);
        }

        public Task<IEnumerable<LanguageVersion>> GetVersionsAsync(LanguageType languageType) {
            return GetVersionsAsync (languageType.ToString ().ToLowerInvariant ());
        }

        public async Task<IEnumerable<LanguageVersion>> GetVersionsAsync(string name) {
            var json = await HttpClient.GetStringAsync ($"languages/{name}").ConfigureAwait (false);
            return JsonConvert.DeserializeObject<IEnumerable<LanguageVersion>> (json);
        }

        //   curl --request POST \
        //--header 'Authorization: Token 0123456-789a-bcde-f012-3456789abcde' \
        //--header 'Content-type: application/json' \
        //--data '{"files": [{"name": "main.py", "content": "print(42)"}]}' \
        //--url 'https://run.glot.io/languages/python/latest'
        // todo: handle command https://github.com/prasmussen/glot-run/blob/master/api_docs/run.md#custom-run-command
        //public async Task<Response> RunAsync(string language, Request request) {
        //    var httpRequest = new HttpRequestMessage (HttpMethod.Post, $"languages/{language}/latest") {
        //        Headers = {
        //            Authorization = new AuthenticationHeaderValue(Token),
        //            Accept = { new MediaTypeWithQualityHeaderValue("aplication/json")} // useful?
        //        },
        //        Content = new StringContent (
        //            JsonConvert.SerializeObject (request), Encoding.UTF8, "application/json") // useful?

        //    };

        //    var httpResponse = await HttpClient.SendAsync (httpRequest).ConfigureAwait (false);
        //    var json = await httpResponse.Content.ReadAsStringAsync ().ConfigureAwait (false);
        //    var response = JsonConvert.DeserializeObject<Response> (json);
        //    return response;
        //}

        public void Dispose() {
            HttpClient.Dispose ();
        }
    }

    //public class Request {
    //    [JsonProperty (PropertyName = "stdin")]
    //    public string StandardInput { get; set; }

    //    public File[] Files { get; set; }
    //}

    //public class File {
    //    public string Name { get; set; }
    //    public string Content { get; set; }
    //}

    //public class Response {
    //    [JsonProperty (PropertyName = "stdout")]
    //    public string StandardOutput { get; set; }

    //    [JsonProperty (PropertyName = "stderr")]
    //    public string StandardError { get; set; }

    //    public string Error { get; set; }
    //}
}
