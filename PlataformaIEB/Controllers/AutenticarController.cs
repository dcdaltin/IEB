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
        
        DatabaseContext db = new DatabaseContext();

        public ActionResult Login()
        {
            VMLogin login = new VMLogin();
            return View(login);

        }

        public bool VerificaSenha(VMLogin login, Usuario usuario)
        {
            if (!Crypto.VerifyHashedPassword(usuario.Senha, login.Senha))
            {
                return false;
            }

            //Gera Aleatório Cadastro Usuario (TOkken)
            string autoriza = Guid.NewGuid().ToString();

            //Cria Cookie da identificação
            var CookieTokken = new HttpCookie("TokkenCookie");

            CookieTokken.Expires = DateTime.Now.AddDays(1);

            CookieTokken.Value = autoriza;

            Response.Cookies.Add(CookieTokken);

            Session["TokkenNome"] = CookieTokken;

            usuario.AutID = autoriza;

            db.SaveChanges();

            return true;
        }

        public Usuario ProcuraUsuario(string AutID)
        {
            if (db.Medicos.Where(b => b.AutID == AutID).FirstOrDefault() != null)
            {
                Usuario usuario = db.Medicos.Where(b => b.AutID == AutID).FirstOrDefault();
                return usuario;
            }
            if (db.Admins.Where(a => a.AutID == AutID).FirstOrDefault() != null)
            {
                Usuario usuario = db.Admins.Where(a => a.AutID == AutID).FirstOrDefault();
                return usuario;
            }
            if (db.Pacientes.Where(c => c.AutID == AutID).FirstOrDefault() != null)
            {
                Usuario usuario = db.Pacientes.Where(c => c.AutID == AutID).FirstOrDefault();
                return usuario;
            }
            if (db.Pesquisadores.Where(c => c.AutID == AutID).FirstOrDefault() != null)
            {
                Usuario usuario = db.Pesquisadores.Where(c => c.AutID == AutID).FirstOrDefault();
                return usuario;
            }
            RedirectToAction("Login","Autenticar");
            return null;
        }

        [HttpPost]
        public ActionResult Login(VMLogin login)
        {
            if (ModelState.IsValid)
            {
                if (db.Admins.Where( a =>  a.Email == login.Email).SingleOrDefault() != null)
                {
                    Admin usuario = db.Admins.Where(a => login.Email == a.Email).SingleOrDefault();
                    if (!VerificaSenha(login, usuario))
                    {
                        ModelState.AddModelError("", "Senha Errada");
                        return View(login);
                    }
                    
                    return RedirectToAction("Index", "Cadastro", new { area = "Admin"});
                        
                }
                else 
                if (db.Medicos.Where(a => login.Email == a.Email).FirstOrDefault() != null)
                {
                    Medico usuario = db.Medicos.Where(a => login.Email == a.Email).FirstOrDefault();
                    if (!VerificaSenha(login, usuario))
                    {
                        ModelState.AddModelError("", "Senha Errada");
                        return View(login);
                    }

                    return RedirectToAction("Index", "Home");
                }
                else 
                if (db.Pacientes.Where(a => login.Email == a.Email).FirstOrDefault() != null)
                {
                    Paciente usuario = db.Pacientes.Where(a => login.Email == a.Email).FirstOrDefault();
                    if (!VerificaSenha(login, usuario))
                    {
                        ModelState.AddModelError("", "Senha Errada");
                        return View(login);
                    }
                    ViewBag.Sucesso = true;
                    ViewBag.Nome = usuario.Nome;
                    return RedirectToAction("Index", "Home");
                }
                if (db.Pesquisadores.Where(a => login.Email == a.Email).FirstOrDefault() != null)
                {
                    Pesquisador usuario = db.Pesquisadores.Where(a => login.Email == a.Email).FirstOrDefault();
                    if (!VerificaSenha(login, usuario))
                    {
                        ModelState.AddModelError("", "Senha Errada");
                        return View(login);
                    }
                    ViewBag.Sucesso = true;
                    ViewBag.Nome = usuario.Nome;
                    return RedirectToAction("Base", "Especialista", new { area="RestritoPesquisador"});
                }
                else
                {
                    ViewBag.Sucesso = "Usuário Não Encontrado! Favor entrar em contato";
                    ModelState.AddModelError("", "Usuário Não Encontrado!Favor entrar em contato");
                    return View(login);
                }

            }
            ModelState.AddModelError("", "Falha no Login! Favor entrar em contato");
            return View(login);

        }

        public ActionResult Logout()
        {
            if (Request.Cookies.AllKeys.Contains("TokkenCookie"))
            {
                String AutID = Request.Cookies["TokkenCookie"].Value;
                Usuario usuario = ProcuraUsuario(AutID);
                if(usuario != null)
                {
                    usuario.AutID = null;
                    db.SaveChanges();
                }

                Request.Cookies["TokkenCookie"].Value = "";
                Request.Cookies["TokkenCookie"].Expires = DateTime.Now;

                Response.SetCookie(Request.Cookies["TokkenCookie"]);

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