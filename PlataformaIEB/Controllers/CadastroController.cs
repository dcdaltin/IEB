
using PlataformaIEB.Filtros;
using PlataformaIEB.Models;
using PlataformaIEB.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace PlataformaIEB.Controllers
{

    public class CadastroController : Controller
    {

        BancoDeDados db = new BancoDeDados();

        public int CalculaIdade(DateTime Nascimento)
        {
            int idade = DateTime.Now.Year - Nascimento.Year;

            if ((Nascimento.Month > DateTime.Now.Month) || (Nascimento.Month == DateTime.Now.Month && Nascimento.Day > DateTime.Now.Day))
                idade--;

            return idade;
        }


        [LiberarAreas(4)]
        public ActionResult PrincipalMedico(string busca)
        {
            if (string.IsNullOrEmpty(busca))
            {
                busca = "";
            }

            List<Medico> TodosMedicos = db.Medicos.Where(a => a.Nome.Contains(busca)).OrderBy(a => a.Nome).ToList();
            return PartialView(TodosMedicos);
        }

        [LiberarAreas(4)]
        [HttpGet]
        public ActionResult CadMedico()
        {
            return View();
        }

        [LiberarAreas(4)]
        [HttpPost]
        public ActionResult CadMedico(Medico medico)
        {
            if (ModelState.IsValid)
            {

                try
                {
                    medico.Senha = Crypto.HashPassword(medico.Senha.ToString());
                    db.Medicos.Add(medico);
                    db.SaveChanges();
                    return Redirect(Request.UrlReferrer.ToString());
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Email em uso");
                    return View(medico);
                }

            }

            return View(medico);
        }

        [LiberarAreas(3)]
        [HttpGet]
        public ActionResult EditMedico(int Id)
        {
            ViewBag.TituloPainel = "Editar Informações";
            var medico = db.Medicos.Where(m => m.Id == Id).FirstOrDefault();
            return View(medico);
        }

        [LiberarAreas(3)]
        [HttpPost]
        public ActionResult EditMedico(Medico medico)
        {
            if (ModelState.IsValid)
            {
                db.Medicos.AddOrUpdate(medico);
                db.SaveChanges();
                return RedirectToAction("PrincipalMedico");

            }
            return View(medico);
        }

        [LiberarAreas(3)]
        public ActionResult PrincipalPaciente(string busca)
        {
            if (string.IsNullOrEmpty(busca))
            {
                busca = "";
            }
            List<Paciente> TodosPacientes = db.Pacientes.Where(a => a.Nome.Contains(busca)).OrderBy(p => p.Nome).ToList();
            return PartialView(TodosPacientes);
        }


        [LiberarAreas(3)]
        [HttpGet]
        public ActionResult CadPaciente()
        {
            ViewBag.Titulo = "Cadastro de Paciente";
            Paciente paciente = new Paciente();
            ViewBag.Sexo = new List<string>
            {
                "Masculino",
                "Feminino"
            };
            return View();

        }


        [LiberarAreas(3)]
        [HttpPost]
        public ActionResult CadPaciente(Paciente paciente)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    paciente.Senha = Crypto.HashPassword(paciente.Senha.ToString());
                    db.Pacientes.Add(paciente);
                    db.SaveChanges();
                    return RedirectToAction("CadPaciente");
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Email em uso");
                    ViewBag.Sexo = new List<string>{"Masculino","Feminino"};
                    return View(paciente);
                }

            }

            ViewBag.Sexo = new List<string> { "Masculino", "Feminino" };
            return View(paciente);
        }

        [LiberarAreas(1)]
        [HttpGet]
        public ActionResult EditPaciente(int Id)
        {
            ViewBag.TituloPainel = "Editar Informações";
            Paciente paciente = db.Pacientes.Where(p => p.Id == Id).FirstOrDefault();
            ViewBag.Sexo = new List<string>
            {
                "Masculino",
                "Feminino"
            };
            return View(paciente);
        }

        [LiberarAreas(1)]
        [HttpPost]
        public ActionResult EditPaciente(Paciente paciente)
        {
            if (ModelState.IsValid)
            {
                db.Pacientes.AddOrUpdate(paciente);
                db.SaveChanges();
                return RedirectToAction("PrincipalPaciente");
            }

            ViewBag.Sexo = new List<string>
            {
                "Masculino",
                "Feminino"
            };
            return View(paciente);
        }

        [LiberarAreas(3)]
        [HttpGet]
        public ActionResult CadConsulta()
        {
            var Tokken = Request.Cookies["TokkenCookie"].Value;
            Usuario usuario = db.Usuarios.Where(a => a.AutID == Tokken).SingleOrDefault();
            List<ListaValores> Valores = new List<ListaValores>();
            var Variaveis = db.Variaveis.Where(a => a.Base.Where(o => !o.Objetivo).Select(b => b.Base.Usuario.Id).Contains(usuario.Id));

            foreach (var item in Variaveis)
            {
                Valores.Add(new ListaValores() { Variavel = item, VariavelID = item.ID, Confianca = 100 });
            }

            VMConsulta Modelo = new VMConsulta() { Valores = Valores };
            Modelo.Pacientes = db.Pacientes.Select(a=>a.Nome).ToList();
            Modelo.Usuario = usuario;
            return View(Modelo);
        }

        [LiberarAreas(3)]
        [HttpPost]
        public ActionResult CadConsulta(VMConsulta Modelo)
        {
            if (Modelo.Nome != null)
            {
                Consulta Consulta = new Consulta()
                {
                    Paciente = db.Pacientes.Where(a => a.Nome == Modelo.Nome).SingleOrDefault(),
                    Data = DateTime.Now,
                    Medico = db.Medicos.Where(a => a.Id == Modelo.Usuario.Id).SingleOrDefault(),
                    Observacao = Modelo.Observacao
                    
                };

                db.Consultas.Add(Consulta);

                if (Modelo.Valores.Count()>0)
                {
                    foreach (var item in Modelo.Valores)
                    {
                        if (item.Valor != null)
                        {
                            item.Valor = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(item.Valor);
                            item.Consulta = Consulta;
                            item.ConsultaID = Consulta.ID;
                            db.Valores.Add(item);
                        }

                    }
                }
                else
                {
                    Modelo.Pacientes = db.Pacientes.Select(a => a.Nome).ToList();
                    Modelo.Usuario = db.Usuarios.Where(a => a.Id == Modelo.Usuario.Id).SingleOrDefault();
                    ModelState.AddModelError("", "Nenhum dado de entrada");
                    return View(Modelo);
                }

                try
                {
                    db.SaveChanges();

                    return RedirectToAction("Teste", Consulta);

                }
                catch (Exception)
                {
                    Modelo.Pacientes = db.Pacientes.Select(a => a.Nome).ToList();
                    Modelo.Usuario = db.Usuarios.Where(a => a.Id == Modelo.Usuario.Id).SingleOrDefault();
                    ModelState.AddModelError("", "Erro grave! Favor entrar em contato");
                    return View(Modelo);
                }
            }

            Modelo.Pacientes = db.Pacientes.Select(a => a.Nome).ToList();
            Modelo.Usuario = db.Usuarios.Where(a => a.Id == Modelo.Usuario.Id).SingleOrDefault();
            ModelState.AddModelError("", "Selecione um paciente");
            return View(Modelo);

        }

        [LiberarAreas(3)]
        public ActionResult Teste(Consulta Cons)
        {
            Cons = db.Consultas.Where(a=>a.ID == Cons.ID).FirstOrDefault();
            Buscador busca = new Buscador(Cons);
            busca.CriarAgentes();
            if (busca.Aplicadas.Count > 0)
            {
                VMTeste teste = new VMTeste();
                teste.Aplicadas = busca.Aplicadas.ToList();
                teste.Consulta = Cons;
                teste.Objetivos = db.Variaveis.Where(a => a.Base.Where(b => b.Base.Usuario.Id == Cons.Medico.Id).Select(b => b.Objetivo).Contains(true)).ToList();
                return View(teste);
            }

            return RedirectToAction("CadConsulta");
        }
    }

}