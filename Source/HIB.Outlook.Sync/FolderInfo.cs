using System;

namespace HIB.Outlook.Sync
{
    public class FolderInfo
    {
        #region Public Members

        public int FolderId { get; set; }
        public int ParentFolderId { get; set; }
        public string FolderName { get; set; }
        public string FolderType { get; set; }
        public System.DateTime InsertedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        #endregion
    }
}
