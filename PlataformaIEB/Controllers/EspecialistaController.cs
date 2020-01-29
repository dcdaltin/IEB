using PlataformaIEB.Models;
using PlataformaIEB.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Migrations;
using PlataformaIEB.Filtros;
using System.Threading;
using System.Xml;
using System.Web.UI;

namespace PlataformaIEB.Controllers
{
    [LiberarAreas(2)]
    public class EspecialistaController : Controller
    {
        BancoDeDados dbSE = new BancoDeDados();

        public Usuario ProcuraUsuario()
        {

            string AutID = null;

            try
            {
                AutID = Request.Cookies["TokkenCookie"].Value;
            }
            catch (Exception)
            {

                return null; ;
            }
            

            return dbSE.Usuarios.Where(a => a.AutID == AutID).SingleOrDefault();

        }


        [HttpGet]
        public ActionResult Base()
        {
            VMBase VModel = new VMBase();
            Usuario usuario = ProcuraUsuario();
            if (usuario != null)
            {
                VModel.Bases = dbSE.Bases.Where(o => o.Usuario.Id == usuario.Id).ToList();
                VModel.Usuario = usuario.Id;
            }
            else return RedirectToAction("Login", "Autenticar", new { area = "" });
            return View(VModel);
        }

        [HttpPost]
        public ActionResult Base(VMBase Model)
        {
            if (ModelState.IsValid)
            {
                BaseDeConhecimento Base = new BaseDeConhecimento()
                {
                    Nome = Model.Nome,
                    Usuario = dbSE.Usuarios.Where(a => a.Id == Model.Usuario).SingleOrDefault()
                };

                dbSE.Bases.Add(Base);

                VarBase vinculaSexo = new VarBase { Variavel = dbSE.Variaveis.Where(o => o.Nome == "Sexo").SingleOrDefault(), Base = Base };
                dbSE.VarBase.Add(vinculaSexo);

                VarBase vinculaIdade = new VarBase { Variavel = dbSE.Variaveis.Where(o => o.Nome == "Idade").SingleOrDefault(), Base = Base };
                dbSE.VarBase.Add(vinculaIdade);

                dbSE.SaveChanges();

                return RedirectToAction("Variavel/" + Base.ID);

            }

            Model.Bases = dbSE.Bases.Where(o => o.Usuario.Id == Model.Usuario).ToList();
            return View(Model);
        }

        
        public ActionResult ExcluirBase(int ID)
        {
            BaseDeConhecimento Base = dbSE.Bases.Where(o => o.ID == ID).SingleOrDefault();
            ICollection<Variavel> Excluir = new List<Variavel>();

            dbSE.VarBase.RemoveRange(Base.Variaveis);

            if (Base.Regras != null)
            {
                foreach (var item in Base.Regras)
                {
                    dbSE.Cabecarios.Remove(item.Se);
                    dbSE.Cabecarios.RemoveRange(item.Conj.Select(a => a.Cabeca));
                    dbSE.ListConj.RemoveRange(item.Conj);

                    dbSE.Acoes.RemoveRange(item.Entao);
                }
                dbSE.Regras.RemoveRange(Base.Regras);
            }
            dbSE.Bases.Remove(Base);



            try
            {
                dbSE.SaveChanges();
                return RedirectToAction("Base");
            }       
            catch (Exception)
            {
                return JavaScript("alert('ERRO!');");
            }
        }

        public ActionResult ExcluirVar(int ID)
        {
            Variavel Variavel = dbSE.Variaveis.Where(o => o.ID == ID).SingleOrDefault();
            dbSE.VarBase.RemoveRange(Variavel.Base);

            try
            {
                dbSE.SaveChanges();
                return Redirect(Request.UrlReferrer.ToString());
            }
            catch (Exception)
            {
                return Redirect(Request.UrlReferrer.ToString());
            }
        }

