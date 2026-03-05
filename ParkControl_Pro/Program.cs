using ParkControl_Pro;

Configuracao config = new Configuracao();
        
Console.WriteLine("=== BEM-VINDO AO SETUP PARKCONTROL PRO ===");
Console.Write("Vagas Carro: "); config.VagasCarro = int.Parse(Console.ReadLine()!);
Console.Write("Vagas Moto: "); config.VagasMoto = int.Parse(Console.ReadLine()!);

Console.Write("Preço Hora Carro: "); config.ValorHoraCarro = double.Parse(Console.ReadLine()!);
Console.Write("Adicional Carro: "); config.ValorAdicionalCarro = double.Parse(Console.ReadLine()!);
Console.Write("Mensalidade Carro: "); config.MensalidadeCarro = double.Parse(Console.ReadLine()!);

Console.Write("Preço Hora Moto: "); config.ValorHoraMoto = double.Parse(Console.ReadLine()!);
Console.Write("Adicional Moto: "); config.ValorAdicionalMoto = double.Parse(Console.ReadLine()!);
Console.Write("Mensalidade Moto: "); config.MensalidadeMoto = double.Parse(Console.ReadLine()!);

EstacionamentoManager sistema = new EstacionamentoManager(config);
int menu;

do
{
    Console.WriteLine("\n1-Entrada | 2-Saída | 3-Vagas | 4-Relatório | 0-Sair");
    if (!int.TryParse(Console.ReadLine(), out menu)) continue;

    switch (menu)
    {
        case 1:
            Veiculo v = new Veiculo();
            Console.Write("Tipo (Carro/Moto): "); 
            v.Tipo = Console.ReadLine()!.Trim().ToLower();
            
            Console.Write("Placa: "); 
            v.Placa = Console.ReadLine()!.Trim().ToUpper();
            
            Console.Write("Marca: "); 
            v.Marca = Console.ReadLine()!.Trim().ToUpper();
            
            Console.Write("Modelo: "); 
            v.Modelo = Console.ReadLine()!.Trim().ToUpper();
            
            Console.Write("Categoria (Avulso/Mensalista): "); 
            v.Categoria = Console.ReadLine()!.Trim().ToLower();
            
            Console.Write("Hora Entrada: "); 
            v.HoraEntrada = int.Parse(Console.ReadLine()!);
            
            sistema.RegistrarEntrada(v);
            break;

        case 2:
            Console.Write("Placa para saída: "); 
            string p = Console.ReadLine()!.Trim().ToUpper();
            Console.Write("Hora Saída: "); 
            int s = int.Parse(Console.ReadLine()!);
            sistema.ProcessarSaida(p, s);
            break;

        case 3:
                sistema.ExibirStatusVagas();
                break;
                
        case 4:
            sistema.GerarRelatorioFinal();
            break;
    }
} while (menu != 0);