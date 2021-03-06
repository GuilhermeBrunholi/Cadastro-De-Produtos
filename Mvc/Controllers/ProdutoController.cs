using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dados;
using Dominio.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mvc.Controllers
{
    public class ProdutoController : Controller
    {
        private readonly ApplicationDbContext _contexto;

        public ProdutoController(ApplicationDbContext contexto)
        {
            _contexto = contexto;
        }

        // [HttpGet]
        // public IActionResult Index(string nome)
        // {
        //     if (!nome.Any())
        //     {
        //         return RedirectToAction("Index");
        //     }
        //     else
        //     {
        //         var produto = _contexto.Produtos.Select(p => p.Nome == nome);
        //         return View(produto.ToList());
        //     }
        // }
        public async Task<IActionResult> Index(string pesquisaNome)
        {
            var produto = from p in _contexto.Produtos
                          select p;

            if (!String.IsNullOrEmpty(pesquisaNome))
            {
                produto = _contexto.Produtos
                    .Where(p => p.Nome.Contains(pesquisaNome))
                    .Include(p => p.Categoria);
                return View(await produto.ToListAsync());
            }
            else
            {
                var queryDeProduto = _contexto.Produtos
                    .Include(p => p.Categoria)
                    .OrderBy(p => p.Categoria);

                if (!queryDeProduto.Any())
                    return View(new List<Produto>());

                return View(queryDeProduto.ToList());
            }
        }

        public async Task<IActionResult> Deletar(int id)
        {
            var produto = _contexto.Produtos.First(p => p.Id == id);
            _contexto.Produtos.Remove(produto);
            await _contexto.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public IActionResult Editar(int id)
        {
            ViewBag.Categorias = _contexto.Categorias.ToList();
            var produto = _contexto.Produtos.First(p => p.Id == id);
            return View("Salvar", produto);
        }

        [HttpGet]
        public IActionResult Salvar()
        {
            ViewBag.Categorias = _contexto.Categorias.ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Salvar(Produto modelo)
        {
            if (modelo.Id == 0)
            {
                _contexto.Produtos.Add(modelo);
            }
            else
            {
                var produto = _contexto.Produtos.First(p => p.Id == modelo.Id);
                produto.Nome = modelo.Nome;
                produto.CategoriaId = modelo.CategoriaId;
                produto.Quantidade = modelo.Quantidade;
                produto.Preco = modelo.Preco;
                produto.Codigo = modelo.Codigo;
            }
            await _contexto.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}