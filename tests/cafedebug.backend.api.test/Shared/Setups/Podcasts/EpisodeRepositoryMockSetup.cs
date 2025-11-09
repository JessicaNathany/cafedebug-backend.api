using System.Linq.Expressions;
using cafedebug_backend.domain.Podcasts;
using cafedebug_backend.domain.Podcasts.Repositories;
using cafedebug_backend.domain.Shared;
using cafedebug.backend.application.Common.Pagination;
using Moq;

namespace cafedebug.backend.api.test.Shared.Setups.Podcasts;

public class EpisodeRepositoryMockSetup(Mock<IEpisodeRepository> episodeRepository)
{
    public void EpisodeExists()
    {
        episodeRepository
            .Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Episode, bool>>>()))
            .ReturnsAsync(true);
    }

    public void EpisodeDoesNotExist()
    {
        episodeRepository
            .Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Episode, bool>>>()))
            .ReturnsAsync(false);
    }

    public void GetEpisodeById(Episode episode)
    {
        episodeRepository
            .Setup(x => x.GetByIdAsync(episode.Id))
            .ReturnsAsync(episode);
    }
    
    public void GetEpisodeByIdNotFound(int episodeId)
    {
        episodeRepository
            .Setup(x => x.GetByIdAsync(episodeId))
            .ReturnsAsync((Episode?)null);
    }
    
    public void EpisodeSave(Action<Episode> callback)
    {
        episodeRepository
            .Setup(x => x.SaveAsync(It.IsAny<Episode>()))
            .Callback(callback)
            .Returns(Task.CompletedTask);
    }

    public void EpisodeSaveThrows(Exception exception)
    {
        episodeRepository
            .Setup(x => x.SaveAsync(It.IsAny<Episode>()))
            .ThrowsAsync(exception);
    }

    public void EpisodeUpdate()
    {
        episodeRepository
            .Setup(x => x.UpdateAsync(It.IsAny<Episode>()))
            .Returns(Task.CompletedTask);
    }
    
    public void EpisodeGetPageList(IPagedResult<Episode> pagedResult, PageRequest pageRequest)
    {
        episodeRepository
            .Setup(x => x.GetPageList(pageRequest.Page, pageRequest.PageSize, pageRequest.SortBy, pageRequest.Descending, CancellationToken.None))
            .ReturnsAsync(pagedResult);   
    }
}