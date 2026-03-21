namespace TripTrackerProject;

using Microsoft.VisualBasic;
using Spectre.Console;
using System.IO;

class Program
{
    static void Main(string[] args)
    {   
        Console.WriteLine(Environment.NewLine + "Welcome to the TripTracker Application!");
        List<Trip> Trips = new List<Trip>();
        
        var tripsFileContent = File.ReadAllLines("trips.txt");
        foreach(var tripName in tripsFileContent){
            Trips.Add(new Trip(tripName));
        }

        foreach(Trip trip in Trips){
            string fileName = trip.Name + ".txt";
            if (File.Exists(fileName))
            {
                var fileData = File.ReadAllLines(fileName);
                foreach(var line in fileData)
                {
                    var splitted = line.Split(",",StringSplitOptions.RemoveEmptyEntries);
                    string trackingType = splitted[0];
                    if(trackingType == "Photo")
                    {
                        string photoName = splitted[1];
                        string photoLocation = splitted[2];
                        string photoTimeOfDay = splitted[3];
                        Photo readPhoto = new Photo(photoName,photoLocation,photoTimeOfDay);
                        trip.Photos.Add(readPhoto);
                    }else if(trackingType == "Cost")
                    {
                        string costDescription = splitted[1];
                        double costPrice = double.Parse(splitted[2]);
                        string costLocation = splitted[3];
                        Cost readCost = new Cost(costDescription,costPrice,costLocation);
                        trip.Costs.Add(readCost);
                    }else if(trackingType == "Note")
                    {
                        string noteName = splitted[1];
                        string noteDescription = splitted[2];
                        string noteSource = splitted[3];
                        Note readNote = new Note(noteName,noteDescription,noteSource);
                        trip.Notes.Add(readNote);
                    }
                }
            }
        }
        List<string> tripSelectChoices = Trips.ConvertAll(t => t.Name);
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
                Trips.Add(newTripName);
                SyncTrips(Trips);
                selectedTrip = tripName;
                }
            }

            string trackEntryCommand;
            if(selectedTrip != "Exit Application"){
                do{
                    Console.WriteLine(Environment.NewLine + "Selected Trip = " + selectedTrip);
                    
                    List<string> trackEntryCommandChoices = new List <string> {"Track Photo", "Track Cost","Track Note", "Display Trip Records", "Return To Home Menu"};
                    trackEntryCommand = AskForSelection("Please select an action", trackEntryCommandChoices);
                    Console.WriteLine(Environment.NewLine + "Selected Action = " + trackEntryCommand);
                    
                    if(trackEntryCommand == "Track Photo"){

                        string photoName = AskForInput("Please enter the photo name:");
                        string photoLocation = AskForInput(Environment.NewLine + "Please enter the photo location:");
                        List<string> timeOfDayChoices = new List<string> {"Morning", "Day", "Night"};
                        string photoTime = AskForSelection("Please select the time of day the photo was taken:", timeOfDayChoices);
                        
                        Photo newPhoto = new Photo(photoName, photoLocation, photoTime);
                        foreach(Trip trip in Trips)
                        {
                            if(selectedTrip == trip.Name)
                            {
                                trip.Photos.Add(newPhoto);
                                string tripFileName = selectedTrip + ".txt";
                                SyncTripData(selectedTrip,Trips);
                            }
                        }
                    }else if(trackEntryCommand == "Track Cost"){

                        string costDescription = AskForInput("Please enter a description: ");
                        
                        double costPrice;
                        while (true){
                            string input = AskForInput(Environment.NewLine + "Please enter the price: ");
                            if(double.TryParse(input, out costPrice)){
                                break;
                            }
                            Console.WriteLine("Invalid Input. Please enter a number.");
                        }
                
                        string costLocation = AskForInput(Environment.NewLine + "Please enter the location of purchase: ");
                        Cost newCost = new Cost(costDescription, costPrice, costLocation);
                        foreach(Trip trip in Trips)
                        {
                            if(selectedTrip == trip.Name)
                            {
                                trip.Costs.Add(newCost);
                                string tripFileName = selectedTrip + ".txt";
                                SyncTripData(selectedTrip,Trips);
                            }
                        }
                    }else if(trackEntryCommand == "Track Note"){

                        string noteName = AskForInput("Please enter the name of the note:  ");
                        string noteDescription = AskForInput(Environment.NewLine + "Please enter a description: ");
                        string noteSource = AskForInput(Environment.NewLine + "Please enter the source of the information: ");
                        Note newNote = new Note(noteName, noteDescription, noteSource);
                        foreach(Trip trip in Trips)
                        {
                            if(selectedTrip == trip.Name)
                            {
                                trip.Notes.Add(newNote);
                                string tripFileName = selectedTrip + ".txt";
                                SyncTripData(selectedTrip,Trips);
                            }
                        }
                    }else if(trackEntryCommand == "Display Trip Records"){
                        foreach (Trip trip in Trips){
                            if(selectedTrip == trip.Name){
                                
                                if(trip.Photos.Count == 0){
                                    Console.WriteLine("No photos recorded!");
                                }else{
                                    var photoTable = new Table();
                                    photoTable.AddColumn("Photo Name");
                                    photoTable.AddColumn("Location");
                                    photoTable.AddColumn("Time of Day");
                                    foreach(var photo in trip.Photos) {
                                        photoTable.AddRow(photo.Name, photo.Location, photo.TimeOfDay);
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
                                    foreach(var cost in trip.Costs) {
                                        costTable.AddRow(cost.Description, cost.Price.ToString(), cost.Location);
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
                                    foreach(var note in trip.Notes) {
                                        noteTable.AddRow(note.Name, note.Description, note.Source);
                                    }
                                    AnsiConsole.Write(noteTable);
                                }
                            }
                        }
                    }
                }while (trackEntryCommand!= "Return To Home Menu");
            }
        }while(selectedTrip != "Exit Application");
    }

    public static string AskForInput(string message)
    {
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

    public static string AskForSelection(string message, List<string> choices){
        return AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title(message)
            .AddChoices(choices));
    }

    public static void FileSaver(string fileName, string writeToFile){
        File.AppendAllText(fileName, writeToFile);
    }
    public static void SyncTrips(List<Trip>Trips)
    {
        File.Delete("trips.txt");
        foreach(Trip enteredTrip in Trips)
        {
            File.AppendAllText("trips.txt", enteredTrip + Environment.NewLine);
        }
        Console.WriteLine("Trips have been synchronized to the text file");
    }

    public static void SyncTripData(string targetTrip, List<Trip>Trips)
    {
        foreach (Trip trip in Trips){
            if (trip.Name == targetTrip){
                string fileName = trip.Name + ".txt";
                File.Delete(fileName);
                foreach (Photo photo in trip.Photos){
                    File.AppendAllText(fileName,"Photo," + photo.Name + "," + photo.Location + "," + photo.TimeOfDay + Environment.NewLine);
                }
                Console.WriteLine(Environment.NewLine + "Photos Synced for: " + trip.Name);

                foreach (Cost cost in trip.Costs){
                    File.AppendAllText(fileName,"Cost," + cost.Description + "," + cost.Price + "," + cost.Location + Environment.NewLine);
                }
                Console.WriteLine(Environment.NewLine + "Costs Synced for: " + trip.Name);
                
                foreach (Note note in trip.Notes){
                    File.AppendAllText(fileName,"Note," + note.Name + "," + note.Description + "," + note.Source + Environment.NewLine);
                }
                Console.WriteLine(Environment.NewLine + "Notes Synced for: " + trip.Name);
            }
        }
    }
}
