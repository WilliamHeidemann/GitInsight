namespace GitInsight;

public class PersistentStorage
{
    private PersistentStorageContext _context;
    public PersistentStorage(PersistentStorageContext context)
    {
        _context = context;
    }

    public Repository GetRepository(string filepath)
    {
        return new Repository();
    }
}