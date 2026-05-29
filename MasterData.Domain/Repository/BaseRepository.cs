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
        public async Task<T?> GetAsync<T>(DataAccessCommand cmd)
        {
            var dt = await dataAccess.GetDataTableAsync(cmd);
            return dt.ToModel<T>() ?? default;
        }
        public async Task<List<T>> GetListAsync<T>(DataAccessCommand cmd)
        {
            var dt = await dataAccess.GetDataTableAsync(cmd);
            return dt.ToModelList<T>() ?? [];
        }
        public async Task<int> SetAsync(DataAccessCommand cmd)
        {
            var id = await dataAccess.GetResultAsync(cmd);
            return Convert.ToInt32(id);
        }

        public async Task<int> CountAsync(DataAccessCommand cmd)
        {
            var result = await dataAccess.GetResultAsync(cmd);
            return Convert.ToInt32(result ?? 0);
        }
        public async Task<bool> CancelAsync(DataAccessCommand cmd)
        {
            var result = await dataAccess.GetResultAsync(cmd);
            return Convert.ToInt32(result) > 0;
        }

    }
}

