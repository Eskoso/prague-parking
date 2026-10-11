// ======================================================
// Prague Parking V1.1 - VG inlämningsuppgift i kursen Objektorienterad programmering C# på KYH Systemutveckling DevSecOps.
// Skapad av: Walter Esko
// Datum: 2026-10-09
// Beskrivning: Parkeringssystem för hantering av bilar och MC. Gjord med hjälp av spectre .console för att skapa en snygg konsolapplikation. Användaren kan parkera, flytta, hämta ut och söka efter fordon samt visa parkeringsstatus. Systemet hanterar både bilar och motorcyklar, där motorcyklar kan stå två på samma plats, och bilar kan stå en per plats. Systemet är byggt med C# 14.0 och .NET 10.0.
// ======================================================

using Spectre.Console;

string?[] parkingGarage = new string?[100]; // array för att hålla alla lagrade bilar i formatet MC#ABC123,  CAR#KLM123. index börjar på 0 vilket motsvarar garageplats 1, index 1 motsvarar garageplats 2 osv. 

// dummy fordon för testing
//parkingGarage[9] = "MC#ABC123|MC#DEF456";
//parkingGarage[15] = "CAR#GHI789";
//parkingGarage[24] = "CAR#JKL012";
//parkingGarage[32] = "CAR#MNO345";


// Parkera fordon
static void ParkCar(string?[] parkingGarage)
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
}

// Flytta fordon
static void MoveCar(string?[] parkingGarage)
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
}

// hämta ut fordon
static void RemoveCar(string?[] parkingGarage)
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
}

// sök efter fordon
static void SearchCars(string?[] parkingGarage)
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
}

// Visa parkeringar
static void ShowParkingStatus(string?[] parkingGarage)
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
            ParkCar(parkingGarage);
            break;

        case "2. Flytta fordon":
            MoveCar(parkingGarage);
            break;

        case "3. Hämta ut fordon":
            RemoveCar(parkingGarage);
            break;

        case "4. Sök efter fordon":
            SearchCars(parkingGarage);
            break;

        case "5. Visa parkering":
            ShowParkingStatus(parkingGarage);
            break;

        case "0. Avsluta":
            return;
    }

}