        public List<string> CID(string termo)
        {
            List<string> Lista = new List<string>();
            string C10 = @"C:\PlataformaIEB\CID10\CID10.xml";
            string C0 = @"C:\PlataformaIEB\CID10\CID-O.xml";
            XmlDocument doc = new XmlDocument();
            doc.XmlResolver = new XmlUrlResolver();
            doc.Load(C0);
            var teste = doc.GetElementsByTagName("subcategoria");
            foreach (XmlNode item in teste)
            {
                var temp = item["nome"].InnerText;
                if (temp.ToLower().Contains(termo.ToLower())) Lista.Add(temp);
            }

            doc.Load(C10);
            teste = doc.GetElementsByTagName("subcategoria");
            foreach (XmlNode item in teste)
            {
                var temp = item["nome"].InnerText;
                if (temp.ToLower().Contains(termo.ToLower())) Lista.Add(temp);
            }

            return Lista;
        }

        public ActionResult ListaNomes(string term)
        {
            List<string> Nomes = new List<string>();
            Nomes.AddRange(dbSE.Variaveis.Select(o => o.Nome).Where(a=>a.ToLower().Contains(term.ToLower())));
            Nomes.AddRange(CID(term).Where(a => (Nomes.Count(b=> b==a ) < 1)));            
            //Nomes.RemoveAll(a => Nomes.Count(b => b == a) > 2);
            //Nomes = Nomes.Where(o=>o.ToUpper().Contains(term.ToUpper())).ToList();
            return Json(Nomes, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Variavel(int ID)
        {
            ViewBag.Base = dbSE.Bases.Where(a => a.ID == ID).Select(o=>o.Nome).SingleOrDefault();
            VMVar Modelo = new VMVar();
            Modelo.BaseID = ID;
            Modelo.Variaveis = dbSE.Variaveis.Where(o => o.Base.Select(a => a.Base.ID).Contains(ID)).ToList();

            return View(Modelo);
        }

        [HttpPost]
        public ActionResult Variavel(VMVar Modelo)
        {
            if (ModelState.IsValid)
            {
                Variavel var;

                try
                {
                    var = dbSE.Variaveis.Where(o => o.Nome == Modelo.Nome).Single();
                }
                catch (Exception)
                {
                    var = new Variavel();
                    var.Nome = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Modelo.Nome);
                    dbSE.Variaveis.AddOrUpdate(var);
                }

                VarBase referencia = new VarBase();
                referencia.Variavel = var;
                referencia.Base = dbSE.Bases.Where(a => a.ID == Modelo.BaseID).SingleOrDefault();
                referencia.Objetivo = Modelo.Obj;

                dbSE.VarBase.AddOrUpdate(referencia);

                try
                {
                    dbSE.SaveChanges();
                }
                catch (Exception)
                {
                    return RedirectToAction("Variavel/" + Modelo.BaseID); ;
                }


                return RedirectToAction("Variavel/" + Modelo.BaseID);
            }

            Modelo.Variaveis = dbSE.Variaveis.Where(o => o.Base.Select(a => a.Base.ID).Contains(Modelo.BaseID)).ToList();
            return View(Modelo);
        }

        [HttpGet]
        public ActionResult Regra(int ID)
        {

            ViewBag.Base = dbSE.Bases.Where(a => a.ID == ID).Select(o => o.Nome).SingleOrDefault();
            VMRegra Regra = new VMRegra();
            Regra.BaseID = ID;
            Regra.Regras = dbSE.Regras.Where(o => o.Base.ID == ID).ToList();
            Regra.Variaveis = dbSE.Variaveis.Where(o => o.Base.Select(a => a.Base.ID).Contains(ID)).Select(a => a.Nome).ToList();
            Regra.Operadores = new List<string>
            {
                "=","!=","<","<=",">",">="
            };
            return View(Regra);
        }

        [HttpPost]
        public ActionResult Regra(VMRegra Modelo)
        {
            if (ModelState.IsValid)
            {
                Cabecario Cabecario = new Cabecario();
                Cabecario.NOT = Modelo.Not;
                Cabecario.Operador = Modelo.Operador;
                Cabecario.Valor = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Modelo.Valor);
                Cabecario.Variavel = dbSE.Variaveis.Where(o => (o.Nome == Modelo.VarID)).SingleOrDefault();
                dbSE.Cabecarios.Add(Cabecario);

                Regra Regra = new Regra();
                Regra.Nome = Modelo.Nome;
                Regra.Se = Cabecario;
                Regra.Base = dbSE.Bases.Where(o => o.ID == Modelo.BaseID).SingleOrDefault();
                Regra.Se = Cabecario;

                dbSE.Regras.Add(Regra);
                dbSE.SaveChanges();

                return RedirectToAction("Cabeca/" + Regra.ID);

            }

            Modelo.Regras = dbSE.Regras.Where(o => o.Base.ID == Modelo.BaseID).ToList();
            Modelo.Variaveis = dbSE.Variaveis.Where(o => o.Base.Select(a => a.Base.ID).Contains(Modelo.BaseID)).Select(a => a.Nome).ToList();
            Modelo.Operadores = new List<string>
            {
                "=","!=","<","<=",">",">="
            };

            return View(Modelo);
        }

