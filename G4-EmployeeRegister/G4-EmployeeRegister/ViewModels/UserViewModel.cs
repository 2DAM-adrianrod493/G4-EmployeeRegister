using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Windows;
using G4_EmployeeRegister.Models;
using G4_EmployeeRegister.Services;
using G4_EmployeeRegister.ViewModels;
using System.ComponentModel;
using G4_EmployeeRegister.Views;
using System.Linq;

namespace G4_EmployeeRegister.ViewModels
{
    public class UserViewModel : INotifyPropertyChanged
    {
        private readonly FichajeService _fichajeService;
        private UsuarioModel _usuario;
        public ObservableCollection<FichajeModel> Fichajes { get; set; }

        #region COMANDOS
        public RelayCommand IniciarJornadaCommand { get; set; }
        public RelayCommand FinalizarJornadaCommand { get; set; }
        public RelayCommand VolverALogin { get; }
        public RelayCommand DownloadCommandHistorial { get; }
        #endregion

        #region CONSTRUCTOR
        public UserViewModel(UsuarioModel usuario)
        {
            _usuario = usuario;
            _fichajeService = new FichajeService();
            Fichajes = new ObservableCollection<FichajeModel>(_fichajeService.GetAllFichajes(usuario));

            // Inicializamos comandos
            IniciarJornadaCommand = new RelayCommand(param => IniciarJornada(), _ => true);
            FinalizarJornadaCommand = new RelayCommand(param => FinalizarJornada(), _ => true);
            VolverALogin = new RelayCommand(_ => VolverLoginVentana(), _ => true);
            DownloadCommandHistorial = new RelayCommand(_ => DownloadReportFichajesHistorial(), _ => true);

        }
        #endregion

        public string NombreCompleto { get => _usuario.Nombre + " " + _usuario.Apellidos; }

        #region MÉTODOS
        private void IniciarJornada()
        {
            // Verificar si hay fichajes pendientes del día anterior
            if (HayFichajesPendientes())
            {
                MessageBox.Show("Debe registrar una SALIDA antes de iniciar una nueva jornada.",
                    "FICHAJE PENDIENTE", MessageBoxButton.OK, MessageBoxImage.Warning);
                RegistrarFichaje("Salida"); // Obliga a fichar la salida
                return;
            }

            // Verificar si ya hay un fichaje de entrada en el día actual
            if (FichajeDelDia("Entrada"))
            {
                MessageBox.Show("Ya ha fichado ENTRADA hoy. No puede registrar otra entrada.",
                    "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            RegistrarFichaje("Entrada");
        }

        private void FinalizarJornada()
        {
            // Verificar si ya hay un fichaje de salida en el día actual
            if (FichajeDelDia("Salida"))
            {
                MessageBox.Show("Ya ha fichado SALIDA hoy. No puede registrar otra salida.",
                    "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // No se puede fichar salida sin haber fichado entrada antes
            if (!FichajeDelDia("Entrada"))
            {
                MessageBox.Show("No puede fichar SALIDA sin haber fichado una ENTRADA.",
                    "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            RegistrarFichaje("Salida");
        }

        private bool HayFichajesPendientes()
        {
            var ultimoFichaje = Fichajes.LastOrDefault();
            if (ultimoFichaje == null) return false;

            return ultimoFichaje.Tipo == "Entrada" && ultimoFichaje.FechaHora.Date < DateTime.Now.Date;
        }

        private bool FichajeDelDia(string tipo)
        {
            return Fichajes.Any(f => f.Tipo == tipo && f.FechaHora.Date == DateTime.Now.Date);
        }

        private void RegistrarFichaje(string tipo)
        {
            var nuevoFichaje = new FichajeModel(0, _usuario.IdUsuario, DateTime.Now, tipo, "");
            _fichajeService.AddFichaje(nuevoFichaje);
            Fichajes.Add(nuevoFichaje);
            OnPropertyChanged(nameof(Fichajes));

            MessageBox.Show($"{tipo.ToUpper()} registrada correctamente.", "FICHAJE",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }


        public void VolverLoginVentana()
        {
            LoginView loginView = new LoginView();
            loginView.Show();
            Application.Current.Windows[0].Close();
        }
        #endregion

        #region DESCARGAR DATOS
        // DESCARGAR DATOS
        public void DownloadReportFichajesHistorial()
        {

            _fichajeService.DownloadReportFichajesHistorial(_usuario);
            //if (UsuarioSelecionado != null) {
            //    _usuariosService.DownloadReportUsuarios();
            //} else
            //{
            //    _usuariosService.DownloadReportUsuario();
            //}
        }
        #endregion

        #region NOTIFICACIÓN
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        #endregion
    }
}
