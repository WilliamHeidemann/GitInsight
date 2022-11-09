namespace GitInsight.Infrastructure;

public partial class PersistentStorageContext : DbContext
{
    public PersistentStorageContext(DbContextOptions<PersistentStorageContext> builderOptions)  : base(builderOptions){}

    public virtual DbSet<DbRepository> Repositories => Set<DbRepository>();
    public virtual DbSet<DbCommit> Commits => Set<DbCommit>();

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DbCommit>()
            .Property(c => c.Date)
            .HasConversion<string>();
        modelBuilder.Entity<DbRepository>()
            .HasIndex(r => r.FilePath)
            .IsUnique();
    }

    public void Clear()
    {
     
    }
}