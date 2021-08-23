using RestSharp;

namespace ConsoleBotLinguist.Translator
{
    public static class Translator
    {
        public static string Translate(string text, Localization lang)
        {
            var response = Get(text, lang.Code);
            if (response.Text == null)
                return "";
            return response.Text;
        }

        public static TranslatorResponse Get(string text, string lang)
        {
            string key = "Yandex.Translator key";
            RestClient client = new RestClient("https://translate.yandex.net/api/v1.5/tr");
            RestRequest request = new RestRequest("translate");
            request.AddParameter("key", key);
            request.AddParameter("text", text);
            request.AddParameter("lang", lang);

            return GetRequest<TranslatorResponse>.Get(client, request);
        }
    }
}
