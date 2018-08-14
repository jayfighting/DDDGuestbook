using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.API.Models
{
    // Note: doesn't expose events or behavior
    public class UserDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
