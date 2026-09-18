// ======================================================
// Prague Parking V1 - Första inlämningsuppgiften i kursen Objektorienterad programmering C# på KYH Systemutveckling DevSecOps.
// Skapad av: Walter Esko
// Datum: 2026-09-18
// Beskrivning: Parkeringssystem för hantering av bilar och MC. Gjord med hjälp av spectre .console för att skapa en snygg konsolapplikation. Användaren kan parkera, flytta, hämta ut och söka efter fordon samt visa parkeringsstatus. Systemet hanterar både bilar och motorcyklar, där motorcyklar kan stå två på samma plats, och bilar kan stå en per plats. Systemet är byggt med C# 14.0 och .NET 10.0.
// ======================================================

using Spectre.Console;

string?[] parkingGarage = new string?[100]; // array för att hålla alla lagrade bilar i formatet MC#ABC123,  CAR#KLM123. index börjar på 0 vilket motsvarar garageplats 1, index 1 motsvarar garageplats 2 osv. 

// dummy fordon för testing
parkingGarage[9] = "MC#ABC123|MC#DEF456";
parkingGarage[15] = "CAR#GHI789";
parkingGarage[24] = "CAR#JKL012";
parkingGarage[32] = "CAR#MNO345";


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

    // återgå till huvudmenyn
    if (vehicleType == "Tillbaka")
    {
        return;
    }

    // prompt användaren att ange registreringsnummer och konvertera till uppercase, satt den som textprompt och allow empty för att kunna göra nästa validering (string.IsNullOrWhiteSpace(licensePlate))
    string licensePlate = AnsiConsole.Prompt(
        new TextPrompt<string>("Ange registreringsnummer (ex. ABC123):")
            .AllowEmpty()
    ).ToUpper();

    // validera att registreringsnummer inte är tomt annars återgå till huvudmenyn
    if (string.IsNullOrWhiteSpace(licensePlate))
    {
        AnsiConsole.MarkupLine("[red]Registreringsnummer får inte vara tomt.[/]");
        Console.ReadKey(true);
        return;
    }

    // validera att registreringsnummer inte är längre än 10 tecken annars återgå till huvudmenyn
    if (licensePlate.Length > 10)
    {
        AnsiConsole.MarkupLine("[red]Registreringsnummer får max vara 10 tecken.[/]");
        Console.ReadKey(true);
        return;
    }

    // validera att registreringsnummer endast innehåller bokstäver och siffror annars återgå till huvudmenyn
    if (!licensePlate.All(char.IsLetterOrDigit))
    {
        AnsiConsole.MarkupLine("[red]Registreringsnummer får endast innehålla bokstäver och siffror.[/]");
        Console.ReadKey(true);
        return;
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
            //MoveCar(parkingGarage);
            break;

        case "3. Hämta ut fordon":
            //RemoveCar(parkingGarage);
            break;

        case "4. Sök efter fordon":
            //SearchCars(parkingGarage);
            break;

        case "5. Visa parkering":
            //ShowParkingStatus(parkingGarage);
            break;

        case "0. Avsluta":
            return;
    }

}