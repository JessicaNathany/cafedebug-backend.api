using AutoFixture;
using cafedebug_backend.domain.Podcasts;

namespace cafedebug.backend.api.test.Shared.Mocks.Podcasts;

public class CategoryTestDataMock(IFixture fixture)
{
    public Category CreateCategory(int id)
    {
        return fixture.Build<Category>()
            .With(c => c.Id, id)
            .Create();
    }
}