using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Core.Entities
{
    public class Tasky:BaseEntity
    {

        public string Name { get; set; }
        public string Description { get; set; }

        public string? Status { get; set; }//must be 

        public string? UploadedAttachment { get; set; }





        #region Task  1-M Comment
        [InverseProperty("Task")]
        public ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();//=>Navigationl prop (many)

        #endregion



        #region Task M-1 Project 

        [InverseProperty("Tasks")]
        [ForeignKey("Project")]
        public int ProjectId { get; set; }
        public Project Project { get; set; }

        #endregion




        #region Task M-1 Developer 222

        //[InverseProperty("DeveloperTasks")]
        //[ForeignKey("Developer")]
        public string? DeveloperId { get; set; }
        public Users Developer { get; set; }

        #endregion

    }
}
