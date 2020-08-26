using System;
using System.Collections.Generic;

namespace GovtechDBLib.Models
{
    public partial class VwUserGroups
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public int? GroupId { get; set; }
    }
}
