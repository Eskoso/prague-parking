// ======================================================
// Prague Parking V2 - inlämningsuppgift i kursen Objektorienterad programmering C# på KYH Systemutveckling DevSecOps.
// Skapad av: Walter Esko
// Datum: 2026-10-09
// Beskrivning: Parkeringssystem för hantering av bilar och MC. Gjord med hjälp av spectre .console för att skapa en snygg konsolapplikation. Användaren kan parkera, flytta, hämta ut och söka efter fordon samt visa parkeringsstatus. Systemet hanterar både bilar och motorcyklar, där motorcyklar kan stå två på samma plats, och bilar kan stå en per plats. Systemet är byggt med C# 14.0 och .NET 10.0.
// ======================================================

using Spectre.Console;
using PragueParkingV2;
using PragueParking.DataAccess;


var parkingFile = new ParkingFile();

string dataPath = Path.Combine(
    AppContext.BaseDirectory,
    "parking.json");

ParkingGarage parkingGarage;
GarageConfig config;

try
{
    string configPath = Path.Combine(
        AppContext.BaseDirectory,
        "config.json");

    config = new ConfigFile().Load(configPath);

    List<VehicleData> data = parkingFile.Load(dataPath);

    parkingGarage = CreateGarage(data, config);
}
catch (Exception ex) when (
    ex is IOException ||
    ex is UnauthorizedAccessException ||
    ex is System.Text.Json.JsonException)
{
    AnsiConsole.MarkupLine(
    "[red]Konfiguration eller parkeringsdata kunde inte läsas in. Programmet avslutas.[/]");

    AnsiConsole.MarkupLine(Markup.Escape(ex.Message));

    Console.WriteLine("Tryck på valfri tangent...");
    Console.ReadKey(true);
    return;
}


// string?[] parkingGarage = new string?[100]; // gamla parkinggarage arrayen.


