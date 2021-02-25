using HNM.DataNC.ModelsNC.ModelsUtility;
using System.Collections.Generic;

namespace HNM.DataNC.ModelsNC.ModelsDTO
{
    public class IntroScreenDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string IntroContent { get; set; }
        public string ImageBackground { get; set; }
        public int? Sort { get; set; }
    }
    public class ListIntroScreenDTO : ModelBaseStatus
    {
        public ListIntroScreenDTO()
        {
            Data = new List<IntroScreenDTO>();
        }
        public IEnumerable<IntroScreenDTO> Data { get; set; }
    }
}
