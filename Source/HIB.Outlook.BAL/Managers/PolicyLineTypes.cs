using HIB.Outlook.Model;
using HIB.Outlook.BAL.Managers.Interfaces;
using HIB.Outlook.BAL.Repository;
using System;
using System.Collections.Generic;
using HIB.Outlook.BAL.Repository.Interfaces;

namespace HIB.Outlook.BAL.Managers
{
    public class PolicyLineTypes : IPolicyLineTypes
    {
        IPolicyLineTypeRepository _policyLineTypeRepository;

        public PolicyLineTypes(IPolicyLineTypeRepository policyLineTypeRepository)
        {
            _policyLineTypeRepository = policyLineTypeRepository;
        }
        ///<summary>
        ///Get list of policy line type to sync local database
        /// </summary>
        /// <param name="lastSyncDate"></param>
        /// <returns></returns>
        public List<PolicyTypeInfo> GetPolicyLineTypes(string userId, DateTime? lastSyncDate,string ipAddress)
        {
            return _policyLineTypeRepository.GetPolicyLineTypes(userId, lastSyncDate, ipAddress);

        }
    }
}
