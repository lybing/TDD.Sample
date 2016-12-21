using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace TDD.Sample.API.Core.HeaderAppenderHandler
{
    public class EndRequestHandler : DelegatingHandler
    {
        async protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.RequestUri.AbsoluteUri.Contains("test"))
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("Unit testing message handlers!")
                };

                var tsc = new TaskCompletionSource<HttpResponseMessage>();
                tsc.SetResult(response);
                return await tsc.Task;
            }
            else
            {
                return await base.SendAsync(request, cancellationToken);
            }
        }
    }
}