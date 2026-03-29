using AutoMapper;
using Microsoft.AspNet.OData.Query;
using SportsHome.Core.Entities;
using SportsHome.Core.Interfaces;
using SportsHome.Core.Interfaces.Services;
using SportsHome.Core.Queries.OData;

namespace SportsHome.Core.Services
{
    public class PartidosService : IPartidosService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PartidosService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Partidos?> GetAsync(int PartidoId)
        {
            return await _unitOfWork.Partidos.GetAsync(PartidoId);
        }

        public async Task<IEnumerable<Partidos>> GetAllAsync()
        {
            return await _unitOfWork.Partidos.GetListAsync();
        }

        public async Task<OQuery<Partidos>> GetFilteredAsync(ODataQueryOptions options)
        {
            return await _unitOfWork.Partidos.GetFilteredAsync(options);
        }

        public async Task<Partidos?> AddAsync(Partidos partido)
        {
            await _unitOfWork.Partidos.AddAsync(partido);
            await _unitOfWork.Complete();

            return partido;
        }

        public async Task Update(Partidos partidoOld, Partidos partidoNew)
        {
            _mapper.Map<Partidos, Partidos>(partidoNew, partidoOld);
            await _unitOfWork.Complete();
        }

        public async Task DeleteAsync(Partidos partido)
        {
            await _unitOfWork.Partidos.RemoveAsync(partido);
            await _unitOfWork.Complete();
        }

        public Task<OQuery<Partidos>> GetPartidosAsync(ODataQueryOptions options)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Partidos>> GetListAsync()
        {
            return await _unitOfWork.Partidos.GetListAsync();
        }
    }
}

