# Engenharia reversa e estudo — Tiny Theme Park

## Objetivo e fluxo

Simulação de parque e economia de atrações. ParkBootstrap monta o cenário no Unity. ParkEconomy calcula fluxo e custos, e core.py oferece um modelo separado para as regras.

**Tecnologias identificadas** · C#, Python, .NET 8, Unity.

**Revisão de referência** · `5dbf7dec28d8`.

## Mapa do código

| Caminho | Papel de leitura | Símbolos selecionados |
| --- | --- | --- |
| [Assets/Scripts/ParkBootstrap.cs](../Assets/Scripts/ParkBootstrap.cs) | Implementação | `TinyThemePark`, `ParkBootstrap`, `Launch`, `Start`, `SetupScene`, `CreateRide`, `SpawnGuests`, `TryAddAttraction`, `TryLaunchMarketing` |
| [Assets/Scripts/ParkEconomy.cs](../Assets/Scripts/ParkEconomy.cs) | Implementação | `TinyThemePark`, `ParkEconomy`, `SimulateMinute`, `MinuteForecast`, `TryBuildRide`, `TryLaunchCampaign`, `PreviewNextMinute` |
| [core.py](../core.py) | Implementação | `Ride`, `Park`, `Park.reset`, `Park.inside`, `Park.add_path`, `Park.build`, `Park.connected_paths`, `Park.run_day` |

## Configuração e referências

Ambiente e dependências: [Tests/ParkEconomy.Tests.csproj](../Tests/ParkEconomy.Tests.csproj).

## Roteiro de estudo e desenvolvimento

1. Comece pelo fluxo descrito acima e localize seus pontos de entrada no mapa. Acompanhe uma entrada válida até a transformação ou a resposta.
2. Abra um teste relacionado ao fluxo e descreva qual regra ele protege. Compare o tratamento esperado para entradas válidas, inválidas e nos limites.
3. Ao adicionar uma regra, determine qual módulo controla o dado e cubra o caso nesse nível. Em seguida, confira o efeito no comando, na API ou na interface de entrada.

**Pergunta de entrevista**

Como prever o próximo minuto mantendo a simulação atual intacta?

## Testes versionados

Arquivos para começar a leitura dos cenários de teste: [Tests/Program.cs](../Tests/Program.cs), [tests/test_core.py](../tests/test_core.py).

Os comandos e versões de runtime usados no CI ficam em [.github/workflows/ci.yml](../.github/workflows/ci.yml), [.github/workflows/unity-core.yml](../.github/workflows/unity-core.yml).

[Voltar ao README](../README.md).
