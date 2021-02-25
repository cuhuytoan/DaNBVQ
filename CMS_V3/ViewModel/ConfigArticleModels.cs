using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS_V3.ViewModel
{
    public class ConfigArticleModels
    {
    }

    public class CheckBoxArticle
    {
        public bool HightLight { get; set; }
        public bool Block1 { get; set; }
        public bool Block2 { get; set; }
        public bool BlockBehind { get; set; }
    }

    public class MetaSetting
    {
        public string MetaKeywordsDefault { get; set; }
        public string MetaDescriptionDefault { get; set; }
    }
}
