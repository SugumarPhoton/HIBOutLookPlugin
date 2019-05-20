
namespace HIB.Outlook.Model
{
    public class FileInfo
    {
        /// <summary>
        ///  UploadFileInfo
        /// </summary>
        /// <param name="FileName"> FileName </param>
        /// <param name="FileName"> NewFileName </param>
        /// <param name="FileExtension"> FileExtension </param>
        /// <param name="FileContentMemStream"> FileContentMemStream </param>
        /// 
        public string FileName { get; set; }
        public string NewFileName { get; set; }
        public string FileExtension { get; set; }
        public byte[] FileContentMemStream { get; set; }
        public string FilePath { get; set; }
    }
}
