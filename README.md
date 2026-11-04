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
