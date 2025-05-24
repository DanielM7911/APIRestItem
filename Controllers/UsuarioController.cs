using System;
using System.Data;
using System.Web.Http;
using APIRestItem.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace APIRestItem.Controllers
{
    public class UsuarioController : ApiController
    {
        // Ruta de la API para la verificación de estudiante (POST)
        [HttpPost]
        [Route("check/usuario/verificarEstudiante")]
        public clsAPIStatus VerificarEstudiante([FromBody] clsUsuario modelo)
        {
            clsAPIStatus objRespuesta = new clsAPIStatus();
            JObject jsonResp = new JObject();
            DataSet ds = new DataSet();

            try
            {
                // Crear un objeto de la clase clsUsuario para realizar la verificación
                clsUsuario objUsuario = new clsUsuario();
                ds = objUsuario.VerificarEstudiante(modelo.nombre, modelo.contraseña);

                objRespuesta.statusExec = true;

                if (ds.Tables[0].Rows.Count > 0)
                {
                    objRespuesta.ban = 0;
                    objRespuesta.msg = "Estudiante verificado exitosamente";

                    jsonResp.Add("nombre", ds.Tables[0].Rows[0]["nombre"].ToString());
                    jsonResp.Add("email", ds.Tables[0].Rows[0]["email"].ToString());
                    jsonResp.Add("rol", ds.Tables[0].Rows[0]["rol"].ToString());

                    objRespuesta.datos = jsonResp;
                }
                else
                {
                    objRespuesta.ban = 1;
                    objRespuesta.msg = "Estudiante no encontrado";
                    jsonResp.Add("msgData", "Estudiante no encontrado");
                    objRespuesta.datos = jsonResp;
                }
            }
            catch (Exception ex)
            {
                objRespuesta.statusExec = false;
                objRespuesta.ban = -1;
                objRespuesta.msg = "Usuario no encontrado";
                jsonResp.Add("msgData", ex.Message.ToString());
                objRespuesta.datos = jsonResp;
            }

            return objRespuesta;
        }


        // POST: api/usuario/registrar
        [HttpPost]
        [Route("api/usuario/registrar")]
        public clsAPIStatus RegistrarUsuario([FromBody] clsUsuario modelo)
        {
            clsAPIStatus respuesta = new clsAPIStatus();
            JObject jsonResp = new JObject();

            try
            {
                clsUsuario nuevoUsuario = new clsUsuario(modelo.nombre, modelo.email, modelo.contraseña, modelo.rol);
                DataSet ds = nuevoUsuario.AgregarUsuario();

                respuesta.statusExec = true;

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    string mensaje = ds.Tables[0].Rows[0][0].ToString();

                    if (mensaje.Contains("CORRECTAMENTE"))
                    {
                        respuesta.ban = 0;
                        respuesta.msg = "Usuario registrado exitosamente";
                    }
                    else
                    {
                        respuesta.ban = 1;
                        respuesta.msg = "No se pudo registrar";
                    }

                    jsonResp.Add("resultado", mensaje);
                    respuesta.datos = jsonResp;
                }
                else
                {
                    respuesta.ban = 1;
                    respuesta.msg = "Respuesta vacía del procedimiento";
                    jsonResp.Add("msgData", "Sin datos devueltos");
                    respuesta.datos = jsonResp;
                }
            }
            catch (Exception ex)
            {
                respuesta.statusExec = false;
                respuesta.ban = -1;
                respuesta.msg = "Error al registrar usuario";
                jsonResp.Add("msgData", ex.Message);
                respuesta.datos = jsonResp;
            }

            return respuesta;
        }


        // POST: api/usuario/modificar
        [HttpPost]
        [Route("api/usuario/modificar")]
        public clsAPIStatus ModificarUsuario([FromBody] clsUsuario modelo)
        {
            clsAPIStatus respuesta = new clsAPIStatus();
            JObject jsonResp = new JObject();

            try
            {
                clsUsuario usuario = new clsUsuario
                {
                    user_id = modelo.user_id,
                    nombre = modelo.nombre,
                    email = modelo.email,
                    contraseña = modelo.contraseña,
                    rol = modelo.rol
                };

                DataSet ds = usuario.ModificarUsuario();

                respuesta.statusExec = true;

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    string mensaje = ds.Tables[0].Rows[0][0].ToString();

                    if (mensaje.Contains("ACTUALIZADOS"))
                    {
                        respuesta.ban = 0;
                        respuesta.msg = "Usuario actualizado correctamente";
                    }
                    else
                    {
                        respuesta.ban = 1;
                        respuesta.msg = "No se encontró el usuario";
                    }

                    jsonResp.Add("resultado", mensaje);
                    respuesta.datos = jsonResp;
                }
                else
                {
                    respuesta.ban = 1;
                    respuesta.msg = "Sin respuesta del procedimiento";
                    jsonResp.Add("msgData", "El procedimiento no devolvió resultados");
                    respuesta.datos = jsonResp;
                }
            }
            catch (Exception ex)
            {
                respuesta.statusExec = false;
                respuesta.ban = -1;
                respuesta.msg = "Error al modificar usuario";
                jsonResp.Add("msgData", ex.Message);
                respuesta.datos = jsonResp;
            }

            return respuesta;
        }



        // POST: api/usuario/eliminar
        [HttpPost]
        [Route("api/usuario/eliminar")]
        public clsAPIStatus EliminarUsuario([FromBody] clsUsuario modelo)
        {
            clsAPIStatus respuesta = new clsAPIStatus();
            JObject jsonResp = new JObject();

            try
            {
                clsUsuario usuario = new clsUsuario();
                DataSet ds = usuario.EliminarUsuario(modelo.user_id);

                respuesta.statusExec = true;

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    string mensaje = ds.Tables[0].Rows[0][0].ToString();

                    if (mensaje.Contains("ELIMINADOS"))
                    {
                        respuesta.ban = 0;
                        respuesta.msg = "Usuario eliminado correctamente";
                    }
                    else
                    {
                        respuesta.ban = 1;
                        respuesta.msg = "Usuario no encontrado";
                    }

                    jsonResp.Add("resultado", mensaje);
                    respuesta.datos = jsonResp;
                }
                else
                {
                    respuesta.ban = 1;
                    respuesta.msg = "Sin respuesta del procedimiento";
                    jsonResp.Add("msgData", "El procedimiento no devolvió resultados");
                    respuesta.datos = jsonResp;
                }
            }
            catch (Exception ex)
            {
                respuesta.statusExec = false;
                respuesta.ban = -1;
                respuesta.msg = "Error al eliminar usuario";
                jsonResp.Add("msgData", ex.Message);
                respuesta.datos = jsonResp;
            }

            return respuesta;
        }


        


        [HttpGet]
        [Route("check/usuario/vwrptusuario")]
        public clsAPIStatus GetVistaUsuarios(string filtro = "")
        {
            clsAPIStatus objRespuesta = new clsAPIStatus();
            JObject jsonResp = new JObject();
            DataSet ds = new DataSet();

            try
            {
                clsUsuario objUsuario = new clsUsuario();
                ds = objUsuario.ConsultarVistaUsuarios();

                objRespuesta.statusExec = true;

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    // Aplicar filtro si viene uno
                    DataTable dt = ds.Tables[0];
                    if (!string.IsNullOrWhiteSpace(filtro))
                    {
                        DataView dv = dt.DefaultView;
                        dv.RowFilter = $"nombre LIKE '%{filtro}%' OR email LIKE '%{filtro}%'";
                        dt = dv.ToTable();
                    }

                    JArray arr = JArray.Parse(JsonConvert.SerializeObject(dt));
                    jsonResp["vwRptUsuario"] = arr;

                    objRespuesta.ban = 0;
                    objRespuesta.msg = "Consulta exitosa";
                    objRespuesta.datos = jsonResp;
                }
                else
                {
                    objRespuesta.ban = 1;
                    objRespuesta.msg = "Sin resultados";
                    jsonResp["vwRptUsuario"] = new JArray();
                    objRespuesta.datos = jsonResp;
                }
            }
            catch (Exception ex)
            {
                objRespuesta.statusExec = false;
                objRespuesta.ban = -1;
                objRespuesta.msg = "Error al consultar la vista";
                jsonResp["msgData"] = ex.Message;
                objRespuesta.datos = jsonResp;
            }

            return objRespuesta;
        }


        [HttpGet]
        [Route("check/usuario/buscar")]
        public clsAPIStatus BuscarUsuarios([FromUri] string parametro)
        {
            clsAPIStatus objRespuesta = new clsAPIStatus();
            JObject jsonResp = new JObject();
            DataSet ds = new DataSet();

            try
            {
                clsUsuario objUsuario = new clsUsuario();
                ds = objUsuario.BuscarUsuarios(parametro);

                objRespuesta.statusExec = true;

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && !ds.Tables[0].Columns.Contains("mensaje"))
                {
                    JArray arr = JArray.Parse(JsonConvert.SerializeObject(ds.Tables[0]));
                    objRespuesta.ban = 0;
                    objRespuesta.msg = "Usuario(s) encontrado(s)";
                    jsonResp["resultado"] = arr;
                }
                else
                {
                    objRespuesta.ban = 1;
                    objRespuesta.msg = "Usuario no encontrado";
                    jsonResp["msgData"] = ds.Tables[0].Rows[0][0].ToString();
                }

                objRespuesta.datos = jsonResp;
            }
            catch (Exception ex)
            {
                objRespuesta.statusExec = false;
                objRespuesta.ban = -1;
                objRespuesta.msg = "Error al buscar usuario";
                jsonResp["msgData"] = ex.Message;
                objRespuesta.datos = jsonResp;
            }

            return objRespuesta;
        }




    }

}

