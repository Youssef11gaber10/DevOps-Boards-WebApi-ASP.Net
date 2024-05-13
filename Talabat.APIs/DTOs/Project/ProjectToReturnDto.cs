using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;

namespace Talabat.APIs.DTOs.Project
{
    public class ProjectToReturnDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<DeveloperDto> Developers { get; set; } = new HashSet<DeveloperDto>();
        public ICollection<TaskDto> Tasks { get; set; } = new HashSet<TaskDto>();


    }
}