// Parkera fordon
/* static void ParkCar(string?[] parkingGarage)
{
    AnsiConsole.Clear();
    // titel
    AnsiConsole.MarkupLine("[bold yellow]Parkera fordon[/]");
    AnsiConsole.WriteLine();

    // lät användaren välja typ av fordon (MC eller CAR eller Tillbaka för att återgå till huvudmenyn)
    string vehicleType = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title("Välj typ av fordon:")
            .AddChoices("MC", "CAR", "Tillbaka")
    );

    // prompt användaren att ange registreringsnummer och konvertera till uppercase, satt den som textprompt och allow empty för att kunna göra nästa validering (string.IsNullOrWhiteSpace(licensePlate))
    string licensePlate;

    AnsiConsole.MarkupLine("[grey]Skriv '0' för att återgå till huvudmenyn.[/]");
    AnsiConsole.WriteLine();

    while (true)
    {
        licensePlate = AnsiConsole.Prompt(
            new TextPrompt<string>("Ange registreringsnummer (ex. ABC123):")
                .AllowEmpty()
        ).ToUpper();

        // Återgå till huvudmenyn
        if (licensePlate == "0")
            return;

        // Kontrollera tom inmatning
        if (string.IsNullOrWhiteSpace(licensePlate))
        {
            AnsiConsole.MarkupLine("[red]Registreringsnummer får inte vara tomt.[/]");
            continue;
        }

        // Kontrollera längden
        if (licensePlate.Length > 10)
        {
            AnsiConsole.MarkupLine("[red]Registreringsnummer får max vara 10 tecken.[/]");
            continue;
        }

        // Kontrollera att endast bokstäver och siffror används
        if (!licensePlate.All(char.IsLetterOrDigit))
        {
            AnsiConsole.MarkupLine("[red]Registreringsnummer får endast innehålla bokstäver och siffror.[/]");
            continue;
        }

        break;
    }

    // kolla om fordonet redan är parkerat i garaget
    int parkedSpot = Array.FindIndex(parkingGarage, spot =>
    spot != null &&
    spot.Split('|').Any(vehicle => vehicle.Split('#')[1] == licensePlate)
);

    // om fordonet redan är parkerat, visa meddelande och återgå till huvudmenyn
    if (parkedSpot != -1)
    {
        AnsiConsole.MarkupLine(
            $"[red]Ett fordon med registreringsnummer {licensePlate} är redan parkerat på plats {parkedSpot + 1}.[/]"
        );

        AnsiConsole.MarkupLine("[gray]Tryck på valfri tangent för att återgå till huvudmenyn...[/]");

        Console.ReadKey(true);
        return;
    }

    // kolla om fordonet redan är parkerat i garaget
    int availableSpot = -1;

    // Om fordonet är en MC, leta först efter en plats
    // där det redan står exakt en MC
    if (vehicleType == "MC")
    {
        availableSpot = Array.FindIndex(parkingGarage, spot =>
            spot != null &&
            spot.StartsWith("MC#") &&
            !spot.Contains("|")
        );

        // Om vi hittar en plats med en MC, parkera bredvid den
        if (availableSpot != -1)
        {
            parkingGarage[availableSpot] += $"|MC#{licensePlate}";

            AnsiConsole.MarkupLine(
                $"[green]MC med registreringsnummer {licensePlate} " +
                $"har parkerats på plats {availableSpot + 1}.[/]"
            );

            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine(
                "[grey]Tryck på valfri tangent för att återgå till huvudmenyn...[/]"
            );

            Console.ReadKey(true);

            return;
        }
    }

    // leta efter första helt tomma platsen
    availableSpot = Array.FindIndex(parkingGarage, spot => spot == null);

    // om vi hittar en tom plats, parkera fordonet där
    if (availableSpot != -1)
    {
        // parkera fordonet på den tomma platsen
        parkingGarage[availableSpot] = $"{vehicleType}#{licensePlate}";

        // visa meddelandet om att fordonet har parkerats
        AnsiConsole.MarkupLine(
            $"[green]Fordonet {vehicleType} med registreringsnummer {licensePlate} " +
            $"har parkerats på plats {availableSpot + 1}.[/]"
        );

        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine(
            "[grey]Tryck på valfri tangent för att återgå till huvudmenyn...[/]"
        );

        Console.ReadKey(true);

        return;
    }

    // om vi kommer hit finns ingen plats kvar
    AnsiConsole.MarkupLine(
        "[red]Tyvärr, det finns inga lediga parkeringsplatser.[/]"
    );

    AnsiConsole.WriteLine();
    AnsiConsole.MarkupLine(
        "[grey]Tryck på valfri tangent för att återgå till huvudmenyn...[/]"
    );

    Console.ReadKey(true);
}*/

static void ParkVehicle(
    ParkingGarage garage,
    ParkingFile parkingFile,
    string dataPath,
    GarageConfig config)
{
    AnsiConsole.Clear();
    AnsiConsole.MarkupLine("[bold yellow]Parkera fordon[/]");

    string vehicleType = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title("Välj typ av fordon:")
            .AddChoices("MC", "CAR", "Tillbaka"));

    if (vehicleType == "Tillbaka")
    {
        return;
    }

    AnsiConsole.MarkupLine(
        "[grey]Skriv '0' för att återgå till huvudmenyn.[/]");

    string licensePlate;

    while (true)
    {
        licensePlate = AnsiConsole.Prompt(
            new TextPrompt<string>("Ange registreringsnummer:")
                .AllowEmpty())
            .ToUpperInvariant();

        if (licensePlate == "0")
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(licensePlate) ||
            licensePlate.Length > 10 ||
            !licensePlate.All(char.IsLetterOrDigit))
        {
            AnsiConsole.MarkupLine(
                "[red]Ange 1–10 tecken med endast bokstäver och siffror.[/]");
            continue;
        }

        if (garage.FindVehicle(licensePlate) != null)
        {
            AnsiConsole.MarkupLine(
                "[red]Registreringsnumret är redan parkerat.[/]");
            continue;
        }

        break;
    }

    Vehicle vehicle = CreateVehicle(
    vehicleType,
    licensePlate,
    DateTimeOffset.Now,
    config);

    if (TrySaveChange(
    garage,
    parkingFile,
    dataPath,
    () => garage.TryPark(vehicle)))
    {
        ParkingSpot spot = garage.Spots.First(
            spot => spot.Vehicles.Contains(vehicle));

        AnsiConsole.MarkupLine(
            $"[green]{licensePlate} har parkerats på plats {spot.Number}.[/]");
    }
    else
    {
        AnsiConsole.MarkupLine(
            "[red]Parkeringen kunde inte genomföras.[/]");
    }

    AnsiConsole.MarkupLine(
        "[grey]Tryck på valfri tangent för att återgå...[/]");
    Console.ReadKey(true);
}

