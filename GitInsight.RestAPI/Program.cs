using Azure.Core;
using GitInsight.CLI;
using GitInsight.Infrastructure;

namespace GitInsight.RestAPI
{
    public class Program
    {

        static GitCommitTracker? tracker;
        static PersistentStorageController? controller;
        static GithubAPIController? githubAPIController;

        public static void Main(string[] args)
        {
            tracker = new GitCommitTracker();
            using var context = new PersistentStorageContext();
            controller = new PersistentStorageController(context);
            githubAPIController = new GithubAPIController();
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
                return await controller.FindAllGithubCommits(githubOrganization, repositoryName);
            });

            app.MapGet("author/{githubOrganization}/{repositoryName}", async (
                    string githubOrganization,
                    string repositoryName)
                =>
            {
                return await controller.FindAllGithubCommits(githubOrganization, repositoryName);
            });

            app.MapGet("forks/{githubOrganization}/{repositoryName}", async (
                    string githubOrganization,
                    string repositoryName)
                =>
            {
                return await githubAPIController.GetForkList(githubOrganization, repositoryName);
            });

            app.Run();
        }
    }
}