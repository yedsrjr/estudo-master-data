using JJMasterData.Commons.Data;
using MasterData.API.Models.DTOs.Customer;
using MasterData.Domain.Models.DTOs.Product;
using System.Data;

namespace MasterData.Domain.Repository
{
    public class ProductRepository(DataAccess dataAccess) : BaseRepository(dataAccess)
    {
        public DataAccessCommand Count()
        {
            return new DataAccessCommand
            {
                Type = CommandType.Text,
                Sql = @"SELECT COUNT(1) FROM [Produtos]"
            };
        }
        public DataAccessCommand GetProducts(int page, int pageSize)
        {
            var offset = (page - 1) * pageSize;

            var cmd = new DataAccessCommand
            {
                Type = CommandType.Text,
                Sql = @"
                    SELECT
                        [CodSku],
                        [Descricao],
                        [PesoBruto],
                        [PesoLiquido],
                        [Quantidade],
                        [UpdatedAt],
                        [Status]
                    FROM [Produtos]
                    ORDER BY [UpdatedAt] ASC
                    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY"
            };

            cmd.Parameters.Add(new DataAccessParameter("@Offset", offset));
            cmd.Parameters.Add(new DataAccessParameter("@PageSize", pageSize));

            return cmd;
        }
        public DataAccessCommand GetProductById(int id)
        {
            var cmd = new DataAccessCommand
            {
                Type = CommandType.Text,
                Sql = @"
                    SELECT
                        [CodSku],
                        [Descricao],
                        [PesoBruto],
                        [PesoLiquido],
                        [Quantidade],
                        [UpdatedAt],
                        [Status]
                    FROM [Produtos]
                    WHERE [CodSku] = @id"
            };

            cmd.Parameters.Add(new DataAccessParameter("@id", id));

            return cmd;
        }
        public DataAccessCommand AddProduct(ProductRequest model)
        {
            var cmd = new DataAccessCommand
            {
                Type = CommandType.Text,
                Sql = @"INSERT INTO [Produtos]
                        ([Descricao], [PesoBruto], [PesoLiquido], [Quantidade], [Status], [UpdatedAt])
                        VALUES (@Descricao, @PesoBruto, 
                        @PesoLiquido, @Status, @Quantidade, @UpdatedAt);
                        SELECT SCOPE_IDENTITY();"
            };

            cmd.Parameters.Add(new DataAccessParameter("@Descricao", model.Description));
            cmd.Parameters.Add(new DataAccessParameter("@PesoBruto", model.GrossWeight));
            cmd.Parameters.Add(new DataAccessParameter("@PesoLiquido", model.NetWeight));
            cmd.Parameters.Add(new DataAccessParameter("@Quantidade", model.Quantity));
            cmd.Parameters.Add(new DataAccessParameter("@Status", model.Status));
            cmd.Parameters.Add(new DataAccessParameter("@UpdatedAt", model.UpdatedAt));

            return cmd;
        }

        public DataAccessCommand UpdateProduct(int id, ProductRequest model)
        {
            var cmd = new DataAccessCommand
            {
                Type = CommandType.Text,
                Sql = @"UPDATE [Produtos]
                        SET 
                           [Descricao] = @Descricao,
                           [PesoBruto] = @PesoBruto,
                           [PesoLiquido] = @PesoLiquido,
                           [Quantidade] = @Quantidade,
                           [Status] = @Status,
                           [UpdatedAt] = @UpdatedAt
                         WHERE CodSku = @id"
            };

            cmd.Parameters.Add(new DataAccessParameter("@id", id));
            cmd.Parameters.Add(new DataAccessParameter("@Descricao", model.Description));
            cmd.Parameters.Add(new DataAccessParameter("@PesoBruto", model.GrossWeight));
            cmd.Parameters.Add(new DataAccessParameter("@PesoLiquido", model.NetWeight));
            cmd.Parameters.Add(new DataAccessParameter("@Quantidade", model.Quantity));
            cmd.Parameters.Add(new DataAccessParameter("@Status", model.Status));
            cmd.Parameters.Add(new DataAccessParameter("@UpdatedAt", model.UpdatedAt));

            return cmd;
        }
        public DataAccessCommand CancelProduct(int id)
        {
            var cmd = new DataAccessCommand
            {
                Type = CommandType.Text,
                Sql = @"UPDATE [Produtos]
                        SET [STATUS] = 0
                        WHERE CodSku = @id"
            };

            cmd.Parameters.Add(new DataAccessParameter("@id", id));

            return cmd;
        }

    }
}
