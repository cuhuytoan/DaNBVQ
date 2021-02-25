using System;
using System.ComponentModel.DataAnnotations;

namespace HNM.DataNC.ModelsStore
{
    public class Resume_Search_Result
    {
        [Key]
        public Nullable<long> NoItem { get; set; }
        public Nullable<int> TotalRows { get; set; }
        public Nullable<int> Resume_ID { get; set; }
        public string FullName { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public Nullable<bool> Sex { get; set; }
        public string Address { get; set; }
        public Nullable<int> Location_ID { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Certificate { get; set; }
        public string Image { get; set; }
        public string IdentityNumber { get; set; }
        public string IntroInfomation { get; set; }
        public string ExprienceDescription { get; set; }
        public string JobExpect { get; set; }
        public Nullable<int> MinSalary { get; set; }
        public string MetaCategory { get; set; }
        public Nullable<System.Guid> User_ID { get; set; }
        public Nullable<System.Guid> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.Guid> LastEditBy { get; set; }
        public Nullable<System.DateTime> LastEditDate { get; set; }
        public string URL { get; set; }
        public string LocationName { get; set; }
        public string Skill { get; set; }
    }
}
