using System;

namespace Dominio.Entidades
{
    public class Produto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int Quantidade { get; set; }
        public float Preco { get; set; }
        public int Codigo { get; set; }
        public Categoria Categoria { get; set; }
        public int CategoriaId { get; set; }

    }
}