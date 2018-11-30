using Dominio.Entidades;
using Microsoft.AspNetCore.Mvc;
using Dados;
using System.Threading.Tasks;
using System.Linq;

namespace Mvc.Controllers
{
    public class CategoriaController : Controller
    {
        private readonly ApplicationDbContext _contexto;

        public CategoriaController(ApplicationDbContext contexto)
        {
            _contexto = contexto;
        }

        public IActionResult Index()
        {
            var categorias = _contexto.Categorias.ToList();
            return View(categorias);
        }

        public async Task<IActionResult> Deletar(int id)
        {
            var categoria = _contexto.Categorias.First(c => c.Id == id);
            _contexto.Categorias.Remove(categoria);
            await _contexto.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public IActionResult Editar(int id)
        {
            var categoria = _contexto.Categorias.First(c => c.Id == id);
            return View("Salvar", categoria);
        }

        [HttpGet]
        public IActionResult Salvar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Salvar(Categoria modelo)
        {
            if (modelo.Id == 0)
            {
                _contexto.Categorias.Add(modelo);
            }
            else
            {
                var categoria = _contexto.Categorias.First(c => c.Id == modelo.Id);
                categoria.Nome = modelo.Nome;
            }
            await _contexto.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}