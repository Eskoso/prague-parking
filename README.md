# Prague Parking

Två fristående svenska konsolapplikationer av Walter Esko, framtagna inom kursen Objektorienterad programmering C# på KYH Systemutveckling DevSecOps.

## Projekten

Båda versionerna har ett garage med 100 parkeringsplatser och ett menygränssnitt med Spectre.Console. En bil tar en plats och två motorcyklar kan dela en plats. Användaren kan parkera, flytta och hämta ut fordon, söka på hela eller delar av registreringsnumret och visa parkeringsstatus. Parkeringsinformationen finns i minnet under körningen och sparas inte mellan starter.

- **PragueParkingV1.0** startar med förifyllda exempel: två motorcyklar på plats 10 och bilar på platserna 16, 25 och 33. Registreringsnummer valideras vid parkering. Sökningen visar träffar eller ett meddelande och återgår sedan till huvudmenyn efter ett tangenttryck.
- **PragueParkingV1.1** startar med ett tomt garage. Vid inmatning av registreringsnummer för parkering och sökning kan användaren försöka igen efter ogiltig inmatning och skriva `0` för att återgå. Sökningen kontrollerar även längd och teckentyp och låter användaren söka igen om inga fordon hittas. Vid flytt kontrolleras att det finns en tillgänglig destinationsplats.

## Krav

.NET 10 SDK och en interaktiv terminal. Båda projekten använder NuGet-paketet Spectre.Console **0.57.2**. För Visual Studio behövs en installation som stöder .NET 10 och lösningsformatet `.slnx`.

## Bygg och kör

Kör följande från katalogen `prague-parking`:

```powershell
dotnet restore .\prague-parking.slnx
dotnet build .\prague-parking.slnx
```

Starta en version i taget:

```powershell
dotnet run --project .\PragueParkingV1.0\PragueParkingV1.0.csproj
dotnet run --project .\PragueParkingV1.1\PragueParkingV1.1.csproj
```

I Visual Studio: öppna `prague-parking.slnx`, välj önskat projekt som startprojekt och kör. Navigera i menyerna med piltangenterna och bekräfta med Enter. Välj `0. Avsluta` i huvudmenyn för att avsluta.

## Struktur

```text
prague-parking/
├── PragueParkingV1.0/
│   ├── Program.cs
│   └── PragueParkingV1.0.csproj
├── PragueParkingV1.1/
│   ├── Program.cs
│   └── PragueParkingV1.1.csproj
├── prague-parking.slnx
├── README.md
├── .gitignore
└── .gitattributes
```

Källkoden är bevarad utan ändringar vid omstruktureringen. Projekten är separata körbara program. Den befintliga Git-historiken från V1.0 är bevarad i repositoryts rot. Befintliga byggfiler under `bin` och `obj` har också bevarats och ignoreras av Git.