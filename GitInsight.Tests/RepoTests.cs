public class RepoTests {
     Repository _repo = new Repository("../../../..");

    [Fact]
    public void First_Commit_Made_By_WilliamHeidemann()
    {
        // Given
        var firstCommit = _repo.Commits.Last(); // Last = Oldest, as First would be newest

        // When

        // Then
        firstCommit.Author.Name.Should().Be("WilliamHeidemann");
    }
}