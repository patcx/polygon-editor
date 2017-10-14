#Edytor wielokątów - podstawowa specyfikacja:

    *Możliwość dodawania nowego wielokąta, usuwania oraz edycji
    *Przy edycji:
        przesuwanie wierzchołka lub całego wielokąta
        usuwanie wierzchołka
        dodawanie wierzchołka w środku wybranej krawędzi
        opcjonalnie: przesuwanie całej krawędzi
    *Dodawanie ograniczeń (relacji) dla wybranej krawędzi:
        moźliwe ograniczenia: krawędź pozioma, krawędź pionowa
        A) - dana długość krawędzi
        B) - zadany kąt w wierzchołu (min: 90st)
        maksymalnie tylko jedno ograniczenie dla krawędzi
        dwie sąsiednie krawędzie nie mogą być obie pionowe lub obie poziome
        opcjonalnie: jeden punkt wielokąta (np. pierwszy wprowadzony) jest "usztywniony". Nie można go przesuwać i usuwać (chyba, że przesuwamy cały wielokąt)
        dodawanie wierzchołka na krawędzi lub usuwanie wierzchołka - usuwa ograniczenia "przyległych" krawędzi
        ustawione ograniczenia są widoczne (jako odpowiednie "ikonki") przy środku krawędzi/w wierzchołku
    *Rysowanie odcinków - własna implementacja - alg. Bresenhama

Proszę również o przygotowanie prostej dokumentacji (może być w notatniku) zawierającej:

    instrukcji obsługi - "klawiszologia"
    przyjętych założeń i opisu zaimplementowanego algorytmu "relacji"

Termin oddania zadania - tydzień: 26,27,30 października. W trakcie tych zajęć - część laboratoryjna.
