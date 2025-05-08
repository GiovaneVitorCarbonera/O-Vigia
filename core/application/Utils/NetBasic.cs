using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace O_Vigia_Docker.core.application.Utils
{
    internal class NetBasic
    {
        public static async Task<Stream> GetStreamFromUrl(string url)
        {
            var httpClient = new HttpClient();
            return await httpClient.GetStreamAsync(url);
        }
    }
}
