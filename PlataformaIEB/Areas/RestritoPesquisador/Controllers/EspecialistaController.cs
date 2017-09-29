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

namespace PlataformaIEB.Areas.RestritoPesquisador.Controllers
{
    [LiberarAreas("Pesquisador")]
    public class EspecialistaController : Controller
    {
        BancoDeDados dbSE = new BancoDeDados();

        public Usuario ProcuraUsuario()
        {
            DatabaseContext db = new DatabaseContext();

            var AutID = Request.Cookies["TokkenCookie"].Value;

            if (db.Pesquisadores.Where(a => a.AutID == AutID).FirstOrDefault() != null)
            {
                return db.Pesquisadores.Where(a => a.AutID == AutID).SingleOrDefault();
            }


            return null;

        }


        [HttpGet]
        public ActionResult Base()
        {
            VMBase VModel = new VMBase();
            Usuario usuario = ProcuraUsuario();
            VModel.Bases = dbSE.Bases.Where(o => o.UsuarioID == usuario.Id).ToList();
            VModel.Usuario = usuario.Id;

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
                    UsuarioID = Model.Usuario
                };
                dbSE.Bases.Add(Base);
                dbSE.SaveChanges();

                return RedirectToAction("Variavel/"+Base.ID);

            }

            Model.Bases = dbSE.Bases.Where(o => o.UsuarioID == Model.Usuario).ToList();
            return View(Model);
        }

        [HttpGet]
        public ActionResult EditBase(int ID)
        {
            BaseDeConhecimento Base = dbSE.Bases.Where(o => o.ID == ID).SingleOrDefault();
            return View(Base);
        }

        [HttpPost]
        public ActionResult EditBase(BaseDeConhecimento Base)
        {
            if (ModelState.IsValid)
            {
                dbSE.Bases.AddOrUpdate(Base);
                dbSE.SaveChanges();
                return RedirectToAction("Base");
            }

            return View(Base);
        }

        [HttpGet]
        public ActionResult Variavel(int ID)
        {
            VMVar Modelo = new VMVar();
            Modelo.BaseID = ID;
            Modelo.Variaveis = dbSE.Variaveis.Where(o => o.Base.ID == ID).ToList();
            return View(Modelo);
        }

        [HttpPost]
        public ActionResult Variavel(VMVar Modelo)
        {
            if (ModelState.IsValid)
            {
                Variavel var = new Variavel();
                var.Base = dbSE.Bases.Where(o => o.ID == Modelo.BaseID).SingleOrDefault();
                var.Nome = Modelo.Nome;
                var.Objetivo = Modelo.Obj;

                dbSE.Variaveis.Add(var);
                dbSE.SaveChanges();
                return RedirectToAction("Variavel/" + Modelo.BaseID);
            }

            Modelo.Variaveis = dbSE.Variaveis.Where(o => o.Base.ID == Modelo.BaseID).ToList();
            return View(Modelo);
        }

        [HttpGet]
        public ActionResult Regra(int ID)
        {
            VMRegra Regra = new VMRegra();
            Regra.BaseID = ID;
            Regra.Regras = dbSE.Regras.Where(o => o.Base.ID == ID).Select(a=>a.Nome).ToList();
            Regra.Variaveis = dbSE.Variaveis.Where(o => o.Base.ID == ID).Select(a => a.Nome).ToList();
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
                Cabecario.Valor = Modelo.Valor;
                Cabecario.Variavel = dbSE.Variaveis.Where(o => (o.Nome == Modelo.VarID) & (o.Base.ID == Modelo.BaseID)).SingleOrDefault();
                dbSE.Cabecarios.Add(Cabecario);

                Regra Regra = new Regra();
                Regra.Nome = Modelo.Nome;
                Regra.Se = Cabecario;
                Regra.Base = dbSE.Bases.Where(o=>o.ID == Modelo.BaseID).SingleOrDefault();
                Regra.Se = Cabecario;

                dbSE.Regras.Add(Regra);
                dbSE.SaveChanges();

                return RedirectToAction("Cabeca/" + Regra.ID);

            }

            Modelo.Regras = dbSE.Regras.Where(o => o.Base.ID == Modelo.BaseID).Select(a => a.Nome).ToList();
            Modelo.Variaveis = dbSE.Variaveis.Where(o => o.Base.ID == Modelo.BaseID).Select(a => a.Nome).ToList();
            Modelo.Operadores = new List<string>
            {
                "=","!=","<","<=",">",">="
            };

            return View(Modelo);
        }

        [HttpGet]
        public ActionResult Cabeca(int ID)
        {
            VMRegra Modelo = new VMRegra();
            Modelo.BaseID = ID;
            Modelo.Regra = dbSE.Regras.Where(o => o.ID == ID).SingleOrDefault();
            Modelo.Variaveis = dbSE.Variaveis.Where(o => (o.Base.ID == Modelo.Regra.Base.ID)).Select(a => a.Nome).ToList();
            Modelo.Operadores = new List<string>
            {
                "=","!=","<","<=",">",">="
            };

            var max = 0;
            int Pos = 0;
            try
            {
                Pos = Modelo.Regra.Ou.OrderByDescending(o => o.Pos).FirstOrDefault().Pos;
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
                Pos = Modelo.Regra.E.OrderByDescending(o => o.Pos).FirstOrDefault().Pos;
                if (Pos > max)
                {
                    max = Pos;
                }
            }
            catch (Exception)
            {


            }

            Modelo.Pos = max;

            return View(Modelo);
        }

        [HttpPost]
        public ActionResult Cabeca(VMRegra Modelo)
        {
            if (ModelState.IsValid)
            {
                Modelo.Regra = dbSE.Regras.Where(o => o.ID == Modelo.BaseID).SingleOrDefault();
                Cabecario Cabecario = new Cabecario();
                Cabecario.NOT = Modelo.Not;
                Cabecario.Operador = Modelo.Operador;
                Cabecario.Valor = Modelo.Valor;
                Cabecario.Variavel = dbSE.Variaveis.Where(o => (o.Nome == Modelo.VarID) && (o.Base.ID == Modelo.Regra.Base.ID)).SingleOrDefault();
                dbSE.Cabecarios.Add(Cabecario);


                if (Modelo.E)
                {
                    ListaE E = new ListaE();
                    E.Cabeca = Cabecario;
                    E.Pos = Modelo.Pos+1;
                    E.Regra = Modelo.Regra;

                    dbSE.ListE.Add(E);
                    Modelo.Regra.E.Add(E);
                    dbSE.Regras.AddOrUpdate(Modelo.Regra);
                    dbSE.SaveChanges();
                    return RedirectToAction("Cabeca/" + Modelo.BaseID);
                }

                if (Modelo.OU)
                {
                    ListaOU OU = new ListaOU();
                    OU.Cabeca = Cabecario;
                    OU.Pos = Modelo.Pos + 1;
                    OU.Regra = Modelo.Regra;

                    dbSE.ListOU.Add(OU);
                    Modelo.Regra.Ou.Add(OU);
                    dbSE.Regras.AddOrUpdate(Modelo.Regra);
                    dbSE.SaveChanges();
                    return RedirectToAction("Cabeca/" + Modelo.BaseID);
                }
            }

            Modelo.Variaveis = dbSE.Variaveis.Where(o => o.Base.ID == Modelo.Regra.Base.ID).Select(a => a.Nome).ToList();
            Modelo.Operadores = new List<string>
            {
                "=","!=","<","<=",">",">="
            };
            return View(Modelo);
        }

        [HttpGet]
        public ActionResult Entao(int ID)
        {
            VMEntao Modelo = new VMEntao();
            Modelo.RegraID = ID;
            Modelo.Regra = dbSE.Regras.Where(o => o.ID == ID).SingleOrDefault();
            Modelo.Variaveis = dbSE.Variaveis.Where(o => o.Base.ID == Modelo.Regra.Base.ID).Select(a => a.Nome).ToList();

            Modelo.Pos = 0;
            int Pos = 0;
            try
            {
                Pos = Modelo.Regra.Ou.OrderByDescending(o => o.Pos).FirstOrDefault().Pos;
                if (Pos > Modelo.Pos)
                {
                    Modelo.Pos = Pos;
                }
            }
            catch (Exception){}
            try
            {
                Pos = Modelo.Regra.E.OrderByDescending(o => o.Pos).FirstOrDefault().Pos;
                if (Pos > Modelo.Pos)
                {
                    Modelo.Pos = Pos;
                }
            }
            catch (Exception){}


            return View(Modelo);
        }

        [HttpPost]
        public ActionResult Entao(VMEntao Modelo)
        {
            if (ModelState.IsValid)
            {
                Modelo.Regra = dbSE.Regras.Where(o => o.ID == Modelo.RegraID).SingleOrDefault();
                ListaEntao LEntao = new ListaEntao();
                Acao Entao = new Acao();
                Entao.Variavel = dbSE.Variaveis.Where(o => (o.Nome == Modelo.Var) & (o.Base.ID == Modelo.Regra.Base.ID)).SingleOrDefault();
                Entao.Valor = Modelo.Valor;
                Entao.Confianca = Modelo.Conf/100;
                dbSE.Acoes.Add(Entao);

                LEntao.Acao = Entao;
                LEntao.Regra = Modelo.Regra;
                dbSE.ListEntao.Add(LEntao);

                Modelo.Regra.Entao.Add(LEntao);
                dbSE.Regras.AddOrUpdate(Modelo.Regra);

                dbSE.SaveChanges();

                return RedirectToAction("Entao/" + Modelo.RegraID);

            }

            Modelo.Variaveis = dbSE.Variaveis.Where(o => o.Base.ID == Modelo.Regra.Base.ID).Select(a => a.Nome).ToList();
            return View(Modelo);
        }

        public ActionResult Limpa(int ID)
        {
            BaseDeConhecimento Base = dbSE.Bases.Where(o => o.ID == ID).SingleOrDefault();
            foreach (var item in Base.Variaveis)
            {
                dbSE.Valores.RemoveRange(item.Valores);
                item.Valores = null;
                dbSE.SaveChanges();
            }
            dbSE.Bases.AddOrUpdate(Base);
            dbSE.SaveChanges();

            return RedirectToAction("Entrada/" + ID);
        }

        [HttpGet]
        public ActionResult Entrada(int ID)
        {
            VMEntrada Modelo = new VMEntrada();
            Modelo.Id = ID;
            Modelo.Variaveis = dbSE.Variaveis.Where(o => (o.Base.ID == ID) & !(o.Objetivo)).ToList();

            return View(Modelo);
        }

        [HttpPost]
        public ActionResult Entrada(VMEntrada Modelo)
        {
            if (ModelState.IsValid)
            {
                ListaValores Val = new ListaValores();
                Val.Confianca = Modelo.Conf / 100;
                Val.Valor = Modelo.Valor;
                Val.Variavel = dbSE.Variaveis.Where(o => (o.Base.ID == Modelo.Id) & (o.Nome == Modelo.Var)).SingleOrDefault();
                Val.Variavel.Valores.Add(Val);
                dbSE.Valores.Add(Val);
                dbSE.SaveChanges();

                return RedirectToAction("Entrada/" + Modelo.Id);

            }

            Modelo.Variaveis = dbSE.Variaveis.Where(o => o.Base.ID == Modelo.Id).ToList();
            return View(Modelo);
        }



        public ActionResult Resultado(int ID)
        {
            BaseDeConhecimento Base = dbSE.Bases.Where(o => o.ID == ID).SingleOrDefault();
            Motor Motor = new Motor(Base);

            string Resposta = "";

            try
            {
                Motor.LigarMotor();                

                foreach (var item in Motor.Base.Variaveis.Where(o => o.Objetivo))
                {
                    if (item.Valores.Count() != 0)
                    {
                        Resposta = Resposta + "O Objetivo " + item.Nome;

                        foreach (var valor in item.Valores)
                        {
                            Resposta = Resposta + " recebeu o valor " + valor.Valor + " devido aos fatos: - ";
                            foreach (var regra in Motor.Aplicadas.Where(o => o.Entao.Where(a => (a.Acao.Valor == valor.Valor) & (a.Acao.Variavel == item)) != null))
                            {
                                Resposta = Resposta + regra.Nome + " - ";
                            }

                        }
                    }
                }
            }
            catch (Exception)
            {

                Resposta = "Nenhum Objetivo Encontrado";
            }

            return Content(Resposta);
        }
    }
}