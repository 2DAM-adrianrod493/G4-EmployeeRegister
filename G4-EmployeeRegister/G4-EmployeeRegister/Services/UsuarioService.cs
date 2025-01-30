using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using G4_EmployeeRegister.Models;

namespace G4_EmployeeRegister.Services
{
    public class UsuarioService
    {
        // Creamos Lista Privada
        private ObservableCollection<UsuarioModel> _usuarioList { get; set; }

        // Obtener Listado de Usuarios ya Definidos
        public ObservableCollection<UsuarioModel> GetAllUsuarios()
        {
            _usuarioList = new ObservableCollection<UsuarioModel>();

            _usuarioList.Add(new UsuarioModel(1, "Adrián", "Rodríguez Rodríguez", "adrianrodry55@gmail.com", "rodry", "adri555", "adri.jpg", "Empleado","Informática"));
            _usuarioList.Add(new UsuarioModel(2, "Irene Naya", "Sánchez Bolívar", "laura.gomez@mail.com", "laurag", "naya555", "naya.jpg", "Empleado", "Informática"));

            return _usuarioList;
        }


        // Agregar Usuario
        public void AddUsuario(UsuarioModel usuarioModel)
        {
            _usuarioList.Add(usuarioModel);
        }

        // Eliminar Usuario
        public void RemoveUsuario(UsuarioModel usuarioModel)
        {
            _usuarioList.Remove(usuarioModel);
        }

        // Actualizar Usuario
        public void UpdateUsuario(UsuarioModel updatedUsuario)
        {
            // Buscar por ID en la lista
            var existing = _usuarioList.FirstOrDefault(u => u.IdUsuario == updatedUsuario.IdUsuario);
            if (existing != null)
            {
                existing.Nombre = updatedUsuario.Nombre;
                existing.Apellidos = updatedUsuario.Apellidos;
                existing.Email = updatedUsuario.Email;
                existing.Username = updatedUsuario.Username;
                existing.Contraseña = updatedUsuario.Contraseña;
                existing.Foto = updatedUsuario.Foto;
                existing.Rol = updatedUsuario.Rol;
                existing.Departamento = updatedUsuario.Departamento;
            }
        }
    }
}
