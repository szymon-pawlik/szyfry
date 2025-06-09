🔐 CryptoTool - Narzędzie do Szyfrów Klasycznych CryptoTool to prosta aplikacja webowa zbudowana w technologii ASP.NET Core MVC, która pozwala na szyfrowanie i deszyfrowanie wiadomości przy użyciu czterech fundamentalnych, historycznych algorytmów kryptograficznych.

Projekt powstał w celach edukacyjnych, aby zaprezentować implementację i działanie klasycznych technik szyfrowania w nowoczesnym środowisku webowym.

✨ Kluczowe Funkcje Obsługa wielu algorytmów: Implementacja czterech popularnych szyfrów:

Szyfr Cezara (przesunięcie liczbowe)

Szyfr Vigenère'a (klucz słowny)

Szyfr Playfair (szyfrowanie digramów)

Siatka Polibiusza

Dwie operacje: Możliwość szyfrowania oraz deszyfrowania tekstu.

Interaktywny interfejs: Formularz dynamicznie dostosowuje pole na klucz w zależności od wybranego algorytmu (np. wymaga cyfr dla Szyfru Cezara).

Walidacja po stronie klienta: Prosty skrypt JavaScript zapewnia, że użytkownik wypełnił wszystkie wymagane pola przed wysłaniem formularza.

🛠️ Technologie Backend: C# / .NET (ASP.NET Core MVC)

Frontend: HTML, CSS, JavaScript (Vanilla JS)

Środowisko deweloperskie: Visual Studio / .NET CLI

🚀 Jak używać? Uruchom aplikację i otwórz ją w przeglądarce.

Z listy rozwijanej wybierz algorytm, którego chcesz użyć.

Wpisz tekst do przetworzenia.

Wprowadź klucz (jeśli jest wymagany przez dany algorytm).

Wybierz operację (Szyfrowanie / Deszyfrowanie).

Kliknij przycisk "Przetwarzaj", aby zobaczyć wynik.

⚙️ Instalacja i Uruchomienie Aby uruchomić projekt lokalnie, wykonaj poniższe kroki:

Sklonuj repozytorium:

git clone https://github.com/twoj-login/twoje-repozytorium.git

Przejdź do folderu projektu:

cd twoje-repozytorium

Przywróć zależności .NET:

dotnet restore

Uruchom aplikację:

dotnet run

Otwórz przeglądarkę i przejdź pod adres https://localhost:XXXX (dokładny port zostanie wyświetlony w konsoli).
