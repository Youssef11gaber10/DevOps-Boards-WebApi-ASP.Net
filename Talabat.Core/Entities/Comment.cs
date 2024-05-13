using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Core.Entities
{
    public class Comment
    {
        public string Id { get; set; }= Guid.NewGuid().ToString();
        public string Text { get; set; }

        public string Role { get; set; }//updated in last migrations
        public string Name { get; set; } //updated in last migrations
       


        #region  Task  1-M Comment

        [InverseProperty("Comments")]
        [ForeignKey("Task")]
        public int TaskId { get; set; }
        public Tasky Task { get; set; }//=>navigational prop (one) 
        #endregion


        // M-M many comments comes from many diffrent users(teamleader, developer)

        public ICollection<CommentUser> CommentUser { get; set; } = new HashSet<CommentUser>();//=>many

    }
}
