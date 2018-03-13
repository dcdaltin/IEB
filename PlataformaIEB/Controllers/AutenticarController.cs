using PlataformaIEB.Models;
using PlataformaIEB.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace PlataformaIEB.Controllers
{
    public class AutenticarController : Controller
    {
        
        BancoDeDados db = new BancoDeDados();

        public ActionResult Login()
        {
            VMLogin login = new VMLogin();
            return View("Login",login);

        }

        private bool VerificaSenha(VMLogin login, Usuario usuario)
        {
            if (!Crypto.VerifyHashedPassword(usuario.Senha, login.Senha))
            {
                return false;
            }

            //Gera Aleatório Cadastro Usuario (TOkken)
            string autoriza = Guid.NewGuid().ToString();

            //Salva Tokken no Banco
            usuario.AutID = autoriza;
            db.SaveChanges();

            //Cria Cookie da identificação
            var CookieTokken = new HttpCookie("TokkenCookie");
            CookieTokken.Expires = DateTime.Now.AddDays(1);
            CookieTokken.Value = autoriza;
            Response.Cookies.Add(CookieTokken);
            Session["Tokken"] = CookieTokken;

            //Cria Cookie de nome de Usuário
            var CookieNome = new HttpCookie("CookieNome");
            CookieNome.Expires = DateTime.Now.AddDays(1);
            CookieNome.Value = usuario.Nome;
            Response.Cookies.Add(CookieNome);
            Session["Tokken"] = CookieNome;


            //Cria Cookie de tipo de Usuário
            var CookieTipo = new HttpCookie("CookieTipo");
            CookieTipo.Expires = DateTime.Now.AddDays(1);
            CookieTipo.Value = ProcuraUsuario(autoriza).ToString();
            Response.Cookies.Add(CookieTipo);
            Session["Tokken"] = CookieTipo;

            return true;
        }


        private Tipo ProcuraUsuario(string AutID)
        {
            if (db.Medicos.Where(b => b.AutID == AutID).FirstOrDefault() != null)
            {
                return Tipo.Medico;
            }
            if (db.Admins.Where(a => a.AutID == AutID).FirstOrDefault() != null)
            {
                return Tipo.ADMIN;
            }
            if (db.Pacientes.Where(c => c.AutID == AutID).FirstOrDefault() != null)
            {
                return Tipo.Paciente;
            }
            if (db.Pesquisadores.Where(c => c.AutID == AutID).FirstOrDefault() != null)
            {
                return Tipo.Pesquisador;
            }

            return Tipo.Paciente;
        }

        public enum Tipo { Pesquisador, Medico, Paciente, ADMIN}

        [HttpPost]
        public ActionResult Login(VMLogin login)
        {
            if (ModelState.IsValid)
            {
                Usuario usuario = db.Usuarios.Where(a => login.Email == a.Email).SingleOrDefault();

                if (VerificaSenha(login, usuario))
                {
                    return RedirectToAction("Principal", "Home", new { area = "" });
                }

                ModelState.AddModelError("", "Falha no Login! Favor entrar em contato");
                return View(login);
            }

            ModelState.AddModelError("", "Falha no Login! Favor entrar em contato");
            return View(login);

        }

        public ActionResult Logout()
        {
            if (Request.Cookies.AllKeys.Contains("TokkenCookie"))
            {
                String AutID = Request.Cookies["TokkenCookie"].Value;
                Usuario usuario = db.Usuarios.Where(a=>a.AutID == AutID).SingleOrDefault();
                if(usuario != null)
                {
                    usuario.AutID = null;
                    db.SaveChanges();
                }

                Request.Cookies["TokkenCookie"].Value = "";
                Request.Cookies["TokkenCookie"].Expires = DateTime.Now;
                Response.SetCookie(Request.Cookies["TokkenCookie"]);

                Request.Cookies["CookieNome"].Value = "";
                Request.Cookies["CookieNome"].Expires = DateTime.Now;
                Response.SetCookie(Request.Cookies["CookieNome"]);

                Request.Cookies["CookieTipo"].Value = "";
                Request.Cookies["CookieTipo"].Expires = DateTime.Now;
                Response.SetCookie(Request.Cookies["CookieTipo"]);

            }

            ViewBag.Sucesso = false;

            Request.Cookies.Clear();
            return RedirectToAction("Index","Home");
        }

        [HttpGet]
        public ActionResult Cadastrar()
        {
            Pesquisador Pesquisa = new Pesquisador();
            return View(Pesquisa);
        }


        [HttpPost]
        public ActionResult Cadastrar(Pesquisador Pesquisa)
        {
            if (ModelState.IsValid)
            {
                Pesquisa.Senha = Crypto.HashPassword(Pesquisa.Senha.ToString());
                db.Pesquisadores.Add(Pesquisa);
                db.SaveChanges();

                return RedirectToAction("Login");
            }

            return View(Pesquisa);
        }

    }
}