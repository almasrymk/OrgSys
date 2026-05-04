using AutoMapper;
using Microsoft.Extensions.Logging;
using System;

namespace Entity
{
    public static class MapperExtension
    {
        public static IMapper Mapper { get; set; }
        //public static t Map<t>(this BaseModel ob) where t : BaseModel
        //{
        //    if (Mapper == null)
        //        Mapper = Activator.CreateInstance<MapperConfig>()?.Mapper;
        //    return Mapper.Map<t>(ob);
        //}

        public static T Map<T>(this BaseModel ob) where T : BaseModel
        {
            if (Mapper == null)
                InitializeMapper();

            return Mapper.Map<T>(ob);
        }

        public static void InitializeMapper()
        {
            if (Mapper == null)
            {
                var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());


                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<MapperConfig>();
                } , loggerFactory);
                Mapper = config.CreateMapper();
            }
        }
    }
}