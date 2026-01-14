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
                .ForMember(dest => dest.Cargo, opt => opt.MapFrom(src => src.Cargo.Nombre))
                .ForMember(dest => dest.EstaActivo, opt => opt.MapFrom(src => src.EstaActivo)); // Add this line

            CreateMap<CrearInspeccionDTO, Inspeccion>()
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => "Pendiente"));

            CreateMap<Inspeccion, InspeccionDetalleDTO>();
        }

    }
}