// Flytta fordon
/* static void MoveCar(string?[] parkingGarage)
{
    AnsiConsole.Clear();

    // titel
    AnsiConsole.MarkupLine("[bold yellow]Flytta fordon[/]");
    AnsiConsole.WriteLine();

    // om det inte finns några parkerade fordon, visa felmeddelande och återgå till huvudmenyn
    if (parkingGarage.All(spot => spot == null))
    {
        AnsiConsole.MarkupLine("[red]Det finns inga parkerade fordon att flytta.[/]");
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine(
            "[grey]Tryck på valfri tangent för att återgå till huvudmenyn...[/]"
        );
        Console.ReadKey(true);
        return;
    }

    // lista alla parkerade fordon
    List<string> vehicles = new List<string>();

    // hämta alla parkerade fordon
    for (int i = 0; i < parkingGarage.Length; i++)
    {
        // om platsen inte är tom, hämta fordonen på platsen
        if (parkingGarage[i] != null)
        {
            // dela upp fordonen på platsen i en array
            string[] vehiclesOnSpot = parkingGarage[i]!.Split('|');

            // loop igenom fordonen och formatera dem snyggt
            foreach (string veh in vehiclesOnSpot)
            {
                // dela upp fordonet i typ och registreringsnummer
                string[] vehicleParts = veh.Split('#');

                string vehicleType = vehicleParts[0];
                string licensePlate = vehicleParts[1];

                // lägg till fordonet i listan med snyggt format "TYPE - REGNR"
                vehicles.Add($"Plats {i + 1} - {vehicleType} - {licensePlate}");
            }
        }
    }

    // lägg till "tillbaka" som ett alternativ
    vehicles.Add("Tillbaka");

    // Låt användaren välja fordon
    string choice = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title("Välj fordon att flytta:")
            .PageSize(15)
            .AddChoices(vehicles)
    );

    // återgå till huvudmeny
    if (choice == "Tillbaka")
    {
        return;
    }

    // hämta index för den valda platsen och fordonet
    string[] parts = choice.Split(" - ");

    // hämta platsnummer från valet
    int currentSpot = int.Parse(
        parts[0].Replace("Plats ", "")
    );

    // index i arrayen är plats - 1
    int currentIndex = currentSpot - 1;

    // sätt tillbaka formatet TYPE#REGNR
    string vehicle = $"{parts[1]}#{parts[2]}";


    // Skapa lista med platser som fordonet kan flyttas till
    List<string> availableSpots = new List<string>();

    // loop igenom alla parkeringsplatser och kolla vilka som är lediga eller har plats för fordonet
    for (int i = 0; i < parkingGarage.Length; i++)
    {
        string? spot = parkingGarage[i];

        // hoppa över platsen fordonet redan står på
        if (i == currentIndex)
        {
            continue;
        }

        // alla fordon kan flyttas till en tom plats
        if (spot == null)
        {
            availableSpots.Add($"Plats {i + 1} - LEDIG");
        }

        // MC kan också flyttas till en plats där det står exakt en MC
        else if (
            vehicle.StartsWith("MC#") &&
            spot.StartsWith("MC#") &&
            !spot.Contains("|")
        )
        {
            availableSpots.Add(
                $"Plats {i + 1} - {spot} (1 MC-plats kvar)"
            );
        }
    }

    // Kontrollera att det finns platser att flytta fordonet till
    if (availableSpots.Count == 0)
    {
        AnsiConsole.MarkupLine(
            "[red]Det finns inga tillgängliga parkeringsplatser att flytta fordonet till.[/]"
        );

        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine(
            "[grey]Tryck på valfri tangent för att återgå till huvudmenyn...[/]"
        );

        Console.ReadKey(true);
        return;
    }

    // välj ny plats
    string newSpotChoice = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title($"Välj plats att flytta [yellow]{vehicle}[/] till:")
            .PageSize(15)
            .AddChoices(availableSpots)
    );


    // hämta index för den nya platsen
    string[] newParts = newSpotChoice.Split(" - ");

    // hämta platsnummer från valet
    int newSpot = int.Parse(
        newParts[0].Replace("Plats ", "")
    );

    // index i arrayen är plats - 1
    int newIndex = newSpot - 1;


    // ta bort fordonet från gamla platsen
    string[] vehiclesOnCurrentSpot = parkingGarage[currentIndex]!.Split('|');

    if (vehiclesOnCurrentSpot.Length == 1)
    {
        // fordonet står ensamt, töm hela platsen
        parkingGarage[currentIndex] = null;
    }
    else
    {
        // två MC står på platsen behåll den MC som inte flyttas
        parkingGarage[currentIndex] = vehiclesOnCurrentSpot
            .First(veh => veh != vehicle);
    }


    // lägg fordonet på nya platsen
    if (parkingGarage[newIndex] == null)
    {
        // nya platsen är tom
        parkingGarage[newIndex] = vehicle;
    }
    else
    {
        // det står redan en MC där
        parkingGarage[newIndex] += $"|{vehicle}";
    }


    // visa meddelande
    AnsiConsole.WriteLine();
    AnsiConsole.MarkupLine(
        $"[green]{vehicle} har flyttats från plats {currentSpot} till plats {newSpot}.[/]"
    );

    AnsiConsole.WriteLine();
    AnsiConsole.MarkupLine(
        "[grey]Tryck på valfri tangent för att återgå till huvudmenyn...[/]"
    );

    Console.ReadKey(true);
}*/

