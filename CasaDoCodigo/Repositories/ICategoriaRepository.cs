using CasaDoCodigo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.Repositories
{
    public interface ICategoriaRepository
    {
        Task<Categoria> SalvarCategoria(string Nome);
        IQueryable<Categoria> GetCategoriaByNome(string Nome);
    }
}
