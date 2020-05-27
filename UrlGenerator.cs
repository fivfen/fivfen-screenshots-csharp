using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Linq;

namespace FivFen
{
    public class UrlGenerator
    {
        private String key;
        private String secret;

        public UrlGenerator(string key, string secret){
            this.key = key;
            this.secret = secret;
        }

        private string ToQueryString(IDictionary<string, object> options)
        {
            var result = options
                .ToList()
                .Select(pair => new KeyValuePair<string, string>(pair.Key, ConvertToString(pair.Value))) // convert values to string
                .Where(pair => !String.IsNullOrEmpty(pair.Value)) // skip empty/null values
                .Select(pair => string.Format("{0}={1}", FormatKeyName(pair.Key), Uri.EscapeDataString(pair.Value)))
                .ToArray();
            return String.Join("&", result);
        }

        private static string FormatKeyName(string input)
        {
            return string.Concat(input.Select((x, i) => i > 0 && char.IsUpper(x) && !input[i-1].Equals('_') ? "_" + x.ToString() : x.ToString())).ToLower();
        }

        private static string ConvertToString(object value)
        {
            var result = Convert.ToString(value);
            if (result.Equals("False") || result.Equals("True"))
            {
                result = result.ToLower();
            }
            return result;
        }

        public string GenerateUrl(IDictionary<string, object> options)
        {
            var qs = ToQueryString(options);
            return string.Format("https://fivfen.com/api?v=v1&key={0}&sec={1}{2}",this.key,generateToken(qs),qs);
        }

        private string generateToken(string queryString)
        {
            HMACSHA1 sha = new HMACSHA1(Encoding.UTF8.GetBytes(this.secret));
            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(queryString));
            return sha.ComputeHash(stream).Aggregate("", (current, next) => current + String.Format("{0:x2}", next), current => current);
        }
    }
}
