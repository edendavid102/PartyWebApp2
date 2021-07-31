using System.Net.Http;

namespace PartyWebApp2.Services
{
    public class FacebookService : IFacebookService
    {
        private readonly string FacebookPageId = "100168142240956";
        private readonly string FacebookPageToken = "EAA3O6dvVFWYBAFfjgzGIQdJZBzSdz07K2LqAWoZAZA3h8ydDRBu7tYRtZBRYINUHkegSYQEgfuxfdhDsO3vuJhEDj7FZCHcJlB9ZBKvcIYXeKPolq1QDjbdNyUefZCdRpT5ZCaSUq9yQDZBGP2dZBaQ5oqVMfOvPqT4QkzzSAq31ZBquQZDZD";
        private readonly string FacebookApi = "https://graph.facebook.com/";
        private readonly HttpClient client;

        public bool PostToFacebook(string postMessage)
        {
            string postReqUrl = FacebookApi + FacebookPageId + "/feed?message=" + postMessage + "&access_token=" + FacebookPageToken;
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, postReqUrl);
            HttpResponseMessage response = client.SendAsync(requestMessage).GetAwaiter().GetResult();
            return response.IsSuccessStatusCode;
        }
    }
}