static void MoveVehicle(ParkingGarage garage, ParkingFile parkingFile, string dataPath)
{
    AnsiConsole.Clear();
    AnsiConsole.MarkupLine("[bold yellow]Flytta fordon[/]");

    Vehicle? vehicle = SelectVehicle(garage);

    if (vehicle == null)
    {
        return;
    }

    List<int> destinations = garage.Spots
        .Where(spot =>
            !spot.Vehicles.Contains(vehicle) &&
            spot.CanFit(vehicle))
        .Select(spot => spot.Number)
        .ToList();

    if (destinations.Count == 0)
    {
        AnsiConsole.MarkupLine(
            "[red]Det finns inga tillgängliga platser att flytta till.[/]");
        Console.ReadKey(true);
        return;
    }

    destinations.Add(0);

    int destination = AnsiConsole.Prompt(
        new SelectionPrompt<int>()
            .Title("Välj ny parkeringsplats:")
            .PageSize(15)
            .UseConverter(number =>
                number == 0 ? "Tillbaka" : $"Plats {number}")
            .AddChoices(destinations));

    if (destination == 0)
    {
        return;
    }

    if (TrySaveChange(
     garage,
     parkingFile,
     dataPath,
     () => garage.MoveVehicle(vehicle.LicensePlate, destination)))
    {
        AnsiConsole.MarkupLine(
            $"[green]{vehicle.LicensePlate} har flyttats till plats {destination}.[/]");
    }
    else
    {
        AnsiConsole.MarkupLine("[red]Fordonet kunde inte flyttas.[/]");
    }

    AnsiConsole.MarkupLine(
        "[grey]Tryck på valfri tangent för att återgå...[/]");
    Console.ReadKey(true);
}

