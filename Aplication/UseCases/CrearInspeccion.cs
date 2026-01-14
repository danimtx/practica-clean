using Aplication.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.Events;
using Domain.Interfaces;
using MediatR;
using System;
using System.Threading.Tasks;

namespace Aplication.UseCases
{
    public class CrearInspeccion
    {
        private readonly IInspeccionRepositorio _repositorio;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public CrearInspeccion(IInspeccionRepositorio repositorio, IMapper mapper, IMediator mediator)
        {
            _repositorio = repositorio;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<Inspeccion> Ejecutar(CrearInspeccionDTO dto, string adminName)
        {
            var inspeccion = _mapper.Map<Inspeccion>(dto);
            var nuevaInspeccion = await _repositorio.CrearInspeccionAsync(inspeccion);

            if (nuevaInspeccion.UsuarioId.HasValue)
            {
                var domainEvent = new InspeccionAsignadaEvent
                {
                    IdInspeccion = nuevaInspeccion.Id,
                    UsuarioIdTecnico = nuevaInspeccion.UsuarioId.Value,
                    NombreAdmin = adminName,
                    TituloInspeccion = nuevaInspeccion.NombreCliente,
                    FechaProgramada = nuevaInspeccion.FechaRegistro 
                };
                await _mediator.Publish(domainEvent);
            }

            return nuevaInspeccion;
        }
    }
}
