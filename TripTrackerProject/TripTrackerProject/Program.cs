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
                    var photoName = splitted[1];
                    var photoLocation = splitted[2];
                    var photoTimeOfDay = splitted[3];
                    Photo readPhoto = new Photo(photoName,photoLocation,photoTimeOfDay);
                    trip.Photos.Add(readPhoto);
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
                    
                    List<string> trackEntryCommandChoices = new List <string> {"Track Photo", "Exit"};
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
                                SyncPhotos(selectedTrip,Trips);
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

    public static void SyncPhotos(string targetTrip, List<Trip>Trips)
    {
        foreach (Trip trip in Trips){
            if (trip.Name == targetTrip){
                string fileName = trip.Name + ".txt";
                File.Delete(fileName);
                foreach (Photo photo in trip.Photos){
                    File.AppendAllText(fileName,"Photo," + photo.Name + "," + photo.Location + "," + photo.TimeOfDay + Environment.NewLine);
                }
                Console.WriteLine(Environment.NewLine + "Photos Synced for: " + trip.Name);
            }
        }
    }
}