// hämta ut fordon
 /* static void RemoveCar(string?[] parkingGarage)
{
    AnsiConsole.Clear();

    // titel
    AnsiConsole.MarkupLine("[bold yellow]Hämta ut fordon[/]");
    AnsiConsole.WriteLine();

    List<string> vehicles = new List<string>();

    // Hämta alla parkerade fordon
    for (int i = 0; i < parkingGarage.Length; i++)
    {
        if (parkingGarage[i] != null)
        {
            // Dela upp fordonen på platsen i en array
            string[] vehiclesOnSpot = parkingGarage[i]!.Split('|');

            // Loop igenom fordonen och formatera dem snyggt
            foreach (string veh in vehiclesOnSpot)
            {
                string[] vehicleParts = veh.Split('#');

                string vehicleType = vehicleParts[0];
                string licensePlate = vehicleParts[1];

                // Lägg till fordonet i listan med snyggt format
                vehicles.Add($"Plats {i + 1} - {vehicleType} - {licensePlate}");
            }
        }
    }

    vehicles.Add("Tillbaka");

    // låt användaren välja fordon
    string choice = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title("Välj fordon att hämta:")
            .PageSize(15)
            .AddChoices(vehicles)
    );

    // återgå till huvudmenyn
    if (choice == "Tillbaka")
    {
        return;
    }

    // hämta plats och fordon
    string[] parts = choice.Split(" - ");

    int currentSpot = int.Parse(
        parts[0].Replace("Plats ", "")
    );

    int currentIndex = currentSpot - 1;

    // sätt tillbaka formatet TYPE#REGNR
    string vehicle = $"{parts[1]}#{parts[2]}";

    // hämta alla fordon på platsen
    string[] vehiclesOnCurrentSpot = parkingGarage[currentIndex]!.Split('|');

    if (vehiclesOnCurrentSpot.Length == 1)
    {
        // fordonet står ensamt
        parkingGarage[currentIndex] = null;
    }
    else
    {
        // två MC står på platsen
        // behåll den MC som inte hämtas
        parkingGarage[currentIndex] = vehiclesOnCurrentSpot
            .First(veh => veh != vehicle);
    }

    AnsiConsole.WriteLine();

    // visa snyggt istället för MC#ABC123
    AnsiConsole.MarkupLine(
        $"[green]{parts[1]} - {parts[2]} har hämtats från plats {currentSpot}.[/]"
    );

    AnsiConsole.WriteLine();
    AnsiConsole.MarkupLine(
        "[grey]Tryck på valfri tangent för att återgå till huvudmenyn...[/]"
    );

    Console.ReadKey(true);
}*/

static void RemoveVehicle(ParkingGarage garage, ParkingFile parkingFile, string dataPath)
{
    AnsiConsole.Clear();
    AnsiConsole.MarkupLine("[bold yellow]Hämta ut fordon[/]");

    Vehicle? vehicle = SelectVehicle(garage);

    if (vehicle == null)
    {
        return;
    }

    ParkingSpot spot = garage.Spots.First(
        spot => spot.Vehicles.Contains(vehicle));

    if (TrySaveChange(
    garage,
    parkingFile,
    dataPath,
    () => garage.RemoveVehicle(vehicle.LicensePlate)))
    {
        AnsiConsole.MarkupLine(
            $"[green]{vehicle.LicensePlate} har hämtats från plats {spot.Number}.[/]");
    }
    else
    {
        AnsiConsole.MarkupLine(
            "[red]Fordonet kunde inte hämtas ut.[/]");
    }

    AnsiConsole.MarkupLine(
        "[grey]Tryck på valfri tangent för att återgå...[/]");
    Console.ReadKey(true);
}

