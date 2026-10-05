namespace Desafio;

public record Produto(int CodigoProduto, string DescricaoProduto, int Estoque);

public enum TipoMovimentacao { Entrada, Saida }

public record Movimentacao(int Id, int CodigoProduto, TipoMovimentacao Tipo, int Quantidade, string Descricao, int EstoqueFinal);

public class Deposito(IEnumerable<Produto> produtos, IEnumerable<Movimentacao> movimentacoes)
{
    private readonly Dictionary<int, Produto> _produtos = produtos.ToDictionary(p => p.CodigoProduto);
    private readonly List<Movimentacao> _movimentacoes = movimentacoes.ToList();

    public IReadOnlyList<Movimentacao> Movimentacoes => _movimentacoes;

    public int Saldo(int codigoProduto) =>
        BuscarProduto(codigoProduto).Estoque + _movimentacoes
            .Where(m => m.CodigoProduto == codigoProduto)
            .Sum(m => m.Tipo == TipoMovimentacao.Entrada ? m.Quantidade : -m.Quantidade);

    public Movimentacao Movimentar(int codigoProduto, TipoMovimentacao tipo, int quantidade, string descricao)
    {
        var produto = BuscarProduto(codigoProduto);

        if (quantidade <= 0)
            throw new ArgumentException("A quantidade deve ser maior que zero.");

        if (string.IsNullOrWhiteSpace(descricao))
            throw new ArgumentException("Informe a descrição da movimentação.");

        var saldoAtual = Saldo(codigoProduto);
        var estoqueFinal = tipo == TipoMovimentacao.Entrada ? saldoAtual + quantidade : saldoAtual - quantidade;

        if (estoqueFinal < 0)
            throw new InvalidOperationException($"Estoque insuficiente: {produto.DescricaoProduto} tem {saldoAtual} unidades.");

        var movimentacao = new Movimentacao(
            _movimentacoes.Select(m => m.Id).DefaultIfEmpty().Max() + 1,
            codigoProduto,
            tipo,
            quantidade,
            descricao.Trim(),
            estoqueFinal);

        _movimentacoes.Add(movimentacao);
        return movimentacao;
    }

    private Produto BuscarProduto(int codigoProduto) =>
        _produtos.GetValueOrDefault(codigoProduto)
            ?? throw new ArgumentException($"Produto {codigoProduto} não encontrado.");
}
