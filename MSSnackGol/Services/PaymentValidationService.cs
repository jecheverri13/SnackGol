namespace MSSnackGol.Services
{
    /// <summary>
    /// Servicio para validaciones de métodos de pago
    /// </summary>
    public interface IPaymentValidationService
    {
        bool ValidateCreditCardLuhn(string cardNumber);
        bool ValidateNequi(string nequiNumber);
    }

    /// <summary>
    /// Implementación del servicio de validación de pagos
    /// </summary>
    public class PaymentValidationService : IPaymentValidationService
    {
        /// <summary>
        /// Valida un número de tarjeta de crédito usando el algoritmo de Luhn
        /// </summary>
        /// <param name="cardNumber">Número de tarjeta (sin espacios)</param>
        /// <returns>true si es válido, false si no</returns>
        public bool ValidateCreditCardLuhn(string cardNumber)
        {
            if (string.IsNullOrWhiteSpace(cardNumber) || !cardNumber.All(char.IsDigit))
                return false;

            int sum = 0;
            bool alternate = false;
            for (int i = cardNumber.Length - 1; i >= 0; i--)
            {
                int digit = cardNumber[i] - '0';
                if (alternate)
                {
                    digit *= 2;
                    if (digit > 9)
                        digit -= 9;
                }
                sum += digit;
                alternate = !alternate;
            }
            return sum % 10 == 0;
        }

        /// <summary>
        /// Valida un número de Nequi (10 dígitos comenzando con 3)
        /// </summary>
        /// <param name="nequiNumber">Número de Nequi</param>
        /// <returns>true si es válido, false si no</returns>
        public bool ValidateNequi(string nequiNumber)
        {
            if (string.IsNullOrWhiteSpace(nequiNumber))
                return false;

            // Verificar que tenga exactamente 10 dígitos
            if (nequiNumber.Length != 10)
                return false;

            // Verificar que todos sean dígitos
            if (!nequiNumber.All(char.IsDigit))
                return false;

            // Verificar que comience con 3
            if (!nequiNumber.StartsWith("3"))
                return false;

            return true;
        }
    }
}
