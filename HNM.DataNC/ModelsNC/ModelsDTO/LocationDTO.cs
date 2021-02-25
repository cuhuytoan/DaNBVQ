using HNM.DataNC.ModelsNC.ModelsUtility;
using System.Collections.Generic;

namespace HNM.DataNC.ModelsNC.ModelsDTO
{
    public class LocationDTO
    {
        public int LocationId { get; set; }
        public string Name { get; set; }
        public int? Sort { get; set; }
        public string PostalCode { get; set; }
    }
    public class ListLocationDTO : ModelBaseStatus
    {
        public ListLocationDTO()
        {
            Data = new List<LocationDTO>();
        }
        public IEnumerable<LocationDTO> Data { get; set; }
    }
    public class DistrictDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Prefix { get; set; }
        public int? ProvinceId { get; set; }
        public int? LocationId { get; set; }
    }
    public class ListDistrictDTO : ModelBaseStatus
    {
        public ListDistrictDTO()
        {
            Data = new List<DistrictDTO>();
        }
        public IEnumerable<DistrictDTO> Data { get; set; }
    }
}
