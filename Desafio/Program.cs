using System.Globalization;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using Desafio;

Console.OutputEncoding = Encoding.UTF8;

var real = CultureInfo.GetCultureInfo("pt-BR");
var json = new JsonSerializerOptions(JsonSerializerDefaults.Web)
{
    WriteIndented = true,
    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
    Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) },
};

var dados = Path.Combine(AppContext.BaseDirectory, "dados");

T Ler<T>(string caminho) => JsonSerializer.Deserialize<T>(File.ReadAllText(caminho), json)!;

switch (args.FirstOrDefault())
{
    case "comissao" when args.Length == 1:
    {
        foreach (var (vendedor, comissao) in Comissao.PorVendedor(Ler<ArquivoVendas>(Path.Combine(dados, "vendas.json")).Vendas))
            Console.WriteLine($"{vendedor}: {comissao.ToString("C", real)}");

        return 0;
    }

    case "estoque" when args.Length == 5
        && int.TryParse(args[1], out var codigo)
        && int.TryParse(args[3], out var quantidade)
        && args[2].ToLowerInvariant() is "entrada" or "saida" or "saída":
    {
        var arquivo = Path.Combine(dados, "movimentacoes.json");
        var tipo = args[2].ToLowerInvariant() == "entrada" ? TipoMovimentacao.Entrada : TipoMovimentacao.Saida;
        var deposito = new Deposito(
            Ler<ArquivoEstoque>(Path.Combine(dados, "estoque.json")).Estoque,
            File.Exists(arquivo) ? Ler<List<Movimentacao>>(arquivo) : []);

        try
        {
            var movimentacao = deposito.Movimentar(codigo, tipo, quantidade, args[4]);
            File.WriteAllText(arquivo, JsonSerializer.Serialize(deposito.Movimentacoes, json));
            Console.WriteLine($"Movimentação #{movimentacao.Id} registrada. Estoque final: {movimentacao.EstoqueFinal}");
            return 0;
        }
        catch (Exception e) when (e is ArgumentException or InvalidOperationException)
        {
            Console.Error.WriteLine(e.Message);
            return 1;
        }
    }

    case "juros" when args.Length == 3
        && decimal.TryParse(args[1], NumberStyles.AllowDecimalPoint, real, out var valor)
        && DateOnly.TryParseExact(args[2], "dd/MM/yyyy", real, DateTimeStyles.None, out var vencimento):
    {
        var hoje = DateOnly.FromDateTime(TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "America/Sao_Paulo"));
        var juros = Juros.Calcular(valor, vencimento, hoje);

        Console.WriteLine($"Juros: {juros.ToString("C", real)}");
        Console.WriteLine($"Total: {(valor + juros).ToString("C", real)}");
        return 0;
    }

    default:
        Console.Error.WriteLine("""
            Uso:
              comissao
              estoque <codigo> <entrada|saida> <quantidade> "<descricao>"
              juros <valor> <vencimento dd/mm/aaaa>
            """);
        return 1;
}

record ArquivoVendas(List<Venda> Vendas);

record ArquivoEstoque(List<Produto> Estoque);
