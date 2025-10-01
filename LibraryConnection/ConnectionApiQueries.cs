using LibraryConnection.ControllerAzure;
using LibraryConnection.DbSet;
using LibraryConnection.Models.ApiQueries;
using LibraryEntities.Models;
using Newtonsoft.Json;
using RestSharp;
using System.Dynamic;
using System.Net;
using System.Text.Json;

namespace LibraryConnection
{
    public class ConnectionApiQueries
    {

        /// <summary>
        /// Realiza consultas a la API Queries y devuelve la respuesta.
        /// </summary>
        /// <param name="token">Token de autenticación.</param>
        /// <param name="requests">Solicitud de consultas a la API.</param>
        /// <returns>Respuesta dinámica de la API.</returns>
        public static Response<dynamic> requestApiQueries(string token, RequestsApiQueries requests)
        {
            string?  kg = string.Empty;
            var payload = token.Split('.')[1];
            var jsonPayload = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(payload.PadRight(payload.Length + (4 - payload.Length % 4) % 4, '=')));

            using (JsonDocument doc = JsonDocument.Parse(jsonPayload))
            {
                JsonElement root = doc.RootElement;
                kg = root.GetProperty("Kg").GetString(); ;
            }

            var oResponse = GroupController.GetDbGroupById(Convert.ToInt32(kg));

            var client = new RestClient("http://"+ oResponse.response.addressIIS+":"+ oResponse.response.portIIS);
            var request = new RestRequest("/api/Query", Method.Post);
            // Agregar el bearer token
            request.AddHeader("Authorization", token);
            request.AddJsonBody(JsonConvert.SerializeObject(requests));
            var response = client.Execute(request);

            if(response.StatusCode != HttpStatusCode.Unauthorized)
            {
                Response<dynamic> result = JsonConvert.DeserializeObject<Response<dynamic>>(response.Content)!;

                result.response = (result.response != null) ? ConvertResponseToDynamicList(result.response) : null;

                return result;
            }
            else
            {
                throw new Exception("Error requestApiQueries : " + HttpStatusCode.Unauthorized);
            }            
        }

                /// <summary>
        /// Realiza consultas a la API Queries y devuelve la respuesta.
        /// </summary>
        /// <param name="token">Token de autenticación.</param>
        /// <param name="requests">Solicitud de consultas a la API.</param>
        /// <returns>Respuesta dinámica de la API.</returns>
        public static Response<dynamic> requestApiQueriesWithOutToken(string query, RequestsApiQueries requests)
        {
            var oResponse = GroupController.GetDbGroupByKeyGroup(query);

            var client = new RestClient("http://"+ oResponse.response.addressIIS+":"+ oResponse.response.portIIS);
            var request = new RestRequest("/api/Query", Method.Post);
            request.AddJsonBody(JsonConvert.SerializeObject(requests));
            var response = client.Execute(request);

            if(response.StatusCode != HttpStatusCode.Unauthorized)
            {
                Response<dynamic> result = JsonConvert.DeserializeObject<Response<dynamic>>(response.Content)!;

                result.response = (result.response != null) ? ConvertResponseToDynamicList(result.response) : null;

                return result;
            }
            else
            {
                throw new Exception("Error requestApiQueries : " + HttpStatusCode.Unauthorized);
            }            
        }

        /// <summary>
        /// Convierte un objeto dinámico de respuesta de una consulta en una lista de objetos dinámicos.
        /// </summary>
        /// <param name="response">Respuesta dinámica que contiene los datos de la consulta.</param>
        /// <returns>Lista de objetos dinámicos.</returns>
        public static List<dynamic> ConvertResponseToDynamicList(dynamic response)
        {
            return ((IEnumerable<dynamic>)response).Select(item =>
            {
                IDictionary<string, object> dynamicItem = new ExpandoObject()!;
                foreach (Newtonsoft.Json.Linq.JProperty property in item)
                {
                    if (property.Value is Newtonsoft.Json.Linq.JValue value)
                    {
                        dynamicItem[property.Name] = value.Value!;
                    }
                    if (property.Value.ToString().Equals("{}"))
                    {
                        dynamicItem[property.Name] = string.Empty;
                    }
                }
                return dynamicItem as dynamic;
            }).ToList();
        }
    }
}
