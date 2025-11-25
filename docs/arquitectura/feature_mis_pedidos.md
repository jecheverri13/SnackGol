# Feature: Mis Pedidos - SnackGol

## Descripción General

La funcionalidad "Mis Pedidos" permite a los usuarios visualizar el historial y estado de sus pedidos realizados en SnackGol, incluyendo los productos ordenados, código de retiro y código QR para recoger.

## Arquitectura

### Componentes Involucrados

```
┌─────────────────────────────────────────────────────────────────┐
│                         FRONTEND                                 │
├─────────────────────────────────────────────────────────────────┤
│  PedidosController.cs          │  Controlador MVC               │
│  PedidosViewModel.cs           │  ViewModels para la vista      │
│  Views/Pedidos/Index.cshtml    │  Lista de pedidos              │
│  Views/Pedidos/Detalle.cshtml  │  Detalle con QR                │
│  SessionTokenHelper.cs         │  Gestión de token persistente  │
└─────────────────────────────────────────────────────────────────┘
                                │
                                ▼ HTTP API
┌─────────────────────────────────────────────────────────────────┐
│                          BACKEND                                 │
├─────────────────────────────────────────────────────────────────┤
│  OrderManagementController.cs  │  API REST de pedidos           │
│  QRGeneratorService.cs         │  Generación de QR              │
│  ApplicationDbContext.cs       │  Acceso a datos EF Core        │
└─────────────────────────────────────────────────────────────────┘
```

## Flujo de Usuario

```
┌──────────────┐     ┌─────────────┐     ┌──────────────┐     ┌──────────────┐
│   Checkout   │────►│  Pedido     │────►│ Mis Pedidos  │────►│   Ver QR     │
│   exitoso    │     │  creado     │     │   (lista)    │     │  (detalle)   │
└──────────────┘     └─────────────┘     └──────────────┘     └──────────────┘
```

## Endpoints API Utilizados

### GET /api/OrderManagement/Session/{sessionToken}
Obtiene todos los pedidos asociados a un token de sesión.

**Request:**
```http
GET /api/OrderManagement/Session/abc123def456
X-Session-Token: abc123def456
```

**Response:**
```json
{
  "success": true,
  "status": 200,
  "response": [
    {
      "orderId": "ORD-20251124-1234",
      "status": "Confirmed",
      "total": 15500.00,
      "orderDate": "2025-11-24T15:30:00Z",
      "pickupCode": "ABC123",
      "itemCount": 3,
      "items": [
        {
          "productName": "Hot Dog Clásico",
          "quantity": 2,
          "unitPrice": 5000.00,
          "subtotal": 10000.00
        },
        {
          "productName": "Gaseosa 500ml",
          "quantity": 1,
          "unitPrice": 5500.00,
          "subtotal": 5500.00
        }
      ]
    }
  ]
}
```

### GET /api/OrderManagement/{orderId}/pickup
Obtiene el detalle completo de un pedido con su código QR.

**Response:**
```json
{
  "success": true,
  "status": 200,
  "response": {
    "orderId": "ORD-20251124-1234",
    "status": "Confirmed",
    "total": 15500.00,
    "pickupCode": "ABC123",
    "pickupQrImageBase64": "data:image/png;base64,...",
    "items": [...]
  }
}
```

## Persistencia del Token de Sesión

El sistema utiliza cookies persistentes para mantener la asociación entre el usuario y sus pedidos:

```csharp
// SessionTokenHelper.cs
public static string GetOrCreate(HttpContext context)
{
    // 1. Verificar Session (para la sesión actual)
    // 2. Verificar Cookie persistente (30 días)
    // 3. Crear nuevo token si no existe
    // 4. Persistir en ambos almacenes
}
```

**Cookie Configuration:**
- Nombre: `SnackGol_SessionToken`
- Duración: 30 días
- HttpOnly: true
- Secure: true (en producción)
- SameSite: Lax

## ViewModels

### PedidosViewModel
```csharp
public class PedidosViewModel
{
    public List<OrderSummaryViewModel> Orders { get; set; }
}
```

### OrderSummaryViewModel
```csharp
public class OrderSummaryViewModel
{
    public string OrderId { get; set; }
    public string Status { get; set; }
    public string StatusDisplay { get; set; }  // "Confirmado", "Preparando", etc.
    public string StatusClass { get; set; }    // CSS class para estilos
    public double Total { get; set; }
    public DateTime OrderDate { get; set; }
    public string? PickupCode { get; set; }
    public int ItemCount { get; set; }
    public List<OrderItemViewModel> Items { get; set; }
    
    public bool IsActive => Status != "Delivered";
    public bool IsReadyForPickup => Status == "ReadyForPickup";
}
```

### OrderItemViewModel
```csharp
public class OrderItemViewModel
{
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public double UnitPrice { get; set; }
    public double Subtotal { get; set; }
}
```

## Estados del Pedido en la UI

| Estado | Badge Color | Icono | Mensaje |
|--------|-------------|-------|---------|
| Confirmed | `bg-primary` | 📋 | "Confirmado" |
| Preparing | `bg-warning` | 🔥 | "Preparando" |
| ReadyForPickup | `bg-success` | 🔔 | "¡Listo para recoger!" |
| Delivered | `bg-secondary` | ✅ | "Entregado" |

## Barra de Progreso Visual

```
[●]───────[●]───────[○]───────[○]
Confirmado  Preparando  Listo    Entregado
```

## Consideraciones de Seguridad

1. **Validación de propiedad**: Solo se muestran pedidos del token de sesión actual
2. **Cookies HttpOnly**: Previene acceso via JavaScript
3. **Token único**: UUID v4 generado por el servidor
4. **Sin datos sensibles**: El QR no contiene información personal

## Tests

Ver: `SnackGol.Tests/Controllers/OrderManagementControllerTests.cs`

- `GetOrdersBySession_WithValidToken_ReturnsOrders`
- `GetOrdersBySession_WithInvalidToken_ReturnsNotFound`
- `Checkout_CreatesOrderWithCorrectStatus`

## Rutas del Frontend

| Ruta | Controlador | Acción | Descripción |
|------|-------------|--------|-------------|
| `/Pedidos` | PedidosController | Index | Lista de pedidos |
| `/Pedidos/Detalle/{id}` | PedidosController | Detalle | Detalle con QR |

## Dependencias

- Bootstrap 5 (UI)
- Bootstrap Icons (iconografía)
- System.Text.Json (serialización)
- HttpClientFactory (llamadas API)
