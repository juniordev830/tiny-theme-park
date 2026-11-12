# Tiny Theme Park

Jogo de construção e administração de parque de diversões **em planejamento**, com escopo pequeno e foco em decisões compreensíveis.

> **Estado atual:** documentação inicial e licença; não existe engine, interface, simulação ou versão jogável nesta branch principal.

## Primeira experiência proposta

O jogador recebe um terreno com área limitada e orçamento fictício. Pode colocar uma atração, abrir caminhos, acompanhar visitantes simulados e conferir receitas e manutenção durante um dia de jogo.

## Regras mínimas

- Toda atração tem custo de construção e operação.
- Construir não pode deixar orçamento negativo.
- Caminhos conectam a entrada às atrações; visitante não deve atravessar áreas não conectadas.
- A atração fechada não gera receita e continua sujeita às regras definidas de manutenção.
- O avanço do tempo deve produzir os mesmos resultados com seed fixa.

## Critérios de aceitação

1. Colocar uma atração reduz o orçamento pelo preço exato.
2. Um caminho desconectado não recebe visitantes.
3. A capacidade máxima de cada atração é respeitada.
4. O resumo diário apresenta entradas e custos sem duplicar lançamentos.
5. O jogador consegue reiniciar o cenário sem carregar dados de sessão anterior.

## Roteiro de implementação

- [ ] Escolher engine e definir tamanho de grade.
- [ ] Criar classes de parque, atração e orçamento com testes.
- [ ] Implementar colocação de objeto e conexão de caminhos.
- [ ] Simular visitantes com dados fixos e HUD resumido.
- [ ] Testar build e documentar controles antes de publicar imagens como gameplay.

Multiplayer, monetização e física 3D complexa não fazem parte da primeira entrega.
