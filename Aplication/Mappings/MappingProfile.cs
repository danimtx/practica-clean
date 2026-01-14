using Aplication.DTOs;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//mapping propile sirve para mapear los dtos con las entidades y viceversa
namespace Aplication.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UsuarioRegistroDTO, Usuario>();

            CreateMap<Usuario, UsuarioDTO>()
                .ForMember(dest => dest.Cargo, opt => opt.MapFrom(src => src.Cargo.Nombre));

            CreateMap<CrearInspeccionDTO, Inspeccion>()
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => "Pendiente"));
            CreateMap<Inspeccion, InspeccionDetalleDTO>()
                .ForMember(dest => dest.NombreResponsable,
                    opt => opt.MapFrom(src => src.Tecnico != null ? src.Tecnico.Nombre : "Sin asignar"));
        }

    }
}
