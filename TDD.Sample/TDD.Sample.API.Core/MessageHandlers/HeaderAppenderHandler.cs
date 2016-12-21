using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace TDD.Sample.API.Core.MessageHandlers
{
    public class HeaderAppenderHandler : DelegatingHandler
    {
        async protected override Task<HttpResponseMessage> SendAsync(
                HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

            response.Headers.Add("X-WebAPI-Header", "Web API Unit testing in chsakell's blog.");
            return response;
        }
    }
}