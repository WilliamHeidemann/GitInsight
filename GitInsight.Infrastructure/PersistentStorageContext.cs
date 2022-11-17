
namespace GitInsight.Infrastructure;

public partial class PersistentStorageContext : DbContext
{
    public virtual DbSet<DbRepository> Repositories {get; set; } = null!;
    public virtual DbSet<DbCommit> Commits {get; set; } = null!;

    public PersistentStorageContext() {}
    public PersistentStorageContext(DbContextOptions<PersistentStorageContext> builderOptions)  
        : base(builderOptions)
    { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if(!optionsBuilder.IsConfigured)
        {
            var configuration = new ConfigurationBuilder().AddUserSecrets<PersistentStorageContext>()
                .Build();
            var connectionString = configuration.GetConnectionString("GitInsight");
            optionsBuilder.UseNpgsql(connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DbCommit>()
            .Property(c => c.Date)
            .HasConversion<string>();
        modelBuilder.Entity<DbRepository>()
            .HasIndex(r => r.FilePath)
            .IsUnique();
        modelBuilder.Entity<DbCommit>()
            .HasKey(c => new { c.SHA, c.RepoId });
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    public void Clear()
    {

    }
}