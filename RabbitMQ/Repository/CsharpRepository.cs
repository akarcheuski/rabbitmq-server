using RabbitMQ.Client;
using System.Text;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web;

namespace RabbitMQ.Repository
{
    public class CsharpRepository : IRepository, IDisposable
    {
        const string HostName = "localhost";
        const string UserName = "guest";
        const string Password = "guest";
        const string QueueName = "Test.Queue2";
        const string ExchangeName = "";
        const string VirtualHost = "";
        int Port = 0;
        string elapsedTime, consumedTime = string.Empty;
        Stopwatch watch = new Stopwatch();
        ConnectionFactory _connectionFactory;
        IConnection _connection;
        IModel _model;

        public bool Enabled { get; set; }

        public delegate void OnReceiveMessage(string message);

        public void Dispose()
        {
            if (_connection != null)
                _connection.Close();

            if (_model != null && _model.IsOpen)
                _model.Abort();

            _connectionFactory = null;

            GC.SuppressFinalize(this);
        }

        public CsharpRepository()
        {
            _connectionFactory = new ConnectionFactory
            {
                HostName = HostName,
                UserName = UserName,
                Password = Password
            };

            if (string.IsNullOrEmpty(VirtualHost) == false)
                _connectionFactory.VirtualHost = VirtualHost;
            if (Port > 0)
                _connectionFactory.Port = Port;

            _connection = _connectionFactory.CreateConnection();
            _model = _connection.CreateModel();
            _model.BasicQos(0, 1, false);
        }

        public IEnumerable<Models.Client> GetClients() => new List<Models.Client> { new Models.Client () { Id = "1", Name = "C#" }};

        public object Send(string filter, bool isTime)
        {
            var consumer = new QueueingBasicConsumer(_model);
            watch.Start();
            var properties = _model.CreateBasicProperties();
            properties.SetPersistent(true);
            byte[] messageBuffer = Encoding.Default.GetBytes("Test message");

            if (isTime)
            {
                while (watch.Elapsed < TimeSpan.FromSeconds(Convert.ToDouble(filter)))
                {
                    _model.BasicPublish(ExchangeName, QueueName, properties, messageBuffer);

                    if (HttpContext.Current.Session["MessagesQuantity"] != null)
                        HttpContext.Current.Session["MessagesQuantity"] = ((int)HttpContext.Current.Session["MessagesQuantity"]) + 1;
                }
            }
            else
            {
                for (int i = Convert.ToInt32(filter); i > 0; i--)
                {
                    _model.BasicPublish(ExchangeName, QueueName, properties, messageBuffer);
                }
            }
            watch.Stop();

            elapsedTime = TimeFormat(watch);
            if (HttpContext.Current.Session["Time"] != null)
                HttpContext.Current.Session["Time"] = elapsedTime;
            
            return new
            {
                quantity = isTime ? HttpContext.Current.Session["MessagesQuantity"].ToString() : filter,
                time = isTime ? filter : HttpContext.Current.Session["Time"].ToString(),
                consumedTime = isTime ? Consume(HttpContext.Current.Session["MessagesQuantity"].ToString(), consumer) : Consume(filter, consumer)
            };
        }

        private string Consume(string param, QueueingBasicConsumer consumer)
        {
            watch.Start();
            for (int i = Convert.ToInt32(param); i > 0; i--)
            {
                _model.BasicConsume(QueueName, false, consumer);
            }
            watch.Stop();
            return TimeFormat(watch);
        }

        private string TimeFormat(Stopwatch watch) =>
            $"{(watch.Elapsed.Hours != 0 ? watch.Elapsed.Hours + " hours " : "")}{(watch.Elapsed.Minutes != 0 ? watch.Elapsed.Minutes + " minutes" : "")}{(watch.Elapsed.Seconds != 0 ? watch.Elapsed.Seconds + " seconds " : "")}{(watch.Elapsed.Milliseconds != 0 ? watch.Elapsed.Milliseconds + " milliseconds " : watch.ElapsedTicks / (Stopwatch.Frequency / (1000L * 1000L)) + " microseconds ")}";
    }
}