        public ActionResult ExcluirRegra(int ID)
        {
            Regra regra = dbSE.Regras.Where(a=> a.ID == ID).SingleOrDefault();
            var id = regra.Base.ID;
            dbSE.Cabecarios.Remove(regra.Se);
            dbSE.Cabecarios.RemoveRange(regra.Conj.Select(a => a.Cabeca));
            dbSE.ListConj.RemoveRange(regra.Conj);

            dbSE.Acoes.RemoveRange(regra.Entao);
            dbSE.Regras.Remove(regra);

            try
            {
                dbSE.SaveChanges();
                return RedirectToAction("Regra/"+id);
            }
            catch (Exception)
            {

                return Redirect(Request.UrlReferrer.ToString());
            }

        }


        [HttpGet]
        public ActionResult Cabeca(int ID)
        {
            VMRegra Modelo = new VMRegra();
            Modelo.Regra = dbSE.Regras.Where(o => o.ID == ID).SingleOrDefault();
            Modelo.BaseID = Modelo.Regra.Base.ID;
            Modelo.Variaveis = dbSE.Variaveis.Where(o => o.Base.Select(a => a.Base.ID).Contains(Modelo.BaseID)).Select(a => a.Nome).ToList();
            Modelo.Operadores = new List<string>
            {
                "=","!=","<","<=",">",">="
            };

            //Modelo.Pos = Modelo.Regra.Conj.Count();

            return View(Modelo);
        }

        [HttpPost]
        public ActionResult Cabeca(int ID, VMRegra Modelo)
        {
            if (ModelState.IsValid)
            {
                Modelo.Regra = dbSE.Regras.Where(a => a.ID == ID).SingleOrDefault();
                Cabecario Cabecario = new Cabecario();
                Cabecario.NOT = Modelo.Not;
                Cabecario.Operador = Modelo.Operador;
                Cabecario.Valor = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Modelo.Valor);
                Cabecario.Variavel = dbSE.Variaveis.Where(o => o.Nome == Modelo.VarID).SingleOrDefault();
                dbSE.Cabecarios.Add(Cabecario);
           


                if (Modelo.Conjuncao)
                {

                    ListaE E = new ListaE();
                    E.Cabeca = Cabecario;
                    E.Pos = Modelo.Regra.Conj.Count();
                    E.Regra = Modelo.Regra;

                    dbSE.ListE.Add(E);
                    Modelo.Regra.E.Add(E);
                    dbSE.Regras.AddOrUpdate(Modelo.Regra);
                    dbSE.SaveChanges();
                    return RedirectToAction("Cabeca/" + ID);
                }
                else
                {

                    ListaOU OU = new ListaOU();
                    OU.Cabeca = Cabecario;
                    OU.Pos = Modelo.Regra.Conj.Count();
                    OU.Regra = Modelo.Regra;

                    dbSE.ListOU.Add(OU);
                    Modelo.Regra.Ou.Add(OU);
                    dbSE.Regras.AddOrUpdate(Modelo.Regra);
                    dbSE.SaveChanges();
                    return RedirectToAction("Cabeca/" + ID);
                }
            }

