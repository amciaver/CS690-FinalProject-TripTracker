namespace TripTrackerProject;

using Microsoft.VisualBasic;
using Spectre.Console;
using System.IO;

public class ConsoleUI {
    Datamanager datamanager;
    Reporter reporter;
    public IAnsiConsole consoleAnsi{ get; set; } = AnsiConsole.Console;
    FileSaver fileSaver;

    public ConsoleUI() {
        datamanager = new Datamanager();
        reporter = new Reporter();
        fileSaver = new FileSaver();
    }

    public void Show() {

        List<string> tripSelectChoices = datamanager.Trips.ConvertAll(t => t.Name);
        tripSelectChoices.Add("Enter New Trip");
        tripSelectChoices.Add("Exit Application");

        string selectedTrip;
        do{    
            selectedTrip = AskForSelection("Please select a Trip", tripSelectChoices);
            Console.WriteLine("You have selected: " + selectedTrip);
            
            string tripName;
            if(selectedTrip == "Enter New Trip"){
                tripName = AskForInput("Enter new trip name: ");
                if(tripSelectChoices.Contains(tripName)){
                    Console.WriteLine("Trip already exists!");
                }else{
                tripSelectChoices.Add(tripName);
                tripSelectChoices.Remove("Enter New Trip");
                tripSelectChoices.Remove("Exit Application");
                tripSelectChoices.Add("Enter New Trip");
                tripSelectChoices.Add("Exit Application");

                Trip newTripName = new Trip(tripName);
                datamanager.Trips.Add(newTripName);
                fileSaver.SyncTrips(datamanager.Trips);
                selectedTrip = tripName;
                }
            }

            string trackEntryCommand;
            if(selectedTrip != "Exit Application"){
                do{
                    Console.WriteLine(Environment.NewLine + "Selected Trip = " + selectedTrip);
                    
                    List<string> trackEntryCommandChoices = new List <string> {"Track Photo", "Track Cost","Track Note", "Display Trip Records", "Total Trip Cost","Edit Entry Information", "Delete Tracked Entry", "Return To Home Menu"};
                    trackEntryCommand = AskForSelection("Please select an action:", trackEntryCommandChoices);
                    Console.WriteLine(Environment.NewLine + "Selected Action = " + trackEntryCommand);
                    
                    if(trackEntryCommand == "Track Photo"){

                        datamanager.TrackPhoto(this, selectedTrip);

                    }else if(trackEntryCommand == "Track Cost"){

                        datamanager.TrackCost(this, selectedTrip);
                    
                    }else if(trackEntryCommand == "Track Note"){
                        
                        datamanager.TrackNote(this, selectedTrip);

                    }else if(trackEntryCommand == "Display Trip Records"){

                        reporter.DisplayRecords(datamanager, selectedTrip);
                        
                    }else if(trackEntryCommand == "Total Trip Cost"){
                        
                        reporter.TotalTripCost(datamanager, selectedTrip);

                    }else if(trackEntryCommand == "Delete Tracked Entry"){
                        datamanager.DeleteTrackedEntry(this, selectedTrip);
                    }else if (trackEntryCommand == "Edit Entry Information"){
                        datamanager.EditTrackedEntry(this, selectedTrip);
                    }
                }while (trackEntryCommand!= "Return To Home Menu");
            }
        }while(selectedTrip != "Exit Application");
    }
    public string AskForInput(string message){
    string? input;
    do{
        Console.WriteLine(message);
        input = Console.ReadLine();
        if(string.IsNullOrEmpty(input)){
            Console.WriteLine("Please enter an input.");
        }

    }while(string.IsNullOrEmpty(input));
    return input;
    }

    public int AskForInteger(string message){
        int input;
        string? rawInput;
        do{
            Console.WriteLine(message);
            do{
                rawInput = Console.ReadLine();
                
            }while(string.IsNullOrEmpty(rawInput));

        }while(!int.TryParse(rawInput, out input));
        return input;
    }

    public string AskForTrackItemString(string message){
        string itemAttribute;
        do{
            itemAttribute = AskForInput(message);
            if(itemAttribute.Contains(",")){
                Console.WriteLine("Input cannot contain ',' characters!");
            }
        }while(itemAttribute.Contains(","));
        return itemAttribute;
    }

    public string AskForSelection(string message, List<string> choices){
        return consoleAnsi.Prompt(
            new SelectionPrompt<string>()
            .Title(message)
            .AddChoices(choices));
    }

    public double AskForPrice(string message){
        double costPrice;
        while (true){
            string input = AskForInput(Environment.NewLine + message);
            if(double.TryParse(input, out costPrice)){
                break;
            }
            Console.WriteLine("Invalid Input. Please enter a number.");
            
        }
        return costPrice;
    }
}
