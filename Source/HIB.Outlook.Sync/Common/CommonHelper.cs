using HIB.Outlook.Helper.Common;
using HIB.Outlook.Helper.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace HIB.Outlook.Sync.Common
{
    public class CommonHelper
    {

        ///<summary>
        ///Get lookup code from directory
        /// </summary>
        /// <returns>LookUpCode</returns>
        public static string GetLookUpCode()
        {
            string pagerId = string.Empty;

            try
            {
                var username = Environment.UserName;
                if (username.Equals("PHOTON1", StringComparison.OrdinalIgnoreCase) || username.Equals(@"HG\PHOTON1", StringComparison.OrdinalIgnoreCase))
                {
                    pagerId = Convert.ToString(ConfigurationManager.AppSettings["User1"]);
                }
                else if (username.Equals("PHOTON2", StringComparison.OrdinalIgnoreCase) || username.Equals(@"HG\PHOTON2", StringComparison.OrdinalIgnoreCase))
                {
                    pagerId = Convert.ToString(ConfigurationManager.AppSettings["User2"]);
                }
                else if (username.Equals("PHOTONADMIN", StringComparison.OrdinalIgnoreCase) || username.Equals(@"HP_NT\PHOTONADMIN", StringComparison.OrdinalIgnoreCase))
                {
                    pagerId = Convert.ToString(ConfigurationManager.AppSettings["User3"]);
                }
                else if (username.Equals("PHOTON1", StringComparison.OrdinalIgnoreCase) || username.Equals(@"HP_NT\PHOTON1", StringComparison.OrdinalIgnoreCase))
                {
                    pagerId = Convert.ToString(ConfigurationManager.AppSettings["User4"]);
                }
                else if (username.Equals("PHOTON2", StringComparison.OrdinalIgnoreCase) || username.Equals(@"HP_NT\PHOTON2", StringComparison.OrdinalIgnoreCase))
                {
                    pagerId = Convert.ToString(ConfigurationManager.AppSettings["User5"]);
                }
                else if (username.Equals("PHOTON3", StringComparison.OrdinalIgnoreCase) || username.Equals(@"HP_NT\PHOTON3", StringComparison.OrdinalIgnoreCase))
                {
                    pagerId = Convert.ToString(ConfigurationManager.AppSettings["User6"]);
                }
                else if (username.Equals("PHOTON4", StringComparison.OrdinalIgnoreCase) || username.Equals(@"HP_NT\PHOTON4", StringComparison.OrdinalIgnoreCase))
                {
                    pagerId = Convert.ToString(ConfigurationManager.AppSettings["User7"]);
                }
                else if (username.Equals("PHOTON5", StringComparison.OrdinalIgnoreCase) || username.Equals(@"HP_NT\PHOTON5", StringComparison.OrdinalIgnoreCase))
                {
                    pagerId = Convert.ToString(ConfigurationManager.AppSettings["User8"]);
                }
                else if (username.Equals("PHOTON6", StringComparison.OrdinalIgnoreCase) || username.Equals(@"HP_NT\PHOTON6", StringComparison.OrdinalIgnoreCase))
                {
                    pagerId = Convert.ToString(ConfigurationManager.AppSettings["User9"]);
                }
                else if (username.Equals("PHOTON7", StringComparison.OrdinalIgnoreCase) || username.Equals(@"HP_NT\PHOTON7", StringComparison.OrdinalIgnoreCase))
                {
                    pagerId = Convert.ToString(ConfigurationManager.AppSettings["User10"]);
                }
                else if (username.Equals("PHOTON8", StringComparison.OrdinalIgnoreCase) || username.Equals(@"HP_NT\PHOTON8", StringComparison.OrdinalIgnoreCase))
                {
                    pagerId = Convert.ToString(ConfigurationManager.AppSettings["User11"]);
                }
                else if (username.Equals("PHOTON9", StringComparison.OrdinalIgnoreCase) || username.Equals(@"HP_NT\PHOTON9", StringComparison.OrdinalIgnoreCase))
                {
                    pagerId = Convert.ToString(ConfigurationManager.AppSettings["User12"]);
                }
                else if (username.Equals("PHOTON10", StringComparison.OrdinalIgnoreCase) || username.Equals(@"HP_NT\PHOTON10", StringComparison.OrdinalIgnoreCase))
                {
                    pagerId = Convert.ToString(ConfigurationManager.AppSettings["User13"]);
                }
                else if (username.Equals("PHOTON11", StringComparison.OrdinalIgnoreCase) || username.Equals(@"HP_NT\PHOTON11", StringComparison.OrdinalIgnoreCase))
                {
                    pagerId = Convert.ToString(ConfigurationManager.AppSettings["User14"]);
                }
                else if (username.Equals("PHOTON12", StringComparison.OrdinalIgnoreCase) || username.Equals(@"HP_NT\PHOTON12", StringComparison.OrdinalIgnoreCase))
                {
                    pagerId = Convert.ToString(ConfigurationManager.AppSettings["User15"]);
                }
                else if (username.Equals("PHOTON13", StringComparison.OrdinalIgnoreCase) || username.Equals(@"HP_NT\PHOTON13", StringComparison.OrdinalIgnoreCase))
                {
                    pagerId = Convert.ToString(ConfigurationManager.AppSettings["User16"]);
                }
                else if (username.Equals("PHOTON14", StringComparison.OrdinalIgnoreCase) || username.Equals(@"HP_NT\PHOTON14", StringComparison.OrdinalIgnoreCase))
                {
                    pagerId = Convert.ToString(ConfigurationManager.AppSettings["User17"]);
                }
                else if (username.Equals("PHOTON15", StringComparison.OrdinalIgnoreCase) || username.Equals(@"HP_NT\PHOTON15", StringComparison.OrdinalIgnoreCase))
                {
                    pagerId = Convert.ToString(ConfigurationManager.AppSettings["User18"]);
                }
                else if (username.Equals("PHOTON16", StringComparison.OrdinalIgnoreCase) || username.Equals(@"HP_NT\PHOTON16", StringComparison.OrdinalIgnoreCase))
                {
                    pagerId = Convert.ToString(ConfigurationManager.AppSettings["User19"]);
                }
                else if (username.Equals(@"PHOTONINT\raghunath_s", StringComparison.OrdinalIgnoreCase) || username.Equals("raghunath_s", StringComparison.OrdinalIgnoreCase))
                {
                    pagerId = Convert.ToString(ConfigurationManager.AppSettings["User20"]);
                }
                else if (username.Equals(@"PHOTONINT\harihara_pa", StringComparison.OrdinalIgnoreCase) || username.Equals("harihara_pa", StringComparison.OrdinalIgnoreCase))
                {
                    pagerId = Convert.ToString(ConfigurationManager.AppSettings["User21"]);
                }
                else if (username.Equals(@"PHOTONINT\ramasamy_l", StringComparison.OrdinalIgnoreCase) || username.Equals("ramasamy_r", StringComparison.OrdinalIgnoreCase))
                {
                    pagerId = Convert.ToString(ConfigurationManager.AppSettings["User22"]);
                }
                else if (username.Equals(@"PHOTONINT\bharathiraja_a", StringComparison.OrdinalIgnoreCase) || username.Equals("bharathiraja_a", StringComparison.OrdinalIgnoreCase))
                {
                    pagerId = Convert.ToString(ConfigurationManager.AppSettings["User23"]);
                }
                else if (username.Equals(@"PHOTONINT\namita_d", StringComparison.OrdinalIgnoreCase) || username.Equals("namita_d", StringComparison.OrdinalIgnoreCase))
                {
                    pagerId = Convert.ToString(ConfigurationManager.AppSettings["User24"]);
                }
                else if (username.Equals(@"PHOTONINT\vairavan_a", StringComparison.OrdinalIgnoreCase) || username.Equals("vairavan_a", StringComparison.OrdinalIgnoreCase))
                {
                    pagerId = Convert.ToString(ConfigurationManager.AppSettings["User25"]);
                }
                else if (username.Equals(@"PHOTONINT\neelakandan_s", StringComparison.OrdinalIgnoreCase) || username.Equals("neelakandan_s", StringComparison.OrdinalIgnoreCase))
                {
                    pagerId = Convert.ToString(ConfigurationManager.AppSettings["User26"]);
                }
                else if (username.Equals(@"PHOTONINT\narayan_g", StringComparison.OrdinalIgnoreCase) || username.Equals("narayan_g", StringComparison.OrdinalIgnoreCase))
                {
                    pagerId = Convert.ToString(ConfigurationManager.AppSettings["User27"]);
                }
                else if (username.Equals(@"PHOTONINT\selvamuthukku_s", StringComparison.OrdinalIgnoreCase) || username.Equals("selvamuthukku_s", StringComparison.OrdinalIgnoreCase))
                {
                    pagerId = Convert.ToString(ConfigurationManager.AppSettings["User28"]);
                }

                else if (username.Equals(@"PHOTONINT\bakiyaraj_r", StringComparison.OrdinalIgnoreCase) || username.Equals("bakiyaraj_r", StringComparison.OrdinalIgnoreCase))
                {
                    pagerId = Convert.ToString(ConfigurationManager.AppSettings["User23"]);
                }
                else if (username.Equals(@"PHOTONINT\gopinath_r", StringComparison.OrdinalIgnoreCase) || username.Equals("gopinath_r", StringComparison.OrdinalIgnoreCase))
                {
                    pagerId = Convert.ToString(ConfigurationManager.AppSettings["User24"]);
                }
                else if (username.Equals(@"PHOTONINT\manojkumarper_p", StringComparison.OrdinalIgnoreCase) || username.Equals("manojkumarper_p", StringComparison.OrdinalIgnoreCase))
                {
                    pagerId = Convert.ToString(ConfigurationManager.AppSettings["User25"]);
                }
                else if (username.Equals(@"PHOTONINT\swaminathan_s", StringComparison.OrdinalIgnoreCase) || username.Equals("swaminathan_s", StringComparison.OrdinalIgnoreCase))
                {
                    pagerId = Convert.ToString(ConfigurationManager.AppSettings["User26"]);
                }

                //else if (username.Equals("GrantL", StringComparison.OrdinalIgnoreCase) || username.Equals("sugumar_ma", StringComparison.OrdinalIgnoreCase))
                //{
                //    pagerId = Convert.ToString(ConfigurationManager.AppSettings["User4"]);
                //}
                //else if (username.Equals("nadiam", StringComparison.OrdinalIgnoreCase))
                //{
                //    pagerId = Convert.ToString(ConfigurationManager.AppSettings["User3"]);
                //}
                else
                {
                    UserPrincipal userDetail = UserPrincipal.Current;
                    using (var principalContext = new PrincipalContext(ContextType.Domain))
                    {
                        using (var user = UserPrincipal.FindByIdentity(principalContext, Environment.UserName))
                        {
                            var directoryEntry = user.GetUnderlyingObject() as DirectoryEntry;//if username
                            pagerId = directoryEntry.Properties["pager"]?.Value?.ToString();

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
            }
            finally
            {
                Logger.save();
            }

            return pagerId;
        }

        /// <summary>
        /// Get lookup code by user 
        /// </summary>
        /// <param name="userList"></param>
        /// <returns></returns>
        public static List<string> GetLookUpCodeByUser(List<string> userList)
        {
            List<string> pagerList = new List<string>();
            try
            {
                foreach (var userDomain in userList)
                {
                    string domainName = string.Empty;
                    var userDomainList = userDomain.Split('\\').ToArray();
                    if (userDomainList != null && userDomainList.Any())
                    {
                        domainName = userDomainList[0];
                        // Logger.InfoLog(string.Format("Domain is {0} and UserName is {1}", domainName, userDomain), typeof(SyncLocal), Logger.SourceType.WindowsService, userDomain);
                    }

                    string username = userDomain;
                    string pagerId = string.Empty;
                    if (username.Equals("PHOTON1", StringComparison.OrdinalIgnoreCase) || username.Equals(@"HG\PHOTON1", StringComparison.OrdinalIgnoreCase))
                    {
                        pagerId = Convert.ToString(ConfigurationManager.AppSettings["User1"]);
                    }
                    else if (username.Equals("PHOTON2", StringComparison.OrdinalIgnoreCase) || username.Equals(@"HG\PHOTON2", StringComparison.OrdinalIgnoreCase))
                    {
                        pagerId = Convert.ToString(ConfigurationManager.AppSettings["User2"]);
                    }
                    else if (username.Equals("PHOTONADMIN", StringComparison.OrdinalIgnoreCase) || username.Equals(@"HP_NT\PHOTONADMIN", StringComparison.OrdinalIgnoreCase))
                    {
                        pagerId = Convert.ToString(ConfigurationManager.AppSettings["User3"]);
                    }
                    else if (username.Equals("PHOTON1", StringComparison.OrdinalIgnoreCase) || username.Equals(@"HP_NT\PHOTON1", StringComparison.OrdinalIgnoreCase))
                    {
                        pagerId = Convert.ToString(ConfigurationManager.AppSettings["User4"]);
                    }
                    else if (username.Equals("PHOTON2", StringComparison.OrdinalIgnoreCase) || username.Equals(@"HP_NT\PHOTON2", StringComparison.OrdinalIgnoreCase))
                    {
                        pagerId = Convert.ToString(ConfigurationManager.AppSettings["User5"]);
                    }
                    else if (username.Equals("PHOTON3", StringComparison.OrdinalIgnoreCase) || username.Equals(@"HP_NT\PHOTON3", StringComparison.OrdinalIgnoreCase))
                    {
                        pagerId = Convert.ToString(ConfigurationManager.AppSettings["User6"]);
                    }
                    else if (username.Equals("PHOTON4", StringComparison.OrdinalIgnoreCase) || username.Equals(@"HP_NT\PHOTON4", StringComparison.OrdinalIgnoreCase))
                    {
                        pagerId = Convert.ToString(ConfigurationManager.AppSettings["User7"]);
                    }
                    else if (username.Equals("PHOTON5", StringComparison.OrdinalIgnoreCase) || username.Equals(@"HP_NT\PHOTON5", StringComparison.OrdinalIgnoreCase))
                    {
                        pagerId = Convert.ToString(ConfigurationManager.AppSettings["User8"]);
                    }
                    else if (username.Equals("PHOTON6", StringComparison.OrdinalIgnoreCase) || username.Equals(@"HP_NT\PHOTON6", StringComparison.OrdinalIgnoreCase))
                    {
                        pagerId = Convert.ToString(ConfigurationManager.AppSettings["User9"]);
                    }
                    else if (username.Equals("PHOTON7", StringComparison.OrdinalIgnoreCase) || username.Equals(@"HP_NT\PHOTON7", StringComparison.OrdinalIgnoreCase))
                    {
                        pagerId = Convert.ToString(ConfigurationManager.AppSettings["User10"]);
                    }
                    else if (username.Equals("PHOTON8", StringComparison.OrdinalIgnoreCase) || username.Equals(@"HP_NT\PHOTON8", StringComparison.OrdinalIgnoreCase))
                    {
                        pagerId = Convert.ToString(ConfigurationManager.AppSettings["User11"]);
                    }
                    else if (username.Equals("PHOTON9", StringComparison.OrdinalIgnoreCase) || username.Equals(@"HP_NT\PHOTON9", StringComparison.OrdinalIgnoreCase))
                    {
                        pagerId = Convert.ToString(ConfigurationManager.AppSettings["User12"]);
                    }
                    else if (username.Equals("PHOTON10", StringComparison.OrdinalIgnoreCase) || username.Equals(@"HP_NT\PHOTON10", StringComparison.OrdinalIgnoreCase))
                    {
                        pagerId = Convert.ToString(ConfigurationManager.AppSettings["User13"]);
                    }
                    else if (username.Equals("PHOTON11", StringComparison.OrdinalIgnoreCase) || username.Equals(@"HP_NT\PHOTON11", StringComparison.OrdinalIgnoreCase))
                    {
                        pagerId = Convert.ToString(ConfigurationManager.AppSettings["User14"]);
                    }
                    else if (username.Equals("PHOTON12", StringComparison.OrdinalIgnoreCase) || username.Equals(@"HP_NT\PHOTON12", StringComparison.OrdinalIgnoreCase))
                    {
                        pagerId = Convert.ToString(ConfigurationManager.AppSettings["User15"]);
                    }
                    else if (username.Equals("PHOTON13", StringComparison.OrdinalIgnoreCase) || username.Equals(@"HP_NT\PHOTON13", StringComparison.OrdinalIgnoreCase))
                    {
                        pagerId = Convert.ToString(ConfigurationManager.AppSettings["User16"]);
                    }
                    else if (username.Equals("PHOTON14", StringComparison.OrdinalIgnoreCase) || username.Equals(@"HP_NT\PHOTON14", StringComparison.OrdinalIgnoreCase))
                    {
                        pagerId = Convert.ToString(ConfigurationManager.AppSettings["User17"]);
                    }
                    else if (username.Equals("PHOTON15", StringComparison.OrdinalIgnoreCase) || username.Equals(@"HP_NT\PHOTON15", StringComparison.OrdinalIgnoreCase))
                    {
                        pagerId = Convert.ToString(ConfigurationManager.AppSettings["User18"]);
                    }
                    else if (username.Equals("PHOTON16", StringComparison.OrdinalIgnoreCase) || username.Equals(@"HP_NT\PHOTON16", StringComparison.OrdinalIgnoreCase))
                    {
                        pagerId = Convert.ToString(ConfigurationManager.AppSettings["User19"]);
                    }
                    else if (username.Equals(@"PHOTONINT\raghunath_s", StringComparison.OrdinalIgnoreCase) || username.Equals("raghunath_s", StringComparison.OrdinalIgnoreCase))
                    {
                        pagerId = Convert.ToString(ConfigurationManager.AppSettings["User20"]);
                    }
                    else if (username.Equals(@"PHOTONINT\harihara_pa", StringComparison.OrdinalIgnoreCase) || username.Equals("harihara_pa", StringComparison.OrdinalIgnoreCase))
                    {
                        pagerId = Convert.ToString(ConfigurationManager.AppSettings["User21"]);
                    }
                    else if (username.Equals(@"PHOTONINT\ramasamy_l", StringComparison.OrdinalIgnoreCase) || username.Equals("ramasamy_r", StringComparison.OrdinalIgnoreCase))
                    {
                        pagerId = Convert.ToString(ConfigurationManager.AppSettings["User22"]);
                    }
                    else if (username.Equals(@"PHOTONINT\bharathiraja_a", StringComparison.OrdinalIgnoreCase) || username.Equals("bharathiraja_a", StringComparison.OrdinalIgnoreCase))
                    {
                        pagerId = Convert.ToString(ConfigurationManager.AppSettings["User23"]);
                    }
                    else if (username.Equals(@"PHOTONINT\namita_d", StringComparison.OrdinalIgnoreCase) || username.Equals("namita_d", StringComparison.OrdinalIgnoreCase))
                    {
                        pagerId = Convert.ToString(ConfigurationManager.AppSettings["User24"]);
                    }
                    else if (username.Equals(@"PHOTONINT\vairavan_a", StringComparison.OrdinalIgnoreCase) || username.Equals("vairavan_a", StringComparison.OrdinalIgnoreCase))
                    {
                        pagerId = Convert.ToString(ConfigurationManager.AppSettings["User25"]);
                    }
                    else if (username.Equals(@"PHOTONINT\neelakandan_s", StringComparison.OrdinalIgnoreCase) || username.Equals("neelakandan_s", StringComparison.OrdinalIgnoreCase))
                    {
                        pagerId = Convert.ToString(ConfigurationManager.AppSettings["User26"]);
                    }
                    else if (username.Equals(@"PHOTONINT\narayan_g", StringComparison.OrdinalIgnoreCase) || username.Equals("narayan_g", StringComparison.OrdinalIgnoreCase))
                    {
                        pagerId = Convert.ToString(ConfigurationManager.AppSettings["User27"]);
                    }
                    else if (username.Equals(@"PHOTONINT\selvamuthukku_s", StringComparison.OrdinalIgnoreCase) || username.Equals("selvamuthukku_s", StringComparison.OrdinalIgnoreCase))
                    {
                        pagerId = Convert.ToString(ConfigurationManager.AppSettings["User28"]);
                    }

                    else if (username.Equals(@"PHOTONINT\bakiyaraj_r", StringComparison.OrdinalIgnoreCase) || username.Equals("bakiyaraj_r", StringComparison.OrdinalIgnoreCase))
                    {
                        pagerId = Convert.ToString(ConfigurationManager.AppSettings["User23"]);
                    }
                    else if (username.Equals(@"PHOTONINT\gopinath_r", StringComparison.OrdinalIgnoreCase) || username.Equals("gopinath_r", StringComparison.OrdinalIgnoreCase))
                    {
                        pagerId = Convert.ToString(ConfigurationManager.AppSettings["User24"]);
                    }
                    else if (username.Equals(@"PHOTONINT\manojkumarper_p", StringComparison.OrdinalIgnoreCase) || username.Equals("manojkumarper_p", StringComparison.OrdinalIgnoreCase))
                    {
                        pagerId = Convert.ToString(ConfigurationManager.AppSettings["User25"]);
                    }
                    else if (username.Equals(@"PHOTONINT\swaminathan_s", StringComparison.OrdinalIgnoreCase) || username.Equals("swaminathan_s", StringComparison.OrdinalIgnoreCase))
                    {
                        pagerId = Convert.ToString(ConfigurationManager.AppSettings["User26"]);
                    }
                    else
                    {
                        using (var principalContext = new PrincipalContext(ContextType.Domain, domainName))
                        {
                            using (var user = UserPrincipal.FindByIdentity(principalContext, username))
                            {
                                if (user != null)
                                {
                                    var directoryEntry = user.GetUnderlyingObject() as DirectoryEntry;//if username
                                    pagerId = directoryEntry.Properties["pager"]?.Value?.ToString();
                                }
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(pagerId))
                        pagerList.Add(pagerId);
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
            }
            finally
            {
                Logger.save();
            }
            return pagerList;
        }

        public static string LocalIPAddress()
        {
            IPHostEntry host;
            string localIP = "";
            host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (IPAddress ip in host.AddressList)
            {
                localIP = ip.ToString();

                string[] temp = localIP.Split('.');

                if (ip.AddressFamily == AddressFamily.InterNetwork && temp[0] == "192")
                {
                    break;
                }
                else
                {
                    localIP = null;
                }
            }

            return localIP;
        }
        public static List<string> GetUserandDomains(string domains)
        {
            List<string> domainList = domains.Split(',').ToList<string>();
            List<string> UserandDomainList = new List<string>();
            string NamespacePath = "\\\\.\\ROOT\\cimv2";
            string ClassName = "Win32_LoggedOnUser";
            ManagementClass oClass = new ManagementClass(NamespacePath + ":" + ClassName);

            foreach (ManagementObject oObject in oClass.GetInstances())
            {
                var usernameDetail = oObject["Antecedent"];
                string userPath = Convert.ToString(usernameDetail);

                string domainLen = "Domain=";
                int indexDomainStart = userPath.IndexOf(domainLen) + domainLen.Length;
                int indexDomainLast = userPath.IndexOf(",Name");
                string domain = userPath.Substring(indexDomainStart, indexDomainLast - indexDomainStart).Replace('"', ' ').Trim();
                if (domainList.Contains(domain))
                {
                    int nameIndex = usernameDetail.ToString().LastIndexOf("Name=");
                    string user = usernameDetail.ToString().Substring(nameIndex).Replace('"', ' ').Remove(0, 5).Trim();
                    if (!string.IsNullOrEmpty(user))
                    {
                        string userDomain = user + "-" + domain;
                        if (!UserandDomainList.Contains(userDomain))
                        {
                            UserandDomainList.Add(userDomain);
                        }
                    }
                }
            }
            return UserandDomainList;
        }

    }
}
