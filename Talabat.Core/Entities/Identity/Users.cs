using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Identity
{
    public class Users : IdentityUser
    {

       
        public string DisplayName { get; set; }



        public string? RoleName { get; set; }
        public string? ProjectStatus { get; set; }


        #region Developer M-1 Project 111
        //[InverseProperty("Developers")]
        //[ForeignKey("Project")]
        public int? ProjectId { get; set; }
        public Project Project { get; set; }
        #endregion


        #region Task M-1 Developer 222
        //[InverseProperty("Developer")]
        public ICollection<Tasky> DeveloperTasks { get; set; }
        #endregion




        public ICollection<CommentUser> CommentUser { get; set; } =new HashSet<CommentUser>();//=>many
        

    }
}
