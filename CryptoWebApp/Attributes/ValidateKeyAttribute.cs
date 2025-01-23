using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CryptoWebApp.Attributes
{
    public class ValidateKeyAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var model = (CryptoFormModel)validationContext.ObjectInstance;

            // Sprawdzenie, czy algorytm został wybrany
            if (string.IsNullOrWhiteSpace(model.Algorithm))
            {
                return new ValidationResult("Algorytm nie został wybrany.");
            }

            // Obsługa szyfru Polibius - klucz nie jest wymagany
            if (model.Algorithm == "Polibius")
            {
                return ValidationResult.Success; // Klucz jest opcjonalny
            }

            // Klucz jest wymagany dla innych algorytmów
            if (string.IsNullOrWhiteSpace(value?.ToString()))
            {
                return new ValidationResult($"Klucz jest wymagany dla algorytmu {model.Algorithm}.");
            }

            // Walidacja klucza dla poszczególnych algorytmów
            switch (model.Algorithm)
            {
                case "Caesar":
                    if (!int.TryParse(value.ToString(), out _))
                    {
                        return new ValidationResult("Dla szyfru Cezara klucz musi być liczbą całkowitą.");
                    }
                    break;

                case "Playfair":
                    if (!Regex.IsMatch(value.ToString(), "^[a-zA-Z]+$"))
                    {
                        return new ValidationResult("Dla szyfru Playfair klucz może zawierać tylko litery.");
                    }
                    break;

                case "Vigenere":
                    if (!Regex.IsMatch(value.ToString(), "^[a-zA-Z]+$"))
                    {
                        return new ValidationResult("Dla szyfru Vigenere klucz musi składać się wyłącznie z liter.");
                    }
                    break;

                default:
                    return new ValidationResult($"Algorytm {model.Algorithm} nie jest obsługiwany.");
            }

            return ValidationResult.Success;
        }
    }
}
