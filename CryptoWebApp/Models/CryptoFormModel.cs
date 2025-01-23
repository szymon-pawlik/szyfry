using System.ComponentModel.DataAnnotations;
using CryptoWebApp.Attributes;

public class CryptoFormModel
{
    [Required(ErrorMessage = "Wybierz algorytm.")]
    public string Algorithm { get; set; }

    [Required(ErrorMessage = "Podaj tekst.")]
    [StringLength(1000, ErrorMessage = "Tekst może mieć maksymalnie 1000 znaków.")]
    public string InputText { get; set; }

    [ValidateKey] // Niestandardowy walidator klucza
    public string Key { get; set; }

    [Required(ErrorMessage = "Wybierz operację.")]
    [RegularExpression("Encrypt|Decrypt", ErrorMessage = "Operacja musi być Encrypt lub Decrypt.")]
    public string Operation { get; set; }
}
