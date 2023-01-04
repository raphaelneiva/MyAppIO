using MyAppIO.Business.Models;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MyAppIO.Business.Interfaces
{
    public interface IRepository <TEntity> : IDisposable where TEntity : Entity
    {
        Task Adicionar(TEntity enity);
        Task<TEntity> ObterPorId(Guid id);
        Task<List<TEntity>> ObterTodos();
        Task Atualizar(TEntity enity);
        Task Remover(Guid id);
        Task<IEnumerable<TEntity>> Buscar(Expression<Func<TEntity, bool>> predicate);
        Task<int> SaveChanges();

    }
}
