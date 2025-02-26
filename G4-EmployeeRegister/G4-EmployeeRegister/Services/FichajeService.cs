using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using ClosedXML.Excel;
using G4_EmployeeRegister.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Win32;

namespace G4_EmployeeRegister.Services
{
    public class FichajeService
    {
        // CREAMOS UNA LISTA PRIVADA
        private ObservableCollection<FichajeModel> _fichajeList { get; set; }

        // CADENA DE CONEXIÓN A LA BD
        private string connectionString = ConfigurationManager.ConnectionStrings["Conexion_App"].ConnectionString;

        // OBTENEMOS LOS FICHAJES
        public ObservableCollection<FichajeModel> GetAllFichajes(UsuarioModel usuario)
        {

            _fichajeList = new ObservableCollection<FichajeModel>();
            FichajeModel fichaje = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {


                connection.Open();

                // CONSULTA QUERY
                string query = @"SELECT f.IdFichaje, f.IdUsuario, f.FechaHora, f.Tipo, f.Observaciones 
                                FROM Fichajes f join Usuarios u on f.IdUsuario = u.IdUsuario 
                                Where u.IdUsuario = @IdUsuario;";

                using (SqlCommand cmdQuery = new SqlCommand(query, connection))
                {
                    cmdQuery.Parameters.AddWithValue("@IdUsuario", usuario.IdUsuario);

                    using (SqlDataReader reader = cmdQuery.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int idFichaje = Convert.ToInt32(reader["IdFichaje"]);
                            int idUsuario = Convert.ToInt32(reader["IdUsuario"]);
                            DateTime fechaHora = Convert.ToDateTime(reader["FechaHora"]);
                            string tipo = reader["Tipo"].ToString();
                            string observaciones = reader["Observaciones"].ToString();

                            fichaje = new FichajeModel(idFichaje, idUsuario, fechaHora, tipo, observaciones);
                            _fichajeList.Add(fichaje);
                        }
                    }

                }
            }
            return _fichajeList;
        }

        #region Descargar fichajes
        // DESCARGAR HISTORIAL
        public void DownloadReportFichajesHistorial(UsuarioModel usuario)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Title = "Guardar Reporte";
            saveFileDialog.Filter = "Archivos Excel (*.xlsx)|*.xlsx";
            saveFileDialog.FileName = "Reporte_Fichajes_" + usuario.Nombre + "_" + usuario.Apellidos + "_" + usuario.IdUsuario + ".xlsx";

            if (saveFileDialog.ShowDialog() == true) // Si el usuario selecciona una ubicación
            {
                string rutaArchivo = saveFileDialog.FileName;
                using (SqlConnection conexion = new SqlConnection(connectionString))
                {
                    conexion.Open();
                    string query = @"SELECT f.IdFichaje, f.IdUsuario, f.FechaHora, f.Tipo, f.Observaciones 
                            FROM Fichajes f join Usuarios u on f.IdUsuario = u.IdUsuario 
                            Where u.IdUsuario = @IdUsuario;";

                    using (SqlCommand cmd = new SqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@IdUsuario", usuario.IdUsuario);

                        using (SqlDataAdapter adapter = new
                        SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            using (XLWorkbook wb = new XLWorkbook())
                            {
                                var hoja = wb.Worksheets.Add(dt,
                                "Fichaje");

                                hoja.Columns().AdjustToContents();

                                hoja.Row(1).Style.Font.Bold = true;
                                var lastcolumn = hoja.LastColumnUsed().ColumnNumber();
                                var lastrow = hoja.LastRowUsed().RowNumber();

                                hoja.Range(1, 1, 1,
                                lastcolumn).Style.Fill.SetBackgroundColor(XLColor.BabyBlue);
                                hoja.Range(2, 1, lastrow,
                                lastcolumn).Style.Fill.SetBackgroundColor(XLColor.WhiteSmoke);
                                wb.SaveAs(rutaArchivo);
                            }
                        }
                    }
                }
                MessageBox.Show($"Report HistorialUsuarios guardado en:\n{rutaArchivo}",
                "Generación Correcta", MessageBoxButton.OK,
                MessageBoxImage.Information);
            }
        }
        #endregion


        // AGREGAR FICHAJE
        public void AddFichaje(FichajeModel fichajeModel)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = @"INSERT INTO Fichajes (IdUsuario, FechaHora, Tipo, Observaciones)
                                VALUES (@IdUsuario, @FechaHora, @Tipo, @Observaciones);";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@IdUsuario", fichajeModel.IdUsuario);
                    cmd.Parameters.AddWithValue("@FechaHora", fichajeModel.FechaHora);
                    cmd.Parameters.AddWithValue("@Tipo", fichajeModel.Tipo);
                    cmd.Parameters.AddWithValue("@Observaciones", fichajeModel.Observaciones);

                    cmd.ExecuteNonQuery();
                }
            }
            _fichajeList.Add(fichajeModel);
        }
    }
}
