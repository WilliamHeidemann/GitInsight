using GitInsight;

namespace WebEndPoint
{
    public class Program
    {
        public static void Main(string[] args)
        {
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
                    string repositoryName,
                    GitCommitTracker tracker,
                    PersistentStorageController controller)
                =>
            {
                var commits = await controller.FindAllCommitsAsync(repositoryName);
                return tracker.GetCommitFrequency(commits);
            });

            app.MapGet("author/{githubOrganization}/{repositoryName}", async (
                    string githubOrganization,
                    string repositoryName,
                    GitCommitTracker tracker,
                    PersistentStorageController controller)
                =>
            {
                var commits = await controller.FindAllCommitsAsync(repositoryName);
                return tracker.GetCommitAuthor(commits);
            });

            app.Run();
        }
    }
}