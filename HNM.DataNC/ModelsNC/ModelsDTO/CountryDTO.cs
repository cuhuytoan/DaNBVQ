using HNM.DataNC.ModelsNC.ModelsUtility;
using System.Collections.Generic;

namespace HNM.DataNC.ModelsNC.ModelsDTO
{
    public class LstCountryDTO : ModelBase
    {
        public LstCountryDTO()
        {
            data = new List<CountryDTO>();
        }
        public List<CountryDTO> data { get; set; }

    }
    public class CountryDTO
    {
        public int CountryId { get; set; }
        public string Name { get; set; }
        public int? Sort { get; set; }
    }
}
