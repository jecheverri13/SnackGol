# Prueba manual: Generar y validar QR de entrega

- **Fecha:** 2025-11-21
- **Ambiente:** ASP.NET Core 8 (backend `MSSnackGol`, frontend `MSSnackGolFrontend`) ejecutados en local.
- **Objetivo:** Confirmar end-to-end que el checkout genera el QR y que el punto de entrega puede validarlo.

## Pasos ejecutados

1. Levantar la API `MSSnackGol` (`dotnet run` en `MSSnackGol/`).
2. Levantar el frontend `MSSnackGolFrontend` (`dotnet run`).
3. Desde el navegador:
   - Abrir `/Carrito`.
   - Agregar dos productos al carrito y verificar el resumen.
   - Ejecutar checkout (botón “Confirmar pedido y generar QR”).
4. La aplicación redirige a `/QR/Confirmacion` mostrando el código QR con el detalle del pedido.
5. Guardar el payload base64 mostrado en la vista (se encuentra en `Pickup.QrImageBase64`).
6. Consumir manualmente el endpoint `GET /api/OrderManagement/{orderId}/pickup` y verificar que retorna `pickupCode`, `pickupPayloadBase64` y `pickupQrImageBase64`.
7. Escanear el QR con un lector móvil y obtener el token del payload (o decodificar `pickupPayloadBase64`).
8. Ejecutar `POST /api/OrderManagement/{orderId}/pickup/validate` con cuerpo `{ "token": "<token>" }`.

## Resultado esperado

- La respuesta del checkout devuelve `201 Created` con `pickup` poblado y el carrito queda vacío.
- La vista `/QR/Confirmacion` muestra el QR sin recargar la sesión.
- `GET /pickup` devuelve el mismo código y payload almacenados en BD.
- La validación responde `200 OK`, marca la orden como `Delivered` y no permite un segundo canje (responde `409 Conflict`).

## Observaciones

- Si la sesión no existía, se genera automáticamente con `SessionTokenHelper`.
- Se identificó que almacenar `CheckoutConfirmation` en TempData generaba demasiadas cookies; fue corregido usando `ISession`.