// sök efter fordon
/* static void SearchCars(string?[] parkingGarage)
{
    AnsiConsole.Clear();

    AnsiConsole.MarkupLine("[bold yellow]Sök efter fordon[/]");
    AnsiConsole.MarkupLine("[grey]Skriv '0' för att återgå till huvudmenyn.[/]");
    AnsiConsole.WriteLine();

    while (true)
    {
        // Be användaren ange registreringsnummer
        string searchTerm = AnsiConsole.Prompt(
            new TextPrompt<string>("Ange registreringsnummer att söka efter:")
                .AllowEmpty()
        ).ToUpper();

        // Återgå till huvudmenyn
        if (searchTerm == "0")
            return;

        // Kontrollera att användaren skrivit något
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            AnsiConsole.MarkupLine("[red]Registreringsnummer får inte vara tomt.[/]");
            continue;
        }

        // Kontrollera längden
        if (searchTerm.Length > 10)
        {
            AnsiConsole.MarkupLine("[red]Registreringsnummer får max vara 10 tecken.[/]");
            continue;
        }

        // Kontrollera att bara bokstäver och siffror används
        if (!searchTerm.All(char.IsLetterOrDigit))
        {
            AnsiConsole.MarkupLine("[red]Registreringsnummer får endast innehålla bokstäver och siffror.[/]");
            continue;
        }

        // Lista över hittade fordon
        List<string> foundVehicles = new List<string>();

        // Sök igenom alla parkeringsplatser
        for (int i = 0; i < parkingGarage.Length; i++)
        {
            if (parkingGarage[i] == null)
                continue;

            string[] vehiclesOnSpot = parkingGarage[i]!.Split('|');

            foreach (string vehicle in vehiclesOnSpot)
            {
                string[] vehicleParts = vehicle.Split('#');

                string vehicleType = vehicleParts[0];
                string licensePlate = vehicleParts[1];

                if (licensePlate.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                {
                    foundVehicles.Add(
                        $"Plats {i + 1} - {vehicleType} - {licensePlate}"
                    );
                }
            }
        }

        // Om inget fordon hittades, visa fel och sök igen
        if (foundVehicles.Count == 0)
        {
            AnsiConsole.MarkupLine(
                "[red]Inga fordon hittades med det angivna registreringsnumret.[/]"
            );
            continue;
        }

        // Visa hittade fordon
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[bold green]Fordon hittades:[/]");

        foreach (string vehicle in foundVehicles)
        {
            AnsiConsole.MarkupLine($"[cyan]{Markup.Escape(vehicle)}[/]");
        }

        AnsiConsole.WriteLine();

        // Återgå till huvudmenyn efter lyckad sökning
        AnsiConsole.MarkupLine(
            "[grey]Tryck på valfri tangent för att återgå till huvudmenyn...[/]"
        );

        Console.ReadKey(true);
        return;
    }
}*/

static void SearchVehicles(ParkingGarage garage)
{
    AnsiConsole.Clear();
    AnsiConsole.MarkupLine("[bold yellow]Sök efter fordon[/]");
    AnsiConsole.MarkupLine(
        "[grey]Skriv '0' för att återgå till huvudmenyn.[/]");

    while (true)
    {
        string searchTerm = AnsiConsole.Prompt(
            new TextPrompt<string>("Ange hela eller delar av registreringsnumret:")
                .AllowEmpty());

        if (searchTerm == "0")
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(searchTerm) ||
            searchTerm.Length > 10 ||
            !searchTerm.All(char.IsLetterOrDigit))
        {
            AnsiConsole.MarkupLine(
                "[red]Ange 1–10 tecken med endast bokstäver och siffror.[/]");
            continue;
        }

        List<Vehicle> matches = garage.SearchVehicles(searchTerm);

        if (matches.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]Inga fordon hittades.[/]");
            continue;
        }

        foreach (Vehicle vehicle in matches)
        {
            ParkingSpot spot = garage.Spots.First(
                spot => spot.Vehicles.Contains(vehicle));

            string vehicleType = vehicle is Car ? "Bil" : "MC";

            AnsiConsole.MarkupLine(
                $"[cyan]Plats {spot.Number} - {vehicleType} - {vehicle.LicensePlate}[/]");
        }

        AnsiConsole.MarkupLine(
            "[grey]Tryck på valfri tangent för att återgå...[/]");
        Console.ReadKey(true);
        return;
    }
}

