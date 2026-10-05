# Desafio Target Sistemas

C# com .NET 10, sem dependências além do xUnit nos testes.

Os JSONs de `dados` são copiados junto com o executável, então dá para rodar pelo terminal ou pelo Visual Studio.

## 1. Comissão por vendedor

```bash
dotnet run --project Desafio -- comissao
```

Lê `dados/vendas.json` e soma a comissão de cada venda:

- abaixo de R$ 100,00: sem comissão
- de R$ 100,00 até R$ 499,99: 1%
- a partir de R$ 500,00: 5%

```
João Silva: R$ 495,68
Maria Souza: R$ 465,95
Carlos Oliveira: R$ 379,37
Ana Lima: R$ 404,98
```

A comissão é somada sem arredondar e só o total de cada vendedor vai para centavos, com meio centavo para cima. Arredondar venda a venda daria R$ 0,01 a mais para cada um.

## 2. Movimentação de estoque

```bash
dotnet run --project Desafio -- estoque 101 saida 30 "Venda balcão"
dotnet run --project Desafio -- estoque 101 entrada 10 "Compra fornecedor"
```

```
Movimentação #1 registrada. Estoque final: 120
Movimentação #2 registrada. Estoque final: 130
```

O estoque inicial (`dados/estoque.json`) não é alterado. Cada movimentação recebe um id sequencial e fica registrada em `movimentacoes.json`, na pasta `dados` ao lado do executável (`Desafio/bin/Debug/net10.0/dados`). O saldo é o estoque inicial mais as movimentações do produto. O programa recusa saída maior que o saldo, produto inexistente, quantidade não inteira ou menor que 1 e descrição vazia.

## 3. Juros por atraso

```bash
dotnet run --project Desafio -- juros 1000,00 25/09/2026
```

Multa de 2,5% ao dia sobre o valor original (juros simples), contando os dias corridos entre o vencimento e hoje, no fuso de São Paulo. Sem atraso, os juros são zero. O valor usa vírgula como separador decimal.

```
Juros: R$ 250,00
Total: R$ 1.250,00
```

## Testes

```bash
dotnet test
```
