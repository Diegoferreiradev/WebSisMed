﻿namespace WebSisMed.ViewModels.MonitoramentoPaciente
{
    public class EditarMonitoramentoViewModel
    {
        public int Id { get; set; }
        public string PressaoArterial { get; set; } = string.Empty;
        public decimal Temperatura { get; set; }
        public int SaturacaoOxigenio { get; set; }
        public int FrequenciaCardiaca { get; set; }
        public DateTime DataAfericao { get; set; }
    }
}
