using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Core.Entities
{
    public  class Project:BaseEntity
    {
        public string Name { get; set; }





        #region  Task M-1 Project 

        [InverseProperty("Project")]
        public ICollection<Tasky> Tasks { get; set; } = new HashSet<Tasky>();

        #endregion



        //1-m many developers works on one project
        #region Developer M-1 Project 111

        public ICollection<Users> Developers { get; set; }

        #endregion



    }
}




