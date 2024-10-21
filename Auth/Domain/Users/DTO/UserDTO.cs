
namespace Auth.Domain.Users{

    public class UserDTO
    {
        public string? jwt {get; set;}

        public string? username{ get; set; }

        public string? telemovel { get; set; }

        public string? email { get; set; }

        public string? role { get; set; }

        public string? nomeCompleto{ get; set; }
    }
}