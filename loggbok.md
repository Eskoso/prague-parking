# Loggbok – Prague Parking V1.0, V1.1, V2.0 och V2.1

**Namn:** Walter Esko  
**Utbildning:** Systemutvecklare DevSecOps, KYH  
**Kurs:** Objektorienterad programmering  
**Projekt:** Prague Parking V1.0, V1.1, V2.0 och V2.1
**Programmeringsspråk:** C#  
**Utvecklingsmiljö:** Visual Studio, .NET 10

## Arbetstillfälle 1 – Planering och grundfunktioner

**Datum:** 2026-09-18

Jag började med att läsa igenom uppgiftsbeskrivningen och planera hur parkeringssystemet skulle fungera. Uppgiften gick ut på att skapa ett system med 100 parkeringsplatser där varje plats kunde innehålla en bil eller max två motorcyklar.

Jag skapade projektet i Visual Studio och använde en array med 100 element för att lagra fordonen. Varje fordon lagrades som en sträng med fordonstyp och registreringsnummer, exempelvis CAR#ABC123 eller MC#ABC123.

Jag valde att använda Spectre.Console för att skapa en tydligare och mer användarvänlig huvudmeny där användaren kan navigera med piltangenterna.

Därefter utveckla jag funktionen för att parkera fordon. Programmet söker automatiskt efter en ledig parkeringsplats och försöker placera två motorcyklar tillsammans för att spara utrymme.

Jag började också dela upp programmet i separata metoder för att göra koden mer strukturerad och lättare att arbeta med.

## Arbetstillfälle 2 – Färdigställande av V1.0 och vidareutveckling till V1.1

**Datum:** 2026-09-25

Jag fortsatte utvecklingen genom att skapa funktionerna för att flytta, hämta ut och söka efter fordon.

En av utmaningarna var att hantera två motorcyklar på samma parkeringsplats. Om en motorcykel flyttades eller hämtades ut behövde den andra finnas kvar. Jag löste detta genom att använda Split() för att dela upp fordonsinformationen och sedan uppdatera arrayen.

Jag skapade även en sökfunktion där användaren kan söka efter hela eller delar av ett registreringsnummer. Därefter utveckla jag en parkeringsöversikt som visar alla 100 platser. Med hjälp av Spectre.Console visas lediga platser grönt och upptagna platser rött.

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

## Prague Parking V2.0

### Arbetstillfälle 1 – Objektorienterad struktur

**Datum:** 2026-10-23

Jag utgick från V1.1-koden som kopierats till projektet PragueParkingV2.
Jag skapade klasserna Vehicle, Car, MC, ParkingSpot och ParkingGarage.

Car och MC fick ärva gemensamma egenskaper från Vehicle. Jag anpassade
parkering, flytt, uthämtning och sökning till objekt istället för
strängar i en array.

Under arbetet testade jag bland annat att två MC kan dela plats och
att den ena finns kvar när den andra flyttas eller hämtas ut.

### Arbetstillfälle 2 – Filhantering, konfiguration och avgifter

**Datum:** 2026-10-26

Jag la till klassbiblioteket PragueParking.DataAccess för filhantering.
Parkeringsdata sparas nu i JSON efter parkering, flytt och uthämtning
och läses in när programmet startar.

Jag la till config.json för antal platser, platskapacitet,
fordonsstorlekar och timpriser. Jag valde att samla priser och
inställningar i samma fil (Config.json).

Jag la också till beräkning av parkeringstid och avgift vid uthämtning,
samt ett menyval för att läsa in priserna igen utan omstart.

Ett problem var att projektets config.json och kopian i körmappen är
olika filer. Jag kontrollerade vilken fil programmet faktiskt läser.

Jag testade att fordon, placeringar och ankomsttider finns kvar efter
omstart. Jag kontrollerade även att ogiltiga inställningar och priser 
avvisas och att tidigare priser behålls vid misslyckad prisomladdning.

### Arbetstillfälle 3 – Parkeringskarta, platsstorlekar och sluttestning

**Datum:** 2026-10-29

Jag la till en parkeringskarta som visar lediga, delvis upptagna och
fulla platser med både färger och bokstäver. Med 100 platser visas kartan 
med 8 platser per rad, uppdelade i grupper om 3, 2 och 3 platser med körgångar 
mellan grupperna. Det blir 12 fulla rader och en sista rad med 4 platser.

Jag gick igenom stödet för olika platsstorlekar och förenkla
kontrollerna till tydligare if-satser och loopar. Plats 100 fick
kapacitet 8, medan standardkapaciteten är 4.

Jag testade att två bilar ryms på plats 100. När platsen är full
försvinner den som alternativ i flyttmenyn. Jag kontrollerade också
att kartan visar rätt efter förändringar.

