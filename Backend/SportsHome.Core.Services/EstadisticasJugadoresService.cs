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
    public class EstadisticasJugadoresService : IEstadisticasJugadoresService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EstadisticasJugadoresService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<EstadisticasJugadores?> GetAsync(int estadisticaJugadorId)
        {
            return await _unitOfWork.EstadisticasJugadores.GetAsync(estadisticaJugadorId);
        }

        public async Task<IEnumerable<EstadisticasJugadores>> GetAllAsync()
        {
            return await _unitOfWork.EstadisticasJugadores.GetListAsync();
        }

        public async Task<OQuery<EstadisticasJugadores>> GetFilteredAsync(ODataQueryOptions options)
        {
            return await _unitOfWork.EstadisticasJugadores.GetFilteredAsync(options);
        }

        public async Task<EstadisticasJugadores?> AddAsync(EstadisticasJugadores estadisticaJugador)
        {
            await _unitOfWork.EstadisticasJugadores.AddAsync(estadisticaJugador);
            await _unitOfWork.Complete();

            return estadisticaJugador;
        }

        public async Task Update(EstadisticasJugadores estadisticaJugadorOld, EstadisticasJugadores estadisticaJugadorNew)
        {
            _mapper.Map<EstadisticasJugadores, EstadisticasJugadores>(estadisticaJugadorNew, estadisticaJugadorOld);
            await _unitOfWork.Complete();
        }

        public async Task DeleteAsync(EstadisticasJugadores estadisticaJugador)
        {
            await _unitOfWork.EstadisticasJugadores.RemoveAsync(estadisticaJugador);
            await _unitOfWork.Complete();
        }

        public Task<OQuery<EstadisticasJugadores>> GetEstadisticasJugadoresAsync(ODataQueryOptions options)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<EstadisticasJugadores>> GetListAsync()
        {
            return await _unitOfWork.EstadisticasJugadores.GetListAsync();
        }
    }
}

