using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TweetBook.IntegrationTests
{
    public static class Extensions
    {
        public static async Task<Tout> ReadAsAsync<Tout>(this HttpContent content)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Tout>(await content.ReadAsStringAsync());
        }
    }
}
