using JJMasterData.Commons.Data;
using MasterData.API.Models.DTOs.Customer;
using MasterData.Domain.Models.DTOs.Customer;
using System.Data;

namespace MasterData.Domain.Repository
{
    public class CustomerRepository(DataAccess dataAccess) : BaseRepository(dataAccess)
    {
        public DataAccessCommand GetCommandCustomerCount()
        {
            return new DataAccessCommand
            {
                Type = CommandType.Text,
                Sql = @"SELECT COUNT(1) FROM [Clientes]"
            };
        }
        public DataAccessCommand GetCommandCustomer(int page, int pageSize)
        {
            var offset = (page - 1) * pageSize;

            var cmd = new DataAccessCommand
            {
                Type = CommandType.Text,
                Sql = @"
            SELECT
                [Id],
                [NomeAbreviado],
                [NomeCliente],
                [NumCPF],
                [Status],
                [UpdatedAt]
            FROM [Clientes]
            ORDER BY [UpdatedAt] ASC
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY"
            };

            cmd.Parameters.Add(new DataAccessParameter("@Offset", offset));
            cmd.Parameters.Add(new DataAccessParameter("@PageSize", pageSize));

            return cmd;
        }
        public DataAccessCommand GetCustomerById(int id)
        {
            var cmd = new DataAccessCommand
            {
                Type = CommandType.Text,
                Sql = @"
                SELECT
                    [Id],
                    [NomeAbreviado],
                    [NomeCliente],
                    [NumCPF],
                    [Status],
                    [UpdatedAt]
                FROM [Clientes]
                WHERE Id = @id"
            };

            cmd.Parameters.Add(new DataAccessParameter("@id", id));

            return cmd;
        }
        public DataAccessCommand InsertCustomer(CustomerRequest request)
        {
            var cmd = new DataAccessCommand
            {
                Type = CommandType.Text,
                Sql = @"INSERT INTO [Clientes]
                        ([NomeAbreviado], [NomeCliente], [NumCPF], [Status], [UpdatedAt])
                        OUTPUT INSERTED.Id
                        VALUES (@NomeAbreviado, @NomeCliente, 
                        @NumCPF, @Status, ISNULL(@UpdatedAt, GETDATE()));
                        SELECT SCOPE_IDENTITY();"
            };

            cmd.Parameters.Add(new DataAccessParameter("@NomeAbreviado", request.ShortName));
            cmd.Parameters.Add(new DataAccessParameter("@NomeCliente", request.Name));
            cmd.Parameters.Add(new DataAccessParameter("@NumCPF", request.CpfCnpj));
            cmd.Parameters.Add(new DataAccessParameter("@Status", request.Status));
            cmd.Parameters.Add(new DataAccessParameter("@UpdatedAt", request.UpdatedAt));

            return cmd;
        }

        public DataAccessCommand UpdateCustomer(int id, CustomerRequest request)
        {
            var cmd = new DataAccessCommand
            {
                Type = CommandType.Text,
                Sql = @"UPDATE [Clientes]
                        SET 
                           [NomeAbreviado] = @NomeAbreviado,
                           [NomeCliente] = @NomeCliente,
                           [NumCPF] = @NumCPF,
                           [Status] = @Status,
                           [UpdatedAt] = @UpdatedAt
                         WHERE Id = @id"
            };

            cmd.Parameters.Add(new DataAccessParameter("@id", id));
            cmd.Parameters.Add(new DataAccessParameter("@NomeAbreviado", request.ShortName));
            cmd.Parameters.Add(new DataAccessParameter("@NomeCliente", request.Name));
            cmd.Parameters.Add(new DataAccessParameter("@NumCPF", request.CpfCnpj));
            cmd.Parameters.Add(new DataAccessParameter("@Status", request.Status));
            cmd.Parameters.Add(new DataAccessParameter("@UpdatedAt", request.UpdatedAt));

            return cmd;
        }
        public DataAccessCommand CancelCustomer(int id)
        {
            var cmd = new DataAccessCommand
            {
                Type = CommandType.Text,
                Sql = @"UPDATE [Clientes]
                        SET [STATUS] = 0
                        WHERE Id = @id"
            };

            cmd.Parameters.Add(new DataAccessParameter("@id", id));

            return cmd;
        }
    }
}
