using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateManagement.Model
{
    public class Message
    {
        public string? FromEmail { get; set; }
        public string? ToEmail { get; set; }
        public string? MessageText { get; set; }
        public string? Name { get; set; }
        public int Id { get; set; }
        public long Contact { get; set; }
    }
}
