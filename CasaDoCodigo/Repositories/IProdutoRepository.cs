using CasaDoCodigo.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.Repositories
{
    public interface IProdutoRepository
    {
        Task SaveProdutos(List<Livro> livros);
        IList<Produto> GetProdutos();
        Task<List<Produto>> GetProdutosWithDependencies();
        Task<List<Produto>> GetProdutosWithDependencies(string search);
        IQueryable<Produto> GetProdutoByCodigo(string Codigo);
    }
}