using G4_EmployeeRegister.Models;
using G4_EmployeeRegister.Services;
using G4_EmployeeRegister.Views;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace G4_EmployeeRegister.ViewModels
{
    class AdminViewModel : INotifyPropertyChanged
    {
        #region PROPIEDADES DE USUARIOS
        // Servicios
        private readonly Services.UsuarioService _usuariosService;

        // ObservableCollection de usuarios
        public ObservableCollection<UsuarioModel> Usuarios { get; set; }
        private string _nombreCompleto;
        private string _nombre;
        private string _apellidos;
        private string _email;
        private string _username;
        private BitmapImage _foto;
        private string _rol;
        private string? _departamento;
        private string _contrasenia;
        private string _contraseniaAntigua;
        byte[] usuarioImg;
        private bool imagenSubida = false;
       public string NombreCompleto
    {
        get => _nombreCompleto;
        set
        {
            _nombreCompleto = value;
            OnPropertyChanged(nameof(NombreCompleto));
        }
    }

    public string Nombre
    {
        get => _nombre;
        set
        {
            _nombre = value;
            OnPropertyChanged(nameof(Nombre));
        }
    }
        public string ContraseniaAntigua
        {
            get => _contraseniaAntigua;
            set
            {
                _contraseniaAntigua = value;
                OnPropertyChanged(nameof(ContraseniaAntigua));
            }
        }
        public string Contrasenia
        {
            get => _contrasenia;
            set
            {
                _contrasenia = value;
                OnPropertyChanged(nameof(Contrasenia));
            }
        }

        public string Apellidos
    {
        get => _apellidos;
        set
        {
            _apellidos = value;
            OnPropertyChanged(nameof(Apellidos));
        }
    }

    public string Email
    {
        get => _email;
        set
        {
            _email = value;
            OnPropertyChanged(nameof(Email));
        }
    }

    public string Username
    {
        get => _username;
        set
        {
            _username = value;
            OnPropertyChanged(nameof(Username));
        }
    }

    public BitmapImage Foto
    {
        get => _foto;
        set
        {
            _foto = value;
            OnPropertyChanged(nameof(Foto));
        }
    }

    public string Rol
    {
        get => _rol;
        set
        {
            _rol = value;
            OnPropertyChanged(nameof(Rol));
        }
    }

    public string? Departamento
    {
        get => _departamento;
        set
        {
            _departamento = value;
            OnPropertyChanged(nameof(Departamento));
        }
    }
        #endregion

        #region Propiedad Usuario seleccionado
        private UsuarioModel _usuarioSeleccionado;
        public UsuarioModel UsuarioSelecionado
        {
            get
            {
                return _usuarioSeleccionado;
            }
            set
            {
                _usuarioSeleccionado = value;
                OnPropertyChanged(nameof(UsuarioSelecionado));
                // Cargar los datos en el formulario
                if(_usuarioSeleccionado != null)
                {
                    Nombre = _usuarioSeleccionado.Nombre;
                    Apellidos = _usuarioSeleccionado.Apellidos;
                    Username = _usuarioSeleccionado.Username;
                    Email = _usuarioSeleccionado.Email;
                    Departamento = _usuarioSeleccionado.Departamento;
                    Rol = _usuarioSeleccionado.Rol;
                    Foto = _usuarioSeleccionado.Foto;
                }
                
            }
        }
        #endregion

        #region COMANDOS
        // MANEJO DE USUARIOS
        public RelayCommand AddUser { get; }
        public RelayCommand EditUser { get; }
        public RelayCommand DeleteUser { get; }
        public RelayCommand MostrarFichajes { get; }
        public RelayCommand SeleccionarImagenCommand { get; }
        public RelayCommand VolverALogin { get; }
        public RelayCommand DownloadCommand { get; }
        #endregion

        #region Cargar Usuarios
        // CARGAMOS USUARIOS
        private void LoadUsers()
        {
            Usuarios = _usuariosService.GetAllUsuarios();
        }
        #endregion

        #region constructor
        public AdminViewModel(UsuarioModel usuario)
        {
            // Inicializamos los valores del usuario actual
            NombreCompleto ="Usuario Conectado: "+  usuario.Nombre + " " + usuario.Apellidos;
            Foto = usuario.Foto;
            _usuariosService = new UsuarioService();
            Usuarios = new ObservableCollection<UsuarioModel>();

            // Acciones Comandos
            AddUser = new RelayCommand(_ => AddUsuario(),_ => PuedeAniadir());

            EditUser = new RelayCommand(_ => EditUsuario(), _ => true);

            DeleteUser = new RelayCommand(_ => DeleteUsuario(),_ => true);
            MostrarFichajes = new RelayCommand(paramUsuario => VerLosFichajes(paramUsuario), _ => true);
            SeleccionarImagenCommand = new RelayCommand(_ => CargaImagen(), _ => true);
            VolverALogin = new RelayCommand(_=> VolverLoginVentana(),_=> true);
            DownloadCommand = new RelayCommand(_ => DownloadReport(UsuarioSelecionado), _ => true);
            
            // Cargamos los usuarios

            LoadUsers();
        }
        #endregion

        #region Metodo de comprobacion (Can execute)
        public bool PuedeAniadir()
        {
            if (string.IsNullOrEmpty(NombreCompleto) 
                || string.IsNullOrEmpty(Apellidos)
                || string.IsNullOrEmpty(Email) 
                || string.IsNullOrEmpty(Departamento)
                || string.IsNullOrEmpty(Rol)
                || string.IsNullOrEmpty(Contrasenia)
                || Foto==null){
                return false;

            }

            return true;
        }
        #endregion

        #region Volver Al login
        public void VolverLoginVentana()
        {
            LoginView loginView = new LoginView();
            loginView.Show();
            Application.Current.Windows[0].Close();
        }


        #endregion

        #region DESCARGAR DATOS
        // DESCARGAR DATOS
        public void DownloadReport(UsuarioModel usuarioSelecionado)
        {
            if (UsuarioSelecionado == null) //Si no hay usuario selecionado se descarga datos de todos los usuarios
            {
                _usuariosService.DownloadReportUsuarios();
            }
            else //En caso hay usuario selecionado
            {
                _usuariosService.DownloadReportUsuarioUnico(usuarioSelecionado);
            }
        }
        #endregion

        #region Cargar Imagen
        // Cargar la imagen seleccionada por el usuario
        public void CargaImagen()
        {
            // Crear un OpenFileDialog para seleccionar una imagen
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Imágenes (*.png;*.jpg;*.jpeg;*.bmp)|*.png;*.jpg;*.jpeg;*.bmp";

            try
            {
                if (openFileDialog.ShowDialog() == true)
                {
                    string archivoSeleccionado = openFileDialog.FileName;

                    // Aquí guardamos la imagen seleccionada en el arreglo de bytes
                    usuarioImg = File.ReadAllBytes(archivoSeleccionado);

                    // Creamos un BitmapImage para mostrar la imagen en la vista
                    Foto = new BitmapImage(new Uri(archivoSeleccionado));

                    // Marcar que una imagen ha sido cargada
                    imagenSubida = true;

                    // Mensaje de confirmación
                    MessageBox.Show("Imagen cargada correctamente!", "Imagen", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar la imagen: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Ver los Fichajes
        public void VerLosFichajes(object paramUser)
        {
            UsuarioModel usuario = (UsuarioModel)paramUser;
            if (usuario != null)
            {
                PaginaFichaje = new Views.VerFichajesPage(usuario);
                PaginaFichajeVisibilty = Visibility.Visible;
                ListadoVisual = Visibility.Hidden;
            }
            else
            {
                MessageBox.Show("Seleccione un usuario para ver los fichajes.", "Atención", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        #endregion

        #region ELIMINAR USUARIO
        // ELIMINAR USUARIO
        private void DeleteUsuario()
        {
            var confirmacion = MessageBox.Show("¿Quieres eliminar el usuario " + UsuarioSelecionado.Username + " ?",
                "ELIMINAR", MessageBoxButton.OKCancel);

            if (confirmacion == MessageBoxResult.OK)
            {
                _usuariosService.RemoveUsuario(UsuarioSelecionado);
                Usuarios.Remove(UsuarioSelecionado);
            }
        }
        #endregion

        #region EDITAR USUARIO
        private void EditUsuario()
        {
            // Validamos que el usuario no sea nulo
            if (UsuarioSelecionado == null)
            {
                MessageBox.Show("Error al cargar los datos del usuario.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Si el usuario intenta cambiar la contraseña, validamos la contraseña antigua
            if (!string.IsNullOrEmpty(Contrasenia))
            {
                if (string.IsNullOrEmpty(_contraseniaAntigua))
                {
                    MessageBox.Show("Debe ingresar la contraseña antigua para cambiar la contraseña.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Verificar si la contraseña antigua es correcta
                if (!_usuariosService.VerificarContrasenia(UsuarioSelecionado.Username, _contraseniaAntigua))
                {
                    MessageBox.Show("La contraseña antigua es incorrecta.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Si es correcta, actualizamos la contraseña
                UsuarioSelecionado.Contrasenia = Contrasenia;
            }

            // Actualizamos los datos del usuario con los nuevos valores
            UsuarioSelecionado.Nombre = Nombre;
            UsuarioSelecionado.Apellidos = Apellidos;
            UsuarioSelecionado.Email = Email;
            UsuarioSelecionado.Username = Username;
            UsuarioSelecionado.Departamento = Departamento;
            if (Rol.EndsWith("Usuario"))
            {
                UsuarioSelecionado.Rol = "Usuario";
            }
            else
            {
                UsuarioSelecionado.Rol = "Administrador";
            }
            

            if (imagenSubida)  // Si se ha cargado una nueva imagen
            {
                UsuarioSelecionado.Foto = Foto;
            }

            try
            {
                // Llamamos al servicio para actualizar el usuario en la base de datos
                _usuariosService.UpdateUsuario(UsuarioSelecionado);

                // Refrescamos la lista de usuarios en la UI
                LoadUsers();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar el usuario: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region AÑADIR USUARIO
        // AÑADIR USUARIO
        public void AddUsuario()
        {
            int id = Usuarios.Count() + 1;
            //String Contrasenia = "$2b$12$9Z6CSQpaRPTSKqUQaGj09.ZL7m8GtWjrGfd3M9bcshsh6yurse7NC";

            if (Usuarios.Any(user => user.Username.Equals(Username)))
            {
                MessageBox.Show("USERNAME YA EN USO");
            }
            else
            {
                if (Rol.EndsWith("Usuario"))
                {
                    Rol = "Usuario";
                }
                else
                {
                    Rol = "Administrador";
                }

                UsuarioModel usuario = new UsuarioModel(id, Nombre, Apellidos, Email, Username, Contrasenia, Foto, Rol, Departamento?.ToUpper());
                _usuariosService.AddUsuario(usuario);
                Usuarios.Add(usuario);
            }
        }
        #endregion
     
        #region CONTROL DE VISIBILIDAD
        private Visibility _paginaFichajeVisibilty = Visibility.Hidden;
        public Visibility PaginaFichajeVisibilty
        {
            get => _paginaFichajeVisibilty;
            set
            {
                _paginaFichajeVisibilty = value;
                OnPropertyChanged(nameof(PaginaFichajeVisibilty));
            }
        }
        



        private Visibility _listadoVisual = Visibility.Visible;
        public Visibility ListadoVisual
        {
            get => _listadoVisual;
            set
            {
                _listadoVisual = value;
                OnPropertyChanged(nameof(ListadoVisual));
            }
        }
        #endregion

        #region Página de Edición
        private Page _paginaFichaje;
        public Page PaginaFichaje
        {
            get => _paginaFichaje;
            set
            {
                _paginaFichaje = value;
                OnPropertyChanged(nameof(PaginaFichaje));
            }
        }
        #endregion

        #region EVENTO DE NOTIFICACIÓN
        // Evento de PropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        #endregion
    }
}
