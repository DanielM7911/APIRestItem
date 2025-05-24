using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace APIRestItem.Models
{
    public class clsItem
    {
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public string categoria { get; set; }
        public string ubicacion { get; set; }
        public string horario { get; set; }
        public string imagen { get; set; }
        public int usuario_id { get; set; }

        private string cadConn = "server=localhost;user=root;password=Daniel11;database=itp_item";

        public clsItem() { }

        public clsItem(string nombre, string descripcion, string categoria, string ubicacion, string horario, string imagen, int usuario_id)
        {
            this.nombre = nombre;
            this.descripcion = descripcion;
            this.categoria = categoria;
            this.ubicacion = ubicacion;
            this.horario = horario;
            this.imagen = imagen;
            this.usuario_id = usuario_id;
        }

        public DataSet AgregarItem()
        {
            string cadSql = "CALL AgregarItem(@nombre, @descripcion, @categoria, @ubicacion, @horario, @imagen, @usuario_id)";
            using (MySqlConnection cnn = new MySqlConnection(cadConn))
            {
                MySqlDataAdapter da = new MySqlDataAdapter(cadSql, cnn);
                da.SelectCommand.Parameters.AddWithValue("@nombre", nombre);
                da.SelectCommand.Parameters.AddWithValue("@descripcion", descripcion);
                da.SelectCommand.Parameters.AddWithValue("@categoria", categoria);
                da.SelectCommand.Parameters.AddWithValue("@ubicacion", ubicacion);
                da.SelectCommand.Parameters.AddWithValue("@horario", horario);
                da.SelectCommand.Parameters.AddWithValue("@imagen", imagen);
                da.SelectCommand.Parameters.AddWithValue("@usuario_id", usuario_id);

                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
        }

        //BSUACAR ITEM
        public DataSet BuscarItemPorNombre(string nombreItem)
        {
            string cadSql = "CALL BuscarItemPorNombre(@nombre)";
            using (MySqlConnection cnn = new MySqlConnection(cadConn))
            {
                MySqlDataAdapter da = new MySqlDataAdapter(cadSql, cnn);
                da.SelectCommand.Parameters.AddWithValue("@nombre", nombreItem);

                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
        }


        // Mostrar Item
        public DataSet MostrarItems()
        {
            string cadSql = "CALL MostrarItems()";
            MySqlConnection cnn = new MySqlConnection(cadConn);
            MySqlDataAdapter da = new MySqlDataAdapter(cadSql, cnn);
            DataSet ds = new DataSet();
            da.Fill(ds, "Items");
            return ds;
        }


        public DataSet ActualizarEstadoItem(int item_id, string nuevo_estado)
        {
            string cadSql = "CALL ActualizarEstadoItem(@item_id, @nuevo_estado)";

            using (MySqlConnection cnn = new MySqlConnection(cadConn))
            {
                MySqlDataAdapter da = new MySqlDataAdapter(cadSql, cnn);
                da.SelectCommand.Parameters.AddWithValue("@item_id", item_id);
                da.SelectCommand.Parameters.AddWithValue("@nuevo_estado", nuevo_estado);

                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
        }



        //Ver iteam tabla
        public DataSet ConsultarVistaItemsPorNombre(string nombre = "")
        {
            string cadSql = string.IsNullOrWhiteSpace(nombre)
                ? "SELECT * FROM Vista_Items"
                : "SELECT * FROM Vista_Items WHERE nombre LIKE CONCAT('%', @nombre, '%')";

            using (MySqlConnection cnn = new MySqlConnection(cadConn))
            {
                MySqlDataAdapter da = new MySqlDataAdapter(cadSql, cnn);
                if (!string.IsNullOrWhiteSpace(nombre))
                    da.SelectCommand.Parameters.AddWithValue("@nombre", nombre);

                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
        }

        public DataSet BuscarItemPorID(int itemId)
        {
            string cadSql = "CALL BuscarItemPorID(@item_id)";
            using (MySqlConnection cnn = new MySqlConnection(cadConn))
            {
                MySqlDataAdapter da = new MySqlDataAdapter(cadSql, cnn);
                da.SelectCommand.Parameters.AddWithValue("@item_id", itemId);

                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
        }



    }
}