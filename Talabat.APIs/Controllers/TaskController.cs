using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Talabat.APIs.DTOs;
using Talabat.APIs.DTOs.Project;
using Talabat.APIs.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Repositories;
using Talabat.Repository;
using Talabat.Repository.Data;

namespace Talabat.APIs.Controllers
{

    public class TaskController : APIsBaseController
    {
        private readonly ITaskRepository _taskRepository;
        private readonly StoreContext _storeContext;
        private readonly UserManager<Users> _userManager;
        private readonly IMapper _mapper;

        public TaskController(ITaskRepository taskRepository,
            StoreContext storeContext,
            UserManager<Users> userManager,
            IMapper mapper)
        {
            _taskRepository = taskRepository;
            _storeContext = storeContext;
            _userManager = userManager;
            _mapper = mapper;
        }


        #region End Points  Must the User Have Role TeamLeader

        #region GetAll Tasks & Filter them by Developer Name

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<TaskDto>>> GetTasks(string? SpecDEV)
        {

            var Tasks = await _taskRepository.GetAllAsync(SpecDEV, null, null, null);

            var TasksDTO = _mapper.Map<IReadOnlyList<Tasky>, IReadOnlyList<TaskDto>>(Tasks);
            return Ok(TasksDTO);

        }
        #endregion


        #region Filter Task By Id
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "TeamLeader")]
        [HttpGet("{id}")]
        public ActionResult<TaskDto> GetTask(int id)
        {

            var Task = _taskRepository.GetById(id);

            if (Task is null)
                return NotFound(new ApiResponse(404));


            return Ok(Task);

        }
        #endregion


        #region CreateTask

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "TeamLeader")]
        [HttpPost]
        public async Task<ActionResult<ProjectToCreateDto>> CreateTask(TaskToCreateDto TaskDto)
        {
            if (TaskDto is null)
                return Ok(TaskDto);


            var Task = _mapper.Map<TaskToCreateDto, Tasky>(TaskDto);

            var Tasks = await _taskRepository.CreateAsync(Task);

            return Ok(Tasks);

        }

        #endregion


        #region updateTask

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "TeamLeader")]
        [HttpPut]
        public async Task<ActionResult<TaskToUpdateDto>> UpdateTask(TaskToUpdateDto TaskDto)
        {
            if (TaskDto is null)
                return Ok("NotUPdated");

            var Developers = (IReadOnlyList<Users>)await _userManager.GetUsersInRoleAsync("Developer");
            if (Developers is not null)
                Developers = Developers.Where(D => D.UserName == TaskDto.DeveloperName).ToList();

            var Task = _taskRepository.GetById(TaskDto.Id);

            if (Task is null)
                return NotFound(new ApiResponse(404));

            Task.Name = TaskDto.Name;
            Task.Description = TaskDto.Description;
            Task.ProjectId = TaskDto.ProjectId;
            Task.DeveloperId = Developers.FirstOrDefault().Id;


            _taskRepository.Update(Task);

            return Ok(TaskDto);

        }

        #endregion


        #region Delete Task


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "TeamLeader")]
        [HttpDelete("{id}")]
        public ActionResult<string> DeleteTask(int id)
        {

            var Task = _taskRepository.GetById(id);

            if (Task is null)
                return NotFound(new ApiResponse(404));



            _taskRepository.Delete(Task);

            return Ok("Deleted");

        }


        #endregion


        #region Add Comment To Spacific Task


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "TeamLeader")]
        [HttpPost("TeamLeaderComment")]
        public async Task<ActionResult<CreateComment>> CommentByTeamLeader(CreateComment CommentDto)
        {
            if (CommentDto is null)
                return Ok(CommentDto);


            var Comment = _mapper.Map<CreateComment, Comment>(CommentDto);

            _storeContext.AddAsync(Comment);
            _storeContext.SaveChanges();


            return Ok(CommentDto);

        }


        #endregion


        #region TeamLeader  View Uploded Attachments


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "TeamLeader")]
        [HttpGet("UploadedAttachments")]
        //public async Task<ActionResult<IReadOnlyList<TaskWithAttach>>> ViewUplodedAttachments()
        public async Task<ActionResult<List<string>>> ViewUplodedAttachments()
        {


            var Tasks = await _taskRepository.GetAllAsync(null, "Done", null, null);

            var Taskss = _mapper.Map<IReadOnlyList<Tasky>, IReadOnlyList<TaskWithAttach>>(Tasks);

            var attachments = new Dictionary<int,string>();
            foreach(var Task in Taskss)
            {
                attachments.Add( Task.Id, Task.UploadedAttachment);
            }

            return Ok(attachments);

        }


        #endregion




        #endregion


        #region Update Task Status

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Developer")]
        [HttpPut("updateStatus")]
        public IActionResult UpdateTaskStatus(UpdateTaskStatus updateTaskStatus)
        {
            if (!updateTaskStatus.Status.IsNullOrEmpty())
            {

            var task = _mapper.Map<Tasky>(updateTaskStatus);
            task.Id = updateTaskStatus.TaskId;
            var result = _taskRepository.UpdateStatus(task);

            if (result)
            {
                return Ok(new { Message = "Task status updated successfully" });
            }
            else
            {
                return BadRequest(new { Message = "Failed to Update Task Status" });
            }

            }
            else
            {
                return BadRequest(new { Message = "Failed to Update Task Status" });
            }

        }

        #endregion





        #region Add Comment


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Developer")]
        [HttpPost("DeveloperComment")]

        public IActionResult AddCommentToTask(CreateComment createComment)
        {
            var comment = _mapper.Map<Comment>(createComment);
            var result = _taskRepository.AddComment(comment);

            if (result)
            {
                return Ok(new { Message = "Comment added successfully" });
            }
            else
            {
                return BadRequest(new { Message = "Task not found or comment not added" });
            }
        }
        #endregion


        #region Upload File

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Developer")]
        [HttpPost("UploadFile")]
        public async Task<IActionResult> UploadFile(IFormFile file, int taskId)
        {

            var filePath = await _taskRepository.UploadFileAsync(file, taskId);
            return Ok(new { Message = "File uploaded successfully", FilePath = filePath });

        }

        #endregion



    }
}
