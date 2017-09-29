using PlataformaIEB.Areas.RestritoMedico.Models;
using PlataformaIEB.Filtros;
using PlataformaIEB.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace PlataformaIEB.Areas.RestritoMedico.Controllers
{
    [LiberarAreas("Medico")]
    public class CadastroController : Controller
    {
        DatabaseContext db = new DatabaseContext();

        private Medico medico;


        // GET: RestritoMedico/Cadastro
        public ActionResult Index()
        {
            var cookie = Request.Cookies["TokkenCookie"].Value;
            medico = db.Medicos.Where(m => m.AutID == cookie).SingleOrDefault();


            ViewBag.Nome = medico.Nome;

            return View();
        }

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
            return View(paciente);

        }


        [HttpPost]
        public ActionResult CadPaciente(Paciente paciente)
        {
            if (ModelState.IsValid)
            {
                paciente.Senha = Crypto.HashPassword(paciente.Senha.ToString());
                db.Pacientes.Add(paciente);
                db.SaveChanges();
                return RedirectToAction("PrincipalPaciente");
            }

            ViewBag.Sexo = new List<string>();
            ViewBag.Sexo.Add("Masculino");
            ViewBag.Sexo.Add("Feminino");
            return View(paciente);
        }


        [HttpGet]
        public ActionResult CadTriagem()
        {
            List<Paciente> pacientes = db.Pacientes.OrderBy(p => p.Nome).ToList();
            VMTriagem exame = new VMTriagem();
            exame.Data = DateTime.Now.ToLocalTime();
            exame.Pacientes = pacientes;


            return PartialView(exame);
        }

        [HttpPost]
        public ActionResult CadTriagem(VMTriagem modeltriagem)
        {

            if (ModelState.IsValid)
            {
                Exame exame = new Exame();
                Triagem triagem = new Triagem();
                triagem.Altura = modeltriagem.Altura;
                triagem.Peso = modeltriagem.Peso;

                try
                {
                    triagem.IMC = triagem.Peso / ((triagem.Altura) * (triagem.Altura));
                }
                catch
                {
                    ViewBag.IMC = false;
                    modeltriagem.Pacientes = db.Pacientes.OrderBy(p => p.Nome).ToList();
                    return View(modeltriagem);
                }


                exame.Medico = medico;
                exame.Paciente = db.Pacientes.Where(m => m.Id == modeltriagem.IDPaciente).SingleOrDefault();
                exame.Data = modeltriagem.Data;
                triagem.Idade = (exame.Data.Year - exame.Paciente.Nascimento.Year);

                if ((exame.Paciente.Nascimento.Month > exame.Data.Month) || (exame.Paciente.Nascimento.Month == exame.Data.Month && exame.Paciente.Nascimento.Day > exame.Data.Day))
                    triagem.Idade--;

                exame.Triagem = triagem;

                db.Triagens.Add(triagem);
                db.Exames.Add(exame);

                try
                {
                    db.SaveChanges();
                    return RedirectToAction("Clinico/" + exame.Id);
                }
                catch (Exception)
                {

                    modeltriagem.Pacientes = db.Pacientes.OrderBy(p => p.Nome).ToList();
                    return View(modeltriagem);
                }

            }

            modeltriagem.Pacientes = db.Pacientes.OrderBy(p => p.Nome).ToList();
            return PartialView(modeltriagem);
        }

        [HttpGet]
        public ActionResult Clinico(int Id)
        {

            Clinico modelo = new Clinico();

            if (db.Pacientes.Where(p => p.Id == Id).SingleOrDefault().Sexo == "Masculino")
            {
                ViewBag.Sexo = "Masculino";
            }
            else
            {
                ViewBag.Sexo = "Feminino";
            }

            return View(modelo);
        }

        [HttpPost]
        public ActionResult Clinico(Clinico modelo, int Id)
        {

            if (ModelState.IsValid)
            {

                Exame exame = db.Exames.Where(e => e.Id == Id).SingleOrDefault();
                exame.Clinico = modelo;

                db.Clinicos.Add(exame.Clinico);
                db.Exames.AddOrUpdate(exame);

                db.SaveChanges();

                return RedirectToAction("Lab/" + exame.Id);
            }
            return View(modelo);
        }

        [HttpGet]
        public ActionResult Imagem(int Id)
        {


            Imagem modelo = new Imagem();

            if (db.Exames.Where(p => p.Id == Id).SingleOrDefault().Paciente.Sexo == "Masculino")
            {
                ViewBag.Sexo = "Masculino";
            }
            else
            {
                ViewBag.Sexo = "Feminino";
            }

            return View(modelo);
        }

        [HttpPost]
        public ActionResult Imagem(Imagem modelo, int Id)
        {

            if (ModelState.IsValid)
            {

                Exame exame = db.Exames.Where(e => e.Id == Id).SingleOrDefault();
                exame.Imagem = modelo;

                db.Imagens.Add(exame.Imagem);
                db.Exames.AddOrUpdate(exame);

                db.SaveChanges();

                return RedirectToAction("Diagnostico/" + exame.Id);
            }

            if (db.Exames.Where(p => p.Id == Id).SingleOrDefault().Paciente.Sexo == "Masculino")
            {
                ViewBag.Sexo = "Masculino";
            }
            else
            {
                ViewBag.Sexo = "Feminino";
            }
            return View(modelo);
        }

        [HttpGet]
        public ActionResult Lab(int Id)
        {


            Laboratorial modelo = new Laboratorial();

            if (db.Exames.Where(p => p.Id == Id).SingleOrDefault().Paciente.Sexo == "Masculino")
            {
                ViewBag.Sexo = "Masculino";
            }

            ViewBag.Lista = new List<string>
            {
                "Diminunído",
                "Normal",
                "Aumentado"
            };
            return View(modelo);
        }

        [HttpPost]
        public ActionResult Lab(Laboratorial modelo, int Id)
        {

            if (ModelState.IsValid)
            {

                Exame exame = db.Exames.Where(e => e.Id == Id).SingleOrDefault();
                exame.Lab = modelo;

                db.Labs.Add(exame.Lab);
                db.Exames.AddOrUpdate(exame);

                db.SaveChanges();

                return RedirectToAction("Imagem/" + exame.Id);
            }

            if (db.Exames.Where(p => p.Id == Id).SingleOrDefault().Paciente.Sexo == "Masculino")
            {
                ViewBag.Sexo = "Masculino";
            }

            ViewBag.Lista.Add("Diminunído");
            ViewBag.Lista.Add("Normal");
            ViewBag.Lista.Add("Aumentado");

            return View(modelo);
        }

        [HttpGet]
        public ActionResult Diagnostico(int Id)
        {
            Diagnostico modelo = new Diagnostico();
            return View(modelo);
        }

        [HttpPost]
        public ActionResult Diagnostico(Diagnostico modelo, int Id)
        {

            if (ModelState.IsValid)
            {
                Exame exame = db.Exames.Where(o => o.Id == Id).SingleOrDefault();

                exame.Diagnostico = modelo;

                db.Diagnosticos.Add(modelo);
                db.Exames.AddOrUpdate(exame);

                db.SaveChanges();

                return RedirectToAction("PrincipalExame");
            }

            return View(modelo);
        }
    }
}