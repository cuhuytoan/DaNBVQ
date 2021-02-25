using System;

namespace HNM.DataNC.ModelsStore
{
    public class CV_Search_Result
    {
        public Nullable<long> NoItem { get; set; }
        public Nullable<int> TotalRows { get; set; }
        public Nullable<int> Id { get; set; }
        public string FullName { get; set; }
        public Nullable<int> YearOfBirth { get; set; }
        public Nullable<bool> Gender { get; set; }
        public string PlaceOfBirth { get; set; }
        public string HomeTown { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Certificate { get; set; }
        public string Avatar { get; set; }
        public string MainOccupation { get; set; }
        public Nullable<int> CareerCategoryId { get; set; }
        public string CareerCategoryName { get; set; }
        public string Degree { get; set; }
        public Nullable<int> YearsOfExperience { get; set; }
        public string TitleName { get; set; }
        public Nullable<decimal> Salary { get; set; }
        public string TypeOfWork { get; set; }
        public string IntroInfomation { get; set; }
        public string LocalWork { get; set; }
        public Nullable<System.Guid> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.Guid> LastEditBy { get; set; }
        public Nullable<System.DateTime> LastEditDate { get; set; }
        public string URL { get; set; }
        public Nullable<int> StatusTypeId { get; set; }
    }
}
