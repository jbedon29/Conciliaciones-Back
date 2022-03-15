using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Protecta.CrossCuting.Utilities.service
{
    public class ConsumeService<T, U> where T : class
    {

        public static T Get(string baseUrl, string url)
        {
            T result = null;
            try
            {
                UriBuilder builder = new UriBuilder(baseUrl + url);
                using (var httpClient = new HttpClient())
                {
                    //LogHelper.Exception(string.Format("Get - {0} : ", 0), LogHelper.Paso.CreateDirectory, "Get toy aki");

                    httpClient.DefaultRequestHeaders.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = httpClient.GetAsync(builder.Uri).Result;

                    result = JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public static T GetWithParameters(string baseUrl, string url, Dictionary<string, string> parameters)
        {
            T result = null;

            try
            {
                UriBuilder builder = new UriBuilder(baseUrl + url);
                string query = "";

                foreach (var item in parameters)
                {
                    query += string.Format("{0}={1}", item.Key, item.Value);
                }
                builder.Query = query;

                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Clear();

                    var response = httpClient.GetAsync(builder.Uri).Result;

                    result = JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public static T PostRequest(string baseUrl, string url, U postObject, string token = null)
        {
            T result = null;
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                UriBuilder builder = new UriBuilder(baseUrl + url);
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();
                    if (token != null)
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    }
                    string json = JsonConvert.SerializeObject(postObject);
                    //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                    var response = client.PostAsync(builder.Uri, stringContent).Result;

                    result = JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public static T PostRequestStringJson(string baseUrl, string url, string jsonFormat, string token = null)
        {
            T result = null;
            try
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)768 | (SecurityProtocolType)3072;
                UriBuilder builder = new UriBuilder(baseUrl + url);
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();
                    if (token != null)
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    }
                    string json = jsonFormat;
                    //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                    var response = client.PostAsync(builder.Uri, stringContent).Result;

                    result = JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }
    }
}
