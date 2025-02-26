using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using ClosedXML.Excel;
using G4_EmployeeRegister.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Win32;

namespace G4_EmployeeRegister.Services
{
    public class UsuarioService
    {
        #region Miembros Privados
        // CREAMOS UNA LISTA PRIVADA
        private ObservableCollection<UsuarioModel> _usuarioList { get; set; }
        private string connectionString = ConfigurationManager.ConnectionStrings["Conexion_App"].ConnectionString;
        #endregion

        #region Obtener Usuarios
        // OBTENEMOS LOS USUARIOS
        public ObservableCollection<UsuarioModel> GetAllUsuarios()
        {
            _usuarioList = new ObservableCollection<UsuarioModel>();
            UsuarioModel usuario = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"SELECT IdUsuario, Nombre, Apellidos, Email, Username, 
                            Contrasenia,Foto, Rol,
                            Departamento FROM Usuarios Where Rol='Usuario';
                        ";
                using (SqlCommand cmdQuery = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = cmdQuery.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int idUsuario = Convert.ToInt32(reader["IdUsuario"]);
                            string nombre = reader["Nombre"].ToString();
                            string apellidos = reader["Apellidos"].ToString();
                            string email = reader["Email"].ToString();
                            string username = reader["Username"].ToString();
                            string contrasenia = reader["Contrasenia"].ToString();
                            string rol = reader["Rol"].ToString();
                            string departamento = reader["Departamento"].ToString();

                            // Manejo de la imagen
                            BitmapImage imagUser = null; // Inicializa la imagen como null

                            if (reader["Foto"] != DBNull.Value) // Verifica si no es NULL
                            {
                                // Convertir el resultado a un array de bytes
                                byte[] imagenBytes = (byte[])reader["Foto"];

                                // Utilizar un MemoryStream para leer los bytes de la imagen
                                using (MemoryStream ms = new MemoryStream(imagenBytes))
                                {
                                    imagUser = new BitmapImage();
                                    imagUser.BeginInit();
                                    imagUser.CacheOption = BitmapCacheOption.OnLoad; // Cargar la imagen completamente en memoria
                                    imagUser.StreamSource = ms; // Asignar el MemoryStream como fuente de la imagen
                                    imagUser.EndInit();
                                }
                            }
                            else
                            {
                                imagUser = null;
                            }

                            usuario = new UsuarioModel(idUsuario, nombre, apellidos, email, username,
                                contrasenia, imagUser, rol, departamento);
                            _usuarioList.Add(usuario);
                        }
                    }
                }
                return _usuarioList;
            }
        }
        #endregion

        #region MÉTODOS DE DESCARGAR DATOS
        // DESCARGAR DATOS USUARIOS
        public void DownloadReportUsuarios()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Title = "Guardar Reporte";
            saveFileDialog.Filter = "Archivos Excel (*.xlsx)|*.xlsx";
            saveFileDialog.FileName = "Reporte_Usuarios.xlsx";

            if (saveFileDialog.ShowDialog() == true) // Si el usuario selecciona una ubicación
            {
                string rutaArchivo = saveFileDialog.FileName;
                using (SqlConnection conexion = new SqlConnection(connectionString))
                {
                    conexion.Open();
                    string query = @"SELECT IdUsuario, Nombre, Apellidos, Email, Username, 
                            Contrasenia, Rol,
                            Departamento FROM Usuarios Where Rol='Usuario';
                            ";

                    using (SqlCommand cmd = new SqlCommand(query, conexion))
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            using (XLWorkbook wb = new XLWorkbook())
                            {
                                var hoja = wb.Worksheets.Add(dt, "Usuarios");

                                hoja.Columns().AdjustToContents();

                                hoja.Row(1).Style.Font.Bold = true;
                                var lastcolumn = hoja.LastColumnUsed().ColumnNumber();
                                var lastrow = hoja.LastRowUsed().RowNumber();

                                hoja.Range(1, 1, 1, lastcolumn).Style.Fill.SetBackgroundColor(XLColor.BabyBlue);
                                hoja.Range(2, 1, lastrow, lastcolumn).Style.Fill.SetBackgroundColor(XLColor.WhiteSmoke);
                                wb.SaveAs(rutaArchivo);
                            }
                        }
                    }
                }
                MessageBox.Show($"Report InfoUsuarios guardado en:\n{rutaArchivo}",
                "Generación Correcta", MessageBoxButton.OK,
                MessageBoxImage.Information);
            }
        }

        // DESCARGAR DATOS USUARIOS
        public void DownloadReportUsuarioUnico(UsuarioModel usuario)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Title = "Guardar Reporte";
            saveFileDialog.Filter = "Archivos Excel (*.xlsx)|*.xlsx";
            saveFileDialog.FileName = "Reporte_" + usuario.Nombre + "_" + usuario.Apellidos + "_" + usuario.IdUsuario + ".xlsx";

            if (saveFileDialog.ShowDialog() == true) // Si el usuario selecciona una ubicación
            {
                string rutaArchivo = saveFileDialog.FileName;
                using (SqlConnection conexion = new SqlConnection(connectionString))
                {
                    conexion.Open();
                    string query = @"SELECT IdUsuario, Nombre, Apellidos, Email, Username, 
                                    Contrasenia, Rol, Departamento
                                    FROM Usuarios u
                                    Where u.IdUsuario = @IdUsuario;";

                    using (SqlCommand cmd = new SqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@IdUsuario", usuario.IdUsuario);

                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            using (XLWorkbook wb = new XLWorkbook())
                            {
                                var hoja = wb.Worksheets.Add(dt, "Usuarios");

                                hoja.Columns().AdjustToContents();

                                hoja.Row(1).Style.Font.Bold = true;
                                var lastcolumn = hoja.LastColumnUsed().ColumnNumber();
                                var lastrow = hoja.LastRowUsed().RowNumber();

                                hoja.Range(1, 1, 1, lastcolumn).Style.Fill.SetBackgroundColor(XLColor.BabyBlue);
                                hoja.Range(2, 1, lastrow, lastcolumn).Style.Fill.SetBackgroundColor(XLColor.WhiteSmoke);
                                wb.SaveAs(rutaArchivo);
                            }
                        }
                    }
                }
                MessageBox.Show($"Report InfoUsuario guardado en:\n{rutaArchivo}",
                "Generación Correcta", MessageBoxButton.OK,
                MessageBoxImage.Information);
            }
        }
        #endregion

        #region Operaciones CRUD de Usuario
        // AGREGAR USUARIO
        public void AddUsuario(UsuarioModel usuarioModel)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"INSERT INTO Usuarios (Nombre, Apellidos, Email, Username, Contrasenia, Foto, Rol, Departamento) 
                         VALUES (@Nombre, @Apellidos, @Email, @Username, @Contrasenia, @Foto, @Rol, @Departamento);";

                string contaseniaHashieada = BCrypt.Net.BCrypt.HashPassword(usuarioModel.Contrasenia);
                using (SqlCommand cmdQuery = new SqlCommand(query, connection))
                {
                    cmdQuery.Parameters.AddWithValue("@Nombre", usuarioModel.Nombre);
                    cmdQuery.Parameters.AddWithValue("@Apellidos", usuarioModel.Apellidos);
                    cmdQuery.Parameters.AddWithValue("@Email", usuarioModel.Email);
                    cmdQuery.Parameters.AddWithValue("@Username", usuarioModel.Username);
                    cmdQuery.Parameters.AddWithValue("@Contrasenia", contaseniaHashieada);
                    cmdQuery.Parameters.AddWithValue("@Rol", usuarioModel.Rol);
                    cmdQuery.Parameters.AddWithValue("@Departamento", usuarioModel.Departamento);

                    // Manejo de la imagen
                    if (usuarioModel.Foto != null)
                    {
                        // Convertir la imagen a un array de bytes
                        byte[] imagenBytes;
                        using (MemoryStream ms = new MemoryStream())
                        {
                            BitmapEncoder encoder = new PngBitmapEncoder();
                            encoder.Frames.Add(BitmapFrame.Create(usuarioModel.Foto));
                            encoder.Save(ms);
                            imagenBytes = ms.ToArray();
                        }
                        cmdQuery.Parameters.AddWithValue("@Foto", imagenBytes);
                    }
                    else
                    {
                        // Si no hay imagen, insertar NULL
                        cmdQuery.Parameters.AddWithValue("@Foto", DBNull.Value);
                    }

                    cmdQuery.ExecuteNonQuery();
                }
            }
        }

        // ELIMINAR USUARIO
        public void RemoveUsuario(UsuarioModel usuarioModel)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM Usuarios WHERE IdUsuario = @IdUsuario;";

                using (SqlCommand cmdQuery = new SqlCommand(query, connection))
                {
                    cmdQuery.Parameters.AddWithValue("@IdUsuario", usuarioModel.IdUsuario);
                    cmdQuery.ExecuteNonQuery();
                }
            }
        }

        public bool VerificarContrasenia(string username, string contraseniaAntigua)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Contrasenia FROM Usuarios WHERE Username = @Username";

                using (SqlCommand cmdQuery = new SqlCommand(query, connection))
                {
                    cmdQuery.Parameters.AddWithValue("@Username", username);

                    object result = cmdQuery.ExecuteScalar();
                    if (result != null)
                    {
                        string contraseniaHash = result.ToString();
                        return BCrypt.Net.BCrypt.Verify(contraseniaAntigua, contraseniaHash);
                    }
                }
            }
            return false;
        }

        // ACTUALIZAR USUARIO
        public void UpdateUsuario(UsuarioModel updatedUsuario)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Verifica si el usuario existe antes de actualizar
                string checkQuery = "SELECT COUNT(*) FROM Usuarios WHERE IdUsuario = @IdUsuario";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, connection))
                {
                    checkCmd.Parameters.AddWithValue("@IdUsuario", updatedUsuario.IdUsuario);
                    int count = (int)checkCmd.ExecuteScalar();

                    if (count == 0)
                    {
                        MessageBox.Show("Usuario no encontrado.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }

                // Si el usuario existe, procede con la actualización
                string updateQuery = @"UPDATE Usuarios SET 
                                Nombre = @Nombre, Apellidos = @Apellidos, Email = @Email, 
                                Username = @Username, Contrasenia = @Contrasenia, 
                                Foto = @Foto, Rol = @Rol, Departamento = @Departamento
                                WHERE IdUsuario = @IdUsuario";

                string contaseniaHashieada = BCrypt.Net.BCrypt.HashPassword(updatedUsuario.Contrasenia);

                using (SqlCommand updateCmd = new SqlCommand(updateQuery, connection))
                {
                    updateCmd.Parameters.AddWithValue("@IdUsuario", updatedUsuario.IdUsuario);
                    updateCmd.Parameters.AddWithValue("@Nombre", updatedUsuario.Nombre);
                    updateCmd.Parameters.AddWithValue("@Apellidos", updatedUsuario.Apellidos);
                    updateCmd.Parameters.AddWithValue("@Email", updatedUsuario.Email);
                    updateCmd.Parameters.AddWithValue("@Username", updatedUsuario.Username);
                    updateCmd.Parameters.AddWithValue("@Contrasenia", contaseniaHashieada);
                    updateCmd.Parameters.AddWithValue("@Rol", updatedUsuario.Rol);
                    updateCmd.Parameters.AddWithValue("@Departamento", updatedUsuario.Departamento);

                    // Manejo de imagen
                    if (updatedUsuario.Foto != null)
                    {
                        byte[] imagenBytes;
                        using (MemoryStream ms = new MemoryStream())
                        {
                            BitmapEncoder encoder = new PngBitmapEncoder();
                            encoder.Frames.Add(BitmapFrame.Create(updatedUsuario.Foto));
                            encoder.Save(ms);
                            imagenBytes = ms.ToArray();
                        }
                        updateCmd.Parameters.AddWithValue("@Foto", imagenBytes);
                    }
                    else
                    {
                        updateCmd.Parameters.AddWithValue("@Foto", DBNull.Value);
                    }

                    updateCmd.ExecuteNonQuery();
                    MessageBox.Show("Usuario actualizado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
        #endregion
    }
}
