using CasaDoCodigo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.Repositories
{
    public class CategoriaRepository : BaseRepository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(ApplicationContext contexto) : base(contexto)
        {
        }

        public async Task<Categoria> SalvarCategoria(string Nome)
        {
            var categoria = GetCategoriaByNome(Nome).FirstOrDefault();

            if (categoria == null)
            {
                categoria = new Categoria(Nome);
                dbSet.Add(categoria);

                await contexto.SaveChangesAsync();
            }

            return categoria;
        }

        public IQueryable<Categoria> GetCategoriaByNome(string Nome)
        {
            return dbSet.Where(p => p.Nome == Nome);
        }
    }
}
