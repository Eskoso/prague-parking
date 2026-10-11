# Personlig reflektion – Prague Parking V1.0 och V1.1

**Namn:** Walter Esko  
**Utbildning:** Systemutvecklare DevSecOps, KYH  
**Kurs:** Objektorienterad programmering  
**Projekt:** Prague Parking V1 & V1.1

## 1. Sammanfattning

I den här uppgiften har jag utvecklat ett parkeringssystem i C# som ska kunna hantera bilar och motorcyklar på en parkering med 100 platser. Programmet gör det möjligt att parkera, flytta, söka efter och hämta ut fordon. Systemet ska även kunna hantera att två motorcyklar delar på samma parkeringsplats.

Jag började med att utveckla grundversionen, V1.0, och fortsatte sedan med V1.1 för att uppfylla kraven för VG. I den senare versionen fokuserade jag framför allt på att förbättra användarvänligheten, valideringen av användarinmatning och visualiseringen av parkeringsplatserna.

Det här projektet har varit en bra möjlighet att använda flera av de delar vi gått igenom i kursen och sätta ihop dem till ett större fungerande program.

## 2. Hur jag löste uppgiften

Jag började med att läsa igenom uppgiftsbeskrivningen och fundera på hur informationen skulle lagras. Eftersom uppgiften krävde att parkeringsplatserna skulle hanteras med en array av strängar använde jag en array med 100 element.

Varje element representerar en parkeringsplats. För att kunna lagra både fordonstyp och registreringsnummer i samma sträng använde jag ett format där informationen separeras med olika tecken. Exempelvis lagras en bil som CAR#ABC123 och två motorcyklar som MC#ABC123|MC#DEF456.

Jag delade upp programmet i olika metoder för parkering, flyttning, uthämtning, sökning och visning av parkeringsstatus. Det gjorde koden lättare att överblicka eftersom varje metod ansvarar för en specifik funktion.

Jag valde också att använda Spectre.Console istället för enbart vanliga Console.WriteLine(). Med hjälp av biblioteket kunde jag skapa interaktiva menyer och använda färger för att tydligare visa information och felmeddelanden.

När grundversionen fungerade vidareutvecklade jag programmet till V1.1. Jag la bland annat till förbättrad inputvalidering där användaren får försöka igen om ett ogiltigt registreringsnummer anges.

## 3. Utmaningar i uppgiften och hur jag löste dem

En av de största utmaningarna var att hantera två motorcyklar på samma parkeringsplats. Det var relativt enkelt att lagra ett fordon, men när två motorcyklar delar en plats blir det mer komplicerat att flytta eller hämta ut bara den ena.

Jag behövde se till att den andra motorcykeln inte försvann när informationen i arrayen uppdaterades. Jag löste det genom att använda Split() för att dela upp strängen och sedan behålla det fordon som inte skulle flyttas eller hämtas ut.

En annan utmaning var att hantera felaktig användarinmatning. I början skickades användaren tillbaka till huvudmenyn om ett registreringsnummer var ogiltigt. Det fungerade tekniskt, men var inte särskilt användarvänligt. Jag ändrade därför inmatningen till en while-loop som fortsätter fråga tills användaren anger ett giltigt värde.

Under den manuella testningen upptäckte jag även ett problem i flyttfunktionen. När parkeringen var full försökte programmet visa en meny med tillgängliga parkeringsplatser trots att listan var tom. Detta orsakade en InvalidOperationException från Spectre.Console.

Jag löste problemet genom att kontrollera om listan availableSpots innehöll några alternativ innan menyn visades. Om inga platser finns tillgängliga får användaren istället ett felmeddelande.

Jag genomförde totalt 31 manuella tester. Vid första genomgången godkändes 29 tester, och efter att jag åtgärdat problemet i flyttfunktionen blev alla 31 tester godkända.

## 4. Metoder och modeller som använts

Under utvecklingen har jag framför allt använt arrayer, loopar, villkorssatser och metoder.

Arrayen används för att representera parkeringsplatserna, medan loopar används för att söka igenom platserna och hantera användarinmatning.

Jag har även använt olika strängmetoder, exempelvis Split(), Contains() och ToUpper(), för att hantera registreringsnummer och information om fordon.

För inputvalideringen har jag använt kontroller med exempelvis string.IsNullOrWhiteSpace() och char.IsLetterOrDigit() för att säkerställa att användaren skriver in ett giltigt registreringsnummer.

Jag har försökt dela upp funktionaliteten i separata metoder istället för att lägga all kod i programmets huvuddel. Det har gjort det lättare att förstå och felsöka de olika funktionerna.

Jag har även arbetat med manuell testning där jag skapat olika testfall och jämfört det förväntade resultatet med programmets faktiska beteende.

## 5. Hur jag skulle lösa uppgiften nästa gång

Om jag skulle göra om projektet idag hade jag lagt mer tid på att planera programmets struktur innan jag började skriva kod.

Även om lösningen med en array av strängar fungerar enligt uppgiftens krav är det inte det mest praktiska sättet att lagra fordonsinformation i ett större system.

Jag hade hellre skapat separata klasser för exempelvis Vehicle och ParkingSpot, där varje objekt innehåller den information som behövs. Det skulle göra det lättare att lägga till nya funktioner och minska behovet av att dela upp strängar för att komma åt olika värden.

Jag hade även skapat gemensamma metoder för validering och sökning. I den nuvarande lösningen finns det viss kod som upprepas i flera funktioner, vilket gör det svårare att underhålla programmet.

En annan sak jag skulle göra annorlunda är att börja testa gränsfall tidigare. Jag upptäckte problemet med en full parkering ganska sent i utvecklingen. Om jag hade testat sådana situationer redan när flyttfunktionen skapades hade jag kunnat åtgärda problemet tidigare.

## 6. Slutsats om uppgiften

Jag tycker att projektet har varit roligt och lärorikt då jag inte gjort större applikationer i C# förut.

Tidigare har många uppgifter varit ganska små och fokuserat på en specifik funktion eller ett visst moment i C#. I Prague Parking behövde jag istället tänka på hur flera funktioner skulle fungera tillsammans.

Jag har framför allt fått bättre förståelse för hur man arbetar med arrayer, stränghantering och metoder, men också hur viktigt det är att tänka på felhantering och användarvänlighet.

Det jag tar med mig mest från uppgiften är att ett program inte är färdigt bara för att det fungerar under normala förhållanden. Det måste också kunna hantera situationer där något går fel.

## 7. Slutsats om kursen hittills

Hittills tycker jag att kursen har gett mig en bättre förståelse för C# och hur man bygger program på ett mer strukturerat sätt.

Jag har sedan tidigare erfarenhet av programmering, men framför allt inom andra språk och tekniker. Därför har det varit intressant att arbeta med C# och se både likheter och skillnader jämfört med det jag använt tidigare.

Jag tycker att praktiska uppgifter som Prague Parking är ett bra sätt att lära sig eftersom man får använda teorin för att lösa ett faktiskt problem.

Något jag vill fortsätta utveckla är min förståelse för objektorienterad programmering, särskilt hur man använder klasser, objekt och olika principer för att skapa kod som är enklare att återanvända och underhålla.

Sammanfattningsvis känner jag att projektet har hjälpt mig att utvecklas inom C# och gett mig en bättre förståelse för hur man planerar, bygger, testar och förbättrar en applikation.