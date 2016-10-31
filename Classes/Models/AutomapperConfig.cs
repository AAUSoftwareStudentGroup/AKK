using AutoMapper;
namespace AKK.Classes.Models {
    public static class Mappings {
        private static MapperConfiguration config = new MapperConfiguration(cfg => {
            cfg.CreateMap<Route, RouteDataTransferObject>()
                .ForMember(dest => dest.SectionName, 
                           opts => opts.MapFrom(r => r.Section.Name));
            cfg.CreateMap<Section, SectionTransferObject>();
        });
        public static IMapper Mapper { get; } = config.CreateMapper();
    }
}