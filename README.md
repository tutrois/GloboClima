# GloboClima 🌍

**Descrição**  
Bem-vindo ao projeto GloboClima, uma aplicação desenvolvida para oferecer aos usuários informações climáticas em tempo real e dados de diversos países, incluindo detalhes como população, idiomas, moedas e muito mais. Os usuários podem salvar suas cidades e países favoritos para consultas rápidas e fáceis no futuro.  

Este sistema foi criado com uma arquitetura backend .NET CORE 8 REST API e uma interface WEB APP MVC responsiva para exibição e interação com os dados.

URL API: https://zjruebwy7opiqotbuz7jcpddsy0rznwk.lambda-url.sa-east-1.on.aws/

---

## Funcionalidades

- **Consulta Climática**: Dados de clima atualizados em tempo real por cidade.
- **Favoritos**: Opção de salvar cidades e países para consulta futura.
- **Interface Responsiva**: Design otimizado para desktop e dispositivos móveis.

---

## Arquitetura

### Backend

- **API RESTful** desenvolvida em **.NET Core 8**, com **Identity** para autenticação e autorização de usuários.
- **Hospedagem na AWS**: O backend é implementado utilizando AWS Lambda ou EC2/ECS, garantindo escalabilidade e performance.
- **Banco de Dados**: **DynamoDB**, utilizado para armazenar informações sobre usuários, favoritos e dados de consulta.


### Frontend

- **Interface Web Responsiva**: Desenvolvida com foco em UX para permitir fácil acesso aos dados e favoritos.
- **Integração com o Backend**: Realiza chamadas à API para recuperação e atualização dos dados.

---

## Tecnologias Utilizadas

- **Frontend**: HTML, CSS, JavaScript, BootStrap
- **Backend**: .NET Core 8 com Identity para controle de acesso e segurança dos usuários
- **Banco de Dados**: DynamoDB (AWS), para armazenamento de dados climáticos e favoritos dos usuários
- **Infraestrutura na AWS**: AWS Lambda, DynamoDB

---


