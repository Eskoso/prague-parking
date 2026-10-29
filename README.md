
# Prague Parking V1 & V1.1

Ett parkeringssystem utvecklat i **C# och .NET 10** av **Walter Esko** som en del av kursen Objektorienterad programmering på KYH, utbildningen Systemutvecklare DevSecOps.

Systemet är utvecklat med fokus på enkel användning, tydlig struktur och effektiv hantering av parkeringsplatser med hjälp av spectre.console.

### Funktioner

Applikationen innehåller följande funktioner:

- **Parkera fordon:** Registrera en bil eller motorcykel och tilldela automatiskt en lämplig parkeringsplats.
- **Flytta fordon:** Flytta ett parkerat fordon till en annan tillgänglig plats.
- **Hämta ut fordon:** Ta bort ett fordon från parkeringen och frigöra parkeringsplatsen.
- **Sök efter fordon:** Hitta parkerade fordon genom hela eller delar av registreringsnumret.
- **Visa parkeringsstatus:** Visa samtliga parkeringsplatser med information om vilka fordon som finns parkerade.
- **Inputvalidering:** Kontrollera registreringsnummer och hantera felaktig användarinmatning.

### Parkeringsregler

Parkeringsanläggningen har totalt **100 parkeringsplatser**.

Varje parkeringsplats kan innehålla:

- En bil
- En motorcykel
- Två motorcyklar
- Inget fordon

När en motorcykel ska parkeras söker programmet först efter en plats där det redan står en ensam motorcykel för att utnyttja parkeringsytan effektivt.

Fordon identifieras genom registreringsnummer med maximalt **10 tecken**.

---

## Projektversioner

### Prague Parking V1.0

Grundversionen innehåller de huvudsakliga funktionerna för att parkera, flytta, söka efter och hämta ut fordon.

### Prague Parking V1.1

Den vidareutvecklade versionen fokuserar på användarvänlighet och förbättrad felhantering.

**Förbättringar:**

- Förbättrad validering av registreringsnummer.
- Stöd för bokstäver som Å, Ä, Ö och Ü.
- Möjlighet att försöka igen vid felaktig inmatning.
- Skydd mot dubbla registreringsnummer.
- Förbättrad hantering när parkeringen är full.

---

## Tekniker

| Teknik | Användningsområde |
|---|---|
| C# | Programmeringsspråk |
| .NET 10 | Applikationsplattform |
| Spectre.Console | Textbaserat användargränssnitt |
| Visual Studio | Utvecklingsmiljö |
| Git & GitHub | Versionshantering och inlämning |

---

## Installation och körning

### Starta via Visual Studio

1. Klona eller ladda ner projektet från GitHub.
2. Öppna filen `prague-parking.slnx` i Visual Studio.
3. Vänta tills Visual Studio har återställt nödvändiga NuGet-paket.
4. Högerklicka på önskat projekt i Solution Explorer.
5. Välj **Set as Startup Project**.
6. Starta applikationen med `F5` eller `Ctrl + F5`.

## Användning

När programmet startas visas en interaktiv huvudmeny:

```text
Välkommen till Prague Parking!

Vad vill du göra?

> 1. Parkera fordon
  2. Flytta fordon
  3. Hämta ut fordon
  4. Sök efter fordon
  5. Visa parkering
  0. Avsluta
```

Navigera med **piltangenterna** och bekräfta ditt val med **Enter**.

### Exempel på fordonsregistrering

En bil med registreringsnummer `ABC123` lagras som:

```text
CAR#ABC123
```

Två motorcyklar på samma parkeringsplats lagras som:

```text
MC#ABC123|MC#DEF456
```

Systemet använder en array med 100 element för att representera parkeringsplatserna.

---

## Testning

Applikationen har genomgått manuell testning för att kontrollera funktionalitet, felhantering och användarvänlighet.

Testningen omfattade:

- Registrering av bilar och motorcyklar.
- Dubbelparkering av motorcyklar.
- Flyttning och uthämtning av fordon.
- Sökning efter registreringsnummer.
- Felaktig användarinmatning.
- Kontroll av dubbla registreringsnummer.
- Full parkering med 100 bilar.
- Full parkering med 200 motorcyklar.
- Kontroll av parkeringsstatus och platsnumrering.
- Hantering av full parkering vid flyttning.

### Testresultat

| Testresultat | Antal |
|---|---:|
| Genomförda tester | 31 |
| Godkända tester efter korrigering | 31 |
| Kvarstående fel i testprotokollet | 0 |

Under testningen upptäcktes ett problem i flyttfunktionen där programmet kunde krascha om inga tillgängliga parkeringsplatser fanns.

Problemet åtgärdades genom att kontrollera antalet tillgängliga platser innan valmenyn visades.

Efter korrigeringen genomfördes berörda tester igen och godkändes.

---

## Begränsningar

- Maximalt 100 parkeringsplatser.
- Maximalt två motorcyklar per plats.
- Ingen permanent lagring av parkeringsinformation.
- Ingen hantering av betalningar eller parkeringsavgifter.
- Ingen automatisk tidsregistrering av parkerade fordon.

---

## Om projektet

Projektet är utvecklat som en individuell inlämningsuppgift inom utbildningen Systemutvecklare DevSecOps på KYH.

| Information | Beskrivning |
|---|---|
| Utvecklare | Walter Esko |
| Utbildning | Systemutvecklare DevSecOps |
| Skola | KYH |
| Kurs | Objektorienterad programmering |
| Programmeringsspråk | C# |
| Plattform | .NET 10 |

---

## Prague Parking V2.0

Den objektorienterade versionen med JSON-lagring,
konfiguration, avgifter och parkeringskarta.

[Läs dokumentationen för V2.0](PragueParkingV2/README.md)

---

*Prague Parking är ett utbildningsprojekt utvecklat för att öva på programmeringslogik, arrayer, metoder, felhantering och användarinteraktion i C#.*

