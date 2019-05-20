using HIB.Outlook.Model;
using System.Collections.Generic;


namespace HIB.Outlook.SQLite.Repository.IRepository
{
    public interface IClientRepository
    {
        ResultInfo SyncClient(List<ClientInfo> clientList);
        ResultInfo SyncClientEmployee(List<ClientEmployee> clientEmployeeList);
    }
}
