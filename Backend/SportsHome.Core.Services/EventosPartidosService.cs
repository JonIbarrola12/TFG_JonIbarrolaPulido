using AutoMapper;
using Microsoft.AspNet.OData.Query;
using SportsHome.Core.Entities;
using SportsHome.Core.Interfaces;
using SportsHome.Core.Interfaces.Services;
using SportsHome.Core.Queries.OData;

namespace SportsHome.Core.Services
{
    public class EventosPartidosService : IEventosPartidosService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EventosPartidosService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<EventosPartidos?> GetAsync(int eventoPartidoId)
        {
            return await _unitOfWork.EventosPartidos.GetAsync(eventoPartidoId);
        }

        public async Task<IEnumerable<EventosPartidos>> GetAllAsync()
        {
            return await _unitOfWork.EventosPartidos.GetListAsync();
        }

        public async Task<OQuery<EventosPartidos>> GetFilteredAsync(ODataQueryOptions options)
        {
            return await _unitOfWork.EventosPartidos.GetFilteredAsync(options);
        }

        public async Task<EventosPartidos?> AddAsync(EventosPartidos eventoPartido)
        {
            await _unitOfWork.EventosPartidos.AddAsync(eventoPartido);
            await _unitOfWork.Complete();

            return eventoPartido;
        }

        public async Task Update(EventosPartidos eventoPartidoOld, EventosPartidos eventoPartidoNew)
        {
            _mapper.Map<EventosPartidos, EventosPartidos>(eventoPartidoNew, eventoPartidoOld);
            await _unitOfWork.Complete();
        }

        public async Task DeleteAsync(EventosPartidos eventoPartido)
        {
            await _unitOfWork.EventosPartidos.RemoveAsync(eventoPartido);
            await _unitOfWork.Complete();
        }

        public Task<OQuery<EventosPartidos>> GetEventosPartidosAsync(ODataQueryOptions options)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<EventosPartidos>> GetListAsync()
        {
            return await _unitOfWork.EventosPartidos.GetListAsync();
        }
    }
}
