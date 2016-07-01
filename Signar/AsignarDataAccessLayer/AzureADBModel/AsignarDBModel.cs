namespace AsignarDataAccessLayer.AzureADBModel
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class AsignarDBModel : DbContext
    {
        public AsignarDBModel()
            : base("name=AsignarDBConnection")
        {
        }

        public virtual DbSet<Attachment> Attachments { get; set; }
        public virtual DbSet<Bug> Bugs { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Filter> Filters { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UsersToProject> UsersToProjects { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Attachment>()
                .Property(e => e.Version)
                .IsFixedLength();

            modelBuilder.Entity<Bug>()
                .Property(e => e.Version)
                .IsFixedLength();

            modelBuilder.Entity<Bug>()
                .HasMany(e => e.Attachments)
                .WithRequired(e => e.Bug)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Bug>()
                .HasMany(e => e.Comments)
                .WithRequired(e => e.Bug)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Comment>()
                .Property(e => e.Version)
                .IsFixedLength();

            modelBuilder.Entity<Filter>()
                .Property(e => e.Version)
                .IsFixedLength();

            modelBuilder.Entity<Project>()
                .Property(e => e.Version)
                .IsFixedLength();

            modelBuilder.Entity<Project>()
                .HasMany(e => e.Bugs)
                .WithRequired(e => e.Project)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Project>()
                .HasMany(e => e.UsersToProjects)
                .WithRequired(e => e.Project)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Version)
                .IsFixedLength();

            modelBuilder.Entity<User>()
                .HasMany(e => e.Bugs)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.AssigneeID);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Filters)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.UsersToProjects)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UsersToProject>()
                .Property(e => e.Version)
                .IsFixedLength();
        }
    }
}
