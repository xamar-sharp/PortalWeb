using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Storage;
namespace PortalWeb
{
    public sealed class Repository : IdentityDbContext<CustomUser, CustomRole, string>
    {
        public DbSet<Comment> Comments { get; set; }
        public Repository(DbContextOptions<Repository> opt) : base(opt)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(System.Reflection.Assembly.GetExecutingAssembly());
        }
    }
    public sealed class CustomRole : IdentityRole
    {
       
    }
    public sealed class CustomUser : IdentityUser
    {
        public byte[] Icon { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public CustomUser()
        {
            Comments = new List<Comment>(5);
        }
    }
    [Index("Title","Id",IsUnique =true,Name ="IDX_Comments")]
    public sealed class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string RateLevel { get; set; }
        [Timestamp]
        public byte[] Timestamp { get; set; }
        [Required]
        public string Intent { get; set; }
        public CustomUser User { get; set; }
    }
    public sealed class DesignFactory : IDesignTimeDbContextFactory<Repository>
    {
        public Repository CreateDbContext(string[] args)
        {
            if (args.Length == 0)
            {
#pragma warning disable
                throw new ExecutionEngineException();
            }
            DbContextOptionsBuilder<Repository> builder = new DbContextOptionsBuilder<Repository>();
            builder.UseSqlServer(args[0]);
            return new Repository(builder.Options);
        }
    }
    public sealed class UsersConfiguration : IEntityTypeConfiguration<CustomUser>
    {
        public void Configure(EntityTypeBuilder<CustomUser> builder)
        {
            
        }
    }
    public sealed class CommentsConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasOne(ent => ent.User).WithMany(ent => ent.Comments).OnDelete(DeleteBehavior.Cascade);
        }
    }

}
