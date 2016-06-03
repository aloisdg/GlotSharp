using Newtonsoft.Json;

namespace GlotSharp.Run
{
    public class Request {
        [JsonProperty ("stdin", NullValueHandling = NullValueHandling.Ignore)]
        public string StandardInput { get; set; }

        public File[] Files { get; set; }
    }
}