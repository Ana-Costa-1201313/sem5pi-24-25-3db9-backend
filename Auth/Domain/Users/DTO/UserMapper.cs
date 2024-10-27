

namespace Auth.Domain.Users
{
    public class UserMapper
    {
        public static UserDTO toDTO(User user)
        {
            return new UserDTO
            {
                username = user.username.username
                // telemovel = user.telemovel.telemovel,
                // email = user.email.email,
                // permissoes = user.PermissaoString,
                // nomeCompleto = user.nomeCompleto.nomeCompleto
            };
        }
        public static UserDTO toDTO(User utilizador, String jwt)
        {
            return new UserDTO
            {
                username = utilizador.username.username,
                //telemovel = utilizador.telemovel.telemovel,
                //email = utilizador.email.email,
                //permissoes = utilizador.PermissaoString,
                //nomeCompleto = utilizador.nomeCompleto.nomeCompleto,
                jwt = jwt
            };
        }

        internal static List<UserDTO> toDTO(List<User> users)
        {
            var list = new List<UserDTO>();

            foreach(User user in users){
                list.Add(toDTO(user));
            }
            return list;
        }
    }
}