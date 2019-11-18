# TodoAppDdd
TodoApp Backend in .NET Core 2.2 with DDD, CQRS and EventSourcing approach

## Why?
I work on this project in order to understand DDD concepts and EventSourcing.

## Architecture
I used no DDD frameworks or anything else. The EventStore is hand-coded and very simple. There are 2 implementations. One saves the events to a text file and the other one works as in-memory list. I did this on purpose in order to keep it as simple as possible.

### Solution structure is:
* Core
  * Domain
  * App
  * App.Contracts
* Infrastructure
  * Bootstrapper
  * Persistence
* Presentation
  * Web
  * Api

#### Domain
The domain is the innermost core of the solution. It depends on nothing.
Aggregates, DomainEvents and pure business logic is placed here.

#### App
Command handler, query handler and mapping logic does live here.

#### App.Contracts
Just interfaces and DTOs.

#### Persistence
EventStore implementation lives here.

#### Web / Api
From here only App.Contracts is referenced. The domain is not visible from here. Only the commands/querys are known here and are used to forward them to their handlers. But only the interface of handlers is known here.

These projects are hosted here:  
Api: https://todoappdddapi.azurewebsites.net/api/todo  
Web: https://todoappdddweb.azurewebsites.net/  

You can test the api via TodoBackend.com:   
https://www.todobackend.com/client/index.html?https://todoappdddapi.azurewebsites.net/api/todo

#### Bootstrapper
This sticks together all dependencies and adds them to the dependency container by hand.
