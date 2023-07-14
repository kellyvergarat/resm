using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateManagement.Model
{
    public class Property 
    {
        public int Id { get; set; }
        public string? ImageUrl { get; set; }
        public double PricePerMonth { get; set; }
        public int Bathrooms { get; set; }
        public int Rooms { get; set; }
        public string? Owner { get; set; }
        public string? Pourpose { get; set; }
        public bool Status { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Province { get; set; }
        public string? Realtor { get; set; }
        public List<Property>? Properties { get; set; }

    }
}
