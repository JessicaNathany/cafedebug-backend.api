using AutoFixture;
using AutoFixture.AutoMoq;

namespace cafedebug.backend.api.test.Shared;

public class BaseTest
{
    protected Fixture Fixture { get; }

    protected BaseTest()
    {
        Fixture = new Fixture();
        ConfigureFixture();
    }

    private void ConfigureFixture()
    {
        Fixture.Customize(new AutoMoqCustomization());
        Fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => Fixture.Behaviors.Remove(b));
        Fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }
}