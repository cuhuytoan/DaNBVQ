using HNM.DataNC.ModelsNC.ModelsDTO;
using HNM.DataNC.ModelsStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS_V3.ViewModel
{
    public class LibraryViewModel
    {
        public LibraryViewModel()
        {
            lstLib1 = new List<Library_Search_By_Cate_Result>();
            lstLib2 = new List<Library_Search_By_Cate_Result>();
            lstLib3 = new List<Library_Search_By_Cate_Result>();
            CateName1 = new LibraryCategoryDTO();
            CateName2 = new LibraryCategoryDTO();
            CateName3 = new LibraryCategoryDTO();
        }
        public List<Library_Search_By_Cate_Result> lstLib1 { get; set;}
        public List<Library_Search_By_Cate_Result> lstLib2 { get; set;}
        public List<Library_Search_By_Cate_Result> lstLib3 { get; set;}
        public LibraryCategoryDTO CateName1 { get; set; }
        public LibraryCategoryDTO CateName2 { get; set; }
        public LibraryCategoryDTO CateName3 { get; set; }
    }
}
