namespace Desafio.Testes;

public class JurosTestes
{
    private static readonly DateOnly Hoje = new(2026, 10, 5);

    [Theory]
    [InlineData("2026-09-25", 250)]
    [InlineData("2026-10-04", 25)]
    [InlineData("2026-10-05", 0)]
    [InlineData("2026-10-10", 0)]
    public void Calcula_2_5_por_cento_por_dia_de_atraso(string vencimento, decimal esperado) =>
        Assert.Equal(esperado, Juros.Calcular(1000m, DateOnly.Parse(vencimento), Hoje));

    [Fact]
    public void Arredonda_para_centavos() =>
        Assert.Equal(0.01m, Juros.Calcular(0.30m, new DateOnly(2026, 10, 4), Hoje));
}
