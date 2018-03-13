using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [Index(IsUnique = true)]
        [MaxLength(50)]
        public string Email { get; set; }

        public virtual ICollection<BaseDeConhecimento> Bases { get; set; }
    }

    public class Admin:Usuario
    {

    }

    public class Paciente:Usuario
    {
        [Required(ErrorMessage = "Campo Obrigatório")]
        [DataType(DataType.Date, ErrorMessage = "Data em formato inválido")]
        public DateTime Nascimento { get; set; }

        [DisplayName("Responsável")]
        public string Responsavel { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        public string Sexo { get; set; }

        public virtual ICollection<Consulta> Consulta { get; set; }

    }
    public class Medico:Usuario
    {

        [Required(ErrorMessage = "Campo Obrigatório")]
        [DisplayName("Registro de classe")]
        public int CRM { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        [DisplayName("Instituição")]
        public string Instituicao { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        [DisplayName("Endereço")]
        public string Endereco { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        [DisplayName("Especialidade")]
        public string Especialidade { get; set; }

        public virtual ICollection<Consulta> Consultas { get; set; }
    }

    public class Pesquisador : Usuario
    {
        [Required(ErrorMessage = "O Lattes é obrigatório")]
        [DataType(DataType.Url)]
        [DisplayName("Link do Lattes")]
        public string Lattes { get; set; }
    }
}