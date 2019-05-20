
using System;

namespace HIB.Outlook.Model
{
    public class FolderInfo
    {
        #region Public Members
        /// <summary>
        /// FolderInfo
        /// </summary>

        /// <param name="ParentFolderId"> ParentFolderId </param>
        /// <param name="ParentFolderName"> ParentFolderName </param>
        /// <param name="FolderId"> FolderId </param>
        /// <param name="FolderName"> FolderName </param>
        /// <param name="SubFolderId"> SubFolderId </param>
        /// <param name="SubFolderName"> SubFolderName </param>
        /// <param name="FolderType"> FolderType </param>
        /// 


        public long? ParentFolderId { get; set; }
        public string ParentFolderName { get; set; }
        public long? FolderId { get; set; }
        public string FolderName { get; set; }
        public string FolderType { get; set; }
        public long? SubFolderId { get; set; }
        public string SubFolderName { get; set; }
        public System.DateTime InsertedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        #endregion
    }
}
