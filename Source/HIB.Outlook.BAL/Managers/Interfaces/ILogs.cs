using HIB.Outlook.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace HIB.Outlook.BAL.Managers.Interfaces
{
    public interface ILogs
    {

        int SaveAuditLogDetails(List<LogInfo> logInfo);
        int SaveErrorLogDetails(List<ErrorLogInfo> errorlogInfo);
    }
}
