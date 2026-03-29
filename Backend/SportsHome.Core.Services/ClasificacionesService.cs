using AutoMapper;
using Microsoft.AspNet.OData.Query;
using SportsHome.Core.Entities;
using SportsHome.Core.Interfaces;
using SportsHome.Core.Interfaces.Services;
using SportsHome.Core.Queries.OData;

namespace SportsHome.Core.Services
{
    public class ClasificacionesService : IClasificacionesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ClasificacionesService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Clasificaciones?> GetAsync(int ClasificacionId)
        {
            return await _unitOfWork.Clasificaciones.GetAsync(ClasificacionId);
        }

        public async Task<IEnumerable<Clasificaciones>> GetAllAsync()
        {
            return await _unitOfWork.Clasificaciones.GetListAsync();
        }

        public async Task<OQuery<Clasificaciones>> GetFilteredAsync(ODataQueryOptions options)
        {
            return await _unitOfWork.Clasificaciones.GetFilteredAsync(options);
        }

        public async Task<Clasificaciones?> AddAsync(Clasificaciones clasificacion)
        {
            await _unitOfWork.Clasificaciones.AddAsync(clasificacion);
            await _unitOfWork.Complete();

            return clasificacion;
        }

        public async Task Update(Clasificaciones clasificacionOld, Clasificaciones clasificacionNew)
        {
            _mapper.Map<Clasificaciones, Clasificaciones>(clasificacionNew, clasificacionOld);
            await _unitOfWork.Complete();
        }

        public async Task DeleteAsync(Clasificaciones clasificacion)
        {
            await _unitOfWork.Clasificaciones.RemoveAsync(clasificacion);
            await _unitOfWork.Complete();
        }

        public Task<OQuery<Clasificaciones>> GetClasificacionesAsync(ODataQueryOptions options)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Clasificaciones>> GetListAsync()
        {
            return await _unitOfWork.Clasificaciones.GetListAsync();
        }
    }
}
