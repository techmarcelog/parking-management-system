using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkControl_Pro
{
    public class Configuracao
    {
        public int VagasCarro { get; set; }
        public int VagasMoto { get; set; }
        
        public double ValorHoraCarro { get; set; }
        public double ValorAdicionalCarro { get; set; }
        public double MensalidadeCarro { get; set; }
        
        public double ValorHoraMoto { get; set; }
        public double ValorAdicionalMoto { get; set; }
        public double MensalidadeMoto { get; set; }
    }
}