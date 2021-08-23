using RestSharp;

namespace ConsoleBotLinguist.Predictor
{
    public static class Predictor
    {
        public static string Predict(string text, Localization lang)
        {
            var response = Get(lang.Code, text);
            if (response.Text.Count == 0)
                return "";
            return response.Text[0].ToString();
        }

        public static PredictorResponse Get(string lang, string q, int limit = 1)
        {
            string key = "Yandex.Predictor key";
            RestClient client = new RestClient("https://predictor.yandex.net/api/v1/predict");
            RestRequest request = new RestRequest("complete");
            request.AddParameter("key", key);
            request.AddParameter("lang", lang);
            request.AddParameter("q", q);
            request.AddParameter("limit", limit);

            return GetRequest<PredictorResponse>.Get(client, request);
        }
    }
}
