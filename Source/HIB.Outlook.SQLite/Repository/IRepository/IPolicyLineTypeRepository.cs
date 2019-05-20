using HIB.Outlook.Model;
using System.Collections.Generic;


namespace HIB.Outlook.SQLite.Repository.IRepository
{
    public interface IPolicyLineTypeRepository
    {
        ResultInfo SyncDBPolicyLineType(List<PolicyTypeInfo> policyTypeList);
    }
}
