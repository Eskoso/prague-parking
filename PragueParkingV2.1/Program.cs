// ======================================================
// Prague Parking V2 - inlämningsuppgift i kursen Objektorienterad programmering C# på KYH Systemutveckling DevSecOps.
// Skapad av: Walter Esko
// Datum: 2026-10-09
// Beskrivning: Parkeringssystem för hantering av bilar och MC. Gjord med hjälp av spectre .console för att skapa en snygg konsolapplikation. Användaren kan parkera, flytta, hämta ut och söka efter fordon samt visa parkeringsstatus. Systemet hanterar både bilar och motorcyklar, där motorcyklar kan stå två på samma plats, och bilar kan stå en per plats. Systemet är byggt med C# 14.0 och .NET 10.0.
// ======================================================

using Spectre.Console;
using PragueParkingV2_1;
using PragueParking.DataAccess;

// Skapa instanser av klasserna för att hantera parkeringsdata och konfiguration
var ParkingFileV21 = new ParkingFileV21();

// Hämta sökvägen till datafilen
string dataPath = Path.Combine(
    AppContext.BaseDirectory,
    "parking.json");

// Deklarera variabler för parkeringsgaraget och konfigurationen
ParkingGarage parkingGarage;
GarageConfig config;

// Läs in konfigurationen och parkeringsdata från filerna
try
{
    // Hämta sökvägen till konfigurationsfilen
    string configPath = Path.Combine(
        AppContext.BaseDirectory,
        "config.json");

    // Läs in konfigurationen
    config = new ConfigFileV21().Load(configPath);

    // Läs in parkeringsdata
    try
    {
        List<VehicleDataV21> data = ParkingFileV21.Load(dataPath);
        parkingGarage = CreateGarage(data, config);
    }
    catch (FileNotFoundException)
    {
        parkingGarage = new ParkingGarage(
            config.NumberOfSpots,
            config.SpotCapacity,
            config.SpotCapacities);

        int number = 1;
        DateTimeOffset arrivalTime = DateTimeOffset.Now.AddHours(-2);

        foreach (VehicleConfig settings in config.VehicleTypes)
        {
            IVehicle vehicle = CreateVehicle(
                settings.Type,
                $"TEST{number}",
                arrivalTime,
                config);

            if (parkingGarage.TryPark(vehicle))
            {
                number++;
            }
        }

        if (number == 1)
        {
            throw new InvalidDataException(
                "Inget testfordon ryms med den aktuella konfigurationen.");
        }

        ParkingFileV21.Save(dataPath, CreateParkingData(parkingGarage));
    }
}
catch (Exception ex)
{
    // Om det uppstår ett fel vid inläsning av konfiguration eller parkeringsdata, visa ett felmeddelande och avsluta programmet
    Console.WriteLine("Konfiguration eller parkeringsdata kunde inte läsas in.");
    Console.WriteLine(ex.Message);
    Console.WriteLine("Programmet avslutas.");
    Console.WriteLine("Tryck på valfri tangent...");
    Console.ReadKey(true);
    return;
}


