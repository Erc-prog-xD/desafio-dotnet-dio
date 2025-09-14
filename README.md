# 🚀 ASP.NET Core Web API com Docker, SQL Server e RabbitMQ

## 🧑‍💻 Tecnologias Utilizadas

- ASP.NET Core Web API
- Entity Framework Core
- RabbitMQ (Login: guest , Password: guest)
- SQL Server
- Docker / Docker Compose

## 📦 Estrutura

├── ApiGateway/             # Projeto ASP.NET Core Web API
-  ├── Proprieties/
-   ├── Controllers/
-   ├── Data/
-   ├── DTO/
-   ├── Enum/
-   ├── Migration/
-   ├── Models/
-   ├── Services/ 
- appsettings.json
- program.cs

## Para preparar o ambiente
- .NET VERSION 8.0
-  docker run -d --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management
- "DefaultConnection": "Server={nome do server};Database={nome do database};User={seu user};Password={sua senha}; TrustServerCertificate=True;"
## Para executar
- Clone o repositorio
- Instale as dependências
- Ao configurar o SQL, vá ao console e faça: add-migration {qualquer nome} depois faça update-migration. (Isso fará subir as tabelas no banco de dados)
- Rode a imagem do rabbit no docker
- Com tudo configurado execute o projeto e faça a execução das APIs no swagger (use o comando .dotnet run)

## 📊 Estrutura das Entidades
# Client

| Campo          | Tipo     | Descrição              |
| -------------- | -------- | ---------------------- |
| `Id`           | int      | ID do cliente          |
| `Name`         | string   | Nome                   |
| `Email`        | string   | Email                  |
| `PasswordHash` | byte\[]  | Hash da senha          |
| `PasswordSalt` | byte\[]  | Salt usado para o hash |
| `authorized`   | bool     | Cliente está ativo     |
| `CreatedAt`    | DateTime | Data de criação        |

# Product
| Campo         | Tipo    | Descrição          |
| ------------- | ------- | ------------------ |
| `Productid`   | int     | ID do produto      |
| `Name`        | string  | Nome               |
| `Price`       | decimal | Preço              |
| `Description` | string  | Descrição          |
| `Stock`       | int     | Estoque disponível |

# Pedidos
| Campo          | Tipo        | Descrição                |
| -------------- | ----------- | ------------------------ |
| `IdPedido`     | int         | ID do pedido             |
| `Product`      | Product     | Produto pedido           |
| `Client`       | Client      | Cliente que fez o pedido |
| `status`       | PedidosEnum | Status do pedido         |
| `quantidade`   | int         | Quantidade               |
| `CreationDate` | DateTime    | Data de criação          |

# PedidoHistoricStatus
| Campo                    | Tipo        | Descrição         |
| ------------------------ | ----------- | ----------------- |
| `PedidoHistoricStatusId` | int         | ID do histórico   |
| `PedidosIdPedidos`       | int         | ID do pedido      |
| `Status`                 | PedidosEnum | Status registrado |
| `CreationDate`           | DateTime    | Data da alteração |

# Notification
| Campo                  | Tipo     | Descrição           |
| ---------------------- | -------- | ------------------- |
| `NotificationId`       | int      | ID da notificação   |
| `Client`               | Client   | Cliente relacionado |
| `Product`              | Product  | Produto relacionado |
| `NotificationMenssage` | string   | Mensagem enviada    |
| `CreationDate`         | DateTime | Data de envio       |



