using RestSharp;
using System.Collections.Generic;
using System.Text;

namespace ConsoleBotLinguist.Speller
{
    public static class Speller
    {
        public static string Spell(string text, Localization responseLanguage)
        {
            var lang = responseLanguage.Code;
            var supportLanguages = new List<string> { "ru", "en", "uk" };
            if (!supportLanguages.Contains(lang))
                return responseLanguage.ErrorLang;

            var response = Get(text, lang);
            int countOfMistakes = 1;

            if (response.Errors.Count == 0)
                return responseLanguage.AllRight;
            else
            {
                StringBuilder content = new StringBuilder();
                foreach (var message in response.Errors)
                {
                    StringBuilder sb = new StringBuilder(text);
                    sb.Insert(message.Pos, "[");
                    sb.Insert(message.Pos + message.Len + 1, "]");
                    int startIndex = message.Pos - 20;
                    int lenght = message.Len + 40;
                    string str = sb.ToString().Substring(startIndex > 0 ? startIndex : startIndex = 0,
                                                         lenght + startIndex < text.Length ? lenght : text.Length + 2 - startIndex);
                    sb = new StringBuilder(str);
                    sb.Insert(0, "...");
                    sb.Append("...");
                    sb.Insert(0, countOfMistakes++ + ".) ");
                    content.AppendLine(sb.ToString());

                    content.Append(message.Ss.Count == 0 ? "\n" : (responseLanguage.Variants + ": ["));
                    for (int i = 0; i < message.Ss.Count - 1; i++)
                    {
                        content.Append(message.Ss[i].Value + ", ");
                    }
                    content.Append(message.Ss[message.Ss.Count - 1].Value + "]\n\n");
                }
                return content.ToString();
            }
        }

        private static ListError Get(string text, string lang)
        {
            RestClient client = new RestClient("https://speller.yandex.net/services/spellservice/");
            RestRequest request = new RestRequest("checkText");
            request.AddParameter("text", text);
            request.AddParameter("lang", lang);

            return GetRequest<ListError>.Get(client, request);
        }
    }
}
