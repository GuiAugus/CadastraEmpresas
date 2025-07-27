# Projeto: Cadastro de Empresas

Este projeto é uma API RESTful para o cadastro de empresas com base no CNPJ, desenvolvida em .NET 9.0 com ASP.NET Core. A aplicação permite o registo e a autenticação de utilizadores e a integração com uma API externa (ReceitaWS) para obter os dados das empresas.

## 🚀 Tecnologias

- **Linguagem:** C#
- **Framework:** .NET 9.0 (ASP.NET Core Web API)
- **Banco de Dados:** SQL Server
- **ORM:** Entity Framework Core
- **Autenticação:** JWT (JSON Web Tokens)
- **Integração:** HttpClient para a API da ReceitaWS
- **Geração de Hash:** BCrypt.Net

## 📋 Pré-requisitos

Para executar este projeto, precisas de ter instalado na tua máquina:

- SDK do .NET 9.0 ou superior
- Um banco de dados SQL Server (podes usar o LocalDB que vem com o Visual Studio)
- Uma ferramenta de testes de API, como o Postman ou o Swagger UI

## ⚙️ Configuração do Projeto

1.  **Clonar o Repositório:**
    ```bash
    git clone https://github.com/GuiAugus/CadastraEmpresas
    cd CadastraEmpresas
    ```

2.  **Configurar a String de Conexão:**
    * Abre o ficheiro `appsettings.json`.
    * Edita a string de conexão `"DefaultConnection"` para que aponte para a tua instância do SQL Server. Por exemplo:
      `"Server=(localdb)\\mssqllocaldb;Database=CadastraEmpresasDB;Trusted_Connection=True;"`

3.  **Configurar a Chave Secreta do JWT:**
    * No mesmo ficheiro `appsettings.json`, atualiza a chave `"Jwt:Key"` para uma string longa e segura.

4.  **Criar o Banco de Dados e as Tabelas:**
    * A partir da pasta do projeto (`CadastraEmpresas.API`), executa os comandos para criar e aplicar as migrations:
      ```bash
      dotnet ef database update
      ```

## 💻 Como Executar

Para executar a aplicação, abre o terminal na pasta do projeto e executa o seguinte comando:

```bash
dotnet run

1. Autenticação de Usuário
Estes endpoints não requerem autenticação e servem para gerir o acesso à API.

POST /api/Auth/register

Descrição: Regista um novo utilizador.

Body (JSON):

JSON

{
  "nome": "João da Silva",
  "e-mail": "joao.silva@exemplo.com",
  "senha": "senhaSegura123"
}
POST /api/Auth/login

Descrição: Autentica um utilizador e retorna um token JWT.

Body (JSON):

JSON

{
  "e-mail": "joao.silva@exemplo.com",
  "senha": "senhaSegura123"
}

JSON

{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
2. Cadastro e Listagem de Empresas (Protegido por JWT)
Estes endpoints exigem que o utilizador esteja autenticado,
e o token JWT deve ser enviado no cabeçalho Authorization com o prefixo Bearer.

POST /api/Empresas/cadastrar

Descrição: Cadastra uma empresa consultando a API da ReceitaWS.
O utilizador logado deve poder cadastrar uma empresa informando apenas o CNPJ.
A aplicação deve consultar os dados no endpoint da ReceitaWS: 

GET //www.receitaws.com.br/v1/cnpj/{cnpj}.

Headers: Authorization: Bearer [TOKEN_JWT]

Body (JSON):

JSON

"33.372.251/0001-56"
GET /api/Empresas/listar

Descrição: Lista as empresas cadastradas pelo utilizador logado.

Headers: Authorization: Bearer [TOKEN_JWT]
