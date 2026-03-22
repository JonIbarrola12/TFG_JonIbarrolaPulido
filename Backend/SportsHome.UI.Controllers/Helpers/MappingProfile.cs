using AutoMapper;
using SportsHome.Core.Entities;
using SportsHome.UI.Controllers.Resources;

namespace SportsHome.UI.Controllers.Helpers
{
    public class MappingProfile : Profile 
    {
        public MappingProfile()
        {
            #region Ligas
            CreateMap<Ligas, LigasResource>();

            CreateMap<LigasResource, Ligas>()
                .ForMember(l => l.LigaId, opt => opt.Ignore());

            CreateMap<Ligas, Ligas>()
                .ForMember(l => l.LigaId, opt => opt.Ignore());
            #endregion
        }
    }
}
