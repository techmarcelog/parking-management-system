using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkControl_Pro
{
    public class EstacionamentoManager
    {
        private Veiculo[] setorCarros;
        private Veiculo[] setorMotos;
        private List<Veiculo> historico = new List<Veiculo>();
        private Configuracao config;

        public EstacionamentoManager(Configuracao configuracao)
        {
            this.config = configuracao;
            this.setorCarros = new Veiculo[config.VagasCarro];
            this.setorMotos = new Veiculo[config.VagasMoto];
        }

        public void RegistrarEntrada(Veiculo v)
        {
            // Normalização interna para garantir que a lógica use apenas "carro" ou "moto"
            Veiculo[] setor = v.Tipo.Equals("carro") ? setorCarros : setorMotos;
            int vagaIndex = LocalizarVagaLivre(setor);

            if (vagaIndex != -1)
            {
                setor[vagaIndex] = v;
                Console.WriteLine("\n==============================");
                Console.WriteLine("      CUPOM DE ENTRADA");
                Console.WriteLine($"PLACA: {v.Placa} | VAGA: {vagaIndex}");
                Console.WriteLine($"ENTRADA: {v.HoraEntrada}h | CATEGORIA: {v.Categoria.ToUpper()}");
                Console.WriteLine("==============================");
                ExibirStatusVagas();
            }
            else
            {
                Console.WriteLine($"\nERRO: SETOR DE {v.Tipo.ToUpper()}S LOTADO!");
            }
        }

        public void ProcessarSaida(string placa, int horaSaida)
        {
            // A busca ignora maiúsculas/minúsculas
            Veiculo v = BuscarPorPlaca(placa, out int index, out string tipoEncontrado);

            if (v != null)
            {
                if (horaSaida < v.HoraEntrada)
                {
                    Console.WriteLine("ERRO: Hora de saída não pode ser menor que a entrada.");
                    return;
                }

                double total = CalcularValor(v, horaSaida);
                v.ValorFinalPago = total;
                
                Console.WriteLine("\n------------------------------");
                Console.WriteLine("         NOTA FISCAL");
                Console.WriteLine($"VEÍCULO: {v.Marca} {v.Modelo} ({v.Placa})");
                Console.WriteLine($"VALOR TOTAL: R$ {total:F2}");
                Console.WriteLine("------------------------------");

                historico.Add(v);
                LiberarVaga(tipoEncontrado, index);
                ExibirStatusVagas();
            }
            else
            {
                Console.WriteLine("\nERRO: Veículo não encontrado.");
            }
        }

        private double CalcularValor(Veiculo v, int horaSaida)
        {
            if (v.Categoria.Equals("mensalista")) return 0;

            int tempo = Math.Max(1, horaSaida - v.HoraEntrada);
            double vHora = v.Tipo.Equals("carro") ? config.ValorHoraCarro : config.ValorHoraMoto;
            double vAdic = v.Tipo.Equals("carro") ? config.ValorAdicionalCarro : config.ValorAdicionalMoto;

            return vHora + (Math.Max(0, tempo - 1) * vAdic);
        }

        private int LocalizarVagaLivre(Veiculo[] setor)
        {
            for (int i = 0; i < setor.Length; i++)
                if (setor[i] == null) return i;
            return -1;
        }

        private Veiculo BuscarPorPlaca(string placa, out int index, out string tipo)
        {
            // Comparação usando OrdinalIgnoreCase para ignorar case
            for (int i = 0; i < setorCarros.Length; i++)
                if (setorCarros[i] != null && setorCarros[i].Placa.Equals(placa, StringComparison.OrdinalIgnoreCase)) 
                    { index = i; tipo = "carro"; return setorCarros[i]; }

            for (int i = 0; i < setorMotos.Length; i++)
                if (setorMotos[i] != null && setorMotos[i].Placa.Equals(placa, StringComparison.OrdinalIgnoreCase)) 
                    { index = i; tipo = "moto"; return setorMotos[i]; }

            index = -1; tipo = ""; return null;
        }

        private void LiberarVaga(string tipo, int index)
        {
            if (tipo == "carro") setorCarros[index] = null;
            else setorMotos[index] = null;
        }

        public void ExibirStatusVagas()
        {
            int ocupCarro = 0, ocupMoto = 0;
            foreach (var v in setorCarros) if (v != null) ocupCarro++;
            foreach (var v in setorMotos) if (v != null) ocupMoto++;
            Console.WriteLine($"\nOCUPAÇÃO: CARROS [{ocupCarro}/{config.VagasCarro}] | MOTOS [{ocupMoto}/{config.VagasMoto}]");
        }

        public void GerarRelatorioFinal()
        {
            double faturamento = 0;
            int tCarro = 0, tMoto = 0;
            List<string> marcas = new List<string>();

            foreach (var v in historico)
            {
                faturamento += v.ValorFinalPago;
                if (v.Categoria.Equals("mensalista"))
                    faturamento += v.Tipo.Equals("carro") ? config.MensalidadeCarro : config.MensalidadeMoto;

                if (v.Tipo.Equals("carro")) tCarro++; else tMoto++;
                if (!marcas.Contains(v.Marca)) marcas.Add(v.Marca);
            }

            Console.WriteLine("\n======= RELATÓRIO FINAL =======");
            Console.WriteLine($"FATURAMENTO: R$ {faturamento:F2} | FROTA: {tCarro} Carros, {tMoto} Motos");
            Console.WriteLine("MARCAS: " + string.Join(", ", marcas));
            Console.WriteLine("===============================");
        }
    }
}