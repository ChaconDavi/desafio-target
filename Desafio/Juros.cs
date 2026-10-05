namespace Desafio;

public static class Juros
{
    public static decimal Calcular(decimal valor, DateOnly vencimento, DateOnly hoje)
    {
        var diasAtraso = Math.Max(0, hoje.DayNumber - vencimento.DayNumber);
        return Math.Round(valor * 0.025m * diasAtraso, 2, MidpointRounding.AwayFromZero);
    }
}
