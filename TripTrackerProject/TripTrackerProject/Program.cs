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
                        var photoName = splitted[1];
                        var photoLocation = splitted[2];
                        var photoTimeOfDay = splitted[3];
                        Photo readPhoto = new Photo(photoName,photoLocation,photoTimeOfDay);
                        trip.Photos.Add(readPhoto);
                    }else if(trackingType == "Cost")
                    {
                        string costDescription = splitted[1];
                        double costPrice = double.Parse(splitted[2]);
                        var costLocation = splitted[3];
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
        
        string selectedTrip = null;
        do{    
            selectedTrip = AskForSelection("Please select a Trip", tripSelectChoices);
            Console.WriteLine("You have selected: " + selectedTrip);
        
            string tripListFileName = "trips.txt";
            
            string tripName = null;
            if(selectedTrip == "Enter New Trip"){
                tripName = AskForInput("Enter new trip name: ");
                if(tripSelectChoices.Contains(tripName)){
                    Console.WriteLine("Trip already exists!");
                }else{
                tripSelectChoices.Add(tripName);
                Trip newTripName = new Trip(tripName);
                Trips.Add(newTripName);
                SyncTrips(Trips);
                selectedTrip = tripName;
                }
            }

            string trackEntryCommand = null;
            if(selectedTrip != "Exit Application"){
                do{
                    Console.WriteLine(Environment.NewLine + "Selected Trip = " + selectedTrip);
                    
                    List<string> trackEntryCommandChoices = new List <string> {"Track Photo", "Track Cost","Track Note", "Exit"};
                    trackEntryCommand = AskForSelection("Please select an action", trackEntryCommandChoices);
                    Console.WriteLine("Selected Action = " + trackEntryCommand);
                    
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
                        double costPrice = double.Parse(AskForInput(Environment.NewLine + "Please enter the price: "));
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
                    }


                }while (trackEntryCommand!= "Exit");
            }
        }while(selectedTrip != "Exit Application");
    }

    public static string AskForInput(string message){
        Console.WriteLine(message);
        return Console.ReadLine();
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
