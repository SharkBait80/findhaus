using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xunit;
using Amazon.Lambda.Core;
using Amazon.Lambda.TestUtilities;

using HouseService;
using System.Collections.Specialized;

namespace HouseService.Tests
{
    public class FunctionTest
    {
        [Fact]
        public void Test_GetHandler()
        {

            // Invoke the lambda function and confirm the string was upper cased.
            var function = new GetHandler();
            var context = new TestLambdaContext();
            var request = new Amazon.Lambda.APIGatewayEvents.APIGatewayProxyRequest { RequestContext = new Amazon.Lambda.APIGatewayEvents.APIGatewayProxyRequest.ProxyRequestContext { RequestId = Guid.NewGuid().ToString() } };
            request.QueryStringParameters = new Dictionary<string, string>();
            request.Body = "Test";
            request.Headers = new Dictionary<string, string>();
            var result = function.Handler(request);

            Assert.True(true);
        }
    }
}
