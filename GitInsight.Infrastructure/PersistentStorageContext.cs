
namespace GitInsight.Infrastructure;

public class PersistentStorageContext : DbContext
{
    public PersistentStorageContext(DbContextOptions<PersistentStorageContext> builderOptions)  : base(builderOptions){}

    public PersistentStorageContext() {}

    public DbSet<DbRepository> Repositories => Set<DbRepository>();
    public DbSet<DbCommit> Commits => Set<DbCommit>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if(!optionsBuilder.IsConfigured){
        var configuration = new ConfigurationBuilder().AddUserSecrets<PersistentStorageContext>()
            .Build();
        var connectionString = configuration.GetConnectionString("GitInsight");
        Console.WriteLine(connectionString);
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
    }
}