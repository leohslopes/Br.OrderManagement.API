<!DOCTYPE html>
<html lang="pt-BR">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
</head>

<body>

<h1>Br.OrderManagement API</h1>

<p>
    API de gerenciamento de pedidos desenvolvida em <strong>.NET 9</strong>,
    aplicando conceitos de <strong>Clean Architecture, Domain-Driven Design (DDD),
    Entity Framework Core e Domain Events</strong>.
</p>

<p>
    O objetivo do projeto é disponibilizar uma solução para gerenciamento de produtos
    e pedidos, contemplando regras de negócio, controle de estoque, persistência de
    dados e uma arquitetura preparada para evolução.
</p>


<h2>Tecnologias utilizadas</h2>

<h3>Back-end</h3>

<ul>
    <li>.NET 9</li>
    <li>ASP.NET Core Web API</li>
    <li>Entity Framework Core 9</li>
    <li>SQL Server</li>
    <li>MediatR</li>
    <li>FluentValidation</li>
    <li>xUnit</li>
    <li>FluentAssertions</li>
</ul>


<h2>Arquitetura</h2>

<pre>
Br.OrderManagement

├── API
│   ├── Controllers
│   ├── Middlewares
│   └── Configurações da aplicação
│
├── Application
│   ├── Services
│   ├── DTOs
│   └── Casos de uso
│
├── Domain
│   ├── Entities
│   ├── Aggregates
│   ├── Domain Events
│   └── Regras de negócio
│
├── Repository
│   ├── DbContext
│   ├── Migrations
│   ├── Configurations
│   └── Repositories
│
└── Tests
    └── Testes unitários
</pre>


<h2>Como executar o Back-end</h2>

<h3>Pré-requisitos</h3>

<ul>
    <li>.NET SDK 9</li>
    <li>SQL Server</li>
    <li>Visual Studio 2022 ou VS Code</li>
</ul>


<h2>Configuração do banco de dados</h2>

<p>
Configure a connection string no arquivo:
</p>

<pre>
Br.OrderManagement.API/appsettings.json
</pre>


<pre>
{
  "ConnectionStrings": {
    "DefaultConnection": 
    "Server=localhost;Database=BrOrderManagementDb;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
</pre>


<h2>Criando o banco de dados</h2>

<p>
O projeto utiliza Entity Framework Core Migrations.
</p>


<h3>Criar migration</h3>

<pre>
dotnet ef migrations add InitialCreate --project Br.OrderManagement.Repository --startup-project Br.OrderManagement.API --output-dir Persistence/Migrations
</pre>


<h3>Atualizar banco de dados</h3>

<pre>
dotnet ef database update --project Br.OrderManagement.Repository --startup-project Br.OrderManagement.API
</pre>


<p>
Após executar o comando, o banco <strong>BrOrderManagementDb</strong>
será criado automaticamente no SQL Server.
</p>


<h2>Cadastro de Produto</h2>

<p>
Exemplo de criação de produto:
</p>

<pre>
POST /api/products
</pre>


<pre>
{
  "name": "Notebook Dell",
  "description": "Notebook Dell Inspiron",
  "price": 4500.00,
  "stockQuantity": 10,
  "imageBase64": null
}
</pre>


<p>
A imagem do produto é opcional e armazenada em Base64 quando informada.
</p>


<h2>Executando a aplicação</h2>

<h3>Build</h3>

<pre>
dotnet build
</pre>


<h3>Executar API</h3>

<pre>
dotnet run --project Br.OrderManagement.API
</pre>


<p>
Swagger disponível em:
</p>

<pre>
https://localhost:&lt;porta&gt;/swagger
</pre>

<h2>Decisões técnicas relevantes</h2>


<h3>Clean Architecture</h3>

<ul>
    <li>Domain concentrando regras de negócio.</li>
    <li>Application responsável pelos casos de uso.</li>
    <li>Repository responsável pela persistência.</li>
    <li>API responsável pela comunicação HTTP.</li>
</ul>


<h3>Domain Driven Design (DDD)</h3>

<p>
Foram aplicados conceitos como:
</p>

<ul>
    <li>Aggregate Root</li>
    <li>Entidades ricas</li>
    <li>Encapsulamento das regras de negócio</li>
    <li>Domain Exceptions</li>
    <li>Domain Events</li>
</ul>


<h3>Domain Events</h3>

<p>
Ao confirmar um pedido, um evento de domínio é disparado:
</p>

<pre>
Pedido Confirmado

        |

        ↓

OrderConfirmedEvent

        |

        ↓

Atualização de estoque
</pre>


<p>
Essa abordagem mantém o agregado desacoplado de processos externos.
</p>


<h3>Unit Of Work</h3>

<pre>
Service

   |

Repository

   |

UnitOfWork

   |

Database
</pre>


<p>
Utilizado para controle das transações através do Entity Framework Core.
</p>


<h3>Tratamento de erros</h3>

<p>
Foi implementado middleware global para padronização das respostas.
</p>


<pre>
{
  "success": false,
  "message": "Produto não encontrado."
}
</pre>


<h2>Testes</h2>

<p>
Testes unitários utilizando:
</p>

<ul>
    <li>xUnit</li>
    <li>FluentAssertions</li>
</ul>


<p>
Executar testes:
</p>

<pre>
dotnet test
</pre>


<h2>Melhorias futuras</h2>

<ul>
    <li>Mensageria com RabbitMQ para processamento assíncrono dos Domain Events.</li>
    <li>Armazenamento de imagens utilizando Azure Blob ou AWS S3.</li>
    <li>Autenticação utilizando JWT.</li>
    <li>Cache utilizando Redis.</li>
    <li>Pipeline CI/CD.</li>
</ul>


</body>
</html>
