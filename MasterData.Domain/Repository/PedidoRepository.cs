using JJMasterData.Commons.Data;
using System.Data;

namespace Domain.Repository
{
    public class PedidoRepository(DataAccess dataAccess)
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
    }
}
