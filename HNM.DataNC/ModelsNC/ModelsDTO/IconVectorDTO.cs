using HNM.DataNC.ModelsNC.ModelsUtility;
using System.Collections.Generic;

namespace HNM.DataNC.ModelsNC.ModelsDTO
{
    public class IconVectorDTO
    {
        public IconVectorDTO()
        {
            IconNormal = new List<IconVector>();
            IconActive = new List<IconVector>();
        }
        public IEnumerable<IconVector> IconNormal { get; set; }
        public IEnumerable<IconVector> IconActive { get; set; }


    }
    public class IconVector
    {
        public string FileName { get; set; }
        public string UrlFileName => Utils.CloudPath() + $"/DataMobile/iconvector/iconvector/{FileName}";
    }

}
