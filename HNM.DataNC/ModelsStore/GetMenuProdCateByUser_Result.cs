using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HNM.DataNC.ModelsStore
{
    public class GetMenuProdCateByUser_Result
    {
        [Key]
        public int ProductCategoryId { get; set; }
        public string Name { get; set; }
        public int? ProductCount { get; set; } = 0;
        public string Icon { get; set; }
    }
}