// Visa parkeringar
/* static void ShowParkingStatus(string?[] parkingGarage)
{
    AnsiConsole.Clear();

    // titel
    AnsiConsole.MarkupLine("[bold yellow]Parkeringsstatus[/]");
    AnsiConsole.WriteLine();

    // Loopa igenom alla parkeringsplatser och visa status
    for (int i = 0; i < parkingGarage.Length; i++)
    {

        // Om platsen är tom, visa "LEDIG" i grönt
        if (parkingGarage[i] == null)
        {
            AnsiConsole.MarkupLine(
                $"Plats {i + 1}: [green]LEDIG[/]"
            );
        }
        else
        {
            // Om platsen är upptagen, visa fordonen i rött
            // Om det är en MC-plats med två MC, visa båda
            string[] vehicles = parkingGarage[i]!.Split('|');

            // Skapa en lista med fordonen i formatet "TYPE - REGNR"
            List<string> displayVehicles = new List<string>();

            // Loop igenom fordonen och formatera dem
            foreach (string vehicle in vehicles)
            {
                // Dela upp fordonet i typ och registreringsnummer vehicletype och licenseplate för att senare formatera snyggare "plats 1: type - regnr"
                string[] vehicleParts = vehicle.Split('#');

                string vehicleType = vehicleParts[0];
                string licensePlate = vehicleParts[1];

                // lägg till fordonet i listan med snyggt format
                displayVehicles.Add($"{vehicleType} - {licensePlate}");
            }

            // Visa fordonen i rött
            AnsiConsole.MarkupLine(
                $"Plats {i + 1}: [red]{string.Join(" | ", displayVehicles)}[/]"
            );
        }
    }

    // standard meddelande för att återgå till huvudmenyn
    AnsiConsole.WriteLine();
    AnsiConsole.MarkupLine(
        "[grey]Tryck på valfri tangent för att återgå till huvudmenyn...[/]"
    );

    Console.ReadKey(true);
}*/ 

static void DisplayParking(ParkingGarage garage)
{
    AnsiConsole.Clear();
    AnsiConsole.MarkupLine("[bold yellow]Parkeringsstatus[/]");

    foreach (ParkingSpot spot in garage.Spots)
    {
        if (spot.Vehicles.Count == 0)
        {
            AnsiConsole.MarkupLine(
                $"Plats {spot.Number}: [green]LEDIG[/]");
            continue;
        }

        List<string> descriptions = spot.Vehicles
            .Select(vehicle =>
            {
                string type = vehicle is Car ? "Bil" : "MC";
                return $"{type} - {vehicle.LicensePlate}";
            })
            .ToList();

        string color = spot.UsedCapacity < spot.Capacity
            ? "yellow"
            : "red";

        AnsiConsole.MarkupLine(
            $"Plats {spot.Number}: [{color}]" +
            $"{string.Join(" | ", descriptions)}[/] " +
            $"({spot.UsedCapacity}/{spot.Capacity})");
    }

    AnsiConsole.MarkupLine(
        "[grey]Tryck på valfri tangent för att återgå...[/]");
    Console.ReadKey(true);
}

static Vehicle? SelectVehicle(ParkingGarage garage)
{
    List<string> choices = garage.Spots
        .SelectMany(spot => spot.Vehicles)
        .Select(vehicle => vehicle.LicensePlate)
        .ToList();

    if (choices.Count == 0)
    {
        AnsiConsole.MarkupLine("[red]Det finns inga parkerade fordon.[/]");
        Console.ReadKey(true);
        return null;
    }

    choices.Add("0. Tillbaka");

    string choice = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title("Välj fordon:")
            .PageSize(15)
            .AddChoices(choices));

    if (choice == "0. Tillbaka")
    {
        return null;
    }

    return garage.FindVehicle(choice);
}

static List<VehicleData> CreateParkingData(ParkingGarage garage)
{
    var data = new List<VehicleData>();

    foreach (ParkingSpot spot in garage.Spots)
    {
        foreach (Vehicle vehicle in spot.Vehicles)
        {
            string vehicleType = vehicle switch
            {
                Car => "CAR",
                MC => "MC",
                _ => throw new InvalidOperationException(
                    "Fordonstypen kan inte sparas.")
            };

            data.Add(new VehicleData
            {
                VehicleType = vehicleType,
                LicensePlate = vehicle.LicensePlate,
                ArrivalTime = vehicle.ArrivalTime,
                SpotNumber = spot.Number
            });
        }
    }

    return data;
}

