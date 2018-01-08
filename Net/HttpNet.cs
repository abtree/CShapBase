using System;
using System.Net.Http;

namespace HttpNet
{
    class HttpNet
    {
        private static async void GetData()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json;odata=verbose"); //改变标题
            
            HttpResponseMessage response = null;
            response = await httpClient.GetAsync("http://cn.bing.com/");
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Response Status Code: " + response.StatusCode + " " + response.ReasonPhrase);
                string responseBodyAsText = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine("Received payload of " + responseBodyAsText.Length + " Characters");
                Console.WriteLine(responseBodyAsText);
            }
        }
        static void Main(string[] args)
        {
            Console.WriteLine("In main before call to GetData");
            GetData();
            Console.WriteLine("Back in main after call to GetData");
            Console.ReadKey();
        }
    }
}
