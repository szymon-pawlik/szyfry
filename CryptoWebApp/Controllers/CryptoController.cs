using Microsoft.AspNetCore.Mvc;
using CryptoWebApp.Models;
using System.Text;

namespace CryptoWebApp.Controllers
{
    public class CryptoController : Controller
    {
        [HttpPost]
        public IActionResult Process(CryptoFormModel formModel)
        {
            if (ModelState.IsValid)
            {
                // Jeśli algorytm to Polibius, nie ustawiaj klucza
                if (formModel.Algorithm == "Polibius")
                {
                    formModel.Key = null; // Klucz nie jest potrzebny
                }

                var resultModel = new CryptoModel
                {
                    Algorithm = formModel.Algorithm,
                    InputText = formModel.InputText,
                    Key = formModel.Key, // Będzie nullem, jeśli Polibius
                    Operation = formModel.Operation,
                    Result = ProcessAlgorithm(formModel) // Przetwórz algorytm
                };

                return View("Result", resultModel);
            }

            return RedirectToAction("Index", "Home");
        }

        private string ProcessAlgorithm(CryptoFormModel model)
        {
            switch (model.Algorithm)
            {
                case "Caesar":
                    return ProcessCaesar(model.InputText, model.Key, model.Operation);
                case "Vigenere":
                    return ProcessVigenere(model.InputText, model.Key, model.Operation);
                case "Playfair":
                    return ProcessPlayfair(model.InputText, model.Key, model.Operation);
                case "Polibius":
                    return ProcessPolibius(model.InputText, model.Operation); // Klucz ignorowany
                default:
                    return "Nieobsługiwany algorytm.";
            }
        }

        private string ProcessCaesar(string input, string key, string operation)
        {
            // Sprawdzamy, czy klucz jest liczbą całkowitą
            if (!int.TryParse(key, out int shift))
                return "Błędny klucz. Podaj liczbę całkowitą.";

            const string alphabet = "aąbcćdeęfghijklłmnńoóprsśtuwyzźżqxv";  // Używamy polskiego alfabetu
            char[] result = new char[input.Length];

            // Iterujemy po każdym znaku w tekście wejściowym
            for (int i = 0; i < input.Length; i++)
            {
                char currentChar = char.ToLower(input[i]);  // Zmiana na małą literę
                if (alphabet.Contains(currentChar))  // Sprawdzamy, czy znak znajduje się w alfabecie
                {
                    int index = alphabet.IndexOf(currentChar);  // Znajdujemy indeks litery w alfabecie
                    int newIndex = operation == "Encrypt"
                        ? (index + shift) % alphabet.Length  // Jeśli operacja to "Encrypt", przesuwamy w prawo
                        : (index - shift + alphabet.Length) % alphabet.Length;  // Jeśli operacja to "Decrypt", przesuwamy w lewo
                    result[i] = alphabet[newIndex];  // Ustawiamy nową literę w wyniku
                }
                else
                {
                    result[i] = currentChar;  // Inne znaki (np. spacje, cyfry, znaki interpunkcyjne) pozostają bez zmian
                }
            }

            return new string(result);  // Zwracamy przetworzony tekst
        }


        private string ProcessVigenere(string input, string key, string operation)
        {
            if (string.IsNullOrWhiteSpace(key) || !key.All(char.IsLetter))
            {
                return "Klucz musi zawierać tylko litery.";
            }

            key = key.ToLower();
            input = input.ToLower();
            var result = new char[input.Length];

            int keyIndex = 0;
            for (int i = 0; i < input.Length; i++)
            {
                if (char.IsLetter(input[i]))
                {
                    int inputCharIndex = input[i] - 'a';
                    int keyCharIndex = key[keyIndex % key.Length] - 'a';

                    int newIndex = operation == "Encrypt"
                        ? (inputCharIndex + keyCharIndex) % 26
                        : (inputCharIndex - keyCharIndex + 26) % 26;

                    result[i] = (char)(newIndex + 'a');
                    keyIndex++;
                }
                else
                {
                    result[i] = input[i];
                }
            }

            return new string(result);
        }

