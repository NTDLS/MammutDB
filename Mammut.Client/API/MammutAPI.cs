using Mammut.Common.Payload.Request;
using Mammut.Common.Payload.Response;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Mammut.Client.API
{
    public class MammutAPI
    {
        private MammutClient _MammutClient;

        public MammutAPI(MammutClient client)
        {
            this._MammutClient = client;
        }

        public r Submit<s, r>(string url, s action) where s : IActionRequest where r : ActionResponseBase
        {
            var postContent = new StringContent(JsonConvert.SerializeObject(action), Encoding.UTF8, "application/json");

            using (var response = _MammutClient.Client.PostAsync(url, postContent))
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

            using (var response = await _MammutClient.Client.PostAsync(url, postContent))
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