            Modelo.Regra = dbSE.Regras.Where(a => a.ID == ID).SingleOrDefault();
            Modelo.Variaveis = dbSE.Variaveis.Where(o => o.Base.Select(a => a.Base.ID).Contains(Modelo.BaseID)).Select(a => a.Nome).ToList();
            Modelo.Operadores = new List<string>
            {
                "=","!=","<","<=",">",">="
            };

            
            return View(Modelo);
           // return RedirectToAction("Cabeca/" + ID);
        }

        public ActionResult ExcluirCabeca(int ID)
        {
            
            var lista = dbSE.ListConj.Where(a => a.CabecaID == ID).SingleOrDefault();
            var cabeca = dbSE.Cabecarios.Where(a => a.ID == ID).SingleOrDefault();
            var regra = dbSE.Regras.Where(a => a.ID == lista.RegraID).SingleOrDefault();
            var teste = regra.Conj.Where(a => a.Pos > lista.Pos);
            foreach (var item in teste)
            {
                item.Pos = item.Pos - 1;
            }
            
            dbSE.ListConj.Remove(lista);
            dbSE.Cabecarios.Remove(cabeca);
            dbSE.Regras.AddOrUpdate(regra);

            try
            {
                dbSE.SaveChanges();
                return Redirect(Request.UrlReferrer.ToString());
            }
            catch (Exception)
            {

                return Redirect(Request.UrlReferrer.ToString());
            }
        }
        [HttpGet]
        public ActionResult Entao(int ID)
        {
            VMEntao Modelo = new VMEntao();
            Modelo.RegraID = ID;
            Modelo.Regra = dbSE.Regras.Where(o => o.ID == ID).SingleOrDefault();
            Modelo.Variaveis = dbSE.Variaveis.Where(o => o.Base.Select(a => a.Base.ID).Contains(Modelo.Regra.Base.ID)).Select(a => a.Nome).ToList();
            Modelo.Conf = 100;
            
            return View(Modelo);
        }

        [HttpPost]
        public ActionResult Entao(int ID, VMEntao Modelo)
        {
            if (ModelState.IsValid)
            {
                Modelo.Regra = dbSE.Regras.Where(o => o.ID == Modelo.RegraID).SingleOrDefault();
                Modelo.Variaveis = dbSE.Variaveis.Where(o => o.Base.Select(a => a.Base.ID).Contains(Modelo.Regra.Base.ID)).Select(a => a.Nome).ToList();

                Acao Entao = new Acao();
                Entao.Regra = Modelo.Regra;
                Entao.Valor = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Modelo.Valor);
                Entao.Confianca = Modelo.Conf / 100;
                Entao.Variavel = dbSE.Variaveis.Where(a=>a.Nome == Modelo.Var).SingleOrDefault();

                dbSE.Acoes.Add(Entao);

                Modelo.Regra.Entao.Add(Entao);
                dbSE.Regras.AddOrUpdate(Modelo.Regra);

                dbSE.SaveChanges();

                return RedirectToAction("Entao/" + Modelo.RegraID);

            }

