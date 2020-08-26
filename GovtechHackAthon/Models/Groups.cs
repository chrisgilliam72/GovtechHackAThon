using GovtechDBLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GovtechHackAthon.Models
{
    public class Groups
    {
        public List<GroupItem> GroupsList { get; set; }

        public Groups()
        {
            GroupsList = new List<GroupItem>();
        }
    }

    public class GroupItem
    {
        public int GroupID { get; set; }
        public String Name { get; set; }
        public bool IsActive { get; set; }
        public String UserNamesList
        {
            get
            {
                var usrNamesLst = _listUsers.Select(x => x.DisplayName).ToList();
                return  String.Join(',', usrNamesLst);
            }
        }

        private List<UserSelectionItem> _listUsers;
        public List<UserSelectionItem> UserSelectionList
        {
            get
            {
                return _listUsers;
            }
        }

        public GroupItem()
        {
            IsActive = true;
            _listUsers = new List<UserSelectionItem>();
        }
        public static explicit operator GroupItem(Group dbGroup)
        {
           
            var grpItem = new GroupItem()
            {
               GroupID = dbGroup.PkId,
               Name = dbGroup.Name,
               IsActive = dbGroup.Active,
              _listUsers = dbGroup.UsersInGroup.Select(x => new UserSelectionItem { UserID = x.FkUserId, DisplayName = x.FkUser.FirstName + " " + x.FkUser.Surname, Selected = false }).ToList()
            };
            return grpItem;
        }
    }


}
