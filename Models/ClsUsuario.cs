using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace APIRestItem.Models
{
    public class clsUsuario
    {
        public int user_id { get; set; }
        public string nombre { get; set; }
        public string contraseña { get; set; }
        public string email { get; set; }
        public string rol { get; set; }


        // Cadena de conexión a la base de datos (reemplaza con tu propia cadena)
        private string cadConn = "server=localhost;user=root;password=Daniel11;database=itp_item";

        // Constructor sin parámetros
        public clsUsuario() { }

        // Constructor con parámetros
        public clsUsuario(string nombre, string contrasena)
        {
            this.nombre = nombre;
            this.contraseña = contrasena;
        
        }
        public clsUsuario(string nombre, string email, string contraseña, string rol)
        {

            this.nombre = nombre;
            this.contraseña = contraseña;
            this.email = email;
            this.rol = rol;



        }

        // Método que ejecuta el procedimiento almacenado 'VerificarEstudiante'
        public DataSet VerificarEstudiante(string nombre, string contrasena)
        {
            string cadSql = "CALL VerificarEstudiante(@nombre, @contrasena)";
            MySqlConnection cnn = new MySqlConnection(cadConn); // Crear la conexión
            MySqlDataAdapter da = new MySqlDataAdapter(cadSql, cnn); // Adaptador de datos
            da.SelectCommand.Parameters.AddWithValue("@nombre", nombre); // Parámetro de nombre
            da.SelectCommand.Parameters.AddWithValue("@contrasena", contrasena); // Parámetro de contraseña

            DataSet ds = new DataSet();
            try
            {
                // Llenar el DataSet con el resultado de la ejecución del procedimiento almacenado
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                // Manejo de errores en caso de que falle la conexión o la consulta
                throw new Exception("Error al ejecutar el procedimiento: " + ex.Message);
            }
            return ds;
        }

        // Método para agregar usuario
        public DataSet AgregarUsuario()
        {
            string cadSql = "CALL AgregarUsuario(@nombre, @correo, @contrasena, @rol)";
            MySqlConnection cnn = new MySqlConnection(cadConn);
            MySqlDataAdapter da = new MySqlDataAdapter(cadSql, cnn);

            da.SelectCommand.Parameters.AddWithValue("@nombre", this.nombre);
            da.SelectCommand.Parameters.AddWithValue("@correo", this.email);
            da.SelectCommand.Parameters.AddWithValue("@contrasena", this.contraseña);
            da.SelectCommand.Parameters.AddWithValue("@rol", this.rol);

            DataSet ds = new DataSet();
            try { da.Fill(ds); }
            catch (Exception ex)
            {
                throw new Exception("Error al registrar usuario: " + ex.Message);
            }
            return ds;
        }

        //  NUEVO MÉTODO: ModificarUsuario
        public DataSet ModificarUsuario()
        {
            string cadSql = "CALL ModificarUsuario(@user_id, @nombre, @correo, @contrasena, @rol)";
            MySqlConnection cnn = new MySqlConnection(cadConn);
            MySqlDataAdapter da = new MySqlDataAdapter(cadSql, cnn);
            da.SelectCommand.Parameters.AddWithValue("@user_id", this.user_id);
            da.SelectCommand.Parameters.AddWithValue("@nombre", this.nombre);
            da.SelectCommand.Parameters.AddWithValue("@correo", this.email);
            da.SelectCommand.Parameters.AddWithValue("@contrasena", this.contraseña);
            da.SelectCommand.Parameters.AddWithValue("@rol", this.rol);

            DataSet ds = new DataSet();
            try { da.Fill(ds); }
            catch (Exception ex) { throw new Exception("Error al modificar usuario: " + ex.Message); }

            return ds;
        }

        //ELIMINAR USUARIOS
        public DataSet EliminarUsuario(int user_id)
        {
            string cadSql = "CALL EliminarUsuario(@user_id)";
            MySqlConnection cnn = new MySqlConnection(cadConn);
            MySqlDataAdapter da = new MySqlDataAdapter(cadSql, cnn);
            da.SelectCommand.Parameters.AddWithValue("@user_id", user_id);

            DataSet ds = new DataSet();
            try
            {
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al ejecutar el procedimiento: " + ex.Message);
            }
            return ds;
        }

        



       

        //VISTA USUARIO
        public DataSet ConsultarVistaUsuarios()
        {
            string cadSql = "SELECT * FROM Vista_Usuarios";
            using (MySqlConnection cnn = new MySqlConnection(cadConn))
            {
                MySqlDataAdapter da = new MySqlDataAdapter(cadSql, cnn);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
        }

        // BUSCAR USUARIO GET
        public DataSet BuscarUsuarios(string parametro)
        {
            string cadSql = "CALL BuscarUsuario(@parametro)";
            using (MySqlConnection cnn = new MySqlConnection(cadConn))
            {
                MySqlDataAdapter da = new MySqlDataAdapter(cadSql, cnn);
                da.SelectCommand.Parameters.AddWithValue("@parametro", parametro);

                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
        }

    }
}
