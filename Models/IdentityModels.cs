using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ExwhyzeeEDI.Web.Models.Entities;
using ExwhyzeeEDI.Web.Models.Dtos;
using ExwhyzeeEDI.Web.Helper;

namespace ExwhyzeeEDI.Web.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? VerifyCode { get; set; }
        public bool ComfirmVerifyCode { get; set; }
        public int? SchoolId { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public DateTime? LockoutEndDateUtc
        {
            get => LockoutEnd?.UtcDateTime;
            set => LockoutEnd = value.HasValue ? new DateTimeOffset(value.Value) : null;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public ApplicationDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var conn = EnvLoader.Get("ConnectionStrings__DefaultConnection",
                           EnvLoader.Get("DEFAULT_CONNECTION", string.Empty));
                if (!string.IsNullOrEmpty(conn))
                {
                    optionsBuilder.UseSqlServer(conn);
                }
            }
        }

        public DbSet<ImageFile> ImageFiles { get; set; } = null!;
        public DbSet<Author> Authors { get; set; } = null!;
        public DbSet<Review> Reviews { get; set; } = null!;
        public DbSet<Course> Courses { get; set; } = null!;
        public DbSet<ProgramCourse> ProgramCourses { get; set; } = null!;
        public DbSet<States> States { get; set; } = null!;
        public DbSet<LocalGovs> LocalGovs { get; set; } = null!;
        public DbSet<Profile> Profiles { get; set; } = null!;
        public DbSet<Post> Posts { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Comment> Comments { get; set; } = null!;
        public DbSet<UserProgram> UserPrograms { get; set; } = null!;
        public DbSet<Gallery> Galleries { get; set; } = null!;
        public DbSet<School> Schools { get; set; } = null!;
        public DbSet<AboutUsImage> AboutUsImages { get; set; } = null!;
        public DbSet<Slider> Sliders { get; set; } = null!;
        public DbSet<Payment> Payments { get; set; } = null!;
        public DbSet<YoungMind> YoungMinds { get; set; } = null!;
        public DbSet<BusinessPlan> BusinessPlans { get; set; } = null!;
        public DbSet<CertificateInfo> CertificateInfos { get; set; } = null!;
        public DbSet<ABP> ABPs { get; set; } = null!;
        public DbSet<ApplicantCategory> ApplicantCategorys { get; set; } = null!;
        public DbSet<ExwhyzeeModel> ExwhyzeeModels { get; set; } = null!;
        public DbSet<RegistrationDto> RegistrationDtoes { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}