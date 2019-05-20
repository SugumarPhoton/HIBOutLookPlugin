using System;
using System.Collections.Generic;
using HIB.Outlook.BAL.Repository.Interfaces;
using HIB.Outlook.DAL;
using System.Linq;
using HIB.Outlook.Model;
using log4net;
using System.Configuration;

namespace HIB.Outlook.BAL.Repository
{
    public class PolicyLineTypeRepository : IPolicyLineTypeRepository
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(PolicyLineTypeRepository));
        private readonly Int32 commandTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["CommandTimeout"]);

        ///<summary>
        ///Get list of policy line type to sync local database
        /// </summary>
        /// <param name="lastSyncDate"></param>
        /// <returns></returns>
        public List<PolicyTypeInfo> GetPolicyLineTypes(string userId, DateTime? lastSyncDate, string ipAddress)
        {
            var policyLineTypes = new List<PolicyTypeInfo>();
            try
            {
                using (var context = new HIBOutlookEntities())
                {
                    context.Database.CommandTimeout = commandTimeout;
                    var result = context.HIBOPGetPolicyLineType_SP(userId, lastSyncDate,ipAddress).ToList();
                    policyLineTypes.AddRange(result.Select(p => new PolicyTypeInfo { PolicyLineTypeId = p.UniqCdPolicyLineType, PolicyLineTypeCode = p.CdPolicyLineTypeCode, PolicyLineTypeDesc = p.PolicyLineTypeDesc, InsertedDate = p.InsertedDate, UpdatedDate = p.UpdatedDate }));
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return policyLineTypes;
        }
    }
}
