using Mameto.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Mameto.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string FName { get; set; } = "";
        public string LName { get; set; } = "";
        public string UImg { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Country { get; set; } = "";
        public string City { get;  set; } = "";
        public string Address { get; set; } = "";
        public string Bio { get; set; } = "";
        public virtual ICollection<PostModel> Post { get; set; } = new HashSet<PostModel>();
        public virtual ICollection<CommentModel> Comment { get; set; } = new HashSet<CommentModel>();
        public virtual ICollection<SpecialistcertifcateModel> Certificate { get; set; } = new HashSet<SpecialistcertifcateModel>();
    }
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<PostModel> Posts { get; set; }
        public DbSet<CommentModel> Comments { get; set; }
        public DbSet<postImgsModel> postImgs { get; set; }
        public DbSet<commentImgsModel> commentImgs { get; set; }
        public DbSet<SpecialistcertifcateModel> certificates { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        //public void Configure (EntityTypeBuilder<CommentModel> builder)
        //{
        //    builder.HasOne(c => c.Post)
        //        .WithMany(c => c.Comment)
        //        .HasForeignKey(c => c.PId)
        //        .IsRequired();


        //    builder.HasOne(c => c.User)
        //       .WithMany(c => c.Comment)
        //       .HasForeignKey(c => c.UId)
        //       .IsRequired();

        //}
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<postImgsModel>(
               i => i.HasKey(p => new { p.ImgId, p.PostId})
           );
            builder.Entity<CommentModel>()
                .HasOne(i => i.User).WithMany(i=>i.Comment)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<CommentModel>()
               .HasOne(i => i.Post).WithMany(i => i.Comment)
               .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<SpecialistcertifcateModel>()
                .HasOne(c => c.User).WithMany(c => c.Certificate)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}