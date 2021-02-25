using System;
using System.ComponentModel.DataAnnotations;
namespace HNM.DataNC.ModelsStore
{
    public class PatchNumber_SearchByCategory_Result
    {
        [Key]
        public int NoItem { get; set; }
        public string PartNumber { get; set; }
        public Nullable<int> ProductCount { get; set; }
    }
}
