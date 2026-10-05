namespace Desafio;

public record Venda(string Vendedor, decimal Valor);

public static class Comissao
{
    public static decimal Percentual(decimal valor) => valor switch
    {
        < 100m => 0m,
        < 500m => 0.01m,
        _ => 0.05m,
    };

    public static Dictionary<string, decimal> PorVendedor(IEnumerable<Venda> vendas) =>
        vendas
            .GroupBy(v => v.Vendedor)
            .ToDictionary(
                g => g.Key,
                g => Math.Round(g.Sum(v => v.Valor * Percentual(v.Valor)), 2, MidpointRounding.AwayFromZero));
}
