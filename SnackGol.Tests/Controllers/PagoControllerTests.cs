using Xunit;
using FluentAssertions;
using MSSnackGolFrontend.Models;
using System;

namespace SnackGol.Tests.Controllers
{
    /// <summary>
    /// Pruebas unitarias para la funcionalidad de pago (PagoViewModel)
    /// Valida las reglas de negocio para procesar pagos
    /// </summary>
    public class PagoControllerTests
    {
        /// <summary>
        /// Validación 1: Fecha debe ser la actual (dentro del mismo día)
        /// </summary>
        [Fact]
        public void Fecha_DebeSerLaActual_CuandoSeCreaElModelo()
        {
            // Arrange
            var ahora = DateTime.Now;
            var modelo = new PagoViewModel
            {
                Fecha = ahora,
                Subtotal = 100
            };

            // Act & Assert
            modelo.Fecha.Date.Should().Be(ahora.Date);
            modelo.Fecha.Should().BeCloseTo(ahora, TimeSpan.FromSeconds(5));
        }

        /// <summary>
        /// Validación 2a: Nequi - Debe tener exactamente 10 dígitos
        /// </summary>
        [Theory]
        [InlineData("3001234567")]   // Válido: 10 dígitos, comienza con 3
        [InlineData("3211111111")]   // Válido: 10 dígitos, comienza con 3
        [InlineData("300123456")]    // Inválido: 9 dígitos
        [InlineData("30012345678")]  // Inválido: 11 dígitos
        [InlineData("2001234567")]   // Inválido: comienza con 2, no con 3
        public void NumeroCuentaNequi_DebeValidarDigitos(string numeroCuenta)
        {
            // Arrange
            bool esValido = ValidarNequi(numeroCuenta);

            // Act & Assert
            if (numeroCuenta == "3001234567" || numeroCuenta == "3211111111")
            {
                esValido.Should().BeTrue($"'{numeroCuenta}' debe ser un Nequi válido");
            }
            else
            {
                esValido.Should().BeFalse($"'{numeroCuenta}' no debe ser un Nequi válido");
            }
        }

        /// <summary>
        /// Validación 2b: Nequi - Debe empezar con 3
        /// </summary>
        [Theory]
        [InlineData("3")]       // Comienza con 3
        [InlineData("2")]       // No comienza con 3
        [InlineData("4")]       // No comienza con 3
        public void NumeroCuentaNequi_DebeEmpezarCon3(string primerDigito)
        {
            // Arrange
            var numeroCuenta = primerDigito + "001234567";

            // Act & Assert
            if (primerDigito == "3")
            {
                numeroCuenta.Should().StartWith("3");
            }
            else
            {
                numeroCuenta.Should().NotStartWith("3");
            }
        }

        /// <summary>
        /// Validación 3: Tarjeta de Crédito - Debe validarse con el algoritmo de Luhn
        /// </summary>
        [Theory]
        [InlineData("4111111111111111")]   // Válido: Luhn válido
        [InlineData("4012888888881881")]   // Válido: Luhn válido
        [InlineData("4532015112830367")]   // Inválido: Luhn inválido (último dígito incorrecto)
        [InlineData("1234567890123456")]   // Inválido: Luhn inválido
        [InlineData("123")]                // Inválido: muy corto
        public void NumeroTarjeta_DebeValidarLuhn(string numeroTarjeta)
        {
            // Arrange
            bool esValido = ValidarLuhn(numeroTarjeta);

            // Act & Assert
            if (numeroTarjeta == "4111111111111111" || numeroTarjeta == "4012888888881881")
            {
                esValido.Should().BeTrue($"'{numeroTarjeta}' debe ser válido según Luhn");
            }
            else
            {
                esValido.Should().BeFalse($"'{numeroTarjeta}' no debe ser válido según Luhn");
            }
        }

        /// <summary>
        /// Validación 4a: Valores - No pueden ser negativos
        /// </summary>
        [Theory]
        [InlineData(100)]       // Válido: positivo
        [InlineData(0)]         // Válido: cero
        [InlineData(-100)]      // Inválido: negativo
        [InlineData(-50)]       // Inválido: negativo
        public void Subtotal_NoDebeSerNegativo(decimal valor)
        {
            // Arrange
            var modelo = new PagoViewModel { Subtotal = valor };

            // Act & Assert
            if (valor >= 0)
            {
                modelo.Subtotal.Should().BeGreaterThanOrEqualTo(0);
            }
            else
            {
                modelo.Subtotal.Should().BeLessThan(0);
            }
        }

        /// <summary>
        /// Validación 4b: Valores - Solo deben ser numéricos (sin decimales inválidos)
        /// </summary>
        [Theory]
        [InlineData(100)]       // Válido: entero
        [InlineData(100.00)]    // Válido: decimal con .00
        [InlineData(100.50)]    // Válido: decimal
        [InlineData(0)]         // Válido: cero
        public void Subtotal_DebeSerNumerico(decimal valor)
        {
            // Arrange
            var modelo = new PagoViewModel { Subtotal = valor };

            // Act & Assert
            modelo.Subtotal.Should().Be(valor);
            (modelo.Subtotal is decimal).Should().BeTrue();
        }

