using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MovieTinder.Model
{
    class RequestHelper
    {
        RestClient client;
        public RequestHelper(string baseURL)
        {
            client = new RestClient(baseURL);
        }

        public async Task<IRestResponse> Get(string endPointPath)
        {
            var request = new RestRequest(endPointPath, DataFormat.Json);
            var cancellationTokenSource = new CancellationTokenSource();
            var response =
                await client.ExecuteAsync(request, cancellationTokenSource.Token);
            return response;
        }

    }
}
