using System;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace G4_EmployeeRegister.Models
{
    public class UsuarioModel : INotifyPropertyChanged
    {
        // Parámetros
        private int _idUsuario;
        private string _nombre;
        private string _apellidos;
        private string _email;
        private string _username;
        private string _contraseña;
        private string _foto;
        private string _rol;
        private string? _departamento;

        // Constructor
        public UsuarioModel(int idUsuario, string nombre, string apellidos, string email, string username, string contraseña, string foto, string rol, string departamento)
        {
            IdUsuario = idUsuario;
            Nombre = nombre;
            Apellidos = apellidos;
            Email = email;
            Username = username;
            Contraseña = contraseña;
            Foto = foto;
            Rol = rol;
        }

        public int IdUsuario
        {
            get => _idUsuario;
            set
            {
                if (_idUsuario != value)
                {
                    _idUsuario = value;
                    OnPropertyChanged(nameof(IdUsuario));
                }
            }
        }

        public string Nombre
        {
            get => _nombre;
            set
            {
                if (_nombre != value)
                {
                    _nombre = value;
                    OnPropertyChanged(nameof(Nombre));
                }
            }
        }

        public string Apellidos
        {
            get => _apellidos;
            set
            {
                if (_apellidos != value)
                {
                    _apellidos = value;
                    OnPropertyChanged(nameof(Apellidos));
                }
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged(nameof(Email));
                }
            }
        }

        public string Username
        {
            get => _username;
            set
            {
                if (_username != value)
                {
                    _username = value;
                    OnPropertyChanged(nameof(Username));
                }
            }
        }

        public string Contraseña
        {
            get => _contraseña;
            set
            {
                if (_contraseña != value)
                {
                    _contraseña = value;
                    OnPropertyChanged(nameof(Contraseña));
                }
            }
        }

        public string Foto
        {
            get => _foto;
            set
            {
                if (_foto != value)
                {
                    _foto = value;
                    OnPropertyChanged(nameof(Foto));
                }
            }
        }

        public string Rol
        {
            get => _rol;
            set
            {
                if (_rol != value)
                {
                    _rol = value;
                    OnPropertyChanged(nameof(Rol));
                }
            }
        }

        public string Departamento
        {
            get => _departamento;
            set
            {
                if (_departamento != value)
                {
                    _departamento = value;
                    OnPropertyChanged(nameof(Departamento));
                }
            }
        }

        // Evento para Notificar Cambios en las Propiedades.
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
