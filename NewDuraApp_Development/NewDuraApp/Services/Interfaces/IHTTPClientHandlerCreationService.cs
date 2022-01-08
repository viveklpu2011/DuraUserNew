using System;
using System.Net.Http;

namespace NewDuraApp.Services.Interfaces
{
    public interface IHTTPClientHandlerCreationService
    {
        HttpClientHandler GetInsecureHandler();
    }
}
