# SnackGol — Arquitectura de Software

Este documento describe la arquitectura de SnackGol y reúne los diagramas en PlantUML. Todos los artefactos están bajo `docs/arquitectura/` dentro de `carrito/SnackGol`.

## Tabla de contenido

1. [Descripción de la solución de software](#descripción-de-la-solución-de-software)
2. [Atributos de calidad](#atributos-de-calidad)
3. [Visión general de la infraestructura](#visión-general-de-la-infraestructura)
   - 3.1 [Listado de herramientas](#listado-de-herramientas-opcional)
4. [Vista lógica](#vista-lógica)
5. [Vista dinámica](#vista-dinámica-opcional)
6. [Vista conceptual](#vista-conceptual)

> Nota: Los diagramas están en PlantUML (`.puml`). Puedes previsualizarlos con la extensión “PlantUML” de VS Code.

## Descripción de la solución de software

SnackGol es una aplicación que permite a los espectadores de eventos deportivos ordenar productos desde sus asientos. El sistema evita filas y mejora la experiencia del evento. La capa cliente es una aplicación web MVC (ASP.NET Core) optimizada para móvil (diseño responsivo). El backend expone una API REST en ASP.NET Core 8 y persiste datos en PostgreSQL via EF Core (Npgsql).

Objetivos principales:

- Facilitar compra móvil durante eventos deportivos
- Reducir tiempos de espera con pedidos anticipados
- Mejorar la experiencia del espectador
- Optimizar gestión de inventario y ventas en tiempo real

Características principales:

- Clientes: catálogo con filtros, carrito, pedido y recolección
- Administradores: gestión de productos, categorías, inventario y reportes
- Plataforma: web (diseño responsivo para móvil)
- Tecnología: ASP.NET Core 8 (MVC) + ASP.NET Core 8 (API) + PostgreSQL

### Experiencia de usuario enfocada al estadio

Para cumplir los objetivos de aumentar ventas y entregar pedidos en menos de 20 minutos dentro del estadio, se plantean mejoras específicas en la capa de presentación:

- **Navbar adaptada a la tribuna:** incorporar en `_Layout.cshtml` un resumen del carrito con ícono y contador visible en todo momento. Así, el asistente sabe cuántos artículos lleva sin abandonar la vista móvil y puede confirmar su compra antes de que termine una jugada clave.
- **Paleta con identidad del club:** ajustar los colores definidos en `wwwroot/css/site.css` a la paleta del equipo o del estadio (por ejemplo, verdes y amarillos si se trata de Atlético Nacional). Esta personalización refuerza la sensación de aplicación oficial del evento.
- **Filtros por punto de entrega:** extender el catálogo (`Views/Producto/Index.cshtml`) con filtros por sección del estadio, como “Tribuna Norte”, “Palcos” o “General”. Al incluir esta metadata en la API, los runners pueden agrupar pedidos y reducir el tiempo de despacho.
- **Checkout rápido para 4G saturado:** añadir en `Views/Carrito/Index.cshtml` un botón destacado “Checkout rápido” que concentre los datos mínimos (ubicación y método de pago) y evite pasos innecesarios cuando la conectividad sea limitada.
- **Feedback en vivo:** utilizar `wwwroot/js/site.js` para mostrar toasts o banners cortos que confirmen acciones de carrito (agregar, actualizar, eliminar) sin recargar la página. Esta retroalimentación mantiene la confianza del usuario incluso cuando la API se ve afectada por alta concurrencia durante el partido.

## Atributos de calidad

ID | Atributo | Descripción
---|---|---
AC-01 | Usabilidad | Flujo de compra en < 2 min en móvil
AC-02 | Rendimiento | Catálogo < 2s y carrito < 1s
AC-03 | Seguridad | JWT (24h) para autenticación
AC-04 | Disponibilidad | 99% en eventos con recuperación automática
AC-05 | Modificabilidad | 5 capas en librerías independientes
AC-06 | Compatibilidad | Android 8+, iOS 13+, navegadores modernos
AC-07 | Validación | Stock validado antes de confirmar

## Visión general de la infraestructura

Diagrama de la infraestructura (cliente, API, BD, telemetría):

```plantuml
!include infraestructura.puml
```

### Listado de herramientas (Opcional)

Herramienta | Versión | Enlace
---|---|---
.NET SDK | 8.0+ | [https://dotnet.microsoft.com/download](https://dotnet.microsoft.com/download)
PostgreSQL | 14+ | [https://www.postgresql.org/download/](https://www.postgresql.org/download/)
Swagger/OpenAPI | 3.0+ | [https://swagger.io/](https://swagger.io/)
PlantUML | — | [https://plantuml.com/](https://plantuml.com/)

## Vista lógica

Diagrama de paquetes por capas:

```plantuml
!include logica.puml
```

Ejemplo simplificado (estilo paquetes e interfaz de servicios):

```plantuml
!include logica_ejemplo_simple.puml
```

Explicación de paquetes y responsabilidades:

- Capa 1 Presentación: Frontend web (ASP.NET Core MVC), controladores API y middleware.
- Capa 2 Lógica de Negocio: Servicios (producto, carrito, pedido, auth).
- Capa 3 Acceso a Datos: Repositorios + `ApplicationDbContext` (EF Core).
- Capa 4 Dominio: Entidades (`LibraryEntities`) y DTOs (`LibraryConnection.Dtos`).
- Capa 5 Infraestructura: Autenticación, encriptación, telemetría.

Interfaces de comunicación: UI → Controllers (HTTP/JSON), Controllers → Services (DI), Services → Repositories, Repositories → DbContext (EF Core), DbContext → PostgreSQL (Npgsql).

## Vista dinámica (opcional)

CU-P01: Visualizar productos

```plantuml
!include dinamica_cu_p01_visualizar_productos.puml
```

CU-P02: Agregar producto al carrito

```plantuml
!include dinamica_cu_p02_agregar_carrito.puml
```

CU-P03: Gestionar carrito de compras

```plantuml
!include dinamica_cu_p03_gestionar_carrito.puml
```

CU-P04: Obtener producto por ID

```plantuml
!include dinamica_cu_p04_obtener_producto.puml
```

## Vista conceptual

Diagrama de conceptos (entidades y relaciones):

```plantuml
!include conceptual.puml
```

Explicación resumida:

- User y Role gestionan credenciales y permisos; Client representa al espectador.
- Product pertenece a Category y es referenciado por `CartItem` y `OrderLine`.
- Cart agrupa `CartItem` por cliente/sesión; Order agrupa `OrderLine`.
- SalesPoint se deja como entidad conceptual planificada.

---

Para renderizar los diagramas localmente en VS Code:

1. Instala la extensión “PlantUML”.
2. Abre cualquier `.puml` y usa “Preview Current Diagram”.
3. Alternativamente, usa un servidor local de PlantUML o CI para exportar a PNG/SVG.

