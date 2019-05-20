
using HIB.Outlook.Helper.Common;
using HIB.Outlook.Model.Activities;
using schemas.appliedsystems.com.epic.sdk._2009._07;
using schemas.appliedsystems.com.epic.sdk._2009._07._common;
using schemas.appliedsystems.com.epic.sdk._2011._01._get;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIB.Outlook.Helper.Helper
{
    public class EpicPushHelperForNewActivity
    {
        EpicSDK_2016_01Client EpicSDKService = new EpicSDK_2016_01Client();
        
        MessageHeader mHeader = new MessageHeader();

        private int GetDate(string date, int index)
        {
            int value = 0;
            if (!string.IsNullOrEmpty(date))
            {
                value = Convert.ToInt32(date.Split('-')[index]);
            }
            return value;
        }

        private int GetTime(string time, int index)
        {
            int value = 0;
            if (!string.IsNullOrEmpty(time))
            {
                value = Convert.ToInt32(time.Remove(5).Split(':')[index]);
            }
            return value;
        }



        public ResultInfo SaveActivities(AddActivity activityInfo)
        {
            var saveResult = new ResultInfo();
            try
            {
               

                mHeader.DatabaseName = Common.Common.DatabaseName;
                mHeader.AuthenticationKey = Common.Common.AuthenticationKey;
                var activity = new schemas.appliedsystems.com.epic.sdk._2011._01._common.Activity();
                activity.AccountID = Convert.ToInt32(activityInfo.ClientId);
                activity.AccountTypeCode = "CUST";

                if (activityInfo.AddtoType == "Account")
                {
                    activity.AssociatedToID = Convert.ToInt32(activityInfo.ClientId);
                    activity.AssociatedToType = activityInfo.AddtoType;
                    activity.AgencyCode = activityInfo.AgencyCode;
                    activity.BranchCode = activityInfo.BranchCode;
                }
                else
                {
                    activity.AssociatedToID = Convert.ToInt32(activityInfo.AddToTypeId);
                    activity.AssociatedToType = activityInfo.AddtoType;
                }
                DateTime startDateTime = DateTime.MinValue;
                DateTime endDateTime = DateTime.MinValue;
                DateTime reminderDateTime = DateTime.MinValue;

                try
                {
                    string date = activityInfo.StartDate;
                    string time = activityInfo.StartTime;
                    startDateTime = DateTime.Parse(date + " " + time);

                    string endDate = activityInfo.EndDate;
                    string endTime = activityInfo.EndTime;
                    endDateTime = DateTime.Parse(endDate + " " + endTime);

                    if (!string.IsNullOrEmpty(activityInfo.ReminderDate))
                    {
                        string reminderdate = activityInfo.ReminderDate;
                        string reminderTime = activityInfo.ReminderTime;
                        reminderDateTime = DateTime.Parse(reminderdate + " " + reminderTime);
                    }

                }
                catch (Exception ex)
                {
                    Logger.ErrorLog(ex, Logger.SourceType.WindowsService, activityInfo.OwnerCode);
                }

                activity.DetailValue = new schemas.appliedsystems.com.epic.sdk._2011._01._common._activity.Detail();

                activity.DetailValue.FollowUpStartDate = DateTime.Today;//FollowUpStartDate //to be discussed

            

                activity.DetailValue.ContactName = activityInfo.WhoToContactName;
                activity.DetailValue.ContactVia = activityInfo.ContactMode;
                activity.DetailValue.ContactNumberEmail = activityInfo.ContactDetail;

                activity.ActivityCode = activityInfo.AddActivityCode;
                activity.WhoOwnerCode = activityInfo.OwnerCode;
                activity.Priority = activityInfo.Priority;

                if (string.IsNullOrEmpty(activityInfo.EndDate))
                {
                    activityInfo.EndDate = activityInfo.StartDate;
                }

              
                activity.Tasks = new schemas.appliedsystems.com.epic.sdk._2011._01._common._activity.TaskItems
                {
                            new schemas.appliedsystems.com.epic.sdk._2011._01._common._activity.TaskItem {
                                Description = activityInfo.AddActivityDescription,
                                Owner = activityInfo.CurrentlyLoggedUserCode,
                                Status = "In Progress",
                                StartDate=Convert.ToDateTime(activityInfo.StartDate),
                                StartTime=startDateTime,
                                DueDate=Convert.ToDateTime(activityInfo.EndDate),
                                DueTime=endDateTime,   
                                            
                                
                                TaskNotes = new schemas.appliedsystems.com.epic.sdk._2011._01._common._activity._common1.NoteItems
                                {
                                    new schemas.appliedsystems.com.epic.sdk._2011._01._common._activity._common1.NoteItem
                                    {
                                        AccessLevel=activityInfo.AccessLevel,
                                        NoteText = activityInfo.Description,
                                    }
                                },
                            }
                 };

                if (activityInfo.IsPushToEpic == 0)
                    saveResult.Id = EpicSDKService.Insert_Activity(mHeader, activity);

                if (saveResult.Id != null)
                {
                    saveResult.IsSuccess = true;
                    saveResult.HasError = false;
                }
            }
            catch (Exception ex)
            {
                saveResult.HasError = true;
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, activityInfo.OwnerCode);
            }
            finally
            {
                Logger.save();
            }
            return saveResult;
        }
    }
}
