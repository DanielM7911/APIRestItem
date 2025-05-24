using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;
using APIRestItem.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace APIRestItem.Controllers
{
    public class ItemController : ApiController
    {
        [HttpPost]
        [Route("check/item/agregaritem")]
        public clsAPIStatus AgregarItem([FromBody] clsItem modelo)
        {
            clsAPIStatus objRespuesta = new clsAPIStatus();
            JObject jsonResp = new JObject();
            DataSet ds = new DataSet();

            try
            {
                clsItem objItem = new clsItem(
                    modelo.nombre,
                    modelo.descripcion,
                    modelo.categoria,
                    modelo.ubicacion,
                    modelo.horario,
                    modelo.imagen,
                    modelo.usuario_id
                );

                ds = objItem.AgregarItem();

                objRespuesta.statusExec = true;
                string mensaje = ds.Tables[0].Rows[0][0].ToString();

                if (mensaje.Contains("CORRECTAMENTE"))
                {
                    objRespuesta.ban = 0;
                    objRespuesta.msg = "Item agregado correctamente";
                }
                else
                {
                    objRespuesta.ban = 1;
                    objRespuesta.msg = "No se pudo agregar el item";
                }

                jsonResp.Add("msgData", mensaje);
                objRespuesta.datos = jsonResp;
            }
            catch (Exception ex)
            {
                objRespuesta.statusExec = false;
                objRespuesta.ban = -1;
                objRespuesta.msg = "Error al agregar el item";
                jsonResp.Add("msgData", ex.Message);
                objRespuesta.datos = jsonResp;
            }

            return objRespuesta;
        }



        //BUSCAR ITEM
        [HttpPost]
        [Route("check/item/buscaritem")]
        public clsAPIStatus BuscarItem([FromBody] JObject body)
        {
            clsAPIStatus objRespuesta = new clsAPIStatus();
            JObject jsonResp = new JObject();

            try
            {
                string nombreItem = body["nombre"]?.ToString();

                if (string.IsNullOrWhiteSpace(nombreItem))
                {
                    objRespuesta.statusExec = false;
                    objRespuesta.ban = 1;
                    objRespuesta.msg = "Nombre de ítem no válido";
                    jsonResp.Add("msgData", "Debes ingresar un nombre válido");
                    objRespuesta.datos = jsonResp;
                    return objRespuesta;
                }

                clsItem objItem = new clsItem();
                DataSet ds = objItem.BuscarItemPorNombre(nombreItem);

                objRespuesta.statusExec = true;

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && !ds.Tables[0].Columns.Contains("mensaje"))
                {
                    objRespuesta.ban = 0;
                    objRespuesta.msg = "Ítems encontrados";

                    JArray items = new JArray();
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        JObject item = new JObject
                        {
                            ["item_id"] = row["item_id"].ToString(),
                            ["nombre"] = row["item_nombre"].ToString(),
                            ["descripcion"] = row["descripcion"].ToString(),
                            ["categoria"] = row["categoria"].ToString(),
                            ["ubicacion"] = row["ubicacion"].ToString(),
                            ["horario"] = row["horario"].ToString(),
                            ["estado"] = row["estado"].ToString(),
                            ["imagen"] = row["imagen"].ToString(),
                            ["propietario"] = row["propietario"].ToString()
                        };
                        items.Add(item);
                    }

                    jsonResp.Add("items", items);
                }
                else
                {
                    objRespuesta.ban = 1;
                    objRespuesta.msg = "No se encontraron ítems";
                    jsonResp.Add("msgData", "Sin coincidencias");
                }

                objRespuesta.datos = jsonResp;
            }
            catch (Exception ex)
            {
                objRespuesta.statusExec = false;
                objRespuesta.ban = -1;
                objRespuesta.msg = "Error al buscar el ítem";
                jsonResp.Add("msgData", ex.Message);
                objRespuesta.datos = jsonResp;
            }

            return objRespuesta;
        }

        //Mostrar Item
        [HttpGet]
        [Route("mostraritems")]
        public clsAPIStatus MostrarItems()
        {
            clsAPIStatus objRespuesta = new clsAPIStatus();
            JArray jsonArray = new JArray();

            try
            {
                clsItem objItem = new clsItem();
                DataSet ds = objItem.MostrarItems();

                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        JObject itemJson = new JObject
                        {
                            ["item_id"] = int.Parse(row["item_id"].ToString()),
                            ["nombre"] = row["nombre"].ToString(),
                            ["descripcion"] = row["descripcion"].ToString(),
                            ["categoria"] = row["categoria"].ToString(),
                            ["ubicacion"] = row["ubicacion"].ToString(),
                            ["horario"] = row["horario"].ToString(),
                            ["estado"] = row["estado"].ToString(),
                            ["imagen"] = row["imagen"].ToString(),
                            ["usuario_id"] = int.Parse(row["usuario_id"].ToString()),
                            ["nombre_usuario"] = row["nombre_usuario"].ToString(),
                            ["email"] = row["email"].ToString()
                        };
                        jsonArray.Add(itemJson);
                    }

                    objRespuesta.statusExec = true;
                    objRespuesta.ban = 0;
                    objRespuesta.msg = "Ítems obtenidos correctamente";
                    objRespuesta.datos = new JObject { ["items"] = jsonArray };
                }
                else
                {
                    objRespuesta.statusExec = true;
                    objRespuesta.ban = 1;
                    objRespuesta.msg = "No se encontraron ítems";
                    objRespuesta.datos = new JObject();
                }
            }
            catch (Exception ex)
            {
                objRespuesta.statusExec = false;
                objRespuesta.ban = -1;
                objRespuesta.msg = "Error al obtener los ítems";
                objRespuesta.datos = new JObject { ["msgData"] = ex.Message };
            }

            return objRespuesta;
        }

        // ESTATUS ITEM
        [HttpPost]
        [Route("check/item/actualizarestado")]
        public clsAPIStatus ActualizarEstadoItem([FromBody] JObject data)
        {
            clsAPIStatus objRespuesta = new clsAPIStatus();
            JObject jsonResp = new JObject();
            DataSet ds = new DataSet();

            try
            {
                int item_id = data["item_id"].ToObject<int>();
                string nuevo_estado = data["estado"].ToString();

                clsItem objItem = new clsItem();
                ds = objItem.ActualizarEstadoItem(item_id, nuevo_estado);

                objRespuesta.statusExec = true;

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    string mensaje = ds.Tables[0].Rows[0][0].ToString();
                    objRespuesta.ban = 0;
                    objRespuesta.msg = mensaje;
                    jsonResp["msgData"] = mensaje;
                }
                else
                {
                    objRespuesta.ban = 1;
                    objRespuesta.msg = "No se realizó ningún cambio";
                    jsonResp["msgData"] = "Respuesta vacía del procedimiento";
                }

                objRespuesta.datos = jsonResp;
            }
            catch (Exception ex)
            {
                objRespuesta.statusExec = false;
                objRespuesta.ban = -1;
                objRespuesta.msg = "Error al actualizar estado del ítem";
                jsonResp["msgData"] = ex.Message;
                objRespuesta.datos = jsonResp;
            }

            return objRespuesta;
        }

        //VER ITEMS POR NOMBRE

        [HttpGet]
        [Route("check/item/vwmostraritem")]
        public clsAPIStatus GetVistaItemsPorNombre([FromUri] string nombre = "")
        {
            clsAPIStatus objRespuesta = new clsAPIStatus();
            JObject jsonResp = new JObject();

            try
            {
                clsItem objItem = new clsItem();
                DataSet ds = objItem.ConsultarVistaItemsPorNombre(nombre);

                objRespuesta.statusExec = true;

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    // Si vino filtro, mostrar solo el primero; si no, todos
                    if (!string.IsNullOrWhiteSpace(nombre) && ds.Tables[0].Rows.Count == 1)
                    {
                        var fila = ds.Tables[0].Rows[0];
                        var dict = new Dictionary<string, object>();

                        foreach (DataColumn col in ds.Tables[0].Columns)
                        {
                            dict[col.ColumnName] = fila[col];
                        }

                        jsonResp["item"] = JObject.FromObject(dict);
                    }
                    else
                    {
                        JArray items = JArray.Parse(JsonConvert.SerializeObject(ds.Tables[0]));
                        jsonResp["items"] = items;
                    }

                    objRespuesta.ban = 0;
                    objRespuesta.msg = "Consulta exitosa";
                }
                else
                {
                    objRespuesta.ban = 1;
                    objRespuesta.msg = "No se encontraron ítems";
                    jsonResp["items"] = new JArray();
                }

                objRespuesta.datos = jsonResp;
            }
            catch (Exception ex)
            {
                objRespuesta.statusExec = false;
                objRespuesta.ban = -1;
                objRespuesta.msg = "Error al consultar ítems";
                jsonResp["msgData"] = ex.Message;
                objRespuesta.datos = jsonResp;
            }

            return objRespuesta;
        }

        [HttpGet]
        [Route("check/item/buscarporid")]
        public clsAPIStatus BuscarItemPorID([FromUri] int item_id)
        {
            clsAPIStatus objRespuesta = new clsAPIStatus();
            JObject jsonResp = new JObject();

            try
            {
                clsItem objItem = new clsItem();
                DataSet ds = objItem.BuscarItemPorID(item_id);

                objRespuesta.statusExec = true;

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && !ds.Tables[0].Columns.Contains("mensaje"))
                {
                    JObject item = JObject.Parse(JsonConvert.SerializeObject(ds.Tables[0].Rows[0]));
                    jsonResp["item"] = item;
                    objRespuesta.ban = 0;
                    objRespuesta.msg = "Ítem encontrado";
                }
                else
                {
                    objRespuesta.ban = 1;
                    objRespuesta.msg = "Ítem no encontrado";
                    jsonResp["msgData"] = "No se encontró un ítem con ese ID";
                }

                objRespuesta.datos = jsonResp;
            }
            catch (Exception ex)
            {
                objRespuesta.statusExec = false;
                objRespuesta.ban = -1;
                objRespuesta.msg = "Error al buscar ítem";
                jsonResp["msgData"] = ex.Message;
                objRespuesta.datos = jsonResp;
            }

            return objRespuesta;
        }




    }
}