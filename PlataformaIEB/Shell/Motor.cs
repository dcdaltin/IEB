using PlataformaIEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PlataformaIEB.Models
{

    public class Motor
    {

        public BaseDeConhecimento Base { get; set; }

        public List<Regra> Aplicadas { get; set; }

        public List<Regra> Aplicaveis { get; set; }

        public Motor(BaseDeConhecimento Base/*, List<Variavel> Valores*/)
        {

            Aplicadas = new List<Regra>();

            Aplicaveis = new List<Regra>();

            this.Base = Base;

            //foreach (var item in Base.Variaveis)
            //{
            //    item.Valores = null;
            //}

            //foreach (var inicial in Valores)
            //{
            //    try
            //    {
            //        Base.Variaveis.Where(o => o.Nome == inicial.Nome).SingleOrDefault().Valores = inicial.Valores;

            //    }
            //    catch (Exception){               
            //    }
            //}
        }

        private void Montar(Regra Regra)
        {
            double Conf = 0;

            bool Cond = false;
            
            if(Regra.Se.Operador == "=")
            {
                Cond = Regra.Se.Variavel.Valores.Select(o=>o.Valor).Contains(Regra.Se.Valor);
                Conf = Regra.Se.Variavel.Valores.Where(a=>a.Valor == Regra.Se.Valor).Select(o => o.Confianca).SingleOrDefault();
            }else 
            if (Regra.Se.Operador == "!=")
            {
                Cond = !Regra.Se.Variavel.Valores.Select(o => o.Valor).Contains(Regra.Se.Valor);
                Conf = Regra.Se.Variavel.Valores.Where(a => a.Valor == Regra.Se.Valor).Select(o => o.Confianca).SingleOrDefault();
            }
            else
            if (Regra.Se.Operador == ">")
            {
                Cond = Convert.ToDecimal(Regra.Se.Variavel.Valores.OrderByDescending(o=>o.Confianca).Select(o=>o.Valor).FirstOrDefault()) > Convert.ToDecimal(Regra.Se.Valor);
                Conf = Regra.Se.Variavel.Valores.OrderByDescending(o => o.Confianca).Select(o => o.Confianca).FirstOrDefault();
            }else
            if (Regra.Se.Operador == "<")
            {
                Cond = Convert.ToDecimal(Regra.Se.Variavel.Valores.OrderByDescending(o => o.Confianca).Select(o => o.Valor).FirstOrDefault()) < Convert.ToDecimal(Regra.Se.Valor);
                Conf = Regra.Se.Variavel.Valores.OrderByDescending(o => o.Confianca).Select(o => o.Confianca).FirstOrDefault();
            }else
            if (Regra.Se.Operador == ">=")
            {
                Cond = Convert.ToDecimal(Regra.Se.Variavel.Valores.OrderByDescending(o => o.Confianca).Select(o => o.Valor).FirstOrDefault()) >= Convert.ToDecimal(Regra.Se.Valor);
                Conf = Regra.Se.Variavel.Valores.OrderByDescending(o => o.Confianca).Select(o => o.Confianca).FirstOrDefault();
            }else
            if (Regra.Se.Operador == "<=")
            {
                Cond = Convert.ToDecimal(Regra.Se.Variavel.Valores.OrderByDescending(o => o.Confianca).Select(o => o.Valor).FirstOrDefault()) <= Convert.ToDecimal(Regra.Se.Valor);
                Conf = Regra.Se.Variavel.Valores.OrderByDescending(o => o.Confianca).Select(o => o.Confianca).FirstOrDefault();
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
            catch (Exception)
            {
                
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
                        Conf = Conf * (temp.Variavel.Valores.Where(a => a.Valor == temp.Valor).Select(o => o.Confianca).SingleOrDefault());
                    }
                    else if (temp.Operador == "!=")
                    {
                        Cond = Cond & !temp.Variavel.Valores.Select(o => o.Valor).Contains(temp.Valor);
                        Conf = Conf * temp.Variavel.Valores.Where(a => a.Valor == temp.Valor).Select(o => o.Confianca).SingleOrDefault();
                    }
                    else if (temp.Operador == ">")
                    {
                        Cond = Cond & Convert.ToDecimal(temp.Variavel.Valores.OrderByDescending(o => o.Confianca).Select(o => o.Valor).FirstOrDefault()) > Convert.ToDecimal(temp.Valor);
                        Conf = Conf * temp.Variavel.Valores.OrderByDescending(o => o.Confianca).Select(o => o.Confianca).FirstOrDefault();
                    }
                    else if (temp.Operador == "<")
                    {
                        Cond = Cond & Convert.ToDecimal(temp.Variavel.Valores.OrderByDescending(o => o.Confianca).Select(o => o.Valor).FirstOrDefault()) < Convert.ToDecimal(temp.Valor);
                        Conf = Conf * temp.Variavel.Valores.OrderByDescending(o => o.Confianca).Select(o => o.Confianca).FirstOrDefault();
                    }
                    else if (temp.Operador == ">=")
                    {
                        Cond = Cond & Convert.ToDecimal(temp.Variavel.Valores.OrderByDescending(o => o.Confianca).Select(o => o.Valor).FirstOrDefault()) >= Convert.ToDecimal(temp.Valor);
                        Conf = Conf * temp.Variavel.Valores.OrderByDescending(o => o.Confianca).Select(o => o.Confianca).FirstOrDefault();
                    }
                    else if (temp.Operador == "<=")
                    {
                        Cond = Cond & Convert.ToDecimal(temp.Variavel.Valores.OrderByDescending(o => o.Confianca).Select(o => o.Valor).FirstOrDefault()) <= Convert.ToDecimal(temp.Valor);
                        Conf = Conf * temp.Variavel.Valores.OrderByDescending(o => o.Confianca).Select(o => o.Confianca).FirstOrDefault();
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
                        Conf = Conf + temp.Variavel.Valores.Where(a => a.Valor == temp.Valor).Select(o => o.Confianca).SingleOrDefault()
                             - Conf * temp.Variavel.Valores.Where(a => a.Valor == temp.Valor).Select(o => o.Confianca).SingleOrDefault();
                    }
                    else if (temp.Operador == "!=")
                    {
                        Cond = Cond | !temp.Variavel.Valores.Select(o => o.Valor).Contains(temp.Valor);
                        Conf = Conf + temp.Variavel.Valores.Where(a => a.Valor == temp.Valor).Select(o => o.Confianca).SingleOrDefault()
                             - Conf * temp.Variavel.Valores.Where(a => a.Valor == temp.Valor).Select(o => o.Confianca).SingleOrDefault();
                    }
                    else if (temp.Operador == ">")
                    {
                        Cond = Cond | Convert.ToDecimal(temp.Variavel.Valores.OrderByDescending(o => o.Confianca).Select(o => o.Valor).FirstOrDefault()) > Convert.ToDecimal(temp.Valor);
                        Conf = Conf + temp.Variavel.Valores.Where(a => a.Valor == temp.Valor).Select(o => o.Confianca).SingleOrDefault()
                             - Conf * temp.Variavel.Valores.Where(a => a.Valor == temp.Valor).Select(o => o.Confianca).SingleOrDefault();
                    }
                    else if (temp.Operador == "<")
                    {
                        Cond = Cond | Convert.ToDecimal(temp.Variavel.Valores.OrderByDescending(o => o.Confianca).Select(o => o.Valor).FirstOrDefault()) < Convert.ToDecimal(temp.Valor);
                        Conf = Conf + temp.Variavel.Valores.Where(a => a.Valor == temp.Valor).Select(o => o.Confianca).SingleOrDefault()
                             - Conf * temp.Variavel.Valores.Where(a => a.Valor == temp.Valor).Select(o => o.Confianca).SingleOrDefault();
                    }
                    else if (temp.Operador == ">=")
                    {
                        Cond = Cond | Convert.ToDecimal(temp.Variavel.Valores.OrderByDescending(o => o.Confianca).Select(o => o.Valor).FirstOrDefault()) >= Convert.ToDecimal(temp.Valor);
                        Conf = Conf + temp.Variavel.Valores.Where(a => a.Valor == temp.Valor).Select(o => o.Confianca).SingleOrDefault()
                             - Conf * temp.Variavel.Valores.Where(a => a.Valor == temp.Valor).Select(o => o.Confianca).SingleOrDefault();
                    }
                    else if (temp.Operador == "<=")
                    {
                        Cond = Cond | Convert.ToDecimal(temp.Variavel.Valores.OrderByDescending(o => o.Confianca).Select(o => o.Valor).FirstOrDefault()) <= Convert.ToDecimal(temp.Valor);
                        Conf = Conf + temp.Variavel.Valores.Where(a => a.Valor == temp.Valor).Select(o => o.Confianca).SingleOrDefault()
                             - Conf * temp.Variavel.Valores.Where(a => a.Valor == temp.Valor).Select(o => o.Confianca).SingleOrDefault();
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
                Base.Regras.Remove(Regra);
                Parallel.ForEach(Regra.Entao, o => Entao(o.Acao,Conf));

            }
        }

        //public bool AplicaRegra()
        //{
        //    //var regras = Base.Regras;


        //    //foreach (var item in Aplicadas)
        //    //{
        //    //    regras.Remove(Base.Regras.Where(o => o.Nome == item.Nome).SingleOrDefault());
        //    //    if (regras.Count == 0)
        //    //        return false;
        //    //}

        //    foreach (var item in Base.Regras)
        //    {
                
        //        Montar(item);
            
        //        if (Cond)
        //        {
        //            Aplicadas.Add(item);

        //            Base.Regras.Remove(item);

        //            foreach (var valor in item.Entao)
        //            {

        //                if (valor.Acao.Confianca * Conf > 0.5)
        //                {
        //                    if (valor.Acao.Variavel.Valores.Select(o => o.Valor).Contains(valor.Acao.Valor))
        //                    {
        //                        var temp = valor.Acao.Variavel.Valores.Where(o => o.Valor == valor.Acao.Valor).SingleOrDefault();
        //                        temp.Confianca = temp.Confianca + Conf - (temp.Confianca * Conf);
        //                    }
        //                    else
        //                    {

        //                        var variavel = new ListaValores()
        //                        {
        //                            Variavel = valor.Acao.Variavel,
        //                            Valor = valor.Acao.Valor,
        //                            Confianca = valor.Acao.Confianca * Conf
        //                        };
        //                        valor.Acao.Variavel.Valores.Add(variavel);
                                

        //                    }

        //                    return true;
        //                }
        //            }
        //        }

        //    }

        //    return false;
        //}


        public bool LigarMotor()
        {
            Aplica();

            return true;
        }

        public void Aplica()
        {
            try
            {
                Parallel.ForEach(Base.Regras, o => Montar(o));
            }
            catch (Exception)
            {

               
            }
            


        }

        public void Entao(Acao Acao, double Conf) { 


                        if (Acao.Confianca * Conf > 0.5)
                        {
                            if (Acao.Variavel.Valores.Select(o => o.Valor).Contains(Acao.Valor))
                            {
                                var temp = Acao.Variavel.Valores.Where(o => o.Valor == Acao.Valor).SingleOrDefault();
                                temp.Confianca = temp.Confianca + Conf - (temp.Confianca * Conf);
                            }
                            else
                            {

                                var variavel = new ListaValores()
                                {
                                    Variavel = Acao.Variavel,
                                    Valor = Acao.Valor,
                                    Confianca = Acao.Confianca * Conf
                                };

                                Acao.Variavel.Valores.Add(variavel);


                            }

                            Aplica();
               
                        }            
        }


    }

}