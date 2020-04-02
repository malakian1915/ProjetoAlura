using CasaDoCodigo.Models;
using CasaDoCodigo.Models.ViewModels;
using CasaDoCodigo.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.Controllers
{
    public class PedidoController : Controller
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IItemPedidoRepository _itemPedidoRepository;

        public PedidoController(IProdutoRepository produtoRepository,
            IPedidoRepository pedidoRepository,
            IItemPedidoRepository itemPedidoRepository)
        {
            this._produtoRepository = produtoRepository;
            this._pedidoRepository = pedidoRepository;
            this._itemPedidoRepository = itemPedidoRepository;
        }

        public IActionResult Carrossel()
        {
            return View(_produtoRepository.GetProdutos());
        }

        public async Task<IActionResult> Carrinho(string codigo)
        {
            if (!string.IsNullOrEmpty(codigo))
            {
                await _pedidoRepository.AddItem(codigo);
            }

            Pedido taskPedido = await _pedidoRepository.GetPedido();
            List<ItemPedido> itens = taskPedido.Itens;
            CarrinhoViewModel carrinhoViewModel = new CarrinhoViewModel(itens);
            return base.View(carrinhoViewModel);
        }

        public async Task<IActionResult> Cadastro()
        {
            var pedido = await _pedidoRepository.GetPedido();

            if (pedido == null)
            {
                return RedirectToAction("Carrossel");
            }

            return View(pedido.Cadastro);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Resumo(Cadastro cadastro)
        {
            if (ModelState.IsValid)
            {
                return View(await _pedidoRepository.UpdateCadastro(cadastro));
            }
            return RedirectToAction("Cadastro");
        }

        public async Task<IActionResult> BuscaDeProdutos(string Pesquisa)
        {
            var produtosViewModel = new ProdutosViewModel();

            if (String.IsNullOrWhiteSpace(Pesquisa))
            {
                produtosViewModel.Produtos = await _produtoRepository.GetProdutosWithDependencies();
                produtosViewModel.semDados = produtosViewModel.Produtos.Any() ? false : true;
                return View(produtosViewModel);
            }
            else if (!String.IsNullOrWhiteSpace(Pesquisa))
            {
                produtosViewModel.Produtos = await _produtoRepository.GetProdutosWithDependencies(Pesquisa);
                produtosViewModel.semDados = produtosViewModel.Produtos.Any() ? false : true;
                return View(produtosViewModel);
            }
            else
            {
                return View();
            }            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<UpdateQuantidadeResponse> UpdateQuantidade([FromBody]ItemPedido itemPedido)
        {
            return await _pedidoRepository.UpdateQuantidade(itemPedido);
        }
    }
}
