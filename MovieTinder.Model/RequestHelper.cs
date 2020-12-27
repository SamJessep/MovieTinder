using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieTinder.Model
{
    class RequestHelper
    {
        RestClient client;
        public RequestHelper(string baseURL)
        {
            client = new RestClient(baseURL);
        }

        public IRestResponse Get(string endPointPath)
        {
            var request = new RestRequest(endPointPath, DataFormat.Json);
            var response = client.Get(request);
            return response;
        }

    }
}
