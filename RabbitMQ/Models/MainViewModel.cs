using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RabbitMQ.Models
{
    public class MainViewModel
    {
        [Required]
        public string ClientId { get; set; }

        public IEnumerable<Client> Clients { get; set; }
    }
}