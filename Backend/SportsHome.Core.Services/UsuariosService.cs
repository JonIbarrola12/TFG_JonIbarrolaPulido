using AutoMapper;
using SportsHome.Core.Entities;
using SportsHome.Core.Interfaces;
using SportsHome.Core.Interfaces.Services;

namespace SportsHome.Core.Services
{
    public class UsuariosService : IUsuariosService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UsuariosService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Usuarios?> GetAsync(int usuarioId)
        {
            return await _unitOfWork.Usuarios.GetAsync(usuarioId);
        }

        public async Task<IEnumerable<Usuarios>> GetListAsync()
        {
            return await _unitOfWork.Usuarios.GetListAsync();
        }

        public async Task<Usuarios?> AddAsync(Usuarios usuario)
        {
            await _unitOfWork.Usuarios.AddAsync(usuario);
            await _unitOfWork.Complete();

            return usuario;
        }

        public async Task Update(Usuarios usuarioOld, Usuarios usuarioNew)
        {
            _mapper.Map<Usuarios, Usuarios>(usuarioNew, usuarioOld);
            await _unitOfWork.Complete();
        }

        public async Task DeleteAsync(Usuarios usuario)
        {
            await _unitOfWork.Usuarios.RemoveAsync(usuario);
            await _unitOfWork.Complete();
        }
    }
}
