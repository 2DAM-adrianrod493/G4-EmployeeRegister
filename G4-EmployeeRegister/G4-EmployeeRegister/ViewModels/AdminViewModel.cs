using G4_EmployeeRegister.Models;
using G4_EmployeeRegister.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G4_EmployeeRegister.ViewModels
{


    class AdminViewModel : INotifyPropertyChanged
    {
        private readonly Services.UsuarioService _usuariosService;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<UsuarioModel> Usuarios { get; set; }

        private string _nombre, _idUsuario, _rol;
        public string Nombre
        {
            get { return _nombre; }
            set
            {
                _nombre = value;
                OnPropertyChanged(nameof(Nombre));
            }
        }
        public string ID
        {
            get
            {
                return _idUsuario;
            }
            set
            {
                _idUsuario = value;
                OnPropertyChanged(nameof(ID));
            }
        }
        #region Métodos de Notificación (INotifyPropertyChanged)
        protected void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        #endregion
        public string Rol
        {
            get { 
                return _rol; 
            }
            set {
                _rol = value; 
            }
        }

        public AdminViewModel()
        {

            _usuariosService = new Services.UsuarioService();
            Usuarios = new ObservableCollection<UsuarioModel>();

            LoadUsers();

        }



        private void LoadUsers()
        {
            Usuarios = _usuariosService.GetAllUsuarios();
        }
    }
}
