using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlataformaIEB.Models
{

    public class Buscador
    {
        private Consulta Consulta { get; set; }

        public ICollection<Regra> Aplicadas { get; set; }

        private static List<Tuple<Cabecario, double>> Cabecas { get; set; }

        private static ICollection<Agente> Agentes { get; set; }

        private bool flag = false;

        public Consulta GetConsulta() { return Consulta; }

        public Buscador(Consulta Cons)
        {
            Consulta = Cons;
            Cabecas = new List<Tuple<Cabecario, double>>();
            Aplicadas = new List<Regra>();
            Agentes = new List<Agente>();
        }

        private void AdicionaCabeca(Agente Agente)
        {
            ICollection<Tuple<Cabecario, double>> Add = Agente.ProcuraCabeca();
            if (Add != null)
            {
                lock (this)
                {
                    Cabecas.AddRange(Add);
                }
            };

        }

        private void NovoAgente(ListaValores Valor)
        {
            Agente Agente = new Agente(Valor);
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

        private void Montagem(Agente agente)
        {
            Parallel.ForEach(agente.Regras, a => Montar(a));
        }

        private void Montar(Regra Regra)
        {
            BancoDeDados db = new BancoDeDados();

            Regra = db.Regras.Where(a => a.ID == Regra.ID).SingleOrDefault();


            double confianca = 1;

            bool resultado = Cabecas.Select(a => a.Item1.ID).Contains(Regra.Se.ID);

            if (resultado)
            {
                confianca = Cabecas.Where(a => a.Item1.ID == Regra.Se.ID).SingleOrDefault().Item2;
            }

            int cont = Regra.E.Count + Regra.Ou.Count;

            if (cont == 0)
            {
                if (resultado)
                {
                    Aplicar(Regra, confianca);
                    return;
                }
                else
                {
                    return;
                }
            };


            for (int i = 1; i <= cont; i++)
            {
                try
                {
                    ListaE temp = Regra.E.Where(a => a.Pos == i).SingleOrDefault();
                    resultado = resultado && Cabecas.Select(a => a.Item1.ID).Contains(temp.Cabeca.ID);
                    if (resultado)
                    {
                        confianca *= Cabecas.Where(a => a.Item1.ID == temp.Cabeca.ID).SingleOrDefault().Item2;
                    }
                }
                catch (Exception)
                {

                }

                try
                {
                    ListaOU temp = Regra.Ou.Where(a => a.Pos == i).SingleOrDefault();
                    resultado = resultado || Cabecas.Select(a => a.Item1.ID).Contains(temp.Cabeca.ID);
                    double aux = Cabecas.Where(a => a.Item1.ID == temp.Cabeca.ID).SingleOrDefault().Item2;
                    if (resultado)
                    {
                        confianca = (confianca + aux) - (confianca * aux);
                    }
                }
                catch (Exception)
                {

                }
            }

            if (resultado)
            {
                Aplicar(Regra, confianca);
            }

        }

        private void Aplicar(Regra Regra, double confianca)
        {
            lock (this)
            {
                Aplicadas.Add(Regra);
                flag = true;
            }

            Parallel.ForEach(Regra.Entao, a => AplicaAção(a, confianca));
        }

        private void AplicaAção(Acao Acao, double confianca)
        {
            ListaValores Nova = new ListaValores { VariavelID = Acao.Variavel.ID, Variavel = Acao.Variavel, Valor = Acao.Valor, Confianca = confianca };
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

        public ICollection<Tuple<Cabecario, double>> ProcuraCabeca()
        {
            List<Cabecario> Cabecas = new List<Cabecario>();
            ICollection<Cabecario> Excluir = new List<Cabecario>();
            ICollection<Tuple<Cabecario, double>> resultado = new List<Tuple<Cabecario, double>>();
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

            Cabecas.RemoveAll(a => Excluir.Select(o => o.ID).Contains(a.ID));

            Cabecas.ForEach(a => resultado.Add(new Tuple<Cabecario, double>(a, Valor.Confianca)));

            return resultado;
        }


        private bool Compara(Cabecario Cabeca)
        {
            decimal v1, v2;
            bool result = false;

            switch (Cabeca.Operador)
            {
                case "=":

                    if (Cabeca.Valor == Valor.Valor)
                    {
                        result = true;
                    }

                    break;
                case "!=":
                    if (Cabeca.Valor != Valor.Valor)
                    {
                        result = true;
                    }

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
                            {
                                result = true;
                            }

                            break;
                        case "<":
                            if (v2 < v1)
                            {
                                result = true;
                            }

                            break;
                        case "<=":
                            if (v2 <= v1)
                            {
                                result = true;
                            }

                            break;
                        case ">=":
                            if (v2 >= v1)
                            {
                                result = true;
                            }

                            break;
                    }
                    break;
            }

            if (Cabeca.NOT)
            {
                return !result;
            }

            return result;

        }
    }
}