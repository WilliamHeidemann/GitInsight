using GitInsight.Core;
using GitInsight.Infrastructure;

namespace MyApp.Integration.Tests.Setup;

public static class TestDataGenerator
{
    public static async Task GenerateTestData(PersistentStorageContext context)
    {
        var commitStorage = new DbCommitPersistentStorage(context);
        var commit = new DbCommitCreateDTO("1234", "Willy", new DateTime(2022, 11, 11), 1);
        await commitStorage.CreateAsync(commit);
        await context.SaveChangesAsync();
    }
}