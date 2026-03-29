using AutoMapper;
using Microsoft.AspNet.OData.Query;
using SportsHome.Core.Entities;
using SportsHome.Core.Interfaces;
using SportsHome.Core.Interfaces.Services;
using SportsHome.Core.Queries.OData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsHome.Core.Services
{
    public class SyncLogsService : ISyncLogsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SyncLogsService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<SyncLogs?> GetAsync(int syncLogId)
        {
            return await _unitOfWork.SyncLogs.GetAsync(syncLogId);
        }

        public async Task<IEnumerable<SyncLogs>> GetAllAsync()
        {
            return await _unitOfWork.SyncLogs.GetListAsync();
        }

        public async Task<OQuery<SyncLogs>> GetFilteredAsync(ODataQueryOptions options)
        {
            return await _unitOfWork.SyncLogs.GetFilteredAsync(options);
        }

        public async Task<SyncLogs?> AddAsync(SyncLogs syncLog)
        {
            await _unitOfWork.SyncLogs.AddAsync(syncLog);
            await _unitOfWork.Complete();

            return syncLog;
        }

        public async Task Update(SyncLogs syncLogOld, SyncLogs syncLogNew)
        {
            _mapper.Map<SyncLogs, SyncLogs>(syncLogNew, syncLogOld);
            await _unitOfWork.Complete();
        }

        public async Task DeleteAsync(SyncLogs syncLog)
        {
            await _unitOfWork.SyncLogs.RemoveAsync(syncLog);
            await _unitOfWork.Complete();
        }

        public Task<OQuery<SyncLogs>> GetSyncLogsAsync(ODataQueryOptions options)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<SyncLogs>> GetListAsync()
        {
            return await _unitOfWork.SyncLogs.GetListAsync();
        }
    }
}
