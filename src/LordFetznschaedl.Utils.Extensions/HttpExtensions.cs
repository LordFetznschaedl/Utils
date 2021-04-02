using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace LordFetznschaedl.Utils.Extensions
{
    public static class HttpExtensions
    {
        public static async Task<HttpResponseMessage> PostJsonAsync(this string url, object data = null)
        {
            try
            {
                var uri = new UriBuilder(url);
                using (var client = new HttpClient() { BaseAddress = uri.Uri })
                {
                    client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    if (data != null)
                    {
                        var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                        return await client.PostAsync(uri.Uri, content);
                    }

                    else
                    {
                        return await client.PostAsync(uri.Uri, null);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("HttpClient throws Exception at PostJsonAsync ...", ex);
            }
        }

        public static async Task<T> PostJsonAsync<T>(this string url, object data = null)
        {
            try
            {
                var uri = new UriBuilder(url);
                using (var client = new HttpClient() { BaseAddress = uri.Uri })
                {
                    client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    Task<HttpResponseMessage> response;

                    if (data != null)
                    {
                        var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                        response = client.PostAsync(uri.Uri, content);
                    }

                    else
                    {
                        response = client.PostAsync(uri.Uri, null);
                    }


                    return await response.ReceiveJson<T>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("HttpClient throws Exception at PostJsonAsync ...", ex);
            }
        }

        public static async Task<HttpResponseMessage> PutJsonAsync(this string url, object data)
        {
            try
            {
                var uri = new UriBuilder(url);
                using (var client = new HttpClient() { BaseAddress = uri.Uri })
                {
                    client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                    return await client.PutAsync(uri.Uri, content);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("HttpClient throws Exception at PostJsonAsync ...", ex);
            }
        }


        public static async Task<T> GetJsonAsync<T>(this string url)
        {
            try
            {
                var uri = new UriBuilder(url);
                using (var client = new HttpClient() { BaseAddress = uri.Uri })
                {
                    client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = client.GetAsync(uri.Uri);
                    return await response.ReceiveJson<T>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("HttpClient throws Exception at GetJsonAsync ...", ex);
            }
        }


        public static async Task<HttpResponseMessage> DeleteAsync(this string url)
        {
            try
            {
                var uri = new UriBuilder(url);
                using (var client = new HttpClient() { BaseAddress = uri.Uri })
                {
                    return await client.DeleteAsync(uri.Uri);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("HttpClient throws Exception at PostJsonAsync ...", ex);
            }
        }


        public static async Task<T> ReceiveJson<T>(this Task<HttpResponseMessage> response)
        {
            T obj = default(T);
            try
            {
                var result = await response;
                if (result.IsSuccessStatusCode)
                {
                    var returnVal = await result.Content.ReadAsStringAsync();
                    obj = JsonConvert.DeserializeObject<T>(returnVal);
                }
            }
            catch (Exception ex)
            {
                obj = default(T);
                throw new Exception("HttpClient throws Exception at ReceiveJson ...", ex);
            }
            return obj;
        }
    }
}
