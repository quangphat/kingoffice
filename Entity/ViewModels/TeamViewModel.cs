using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.ViewModels
{
    public class TeamViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string ManageUser { get; set; }
        public string ParentTeamCode { get; set; }
    }
}
