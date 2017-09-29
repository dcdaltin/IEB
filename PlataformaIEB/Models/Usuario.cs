using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PlataformaIEB.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome Obrigatório")]
        [DisplayName("Nome")]
        public string Nome { get; set; }

        public string AutID { get; set; }

        [Required(ErrorMessage = "A senha deve ser BOA")]
        [DataType(DataType.Password)]
        [DisplayName("Senha")]
        public string Senha { get; set; }


        [Required(ErrorMessage = "O E-mail deve ter a forma xxx@xxx.xxx")]
        [EmailAddress]
        [DisplayName("Email")]
        public string Email { get; set; }
    }

    public class Admin:Usuario
    {

    }

    public class Paciente:Usuario
    {
        [Required(ErrorMessage = "A data é necessária")]
        [DataType(DataType.Date, ErrorMessage = "Data em formato inválido")]
        public DateTime Nascimento { get; set; }


        [Required(ErrorMessage = "Nome do responsável")]
        public string Responsavel { get; set; }

        [Required(ErrorMessage = "Favor indicar o Sexo")]
        public string Sexo { get; set; }

        public virtual ICollection<Exame> Exames { get; set; }

    }
    public class Medico:Usuario
    {

        [Required(ErrorMessage = "A senha deve ser BOA")]
        [DisplayName("CRM")]
        public int CRM { get; set; }

        public string Instituicao { get; set; }

        public string Endereco { get; set; }

        public string Especialidade { get; set; }

        public virtual ICollection<Exame> Exames { get; set; }
    }

    public class Pesquisador : Usuario
    {
        [Required(ErrorMessage = "O Lattes é obrigatório")]
        [DataType(DataType.Url)]
        [DisplayName("Link do Lattes")]
        public string Lattes { get; set; }
    }
}