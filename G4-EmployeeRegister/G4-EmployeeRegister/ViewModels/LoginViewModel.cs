using G4_EmployeeRegister.Models;
using G4_EmployeeRegister.Services;
using G4_EmployeeRegister.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace G4_EmployeeRegister.ViewModels
{
    class LoginViewModel : INotifyPropertyChanged
    {

        #region PROPIEDADES PARA RECOGER EL LOGIN
        private string _username;
        private string _password;
        private string _errorMessage;
        private LoginView loginView;
        private readonly LoginService _loginService;

        // Ventana de donde venimos
        private readonly LoginView _windowLogin;


        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        public string Password
        {
            private get { return _password; }  // Solo accesible dentro de la clase por seguridad
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }


        #region COMANDO LOGIN
        public RelayCommand LoginCommand { get; }

        #endregion

        public LoginViewModel(LoginView ventanaLogin)
        {
            _windowLogin = ventanaLogin;
            _loginService = new Services.LoginService();

            LoginCommand = new RelayCommand(
                  _ => GoToLogin(), _ => checkLogin()
            );


        }
        #endregion
      
        #region MÉTODOS
        private void GoToLogin()
        {
            UsuarioModel usuario = _loginService.GetUsuarioLogin(Username, Password);

            if (usuario != null)
            {
                if (usuario.Rol.Equals("Administrador"))
                {
                    // Abrimos AdminView
                    G4_EmployeeRegister.Views.AdminView adminView = new AdminView(usuario);
                    adminView.Show();
                }
                else if (usuario.Rol.Equals("Usuario"))
                {
                    // Abrimos UserView
                    G4_EmployeeRegister.Views.UserView userView = new UserView(usuario);
                    userView.Show();
                }
                

                // Cerramos el login si está inicializado
                _windowLogin?.Close();
            }
            else
            {
                MessageBox.Show("USUARIO O CONTRASEÑA INCORRECTOS!");
            }
        }


        private bool checkLogin()
        {
            bool check = false;
            if (!string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password))
            {
                check = true;
            }
            return check;
        }

        #endregion

        #region EVENTO DE NOTIFICACIÓN
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        #endregion
    }
}
