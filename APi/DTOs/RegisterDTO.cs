using System.ComponentModel.DataAnnotations;

namespace APi.DTOs
{
    public class RegisterDTO
    {
        [Required]
        public string UserName  { get; set; }
        [Required]
        public string password { get; set; }
    }
}