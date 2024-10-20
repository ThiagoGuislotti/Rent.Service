using Microsoft.EntityFrameworkCore;

namespace Rent.Service.Infrastructure.Context
{
    public class GenericDbScopedFactory<TDbContext> : IDbContextFactory<TDbContext>
        where TDbContext : DbContext
    {
        #region Variáveis
        private readonly IDbContextFactory<TDbContext> _pooledFactory;
        #endregion

        #region Construtores
        public GenericDbScopedFactory(IDbContextFactory<TDbContext> pooledFactory)
        {
            _pooledFactory = pooledFactory ?? throw new ArgumentNullException(nameof(pooledFactory));
        }
        #endregion

        #region Métodos/Operadores Públicos
        public virtual TDbContext CreateDbContext()
        {
            return _pooledFactory.CreateDbContext();
        }
        #endregion
    }
}