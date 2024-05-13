using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class ProjectRepostiory :GenericRepository<Project> ,IProjectRepository
    {
        private readonly StoreContext _dbContext;

        public ProjectRepostiory(StoreContext dbContext) :base(dbContext)
        {
            _dbContext = dbContext;
           
        }


        #region reject project
        public void RejectProject(string userId, int projectId)
        {
            var user = _dbContext.Users.Find(userId);
            if (user.ProjectId == projectId)
            {
                user.ProjectId = null;
                user.ProjectStatus = null;
                _dbContext.SaveChanges();
            }
        }

        #endregion
        #region Accept

        public void AcceptProject(string userId, int projectId)
        {
            var user = _dbContext.Users.Find(userId);
            if (user.ProjectId == projectId)
            {
                user.ProjectStatus = "Accept";
                _dbContext.SaveChanges();
            }
        } 
        #endregion

    }
}
