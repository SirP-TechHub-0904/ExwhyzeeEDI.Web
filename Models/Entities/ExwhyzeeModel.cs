using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExwhyzeeEDI.Web.Models.Entities
{
    public class ExwhyzeeModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Surname { get; set; }
        public string Firstname { get; set; }
        public string Othername { get; set; }
        public string RegistrationNumber { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime Date { get; set; }
        public bool Paid { get; set; }
        public bool Uploaded { get; set; }
        public string BVN { get; set; }
        public string StateOfBusinessLocation { get; set; }
        public string Gender { get; set; }
        public string EDI { get; set; }
        public string Sector { get; set; }
        public string SubSector { get; set; }
        public string EquipmentAmount { get; set; }
        public string WorkingCapital { get; set; }
        public string LoanTenor { get; set; }
        public string LoanAmount { get; set; }
        public int SortOrder { get; set; }
        public string UploadBy { get; set; }
        public string CertificateNumber { get; set; }
        public string CertificateUploaded { get; set; }
        public string BusinessPlanUploaded { get; set; }
        public string MonthOfTraining { get; set; }
        public string YearOfTraining { get; set; }
        public string NumberOfTrainingDays { get; set; }
        public string Interviewed { get; set; }
        public string Disbursed { get; set; }
        public string OtherCooment { get; set; }

        public int ApplicantCategoryId { get; set; }
        public string Parish { get; set; }
        public string ParishState { get; set; }
        public string PassportPhoto { get; set; }
        public string IDUpload { get; set; }
    }
}