# Loggbok – Prague Parking V1.0 och V1.1

**Namn:** Walter Esko  
**Utbildning:** Systemutvecklare DevSecOps, KYH  
**Kurs:** Objektorienterad programmering  
**Projekt:** Prague Parking V1 & V1.1
**Programmeringsspråk:** C#  
**Utvecklingsmiljö:** Visual Studio, .NET 10

## Arbetstillfälle 1 – Planering och grundfunktioner

**Datum:** 2026-09-18

Jag började med att läsa igenom uppgiftsbeskrivningen och planera hur parkeringssystemet skulle fungera. Uppgiften gick ut på att skapa ett system med 100 parkeringsplatser där varje plats kunde innehålla en bil eller max två motorcyklar.

Jag skapade projektet i Visual Studio och använde en array med 100 element för att lagra fordonen. Varje fordon lagrades som en sträng med fordonstyp och registreringsnummer, exempelvis CAR#ABC123 eller MC#ABC123.

Jag valde att använda Spectre.Console för att skapa en tydligare och mer användarvänlig huvudmeny där användaren kan navigera med piltangenterna.

Därefter utvecklade jag funktionen för att parkera fordon. Programmet söker automatiskt efter en ledig parkeringsplats och försöker placera två motorcyklar tillsammans för att spara utrymme.

Jag började också dela upp programmet i separata metoder för att göra koden mer strukturerad och lättare att arbeta med.

## Arbetstillfälle 2 – Färdigställande av V1.0 och vidareutveckling till V1.1

**Datum:** 2026-09-25

Jag fortsatte utvecklingen genom att skapa funktionerna för att flytta, hämta ut och söka efter fordon.

En av utmaningarna var att hantera två motorcyklar på samma parkeringsplats. Om en motorcykel flyttades eller hämtades ut behövde den andra finnas kvar. Jag löste detta genom att använda Split() för att dela upp fordonsinformationen och sedan uppdatera arrayen.

Jag skapade även en sökfunktion där användaren kan söka efter hela eller delar av ett registreringsnummer. Därefter utvecklade jag en parkeringsöversikt som visar alla 100 platser. Med hjälp av Spectre.Console visas lediga platser grönt och upptagna platser rött.

När grundfunktionerna fungerade gick jag vidare med V1.1.

Jag fokuserade och förbättrad inputvalidering och fixa småfel. Jag la till kontroller som säkerställer att registreringsnummer inte är tomma, inte innehåller mellanslag eller otillåtna specialtecken och inte överstiger 10 tecken.

Jag använde char.IsLetterOrDigit() för att kontrollera bokstäver och siffror samt while-loopar och continue för att användaren skulle kunna försöka igen vid felaktig inmatning.

Jag la också till möjligheten att skriva 0 för att återgå till huvudmenyn vid registrering och sökning.

## Arbetstillfälle 3 – Testning, felsökning och dokumentation

**Datum:** 2026-10-07

Under det sista arbetstillfället fokuserade jag på att testa systemet, åtgärda problem och förbereda projektet för inlämning.

Jag genomförde 31 manuella tester som omfattade parkering, flyttning, uthämtning, sökning, visualisering, inputvalidering och olika gränsfall.

Vid den första testningen blev 29 av 31 tester godkända. De två misslyckade testerna berodde på samma problem.

När parkeringen var full och användaren försökte flytta ett fordon kunde programmet krascha eftersom Spectre.Console försökte visa en SelectionPrompt utan några tillgängliga alternativ.

Felet löstes genom att kontrollera om listan availableSpots var tom innan menyn visades. Om inga platser finns tillgängliga får användaren istället ett tydligt felmeddelande och kan återgå till huvudmenyn.

Efter ändringen testade jag funktionen igen och alla 31 tester blev godkända.

## Arbetstillfälle 4 – Slutlig testning och förberedelse för inlämning

**Datum:** 2026-10-11

Under det sista arbetstillfället fokuserade jag på att färdigställa projektet och förbereda det för inlämning. Jag gick igenom projektstrukturen och såg till att både V1.0 och V1.1 låg i en gemensam mapp med en Solution-fil, så att båda versionerna enkelt kan öppnas och köras i Visual Studio.

Jag arbetade även med dokumentationen och skapade en README.md där jag beskrev projektets funktioner, vilka tekniker som använts och hur man installerar och startar programmet.

Syftet var att göra projektet så tydligt och lättillgängligt som möjligt, så att läraren enkelt kan granska koden, förstå hur systemet fungerar och testa de båda versionerna via GitHub.

## Sammanfattning

Under projektets gång har jag fått arbeta praktiskt med arrayer, stränghantering, loopar, villkorssatser, metoder och användarinmatning i C#.

Jag har också fått mer erfarenhet av att strukturera ett större program och hantera olika situationer som kan uppstå när användaren matar in felaktiga uppgifter.

En viktig lärdom var hur betydelsefull testning är. Programmet fungerade under normal användning, men genom att testa gränsfall upptäckte jag en bugg som annars hade kunnat finnas kvar vid inlämningen.

Efter felsökning och förbättringar fungerar samtliga testade funktioner och projektet är förberett för inlämning.