namespace Desafio.Testes;

public class ComissaoTestes
{
    [Theory]
    [InlineData(99.99, 0)]
    [InlineData(100.00, 0.01)]
    [InlineData(499.99, 0.01)]
    [InlineData(500.00, 0.05)]
    public void Percentual_respeita_as_faixas(decimal valor, decimal esperado) =>
        Assert.Equal(esperado, Comissao.Percentual(valor));

    [Fact]
    public void Arredonda_meio_centavo_para_cima() =>
        Assert.Equal(60.03m, Comissao.PorVendedor([new("Ana", 1200.50m)])["Ana"]);

    [Fact]
    public void Arredonda_so_o_total_do_vendedor() =>
        Assert.Equal(120.05m, Comissao.PorVendedor([new("Ana", 1200.50m), new("Ana", 1200.50m)])["Ana"]);

    [Fact]
    public void Soma_a_comissao_por_vendedor()
    {
        var comissoes = Comissao.PorVendedor(
        [
            new("Ana", 90m),
            new("Ana", 200m),
            new("Ana", 1000m),
            new("Bia", 500m),
        ]);

        Assert.Equal(52m, comissoes["Ana"]);
        Assert.Equal(25m, comissoes["Bia"]);
    }
}