Jag genomförde manuella sluttester av grundfunktionerna. En separat
ren körkopia kontrollerades också för att verifiera bygge, startdata
och att befintliga parkeringsdata inte skrivs över.

Jag arbetade därefter med dokumentationen för V2.0 inför inlämningen.

## Prague Parking V2.1

### Arbetstillfälle 1 – Interface, nya fordonstyper och testning

**Datum:** 2026-11-01

Jag kopierade projektet PragueParkingV2 till PragueParkingV2.1 och la in den i samma solution. Jag ändrade namespace till PragueParkingV2_1 samt behöll V2.0 som eget körbart projekt.

Jag la till interfacen IVehicle och IParkingSpot och anpassade
listor, parametrar och returvärden så att parkeringskoden använder dem korrekt.
Under arbetet gick jag igenom skillnaden mellan ett interface som
beskriver ett kontrakt och basklassen Vehicle som samlar gemensam kod.

Jag skapade klasserna Bicycle och Bus med arv från Vehicle.
Konfigurationen fick cyklar med storlek 1 och timpris 5 CZK samt
bussar med storlek 16 och timpris 80 CZK. Eftersom cyklar saknar
registreringsskylt valde jag att personalen anger ett unikt cykel-ID, eg. CYKEL001.

För bussarna valde jag modellen att en buss reserverar fyra hela
platser i nummerföljd inom plats 1–50, det är mitt lösningsval.
Jag anpassade parkering, flytt, uthämtning och kartan för den nya modellen.
Vid flytt får bussens nya platser överlappa de egna, men inga andra
fordon får blockera platserna. Samma bussobjekt används på alla fyra 
platser så att ankomsttiden bevaras.

En viktig del var att bussen inte skulle räknas som fyra olika fordon.
Jag använde .Distinct() i sök och fordonsval. I datafilen sparas varje 
fordon en gång med en lista av platsnummer i SpotNumbers. Bekräftelserna 
visar också alla platser.

Jag la till ConfigFileV21, ParkingFileV21 och VehicleDataV21 i klassbiblioteket 
för att kunna göra V2.1 utan att ändra orgiginala V2.0:s klasser. Läsaren kan 
även läsa det äldre formatet med SpotNumber.

Jag ersatte byggkopieringen av testdata med att programmet själv
skapar datafilen när den saknas, testfordonen placeras med vanliga
parkeringslogiken. En befintlig fil, även ett tomt garage med [],
ska fortsätta användas och felaktiga data ska inte ersättas med testfordonen.

Meny för prisomladdning ändrades till Läs om konfiguration.
Programmet läser först de nya inställningarna och försöker bygga ett
nytt garage med de befintliga fordonens placeringar och ankomsttider.
När allt har godkänts ersätts det aktiva garaget och konfigurationen. 
Vid fel behålls de tidigare inställningarna i minnet.

Jag testade programmet manuellt med hjälp av en checklista för bland
annat parkering, validering, sökning, karta, flytt, uthämtning, avgifter,
sparning, omstart och omladdning av konfigurationen. Hittills har jag
arbetat med manuell testning och inte skrivit eller kört automatiska
tester själv. Körning på en annan fysisk dator är inte separat
dokumenterad här.

- **Parkering:** Parkera bil, MC, cykel och buss samt kontrollera att fordonen får rätt antal platser.
- **Delade platser:** Kontrollera att flera mindre fordon kan dela en plats när kapaciteten räcker.
- **Validering:** Prova tomma, för långa och ogiltiga registreringsnummer samt dubbla registreringsnummer och cykel-ID.
- **Sökning:** Söka med hela och delar av registreringsnummer eller cykel-ID.
- **Parkeringskarta:** Kontrollera att lediga, delvis upptagna och fulla platser visas korrekt.
- **Flytt:** Flytta fordon och kontrollera att övriga fordon står kvar. För bussar testades även överlappning med egna platser och gränsen vid plats 50.
- **Uthämtning:** Hämta ut fordon och kontrollera att rätt platser frigörs.
- **Avgifter:** Kontrollera gratisperioden och avgifterna för de olika fordonstyperna.
- **Sparning och omstart:** Kontrollera att fordon, placeringar och ankomsttider bevaras.
- **Testdata:** Kontrollera att en saknad datafil skapas och att en befintlig fil, även ett tomt garage, bevaras.
- **Konfiguration:** Läsa om ändrade inställningar och kontrollera att ogiltiga ändringar avvisas.

Jag uppdaterade V2.1 README, med beskrivningar av de nya funktionerna, filformatet, bussmodellen, konfigurationsomladdningen och testningen. Den personliga reflektionen
återstår att skriva utifrån mina egna erfarenheter av projektet och kursen.
