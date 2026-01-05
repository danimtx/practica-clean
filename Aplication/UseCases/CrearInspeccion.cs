using Aplication.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.UseCases
{
    public class CrearInspeccion
    {
        private readonly IInspeccionRepositorio _repositorio;
        private readonly IMapper _mapper;

        public CrearInspeccion(IInspeccionRepositorio repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<Inspeccion> Ejecutar(CrearInspeccionDTO dto)
        {
            var inspeccion = _mapper.Map<Inspeccion>(dto);
            return await _repositorio.CrearInspeccionAsync(inspeccion);
        }
    }
}
