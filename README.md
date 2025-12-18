# PdfFontAnalize
Analysis of docx to pdf conversion behavior with and without fonts using popular options


## Docker — budowa i uruchomienie obrazu
Poniżej znajdują się przykładowe komendy PowerShell do zbudowania i uruchomienia obrazu Docker dla projektu `AnalysisOfAspose`.

Budowa obrazu z katalogu repozytorium (kontekst = repo root):

```powershell
cd C:\REPO\private\PdfFontAnalize
docker build -f AnalysisOfAspose\Dockerfile -t analysis-of-aspose .
```

Budowa obrazu z katalogu `AnalysisOfAspose` — użyj kontekstu repozytorium (`..`) (ważne):

```powershell
# uruchomione z C:\REPO\private\PdfFontAnalize\AnalysisOfAspose
# wskazujemy Dockerfile w bieżącym katalogu, a jako kontekst przekazujemy katalog nadrzędny (repo root)
docker build -f Dockerfile -t analysis-of-aspose ..
```

Dlaczego to ma znaczenie?
- Błąd MSB1009 ("Project file does not exist") zwykle wynika z tego, że Docker build używa zbyt wąskiego kontekstu (np. `.` w katalogu `AnalysisOfAspose`) i nie ma dostępu do plików referencyjnych (sąsiednich projektów, katalogu `DocxFiles`, itp.).
- Najprostsze rozwiązanie to uruchomić build tak, aby kontekst zawierał cały repozytorium (przykłady powyżej).

Uruchomienie obrazu (szybki test):

```powershell
# uruchamia i używa domyślnego ENTRYPOINT
docker run --rm analysis-of-aspose
```

Uruchomienie z montowaniem katalogu `results` (wyniki zapisywane w lokalnym katalogu):

```powershell
# uruchomienie interaktywne, montowanie folderu results
cd C:\REPO\private\PdfFontAnalize
docker run --rm -it -v "${PWD}\\AnalysisOfAspose\\results:/data" -v "${PWD}\\AnalysisOfAspose\\Fonts:/app/Fonts:ro" analysis-of-aspose
```

Uruchomienie w tle z nazwą kontenera:

```powershell
docker run -d --name aspose -v "%CD%\\AnalysisOfAspose\\results:/data" -v "%CD%\\AnalysisOfAspose\\Fonts:/app/Fonts:ro" analysis-of-aspose
# sprawdź logi
docker logs -f aspose
```

Wskazówki:
- Dostosuj ścieżki montowania (`-v`) do swojej struktury katalogów.
- Jeśli chcesz mapować porty, dodaj `-p <hostPort>:<containerPort>` (nie dotyczy domyślnego tego projektu, jeśli nie nasłuchuje na porcie).
- Jeżeli obraz ma problemy z brakującymi czcionkami, możesz podać katalog `Fonts` z repo do `/app/Fonts` jak powyżej.
