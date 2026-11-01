# Prague Parking V2.1

Ett parkeringssystem i **C# och .NET 10**, utvecklat av **Walter Esko**
inom utbildningen Systemutvecklare DevSecOps på KYH.

Projektet innehåller V1.0, V1.1, V2.0 och den vidareutvecklade versionen V2.1.
V2.0 finns kvar som ett separat körbart projekt.

## Starta programmet

För att köra projektet behövs .NET 10 SDK och Visual Studio
med stöd för .NET 10.

1. Klona eller ladda ner hela repot.
2. Öppna `prague-parking.slnx` i Visual Studio.
3. Vänta tills NuGet-paketen har återställts.
4. Högerklicka på `PragueParkingV2.1` och välj Set as Startup Project.
5. Starta med F5 eller Ctrl+F5.

Menyerna styrs med piltangenterna och Enter.

## Funktioner i V2.1

- Parkera bilar, motorcyklar, cyklar och bussar.
- Låta mindre fordon dela plats när kapaciteten räcker.
- Flytta fordon till platser där de ryms.
- Hämta ut fordon och visa parkeringstid och avgift.
- Söka med hela eller delar av registreringsnumret eller cykel-ID:t.
- Visa en detaljerad parkeringslista.
- Visa en parkeringskarta med färger och statusbokstäver.
- Läsa om hela konfigurationen utan att starta om programmet.
- Spara parkerade fordon i JSON och återställa dem vid start.
- Skapa en saknad datafil och fylla den med testfordon vid programstart.

Data sparas efter varje lyckad parkering, flytt och uthämtning.
Om sparningen misslyckas återställs ändringen i minnet.

Cyklar identifieras med ett unikt cykel-ID, exempelvis "CYKEL001",
som personalen anger vid parkering. Registreringsnummer och cykel-ID
måste vara unika och innehålla 1–10 bokstäver eller siffror.

## Projektstruktur

- `PragueParkingV1.0`: första versionen.
- `PragueParkingV1.1`: vidareutveckling av första versionen.
- `PragueParkingV2`: den objektorienterade versionen V2.0.
- `PragueParkingV2.1`: konsolgränssnitt, fordonsklasser och parkeringsregler för V2.1.
- `PragueParking.DataAccess`: läsning, sparning och validering av filer.

V2.1 använder klasserna Vehicle, Car, MC, Bicycle, Bus,
ParkingSpot och ParkingGarage. Car, MC, Bicycle och Bus ärver från Vehicle.

Fordon och parkeringsplatser hanteras främst genom interfacen
IVehicle och IParkingSpot.

V2.1 använder ConfigFileV21, ParkingFileV21 och VehicleDataV21
i klassbiblioteket. V2.0 använder sina ursprungliga filklasser.

## Konfiguration

Inställningar och priser finns i `PragueParkingV2.1/config.json`.

Standardinställningarna är:

- 100 parkeringsplatser.
- Standardkapacitet 4 per plats.

| Fordon | Typ i konfigurationen | Storlek | CZK per påbörjad timme |
|---|---|---:|---:|
| Bil | CAR | 4 | 20 |
| MC | MC | 2 | 10 |
| Cykel | BICYCLE | 1 | 5 |
| Buss | BUS | 16 | 80 |

En vanlig plats rymmer alltså en bil, två MC eller fyra cyklar.
Mindre fordon kan också dela plats i olika kombinationer när de ryms.

Enskilda platser kan ha en annan kapacitet genom SpotCapacities:

```json
"SpotCapacities": {
  "100": 8
}
```

Det betyder att plats 100 har kapacitet 8. Övriga platser använder
standardkapaciteten. Plats 100 rymmer exempelvis två bilar eller fyra MC.

En buss reserverar fyra hela platser i nummerföljd, eg. 15-18. Alla måste
ligga inom plats 1–50, där takhöjden är tillräcklig. Platsernas kapacitet 
måste räcka och inga andra fordon får stå där. Vid flytt får målplatserna 
överlappa bussens egna platser.

Konfigurationen kontrolleras vid start. Ogiltiga värden,
dubbla fordonstyper och parkeringsdata som inte ryms avvisas.
CAR och MC måste finnas i konfigurationen. BICYCLE och BUS kan läggas till.

## Filer under körning

Vid bygge kopieras konfigurationsfilen till programmets körmapp.
Vid vanlig Debug-körning är det:

`PragueParkingV2.1/bin/Debug/net10.0/`

Programmet läser `config.json` och sparar aktuell parkering
i `parking.json` i den mappen.

För att ändra inställningar medan programmet körs:

1. Redigera `config.json` i körmappen och spara.
2. Välj menyvalet Läs om konfiguration.
3. Kontrollera de aktiva inställningarna som visas.

Menyvalet läser om antal platser, platskapaciteter, fordonstyper,
fordonsstorlekar och priser. De nya inställningarna kontrolleras
mot alla parkerade fordon innan de börjar användas.

Det går exempelvis inte att ta bort en upptagen plats eller minska
en kapacitet så att parkerade fordon inte längre ryms.
Om ändringen avvisas behålls tidigare inställningar och garage i minnet.

En avvisad ändring finns fortfarande kvar i den redigerade JSON-filen.
Filen behöver alltså rättas till innan nästa start.

Ändra även projektets konfigurationsfil om inställningarna ska
följa med framtida byggen.

## Startdata

Om `parking.json` saknas vid programstart skapar programmet ett tomt
garage och försöker parkera ett testfordon per konfigurerad fordonstyp.
De fordon som ryms sparas i den nya datafilen.

Med standardinställningarna skapas fyra testfordon: en bil, en MC,
en cykel och en buss. Deras ankomsttid sätts till två timmar före
skapandet, så att även avgifter kan kontrolleras.

En befintlig datafil skrivs inte över med testfordon. Det gäller
även ett tomt garage med innehållet `[]`. Felaktiga data avvisas
och ersätts inte med testdata.

Varje fordon sparas en gång med en lista av platsnummer i SpotNumbers.
En buss har fyra platsnummer i samma post. Det äldre formatet med
ett enda SpotNumber kan också läsas och omvandlas till det nya
formatet vid nästa lyckade sparning.

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

Bussens fyra platser visas som fulla. I sökresultat och fordonsval
visas bussen som ett enda fordon. Bekräftelser vid parkering, flytt
och uthämtning visar samtliga platsnummer.

## Testning

Hela solution har byggts utan varningar eller fel.

V2.1 har hittills testats manuellt med hjälp av en checklista för:

- Parkering av bil, MC, cykel och buss.
- Validering av registreringsnummer och cykel-ID.
- Sökning, parkeringslista och karta.
- Flytt och uthämtning av fordon.
- Parkeringstid och avgifter.
- Sparning och omstart.
- Skapande av testdata när datafilen saknas.
- Omladdning av konfigurationen.

Det finns ingen automatiserad testsvit i repot.
Bygge och körning på en annan dator har verifierats av Walter.

## Reflektion

Den personliga reflektionen för V2.0 och V2.1 lämnas in separat som PDF.

## Begränsningar i V2.1

- Fordonstyperna som stöds är bil, MC, cykel och buss.
- Bussar reserverar alltid fyra hela platser, även om platskapaciteterna varierar.
- Cykel-ID anges manuellt av personalen.
- Avgiften beräknas och visas; programmet genomför ingen betalning.
- Kartan är schematisk.
