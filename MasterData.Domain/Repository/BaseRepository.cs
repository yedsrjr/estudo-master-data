using JJMasterData.Commons.Data;
using MasterData.Domain.Extensions;
using MasterData.Domain.Models.DTOs;
using MasterData.Domain.Models.DTOs.Customer;
using System;
using System.Collections.Generic;
using System.Text;

namespace MasterData.Domain.Repository
{
    public class BaseRepository(DataAccess dataAccess)
    {
        public async Task<List<T>> GetAsync<T>(DataAccessCommand cmd)
        {
            var dt = await dataAccess.GetDataTableAsync(cmd);
            return dt.ToModelList<T>() ?? [];
        }

        public async Task<int> CountAsync(DataAccessCommand cmd)
        {
            var result = await dataAccess.GetResultAsync(cmd);
            return Convert.ToInt32(result ?? 0);
        }

        public async Task<PagedResult<T>> GetPagedAsync<T>(DataAccessCommand listCmd, DataAccessCommand countCmd)
        {
            var total = await CountAsync(countCmd);
            var items = await GetAsync<T>(listCmd);

            return new PagedResult<T>
            {
                Total = total,
                Items = items
            };
        }
        public async Task<int> SetAsync(DataAccessCommand cmd)
        {
            return await dataAccess.SetCommandAsync(cmd);
        }

    }
}

