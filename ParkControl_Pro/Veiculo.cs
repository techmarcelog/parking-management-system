using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkControl_Pro
{
    public class Veiculo
    {
        public string Placa { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Tipo { get; set; }        // "Carro" ou "Moto"
        public string Categoria { get; set; }   // "Avulso" ou "Mensalista"
        public int HoraEntrada { get; set; }
        public double ValorFinalPago { get; set; } // Atributo para o relatório de faturamento
    }
}