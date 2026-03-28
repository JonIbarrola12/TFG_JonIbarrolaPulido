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
    public class JugadoresService : IJugadoresService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public JugadoresService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Jugadores?> GetAsync(int JugadorId)
        {
            return await _unitOfWork.Jugadores.GetAsync(JugadorId);
        }

        public async Task<IEnumerable<Jugadores>> GetAllAsync()
        {
            return await _unitOfWork.Jugadores.GetListAsync();
        }

        public async Task<OQuery<Jugadores>> GetFilteredAsync(ODataQueryOptions options)
        {
            return await _unitOfWork.Jugadores.GetFilteredAsync(options);
        }

        public async Task<Jugadores?> AddAsync(Jugadores jugador)
        {
            await _unitOfWork.Jugadores.AddAsync(jugador);
            await _unitOfWork.Complete();

            return jugador;
        }

        public async Task Update(Jugadores jugadorOld, Jugadores jugadorNew)
        {
            _mapper.Map<Jugadores, Jugadores>(jugadorNew, jugadorOld);
            await _unitOfWork.Complete();
        }

        public async Task DeleteAsync(Jugadores jugador)
        {
            await _unitOfWork.Jugadores.RemoveAsync(jugador);
            await _unitOfWork.Complete();
        }

        public Task<OQuery<Jugadores>> GetJugadoresAsync(ODataQueryOptions options)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Jugadores>> GetListAsync()
        {
            return await _unitOfWork.Jugadores.GetListAsync();
        }
    }
}
