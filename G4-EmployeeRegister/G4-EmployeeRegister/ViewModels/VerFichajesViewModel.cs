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
        private readonly FichajeService _fichajeService;
        private UsuarioModel _usuario;

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
        public string texto;

        public string Texto
        {
            get => texto;
            set
            {
                texto = value;
                OnPropertyChanged(nameof(Texto));
            }
        }

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

        private void loadFichajes()
        {

            Fichajes = new ObservableCollection<FichajeModel>(_fichajeService.GetAllFichajes(_usuario));
        }

        #region COMANDOS
        public RelayCommand VolverAtrasCommand { get; }
        #endregion

        public void VolverAtras()
        {
            AdminView adminView = new AdminView(_usuario);
            adminView.Show();
            Application.Current.Windows[0].Close();

        }

        #region PÁGINA EDICIÓN
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

        #region EVENTO DE NOTIFICACIÓN
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        #endregion
    }
}