using System;
using System.IO;
using System.Text;
using System.Web;

using Amazon;
using Amazon.KinesisFirehose;
using Amazon.KinesisFirehose.Model;

namespace KinesisFirehoseDemo
{
    /// 
    /// This http module adds an event handler for incoming requests.
	/// For each request a record is sent to Kinesis Firehose. For this demo a
    /// single record is sent at time with the PutRecord operation to
	/// keep the demo simple. This can be optimized by batching records and
	/// using the PutRecordBatch operation.
    /// 
    public class FirehoseSiteTracker : IHttpModule
    {
        IAmazonKinesisFirehose _client;

        // The delivery stream that was created using the KFSetup.ps1 script.
        string _deliveryStreamName = "";

        public FirehoseSiteTracker()
        {
            this._client = new AmazonKinesisFirehoseClient(RegionEndpoint.USWest2);
        }

        public void Dispose() 
        {
            this._client.Dispose(); 
        }

        public bool IsReusable
        {
            get { return true; }
        }

        /// 
        /// Setup the event handler for BeginRequest events.
        /// 
        /// 
        public void Init(HttpApplication application)
        {
            application.BeginRequest +=
                (new EventHandler(this.RecordRequest));
        }

        /// 
        /// Write to Firehose a record with the starting page and the page being requested.
        /// 
        /// 
        /// 
        private void RecordRequest(Object source, EventArgs e)
        {
            // Create HttpApplication and HttpContext objects to access
            // request and response properties.
            HttpApplication application = (HttpApplication)source;
            HttpContext context = application.Context;

            string startingRequest = string.Empty;
            if (context.Request.UrlReferrer != null)
                startingRequest = context.Request.UrlReferrer.PathAndQuery;

            var record = new MemoryStream(UTF8Encoding.UTF8.GetBytes(string.Format("{0}t{1}n",
                startingRequest, context.Request.Path)));

            var request = new PutRecordRequest
            {
                DeliveryStreamName = this._deliveryStreamName,
                Record = new Record
                {
                    Data = record
                }
            };
            this._client.PutRecordAsync(request);
        }
    }
}