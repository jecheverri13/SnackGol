using B1ServiceLayerCSS;
using LibraryConnection.Context;
using LibraryConnection.DbSet;
using LibraryConnection.Models.ApiQueries;
using Newtonsoft.Json;
using RestSharp;
using ServiceLayerCSS.DTO;
using ServiceLayerCSS;
using ServiceLayerCSS.Enum;
using ServiceLayerCSS.Utilities;
using System.Net;
using System.Security.Claims;
using System.Dynamic;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using LibraryEntities.Models;
using LibraryEntities.EntitiesSL;
using System.Text.RegularExpressions;
using LibraryConnection.ControllerAzure;

namespace LibraryConnection
{
    public class ConnectionSL
    {
        private static readonly object slFileLock = new object();

        /// <summary>
        /// Establece una conexión con el Service Layer.
        /// </summary>
        /// <param name="User">Usuario autenticado.</param>
        /// <returns>Instancia de ServiceLayer para la conexión.</returns>
        private static ServiceLayer connectionSL(ClaimsPrincipal User)
        {
            var Uri = User.Claims.FirstOrDefault(c => c.Type == "U");

            List<Company>? _companies = CompanyController.GetDbCompanies().response;
            RequestSL connection = (from company in _companies
                                    where company.nameCompany == Convert.ToString(Uri != null ? Uri.Value : string.Empty)
                                    select new RequestSL
                                    {
                                        ServerSL = company.serverSlCompany,
                                        PortSL = company.portSlCompany.ToString(),
                                        SapUser = company.sapUserCompany,
                                        SapPass = company.sapPassUserCompany,
                                        CompanyDB = company.dataBaseNameCompany
                                    }).SingleOrDefault()!;
                      
            return new ServiceLayer(Convert.ToInt32(connection.PortSL),
                true,
                connection.ServerSL != null ? connection.ServerSL : string.Empty,
                connection.SapUser != null ? connection.SapUser : string.Empty,
                connection.SapPass != null ? connection.SapPass : string.Empty,
                connection.CompanyDB != null ? connection.CompanyDB : string.Empty,
                VersionSL.v1);
        }


        /// <summary>
        /// Establece una conexión con el Service Layer utilizando los datos de la solicitud.
        /// </summary>
        /// <param name="requestSL">Solicitud de conexión con el Service Layer.</param>
        /// <returns>Instancia de ServiceLayer para la conexión.</returns>
        private static ServiceLayer connectionSL(RequestSL requestSL)
        {
            try
            {
                    return new ServiceLayer(Convert.ToInt32(requestSL.PortSL != null ? requestSL.PortSL : string.Empty),
                true,
                Convert.ToString(requestSL.ServerSL != null ? requestSL.ServerSL : string.Empty),
                Convert.ToString(requestSL.SapUser != null ? requestSL.SapUser : string.Empty),
                Convert.ToString(requestSL.SapPass != null ? requestSL.SapPass : string.Empty),
                Convert.ToString(requestSL.CompanyDB != null ? requestSL.CompanyDB : string.Empty),
                VersionSL.v1);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error Método connectionSL " + ex.Message);
            }
        }

        public static ResponseSL<Branch> GetBranches(RequestSL requestSL, Entity entity)
        {
            ServiceLayer oService = connectionSL(requestSL);
            ResponseSL<Branch> getAll = new ResponseSL<Branch>();
            try
            {
                oService.Refresh();
                getAll = oService.GetAll(entity)
                        .Exec<Branch>();
            }
            catch (Exception)
            {
                //oService.Logout();
                return getAll;
            }
            finally
            {
                //oService.Logout();
            }
            return getAll;
        }

        public static ResponseSL<dynamic> GetById(ClaimsPrincipal User, string reference, object key)
        {
            ServiceLayer oService = connectionSL(User);
            ResponseSL<dynamic> getAll = new ResponseSL<dynamic>();
            try
            {
                oService.Refresh();
                getAll = oService.GetByKey(reference, key)
                        .Exec<dynamic>();
            }
            catch (Exception)
            {
                oService.Logout();
                return getAll;
            }
            finally
            {
                oService.Logout();
            }
            return getAll;
        }

        public static ResponseSL PostSL(RequestSL requestSL, string reference, object body)
        {
            ServiceLayer oService;
            lock (slFileLock)
            {
                oService = connectionSL(requestSL);
            }
            ResponseSL oPostSL = new ResponseSL();
            try
            {
                string json = JsonConvert.SerializeObject(body);
                oService.Refresh();
                oPostSL = oService.Post(reference, json)
                        .NoReturnContent(true)
                        .ExecSL();

            }
            catch (Exception)
            {
                oService.Logout();
                throw;
            }
            finally
            {
                oService.Logout();
            }
            return oPostSL;
        }

