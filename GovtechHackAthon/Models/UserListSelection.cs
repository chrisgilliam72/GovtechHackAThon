using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GovtechHackAthon.Models
{
    public class UserListSelection
    {
        public int GroupID { get; set; }
        public String DisplayTitle { get; set; }
        public List<UserSelectionItem> ListUsers { get; set; }

        public List<UserSelectionItem> SelectedUsers
        {
            get
            {
                return ListUsers.Where(x => x.Selected).ToList();
            }
        }
        public int ItemCount
        {
            get
            {
                return ListUsers.Count;
            }
        }
        public UserListSelection()
        {
            ListUsers = new List<UserSelectionItem>();
        }
    }

}
