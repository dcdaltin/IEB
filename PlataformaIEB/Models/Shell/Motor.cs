using PlataformaIEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlataformaIEB.Models
{

    public class Motor
    {
        public ICollection<ListaValores> Valores { get; set; }

        public List<ListaValores> Resultados { get; set; }

        public List<Regra> Aplicadas { get; set; }

        public List<Regra> Regras { get; set; }

        public Motor(ICollection<ListaValores> valores)
        {
            this.Valores = valores;
            Aplicadas = new List<Regra>();
            BancoDeDados db = new BancoDeDados();
            var Regras = db.Regras.ToList();

        }

        private void Montar(Regra Regra)
        {

            bool Cond = false;
            
            if(Regra.Se.Operador == "=")
            {
                Cond = Regra.Se.Variavel.Valores.Select(o=>o.Valor).Contains(Regra.Se.Valor);
            }else 
            if (Regra.Se.Operador == "!=")
            {
                Cond = !Regra.Se.Variavel.Valores.Select(o => o.Valor).Contains(Regra.Se.Valor);
            }
            else
            if (Regra.Se.Operador == ">")
            {
                Cond = Convert.ToDecimal(Regra.Se.Variavel.Valores.OrderByDescending(o=>o.Confianca).Select(o=>o.Valor).FirstOrDefault()) > Convert.ToDecimal(Regra.Se.Valor);
            }else
            if (Regra.Se.Operador == "<")
            {
                Cond = Convert.ToDecimal(Regra.Se.Variavel.Valores.OrderByDescending(o => o.Confianca).Select(o => o.Valor).FirstOrDefault()) < Convert.ToDecimal(Regra.Se.Valor);
            }else
            if (Regra.Se.Operador == ">=")
            {
                Cond = Convert.ToDecimal(Regra.Se.Variavel.Valores.OrderByDescending(o => o.Confianca).Select(o => o.Valor).FirstOrDefault()) >= Convert.ToDecimal(Regra.Se.Valor);
            }else
            if (Regra.Se.Operador == "<=")
            {
                Cond = Convert.ToDecimal(Regra.Se.Variavel.Valores.OrderByDescending(o => o.Confianca).Select(o => o.Valor).FirstOrDefault()) <= Convert.ToDecimal(Regra.Se.Valor);
            }
            if (Regra.Se.NOT)
            {
                Cond = !Cond;
            }

            int max = 0;
            int Pos = 0;
            try
            {
                Pos = Regra.Ou.OrderByDescending(o => o.Pos).FirstOrDefault().Pos;
                if (Pos > max)
                {
                    max = Pos;
                }

            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine(e);
            }

            try
            {
                Pos = Regra.E.OrderByDescending(o => o.Pos).FirstOrDefault().Pos;
                if (Pos > max)
                {
                    max = Pos;
                }
            }
            catch (Exception)
            {

            }



            for (int i = 1; i <= max; i++)
            {
                Cabecario temp = new Cabecario();

                try
                {
                    temp = Regra.E.Where(o => o.Pos == i).Select(o => o.Cabeca).SingleOrDefault();
                }
                catch (Exception)
                {

                    temp = null;
                }
                
  


                if (temp != null)
                {
                    if (temp.Operador == "=")
                    {
                        Cond = Cond & temp.Variavel.Valores.Select(o => o.Valor).Contains(temp.Valor);
                    }
                    else if (temp.Operador == "!=")
                    {
                        Cond = Cond & !temp.Variavel.Valores.Select(o => o.Valor).Contains(temp.Valor);
                    }
                    else if (temp.Operador == ">")
                    {
                        Cond = Cond & Convert.ToDecimal(temp.Variavel.Valores.OrderByDescending(o => o.Confianca).Select(o => o.Valor).FirstOrDefault()) > Convert.ToDecimal(temp.Valor);
                    }
                    else if (temp.Operador == "<")
                    {
                        Cond = Cond & Convert.ToDecimal(temp.Variavel.Valores.OrderByDescending(o => o.Confianca).Select(o => o.Valor).FirstOrDefault()) < Convert.ToDecimal(temp.Valor);
                    }
                    else if (temp.Operador == ">=")
                    {
                        Cond = Cond & Convert.ToDecimal(temp.Variavel.Valores.OrderByDescending(o => o.Confianca).Select(o => o.Valor).FirstOrDefault()) >= Convert.ToDecimal(temp.Valor);
                    }
                    else if (temp.Operador == "<=")
                    {
                        Cond = Cond & Convert.ToDecimal(temp.Variavel.Valores.OrderByDescending(o => o.Confianca).Select(o => o.Valor).FirstOrDefault()) <= Convert.ToDecimal(temp.Valor);
                    }
                    if (temp.NOT)
                    {
                        Cond = !Cond;
                    }
                }

                try
                {
                    temp = Regra.Ou.Where(o => o.Pos == i).Select(o => o.Cabeca).SingleOrDefault();
                }
                catch (Exception)
                {

                    temp = null;
                }
                

                if (temp != null)
                {
                    if (temp.Operador == "=")
                    {
                        Cond = Cond | temp.Variavel.Valores.Select(o => o.Valor).Contains(temp.Valor);
                    }
                    else if (temp.Operador == "!=")
                    {
                        Cond = Cond | !temp.Variavel.Valores.Select(o => o.Valor).Contains(temp.Valor);
                    }
                    else if (temp.Operador == ">")
                    {
                        Cond = Cond | Convert.ToDecimal(temp.Variavel.Valores.OrderByDescending(o => o.Confianca).Select(o => o.Valor).FirstOrDefault()) > Convert.ToDecimal(temp.Valor);
                    }
                    else if (temp.Operador == "<")
                    {
                        Cond = Cond | Convert.ToDecimal(temp.Variavel.Valores.OrderByDescending(o => o.Confianca).Select(o => o.Valor).FirstOrDefault()) < Convert.ToDecimal(temp.Valor);
                    }
                    else if (temp.Operador == ">=")
                    {
                        Cond = Cond | Convert.ToDecimal(temp.Variavel.Valores.OrderByDescending(o => o.Confianca).Select(o => o.Valor).FirstOrDefault()) >= Convert.ToDecimal(temp.Valor);
                    }
                    else if (temp.Operador == "<=")
                    {
                        Cond = Cond | Convert.ToDecimal(temp.Variavel.Valores.OrderByDescending(o => o.Confianca).Select(o => o.Valor).FirstOrDefault()) <= Convert.ToDecimal(temp.Valor);
                    }
                    if (temp.NOT)
                    {
                        Cond = !Cond;
                    }
                }

            }

            if (Cond)
            {
                Aplicadas.Add(Regra);
                foreach (var item in Regra.Entao)
                {
                    Entao(item);
                }

            }
        }


        public bool LigarMotor()
        {
            Aplica();

            return true;
        }

        public void Aplica()
        {



        }

        public void Entao(Acao Acao)
        {
            var variavel = new ListaValores()
            {
                Variavel = Acao.Variavel,
                Valor = Acao.Valor,
            };



            Aplica();



        }


    }

}