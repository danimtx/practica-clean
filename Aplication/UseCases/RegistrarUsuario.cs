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
        private readonly ICargoRepositorio _cargoRepositorio;
        private readonly IMapper _mapper;

        public RegistrarUsuario(IUsuarioRepositorio repositorio, ICargoRepositorio cargoRepositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _cargoRepositorio = cargoRepositorio;
            _mapper = mapper;
        }

        public async Task<Usuario> Ejecutar(UsuarioRegistroDTO dto)
        {
            var usuario = _mapper.Map<Usuario>(dto);

            var cargoInvitado = await _cargoRepositorio.ObtenerPorNombreAsync("Invitado");
            if (cargoInvitado == null)
            {
                // En un caso real, podrías tener una lógica más robusta aquí.
                throw new InvalidOperationException("El cargo 'Invitado' no se encontró en la base de datos.");
            }

            usuario.PasswordHash = dto.Password; // TODO: Hash password
            usuario.CargoId = cargoInvitado.Id;
            usuario.EstaActivo = false;
            usuario.Permisos = new List<string> { "home" };
            usuario.FotoPerfil = "/uploads/profiles/default.png";

            return await _repositorio.CrearUsuarioAsync(usuario);
        }
    }
}
