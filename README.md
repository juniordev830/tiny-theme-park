# Tiny Theme Park

**Status:** protótipo inicial em C# para **Unity 2022.3 LTS**, com cenário 3D criado automaticamente em Play Mode (sem modelos de terceiros). O núcleo de economia foi escrito sem dependências de Unity, permitindo testes automatizados.

## Abrir no Unity
1. Abra a pasta raiz no **Unity Hub** e escolha uma instalação compatível com Unity 2022.3 LTS.
2. Espere a importação dos scripts e abra ou crie uma cena 3D vazia.
3. Pressione **Play**. Um bootstrap com `RuntimeInitializeOnLoadMethod` gera o parque, câmera, árvores, caminhos, atrações e visitantes animados.

## Jogar
- O parque começa com 300 moedas e uma atração.
- O tempo de simulação avança um minuto a cada oito segundos.
- Visitantes e receitas aumentam de acordo com o número de atrações.
- Clique em **Construir atração** ou pressione **B** para expandir quando houver saldo.
- Pressione **M** ou clique em **Iniciar marketing** para promover o parque por cinco minutos simulados.
- O painel mostra moedas, visitantes, satisfação e custo de expansão.

## Testes do núcleo sem Unity
Instale .NET SDK 8 e execute:

```bash
dotnet run --project Tests/ParkEconomy.Tests.csproj --configuration Release
```

O workflow do GitHub Actions roda este teste de regras de economia. Ele **não** compila nem valida o jogo no editor Unity.

## Arquivos
- `Assets/Scripts/ParkBootstrap.cs`: construção 3D, movimentação visual e interação OnGUI.
- `Assets/Scripts/ParkEconomy.cs`: regras de preço, visitantes e dinheiro.
- `Tests/`: projeto de testes executável em .NET.

## Limites
Protótipo sem sistema de salvamento, mapa editável, navegação por caminhos, física detalhada, modelos finais, UI moderna ou build para distribuição. A cena é gerada em tempo de execução; a aparência precisa ser verificada no editor Unity antes de um release.


## Campanhas de marketing (novo)

Quando há moedas suficientes e nenhuma campanha ativa, é possível comprar uma promoção. O custo cresce com o número de atrações (`45 + 10 × atrações` moedas), e o efeito dura **cinco minutos simulados**. Durante esse período, cada minuto recebe quatro visitantes adicionais (até o limite de capacidade atual) e um bônus de satisfação. Uma campanha ativa não pode ser comprada novamente até expirar.

O núcleo `ParkEconomy.cs` permanece independente da Unity e possui testes sobre cobrança única, expiração, aumento de visitantes, felicidade e rejeição de compras sem saldo. O painel informa o custo ou o tempo restante. A execução visual da Unity ainda precisa de validação no editor para lançamento.

## Relatório financeiro por minuto

O painel apresenta o **saldo líquido do último minuto** (`LastMinuteNetRevenue`): receita bruta de visitantes menos custo de operação das atrações. O saldo total agora é calculado com aritmética de 64 bits e limitado ao maior inteiro suportado, evitando valores negativos por overflow quando a simulação cresce. Os cenários de limite e receita sem visitantes estão cobertos pelos testes do núcleo; não representam uma contabilidade de produção.
