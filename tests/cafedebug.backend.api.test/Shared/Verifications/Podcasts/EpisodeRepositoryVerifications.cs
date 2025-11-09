using System.Linq.Expressions;
using cafedebug_backend.domain.Podcasts;
using cafedebug_backend.domain.Podcasts.Repositories;
using Moq;

namespace cafedebug.backend.api.test.Shared.Verifications.Podcasts;

public class EpisodeRepositoryVerifications(Mock<IEpisodeRepository> episodeRepository)
{
    public void VerifyEpisodeExistenceChecked(Times times)
    {
        episodeRepository.Verify(
            x => x.AnyAsync(It.IsAny<Expression<Func<Episode, bool>>>()),
            times);
    }
    
    public void VerifyEpisodeSaved(Times times)
    {
        episodeRepository.Verify(x => x.SaveAsync(It.IsAny<Episode>()), times);
    }
    
    public void VerifyEpisodeUpdated(Times times)
    {
        episodeRepository.Verify(x => x.UpdateAsync(It.IsAny<Episode>()), times);
    }
    
    public void VerifyEpisodeRetrieved(int episodeId, Times times)
    {
        episodeRepository.Verify(x => x.GetByIdAsync(episodeId), times);
    }
    
    public void VerifyEpisodeDeleted(Episode episode, Times once)
    {
        episodeRepository.Verify(x => x.DeleteAsync(episode), once);
    }

    public void VerifyEpisodePageListRetrieved(int page, int pageSize,
        string? sortBy, bool descending, Times times)
    {
        episodeRepository.Verify(x => x.GetPageList(page, pageSize, sortBy, descending, CancellationToken.None), times);
    }
}