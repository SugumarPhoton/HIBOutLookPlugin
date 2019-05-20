using HIB.Outlook.Model;
using HIB.Outlook.BAL.Managers.Interfaces;
using HIB.Outlook.BAL.Repository;
using System.Collections.Generic;
using HIB.Outlook.BAL.Repository.Interfaces;

namespace HIB.Outlook.BAL.Managers
{
    public class Logs : ILogs
    {
        ILogRepository _logRepository;
        public Logs(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }
        ///<summary>
        /// Save Audit Log Details
        /// </summary>
        /// <param name="uniqId"></param>
        /// <returns></returns>   
        public int SaveAuditLogDetails(List<LogInfo> logInfo)
        {
            return _logRepository.SaveAuditLogDetails(logInfo);
        }
        ///<summary>
        /// Save Error Log Details
        /// </summary>
        /// <param name="uniqId"></param>
        /// <returns></returns>   
        public int SaveErrorLogDetails(List<ErrorLogInfo> errorLogInfo)
        {
            return _logRepository.SaveErrorLogDetails(errorLogInfo);
        }

    }
}
