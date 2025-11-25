# Lógica de Estados de Pedidos - SnackGol

## Flujo de Estados

Los pedidos en SnackGol siguen un flujo lineal de estados:

```
┌───────────────┐     ┌──────────────┐     ┌─────────────────┐     ┌───────────────┐
│  Confirmed    │ ──► │  Preparing   │ ──► │ ReadyForPickup  │ ──► │   Delivered   │
│  (Confirmado) │     │(Preparando)  │     │(Listo p/recoger)│     │  (Entregado)  │
└───────────────┘     └──────────────┘     └─────────────────┘     └───────────────┘
```

## Descripción de cada Estado

| Estado | Descripción | Quién lo activa | Acción del cliente |
|--------|-------------|-----------------|-------------------|
| **Confirmed** | Pedido recibido y pagado | Sistema (automático al checkout) | Esperar |
| **Preparing** | Cocina preparando el pedido | Staff de cocina | Esperar |
| **ReadyForPickup** | Pedido listo para recoger | Staff de cocina | Ir a recoger |
| **Delivered** | Pedido entregado al cliente | Staff al escanear QR | Disfrutar 🍔 |

## API Endpoints para Gestión de Estados

### 1. Actualizar Estado (Staff/Admin)
```http
PATCH /api/OrderManagement/{orderId}/status
Content-Type: application/json

{
    "newStatus": "Preparing",    // Estados: Confirmed, Preparing, ReadyForPickup, Delivered
    "updatedBy": "staff_user",   // Opcional: quién hizo el cambio
    "forceUpdate": false         // Opcional: permite retroceder estados (solo correcciones)
}
```

**Respuesta exitosa:**
```json
{
    "isSuccess": true,
    "status": 200,
    "result": {
        "orderId": "ORD-ABC123",
        "previousStatus": "Confirmed",
        "newStatus": "Preparing",
        "updatedAt": "2025-11-24T15:30:00Z"
    }
}
```

### 2. Obtener Pedidos Activos (Panel Admin)
```http
GET /api/OrderManagement/Active
```

Devuelve todos los pedidos que NO están en estado `Delivered`.

### 3. Validar QR (Marcar como Entregado)
```http
POST /api/OrderManagement/{orderId}/pickup/validate
Content-Type: application/json

{
    "token": "abc123...",        // Token del QR escaneado
    "verified_by": "staff_name"  // Opcional: quién entregó
}
```

Este endpoint automáticamente:
- Valida el token del QR
- Cambia el estado a `Delivered`
- Registra la fecha y hora de entrega
- Previene entregas duplicadas

## Reglas de Negocio

### Transiciones Permitidas
- ✅ `Confirmed` → `Preparing`
- ✅ `Preparing` → `ReadyForPickup`
- ✅ `ReadyForPickup` → `Delivered`
- ✅ Cualquier estado → `Delivered` (via QR validation)

### Transiciones Bloqueadas (sin `forceUpdate`)
- ❌ `Preparing` → `Confirmed` (no retroceder)
- ❌ `ReadyForPickup` → `Preparing` (no retroceder)
- ❌ `Delivered` → cualquier otro (pedido ya entregado)

## Implementación Sugerida para Panel de Staff

### Pantalla de Cocina
```
┌─────────────────────────────────────────────────────────────────┐
│                    🍳 PEDIDOS EN COCINA                         │
├─────────────────────────────────────────────────────────────────┤
│ ┌─────────────────────┐  ┌─────────────────────┐               │
│ │ CONFIRMADOS (3)      │  │ EN PREPARACIÓN (2)  │               │
│ ├─────────────────────┤  ├─────────────────────┤               │
│ │ #ABC123 - 14:30     │  │ #DEF456 - 14:25    │               │
│ │ 2x Hamburguesa      │  │ 1x Pizza           │               │
│ │ 1x Papas            │  │ 2x Gaseosa         │               │
│ │ [▶ Preparar]        │  │ [✓ Listo]          │               │
│ └─────────────────────┘  └─────────────────────┘               │
└─────────────────────────────────────────────────────────────────┘
```

### Flujo de Trabajo del Staff

1. **Recibe pedido** → Aparece en columna "Confirmados"
2. **Click "Preparar"** → Llama `PATCH /status` con `Preparing`
3. **Termina de preparar** → Click "Listo" → Llama `PATCH /status` con `ReadyForPickup`
4. **Cliente llega** → Escanea QR → `POST /pickup/validate`

## Frontend del Cliente

La página "Mis Pedidos" (`/Pedidos`) muestra:

- **Pedidos activos** con barra de progreso visual
- **Estado actual** resaltado
- **Código de retiro** visible
- **Botón "Ver QR"** para mostrar el código

El cliente puede:
1. Ver el progreso de su pedido en tiempo real
2. Acceder a su QR desde la lista de pedidos
3. Ver historial de pedidos completados

## Endpoints Adicionales de Gestión

### Reset de Stock (Admin/Dev)
```http
POST /api/OrderManagement/ResetStock
```

Restablece el stock de todos los productos a 100 unidades. Útil para:
- Desarrollo y testing
- Reinicio de inventario demo
- Recuperación de stock agotado

**Respuesta:**
```json
{
    "success": true,
    "message": "Stock reset successful",
    "productsUpdated": 9,
    "newStock": 100
}
```

### Obtener Pedidos por Sesión
```http
GET /api/OrderManagement/Session/{sessionToken}
X-Session-Token: {sessionToken}
```

Devuelve todos los pedidos asociados a un token de sesión, **incluyendo la lista de productos** de cada pedido.

**Respuesta:**
```json
{
    "success": true,
    "response": [
        {
            "orderId": "ORD-ABC123",
            "status": "Confirmed",
            "total": 15500.00,
            "itemCount": 3,
            "items": [
                {
                    "productName": "Hot Dog Clásico",
                    "quantity": 2,
                    "unitPrice": 5000.00,
                    "subtotal": 10000.00
                }
            ]
        }
    ]
}
```

## Notas Técnicas

- Los estados se almacenan en `Order.status` (string)
- El QR contiene un token único hasheado para validación segura
- La fecha de entrega se registra en `Order.pickup_redeemed_at`
- El staff que entrega se registra en `Order.pickup_verified_by`
- Los items del pedido se obtienen de la relación `Order.OrderLines`
