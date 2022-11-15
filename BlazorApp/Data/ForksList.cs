namespace BlazorApp.Data;
public class ForksList
{
    public Task<Fork[]> GetForksAsync()
    {
        return Task.FromResult(Enumerable.Range(1, 5).Select(index => new Fork
        {
            Identifier = "SHA"
        }).ToArray());
    }
}