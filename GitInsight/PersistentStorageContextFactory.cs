﻿using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace GitInsight;

public class PersistentStorageContextFactory : IDesignTimeDbContextFactory<PersistentStorageContext>
{
    public PersistentStorageContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
        var connectionString = configuration.GetConnectionString("GitInsight");

        var optionsBuilder = new DbContextOptionsBuilder<PersistentStorageContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new PersistentStorageContext(optionsBuilder.Options);
    }
}