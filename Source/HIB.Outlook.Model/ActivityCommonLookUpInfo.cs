using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIB.Outlook.Model
{
    public class ActivityCommonLookUpInfo
    {
        #region Public Members
        public int CommonLkpId { get; set; }
        public string CommonLkpTypeCode { get; set; }
        public string CommonLkpCode { get; set; }
        public string CommonLkpName { get; set; }
        public string CommonLkpDescription { get; set; }
        public Nullable<int> SortOrder { get; set; }
        public bool IsDeleted { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime ModifiedDate { get; set; } 
        #endregion
    }
}
