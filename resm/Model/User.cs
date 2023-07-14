using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateManagement.Model
{
    public class User
    {
        public string Email;
        public string FirstName;
        public string LastName;
        public string Role;
        public User(string Email, string FirstName, string LastName, string Role)
        {
            this.Email = Email;
            this.FirstName=FirstName;
            this.LastName=LastName;
            this.Role=Role;
        }
    }
}
