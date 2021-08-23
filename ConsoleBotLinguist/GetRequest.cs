using RestSharp;
using RestSharp.Serialization.Xml;
using System;

namespace ConsoleBotLinguist
{
    public static class GetRequest<T>
        where T : new()
    {
        public static T Get(RestClient client, RestRequest request)
        {
            try
            {
                RestResponse response = (RestResponse)client.Execute(request);
                XmlAttributeDeserializer deserializer = new XmlAttributeDeserializer();
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var completeResponse = deserializer.Deserialize<T>(response);
                    return completeResponse;
                }
                else
                {
                    if (response.StatusCode != 0)
                    {
                        var error = deserializer.Deserialize<YandexError>(response);
                        throw new YandexException(error);
                    }
                    else
                    {
                        throw new YandexException(0, response.ErrorMessage);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new T();
            }
        }
    }
}
