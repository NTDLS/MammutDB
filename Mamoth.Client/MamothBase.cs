using Mamoth.Common.Payload.Request;
using Mamoth.Common.Payload.Response;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Mamoth.Client
{
    public class MamothBase
    {
        private MamothClientBase _mamothClient;

        public MamothBase(MamothClientBase client)
        {
            this._mamothClient = client;
        }

        public r Submit<s, r>(string url, s action) where s : IActionRequest where r : ActionResponseBase
        {
            var postContent = new StringContent(JsonConvert.SerializeObject(action), Encoding.UTF8, "application/json");

            using (var response = _mamothClient.Client.PostAsync(url, postContent))
            {
                var resultText = response.Result.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<r>(resultText);
                if (result.Success == false)
                {
                    throw new Exception(result.Message);
                }

                return result;
            }
        }

        public async Task<r> SubmitAsync<s, r>(string url, s action) where s : IActionRequest where r : ActionResponseBase
        {
            var postContent = new StringContent(JsonConvert.SerializeObject(action), Encoding.UTF8, "application/json");

            using (var response = await _mamothClient.Client.PostAsync(url, postContent))
            {
                var resultText = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<r>(resultText);
                if (result.Success == false)
                {
                    throw new Exception(result.Message);
                }

                return result;
            }
        }
    }
}
