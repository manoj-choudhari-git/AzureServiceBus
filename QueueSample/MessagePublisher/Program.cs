using Microsoft.Azure.ServiceBus;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace MessagePublisher
{
    class Program
    {
        static IQueueClient queueClient;

        static void Main(string[] args)
        {
            string serviceBusConnectionString = "Endpoint=sb://mydemo.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=<< your-SAS-key>>";
            string queueName = "demoqueue";

            StartPublishing(serviceBusConnectionString, queueName).GetAwaiter().GetResult();
        }

        static async Task StartPublishing(string serviceBusConnectionString, string queueName)
        {
            queueClient = new QueueClient(serviceBusConnectionString, queueName);

            Console.WriteLine("======================================================");
            Console.WriteLine("PUBLISHER sending messages, Press ENTER to exit.");
            Console.WriteLine("======================================================");

            // Send Messages
            await SendMessagesAsync();

            Console.ReadLine();

            await queueClient.CloseAsync();
        }

        static async Task SendMessagesAsync()
        {
            DateTime start = DateTime.Now;

            try
            {
                for (var i = 0; DateTime.Now.Subtract(start).Minutes < 5; i++)
                {
                    // Create a new message to send to the queue
                    string messageBody = $"Message {i}";
                    var message = new Message(Encoding.UTF8.GetBytes(messageBody));

                    // Write the body of the message to the console
                    Console.WriteLine($"Sending message: {messageBody}");

                    // Send the message to the queue
                    await queueClient.SendAsync(message);
                    Thread.Sleep(1000);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"{DateTime.Now} :: Exception: {exception.Message}");
            }
        }
    }
}




