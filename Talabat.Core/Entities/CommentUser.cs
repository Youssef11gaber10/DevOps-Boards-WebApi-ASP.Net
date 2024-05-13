using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Core.Entities
{
    public class CommentUser
    {

        public string Role { get; set; }

        [ForeignKey("Comment")]
        public string CommentId { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        public Comment Comment { get; set; }//=>one
        public Users User { get; set; }//=>one
    }
}
