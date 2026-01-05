using Aplication.DTOs;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UsuarioRegistroDTO, Usuario>()
                .ForMember(dest => dest.EstaActivo, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.Cargo, opt => opt.MapFrom(src => "Invitado"))
                .ForMember(dest => dest.Permisos, opt => opt.MapFrom(src => new List<string> { "home" }));

            CreateMap<CrearInspeccionDTO, Inspeccion>()
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => "Pendiente"));
            CreateMap<Inspeccion, InspeccionDetalleDTO>()
                .ForMember(dest => dest.NombreResponsable,
                    opt => opt.MapFrom(src => src.Tecnico != null ? src.Tecnico.Nombre : "Sin asignar"));
        }

    }
}
