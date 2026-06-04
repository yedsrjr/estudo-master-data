using JJMasterData.Commons.Data;
using System.Data;

namespace MasterData.Domain.Repository
{
    public class UserRepository(DataAccess dataAccess) : BaseRepository(dataAccess)
    {
        public DataAccessCommand GetUser(string email)
        {
            var cmd = new DataAccessCommand
            {
                Type = CommandType.Text,
                Sql = @"SELECT 
                            Id,
                            Name,
                            Email,
                            PasswordHash,
                            Status,
                            Role
                         FROM Users
                         WHERE Email = @Email"
            };

            cmd.Parameters.Add(new DataAccessParameter("@Email", email));

            return cmd;
        }

        public DataAccessCommand CreateUser(string email)
        {
            var cmd = new DataAccessCommand
            {
                Type = CommandType.Text,
                Sql = @"SELECT 
                            Id,
                            Name,
                            Email,
                            PasswordHash,
                            Status,
                            Role
                         FROM Users
                         WHERE Email = @Email"
            };

            cmd.Parameters.Add(new DataAccessParameter("@Email", email));

            return cmd;
        }
    }
}
