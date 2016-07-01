namespace AsignarDataAccessLayer.AzureADBModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            Bugs = new HashSet<Bug>();
            Comments = new HashSet<Comment>();
            Filters = new HashSet<Filter>();
            UsersToProjects = new HashSet<UsersToProject>();
        }

        public int UserID { get; set; }

        [Required]
        [StringLength(35)]
        public string Name { get; set; }

        [Required]
        [StringLength(35)]
        public string Surname { get; set; }

        [Required]
        [StringLength(254)]
        public string Email { get; set; }

        public string AvatarImagePath { get; set; }

        [Required]
        [StringLength(254)]
        public string Login { get; set; }

        [Required]
        [StringLength(254)]
        public string Password { get; set; }

        public bool IsAdmin { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] Version { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Bug> Bugs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comment> Comments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Filter> Filters { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UsersToProject> UsersToProjects { get; set; }
    }
}
