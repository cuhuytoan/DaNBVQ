using HNM.DataNC.ModelsNC.ModelsUtility;
using System.Collections.Generic;

namespace HNM.DataNC.ModelsNC.ModelsDTO
{
    public class ImageUploadDTO
    {   
        public string FileName { get; set; }
        public string PathSave { get; set; }
        public string ExtensionType { get; set; }
        public string Base64 { get; set; }        
        public int FileSize { get; set; }
        public string ChatType { get; set; }
    }
    public class ImageUploadAvatarDTO : ModelBase
    {
        public string FileName { get; set; }
        public string PathSave { get; set; }
        public string ExtensionType { get; set; }
        public string Base64 { get; set; }
        public ImageUploadDTO MainImage { get; set; }
    }
    public class DeleteImageProductPicture
    {

        /// <summary>
        /// ProductPicture ID for delete
        /// </summary>
        public string ProductPicture_ID { get; set; }
    }
    public class ImageUploadAWSDTO
    {
        public ImageUploadDTO MainImage { get; set; }
        public List<ImageUploadDTO> SubImage { get; set; }
        public int ParameterId { get; set; }
        public string Type { get; set; }
    }
    public class UrlReturnUploadDTO :ModelBase
    {
        public string UrlFile { get; set; }
        public string FileName { get; set; }
        public int FileSize { get; set; }
        public string ChatType { get; set; }
    }
}
