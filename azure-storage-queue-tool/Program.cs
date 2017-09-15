using System.Collections.Generic;

namespace azure_storage_queue_tool
{
    class Program
    {
        private const string connectionString = "<Replace Connection String here and then delete later>";
        static void Main(string[] args)
        {
            var queueName = "<Insert queue name here>";
            var queueClient = new QueueHelper(connectionString);

            //Simple message example
            queueClient.SendMessageToQueueAsync(queueName, "<Insert you message object here>", true).GetAwaiter().GetResult();

            //Multiple message example
            var sampleMultipleMessages = new List<SampleClass>
            {
                new SampleClass(){MyProperty1 = 1, MyProperty2 = "1"},
                new SampleClass(){MyProperty1 = 2, MyProperty2 = "2"}
            };
            queueClient.SendBatchMessagesToQueueAsync(queueName, sampleMultipleMessages, true).GetAwaiter().GetResult();
        }
    }

    public class SampleClass
    {
        public int MyProperty1 { get; set; }
        public string MyProperty2 { get; set; }
    }
}