static ParkingGarage CreateGarage(
    List<VehicleData> data,
    GarageConfig config)
{
    ArgumentNullException.ThrowIfNull(data);

    var garage = new ParkingGarage(
    config.NumberOfSpots,
    config.SpotCapacity);
    var plates = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
    var now = DateTimeOffset.Now;

    foreach (VehicleData item in data)
    {
        if (item == null ||
            string.IsNullOrWhiteSpace(item.LicensePlate) ||
            item.LicensePlate.Length > 10 ||
            !item.LicensePlate.All(char.IsLetterOrDigit))
        {
            throw new InvalidDataException(
                "Datafilen innehåller ett ogiltigt registreringsnummer.");
        }

        if (!plates.Add(item.LicensePlate))
        {
            throw new InvalidDataException(
                "Datafilen innehåller dubbla registreringsnummer.");
        }

        if (item.ArrivalTime == default || item.ArrivalTime > now)
        {
            throw new InvalidDataException(
                "Datafilen innehåller en ogiltig ankomsttid.");
        }

        Vehicle vehicle = CreateVehicle(
            item.VehicleType,
            item.LicensePlate,
            item.ArrivalTime,
            config);

        ParkingSpot? spot = garage.Spots.FirstOrDefault(
            spot => spot.Number == item.SpotNumber);

        if (spot == null || !spot.CanFit(vehicle))
        {
            throw new InvalidDataException(
                "Datafilen anger en ogiltig eller överfull parkeringsplats.");
        }

        spot.Vehicles.Add(vehicle);
    }

    return garage;
}

static bool TrySaveChange(
    ParkingGarage garage,
    ParkingFile parkingFile,
    string dataPath,
    Func<bool> change)
{
    var previousVehicles = garage.Spots
        .Select(spot => spot.Vehicles.ToList())
        .ToList();

    if (!change())
    {
        return false;
    }

    try
    {
        parkingFile.Save(dataPath, CreateParkingData(garage));
        return true;
    }
    catch (Exception ex) when (
        ex is IOException ||
        ex is UnauthorizedAccessException)
    {
        for (int i = 0; i < garage.Spots.Count; i++)
        {
            garage.Spots[i].Vehicles.Clear();
            garage.Spots[i].Vehicles.AddRange(previousVehicles[i]);
        }

        AnsiConsole.MarkupLine(
            "[red]Ändringen kunde inte sparas och har återställts.[/]");

        AnsiConsole.MarkupLine(Markup.Escape(ex.Message));
        return false;
    }
}

static Vehicle CreateVehicle(
    string vehicleType,
    string licensePlate,
    DateTimeOffset arrivalTime,
    GarageConfig config)
{
    VehicleConfig? settings = config.VehicleTypes.FirstOrDefault(
        item => item.Type == vehicleType);

    if (settings == null)
    {
        throw new InvalidDataException(
            "Fordonstypen saknas i konfigurationen.");
    }

    return vehicleType switch
    {
        "CAR" => new Car(licensePlate, arrivalTime, settings.Size),
        "MC" => new MC(licensePlate, arrivalTime, settings.Size),
        _ => throw new InvalidDataException("Okänd fordonstyp.")
    };
}

// Huvudmeny
while (true)
{
    AnsiConsole.Clear();

    // titel
    AnsiConsole.MarkupLine("[bold yellow]Välkommen till Prague Parking![/]");
    AnsiConsole.WriteLine();

    string choice = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title("Vad vill du göra?")
            .PageSize(15)
            .AddChoices(
                "1. Parkera fordon",
                "2. Flytta fordon",
                "3. Hämta ut fordon",
                "4. Sök efter fordon",
                "5. Visa parkering",
                "0. Avsluta"
            )
    );

    // switch case för att hantera användarens val
    switch (choice)
    {
        case "1. Parkera fordon":
            ParkVehicle(parkingGarage, parkingFile, dataPath, config);
            break;

        case "2. Flytta fordon":
            MoveVehicle(parkingGarage, parkingFile, dataPath);
            break;

        case "3. Hämta ut fordon":
            RemoveVehicle(parkingGarage, parkingFile, dataPath);
            break;

        case "4. Sök efter fordon":
            SearchVehicles(parkingGarage);
            break;

        case "5. Visa parkering":
            DisplayParking(parkingGarage);
            break;

        case "0. Avsluta":
            return;
    }

}