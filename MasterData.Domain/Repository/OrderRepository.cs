using JJMasterData.Commons.Data;
using MasterData.Domain.Models.Enums;
using MasterData.Domain.Repository;
using System.Data;

namespace Domain.Repository
{
    public class OrderRepository(DataAccess dataAccess) : BaseRepository(dataAccess)
    {
        public Task UpdateTotalOrder(int pedidoId, CancellationToken cancellationToken = default)
        {
            var cmd = new DataAccessCommand
            {
                Type = CommandType.Text,
                Sql = @"UPDATE p
                        SET p.Total = ISNULL(x.TotalItens, 0)
                        FROM dbo.Pedidos p
                        OUTER APPLY
                        (
                            SELECT SUM(ISNULL(i.ValorTotal, 0)) AS TotalItens
                            FROM dbo.PedidoItem i
                            WHERE i.PedidoId = p.Id
                        ) x
                        WHERE p.Id = @PedidoId"
            };

            cmd.Parameters.Add(new DataAccessParameter("@PedidoId", pedidoId, DbType.Int32));

            return dataAccess.SetCommandAsync(cmd, cancellationToken);
        }
        public DataAccessCommand CountOrderByStatus(int? status)
        {
            var cmd = new DataAccessCommand
            {
                Type = CommandType.Text,
                Sql = @"SELECT COUNT(*)
                        FROM Pedidos
                        WHERE (@status IS NULL OR [Status] = @status)
                        "
            };

            cmd.Parameters.Add(new DataAccessParameter("@status", status));

            return cmd;
        }
        public DataAccessCommand GetAllOrders(int page, int pageSize, int? status)
        {
            var offset = (page - 1) * pageSize;

            var cmd = new DataAccessCommand
            {
                Type = CommandType.Text,
                Sql = @"SELECT 
	                    [Id],
	                    [DataCriacao],
	                    [CodClient],
	                    [Total],
	                    [Status],
	                    [Anexo],
	                    [ObservacaoNF]
                    FROM 
	                    [Pedidos]
                    WHERE (@status IS NULL OR [Status] = @status)
                    ORDER BY [Id] ASC
                    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY"
            };

            cmd.Parameters.Add(new DataAccessParameter("@status", status));
            cmd.Parameters.Add(new DataAccessParameter("@Offset", offset));
            cmd.Parameters.Add(new DataAccessParameter("@PageSize", pageSize));

            return cmd;
        }

        public DataAccessCommand GetOrderById(int id)
        {
            var cmd = new DataAccessCommand
            {
                Type = CommandType.Text,
                Sql = @"SELECT 
	                        Id,
	                        DataCriacao,
	                        CodClient,
	                        Total,
	                        [Status],
	                        Anexo,
	                        ObservacaoNF
                        FROM 
	                        Pedidos
                        WHERE Id = @id"
            };

            cmd.Parameters.Add(new DataAccessParameter("@id", id));

            return cmd;
        }

        public DataAccessCommand GetOrderItems(int id)
        {
            var cmd = new DataAccessCommand
            {
                Type = CommandType.Text,
                Sql = @"SELECT 
	                        Id,
	                        ItemId,
                            PedidoId,
	                        Quantidade,
	                        PesoBruto,
	                        PesoLiquido,
	                        ValorUnit,
	                        ValorTotal
                        FROM 
	                        PedidoItem
                        WHERE
                            PedidoId = @id"          
            };

            cmd.Parameters.Add(new DataAccessParameter("@id", id));

            return cmd;
        }
    }
}