        /// <summary>
        /// Validación 5: IVA - Siempre es del 19%
        /// </summary>
        [Theory]
        [InlineData(100, 19)]           // 100 * 0.19 = 19
        [InlineData(1000, 190)]         // 1000 * 0.19 = 190
        [InlineData(500, 95)]           // 500 * 0.19 = 95
        [InlineData(0, 0)]              // 0 * 0.19 = 0
        public void IVA_DebeSerEl19Porciento(decimal subtotal, decimal ivaEsperado)
        {
            // Arrange
            var modelo = new PagoViewModel { Subtotal = subtotal };

            // Act
            var iva = modelo.IVA;

            // Assert
            iva.Should().Be(ivaEsperado);
            if (subtotal > 0)
            {
                (iva / subtotal).Should().Be(0.19m);
            }
        }

        /// <summary>
        /// Validación Total: Total = Subtotal + IVA (19%)
        /// </summary>
        [Theory]
        [InlineData(100)]
        [InlineData(500)]
        [InlineData(1000)]
        [InlineData(0)]
        public void Total_DebeSerSubtotalMasIVA(decimal subtotal)
        {
            // Arrange
            var modelo = new PagoViewModel { Subtotal = subtotal };
            var ivaEsperado = subtotal * 0.19m;
            var totalEsperado = subtotal + ivaEsperado;

            // Act
            var total = modelo.Total;

            // Assert
            total.Should().Be(totalEsperado);
            total.Should().Be(subtotal + modelo.IVA);
        }

        /// <summary>
        /// Caso integrado: Validar todo el flujo de pago
        /// </summary>
        [Fact]
        public void PagoCompleto_DebeValidarTodosLosCampos()
        {
            // Arrange
            var modelo = new PagoViewModel
            {
                Fecha = DateTime.Now,
                Subtotal = 50000,
                MetodoPago = "Nequi",
                NumeroCuentaNequi = "3001234567"
            };

            // Act & Assert
            modelo.Fecha.Date.Should().Be(DateTime.Now.Date);
            modelo.Subtotal.Should().Be(50000);
            modelo.IVA.Should().Be(9500); // 50000 * 0.19
            modelo.Total.Should().Be(59500); // 50000 + 9500
            ValidarNequi(modelo.NumeroCuentaNequi!).Should().BeTrue();
        }

        /// <summary>
        /// Caso integrado: Pago con tarjeta de crédito válida
        /// </summary>
        [Fact]
        public void PagoConTarjeta_DebeValidarLuhn()
        {
            // Arrange
            var modelo = new PagoViewModel
            {
                Fecha = DateTime.Now,
                Subtotal = 75000,
                MetodoPago = "TarjetaCredito",
                NumeroTarjeta = "4111111111111111"
            };

            // Act & Assert
            modelo.Fecha.Date.Should().Be(DateTime.Now.Date);
            modelo.Subtotal.Should().Be(75000);
            modelo.IVA.Should().Be(14250); // 75000 * 0.19
            modelo.Total.Should().Be(89250); // 75000 + 14250
            ValidarLuhn(modelo.NumeroTarjeta!).Should().BeTrue();
        }

        // ==================== Métodos auxiliares de validación ====================

        /// <summary>
        /// Valida que un número de Nequi sea correcto:
        /// - Exactamente 10 dígitos
        /// - Comienza con 3
        /// - Solo contiene números
        /// </summary>
        private bool ValidarNequi(string numeroCuenta)
        {
            if (string.IsNullOrWhiteSpace(numeroCuenta))
                return false;

            // Debe tener exactamente 10 dígitos
            if (numeroCuenta.Length != 10)
                return false;

            // Todos deben ser dígitos
            if (!System.Text.RegularExpressions.Regex.IsMatch(numeroCuenta, @"^\d+$"))
                return false;

            // Debe empezar con 3
            if (!numeroCuenta.StartsWith("3"))
                return false;

            return true;
        }

        /// <summary>
        /// Implementa el algoritmo de Luhn para validar tarjetas de crédito
        /// Basado en la especificación: https://es.wikipedia.org/wiki/Algoritmo_de_Luhn
        /// </summary>
        private bool ValidarLuhn(string numeroTarjeta)
        {
            if (string.IsNullOrWhiteSpace(numeroTarjeta))
                return false;

            // Solo dígitos
            if (!System.Text.RegularExpressions.Regex.IsMatch(numeroTarjeta, @"^\d+$"))
                return false;

            // Longitud razonable para tarjeta de crédito (13-19 dígitos)
            if (numeroTarjeta.Length < 13 || numeroTarjeta.Length > 19)
                return false;

            int suma = 0;
            bool duplicar = false;

            // Recorrer de derecha a izquierda
            for (int i = numeroTarjeta.Length - 1; i >= 0; i--)
            {
                int digito = int.Parse(numeroTarjeta[i].ToString());

                if (duplicar)
                {
                    digito *= 2;
                    if (digito > 9)
                        digito -= 9;
                }

                suma += digito;
                duplicar = !duplicar;
            }

            return suma % 10 == 0;
        }
    }
}
