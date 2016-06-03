using Newtonsoft.Json;

namespace GlotSharp.Run
{
    public class Response {
        [JsonProperty (PropertyName = "stdout")]
        public string StandardOutput { get; set; }

        [JsonProperty (PropertyName = "stderr")]
        public string StandardError { get; set; }

        public string Error { get; set; }
    }
}