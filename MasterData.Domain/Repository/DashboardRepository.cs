using Domain.Models.ViewModels;
using JJMasterData.Commons.Data;
using System.Data;

namespace Domain.Repository
{
    public class DashboardRepository(DataAccess dataAccess)
    {
        public int GetCountRecord(string tableName)
        {
            var cmd = new DataAccessCommand
            {
                Type = CommandType.StoredProcedure,
                Sql = "dbo.SP_COUNT_RECORD"
            };

            cmd.Parameters.Add(new DataAccessParameter("@TABLE", tableName));

            var result = dataAccess.GetResult(cmd);

            if (result is null || result == DBNull.Value)
                return 0;

            return Convert.ToInt32(result);
        }
        public List<DashResumoViewModel> GetOrdersByStatus(DateTime? de = null, DateTime? ate = null)
        {
            var cmd = new DataAccessCommand
            {
                Type = CommandType.Text,
                Sql = @"
                    SELECT
                        aux.Descricao AS [Status],
                        COUNT(*) AS Qtd,
                        ISNULL(SUM(ISNULL(p.Total,0)),0) AS TotalPedidos,
                        aux.Id
                    FROM Pedidos p
                    LEFT JOIN AuxStatusPedido aux ON aux.Id = p.Status
                    WHERE (@De IS NULL OR p.DataCriacao >= @De)
                      AND (@Ate IS NULL OR p.DataCriacao < DATEADD(DAY, 1, @Ate))
                    GROUP BY aux.Descricao, aux.Id
                    ORDER BY aux.Id"
            };
            cmd.Parameters.Add(new DataAccessParameter("@De", de.HasValue ? de.Value : DBNull.Value));
            cmd.Parameters.Add(new DataAccessParameter("@Ate", ate.HasValue ? ate.Value : DBNull.Value));

            var table = dataAccess.GetDataTable(cmd);
            var result = new List<DashResumoViewModel>();

            foreach (DataRow row in table.Rows)
            {
                result.Add(new DashResumoViewModel
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Status = row["Status"]?.ToString() ?? string.Empty,
                    Qtd = Convert.ToInt32(row["Qtd"]),
                    TotalPedidos = Convert.ToDecimal(row["TotalPedidos"])
                });
            }
            return result;
        }
    }
}
