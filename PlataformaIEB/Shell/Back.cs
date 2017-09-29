using PlataformaIEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PlataformaIEB.Shell
{
    public class Back
    {
        BancoDeDados db = new BancoDeDados();

        public List<Regra> Aplicaveis { get; set; }

        public Back()
        {

        }
        public void Verificar(Variavel objetivo)
        {
            Aplicaveis = db.Regras.Where(a=>a.Entao.Select(b=>b.Acao.Variavel).Contains(objetivo)).OrderByDescending(a=>a.Entao.Select(b=>b.Acao.Confianca)).ToList();
            Parallel.ForEach(Aplicaveis);
        }
    }
}