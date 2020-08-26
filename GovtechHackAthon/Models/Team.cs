using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GovtechHackAthon.Models
{
    public class RegistrationTeam
    {
        public int IndexToDelete { get; set; }
        public ApplicantInfo Leader { get; set; }
        private List<TeamMemberData> _Members { get; set; }
        public List<TeamMemberData> Members
        {
            get
            {
                return _Members;
            }

        }
        public int MemberCount 
        {
            get
            {
                return Members.Count();
            }

        }
        
        public bool IDIsUnique(String Identity, int index)
        {
            if (Members.Any(x => x.ID == Identity && x.Index != index))
                return false;
            else
                return true;
        }

        public bool EmailShared(String email, int index)
        {
            if (Members.Any(x => x.Email == email && x.Index != index))
                return true;
            else
                return false;
        }

        public bool CanShareEmail(String email, int index)
        {
            var sharedMembers= Members.Where(x => x.Email == email && x.Index != index).ToList();
            if (sharedMembers.Count == 0)
                return true;

            if (sharedMembers.All(x => x.IsStudent))
                return true;

            return false;
        }

        public void AddTeamMember()
        {
            int memmberCount = _Members.Count;
            var teamMember = new TeamMemberData();
            teamMember.Index = memmberCount+1;
            _Members.Add(teamMember);
        }

        public void AddTeamMembers(int memberCount)
        {
            for (int i = 0; i < memberCount; i++)
                AddTeamMember();
        }

        public void Clear()
        {
            _Members.Clear();
        }

        public void RemoveTeamMember(int index)
        {
            var item = _Members.FirstOrDefault(x => x.Index == index);
            if(item!=null)
            {
                _Members.Remove(item);
                for (int i = 0; i < _Members.Count; i++)
                    _Members[i].Index = i+1;
            }
        }

        public RegistrationTeam()
        {
            _Members = new List<TeamMemberData>();
        }
    }
}
