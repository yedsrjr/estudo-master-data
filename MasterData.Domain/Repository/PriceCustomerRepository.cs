using JJMasterData.Commons.Data;
using MasterData.Domain.Models.DTOs.PriceCustomer;
using System.Data;

namespace MasterData.Domain.Repository
{
    public class PriceCustomerRepository(DataAccess dataAccess) : BaseRepository(dataAccess)
    {
        public DataAccessCommand CountPrices()
        {
            return new DataAccessCommand
            {
                Type = CommandType.Text,
                Sql = @"SELECT COUNT(1)
                        FROM (
                            SELECT 
                                CodClient, CodProduto,
                                ROW_NUMBER() OVER (PARTITION BY CodClient, CodProduto 
                                ORDER BY DataInclusao DESC) AS rn
                            FROM TabelaPrecos) x
                        WHERE rn = 1"
            };
        }
        public DataAccessCommand GetPrices(int page, int pageSize)
        {
            var offset = (page - 1) * pageSize;

            var cmd = new DataAccessCommand
            {
                Type = CommandType.Text,
                Sql = @"
                    SELECT 
                        CodClient, 
                        CodProduto, 
                        ValorUnit, 
                        DataInclusao
                    FROM (SELECT 
                        CodClient, 
                        CodProduto, 
                        ValorUnit, 
                        DataInclusao,
                        ROW_NUMBER() OVER (PARTITION BY CodClient, CodProduto ORDER BY DataInclusao DESC) AS rn
                        FROM TabelaPrecos) x
                    WHERE rn = 1
                    ORDER BY CodClient ASC
                    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY"
            };

            cmd.Parameters.Add(new DataAccessParameter("@Offset", offset));
            cmd.Parameters.Add(new DataAccessParameter("@PageSize", pageSize));

            return cmd;
        }
        public DataAccessCommand GetPriceById(int id)
        {
            var cmd = new DataAccessCommand
            {
                Type = CommandType.Text,
                Sql = @"
                    SELECT 
                        CodClient, 
                        CodProduto, 
                        ValorUnit, 
                        DataInclusao
                    FROM (SELECT 
                        CodClient, 
                        CodProduto, 
                        ValorUnit, 
                        DataInclusao,
                        ROW_NUMBER() OVER (PARTITION BY CodClient, CodProduto ORDER BY DataInclusao DESC) AS rn
                        FROM TabelaPrecos
                        WHERE CodClient = @id) x
                    WHERE rn = 1"
            };

            cmd.Parameters.Add(new DataAccessParameter("@id", id));

            return cmd;
        }

        public DataAccessCommand AddPrice(PriceItemRequest request)
        {
            var cmd = new DataAccessCommand
            {
                Type = CommandType.Text,
                Sql = @"
                    INSERT INTO TabelaPrecos (CodClient, CodProduto, ValorUnit, DataInclusao)
                    VALUES (@CodClient, @CodProduto, @ValorUnit, @DataInclusao)"
            };

            cmd.Parameters.Add(new DataAccessParameter("@CodClient", request.IdClient));
            cmd.Parameters.Add(new DataAccessParameter("@CodProduto", request.IdProduct));
            cmd.Parameters.Add(new DataAccessParameter("@ValorUnit", request.UnitValue));
            cmd.Parameters.Add(new DataAccessParameter("@DataInclusao", request.InsertionDate));

            return cmd;
        }
    }
}
