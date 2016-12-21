using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TDD.Sample.API.Core.HeaderAppenderHandler;
using TDD.Sample.API.Core.MessageHandlers;

namespace TDD.Sample.Tests
{
    [TestFixture]
    public class MessageHandlerTest
    {
        #region Variables
        private EndRequestHandler _endRequestHandler;
        private HeaderAppenderHandler _headerAppenderHandler;
        #endregion

        #region Setup
        [SetUp]
        public void Setup()
        {
            // Direct MessageHandler test
            _endRequestHandler = new EndRequestHandler();
            _headerAppenderHandler = new HeaderAppenderHandler()
            {
                InnerHandler = _endRequestHandler
            };
        }
        #endregion

        #region Test Method

        [Test]
        public async void ShouldAppendCustomHeader()
        {
            var invoker = new HttpMessageInvoker(_headerAppenderHandler);
            var result = await invoker.SendAsync(new HttpRequestMessage(HttpMethod.Get, 
                new Uri("http://localhost/api/test/")), CancellationToken.None);

            Assert.That(result.Headers.Contains("X-WebAPI-Header"), Is.True);
            Assert.That(result.Content.ReadAsStringAsync().Result,
                Is.EqualTo("Unit testing message handlers!"));
        }

        #endregion
    }
}
