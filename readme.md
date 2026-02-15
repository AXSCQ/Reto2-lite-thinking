Reto 2: Arquitectura, Calidad de Código y Diseño en Capas

Descripción General
Este proyecto es la solución al Reto 2 del Módulo 2. El objetivo principal fue construir una API en .NET desde cero aplicando los principios de Clean Architecture y una introducción práctica a Domain-Driven Design (DDD). 

En lugar de hacer un sistema gigante, me enfoqué en crear un flujo claro y funcional para un solo caso de uso: crear una orden y agregarle un producto, asegurando que cada capa tenga una responsabilidad única y no se mezclen entre sí.


Explicación de las Capas

Para mantener el código ordenado y escalable, dividí la solución en 4 proyectos distintos:

Domain (Dominio):Es el corazón del proyecto. Aquí viven las reglas de negocio, específicamente la clase `Order`. Esta capa es completamente independiente, no sabe nada de bases de datos, ni de internet, ni de frameworks externos.
Application (Aplicación):Aquí está la lógica de lo que el sistema "sabe hacer". Implementé el caso de uso `CreateOrderUseCase`, que funciona como un coordinador: recibe la instrucción de crear la orden y usa los contratos (interfaces) para mandarla a guardar.
Infrastructure (Infraestructura):Es la capa que hace el trabajo sucio de hablar con el mundo exterior. Aquí implementé `OrderRepository`,  donde se guardan los datos.
Api (Presentación): Es la ventanilla de atención. Usé Minimal APIs en .NET para exponer un endpoint `POST /orders` que recibe las peticiones por internet, configura las dependencias y delega el trabajo a la capa de Aplicación.



Patrones Utilizados y Justificación

Durante el desarrollo apliqué patrones de diseño solo donde realmente aportaban valor para desacoplar el código:

1. Patrón Repository (`IOrderRepository`): Lo usé para crear una frontera estricta entre mi lógica de negocio y la base de datos. Gracias a este contrato, el caso de uso no tiene idea de cómo se guardan los datos. Esto me permite cambiar la forma de guardar la información en el futuro sin tener que tocar ni romper la capa de Aplicación.

2. Inyección de Dependencias (Dependency Injection):
   Lo configuré en el `Program.cs` usando `AddSingleton` (para mantener la base de datos viva) y `AddScoped` (para los casos de uso). Lo usé para que el sistema le entregue automáticamente las herramientas (como el repositorio) al caso de uso cuando se crea, en lugar de instanciarlas a mano con `new`. Esto hace que el código sea mucho más fácil de probar y mantener.


Decisiones Arquitectónicas y Trade-offs Asumidos

Trade-off: Persistencia simulada en memoria en lugar de SQL:
Como el alcance del reto especificaba que "no se espera persistencia real compleja", decidí no instalar Entity Framework ni levantar un servidor de base de datos. Asumí el trade-off de usar una simple `List<Order>` en memoria dentro de `OrderRepository`. Esto me permitió ahorrar tiempo de configuración y enfocar todo el esfuerzo en lograr una separación de capas perfecta.

Decisión: Evitar un Modelo Anémico (Dominio Rico):
Al diseñar la entidad `Order`, tomé la decisión de usar `private set` en sus propiedades (como la lista de `Products`). En lugar de dejar la lista abierta para que cualquiera la modifique, implementé un comportamiento específico: la función `AddProduct(string productName)`. Esto protege la integridad de los datos de la orden, evitando que otra capa modifique o borre la lista por accidente.
