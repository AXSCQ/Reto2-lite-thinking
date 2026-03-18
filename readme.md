# Microservicio de Gestión de Órdenes (Reto 2 y 3)

## Descripción General
Este proyecto consiste en un microservicio básico para la gestión de órdenes de compra, diseñado originalmente para demostrar los principios de Clean Architecture y evolucionado en esta fase hacia un entorno de contenedores y orquestación distribuida. La aplicación permite el registro de pedidos y la validación de estados de pago, simulando un flujo de negocio real donde cada paso está protegido por reglas arquitectónicas.

## Arquitectura en Capas (Base del Reto 2)
Para garantizar la mantenibilidad y el desacoplamiento total de los componentes, el sistema se divide en cuatro capas estrictas:
- **Dominio (Domain):** Contiene la lógica central y las entidades de negocio (como `Order`). Es el núcleo del sistema y no tiene ninguna dependencia externa o de frameworks.
- **Aplicación (Application):** Orquestra los casos de uso, como la creación de órdenes. Define qué es lo que el sistema puede hacer mediante contratos e interfaces.
- **Infraestructura (Infrastructure):** Implementa los detalles técnicos, como la persistencia de datos y la comunicación con otros servicios mediante HttpClient.
- **API (Presentación):** Expone los puntos de entrada HTTP mediante Minimal APIs, gestionando la entrada de datos inicial y la configuración del contenedor de dependencias.

## Patrones de Diseño Utilizados
- **Patrón Repositorio:** Se implementó para mediar entre la capa de dominio y la persistencia física de los datos. Esto permite que la lógica de aplicación trabaje con abstracciones, facilitando cambios futuros en el motor de base de datos sin afectar el núcleo del sistema.
- **Inyección de Dependencias:** Utilizado para gestionar el ciclo de vida de los objetos y reducir el acoplamiento. Gracias a este patrón, el sistema entrega automáticamente las dependencias necesarias a cada capa, facilitando el mantenimiento y las pruebas unitarias distribuidas.

## Contenerización y Orquestación (Novedad del Reto 3)
La aplicación fue preparada para entornos de nube local mediante los siguientes componentes de infraestructura:
- **Dockerfile Multi-stage:** Se diseñó un proceso de construcción optimizado que separa la compilación (SDK pesado) de la ejecución (Runtime ligero), reduciendo significativamente el tamaño de la imagen final del servicio.
- **Docker Compose:** Configurado para orquestar la comunicación entre dos servicios en una red privada: la API de Órdenes y una API secundaria de Pagos. Esto permite validar la resolución de nombres DNS interna de Docker durante el procesamiento de pagos.
- **Kubernetes (K8s):** Se crearon manifiestos de `deployment.yaml` para gestionar la disponibilidad y replicas del servicio, junto con un `service.yaml` de tipo `NodePort` para exponer la aplicación hacia el host exterior de forma controlada en el puerto 30007.

## Pruebas Unitarias y Reglas de Negocio
Se implementaron pruebas automáticas utilizando xUnit y Moq para validar las reglas críticas antes del despliegue:
1. **Validación de Dominio:** Se garantiza mediante tests que toda nueva orden inicie estrictamente en estado "Creado".
2. **Validación de Aplicación:** Se implementó una lógica de protección que impide procesar un pago dos veces sobre la misma orden, lanzando una excepción controlada que fue validada con éxito en las pruebas de integración.

## Trade-offs y Decisiones Técnicas
Como arquitecto de esta solución, se asumieron las siguientes decisiones para equilibrar la agilidad del desarrollo con los requisitos del reto:
- **Trade-off de Persistencia en Memoria:** Se optó por utilizar colecciones en memoria (`List`) en lugar de un motor SQL relacional real. El beneficio es la velocidad de desarrollo y despliegue en Kubernetes sin la sobrecarga de configurar volúmenes persistentes en esta fase. Se asume que los datos son volátiles al reiniciar el pod.
- **Trade-off de Alcance Funcional:** El sistema se centra exclusivamente en el flujo ininterrumpido de creación y pago (Happy Path). Se dejaron fuera operaciones secundarias como la actualización o el borrado de registros para enfocar los esfuerzos en la calidad de los patrones y la robustez de la orquestación.
