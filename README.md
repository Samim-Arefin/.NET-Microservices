There are a couple of microservices that implemented **e-commerce** modules over **Authentication, Catalog, Cart, Discount** and **Ordering** microservices with **NoSQL (MongoDB, Redis)** and **Relational databases (PostgreSQL, SQL Server)** with communicating over **RabbitMQ Event-Driven Communication** and using **Ocelot API Gateway**.

## What Includes In This Repository

#### Authentication Microservice 
* .NET 8 Core Web API application, Following REST API principles.
* Implementented **.NET Identity**.
* Authentication implementation using **JWT Bearer Token**.
* Used **Sql Server** as Database.
* Cross-cutting concerns Logging and global Exception Handling.

#### Catalog Microservice 
* .NET 8 Core Web API application, Following REST API principles, CRUD.
* Used **Repository Pattern** for CRUD operation.
* Used **MongoDb as Database**.
* Cross-cutting concerns Logging and global Exception Handling.
* Secure access through JWT-based authentication.

#### Cart Microservice
* .NET 8 Core Web API application, Following REST API principles, CRUD.
* Using **Redis** as a **Distributed Cache** over CartDb.
* Consume Discount **Grpc Service** for inter-service sync communication to calculate product final price.
* Publish CartCheckOutEvent Queue with using **MassTransit and RabbitMQ**.
* Secure access through JWT-based authentication.
  
#### Discount Microservice
* .NET **Grpc Server** application.
* Build a Highly Performant **inter-service gRPC Communication** with Cart Microservice.
* Exposing Grpc Services with creating **Protobuf messages**.
* Using **Dapper as ORM**.
* **PostgreSql Database** connection and containerization.
* Secure access through JWT-based authentication.

#### Microservices Communication
* Sync inter-service **gRPC Communication**.
* Implemented **authentication and authorization in gRPC using JWT-based authentication**.
* Async Microservices Communication with **RabbitMQ Message-Broker Service**.
* Using **RabbitMQ Publish/Subscribe Fanout** Exchange Model.
* Using **MassTransit** for abstraction over the RabbitMQ Message-Broker system.
* Publishing CartCheckOutEvent event queue from Cart microservices and Subscribing this event from Ordering microservices.	

#### Ordering Microservice
* .NET 8 Core Web API application, Following REST API principles, CRUD
* Implementing **CQRS, and Clean Architecture** with using Best Practices.
* Developing **CQRS using MediatR, FluentValidation, and AutoMapper packages**.
* Consuming **RabbitMQ** CartCheckOutEvent event queue with using **MassTransit-RabbitMQ** Configuration.
* **SqlServer Database** connection and containerization.
* Using **Dapper as ORM and Stored Procedure** to provide advanced database functionality.
	
#### Ocelot API Gateway Microservice
* Develop API Gateways with **Ocelot API Gateway** applying Gateway Routing Pattern.
* Ocelot API Gateway Configuration; **Route, Rate Limiting, Caching, Authentication and Authorization**.
