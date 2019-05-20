using HIB.Outlook.Model;
using System;
using System.Collections.Generic;


namespace HIB.Outlook.BAL.Managers.Interfaces
{
    public interface IPolicyLineTypes
    {
        List<PolicyTypeInfo> GetPolicyLineTypes(string userId, DateTime? lastSyncDate, string ipAddress);
    }
}
