using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.ViewModels
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public int ParentTeamId { get; set; }
        public string ParentTeamCode { get; set; }
        public int ManageUserId { get; set; }
        public List<int> MemberIds { get; set; }
    }
}
