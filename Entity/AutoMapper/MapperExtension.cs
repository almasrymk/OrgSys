using System;
using AutoMapper;

namespace Entity
{
    public static class MapperExtension
    {
        public static IMapper Mapper { get; set; }
        public static t Map<t>(this BaseModel ob) where t : BaseModel
        {
            if (Mapper == null)
                Mapper = Activator.CreateInstance<MapperConfig>()?.Mapper;
            return Mapper.Map<t>(ob);
        }
    }
}