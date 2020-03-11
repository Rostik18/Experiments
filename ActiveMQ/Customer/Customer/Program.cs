using Apache.NMS;
using System;
using System.Threading;

namespace Customer
{
    class Program
    {
        static bool ReadNextMessageTopic()
        {
            string topic = "TestTopic";

            string brokerUri = $"activemq:tcp://localhost:61616";  // Default port
            NMSConnectionFactory factory = new NMSConnectionFactory(brokerUri);

            using (IConnection connection = factory.CreateConnection())
            {
                connection.Start();
                using (ISession session = connection.CreateSession(AcknowledgementMode.AutoAcknowledge))
                using (IDestination dest = session.GetTopic(topic))
                using (IMessageConsumer consumer = session.CreateConsumer(dest))
                {
                    IMessage msg = consumer.Receive();
                    if (msg is ITextMessage)
                    {
                        ITextMessage txtMsg = msg as ITextMessage;
                        string body = txtMsg.Text;

                        Console.WriteLine($"Received message: {txtMsg.Text}");

                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Unexpected message type: " + msg.GetType().Name);
                    }
                }
            }

            return false;
        }

        static bool ReadNextMessageQueue()
        {
            string queueName = "TestQueue";

            string brokerUri = $"activemq:tcp://localhost:61616";  // Default port
            NMSConnectionFactory factory = new NMSConnectionFactory(brokerUri);

            using (IConnection connection = factory.CreateConnection())
            {
                connection.Start();
                using (ISession session = connection.CreateSession(AcknowledgementMode.AutoAcknowledge))
                using (IDestination dest = session.GetQueue(queueName))
                using (IMessageConsumer consumer = session.CreateConsumer(dest))
                {
                    IMessage msg = consumer.Receive();
                    if (msg is ITextMessage)
                    {
                        ITextMessage txtMsg = msg as ITextMessage;
                        string body = txtMsg.Text;

                        Thread.Sleep(2000);

                        Console.WriteLine($"Received message: {txtMsg.Text}");

                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Unexpected message type: " + msg.GetType().Name);
                    }
                }
            }

            return false;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Waiting for messages\n");

            // Read all messages off the queue
            //while (ReadNextMessageTopic())
            //{
            //    Console.WriteLine("Successfully read message");
            //}

            while (ReadNextMessageQueue())
            {
                Console.WriteLine("Successfully read message");
            }

            Console.WriteLine("\nFinished");
        }
    }
}