        private string ProcessPlayfair(string text, string key, string operation)
        {
            char[,] keyTable = GeneratePlayfairKeyTable(key);
            text = PreparePlayfairText(text);

            StringBuilder result = new StringBuilder();

            for (int i = 0; i < text.Length; i += 2)
            {
                char a = text[i];
                char b = text[i + 1];

                var (rowA, colA) = FindPosition(keyTable, a);
                var (rowB, colB) = FindPosition(keyTable, b);

                if (rowA == rowB)
                {
                    colA = operation == "Encrypt" ? (colA + 1) % 5 : (colA + 4) % 5;
                    colB = operation == "Encrypt" ? (colB + 1) % 5 : (colB + 4) % 5;
                }
                else if (colA == colB)
                {
                    rowA = operation == "Encrypt" ? (rowA + 1) % 5 : (rowA + 4) % 5;
                    rowB = operation == "Encrypt" ? (rowB + 1) % 5 : (rowB + 4) % 5;
                }
                else
                {
                    int temp = colA;
                    colA = colB;
                    colB = temp;
                }

                result.Append(keyTable[rowA, colA]);
                result.Append(keyTable[rowB, colB]);
            }

            return result.ToString();
        }

        private char[,] GeneratePlayfairKeyTable(string key)
        {
            const string alphabet = "abcdefghiklmnopqrstuvwxyz";
            string normalizedKey = new string(key.ToLower().Replace("j", "i").Distinct().ToArray());
            string tableContent = normalizedKey + new string(alphabet.Where(c => !normalizedKey.Contains(c)).ToArray());

            char[,] keyTable = new char[5, 5];
            for (int i = 0; i < tableContent.Length; i++)
            {
                keyTable[i / 5, i % 5] = tableContent[i];
            }

            return keyTable;
        }

        private (int, int) FindPosition(char[,] keyTable, char letter)
        {
            for (int row = 0; row < 5; row++)
            {
                for (int col = 0; col < 5; col++)
                {
                    if (keyTable[row, col] == letter)
                        return (row, col);
                }
            }
            return (-1, -1);
        }

        private string PreparePlayfairText(string text)
        {
            text = text.Replace(" ", "").ToLower();
            if (text.Length % 2 != 0)
                text += 'x';
            return text;
        }

        private string ProcessPolibius(string input, string operation)
        {
            const string alphabet = "abcdefghiklmnopqrstuvwxyz"; // Bez litery 'j'
            const int size = 5; // Rozmiar tablicy 5x5
            char[,] table = new char[size, size];
            int index = 0;

            // Wypełnienie tablicy znakami alfabetu
            for (int row = 0; row < size; row++)
            {
                for (int col = 0; col < size; col++)
                {
                    table[row, col] = alphabet[index++];
                }
            }

            if (operation == "Encrypt")
            {
                StringBuilder encrypted = new StringBuilder();
                foreach (char c in input.ToLower())
                {
                    if (c == 'j') // Zamiana 'j' na 'i'
                    {
                        encrypted.Append(FindPosition('i', table));
                    }
                    else if (alphabet.Contains(c)) // Znaki będące w alfabecie
                    {
                        encrypted.Append(FindPosition(c, table));
                    }
                    else // Zachowujemy inne znaki (np. spacje, cyfry, znaki interpunkcyjne)
                    {
                        encrypted.Append(c);
                    }
                }
                return encrypted.ToString();
            }
            else if (operation == "Decrypt")
            {
                StringBuilder decrypted = new StringBuilder();
                int i = 0;

                while (i < input.Length)
                {
                    if (i + 1 < input.Length && char.IsDigit(input[i]) && char.IsDigit(input[i + 1]))
                    {
                        int row = input[i] - '1';
                        int col = input[i + 1] - '1';

                        if (row >= 0 && row < size && col >= 0 && col < size)
                        {
                            decrypted.Append(table[row, col]);
                        }
                        else
                        {
                            decrypted.Append("?");
                        }
                        i += 2;
                    }
                    else
                    {
                        decrypted.Append(input[i]);
                        i++;
                    }
                }
                return decrypted.ToString();
            }

            return "Nieobsługiwana operacja.";
        }

        private string FindPosition(char c, char[,] table)
        {
            for (int row = 0; row < table.GetLength(0); row++)
            {
                for (int col = 0; col < table.GetLength(1); col++)
                {
                    if (table[row, col] == c)
                        return $"{row + 1}{col + 1}";
                }
            }
            return "";
        }
    }
}
