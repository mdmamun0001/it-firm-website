using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Reflection.Metadata;
using Zay.Models;

namespace Zay.Data
{
    public class AplicationDbContext: IdentityDbContext<ApplicationUser>
    {
        public AplicationDbContext(DbContextOptions <AplicationDbContext> options): base(options)
        {

        }
        public DbSet<ServiceCategory> ServiceCategories {set; get; }
        public DbSet<Slider>Sliders { set; get; }
        public DbSet<Employe> Employes { set; get; }
        public DbSet<Client> Clients { set; get; }
        public DbSet<CustomerReview> CustomerReviews { set; get; }
        public DbSet<Tool> Tools { set; get; }
        public DbSet<Project> Projects { set; get; }
        public DbSet<Service> Services { set; get; }
        public DbSet<About> Abouts { set; get; }
        public DbSet<Gallary> gallaries { set; get; }
        public DbSet<SocialMedia> socialMedias { set; get; }
        public DbSet<CustomerMessage> CustomerMessages { set; get; }
        public DbSet<AppoinmentRequest> AppoinmentRequests { set; get; }
        public DbSet<JobApplication> JobApplications { set; get; }
        public DbSet<MailAddress> MailAddresses { set; get; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { 
            base.OnModelCreating(modelBuilder);
            modelBuilder
            .Entity<Project>()
            .HasMany(p => p.Tools)
            .WithMany(p => p.Projects)
            .UsingEntity(j => j.ToTable("ProjectTool"));

           modelBuilder.Entity<Project>()
            .Navigation(b => b.Tools)
            .UsePropertyAccessMode(PropertyAccessMode.Property);

           modelBuilder.Entity<Tool>()
            .Navigation(T => T.Projects)
            .UsePropertyAccessMode(PropertyAccessMode.Property);
              
           modelBuilder
           .Entity<ServiceCategory>()
           .HasMany(sc => sc.Services)
           .WithMany(sc => sc.ServiceCategories)
           .UsingEntity(jt => jt.ToTable("ServiceCategoryService"));

          modelBuilder
          .Entity<Service>()
          .HasMany(s => s.ServiceCategories)
          .WithMany(s => s.Services)
          .UsingEntity(jt => jt.ToTable("ServiceCategoryService"));

          modelBuilder.Entity<ServiceCategory>()
          .Navigation(sc => sc.Services)
          .UsePropertyAccessMode(PropertyAccessMode.Property);

          modelBuilder.Entity<Service>()
          .Navigation(s => s.ServiceCategories)
          .UsePropertyAccessMode(PropertyAccessMode.Property);
          
        }
    }
}
