using HIB.Outlook.Model;
using System.Collections.Generic;


namespace HIB.Outlook.BAL.Repository.Interfaces
{
    public interface ILogRepository
    {

        int SaveAuditLogDetails(List<LogInfo> logInfo);
        int SaveErrorLogDetails(List<ErrorLogInfo> errorlogInfo);

    }
}
