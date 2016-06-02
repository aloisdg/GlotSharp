namespace GlotSharp.Run
{
    public class LanguageVersion {
        public string Url { get; set; }
        public string Version { get; set; }

        public override string ToString() {
            return Version;
        }
    }
}