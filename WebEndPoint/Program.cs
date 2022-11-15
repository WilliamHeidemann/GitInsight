using Azure.Core;
using GitInsight;
using GitInsight.Infrastructure;

namespace WebEndPoint
{
    public class Program
    {

        static GitCommitTracker tracker;
        static PersistentStorageController controller;

        public static void Main(string[] args)
        {
            tracker = new();
            var factory = new PersistentStorageContextFactory();
            using var context = factory.CreateDbContext(Array.Empty<string>());
            controller = new(new DbCommitPersistentStorage(context), new DbRepositoryPersistentStorage(context));
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapGet("frequency/{githubOrganization}/{repositoryName}", async (
                    string githubOrganization,
                    string repositoryName)
                =>
            {
                var commits = await controller.FindAllGithubCommits(githubOrganization, repositoryName);
                return tracker.GetCommitFrequency(commits);
            });

            app.MapGet("author/{githubOrganization}/{repositoryName}", async (
                    string githubOrganization,
                    string repositoryName)
                =>
            {
                var commits = await controller.FindAllGithubCommits(githubOrganization, repositoryName);
                return tracker.GetCommitAuthor(commits);
            });

            app.Run();
        }
    }
}