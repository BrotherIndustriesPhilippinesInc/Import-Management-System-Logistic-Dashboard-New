using AutoMapper;
using LogisticDashboard.API.DTO;
using LogisticDashboard.Core;

namespace LogisticDashboard.API.Mapping
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<SailingSchedule, SailingScheduleNoRoutesDTO>().ReverseMap();

            CreateMap<Routes, RouteDto>()
            .ForMember(dest => dest.Schedules,
                       opt => opt.MapFrom(src => src.SailingSchedule));
        }
    }
}
