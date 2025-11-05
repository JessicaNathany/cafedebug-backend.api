using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using cafedebug.backend.application.Common.Mappings;
using Microsoft.Extensions.Logging;

namespace cafedebug.backend.api.test.Shared;

public class BaseTest
{
    protected Fixture Fixture { get; }
    protected IMapper Mapper { get; }
    
    protected BaseTest()
    {
        IConfigurationProvider configuration = new MapperConfiguration(config => config.AddProfile<MappingProfile>(), new LoggerFactory());
        Mapper = configuration.CreateMapper();
        
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