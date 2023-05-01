using System.ComponentModel.DataAnnotations;
using WebSisMed.Models.Enums;

namespace WebSisMed.ViewModels.Consultas
{
    public class EditarConsultaViewModel
    {
        public int Id { get; set; }

        [DataType(DataType.Date)]
        public DateTime Data { get; set; }
        public TipoConsulta Tipo { get; set; }

        [Display(Name = "Paciente")]
        public int IdPaciente { get; set; }

        [Display(Name = "Nome do Paciente")]
        public string NomePaciente { get; set; } = string.Empty;

        [Display(Name = "Médico")]
        public int IdMedico { get; set; }
    }
}
