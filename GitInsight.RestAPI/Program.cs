using Azure.Core;
using GitInsight.CLI;
using GitInsight.Infrastructure;

namespace GitInsight.RestAPI
{
    public class Program
    {
        IPersistentStorageController? _controller;
        static GitCommitTracker? tracker;
        static PersistentStorageController? controller;
        static GithubAPIController? githubAPIController;

        public static void Main(string[] args)
        {
            _controller = controller;
            var builder = WebApplication.CreateBuilder();

            GithubAPIController githubAPIController = new();

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
                var remoteRepoPath = controller.FindAllGithubCommits(githubOrganization, repositoryName);
                return await controller.GetFrequencyMode(remoteRepoPath);
            });

            app.MapGet("author/{githubOrganization}/{repositoryName}", async (
                    string githubOrganization,
                    string repositoryName)
                =>
            {
                var remoteRepoPath = controller.FindAllGithubCommits(githubOrganization, repositoryName);
                return await controller.GetAuthorMode(remoteRepoPath);
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

        public static void Main(string[] args)
        {
            using var context = new PersistentStorageContext();
            new Program(new PersistentStorageController(context));
        }   
    }
}