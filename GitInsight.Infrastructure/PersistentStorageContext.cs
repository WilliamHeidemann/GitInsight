namespace GitInsight.Infrastructure;

public class PersistentStorageContext : DbContext
{
    public PersistentStorageContext(DbContextOptions<PersistentStorageContext> builderOptions)  : base(builderOptions){}

    public virtual DbSet<DbRepository> Repositories => Set<DbRepository>();
    public virtual DbSet<DbCommit> Commits => Set<DbCommit>();
}