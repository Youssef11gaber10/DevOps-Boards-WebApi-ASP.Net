using AutoMapper;
using Microsoft.Extensions.Configuration;
using Talabat.APIs.DTOs;
using Talabat.Core.Entities;

namespace Talabat.APIs.Helpers
{
    public class TaskFileUrl : IValueResolver<Tasky, TaskWithAttach, string>
    {
        private readonly IConfiguration _configuration;
        public TaskFileUrl(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(Tasky source, TaskWithAttach destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.UploadedAttachment))
                return $"{_configuration["ApiBaseUrl"]}{source.UploadedAttachment}";
            return string.Empty;
        }
    }
}
