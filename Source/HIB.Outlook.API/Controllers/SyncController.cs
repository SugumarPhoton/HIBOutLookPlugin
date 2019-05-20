using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HIB.Outlook.BAL.Managers;
using HIB.Outlook.BAL.Managers.Interfaces;
using HIB.Outlook.Model;
using System.Collections.Generic;
using HIB.Outlook.Model.Activities;
using HIB.Outlook.Epic.Helper;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;
using HIB.Outlook.API.Helper;
using System.Linq;
using log4net;

namespace HIB.Outlook.API.Controllers
{
    [RoutePrefix("api/sync")]
    public class SyncController : ApiController
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(SyncController));

        #region Fields

        IActivities _activities;
        IClients _clients;
        IPolicyLineTypes _policyLineTypes;
        IFolders _folders;
        ILogs _logs;
        IFavourites _favourites;

        #endregion

        #region Constructor
        //, 
        public SyncController(IActivities activities, IClients clients, IPolicyLineTypes policyLineTypes, IFolders folders, ILogs logs, IFavourites favourites)
        {
            _activities = activities;
            _clients = clients;
            _policyLineTypes = policyLineTypes;
            _folders = folders;
            _logs = logs;
            _favourites = favourites;
        }
        #endregion

        #region Public Members

        /// <summary>
        /// Get List of user client detail to sync local Database
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="lastSyncDate"></param>
        /// <returns></returns>

        [Route("SyncClients"), HttpPost]
        public HttpResponseMessage SyncClients(SyncParams syncParams)
        {
            if (!string.IsNullOrEmpty(syncParams.UserId))
                GlobalContext.Properties["logged_by"] = syncParams.UserId;

            var clients = _clients.SyncClients(syncParams.UserId, syncParams.LastSyncDate, syncParams.IPAddress, syncParams.RowsPerPage, syncParams.PageNumber);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, clients);
            return response;
        }
        /// <summary>
        /// Get list of activity detail to sync local database
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="lastSyncDate"></param>
        /// <returns></returns>

        [Route("SyncActivities"), HttpPost]
        public HttpResponseMessage SyncActivities(SyncParams syncParams)
        {
            // var activities = _activities.SyncActivities(syncParams.UserId, syncParams.LastSyncDate);
            var activities = _activities.SyncActivities(syncParams.UserId, syncParams.LastSyncDate, syncParams.IPAddress, syncParams.RowsPerPage, syncParams.PageNumber);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, activities);
            return response;
        }

        /// <summary>
        /// Get list of activity detail to sync local database
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="lastSyncDate"></param>
        /// <returns></returns>

        [Route("SyncActivityEmployees"), HttpPost]
        public HttpResponseMessage SyncActivityEmployees(SyncParams syncParams)
        {
            // var activities = _activities.SyncActivities(syncParams.UserId, syncParams.LastSyncDate);
            var activities = _activities.SyncActivityEmployees(syncParams.UserId, syncParams.LastSyncDate, syncParams.IPAddress, syncParams.RowsPerPage, syncParams.PageNumber);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, activities);
            return response;
        }


        ///<summary>
        /// Get list of folder to sync local database
        /// </summary>
        /// <param name="lastSyncDate"></param>
        /// <returns></returns>

        [Route("SyncFolders"), HttpPost]
        public HttpResponseMessage SyncFolders(SyncParams syncParams)
        {
            var folders = _folders.SyncFolders(syncParams.UserId, syncParams.LastSyncDate, syncParams.IPAddress);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, folders);
            return response;
        }

        ///<summary>
        ///Get list of policy line type to sync local database
        /// </summary>
        /// <param name="lastSyncDate"></param>
        /// <returns></returns>
        [Route("GetPolicyLineTypes"), HttpPost]
        public HttpResponseMessage GetPolicyLineTypes(SyncParams syncParams)
        {
            var policyLineTypes = _policyLineTypes.GetPolicyLineTypes(syncParams.UserId, syncParams.LastSyncDate, syncParams.IPAddress);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, policyLineTypes);
            return response;
        }

        ///<summary>
        /// Save Audit Log Details from local to Sync centralized database
        /// </summary>
        /// <param name="logInfo"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveAuditLogDetails")]
        public HttpResponseMessage SaveAuditLogDetails(List<LogInfo> logInfo)
        {
            var auditLog = _logs.SaveAuditLogDetails(logInfo);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, auditLog);
            return response;
        }
        ///<summary>
        ///Save error Log Details from local to Sync centralized database
        /// </summary>
        /// <param name="errorlogInfo"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveErrorLogDetails")]
        public HttpResponseMessage SaveErrorLogDetails(List<ErrorLogInfo> errorlogInfo)
        {
            var errorLog = _logs.SaveErrorLogDetails(errorlogInfo);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, errorLog);
            return response;
        }

        /// <summary>
        /// Get list of activity Claim detail to sync local database
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="lastSyncDate"></param>
        /// <returns></returns>
        [Route("SyncActivityClaims"), HttpPost]
        public HttpResponseMessage SyncActivityClaims(SyncParams syncParams)
        {
            var activityClaims = _activities.SyncActivityClaims(syncParams.UserId, syncParams.LastSyncDate, syncParams.IPAddress);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, activityClaims);
            return response;
        }


        /// <summary>
        /// Get list of activity policy detail to sync local database
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="lastSyncDate"></param>
        /// <returns></returns>
        [Route("SyncActivityPolicies"), HttpPost]
        public HttpResponseMessage SyncActivityPolicies(SyncParams syncParams)
        {
            var activityPolicy = _activities.SyncActivityPolicies(syncParams.UserId, syncParams.LastSyncDate, syncParams.IPAddress, syncParams.RowsPerPage, syncParams.PageNumber);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, activityPolicy);
            return response;
        }


        /// <summary>
        /// Get list of activity service detail to sync local database
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="lastSyncDate"></param>
        /// <returns></returns>
        [Route("SyncActivityServices"), HttpPost]
        public HttpResponseMessage SyncActivityServices(SyncParams syncParams)
        {
            var activityService = _activities.SyncActivityServices(syncParams.UserId, syncParams.LastSyncDate, syncParams.IPAddress);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, activityService);
            return response;
        }
        /// <summary>
        /// Get list of activity line detail to sync local database
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="lastSyncDate"></param>
        /// <returns></returns>
        [Route("SyncActivityLines"), HttpPost]
        public HttpResponseMessage SyncActivityLines(SyncParams syncParams)
        {
            var activityLine = _activities.SyncActivityLines(syncParams.UserId, syncParams.LastSyncDate, syncParams.IPAddress, syncParams.RowsPerPage, syncParams.PageNumber);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, activityLine);
            return response;
        }

        /// <summary>
        /// Get list of activity Opportunities detail to sync local database
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="lastSyncDate"></param>
        /// <returns></returns>
        [Route("SyncActivityOpportunities"), HttpPost]
        public HttpResponseMessage SyncActivityOpportunities(SyncParams syncParams)
        {
            var activityOpportunity = _activities.SyncActivityOpportunities(syncParams.UserId, syncParams.LastSyncDate, syncParams.IPAddress);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, activityOpportunity);
            return response;
        }
        /// <summary>
        /// Get list of activity account detail to sync local database
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="lastSyncDate"></param>
        /// <returns></returns>
        [Route("SyncActivityAccounts"), HttpPost]
        public HttpResponseMessage SyncActivityAccounts(SyncParams syncParams)
        {
            var activityAccount = _activities.SyncActivityAccounts(syncParams.UserId, syncParams.LastSyncDate, syncParams.IPAddress, syncParams.RowsPerPage, syncParams.PageNumber);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, activityAccount);
            return response;
        }

        /// <summary>
        /// Get list of activity marketing detail to sync local database
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="lastSyncDate"></param>
        /// <returns></returns>

        [Route("SyncActivityMarketing"), HttpPost]
        public HttpResponseMessage SyncActivityMarketing(SyncParams syncParams)
        {
            var activityMarketing = _activities.SyncActivityMarketing(syncParams.UserId, syncParams.LastSyncDate, syncParams.IPAddress);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, activityMarketing);
            return response;
        }

        /// <summary>
        /// Get Sync Interval Count From Database
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="lastSyncDate"></param>
        /// <returns></returns>
        [Route("SyncIntervalTime"), HttpPost]
        public HttpResponseMessage SyncIntervalTime()
        {
            var activityIntervalTime = _clients.SyncIntervalTime();
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, activityIntervalTime);
            return response;
        }

        /// <summary>
        /// Get Sync Interval Count From Database
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="lastSyncDate"></param>
        /// <returns></returns>
        [Route("GetDeltaSyncObjectDetail"), HttpPost]
        public HttpResponseMessage GetDeltaSyncObjectDetail(SyncParams syncParams)
        {
            var deltaSyncObjectDetail = _clients.GetDeltaSyncObjectDetail(syncParams.UserId, syncParams.IPAddress, syncParams.IsClient, syncParams.isFirstSync);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, deltaSyncObjectDetail);
            return response;
        }


        /// <summary>
        /// Get list of activity client contact detail to sync local database
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="lastSyncDate"></param>
        /// <returns></returns>

        [Route("SyncActivityClientContacts"), HttpPost]
        public HttpResponseMessage SyncActivityClientContacts(SyncParams syncParams)
        {
            var activityClientContact = _activities.SyncActivityClientContacts(syncParams.UserId, syncParams.LastSyncDate, syncParams.IPAddress);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, activityClientContact);
            return response;
        }

        /// <summary>
        /// Get list of activity common look up detail to sync local database
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="lastSyncDate"></param>
        /// <returns></returns>

        [Route("SyncActivityCommonLookUp"), HttpPost]
        public HttpResponseMessage SyncActivityCommonLookUp(SyncParams syncParams)
        {
            var activityCommmonLookUp = _activities.SyncActivityCommonLookUp(syncParams.UserId, syncParams.LastSyncDate, syncParams.IPAddress);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, activityCommmonLookUp);
            return response;
        }

        /// <summary>
        /// Get list of activity Owner detail to sync local database
        /// </summary>
        /// <param name = "userId" ></ param >
        /// < param name="lastSyncDate"></param>
        /// <returns></returns>
        [Route("SyncActivityOwnerList"), HttpPost]
        public HttpResponseMessage SyncActivityOwnerList(SyncParams syncParams)
        {
            var activityOwnerList = _activities.SyncActivityOwnerList(syncParams.UserId, syncParams.LastSyncDate, syncParams.IPAddress, syncParams.RowsPerPage, syncParams.PageNumber);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, activityOwnerList);
            return response;
        }

        /// <summary>
        /// Get list of activity list detail to sync local database
        /// </summary>
        /// <param name = "userId" ></ param >
        /// < param name="lastSyncDate"></param>
        /// <returns></returns>
        [Route("SyncActivityList"), HttpPost]
        public HttpResponseMessage SyncActivityList(SyncParams syncParams)
        {
            var activityList = _activities.SyncActivityList(syncParams.UserId, syncParams.LastSyncDate, syncParams.IPAddress);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, activityList);
            return response;
        }


        ///<summary>
        /// Save Favourite Details from local to Sync centralized database
        /// </summary>
        /// <param name="logInfo"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveFavouriteDetails")]
        public HttpResponseMessage SaveFavouriteDetails(List<FavouriteInfo> favouriteInfo)
        {
            var favourites = _favourites.SaveFavouriteDetails(favouriteInfo);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, favourites);
            return response;
        }


        ///<summary>
        /// Save Favourite Details from local to Sync centralized database
        /// </summary>
        /// <param name="logInfo"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SyncActivityBills")]
        public HttpResponseMessage SyncActivityBills(SyncParams syncParams)
        {
            var activityBill = _activities.SyncActivityBills(syncParams.UserId, syncParams.LastSyncDate, syncParams.IPAddress);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, activityBill);
            return response;

        }

        /// <summary>
        /// Get list of activity carrier submission detail to sync local database
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="lastSyncDate"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SyncActivityCarriers")]
        public HttpResponseMessage SyncActivityCarriers(SyncParams syncParams)
        {
            var activityCarrier = _activities.SyncActivityCarriers(syncParams.UserId, syncParams.LastSyncDate, syncParams.IPAddress);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, activityCarrier);
            return response;
        }

        /// <summary>
        /// Get list of activity transaction detail to sync local database
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="lastSyncDate"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SyncActivityTransactions")]
        public HttpResponseMessage SyncActivityTransactions(SyncParams syncParams)
        {
            var activityTransaction = _activities.SyncActivityTransactions(syncParams.UserId, syncParams.LastSyncDate, syncParams.IPAddress);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, activityTransaction);
            return response;
        }


        /// <summary>
        /// Get list of activity certificate detail to sync local database
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="lastSyncDate"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SyncActivityCertificate")]
        public HttpResponseMessage SyncActivityCertificate(SyncParams syncParams)
        {
            var activityCertificate = _activities.SyncActivityCertificate(syncParams.UserId, syncParams.LastSyncDate, syncParams.IPAddress);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, activityCertificate);
            return response;
        }

        /// <summary>
        /// Get list of activity evidence detail to sync local database
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="lastSyncDate"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SyncActivityEvidence")]
        public HttpResponseMessage SyncActivityEvidence(SyncParams syncParams)
        {
            var activityEvidence = _activities.SyncActivityEvidence(syncParams.UserId, syncParams.LastSyncDate, syncParams.IPAddress);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, activityEvidence);
            return response;
        }

        /// <summary>
        /// Get list of activity Look Up detail to sync local database
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="lastSyncDate"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SyncActivityLookUps")]
        public HttpResponseMessage SyncActivityLookUps(SyncParams syncParams)
        {
            var activityLookUp = _activities.SyncActivityLookUps(syncParams.UserId, syncParams.LastSyncDate);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, activityLookUp);
            return response;
        }



        /// <summary>
        /// Get list of activity employee agency detail to sync local database
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="lastSyncDate"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SyncActivityEmployeeAgencies")]
        public HttpResponseMessage SyncActivityEmployeeAgencies(SyncParams syncParams)
        {
            var activityEmployeeAgency = _activities.SyncActivityEmployeeAgencies(syncParams.UserId, syncParams.LastSyncDate, syncParams.IPAddress);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, activityEmployeeAgency);
            return response;
        }
        /// <summary>
        /// Get list of activity employee detail to sync local database
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="lastSyncDate"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SyncActivityEmployee")]
        public HttpResponseMessage SyncActivityEmployee(SyncParams syncParams)
        {
            //if(!string.IsNullOrEmpty(syncParams.UserId))
            //GlobalContext.Properties["logged_by"] = syncParams.UserId;

            var activityEmployee = _activities.SyncActivityEmployee(syncParams.UserId);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, activityEmployee);
            return response;
        }
        /// <summary>
        /// Get list of client employee detail to sync local database
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="lastSyncDate"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SyncClientEmployee")]
        public HttpResponseMessage SyncClientEmployee(SyncParams syncParams)
        {
            var clientEmployee = _clients.SyncClientEmployee(syncParams.UserId, syncParams.LastSyncDate, syncParams.IPAddress, syncParams.RowsPerPage, syncParams.PageNumber);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, clientEmployee);
            return response;
        }
        /// <summary>
        ///Push add activity from local to Epic
        /// </summary>
        /// <param name="activities"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("activity")]
        public HttpResponseMessage CreateActivity(AddActivity activities)
        {

            var helper = new SdkHelper();
            var resultInfo = helper.SaveActivities(activities, activities.AddActivityDisplayDescription);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, resultInfo);
            return response;
        }

        public void GetActivity()
        {
            var helper = new SdkHelper();
            helper.GetActivities();
        }

        public const string SAVE_ATTACHMENT_MODEL_KEY = "attachmentMetaData";

        /// <summary>
        /// SaveAttachments
        /// </summary>
        /// <returns>Task<IHttpActionResult></returns>
        [HttpPost, Route("attachments/upload")]
        public async Task<IHttpActionResult> SaveAttachments()
        {
            try
            {
                var tempPath = GetAttachmentPath(true);
                if (Request.Content.IsMimeMultipartContent())
                {
                    if (!Directory.Exists(tempPath))
                        Directory.CreateDirectory(tempPath);

                    var provider = new MultipartFormDataMemoryStreamProvider();
                    await Request.Content.ReadAsMultipartAsync(provider);

                    // Check if files are on the request.
                    if (!provider.FileStreams.Any())
                    {
                        return NotFound();
                    }
                    foreach (MultipartFormFile file in provider.FileStreams)
                    {
                        using (FileStream fileStream = File.Create(tempPath + "\\" + file.Name, (int)file.Stream.Length))
                        {   // Initialize the bytes array with the stream length and then fill it with data
                            byte[] bytesInStream = new byte[file.Stream.Length];
                            file.Stream.Read(bytesInStream, 0, bytesInStream.Length);
                            // Use write method to write to the file specified above
                            fileStream.Write(bytesInStream, 0, bytesInStream.Length);

                        }
                    }
                }
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return InternalServerError(ex);
            }
            return Ok();
        }

        /// <summary>
        /// SaveAttachments
        /// </summary>
        /// <returns>Task<IHttpActionResult></returns>   
        [HttpPost, Route("attachments/save")]
        public HttpResponseMessage SaveEpicAttachments(AttachmentInfo attachmentInfo)
        {
            var tempPath = GetAttachmentPath(false);
            var helper = new SdkHelper();
            var resultInfo = helper.SaveAttachmentInfo(attachmentInfo, tempPath);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, resultInfo);
            return response;
        }

        #endregion

        #region Private Members

        /// <summary>
        /// Get Attachment path
        /// </summary>
        /// <param name="isSave"></param>
        /// <returns></returns>
        private string GetAttachmentPath(bool isSave)
        {
            var path = string.Empty;
            try
            {
                var isOptMail = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableoptEmail"]);
                if (isOptMail)
                {
                    path = Convert.ToString(isSave ? ConfigurationManager.AppSettings["OptemailInPath"] : ConfigurationManager.AppSettings["OptemailOutPath"]);
                }
                else
                    path = Convert.ToString(ConfigurationManager.AppSettings["AttachmentPath"]);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return path;
        }

        #endregion
    }
}
