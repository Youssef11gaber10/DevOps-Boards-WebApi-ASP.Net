using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Repositories
{
    public interface IProjectRepository : IGenericRepository<Project>
    {
        void RejectProject(string userId, int projectId);
        void AcceptProject(string userId, int projectId);

    }
}
