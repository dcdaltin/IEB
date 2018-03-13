using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace PlataformaIEB.Models
{

    public class Buscador
    {
        private Consulta Consulta { get; set; }

        public ICollection<Regra> Aplicadas { get; set; }

        private static List<Cabecario> Cabecas { get; set; }

        private static ICollection<Agente> Agentes { get; set; }

        private bool flag = false;

        public Consulta GetConsulta() { return Consulta; }

        public Buscador(Consulta Cons)
        {
            Consulta = Cons;
            Cabecas = new List<Cabecario>();
            Aplicadas = new List<Regra>();
            Agentes = new List<Agente>();
        }

        private void AdicionaCabeca(Agente Agente)
        {
            var Add = Agente.ProcuraCabeca();
            if(Add!=null)
            lock (this)
            {
                Cabecas.AddRange(Add);
            };
            
        }

        private void NovoAgente(ListaValores Valor)
        {
            var Agente = new Agente(Valor);
            AdicionaCabeca(Agente);
            lock (this)
            {
                Agentes.Add(Agente);
            }
            
        }

        public void CriarAgentes()
        {
            Parallel.ForEach(Consulta.Variaveis, a => NovoAgente(a));
            Trabalhar();

        }

        private void Trabalhar()
        {
            Parallel.ForEach(Agentes, a =>
            {
                Montagem(a);
                a.RemoveRegras(Aplicadas);
            });
            if (flag)
            {
                flag = false;
                Trabalhar();
            }
        }

        private void Montagem(Agente Agente)
        {
            Parallel.ForEach(Agente.Regras,a=> Montar(a));
        }

        private void Montar(Regra Regra)
        {
            BancoDeDados db = new BancoDeDados();

            Regra = db.Regras.Where(a => a.ID == Regra.ID).SingleOrDefault();            

            bool resultado = Cabecas.Select(a=>a.ID).Contains(Regra.Se.ID);
            int cont, cont1, cont2;
            cont1 = Regra.E.Count;
            cont2 = Regra.Ou.Count;
            cont = cont1 + cont2;

            if (cont == 0)
                if (resultado)
                {
                    Aplicar(Regra);
                    return;
                }
                else return; ;           


            for (int i = 1; i <= cont; i++)
            {
                try
                {
                    var temp = Regra.E.Where(a => a.Pos == i).SingleOrDefault();
                    resultado = resultado && Cabecas.Contains(temp.Cabeca);
                }
                catch (Exception)
                {

                }

                try
                {
                    var temp = Regra.Ou.Where(a => a.Pos == i).SingleOrDefault();
                    resultado = resultado || Cabecas.Contains(temp.Cabeca);
                }
                catch (Exception)
                {

                }
            }

            if (resultado)
            {
                Aplicar(Regra);
            }

        }

        private void Aplicar(Regra Regra)
        {
            lock (this)
            {
                Aplicadas.Add(Regra);
                flag = true;
            }
            Parallel.ForEach(Regra.Entao, a => AplicaAção(a));

        }

        private void AplicaAção(Acao Acao)
        {
            ListaValores Nova = new ListaValores { VariavelID = Acao.Variavel.ID, Variavel=Acao.Variavel, Valor = Acao.Valor };
            NovoAgente(Nova);
        }


    }

    public class Agente
    {
        private ListaValores Valor { get; set; }
        public List<Regra> Regras { get; set; }

        public Agente(ListaValores Valor)
        {
            this.Valor = Valor;
            Regras = new List<Regra>();
        }

        public void RemoveRegras(ICollection<Regra> Excluir)
        {
            lock (this)
            {
                Regras.RemoveAll(a => Excluir.Select(o => o.ID).Contains(a.ID));
            }            
        }

        private bool TestaCabeca(Cabecario item)
        {
            bool resultado = Compara(item);
            if (resultado)
            {
                using (BancoDeDados db = new BancoDeDados())
                {
                    Regras.AddRange(db.Regras.Where(a => (a.Se.ID == item.ID
                                        | a.Ou.Select(o => o.CabecaID).Contains(item.ID)
                                        | a.E.Select(o => o.CabecaID).Contains(item.ID))).ToList());
                }
                
            }

            return resultado;
        }

        public ICollection<Cabecario> ProcuraCabeca()
        {
            List<Cabecario> Cabecas = new List<Cabecario>();
            ICollection<Cabecario> Excluir = new List<Cabecario>();
            using (BancoDeDados db = new BancoDeDados())
            {
                Cabecas.AddRange(db.Cabecarios.Where(a => a.Variavel.ID == Valor.VariavelID).ToList());                
            }
            
            Parallel.ForEach(Cabecas, a => 
            {
                if (!TestaCabeca(a))
                {
                    Excluir.Add(a);                    
                }                
            });

            Cabecas.RemoveAll(a=>Excluir.Select(o=>o.ID).Contains(a.ID));
            return Cabecas;
        }


        private bool Compara(Cabecario Cabeca)
        {
            decimal v1, v2;
            bool result = false;

            switch (Cabeca.Operador)
            {
                case "=":

                    if (Cabeca.Valor == Valor.Valor)
                        result = true;
                    break;
                case "!=":
                    if (Cabeca.Valor != Valor.Valor)
                        result = true;
                    break;
                default:
                    try
                    {
                        v1 = Convert.ToDecimal(Cabeca.Valor);
                        v2 = Convert.ToDecimal(Valor.Valor);
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                    switch (Cabeca.Operador)
                    {
                        case ">":
                            if (v2 > v1)
                                result = true;
                            break;
                        case "<":
                            if (v2 < v1)
                                result = true;
                            break;
                        case "<=":
                            if (v2 <= v1)
                                result = true;
                            break;
                        case ">=":
                            if (v2 >= v1)
                                result = true;
                            break;
                    }
                    break;
            }

            if (Cabeca.NOT) return !result;

            return result;
            
        }
    }
}