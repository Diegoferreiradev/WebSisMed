using System.ComponentModel.DataAnnotations;

namespace WebSisMed.ViewModels.MonitoramentoPaciente
{
    public class AdicionarMonitoramentoViewModel
    {
        [Display(Name = "Pressão Arterial")]
        public string PressaoArterial { get; set; } = string.Empty;
        public decimal Temperatura { get; set; }

        [Display(Name = "Saturação de Oxigênio")]
        public int SaturacaoOxigenio { get; set; }

        [Display(Name = "Frequência Cardíaca")]
        public int FrequenciaCardiaca { get; set; }

        [Display(Name = "Data de Aferição")]
        public DateTime DataAfericao { get; set; }
    }
}
