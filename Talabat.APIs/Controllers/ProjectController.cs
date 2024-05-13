using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;
using Talabat.APIs.DTOs;
using Talabat.APIs.DTOs.Project;
using Talabat.APIs.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Repositories;
using Talabat.Repository;

namespace Talabat.APIs.Controllers
{

    public class ProjectController : APIsBaseController
    {
        private readonly IGenericRepository<Project> _projectGenericRepo;
        private readonly IMapper _mapper;
        private readonly UserManager<Users> _userManager;
        private readonly IProjectRepository _projectRepository;

        public ProjectController(IGenericRepository<Project> projectGenericRepo, IMapper mapper, UserManager<Users> userManager, IProjectRepository projectRepository)
        {
            _projectGenericRepo = projectGenericRepo;
            _mapper = mapper;
            _userManager = userManager;
            _projectRepository = projectRepository;
        }
        #region End Points  Must the User Have Role TeamLeader

        #region GetAllProjects

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProjectToReturnDto>>> GetProjects(string? name,int? projectId)
        {
            dynamic projects;
            if (name is not null)
            {
                projects = await _projectGenericRepo.GetAllAsync(null, null, name,null);
                var ProjectsDT = _mapper.Map<IReadOnlyList<Project>, IReadOnlyList<ProjectToReturnDto>>(projects);

                return Ok(ProjectsDT);

            }

            if(projectId is not null)
            {
                projects = await _projectGenericRepo.GetAllAsync(null, null, null, projectId);
                var ProjectsDT = _mapper.Map<IReadOnlyList<Project>, IReadOnlyList<ProjectToReturnDto>>(projects);

                return Ok(ProjectsDT);
            }

            projects = await _projectGenericRepo.GetAllAsync(null, null, null,null);


            var ProjectsDTO = _mapper.Map<IReadOnlyList<Project>, IReadOnlyList<ProjectToReturnDto>>(projects);

            return Ok(ProjectsDTO);

        }
        #endregion

        #region CreateProject

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "TeamLeader")]
        [HttpPost]
        public async Task<ActionResult<ProjectToCreateDto>> CreateProject(ProjectToCreateDto ProjectDto)
        {
            if (ProjectDto is null)
                return Ok(ProjectDto);


            var Project = _mapper.Map<ProjectToCreateDto, Project>(ProjectDto);

            var Products = await _projectGenericRepo.CreateAsync(Project);

            return Ok(Products);

        }

        #endregion


        #region UpdateProject

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "TeamLeader")]
        [HttpPut]
        public ActionResult<ProjectToUpdateDto> UpdateProject(ProjectToUpdateDto ProjectDto)
        {
            if (ProjectDto is null)
                return Ok("NotUPdated");

            var Project = _projectGenericRepo.GetById(ProjectDto.Id);

            if (Project is null)
                return NotFound(new ApiResponse(404));

            Project.Name = ProjectDto.Name;

            _projectGenericRepo.Update(Project);

            return Ok(ProjectDto);

        }

        #endregion


        #region Delete Project


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "TeamLeader")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<string>> DeleteProject(int id)
        {

            var Developers = (IReadOnlyList<Users>)await _userManager.GetUsersInRoleAsync("Developer");
            if (Developers is not null)
                Developers = Developers.Where(D => D.ProjectId == id).ToList();

            foreach (var dev in Developers)
                dev.ProjectId = null;

            var Project = _projectGenericRepo.GetById(id);

            if (Project is null)
                return NotFound(new ApiResponse(404));

            _projectGenericRepo.Delete(Project);

            return Ok("Deleted");

        }


        #endregion



        #region Add Developer to spacific Project

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "TeamLeader")]
        [HttpPost("AddDeveloperToProject")]
        public async Task<ActionResult<string>> AddDEVToProject(string DeveloperName, string ProjectName)
        {
            if (!string.IsNullOrEmpty(DeveloperName))
            {
                var Developer = await _userManager.FindByNameAsync(DeveloperName);
                if (Developer is null) return "Developer Name is is Not Found";
                if (!string.IsNullOrEmpty(ProjectName))
                {
                    var Project = await _projectGenericRepo.GetAllAsync(null, null, ProjectName,null);
                    if (Project is null) return "Project Name is is Not Found";

                    Developer.ProjectId = Project.FirstOrDefault()?.Id;
                    Developer.ProjectStatus = "Pending";


                    var Result = await _userManager.UpdateAsync(Developer);//then Update Developer(User)
                    if (Result.Succeeded)
                        return "Developer Added Need His Accept or Reject";
                }
            }
            return Ok("DeveloperName|Project Name Is Null ");
        }


        #endregion





        #endregion



        #region Reject Project

        [HttpPut("reject")]
        public async Task<ActionResult> RejectProject(string userName, int projectId)
        {
            var Developers = (IReadOnlyList<Users>) await _userManager.GetUsersInRoleAsync("Developer");
            if (!string.IsNullOrWhiteSpace(userName))
            {
                if (Developers is not null)
                    Developers = Developers.Where(D => D.UserName == userName).ToList();//get project by name
            }
            
            _projectRepository.RejectProject(Developers.FirstOrDefault().Id, projectId);
            return Ok(new { Message = "Project rejected successfully." });
        }

        #endregion

        [HttpPut("accept")]
        public async Task<ActionResult> AcceptProject(string userName, int projectId)
        {
            var Developers = (IReadOnlyList<Users>)await _userManager.GetUsersInRoleAsync("Developer");
            if (!string.IsNullOrWhiteSpace(userName))
            {
                if (Developers is not null)
                    Developers = Developers.Where(D => D.UserName == userName).ToList();//get project by name
            }
            _projectRepository.AcceptProject(Developers.FirstOrDefault().Id, projectId);
            return Ok(new { Message = "Project Accepted successfully." });
        }





    }
}
