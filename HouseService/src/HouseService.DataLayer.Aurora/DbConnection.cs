using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace HouseService.DataLayer.Aurora
{
    public class DbConnection
    {
        static string SSMParameterName = "/FindHaus/HouseService/ConnString";
        public static NpgsqlConnection CreateConnection()
        {
            return new NpgsqlConnection(GetConnectionString());

        }

        public static string GetConnectionString()
        {
            Amazon.SimpleSystemsManagement.AmazonSimpleSystemsManagementClient client = new Amazon.SimpleSystemsManagement.AmazonSimpleSystemsManagementClient();
            try
            {
                var result = client.GetParameterAsync(new Amazon.SimpleSystemsManagement.Model.GetParameterRequest { Name = SSMParameterName }).Result;

                return result.Parameter.Value;
            }catch
            {
                return "Server=findmyhaus-db-instance-1.cs95hzohy0fj.ap-southeast-1.rds.amazonaws.com;Port=5432;Database=findhaus;User Id=postgres;Password = Password1; ";
            }

        }
    }
}
