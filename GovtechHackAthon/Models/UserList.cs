using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GovtechHackAthon.Models
{
    public class UserList
    {
        public bool? UpdateSuccessful { get; set; }
        public int  UserCount
        {
            get
            {
                return Users.Count();
            }
        }
        public List<UserDetails> Users { get; set; }

        public UserList()
        {
            UpdateSuccessful = null;
            Users = new List<UserDetails>();
        }
    }
}
