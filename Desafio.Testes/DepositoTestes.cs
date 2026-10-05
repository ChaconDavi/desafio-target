namespace Desafio.Testes;

public class DepositoTestes
{
    private static Deposito NovoDeposito() => new([new(101, "Caneta Azul", 150), new(102, "Caderno Universitário", 75)], []);

    [Fact]
    public void Entrada_e_saida_atualizam_o_estoque_do_produto()
    {
        var deposito = NovoDeposito();

        var saida = deposito.Movimentar(101, TipoMovimentacao.Saida, 30, "Venda");
        var entrada = deposito.Movimentar(101, TipoMovimentacao.Entrada, 10, "Compra");

        Assert.Equal(120, saida.EstoqueFinal);
        Assert.Equal(130, entrada.EstoqueFinal);
        Assert.Equal(75, deposito.Saldo(102));
    }

    [Fact]
    public void Id_continua_a_partir_das_movimentacoes_existentes()
    {
        var deposito = new Deposito([new(101, "Caneta Azul", 150)], [new(7, 101, TipoMovimentacao.Saida, 10, "Venda", 140)]);

        var movimentacao = deposito.Movimentar(101, TipoMovimentacao.Saida, 5, "Venda");

        Assert.Equal(8, movimentacao.Id);
        Assert.Equal(135, movimentacao.EstoqueFinal);
    }

    [Fact]
    public void Recusa_saida_maior_que_o_estoque()
    {
        var deposito = NovoDeposito();

        var erro = Assert.Throws<InvalidOperationException>(() => deposito.Movimentar(102, TipoMovimentacao.Saida, 76, "Venda"));

        Assert.Equal("Estoque insuficiente: Caderno Universitário tem 75 unidades.", erro.Message);
        Assert.Empty(deposito.Movimentacoes);
    }

    [Theory]
    [InlineData(999, 1, "Venda", "Produto 999 não encontrado.")]
    [InlineData(101, 0, "Venda", "A quantidade deve ser maior que zero.")]
    [InlineData(101, 1, " ", "Informe a descrição da movimentação.")]
    public void Recusa_movimentacao_invalida(int codigo, int quantidade, string descricao, string mensagem) =>
        Assert.Equal(mensagem, Assert.Throws<ArgumentException>(() => NovoDeposito().Movimentar(codigo, TipoMovimentacao.Entrada, quantidade, descricao)).Message);
}
