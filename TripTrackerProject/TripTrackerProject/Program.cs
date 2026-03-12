namespace TripTrackerProject;

using Spectre.Console;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Welcome to the Trip Tracker Application!");

        List<string> tripList = new List<string>();
        var trips = File.ReadAllLines("trips.txt");
        foreach (var trip in trips){
            tripList.Add(trip);
        }
        tripList.Add("Enter New Trip");
        tripList.Add("Exit Application");
        
        string selectedTrip = null;
        do{
            selectedTrip = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Please select a Trip:")
                .AddChoices(tripList));
            Console.WriteLine("You have selected: " + selectedTrip);

            string tripName = null;
            if(selectedTrip == "Enter New Trip"){
                Console.WriteLine("Please Enter The Trip Name: ");
                tripName = Console.ReadLine();
                if(tripList.Contains(tripName)){
                    Console.WriteLine("Trip already exists!");
                }else{
                File.AppendAllText("trips.txt", tripName + Environment.NewLine);
                tripList.Add(tripName);
                }
                selectedTrip = tripName;
            }
            string trackEntryCommand = null;
            if(selectedTrip != "Exit Application"){
                do{
                    Console.WriteLine(Environment.NewLine + "Selected Trip = " + selectedTrip);
                    trackEntryCommand = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                        .Title("Please select an action")
                        .AddChoices("Track Photo", "Exit"));
                    
                    Console.WriteLine("Selected Action = " + trackEntryCommand);
                    
                    if(trackEntryCommand == "Track Photo"){
                        Console.WriteLine("Please enter the photo name:");
                        string photoName = Console.ReadLine();

                        Console.WriteLine(Environment.NewLine + "Please enter the photo location:");
                        string photoLocation = Console.ReadLine();

                        var photoTime = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                            .Title("Please choose the time of day the photo was taken:")
                            .AddChoices("Morning", "Day", "Night"));
                        
                        string tripFileName = selectedTrip + ".txt";
                        File.AppendAllText(tripFileName, photoName + "," + photoLocation + "," + photoTime + Environment.NewLine);
                        Console.WriteLine(Environment.NewLine + "Photo information written to " + tripFileName + ": " +
                        photoName + "," + photoLocation + "," + photoTime);
                    }
                }while (trackEntryCommand!= "Exit");
            }
        }while(selectedTrip != "Exit Application");
    }
}
