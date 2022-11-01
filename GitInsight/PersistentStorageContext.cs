
namespace GitInsight;

public class PersistentStorageContext : DbContext
{
    public PersistentStorageContext(DbContextOptions<PersistentStorageContext> builderOptions)  : base(builderOptions){}
}