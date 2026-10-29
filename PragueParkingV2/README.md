# Prague Parking V2.0

Ett parkeringssystem i **C# och .NET 10**, utvecklat av **Walter Esko**
inom utbildningen Systemutvecklare DevSecOps på KYH.

Projektet innehåller V1.0, V1.1 och den objektorienterade versionen V2.0.

## Starta programmet

För att köra projektet behövs .NET 10 SDK och Visual Studio
med stöd för .NET 10.

1. Klona eller ladda ner hela repot.
2. Öppna `prague-parking.slnx` i Visual Studio.
3. Vänta tills NuGet-paketen har återställts.
4. Högerklicka på `PragueParkingV2` och välj Set as Startup Project.
5. Starta med F5 eller Ctrl+F5.

Menyerna styrs med piltangenterna och Enter.

## Funktioner i V2.0

- Parkera bilar och motorcyklar.
- Flytta fordon till en plats där de ryms.
- Hämta ut fordon och visa parkeringstid och avgift.
- Söka med hela eller delar av registreringsnumret.
- Visa en detaljerad parkeringslista.
- Visa en parkeringskarta med färger och statusbokstäver.
- Läsa om timpriser utan att starta om programmet.
- Spara parkerade fordon i JSON och återställa dem vid start.

Data sparas efter varje lyckad parkering, flytt och uthämtning.
Om sparningen misslyckas återställs ändringen i minnet.

## Projektstruktur

- `PragueParkingV1.0`: första versionen.
- `PragueParkingV1.1`: vidareutveckling av första versionen.
- `PragueParkingV2`: konsolgränssnitt, fordonsklasser och parkeringsregler.
- `PragueParking.DataAccess`: läsning, sparning och validering av filer.

V2.0 använder klasserna Vehicle, Car, MC, ParkingSpot och ParkingGarage.
Car och MC ärver från Vehicle.

## Konfiguration

Inställningar och timpriser finns i `PragueParkingV2/config.json`.

Standardinställningarna är:

- 100 parkeringsplatser.
- Standardkapacitet 4 per plats.
- Bil har storlek 4.
- MC har storlek 2.
- Bil kostar 20 CZK per påbörjad timme.
- MC kostar 10 CZK per påbörjad timme.

En vanlig plats rymmer alltså en bil eller två MC.

Enskilda platser kan ha en annan kapacitet genom SpotCapacities:

```json
"SpotCapacities": {
  "100": 8
}
```

Det betyder att plats 100 har kapacitet 8. Övriga platser använder
standardkapaciteten. Plats 100 rymmer då två bilar eller fyra MC.

Konfigurationen kontrolleras vid inläsning. Ogiltiga värden,
dubbla fordonstyper och parkeringsdata som inte ryms avvisas.

## Filer under körning

Vid bygge kopieras konfigurationsfilen till programmets körmapp.
Vid vanlig Debug-körning är det:

`PragueParkingV2/bin/Debug/net10.0/`

Programmet läser `config.json` och sparar aktuell parkering
i `parking.json` i den mappen.

För att ändra priser medan programmet körs:

1. Redigera `config.json` i körmappen och spara.
2. Välj menyvalet Läs om priser.
3. Kontrollera de aktuella priserna som visas.

Detta menyval uppdaterar bara priserna. Ändringar av antal platser
och fordonsstorlekar börjar gälla vid nästa programstart.

Ändra även projektets konfigurationsfil om inställningarna ska
följa med framtida byggen.

## Startdata

Projektet innehåller en `parking.json` med fyra testfordon.

Vid bygge kopieras den till körmappen endast om ingen datafil
redan finns där. Befintliga parkeringar skrivs inte över vid bygge.
Det gäller även om garaget har tömts.

Testfordonen har fasta ankomsttider. Deras avgifter beror därför
på hur lång tid som gått när programmet testas.

## Avgiftsregler

Parkering till och med tio minuter är gratis.

Efter gratisgränsen debiteras hela parkeringstiden per påbörjad timme.
Gratisperioden dras inte av från parkeringstiden.

Exempel för bil:

- 9 minuter: 0 CZK.
- 11 minuter: 20 CZK.
- 60 minuter: 20 CZK.
- 61 minuter: 40 CZK.

Ankomsttiden bevaras vid flytt och omstart.

## Parkeringskarta

- Grön, L: helt ledig plats.
- Gul, D: delvis upptagen plats.
- Röd, F: full plats.

Kartan visar även antalet helt lediga platser.
Kartan visar åtta platser per rad, uppdelade i grupper om
3, 2 och 3 platser med körgångar mellan grupperna.
Med 100 platser blir det 12 fulla rader och en sista rad med fyra platser.

## Testning

Manuella tester har genomförts för parkering, flytt, uthämtning,
sökning, avgifter, prisomladdning, olika platsstorlekar och omstart.

En ren körkopia har också kontrollerats på utvecklingsdatorn:

- Projektet bygger utan gamla bin- och obj-mappar.
- Konfiguration och testdata följer med.
- Ändrade parkeringsdata bevaras vid nytt bygge.
- Ett tomt garage förblir tomt vid nytt bygge.
- Programmet startar och visar kartan.

Körning på en annan dator är ännu inte verifierad.

## Begränsningar i V2.0

- Endast bil och MC kan parkeras.
- Avgiften beräknas och visas; programmet genomför ingen betalning.
- Kartan är schematisk.
