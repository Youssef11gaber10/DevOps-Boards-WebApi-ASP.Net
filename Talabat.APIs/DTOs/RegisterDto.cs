using System.ComponentModel.DataAnnotations;

namespace Talabat.APIs.DTOs
{
    public class RegisterDto
    {
        [Required]
        public string DisplayName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        [Required]
        [RegularExpression("(?=^.{6,10}$)(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[a-z])(?=.*[!@#$%^&amp;*()_+]).*$", 
            ErrorMessage ="Password must contains 1 UpperCase ,1 LowerCase,1Digit ,1 Spaecial Character")]
        public string Password { get; set; }

        [RegularExpression("^(TeamLeader|Developer)$",ErrorMessage = "Role Name Must Be (TeamLeader Or Developer)")]
        [Required]
        public string RoleName { get; set; }


        //public int ProjectId { get; set; }



    }
}
