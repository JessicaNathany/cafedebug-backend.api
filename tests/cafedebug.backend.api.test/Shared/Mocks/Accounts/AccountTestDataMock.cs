using AutoFixture;
using cafedebug.backend.application.Accounts.DTOs.Requests;
using cafedebug_backend.domain.Accounts;
using cafedebug_backend.domain.Messages.Email.Request;
using cafedebug_backend.infrastructure.Data.Pagination;
using MockQueryable;

namespace cafedebug.backend.api.test.Shared.Mocks.Accounts
{
    public class AccountTestDataMock(IFixture fixture)
    {
        public SendEmailRequest SendEmailRequest()
        {
            var build = fixture.Build<SendEmailRequest>();
            return build.Create();
        }

        public ForgotPasswordRequest ForgotPasswordRequest()
        {
            var build = fixture.Build<ForgotPasswordRequest>();
            return build.Create();
        }

        public ChangePasswordRequest ChangePasswordRequest()
        {
            var build = fixture.Build<ChangePasswordRequest>();
            return build.Create();
        }

        public PagedList<UserAdmin> CreateBannerPagedResult(int page, int pageSize, string? sortBy = null, bool descending = false, int totalCount = 10)
        {
            var user = fixture.CreateMany<UserAdmin>(totalCount).ToList();

            var userQuery = user.BuildMock();

            return new PagedList<UserAdmin>(userQuery, page, pageSize, sortBy, descending);
        }
    }
}
