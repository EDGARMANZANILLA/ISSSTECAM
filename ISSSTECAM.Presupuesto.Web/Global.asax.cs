using ISSSTECAM.Presupuesto.Entidades;
using ISSSTECAM.Presupuesto.Entidades.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ISSSTECAM.Presupuesto.Web
{
    // Nota: para obtener instrucciones sobre cómo habilitar el modo clásico de IIS6 o IIS7, 
    // visite http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            AutoMapper.Mapper.Initialize(cfg =>{
                cfg.CreateMap<CentrosCostos, CentroCostoDTO>();
                cfg.CreateMap<Partidas, PartidaDTO>();
                cfg.CreateMap<Programas, ProgramaDTO>();
                cfg.CreateMap<Proyectos, ProyectoDTO>();
                cfg.CreateMap<Actividades, ActividadDTO>();
                cfg.CreateMap<ClavesPresupuestales, ClavePresupuestalDTO>()
                    .ForMember(dest => dest.ClavePartida, opt => opt.MapFrom(src => src.Partidas.Clave))
                    .ForMember(dest => dest.Partida, opt => opt.MapFrom(src => src.Partidas.Descripcion))
                    .ForMember(dest => dest.ClaveCentroCosto, opt => opt.MapFrom(src => src.CentrosCostos.Clave))
                    .ForMember(dest => dest.CentroCosto, opt => opt.MapFrom(src => src.CentrosCostos.ClaveNombre))
                    .ForMember(dest => dest.ClaveActividad, opt => opt.MapFrom(src => src.Actividades.Clave))
                    .ForMember(dest => dest.Actividad, opt => opt.MapFrom(src => src.Actividades.Nombre))
                    .ForMember(dest => dest.ClavePrograma, opt => opt.MapFrom(src => src.Programas.Clave))
                    .ForMember(dest => dest.Programa, opt => opt.MapFrom(src => src.Programas.Nombre))
                    .ForMember(dest => dest.ClaveProyecto, opt => opt.MapFrom(src => src.Proyectos.Clave))
                    .ForMember(dest => dest.Proyecto, opt => opt.MapFrom(src => src.Proyectos.Nombre));

                cfg.CreateMap<ClavePresupuestalDTO, ClavesPresupuestales>();
                cfg.CreateMap<NominaDTO, Nomina>();
                cfg.CreateMap<DetallesNominaDTO, DetallesNomina>();
            });
        }
    }
}