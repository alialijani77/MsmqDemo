using Experimental.System.Messaging;

namespace MsmqSender
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
                Console.WriteLine($"Queue created at: {queuePath}");
            }

            using (var queue = new MessageQueue(queuePath))
            {
                Console.WriteLine("=== MSMQ Sender Started ===");

                while (true)
                {
                    Console.Write("\nEnter payment description (or 'exit' to quit): ");
                    string desc = Console.ReadLine();

                    if (string.Equals(desc, "exit", StringComparison.OrdinalIgnoreCase))
                        break;

                    var payload = new PaymentMessage
                    {
                        Id = Guid.NewGuid(),
                        Amount = new Random().Next(10000, 999999),
                        Description = desc,
                        CreatedAt = DateTime.Now
                    };

                    var msmqMessage = new Message
                    {
                        Label = $"Payment-{payload.Id.ToString().Substring(0, 8)}",
                        Body = payload,
                        Recoverable = true
                    };

                    queue.Send(msmqMessage);

                    Console.WriteLine($"[Sent] ID: {payload.Id} | Amount: {payload.Amount:N0}");
                }
            }
        }
    }
}