using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using System.Text;


namespace FivFen
{
    public class Fivfen
    {
        private String key;
        private String secret;
        private UrlGenerator urlGenerator;

        public Fivfen(string key, string secret)
        {
            if (String.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Please provide your FivFen API Key");
            }
            if (String.IsNullOrEmpty(secret))
            {
                throw new ArgumentException("Please provide your FivFen API Secret");
            }
            this.key = key;
            this.secret = secret;
            this.urlGenerator = new UrlGenerator(key, secret);
        }

        public async Task<string> DownloadAsBase64(IDictionary<string, object> options){
            var fivfenUrl = this.GenerateUrl(options);
            return await DownloadAsBase64(fivfenUrl);
        }

        public async Task<string> DownloadAsBase64(string fivfenUrl)
        {
            Func<HttpResponseMessage, Task<string>> onSuccess = async (result) =>
            {
                var bytes = await result.Content.ReadAsByteArrayAsync();
                var contentType = result.Content.Headers.ToDictionary(l => l.Key, k => k.Value)["Content-Type"];
                var base64 = contentType.First() + ";base64," + Convert.ToBase64String(bytes);
                return base64;
            };
            return await this.Download(fivfenUrl, onSuccess);
        }

        public async Task<string> DownloadToFile(IDictionary<string, object> options, string filename){
            var fivfenUrl = GenerateUrl(options);
            return await DownloadToFile(fivfenUrl, filename);
        }

        public async Task<string> DownloadToFile(string fivfenUrl, string filename)
        {
            Func<HttpResponseMessage, Task<string>> onSuccess = async (result) =>
            {
                using (
                        Stream contentStream = await result.Content.ReadAsStreamAsync(),
                        stream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    await contentStream.CopyToAsync(stream);
                }
                return await result.Content.ReadAsStringAsync();
            };
            return await Download(fivfenUrl, onSuccess);
        }

        private async Task<string> Download(string fivfenUrl, Func<HttpResponseMessage, Task<string>> onSuccess)
        {            
            using (var client = new HttpClient())
            {
                using (var result = await client.GetAsync(fivfenUrl).ConfigureAwait(false))
                {
                    if (result.IsSuccessStatusCode)
                    {
                        Debug.WriteLine(result, "SUCCESS!");
                        return await onSuccess(result);
                    }
                    else
                    {
                        Debug.WriteLine(result, "FAIL");
                        return "FAIL";
                    }
                }
            }
        }

        public string GenerateUrl(IDictionary<string, object> options)
        {
            return urlGenerator.GenerateUrl(options);
        }
    }
}