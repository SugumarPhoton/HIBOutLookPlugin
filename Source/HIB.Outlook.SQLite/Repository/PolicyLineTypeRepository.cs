using HIB.Outlook.Helper.Common;
using HIB.Outlook.Helper.Helper;
using HIB.Outlook.Model;
using HIB.Outlook.SQLite.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;



namespace HIB.Outlook.SQLite.Repository
{
    public class PolicyLineTypeRepository : IPolicyLineTypeRepository
    {
        #region Public Members
        /// <summary>
        /// Save Policy detail to local Database
        /// </summary>
        /// <param name="policyTypeInfo"></param>
        /// <returns></returns>  
        public ResultInfo SyncDBPolicyLineType(List<PolicyTypeInfo> policyTypeList)
        {
            var resultInfo = new ResultInfo();

            try
            {
                using (var cmd = SqliteHelper.Connection.CreateCommand())
                {
                    using (var transaction = SqliteHelper.Connection.BeginTransaction())
                    {
                        foreach (var p in policyTypeList)
                        {
                            cmd.CommandText = "INSERT OR REPLACE INTO HIBOPPolicyLineType(UniqCdPolicyLineType,CdPolicyLineTypeCode,PolicyLineTypeDesc,InsertedDate,UpdatedDate) VALUES ('" + p.PolicyLineTypeId + "', '" + p.PolicyLineTypeCode + "','" + p.PolicyLineTypeDesc.Replace("'", "''") + "','" + ExtensionClass.SqliteDateTimeFormat(p.InsertedDate) + "','" + ExtensionClass.SqliteDateTimeFormat(p.UpdatedDate) + "')";
                            cmd.ExecuteNonQuery();
                        }
                        transaction.Commit();
                        resultInfo.IsSuccess = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                resultInfo.IsSuccess = false;
                resultInfo.ErrorMessage = ex.Message;
            }
            finally
            {
                Logger.save();
            }

            return resultInfo;
        }


        #endregion
    }
}
