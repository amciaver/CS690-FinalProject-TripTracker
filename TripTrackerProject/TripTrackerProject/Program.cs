namespace TripTrackerProject;

using Microsoft.VisualBasic;
using Spectre.Console;
using System.IO;

class Program
{
    static void Main(string[] args)
    {   
        Datamanager datamanager = new Datamanager();
        Console.WriteLine(Environment.NewLine + "Welcome to the TripTracker Application!");

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
                datamanager.SyncTrips(datamanager.Trips);
                selectedTrip = tripName;
                }
            }

            string trackEntryCommand;
            if(selectedTrip != "Exit Application"){
                do{
                    Console.WriteLine(Environment.NewLine + "Selected Trip = " + selectedTrip);
                    
                    List<string> trackEntryCommandChoices = new List <string> {"Track Photo", "Track Cost","Track Note", "Display Trip Records", "Total Trip Cost", "Return To Home Menu"};
                    trackEntryCommand = AskForSelection("Please select an action:", trackEntryCommandChoices);
                    Console.WriteLine(Environment.NewLine + "Selected Action = " + trackEntryCommand);
                    
                    if(trackEntryCommand == "Track Photo"){

                        TrackPhoto(datamanager, selectedTrip);

                    }else if(trackEntryCommand == "Track Cost"){

                        TrackCost(datamanager, selectedTrip);
                    
                    }else if(trackEntryCommand == "Track Note"){
                        
                        TrackNote(datamanager, selectedTrip);
    
                    }else if(trackEntryCommand == "Display Trip Records"){
                        foreach (Trip trip in datamanager.Trips){
                            if(selectedTrip == trip.Name){
                                
                                if(trip.Photos.Count == 0){
                                    Console.WriteLine("No photos recorded!");
                                }else{
                                    var photoTable = new Table();
                                    photoTable.AddColumn("Photo Name");
                                    photoTable.AddColumn("Location");
                                    photoTable.AddColumn("Time of Day");
                                    photoTable.AddColumn("Entry Date & Time");
                                    foreach(var photo in trip.Photos) {
                                        photoTable.AddRow(photo.Name, photo.Location, photo.TimeOfDay, photo.DateTimeStamp);
                                    }
                                    AnsiConsole.Write(photoTable);
                                }
                                    

                                if(trip.Costs.Count == 0){
                                    Console.WriteLine("No costs recorded!");
                                }else{
                                    var costTable = new Table();
                                    costTable.AddColumn("Cost Description");
                                    costTable.AddColumn("Price");
                                    costTable.AddColumn("Location");
                                    costTable.AddColumn("Entry Date & Time");
                                    foreach(var cost in trip.Costs) {
                                        costTable.AddRow(cost.Description, cost.Price.ToString("N2"), cost.Location, cost.DateTimeStamp);
                                    }
                                    AnsiConsole.Write(costTable);
                                }

                                if(trip.Notes.Count == 0){
                                    Console.WriteLine("No notes recorded!");
                                }else{
                                    var noteTable = new Table();
                                    noteTable.AddColumn("Note Name");
                                    noteTable.AddColumn("Description");
                                    noteTable.AddColumn("Source");
                                    noteTable.AddColumn("Entry Date & Time");
                                    foreach(var note in trip.Notes) {
                                        noteTable.AddRow(note.Name, note.Description, note.Source, note.DateTimeStamp);
                                    }
                                    AnsiConsole.Write(noteTable);
                                }
                            }
                        }
                    }else if(trackEntryCommand == "Total Trip Cost"){
                        foreach (Trip trip in datamanager.Trips){
                            if(selectedTrip == trip.Name){
                                double tripCostSum = 0;
                                foreach(var cost in trip.Costs){
                                    tripCostSum += cost.Price;
                                }
                                Console.WriteLine("Total trip cost is: $" + tripCostSum.ToString("N2"));
                            }
                        }
                    }
                }while (trackEntryCommand!= "Return To Home Menu");
            }
        }while(selectedTrip != "Exit Application");
    }

    public static string AskForInput(string message){
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

    public static string AskForTrackItemString(string message){
        string itemAttribute;
        do{
            itemAttribute = AskForInput(message);
            if(itemAttribute.Contains(",")){
                Console.WriteLine("Input cannot contain ',' characters!");
            }
        }while(itemAttribute.Contains(","));
        return itemAttribute;
    }

    public static string AskForSelection(string message, List<string> choices){
        return AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title(message)
            .AddChoices(choices));
    }
    public static void TrackPhoto(Datamanager datamanager, string selectedTrip){
        string photoName = AskForTrackItemString("Please enter the photo name:");
        string photoLocation = AskForTrackItemString("Please enter the photo location");
        List<string> timeOfDayChoices = new List<string> {"Morning", "Day", "Night"};
        string photoTime = AskForSelection("Please select the time of day the photo was taken:", timeOfDayChoices);
        string photoDateTimeStamp = DateTime.Now.ToString();
        Photo newPhoto = new Photo(photoName, photoLocation, photoTime, photoDateTimeStamp);
        foreach(Trip trip in datamanager.Trips){
            if(selectedTrip == trip.Name){
                trip.Photos.Add(newPhoto);
                string tripFileName = selectedTrip + ".txt";
                datamanager.SyncTripData(selectedTrip,datamanager.Trips);
            }
        }
    }

    public static void TrackCost(Datamanager datamanager, string selectedTrip){
        
        string costDescription = AskForTrackItemString("Please enter a description: ");
        double costPrice;
        while (true){
            string input = AskForInput(Environment.NewLine + "Please enter the price: ");
            if(double.TryParse(input, out costPrice)){
                break;
            }
            Console.WriteLine("Invalid Input. Please enter a number.");
            
        }

        string costLocation = AskForTrackItemString(Environment.NewLine + "Please enter the location of the purchase: ");
        string costDateTimeStamp = DateTime.Now.ToString();
        Cost newCost = new Cost(costDescription, costPrice, costLocation, costDateTimeStamp);
        foreach(Trip trip in datamanager.Trips){
            if(selectedTrip == trip.Name){
                trip.Costs.Add(newCost);
                string tripFileName = selectedTrip + ".txt";
                datamanager.SyncTripData(selectedTrip,datamanager.Trips);
            }
        }
    }

    public static void TrackNote(Datamanager datamanager, string selectedTrip){
        
        string noteName = AskForTrackItemString("Please enter the name of the note: ");
        string noteDescription = AskForTrackItemString(Environment.NewLine + "Please enter a description");
        string noteSource = AskForTrackItemString(Environment.NewLine + "Please enter the source of the information: ");
        string noteDateTimeStamp = DateTime.Now.ToString();
        Note newNote = new Note(noteName, noteDescription, noteSource, noteDateTimeStamp);
        foreach(Trip trip in datamanager.Trips){
            if(selectedTrip == trip.Name){
                trip.Notes.Add(newNote);
                string tripFileName = selectedTrip + ".txt";
                datamanager.SyncTripData(selectedTrip,datamanager.Trips);
            }
        }
    }
}
