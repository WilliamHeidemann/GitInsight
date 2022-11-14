using MyApp.Integration.Tests.Setup;

namespace GitInsight.WebEndPoint.Tests;

public class UnitTest1
{
    private readonly IntegrationTestFactory _factory;

    public UnitTest1(IntegrationTestFactory factory) 
    {
        _factory = factory;
    }
    
    [Fact]
    public void Test1()
    {
        
    }
}