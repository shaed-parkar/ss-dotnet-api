namespace SS.Mappings;

public class AuthorMap : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        builder.ToTable(nameof(Author));

        builder.HasKey(x => x.Id);
        builder.Property(x => x.FirstName).IsRequired();
        builder.Property(x => x.LastName).IsRequired();

        builder.HasMany(x => x.Notes).WithOne().OnDelete(DeleteBehavior.Cascade);

        var notesNavigation = builder.Metadata.FindNavigation(nameof(Author.Notes));
        notesNavigation!.SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}