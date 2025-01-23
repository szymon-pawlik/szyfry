using System.ComponentModel.DataAnnotations;
using CryptoWebApp.Attributes;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CryptoWebApp.Models
{
    public class CryptoModel
    {
        [Required(ErrorMessage = "Wybierz algorytm.")]
        public string Algorithm { get; set; }

        [Required(ErrorMessage = "Podaj tekst.")]
        public string InputText { get; set; }

        // Klucz może być nullem, w zależności od algorytmu
        public string Key { get; set; }

        [Required(ErrorMessage = "Wybierz operację.")]
        public string Operation { get; set; }

        [BindNever] // Wykluczenie z bindingu i walidacji
        public string Result { get; set; }
    }
}