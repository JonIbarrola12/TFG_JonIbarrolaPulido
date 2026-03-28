using AutoMapper;
using Microsoft.AspNet.OData.Query;
using SportsHome.Core.Entities;
using SportsHome.Core.Interfaces;
using SportsHome.Core.Interfaces.Services;
using SportsHome.Core.Queries.OData;


namespace SportsHome.Core.Services
{
    public class EquiposService : IEquiposService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EquiposService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Equipos?> GetAsync(int EquipoId)
        {
            return await _unitOfWork.Equipos.GetAsync(EquipoId);
        }

        public async Task<IEnumerable<Equipos>> GetAllAsync()
        {
            return await _unitOfWork.Equipos.GetListAsync();
        }

        public async Task<OQuery<Equipos>> GetFilteredAsync(ODataQueryOptions options)
        {
            return await _unitOfWork.Equipos.GetFilteredAsync(options);
        }

        public async Task<Equipos?> AddAsync(Equipos equipo)
        {
            await _unitOfWork.Equipos.AddAsync(equipo);
            await _unitOfWork.Complete();

            return equipo;
        }

        public async Task Update(Equipos equipoOld, Equipos equipoNew)
        {
            _mapper.Map<Equipos, Equipos>(equipoNew, equipoOld);
            await _unitOfWork.Complete();
        }

        public async Task DeleteAsync(Equipos equipo)
        {
            await _unitOfWork.Equipos.RemoveAsync(equipo);
            await _unitOfWork.Complete();
        }

        public Task<OQuery<Equipos>> GetEquiposAsync(ODataQueryOptions options)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Equipos>> GetListAsync()
        {
            return await _unitOfWork.Equipos.GetListAsync();
        }
    }
}
