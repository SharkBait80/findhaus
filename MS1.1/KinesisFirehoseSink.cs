using System.Collections.Generic;
using System.IO;
using System.Text;
using Amazon.KinesisFirehose;
using Amazon.KinesisFirehose.Model;

namespace Amazon.Kinesis.Firehose.Sinks
{
    /// <summary>
    /// Writes log events as documents to a Amazon KinesisFirehose.
    /// </summary>
    public class KinesisFirehoseSink
    {
        readonly KinesisSinkState _state;
        readonly LogEventLevel? _minimumAcceptedLevel;

        /// <summary>
        /// Free resources held by the sink.
        /// </summary>
        /// <param name="disposing">If true, called because the object is being disposed; if false,
        /// the object is being disposed from the finalizer.</param>
        protected override void Dispose(bool disposing)
        {
            // First flush the buffer
            base.Dispose(disposing);

            if (disposing)
            {
                _state.KinesisFirehoseClient.Dispose();
            }
        }

        /// <summary>
        /// Emit a batch of log events, running to completion asynchronously.
        /// </summary>
        /// <param name="events">The events to be logged to Kinesis Firehose</param>
        protected override void EmitBatch(IEnumerable<LogEvent> events)
        {
            var request = new PutRecordBatchRequest
            {
                DeliveryStreamName = _state.Options.StreamName
            };

            foreach (var logEvent in events)
            {
                var json = new StringWriter();
                _state.Formatter.Format(logEvent, json);

                var bytes = Encoding.UTF8.GetBytes(json.ToString());

                var entry = new Record
                {
                    Data = new MemoryStream(bytes),
                };

                request.Records.Add(entry);
            }

            _state.KinesisFirehoseClient.PutRecordBatch(request);
        }

        public PutRecordBatchResponse PutRecordBatch(string deliveryStreamName, List<Record> records)
        {
            var request = new PutRecordBatchRequest();
            request.DeliveryStreamName = deliveryStreamName;
            request.Records = records;
            return PutRecordBatch(request);
        }        

    }
}