        public static ResponseSL PostSL(RequestSL requestSL, Entity entity, object body)
        {
            ServiceLayer oService = connectionSL(requestSL);
            ResponseSL oPostSL = new ResponseSL();
            try
            {
                string json = JsonConvert.SerializeObject(body);
                oService.Refresh();
                oPostSL = oService.Post(entity, json)
                        .NoReturnContent(true)
                        .ExecSL();

            }
            catch (Exception)
            {
                oService.Logout();
                throw;
            }
            finally
            {
                oService.Logout();
            }
            return oPostSL;
        }

        public static ResponseSL PostSL(ClaimsPrincipal User, Entity entity, object body)
        {
            ServiceLayer oService = connectionSL(User);
            ResponseSL oPostSL = new ResponseSL();
            string json = string.Empty;
            try
            {
                if (entity == Entity.UserFieldsMD || entity == Entity.UserTablesMD)
                {
                    json = body.ToString();
                }
                else
                {
                    json = JsonConvert.SerializeObject(body);
                }
                oService.Refresh();
                oPostSL = oService.Post(entity, json)
                        .NoReturnContent(true)
                        .ExecSL();

            }
            catch (Exception)
            {
                oService.Logout();
                throw;
            }
            finally
            {
                oService.Logout();
            }
            return oPostSL;
        }

        public static ResponseSL PostSL(ClaimsPrincipal User, string reference, object body)
        {
            ServiceLayer oService = connectionSL(User);
            ResponseSL oPostSL = new ResponseSL();
            try
            {
                string json = JsonConvert.SerializeObject(body);
                oService.Refresh();
                oPostSL = oService.Post(reference, json)
                        .NoReturnContent(true)
                        .ExecSL();

            }
            catch (Exception)
            {
                oService.Logout();
                throw;
            }
            finally
            {
                oService.Logout();
            }
            return oPostSL;
        }

        public static ResponseSL PostCancel(ClaimsPrincipal User, Entity entity, int key)
        {
            ServiceLayer oService = connectionSL(User);
            ResponseSL oPostSL = new ResponseSL();
            try
            {
                oService.Refresh();
                oPostSL = oService.Post(entity, key, Operation.Cancel)
                        .ExecSL();

            }
            catch (Exception)
            {
                oService.Logout();
                throw;
            }
            finally
            {
                oService.Logout();
            }
            return oPostSL;
        }

        public static ResponseSL PatchSL(RequestSL requestSL, Entity entity, object key, object body)
        {
            ServiceLayer oService = connectionSL(requestSL);
            ResponseSL oPatchSL = new ResponseSL();
            try
            {
                string json = JsonConvert.SerializeObject(body);
                oService.Refresh();
                oPatchSL = oService.Patch(entity, key, json)
                        .ExecSL();

            }
            catch (Exception)
            {
                oService.Logout();
                throw;
            }
            finally
            {
                oService.Logout();
            }
            return oPatchSL;
        }

        public static ResponseSL PatchSL(ClaimsPrincipal User, Entity entity, object key, object body)
        {
            ServiceLayer oService = connectionSL(User);
            ResponseSL oPatchSL = new ResponseSL();
            string json = string.Empty;
            try
            {
                if (entity == Entity.UserFieldsMD || entity  == Entity.UserTablesMD)
                {
                    json = body.ToString();
                }
                else
                {
                    json = JsonConvert.SerializeObject(body);                    
                }
                
                oService.Refresh();
                oPatchSL = oService.Patch(entity, key, json)
                        .ExecSL();

            }
            catch (Exception)
            {
                oService.Logout();
                throw;
            }
            finally
            {
                oService.Logout();
            }
            return oPatchSL;
        }

        public static ResponseSL PatchSL(ClaimsPrincipal User, string reference, object key, object body)
        {
            ServiceLayer oService = connectionSL(User);
            ResponseSL oPatchSL = new ResponseSL();
            try
            {
                string json = JsonConvert.SerializeObject(body);
                oService.Refresh();
                oPatchSL = oService.Patch(reference, key, json)
                        .ExecSL();

            }
            catch (Exception)
            {
                oService.Logout();
                throw;
            }
            finally
            {
                oService.Logout();
            }
            return oPatchSL;
        }

        public static ResponseSL PutSL(ClaimsPrincipal User, string reference, object key, object body)
        {
            ServiceLayer oService = connectionSL(User);
            ResponseSL oPutSL = new ResponseSL();
            try
            {
                string json = JsonConvert.SerializeObject(body);
                oService.Refresh();
                oPutSL = oService.Put(reference, key, json)
                        .ExecSL();

            }
            catch (Exception)
            {
                oService.Logout();
                throw;
            }
            finally
            {
                oService.Logout();
            }
            return oPutSL;
        }
    }
}