using System.Collections.Generic;

namespace RabbitMQ.Repository
{
    public interface IRepository
    {
        object Send(string filter, bool isTime);
        IEnumerable<Models.Client> GetClients();
    }
}