            Modelo.Regra = dbSE.Regras.Where(o => o.ID == ID).SingleOrDefault();
            Modelo.Variaveis = dbSE.Variaveis.Where(o => o.Base.Select(a => a.Base.ID).Contains(Modelo.Regra.Base.ID)).Select(a => a.Nome).ToList();
            return View(Modelo);
        }

        public ActionResult ExcluirAcao(string id)
        {

            var regra = Convert.ToInt16(id.Substring(0,id.IndexOf('-')));
            var var = Convert.ToInt16(id.Substring(id.IndexOf('-')+1));
            var acao = dbSE.Acoes.Where(a => a.RegraID == regra && a.VarID == var).FirstOrDefault();
            dbSE.Acoes.Remove(acao);

            try
            {
                dbSE.SaveChanges();
                return Redirect(Request.UrlReferrer.ToString());
            }
            catch (Exception)
            {

                return Redirect(Request.UrlReferrer.ToString());
            }
            
        }

        //public ActionResult Limpa(int ID)
        //{
        //    BaseDeConhecimento Base = dbSE.Bases.Where(o => o.ID == ID).SingleOrDefault();
        //    foreach (var item in Base.Variaveis)
        //    {
        //        dbSE.Valores.RemoveRange(item.Valores);
        //        item.Valores = null;
        //        dbSE.SaveChanges();
        //    }
        //    dbSE.Bases.AddOrUpdate(Base);
        //    dbSE.SaveChanges();

        //    return RedirectToAction("Entrada/" + ID);
        //}

        public ActionResult Teste()
        {
            var Tokken = Request.Cookies["TokkenCookie"].Value;
            Usuario usuario = dbSE.Usuarios.Where(a => a.AutID == Tokken).SingleOrDefault();
            List<ListaValores> Valores = new List<ListaValores>();
            var Variaveis = dbSE.Variaveis.Where(a => a.Base.Where(o => !o.Objetivo).Select(b => b.Base.Usuario.Id).Contains(usuario.Id));

            foreach (var item in Variaveis)
            {
                Valores.Add(new ListaValores() { Variavel = item, VariavelID = item.ID, Confianca = 100 });
            }

            VMConsulta Modelo = new VMConsulta() { Valores = Valores };
            return View(Modelo);
        }

        public ActionResult Resultado(Consulta Cons)
        {
            var Tokken = Request.Cookies["TokkenCookie"].Value;
            Usuario usuario = dbSE.Usuarios.Where(a => a.AutID == Tokken).SingleOrDefault();
            Buscador busca = new Buscador(Cons);
            busca.CriarAgentes();
            if (busca.Aplicadas.Count > 0)
            {
                VMTeste teste = new VMTeste();
                teste.Aplicadas = busca.Aplicadas.ToList();
                teste.Objetivos = dbSE.Variaveis.Where(a => a.Base.Where(b => b.Base.Usuario.Id == usuario.Id).Select(b => b.Objetivo).Contains(true)).ToList();
                return View(teste);
            }

            return RedirectToAction("CadConsulta");
        }

        //[HttpPost]
        //public ActionResult Entrada(VMEntrada Modelo)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        ListaValores Val = new ListaValores();
        //        Val.Confianca = Modelo.Conf / 100;
        //        Val.Valor = Modelo.Valor;
        //        Val.Variavel = dbSE.Variaveis.Where(o => (o.Base.ID == Modelo.Id) & (o.Nome == Modelo.Var)).SingleOrDefault();
        //        Val.Variavel.Valores.Add(Val);
        //        dbSE.Valores.Add(Val);
        //        dbSE.SaveChanges();

        //        return RedirectToAction("Entrada/" + Modelo.Id);

        //    }

        //    Modelo.Variaveis = dbSE.Variaveis.Where(o => o.Base.ID == Modelo.Id).ToList();
        //    return View(Modelo);
        //}



        //public ActionResult Resultado(int ID)
        //{
        //    BaseDeConhecimento Base = dbSE.Bases.Where(o => o.ID == ID).SingleOrDefault();
        //    Motor Motor = new Motor(Base);

        //    string Resposta = "";

        //    try
        //    {
        //        Motor.LigarMotor();

        //        foreach (var item in Motor.Base.Variaveis.Where(o => o.Objetivo))
        //        {
        //            if (item.Variavel != null)
        //            {
        //                Resposta = Resposta + "O Objetivo " + item.Variavel.Nome;

        //                foreach (var valor in item.Variavel.Valores)
        //                {
        //                    Resposta = Resposta + " recebeu o valor " + valor.Valor + " devido aos fatos: - ";
        //                    foreach (var regra in Motor.Aplicadas.Where(o => o.Entao.Where(a => (a.Valor == valor.Valor) & (a.Variavel == item.Variavel)) != null))
        //                    {
        //                        Resposta = Resposta + regra.Nome + " - ";
        //                    }

        //                }
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        Resposta = "Nenhum Objetivo Encontrado";
        //    }

        //    return Content(Resposta);
        //}

    }
}