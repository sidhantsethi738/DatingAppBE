using DatingAppBE.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingAppBE.Entities
{
    public class AppUser
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PassowrdSalt { get; set; }
        public DateTime DateofBirth { get; set; }
        public string KnownAs { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime LastActive { get; set; } = DateTime.Now;
        public string Gender { get; set; }
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
      

        public ICollection<Photo> Photos { get; set; }

        public int GetAge()
        {

            return DateofBirth.CalculateAge();
        }
    }
}
