using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static ExwhyzeeEDI.Web.Models.Entities.Enum;

namespace ExwhyzeeEDI.Web.Models.Entities
{
    public class Profile
    {
        public int Id { get; set; }
        public string RegistrationNumber { get; set; }
        public string Title { get; set; }
        public string Surname { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Other Name")]
        public string OtherName { get; set; }
        public string Gender { get; set; }
        [Display(Name = "Date of birth")]
        public DateTime Dateofbirth { get; set; }
        public DateTime DateRegistered { get; set; }
        [Display(Name = "Marital Status")]
        public string MaritalStatus { get; set; }
        [Display(Name = "Religion")]
        public string Religion { get; set; }


        [Display(Name = "Permanent Home Address")]
        public string PermanentHomeAddress { get; set; }

        
        [Display(Name = "Mode Of Identification")]
        public string ModeOfIdentification { get; set; }
        [Display(Name = "Identification Number")]
        public string IdentificationNumber { get; set; }
        [Display(Name = "Contact Address")]
        public string ContactAddress { get; set; }
        [Display(Name = "State of Origin")]
        public string StateofOrigin { get; set; }
        [Display(Name = "Local Government Area")]
        public string LocalGovernmentArea { get; set; }
        [Display(Name = "Current Occupation")]
        public string CurrentOccupation { get; set; }
        [Display(Name = "Business area of Interest")]
        public string AreaOfInterest { get; set; }
      
        [Display(Name = "Any Vocation Now")]
        public string AreYouInAbyVocationNow { get; set; }


        [Display(Name = "Specify Vocation")]
        public string SpecifyVocation { get; set; }

        [Display(Name = "Do You Require ICT Skills For Your Business")]
        public string DoYouRequireICTSkillsForYourBusiness { get; set; }




        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        [Display(Name = "Educational Qualification")]
        public string EducationalQualification { get; set; }

        [Display(Name = "Last School Attended")]
        public string SchoolAttended { get; set; }

        [Display(Name = "Discipline")]
        public string Discipline { get; set; }

        [Display(Name = "Year Graduated")]
        public string YearGraduated { get; set; }


        [Display(Name = "Certificate Number")]
        public string CertificateId { get; set; }

        [Display(Name = "Certificate Date")]
        public string CertificateDate { get; set; }

        [Display(Name = "How Did You Get To Know About EDI")]
        public string HowDidYouGetToKnowAboutEDI { get; set; }

        [Display(Name = "What Is Your Personal Motivation for applying for this training")]
        public string WhatIsYourMotivation { get; set; }

        [Display(Name = "How will the training Benefit You")]
        public string WhatsTheBenefitOfEDI { get; set; }


        [Display(Name = "Passport")]
        public string PassportUrl { get; set; }

        public int? SchoolId { get; set; }
        public School GetSchool { get; set; }

        public bool NirsalRegistration { get; set; }
        public bool AccountOpen { get; set; }
        public bool BusinessPlan { get; set; }
        public bool CollectedCertificate { get; set; }
        public string Department { get; set; }

        [Display(Name = "User Status")]

        public UserStatus UserStatus { get; set; }
        [Display(Name = "Date Enrolled For Training")]

        public DateTime? DateEnrolledForTraining { get; set; }

        [Display(Name = "Recommendation Note")]
        [AllowHtml]
        public string RecommendationNote { get; set; }

        [Display(Name = "Paid For Training")]
         public bool PaidForTraining { get; set; }
        [Display(Name = "Attendance Status")]
        public EdiStatus Attendance { get; set; }
        [Display(Name = "Certificate Collected")]
        public EdiStatus CertificateCollected { get; set; }
        [Display(Name = "Certificate Printed")]
        public EdiStatus CertificatePrinted { get; set; }
        [Display(Name = "Nirsal Payment")]
        public EdiStatus NirsalPayment { get; set; }
        [Display(Name = "Business Plan Upload")]
        public EdiStatus BusinessPlanUpload { get; set; }
        [Display(Name = "BusinessPlanLink")]
        public string BusinessPlanLink { get; set; }
        [Display(Name = "BusinessPlanDateUploaded")]
        public DateTime BusinessPlanDateUploaded { get; set; }
        [Display(Name = "Program By")]
        public string ProgramBy { get; set; }
        public string RegisteredBy { get; set; }
        [Display(Name = "Form Status")]
        public FormStatus FormStatus { get; set; }
        public bool NeedCertificate { get; set; }
        public bool NeedBusinessPlan { get; set; }
    }
}