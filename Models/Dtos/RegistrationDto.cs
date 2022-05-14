using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ExwhyzeeEDI.Web.Models.Dtos
{
    public class RegistrationDto
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }


        [Required]
        [UniquePhone(ErrorMessage = "This Phone Number is alread taken.")]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; }

       

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }


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

        [Display(Name = "Any Vocation Now")]
        public string AreYouInAbyVocationNow { get; set; }

        [Display(Name = "Hobby")]
        public string Hobby { get; set; }

        [Display(Name = "Skills seperate with comma")]
        public string Skills { get; set; }


        public string UserId { get; set; }

        [Display(Name = "Write about yourself")]
        public string WriteAboutYourself { get; set; }
        [Display(Name = "Your Biggest Achievement?")]
        public string YourBiggestAchievement { get; set; }
        [Display(Name = "What is Your Passion?")]
        public string WhatIsYourPassion { get; set; }
        [Display(Name = "Define Your Personality")]
        public string DefineYourPersonality { get; set; }
        [Display(Name = "How Do You See YourSelf In 10 Years")]
        public string HowDoYouSeeYourSelfIn10Years { get; set; }

        [Display(Name = "What Are Your Limitations Towards Your Goal")]
        public string WhatAreYourLimitationsTowardsYourGoal { get; set; }
        [Display(Name = "How Do You Think You Can Overcome Your Limitations")]
        public string HowDoYouThinkYouCanOvercomeYourLimitations { get; set; }

        [Display(Name = "What Do You Want To Change In The Society")]
        public string WhatDoYouWantToChangeInTheSociety { get; set; }

        [Display(Name = "Previouse Project/Attempted/Research/Publicity/Invention. if any?")]
        public string PreviouseProjectAttemptedResearchPublicityInvention { get; set; }

        [Display(Name = "What Is Your Greatest Goal")]
        public string WhatIsYourGreatestGoal { get; set; }



        [Display(Name = "Are You Ready To Make Changes To Your Environment?")]

        public string AreYouReadyToMakeChangesToYourEnvironment { get; set; }
        [Display(Name = "If Yes, How Can You Make Changes")]
        public string IfYes_HowCanYouMakeChanges { get; set; }
        [Display(Name = "List the name(s) and email of referee(s) ifany")]

        public string Referee { get; set; }
        [Display(Name = "Know A Friend That You Think Will Make A Great Change In His Environment? Drop their emails and phone number and we'll reachout")]
        public string KnowAFriendThatYouThinkWillMakeAGreatChangeInHisEnvironment { get; set; }
        [Display(Name = "We're listening! Do you have any suggestion, idea or feedback that will help make you the best instrument of change?")]
        public string IdeaAndSuggestion { get; set; }


        [Display(Name = "All Your Social Media Handles. facebook: xxxxxx, Instagram: xxxxx, LinkIn, Github,whatsapp number, cryptocurrency wallet... if any.")]
        public string SocialMedia { get; set; }


        [Display(Name = "Passport")]
        public string PassportUrl { get; set; }
    }
}