using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
    public class TaskRepository : GenericRepository<Tasky>, ITaskRepository
    {
        private readonly StoreContext _dbContext;
        private readonly IHostingEnvironment _environment;
        public TaskRepository(StoreContext dbContext, IHostingEnvironment environment) : base(dbContext)
        {
            _dbContext = dbContext;
            _environment = environment;
        }

        #region Update task status

        public bool UpdateStatus(Tasky tasky)
        {
            var existingTask = _dbContext.Tasks.FirstOrDefault(t => t.Id == tasky.Id);
            existingTask.Status = tasky.Status;
            _dbContext.SaveChanges();
            return true; // Status updated successfully
        }



        #endregion



        #region Add Comment
        public bool AddComment(Comment comment)
        {

            _dbContext.Add(comment);
            _dbContext.SaveChanges();
            return true; // ** Comment added successfully
        }

        #endregion

        #region upload file

        public async Task<string> UploadFileAsync(IFormFile file, int taskId)
        {
            var task = _dbContext.Tasks.FirstOrDefault(t => t.Id == taskId);
            var directoryPath = Path.Combine(_environment.ContentRootPath, "wwwroot\\UploadedFiles");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            var filePath = Path.Combine(directoryPath, file.FileName);
       

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }


            task.UploadedAttachment = "UploadedFiles/" + file.FileName;//was file path
            _dbContext.SaveChanges();
            return filePath;

        }


        #endregion
   






    }
}