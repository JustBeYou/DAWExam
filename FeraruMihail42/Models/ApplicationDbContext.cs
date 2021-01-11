namespace FeraruMihail42.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Linq;

    public class ApplicationDbInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            var domainInfo = context.Domains.Add(new Domain { Denumire = "Informatica" });
            var domainMate = context.Domains.Add(new Domain { Denumire = "Matematica" });
            context.Domains.Add(new Domain { Denumire = "CTI" });

            context.Students.Add(new Student { Nume = "Mihail", Email = "mihail@yahoo.com", DataNastere = new DateTime(2000, 06, 30), Domain = domainInfo });
            context.Students.Add(new Student { Nume = "Maricica", Email = "maricica@yahoo.com", DataNastere = new DateTime(1999, 06, 30), Domain = domainInfo });
            context.Students.Add(new Student { Nume = "Cristi", Email = "cristi@yahoo.com", DataNastere = new DateTime(1995, 06, 30), Domain = domainInfo });
            context.Students.Add(new Student { Nume = "Elisabeta", Email = "elisabeta@yahoo.com", DataNastere = new DateTime(1996, 06, 30), Domain = domainMate });
            context.Students.Add(new Student { Nume = "Costica", Email = "costica@yahoo.com", DataNastere = new DateTime(2001, 06, 30), Domain = domainMate });
            context.Students.Add(new Student { Nume = "Ana", Email = "ana@yahoo.com", DataNastere = new DateTime(1993, 06, 30), Domain = domainMate });

            base.Seed(context);
        }
    }

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
            : base("name=ApplicationDbContext")
        {
            Database.SetInitializer(new ApplicationDbInitializer());
        }

        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Domain> Domains { get; set; }
    }

    public class Student
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(128, ErrorMessage = "Numele nu poate fi mai lung de 128 de caractere.")]
        public string Nume { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Email-ul trebuie sa fie valid.")]
        [StringLength(25, ErrorMessage = "Adresa de email nu poate avea mai mult de 25 de caractere.")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Data de nastere")]
        [DisplayFormat(DataFormatString = @"{0:MM\/dd\/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DataNastere { get; set; }

        [ForeignKey("Domain")]
        public int DomainId { get; set; }
        public virtual Domain Domain { get; set; }
    }

    public class Domain
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Denumirea nu poate fi mai lunga de 50 de caractere.")]
        [Display(Name = "Denumire domeniu")]
        public string Denumire { get; set; } 
    }
}