namespace GlotSharp.Run
{
    public class Language {
        public string Url { get; set; }
        public string Name { get; set; }

        public override string ToString() {
            return Name;
        }
    }
}