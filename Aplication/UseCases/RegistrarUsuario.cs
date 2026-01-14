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
            usuario.PasswordHash = dto.Password; // TODO: Hash password
            usuario.FotoPerfil = "/uploads/profiles/default.png";

            if (dto.CargoId.HasValue)
            {
                // Creación por un admin
                var cargo = await _cargoRepositorio.ObtenerPorIdAsync(dto.CargoId.Value);
                if (cargo == null) throw new InvalidOperationException("El cargo especificado no existe.");
                
                usuario.CargoId = cargo.Id;
                usuario.EstaActivo = true; // El admin crea usuarios activos por defecto
                usuario.Permisos = new List<string>(); // El admin los gestionará después
            }
            else
            {
                // Registro público
                var cargoInvitado = await _cargoRepositorio.ObtenerPorNombreAsync("Invitado");
                if (cargoInvitado == null) throw new InvalidOperationException("El cargo 'Invitado' no se encontró.");

                usuario.CargoId = cargoInvitado.Id;
                usuario.EstaActivo = false;
                usuario.Permisos = new List<string> { "home" };
            }

            return await _repositorio.CrearUsuarioAsync(usuario);
        }
    }
}
