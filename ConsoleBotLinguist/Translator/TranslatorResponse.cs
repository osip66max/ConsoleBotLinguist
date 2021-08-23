using RestSharp.Deserializers;

namespace ConsoleBotLinguist.Translator
{
    public class TranslatorResponse
    {
        public int Code { get; set; }

        [DeserializeAs(Name = "lang")]
        public string LangPairString { get; set; }

        [DeserializeAs(Attribute = false)]
        public string Text { get; set; }
    }
}
