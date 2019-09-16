using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Autofac;
using HouseService.DataLayer;
using System;
using System.Collections.Generic;
using System.Text;

namespace HouseService
{
    public class PutHandler : AbstractHandler
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

                var house = Newtonsoft.Json.JsonConvert.DeserializeObject<HouseDTO>(apigProxyEvent.Body);
                if (string.IsNullOrEmpty(house.Id))
                    house.Id = Guid.NewGuid().ToString();

                dataService.Create(house);

                return new APIGatewayProxyResponse
                {
                    Body = Newtonsoft.Json.JsonConvert.SerializeObject(house),
                    StatusCode = 200,
                };
            }



        }
    }
}
