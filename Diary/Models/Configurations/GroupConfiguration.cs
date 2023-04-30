using Diary.Models.Domains;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Diary.Models.Configurations
{
    public class GroupConfiguration : EntityTypeConfiguration<Group>
    {
        public GroupConfiguration()
        {
            ToTable("dbo.Groups");

            Property(g => g.Id)
                 .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(g => g.Name)
                .HasMaxLength(20)
                .IsRequired();
        }
    }
}
