using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.Models.ViewModels
{
    public class ProdutosViewModel
    {
        public List<Produto> Produtos { get; set; }
        public string Pesquisa { get; set; }
        
        public bool semDados { get; set; }
    }
}
