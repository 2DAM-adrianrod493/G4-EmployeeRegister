using G4_EmployeeRegister.Models;
using G4_EmployeeRegister.Services;
using G4_EmployeeRegister.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace G4_EmployeeRegister.ViewModels
{
    public class VerFichajesViewModel : INotifyPropertyChanged
    {
        #region Campos Privados
        private readonly FichajeService _fichajeService;
        private UsuarioModel _usuario;
        #endregion

        #region Propiedades
        // Propiedad que contiene los fichajes de un usuario
        private ObservableCollection<FichajeModel> _fichajes;
        public ObservableCollection<FichajeModel> Fichajes
        {
            get => _fichajes;
            set
            {
                _fichajes = value;
                OnPropertyChanged(nameof(Fichajes));
            }
        }

        public string NombreCompleto { get => _usuario.Nombre + " " + _usuario.Apellidos; }

        private string texto;
        public string Texto
        {
            get => texto;
            set
            {
                texto = value;
                OnPropertyChanged(nameof(Texto));
            }
        }
        #endregion

        #region Constructor
        public VerFichajesViewModel(UsuarioModel usuario)
        {
            Texto = "Historial de fichaje";
            _usuario = usuario;
            _fichajeService = new FichajeService();

            // Cargamos los fichajes del usuario a la propiedad
            Fichajes = new ObservableCollection<FichajeModel>();
            loadFichajes();
            VolverAtrasCommand = new RelayCommand(_ => VolverAtras(), _ => true);
        }
        #endregion

        #region Métodos Privados
        private void loadFichajes()
        {
            Fichajes = new ObservableCollection<FichajeModel>(_fichajeService.GetAllFichajes(_usuario));
        }
        #endregion

        #region Comandos y Navegación
        public RelayCommand VolverAtrasCommand { get; }
        public void VolverAtras()
        {
            AdminView adminView = new AdminView(_usuario);
            adminView.Show();
            Application.Current.Windows[0].Close();
        }
        #endregion

        #region Página de Edición
        private Page _paginaFichaje;
        public Page PaginaAdmin
        {
            get => _paginaFichaje;
            set
            {
                _paginaFichaje = value;
                OnPropertyChanged(nameof(PaginaAdmin));
            }
        }
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        #endregion
    }
}