// Parkera fordon
static void ParkVehicle(
    ParkingGarage garage,
    ParkingFileV21 ParkingFileV21,
    string dataPath,
    GarageConfig config)
{
    AnsiConsole.Clear();
    AnsiConsole.MarkupLine("[bold yellow]Parkera fordon[/]");

    // Användaren får välja en fordonstyp
    string vehicleType = AnsiConsole.Prompt(
    new SelectionPrompt<string>()
        .Title("Välj typ av fordon:")
        .AddChoices(config.VehicleTypes.Select(item => item.Type))
        .AddChoices("Tillbaka"));

    // eventuellt "tillbaka" avslutar metoden
    if (vehicleType == "Tillbaka")
    {
        return;
    }

    AnsiConsole.MarkupLine(
        "[grey]Skriv '0' för att återgå till huvudmenyn.[/]");

    string licensePlate;
    string prompt = "Ange registreringsnummer:";

    if (vehicleType == "BICYCLE")
    {
        prompt = "Ange cykel-ID, exempelvis CYKEL001:";
    }

    while (true)
    {
        licensePlate = AnsiConsole.Prompt(
            new TextPrompt<string>(prompt)
                .AllowEmpty())
            .ToUpperInvariant();

        if (licensePlate == "0")
        {
            return;
        }

        // Validera registreringsnumret
        if (string.IsNullOrWhiteSpace(licensePlate) ||
            licensePlate.Length > 10 ||
            !licensePlate.All(char.IsLetterOrDigit))
        {
            AnsiConsole.MarkupLine(
                "[red]Ange 1–10 tecken med endast bokstäver och siffror.[/]");
            continue;
        }

        // Kontrollera om registreringsnumret redan är parkerat
        if (garage.FindVehicle(licensePlate) != null)
        {
            AnsiConsole.MarkupLine(
                "[red]Registreringsnumret eller cykel-ID:t används redan.[/]");
            continue;
        }

        break;
    }

    // Skapa ett nytt fordon baserat på användarens val
    IVehicle vehicle = CreateVehicle(
    vehicleType,
    licensePlate,
    DateTimeOffset.Now,
    config);

    // Försök att parkera fordonet och spara ändringen
    if (TrySaveChange(
    garage,
    ParkingFileV21,
    dataPath,
    () => garage.TryPark(vehicle)))
    {
        string spotNumbers = string.Join(", ", garage.Spots
            .Where(spot => spot.Vehicles.Contains(vehicle))
            .Select(spot => spot.Number));

        AnsiConsole.MarkupLine(
            $"[green]{licensePlate} har parkerats. Platsnummer: {spotNumbers}.[/]");
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
static void MoveVehicle(ParkingGarage garage, ParkingFileV21 ParkingFileV21, string dataPath)
{
    AnsiConsole.Clear();
    AnsiConsole.MarkupLine("[bold yellow]Flytta fordon[/]");

    IVehicle? vehicle = SelectVehicle(garage);

    if (vehicle == null)
    {
        return;
    }

    // Hitta alla parkeringsplatser som kan ta emot fordonet och som inte redan innehåller det
    List<int> destinations = new();

    if (vehicle is Bus)
    {
        foreach (IParkingSpot spot in garage.Spots)
        {
            List<IParkingSpot> busSpots = garage.FindBusSpots(
                vehicle, spot.Number);

            if (busSpots.Count == 4 &&
                !busSpots.All(busSpot => busSpot.Vehicles.Contains(vehicle)))
            {
                destinations.Add(spot.Number);
            }
        }
    }
    else
    {
        destinations = garage.Spots
            .Where(spot =>
                !spot.Vehicles.Contains(vehicle) &&
                spot.CanFit(vehicle))
            .Select(spot => spot.Number)
            .ToList();
    }

    if (destinations.Count == 0)
    {
        AnsiConsole.MarkupLine(
            "[red]Det finns inga tillgängliga platser att flytta till.[/]");
        Console.ReadKey(true);
        return;
    }

    destinations.Add(0);

    // Användaren får välja en ny parkeringsplats
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

    // Försök att flytta fordonet och spara ändringen
    if (TrySaveChange(
     garage,
     ParkingFileV21,
     dataPath,
     () => garage.MoveVehicle(vehicle.LicensePlate, destination)))
    {
        string spotNumbers = string.Join(", ", garage.Spots
            .Where(spot => spot.Vehicles.Contains(vehicle))
            .Select(spot => spot.Number));

        AnsiConsole.MarkupLine(
            $"[green]{vehicle.LicensePlate} har flyttats. Platsnummer: {spotNumbers}.[/]");
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
static void RemoveVehicle(ParkingGarage garage, ParkingFileV21 ParkingFileV21, string dataPath, GarageConfig config)
{
    AnsiConsole.Clear();
    AnsiConsole.MarkupLine("[bold yellow]Hämta ut fordon[/]");

    IVehicle? vehicle = SelectVehicle(garage);

    if (vehicle == null)
    {
        return;
    }

    // Hitta parkeringsplatsen där fordonet är parkerat
    string spotNumbers = string.Join(", ", garage.Spots
            .Where(spot => spot.Vehicles.Contains(vehicle))
            .Select(spot => spot.Number));

    // Beräkna parkeringstiden och avgiften baserat på fordonets ankomsttid och timpris
    string vehicleType = vehicle switch
    {
        Car => "CAR",
        MC => "MC",
        Bicycle => "BICYCLE",
        Bus => "BUS",
        _ => throw new InvalidDataException("Okänd fordonstyp.")
    };

    VehicleConfig settings = config.VehicleTypes.First(
        item => item.Type == vehicleType);

    TimeSpan parkedTime = DateTimeOffset.Now - vehicle.ArrivalTime;
    decimal fee = CalculateFee(parkedTime, settings.HourlyRate);


    // Försök att ta bort fordonet och spara ändringen
    if (TrySaveChange(
    garage,
    ParkingFileV21,
    dataPath,
    () => garage.RemoveVehicle(vehicle.LicensePlate)))
    // Beräkna parkeringstiden och visa ett meddelande om att fordonet har hämtats ut
    {
        AnsiConsole.MarkupLine(
            $"[green]{vehicle.LicensePlate} har hämtats ut. Platsnummer: {spotNumbers}.[/]");

        Console.WriteLine(
            $"Parkeringstid: {(int)parkedTime.TotalHours} timmar " +
            $"och {parkedTime.Minutes} minuter.");

        Console.WriteLine($"Avgift: {fee:0.00} CZK");
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
static void SearchVehicles(ParkingGarage garage)
{
    AnsiConsole.Clear();
    AnsiConsole.MarkupLine("[bold yellow]Sök efter fordon[/]");
    AnsiConsole.MarkupLine(
        "[grey]Skriv '0' för att återgå till huvudmenyn.[/]");

    // Loop för att söka efter fordon baserat på registreringsnummer
    while (true)
    {
        // Användaren får ange hela eller delar av registreringsnumret
        string searchTerm = AnsiConsole.Prompt(
            new TextPrompt<string>("Ange hela eller delar av registreringsnumret:")
                .AllowEmpty());

        if (searchTerm == "0")
        {
            return;
        }

        // Validera sök
        if (string.IsNullOrWhiteSpace(searchTerm) ||
            searchTerm.Length > 10 ||
            !searchTerm.All(char.IsLetterOrDigit))
        {
            AnsiConsole.MarkupLine(
                "[red]Ange 1–10 tecken med endast bokstäver och siffror.[/]");
            continue;
        }

        // Sök efter fordon som matchar sök
        List<IVehicle> matches = garage.SearchVehicles(searchTerm);

        // Om inga fordon hittas, visa meddelande
        if (matches.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]Inga fordon hittades.[/]");
            continue;
        }

        // Visa alla matchande fordon med deras parkeringsplats och typ
        foreach (IVehicle vehicle in matches)
        {
            string spotNumbers = string.Join(", ", garage.Spots
                .Where(spot => spot.Vehicles.Contains(vehicle))
                .Select(spot => spot.Number));

            string vehicleType = vehicle switch
            {
                Car => "Bil",
                MC => "MC",
                Bicycle => "Cykel",
                Bus => "Buss",
                _ => throw new InvalidDataException("Okänd fordonstyp.")
            };

            AnsiConsole.MarkupLine(
                $"[cyan]Platsnummer: {spotNumbers} - {vehicleType} - {vehicle.LicensePlate}[/]");
        }

        AnsiConsole.MarkupLine(
            "[grey]Tryck på valfri tangent för att återgå...[/]");
        Console.ReadKey(true);
        return;
    }
}

// Visa parkeringar
static void DisplayParking(ParkingGarage garage)
{
    AnsiConsole.Clear();
    AnsiConsole.MarkupLine("[bold yellow]Parkeringsstatus[/]");

    // Loop för att visa status för varje parkeringsplats
    foreach (IParkingSpot spot in garage.Spots)
    {
        // Om parkeringsplats är tom, visa att den är ledig
        if (spot.Vehicles.Count == 0)
        {
            AnsiConsole.MarkupLine(
                $"Plats {spot.Number}: [green]LEDIG[/] (0/{spot.Capacity})");
            continue;
        }

        // Skapa en lista med beskrivningar av fordonen på parkeringsplatsen
        List<string> descriptions = spot.Vehicles
            .Select(vehicle =>
            {
                string type = vehicle switch
                {
                    Car => "Bil",
                    MC => "MC",
                    Bicycle => "Cykel",
                    Bus => "Bus",
                    _ => throw new InvalidDataException("Okänd fordonstyp.")
                };
                return $"{type} - {vehicle.LicensePlate}";
            })
            .ToList();

        // Bestäm färgen baserat på om parkeringsplatsen är full eller inte, ex 2/4 = gul, 4/4 = röd
        string color = spot.UsedCapacity < spot.Capacity
            ? "yellow"
            : "red";

        // Visa parkeringsplatsens status med fordonen och kapaciteten
        AnsiConsole.MarkupLine(
            $"Plats {spot.Number}: [{color}]" +
            $"{string.Join(" | ", descriptions)}[/] " +
            $"({spot.UsedCapacity}/{spot.Capacity})");
    }

    AnsiConsole.MarkupLine(
        "[grey]Tryck på valfri tangent för att återgå...[/]");
    Console.ReadKey(true);
}

// Metod för att välja ett fordon från parkeringsgaraget
static IVehicle? SelectVehicle(ParkingGarage garage)
{
    // Skapa en lista med registreringsnummer för alla parkerade fordon
    List<string> choices = garage.Spots
        .SelectMany(spot => spot.Vehicles)
        .Distinct()
        .Select(vehicle => vehicle.LicensePlate)
        .ToList();

    // Om inga fordon finns parkerade, visa ett meddelande
    if (choices.Count == 0)
    {
        AnsiConsole.MarkupLine("[red]Det finns inga parkerade fordon.[/]");
        Console.ReadKey(true);
        return null;
    }

    // Add alternativ att gå tillbaka till huvudmenyn
    choices.Add("0. Tillbaka");

    // Användaren får välja ett fordon från listan
    string choice = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title("Välj fordon:")
            .PageSize(15)
            .AddChoices(choices));

    // om 0 gå tillbaka
    if (choice == "0. Tillbaka")
    {
        return null;
    }

    // Hitta och skicka tillbaka det valda fordonet baserat på registreringsnumret
    return garage.FindVehicle(choice);
}

// Skapa en lista med parkeringsdata från parkeringsgaraget
static List<VehicleDataV21> CreateParkingData(ParkingGarage garage)
{
    var data = new List<VehicleDataV21>();

    List<IVehicle> vehicles = garage.Spots
        .SelectMany(spot => spot.Vehicles)
        .Distinct()
        .ToList();

    foreach (IVehicle vehicle in vehicles)
    {
        string vehicleType = vehicle switch
        {
            Car => "CAR",
            MC => "MC",
            Bicycle => "BICYCLE",
            Bus => "BUS",
            _ => throw new Exception("Fordonstypen kan inte sparas.")
        };

        List<int> spotNumbers = garage.Spots
            .Where(spot => spot.Vehicles.Contains(vehicle))
            .Select(spot => spot.Number)
            .OrderBy(number => number)
            .ToList();

        data.Add(new VehicleDataV21
        {
            VehicleType = vehicleType,
            LicensePlate = vehicle.LicensePlate,
            ArrivalTime = vehicle.ArrivalTime,
            SpotNumbers = spotNumbers
        });
    }

    return data;
}

// Skapa parkeringsgaraget beroende på konfigurationen och parkeringsdata
static ParkingGarage CreateGarage(
    List<VehicleDataV21> data,
    GarageConfig config)
{
    ArgumentNullException.ThrowIfNull(data);

    // skapa parkeringsgaraget med antal platser och kapacitet utifrån konfigurationen
    var garage = new ParkingGarage(
        config.NumberOfSpots,
        config.SpotCapacity,
        config.SpotCapacities);
    var plates = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
    var now = DateTimeOffset.Now;

    // Loop genom alla fordon i parkeringsdata för att lägga till dem i parkeringsgaraget
    foreach (VehicleDataV21 item in data)
    {
        // Validera registreringsnummer, ankomsttid och parkeringsplats
        if (item == null ||
            string.IsNullOrWhiteSpace(item.LicensePlate) ||
            item.LicensePlate.Length > 10 ||
            !item.LicensePlate.All(char.IsLetterOrDigit))
        {
            throw new InvalidDataException(
                "Datafilen innehåller ett ogiltigt registreringsnummer.");
        }

        // Kontrollera om registreringsnumret redan finns i listan
        if (!plates.Add(item.LicensePlate))
        {
            throw new InvalidDataException(
                "Datafilen innehåller dubbla registreringsnummer.");
        }

        // Kontrollera om ankomsttiden är giltig (inte standardvärde och inte i framtiden)
        if (item.ArrivalTime == default || item.ArrivalTime > now)
        {
            throw new InvalidDataException(
                "Datafilen innehåller en ogiltig ankomsttid.");
        }

        // Skapa fordon och lägg till den på parkeringsplatsen
        IVehicle vehicle = CreateVehicle(
            item.VehicleType,
            item.LicensePlate,
            item.ArrivalTime,
            config);

        // Kontrollera att parkeringsplatsen är giltig och att fordonet kan stå där
        int expectedSpotCount = vehicle is Bus ? 4 : 1;

        if (item.SpotNumbers == null ||
            item.SpotNumbers.Count != expectedSpotCount ||
            item.SpotNumbers.Distinct().Count() != item.SpotNumbers.Count)
        {
            throw new InvalidDataException(
                "Datafilen anger fel antal platser eller upprepade platsnummer.");
        }

        if (vehicle is Bus)
        {
            List<int> numbers = item.SpotNumbers
                .OrderBy(number => number)
                .ToList();

            List<IParkingSpot> busSpots = garage.FindBusSpots(
                vehicle, numbers[0]);

            if (busSpots.Count != 4 ||
                !busSpots.Select(spot => spot.Number).SequenceEqual(numbers))
            {
                throw new InvalidDataException(
                    "Datafilen anger ogiltiga eller upptagna bussplatser.");
            }

            foreach (IParkingSpot busSpot in busSpots)
            {
                busSpot.Vehicles.Add(vehicle);
            }
        }
        else
        {
            IParkingSpot? spot = garage.Spots.FirstOrDefault(
                spot => spot.Number == item.SpotNumbers[0]);

            if (spot == null || !spot.CanFit(vehicle))
            {
                throw new InvalidDataException(
                    "Datafilen anger en ogiltig eller överfull parkeringsplats.");
            }

            spot.Vehicles.Add(vehicle);
        }
    }

    return garage;
}


// Försök att spara ändringen i parkeringsdatafilen och återställ om det misslyckas
static bool TrySaveChange(
    ParkingGarage garage,
    ParkingFileV21 ParkingFileV21,
    string dataPath,
    Func<bool> change)
{
    // Kopia av tidigare parkerade fordon för att återställa om det misslyckas
    var previousVehicles = garage.Spots
        .Select(spot => spot.Vehicles.ToList())
        .ToList();

    // Om ändringen misslyckas, återställ
    if (!change())
    {
        return false;
    }

    // Försök att spara ändringen i parkeringsdatafilen
    try
    {
        ParkingFileV21.Save(dataPath, CreateParkingData(garage));
        return true;
    }
    catch (Exception ex)
    {
        for (int i = 0; i < garage.Spots.Count; i++)
        {
            garage.Spots[i].Vehicles.Clear();
            garage.Spots[i].Vehicles.AddRange(previousVehicles[i]);
        }

        Console.WriteLine("Ändringen kunde inte sparas och har återställts.");
        Console.WriteLine(ex.Message);
        return false;
    }
}

// Skapa fordon baserat på fordonstyp, registreringsnummer, ankomsttid och konfiguration
static IVehicle CreateVehicle(
    string vehicleType,
    string licensePlate,
    DateTimeOffset arrivalTime,
    GarageConfig config)
{
    // Hämta fordonets inställningar från konfigurationen
    VehicleConfig? settings = config.VehicleTypes.FirstOrDefault(
        item => item.Type == vehicleType);

    // Om inställningarna saknas, kasta
    if (settings == null)
    {
        throw new InvalidDataException(
            "Fordonstypen saknas i konfigurationen.");
    }

    // Skapa fordonet baserat på fordonstypen och skicka tillbaka den
    return vehicleType switch
    {
        "CAR" => new Car(licensePlate, arrivalTime, settings.Size),
        "MC" => new MC(licensePlate, arrivalTime, settings.Size),
        "BICYCLE" => new Bicycle(licensePlate, arrivalTime, settings.Size),
        "BUS" => new Bus(licensePlate, arrivalTime, settings.Size),
        _ => throw new InvalidDataException("Okänd fordonstyp.")
    };
}

// Beräkna parkeringsavgiften baserat på parkeringstid och timpris
static decimal CalculateFee(TimeSpan parkedTime, decimal hourlyRate)
{
    if (parkedTime < TimeSpan.Zero)
    {
        throw new Exception("Parkeringstiden får inte vara negativ.");
    }

    if (hourlyRate <= 0)
    {
        throw new Exception("Timpriset måste vara större än noll.");
    }

    if (parkedTime <= TimeSpan.FromMinutes(10))
    {
        return 0;
    }

    decimal startedHours = (decimal)Math.Ceiling(parkedTime.TotalHours);

    return startedHours * hourlyRate;
}

// Läs om konfigurationen och kontrollera den mot parkerade fordon.
static void ReloadConfig(
    ref ParkingGarage garage,
    ref GarageConfig config)
{
    AnsiConsole.Clear();
    Console.WriteLine("Läs om konfiguration");

    try
    {
        string configPath = Path.Combine(
            AppContext.BaseDirectory,
            "config.json");

        GarageConfig updatedConfig =
            new ConfigFileV21().Load(configPath);

        List<VehicleDataV21> data = CreateParkingData(garage);

        ParkingGarage updatedGarage = CreateGarage(
            data, updatedConfig);

        garage = updatedGarage;
        config = updatedConfig;

        Console.WriteLine("Konfigurationen har uppdaterats.");
    }
    catch (Exception ex)
    {
        Console.WriteLine(
            "Ändringen avvisades. Tidigare inställningar används.");
        Console.WriteLine(ex.Message);
    }

    Console.WriteLine();
    Console.WriteLine("Aktiva inställningar:");
    Console.WriteLine($"Antal platser: {config.NumberOfSpots}");
    Console.WriteLine($"Standardkapacitet: {config.SpotCapacity}");

    foreach (var item in config.SpotCapacities)
    {
        Console.WriteLine(
            $"Plats {item.Key}: kapacitet {item.Value}");
    }

    foreach (VehicleConfig vehicle in config.VehicleTypes)
    {
        Console.WriteLine(
            $"{vehicle.Type}: storlek {vehicle.Size}, " +
            $"{vehicle.HourlyRate:0.00} CZK/timme");
    }

    Console.WriteLine("Tryck på valfri tangent...");
    Console.ReadKey(true);
}

// Visa parkeringskarta med färgkodning för lediga, delvis upptagna och fulla platser
static void ShowParkingMap(ParkingGarage garage)
{
    AnsiConsole.Clear();
    AnsiConsole.MarkupLine("[bold yellow]Parkeringskarta[/]");
    Console.WriteLine("                         IN / UT");
    Console.WriteLine();

    int column = 0;
    int freeSpots = 0;

    // Loop genom alla parkeringsplatser och visa deras status med färgkodning
    foreach (IParkingSpot spot in garage.Spots)
    {
        string color;
        string status;

        if (spot.Vehicles.Count == 0)
        {
            color = "black on green";
            status = "L";
            freeSpots++;
        }
        else if (spot.UsedCapacity < spot.Capacity)
        {
            color = "black on yellow";
            status = "D";
        }
        else
        {
            color = "white on red";
            status = "F";
        }

        AnsiConsole.Markup(
            $"[{color}] {spot.Number:000} {status} [/]");

        column++;

        if (column == 3 || column == 5)
        {
            Console.Write("   ");
        }

        if (column == 8)
        {
            Console.WriteLine();
            column = 0;
        }
    }

    // Om det finns kvarvarande kolumner, lägg till en ny rad
    if (column > 0)
    {
        Console.WriteLine();
    }

    Console.WriteLine();
    Console.WriteLine("L = Ledig   D = Delvis upptagen   F = Full");
    Console.WriteLine(
        $"Helt lediga platser: {freeSpots} av {garage.Spots.Count}");

    Console.WriteLine("Tryck på valfri tangent...");
    Console.ReadKey(true);
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
                "6. Läs om konfiguration",
                "7. Visa parkeringskarta",
                "0. Avsluta"
            )
    );

    // switch case för att hantera användarens val
    switch (choice)
    {
        case "1. Parkera fordon":
            ParkVehicle(parkingGarage, ParkingFileV21, dataPath, config);
            break;

        case "2. Flytta fordon":
            MoveVehicle(parkingGarage, ParkingFileV21, dataPath);
            break;

        case "3. Hämta ut fordon":
            RemoveVehicle(parkingGarage, ParkingFileV21, dataPath, config);
            break;

        case "4. Sök efter fordon":
            SearchVehicles(parkingGarage);
            break;

        case "5. Visa parkering":
            DisplayParking(parkingGarage);
            break;

        case "6. Läs om konfiguration":
            ReloadConfig(ref parkingGarage, ref config);
            break;

        case "7. Visa parkeringskarta":
            ShowParkingMap(parkingGarage);
            break;

        case "0. Avsluta":
            return;
    }

}
