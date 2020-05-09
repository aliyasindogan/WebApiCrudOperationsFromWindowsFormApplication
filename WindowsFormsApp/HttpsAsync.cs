using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace WindowsFormsApp
{
    public class HttpsAsync<T> : IHttpsAsync<T>
    {
        public async void Post(T entity, string baseUrl)
        {
            Uri baseUri = new Uri(baseUrl);
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Host = baseUri.Host;
            DataContractJsonSerializer jsonSer = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream();
            jsonSer.WriteObject(ms, entity);
            ms.Position = 0;
            StreamReader sr = new StreamReader(ms);
            StringContent theContent = new StringContent(sr.ReadToEnd(), System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage aResponse = await httpClient.PostAsync(baseUri, theContent);
            string result = aResponse.Content.ReadAsStringAsync().Result;
        }

        public async Task<string> PostAdd(T entity, string baseUrl)
        {
            Uri baseUri = new Uri(baseUrl);
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Host = baseUri.Host;
            DataContractJsonSerializer jsonSer = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream();
            jsonSer.WriteObject(ms, entity);
            ms.Position = 0;
            StreamReader sr = new StreamReader(ms);
            StringContent theContent = new StringContent(sr.ReadToEnd(), System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage aResponse = await httpClient.PostAsync(baseUri, theContent);
            string result = await aResponse.Content.ReadAsStringAsync();
            return result;
        }

        public async Task<T> PostAddModelReturn(T entity, string baseUrl)
        {
            Uri baseUri = new Uri(baseUrl);
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Host = baseUri.Host;
            DataContractJsonSerializer jsonSer = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream();
            jsonSer.WriteObject(ms, entity);
            ms.Position = 0;
            StreamReader sr = new StreamReader(ms);
            StringContent theContent = new StringContent(sr.ReadToEnd(), System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage aResponse = await httpClient.PostAsync(baseUri, theContent);
            string result = aResponse.Content.ReadAsStringAsync().Result;
            var model = JsonConvert.DeserializeObject<T>(result);
            return model;
        }

        public async Task<List<T>> GetAll(string baseUrl)
        {
            HttpClient httpClient = new HttpClient();
            //httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //httpClient.DefaultRequestHeaders.Host = baseUri.Host;
            //DataContractJsonSerializer jsonSer = new DataContractJsonSerializer(typeof(T));
            //MemoryStream ms = new MemoryStream();
            //jsonSer.WriteObject(ms, entity);
            //ms.Position = 0;
            //StreamReader sr = new StreamReader(ms);
            //StringContent theContent = new StringContent(sr.ReadToEnd(), System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage aResponse = await httpClient.GetAsync(new Uri(baseUrl));
            string result = aResponse.Content.ReadAsStringAsync().Result;
            var model = JsonConvert.DeserializeObject<List<T>>(result);
            return model;
        }
    }
}