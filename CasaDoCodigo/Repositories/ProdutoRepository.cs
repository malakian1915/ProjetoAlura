using CasaDoCodigo.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.Repositories
{
    public class ProdutoRepository : BaseRepository<Produto>, IProdutoRepository
    {
        private ICategoriaRepository _categoriaRepository;

        public ProdutoRepository(
            ApplicationContext contexto,
            ICategoriaRepository categoriaRepository) : base(contexto)
        {
            _categoriaRepository = categoriaRepository;
        }

        public IList<Produto> GetProdutos()
        {
            return dbSet.ToList();
        }

        public async Task<List<Produto>> GetProdutosWithDependencies()
        {
            var result = await dbSet.Include(x => x.Categoria).ToListAsync();

            if (!result.Any()) return new List<Produto>();

            return result;
        }

        public async Task<List<Produto>> GetProdutosWithDependencies(string search)
        {
            var result = await dbSet.Include(x => x.Categoria)
                .Where(w => w.Nome.Contains(search) || w.Categoria.Nome.Contains(search)).ToListAsync();

            if (!result.Any()) return new List<Produto>();

            return result;
        }

        public async Task SaveProdutos(List<Livro> livros)
        {
            foreach (var livro in livros)
            {
                var categoria = await SalvarCategoriaProduto(livro.Categoria);

                if (!GetProdutoByCodigo(livro.Codigo).Any())
                {
                    dbSet.Add(new Produto(livro.Codigo, livro.Nome, livro.Preco, categoria));

                    await contexto.SaveChangesAsync();
                }                
            }           
        }

        public IQueryable<Produto> GetProdutoByCodigo(string Codigo)
        {
            return dbSet.Where(p => p.Codigo == Codigo);
        }

        private async Task<Categoria> SalvarCategoriaProduto(string Categoria)
        {
            return await _categoriaRepository.SalvarCategoria(Categoria);
        }
    }

    public class Livro
    {
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public string Categoria { get; set; }
        public string Subcategoria { get; set; }
        public decimal Preco { get; set; }
    }
}
