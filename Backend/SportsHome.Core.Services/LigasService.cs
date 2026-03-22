using AutoMapper;
using Microsoft.AspNet.OData.Query;
using SportsHome.Core.Entities;
using SportsHome.Core.Interfaces;
using SportsHome.Core.Interfaces.Services;
using SportsHome.Core.Queries.OData;

namespace SportsHome.Core.Services
{
    public class LigasService : ILigasService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LigasService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Ligas?> GetAsync(int LigaId)
        {
            return await _unitOfWork.Ligas.GetAsync(LigaId);
        }

        public async Task<IEnumerable<Ligas>> GetAllAsync()
        {
            return await _unitOfWork.Ligas.GetListAsync();
        }

        public async Task<OQuery<Ligas>> GetFilteredAsync(ODataQueryOptions options)
        {
            return await _unitOfWork.Ligas.GetFilteredAsync(options);
        }

        public async Task<Ligas?> AddAsync(Ligas liga)
        {
            await _unitOfWork.Ligas.AddAsync(liga);
            await _unitOfWork.Complete();

            return liga;
        }

        public async Task Update(Ligas ligaOld, Ligas ligaNew)
        {
            _mapper.Map<Ligas, Ligas>(ligaNew, ligaOld);
            await _unitOfWork.Complete();
        }

        public async Task DeleteAsync(Ligas liga)
        {
            await _unitOfWork.Ligas.RemoveAsync(liga);
            await _unitOfWork.Complete();
        }

        public Task<OQuery<Ligas>> GetLigasAsync(ODataQueryOptions options)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Ligas>> GetListAsync()
        {
            return await _unitOfWork.Ligas.GetListAsync();
        }
    }
}
