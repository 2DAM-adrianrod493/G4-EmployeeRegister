using System;
using System.Windows;

namespace G4_EmployeeRegister.Views
{
    public partial class AdminView : Window
    {
        public AdminView()
        {
            InitializeComponent();
            this.DataContext = new ViewModels.AdminViewModel();
        }

        private void BotonSalir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            var loginView = new LoginView();
            loginView.Show();
        }


        private void BotonBuscar_Click(object sender, RoutedEventArgs e)
        {
        }

        private void BotonFiltrar_Click(object sender, RoutedEventArgs e)
        {
        }

        // Formulario
        private void ClickAñadir(object sender, RoutedEventArgs e)
        {
        }

        private void ClickEditar(object sender, RoutedEventArgs e)
        {
        }

        private void ClickEliminar(object sender, RoutedEventArgs e)
        {
        }
    }
}
