using HNM.DataNC.ModelsNC.ModelsUtility;
using System;
using System.Collections.Generic;

namespace HNM.DataNC.ModelsNC.ModelsDTO
{
    public class ResumeDTO
    {
        public Nullable<int> ResumeId { get; set; }
        public string FullName { get; set; }
        public Nullable<int> LocationId { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> LastEditDate { get; set; }
        public string LocationName { get; set; }
        public string Skill { get; set; }
    }
    public class TopResumeDTO : ModelBaseStatus
    {
        public TopResumeDTO()
        {
            Data = new List<ResumeDTO>();
        }
        public IEnumerable<ResumeDTO> Data { get; set; }
    }

    public class ResumeSearchResultDTO
    {
        public Nullable<int> ResumeId { get; set; }
        public string FullName { get; set; }
        public Nullable<int> LocationId { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> LastEditDate { get; set; }
        public string LocationName { get; set; }
        public string Skill { get; set; }
    }
    public class ResumePaggingDTO : ModelBaseStatus
    {
        public int TotalRecord { get; set; }
        public int TotalPage { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public IEnumerable<ResumeSearchResultDTO> Data { get; set; }
        public ResumePaggingDTO()
        {
            Data = new List<ResumeSearchResultDTO>();
        }

    }
}
   
