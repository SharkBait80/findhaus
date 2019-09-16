using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Autofac;
using HouseService.DataLayer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace HouseService
{
    public class GetHandler : AbstractHandler
    {

        public APIGatewayProxyResponse Handler(APIGatewayProxyRequest apigProxyEvent)
        {
            Console.WriteLine($"Processing request data for request {apigProxyEvent.RequestContext.RequestId}.");
            Console.WriteLine($"Body size = {apigProxyEvent.Body.Length}.");
            var headerNames = string.Join(", ", apigProxyEvent.Headers.Keys);
            Console.WriteLine($"Specified headers = {headerNames}.");

            using (var scope = Container.BeginLifetimeScope())
            {
                var dataService = scope.Resolve<IHouseDataService>();

                string searchQuery = null;

                long recordIndex = 0;

                if (apigProxyEvent.QueryStringParameters.ContainsKey("q"))
                    searchQuery = apigProxyEvent.QueryStringParameters["q"];

                if (apigProxyEvent.QueryStringParameters.ContainsKey("idx"))
                {
                    long.TryParse(apigProxyEvent.QueryStringParameters["idx"], out recordIndex);
                }

                var results = dataService.ListHouses(searchQuery, recordIndex, 50).Result;

                // Fire a SNS notification here

                Task.WaitAll(Notifications.SendGetNotification(new { Request = apigProxyEvent }));

                return new APIGatewayProxyResponse
                {
                    Body = Newtonsoft.Json.JsonConvert.SerializeObject(results),
                    StatusCode = 200,
                };
            }



        }
    }
}
