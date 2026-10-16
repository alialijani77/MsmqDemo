using Experimental.System.Messaging;

namespace MsmqReceiver
{
    public class PaymentMessage
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string queuePath = @".\private$\MyCompanyQueue";

            if (!MessageQueue.Exists(queuePath))
            {
                MessageQueue.Create(queuePath);
            }

            using (var queue = new MessageQueue(queuePath))
            {
                queue.Formatter = new XmlMessageFormatter(new Type[] { typeof(PaymentMessage) });

                Console.WriteLine("=== MSMQ Receiver Waiting for Messages... ===");

                while (true)
                {
                    try
                    { 
                        Message msmqMessage = queue.Receive();

                        var data = (PaymentMessage)msmqMessage.Body;

                        Console.WriteLine($"\n[Received] Label: {msmqMessage.Label}");
                        Console.WriteLine($" -> ID: {data.Id}");
                        Console.WriteLine($" -> Amount: {data.Amount:N0}");
                        Console.WriteLine($" -> Description: {data.Description}");
                        Console.WriteLine($" -> Time: {data.CreatedAt:HH:mm:ss}");
                    }
                    catch (MessageQueueException ex)
                    {
                        Console.WriteLine($"MSMQ Error: {ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"General Error: {ex.Message}");
                    }
                }
            }
        }
    }
}