using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using ExwhyzeeEDI.Web.Models.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ExwhyzeeEDI.Web.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
        public string VerifyCode { get; set; }

        public bool ComfirmVerifyCode { get; set; }

        public int? SchoolId { get; set; }

    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

       

        public DbSet<ImageFile> ImageFiles { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<ProgramCourse> ProgramCourses { get; set; }
        public DbSet<States> States { get; set; }
        public DbSet<LocalGovs> LocalGovs { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<UserProgram> UserPrograms { get; set; }
        public DbSet<Gallery> Galleries { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<AboutUsImage> AboutUsImages { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<YoungMind> YoungMinds { get; set; }
        public DbSet<BusinessPlan> BusinessPlans { get; set; }
        public DbSet<CertificateInfo> CertificateInfos { get; set; }
        public DbSet<ABP> ABPs { get; set; }
        public DbSet<ApplicantCategory> ApplicantCategorys { get; set; }
        public DbSet<ExwhyzeeModel> ExwhyzeeModels { get; set; }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<ExwhyzeeEDI.Web.Models.Dtos.RegistrationDto> RegistrationDtoes { get; set; }
    }
}