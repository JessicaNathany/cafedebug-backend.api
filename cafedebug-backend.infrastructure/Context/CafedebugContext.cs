using Microsoft.EntityFrameworkCore;
namespace cafedebug_backend.infrastructure.Context
{
    public class CafedebugContext : DbContext
    {
        public CafedebugContext(DbContextOptions<CafedebugContext> options) : base(options)
        { }
    }
}
