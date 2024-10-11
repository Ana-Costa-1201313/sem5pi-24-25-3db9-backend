
namespace Auth.Domain.Users.DTO {

    public class LoginDTO {
        public string? username{ get; set; }

        public string? password { get; set; }

        public string? googleCredentials { get; set; }

        public string? jwt {get;set;}

        public int? a {get;set;}
    }   
}