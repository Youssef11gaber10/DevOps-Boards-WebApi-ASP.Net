using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Repositories
{
    public interface ITaskRepository : IGenericRepository<Tasky>
    {
        bool UpdateStatus(Tasky tasky);
        bool AddComment(Comment comment);
        Task<string> UploadFileAsync(IFormFile file, int taskId);

    }
}
