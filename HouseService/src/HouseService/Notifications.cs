using Amazon.SimpleNotificationService.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HouseService
{
    public class Notifications
    {
        static string SnsTopicArn = null;

        static Notifications()
        {
            string SSMParameterName = @"/FindHaus/HouseService/SnsTopic";

            try
            {
                Amazon.SimpleSystemsManagement.AmazonSimpleSystemsManagementClient client = new Amazon.SimpleSystemsManagement.AmazonSimpleSystemsManagementClient();

                var result = client.GetParameterAsync(new Amazon.SimpleSystemsManagement.Model.GetParameterRequest { Name = SSMParameterName }).Result;

                SnsTopicArn = result.Parameter.Value;
            }
            catch
            {

            }

        }

        public async static Task SendGetNotification(object data)
        {
            if (string.IsNullOrEmpty(SnsTopicArn))
                return;

            Amazon.SimpleNotificationService.AmazonSimpleNotificationServiceClient client = new Amazon.SimpleNotificationService.AmazonSimpleNotificationServiceClient();
            string strMessage = "Message";

            if (data != null)
                strMessage = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            await client.PublishAsync(new PublishRequest { TopicArn = SnsTopicArn, Message = strMessage });
        }
    }
}
