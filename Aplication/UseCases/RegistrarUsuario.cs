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
    public class RegistrarUsuario
    {
        private readonly IUsuarioRepositorio _repositorio;
        private readonly IMapper _mapper;

        public RegistrarUsuario(IUsuarioRepositorio repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        public async Task<Usuario> Ejecutar(UsuarioRegistroDTO dto)
        {
            var usuario = _mapper.Map<Usuario>(dto);
            // Aquí iría la lógica para hashear la password antes de guardar
            usuario.PasswordHash = dto.Password;
            return await _repositorio.CrearUsuarioAsync(usuario);
        }
    }
}
