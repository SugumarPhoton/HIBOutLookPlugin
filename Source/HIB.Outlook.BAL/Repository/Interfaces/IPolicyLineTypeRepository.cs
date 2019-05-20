using HIB.Outlook.Model;
using System;
using System.Collections.Generic;


namespace HIB.Outlook.BAL.Repository.Interfaces
{
    public interface IPolicyLineTypeRepository
    {
        List<PolicyTypeInfo> GetPolicyLineTypes(string userId, DateTime? lastSyncDate, string ipAddress);
    }
}
