namespace TripTrackerProject;

using Microsoft.VisualBasic;
using Spectre.Console;
using System.IO;

public class Datamanager{

    public IAnsiConsole iAnsi { get; set; } = AnsiConsole.Console;
    FileSaver fileSaver = new FileSaver();

    public List <Trip> Trips {get;}

    public Datamanager(){

        Trips = new List<Trip>();

        ReadSavedTrips();
        ReadTripData();
    }




    public void TrackPhoto(ConsoleUI consoleUI, string selectedTrip){
        string photoName = consoleUI.AskForTrackItemString("Please enter the photo name:");
        string photoLocation = consoleUI.AskForTrackItemString("Please enter the photo location");
        List<string> timeOfDayChoices = new List<string> {"Morning", "Day", "Night"};
        string photoTime = consoleUI.AskForSelection("Please select the time of day the photo was taken:", timeOfDayChoices);
        string photoDateTimeStamp = DateTime.Now.ToString();
        Photo newPhoto = new Photo(photoName, photoLocation, photoTime, photoDateTimeStamp);
        foreach(Trip trip in Trips){
            if(selectedTrip == trip.Name){
                trip.Photos.Add(newPhoto);
                string tripFileName = selectedTrip + ".txt";
                fileSaver.SyncTripData(selectedTrip,Trips);
            }
        }
    }

    public void TrackCost(ConsoleUI consoleUI, string selectedTrip){
        
        string costDescription = consoleUI.AskForTrackItemString("Please enter a description: ");
        double costPrice;
        while (true){
            string input = consoleUI.AskForInput(Environment.NewLine + "Please enter the price: ");
            if(double.TryParse(input, out costPrice)){
                break;
            }
            Console.WriteLine("Invalid Input. Please enter a number.");
            
        }

        string costLocation = consoleUI.AskForTrackItemString(Environment.NewLine + "Please enter the location of the purchase: ");
        string costDateTimeStamp = DateTime.Now.ToString();
        Cost newCost = new Cost(costDescription, costPrice, costLocation, costDateTimeStamp);
        foreach(Trip trip in Trips){
            if(selectedTrip == trip.Name){
                trip.Costs.Add(newCost);
                string tripFileName = selectedTrip + ".txt";
                fileSaver.SyncTripData(selectedTrip,Trips);
            }
        }
    }

    public void TrackNote(ConsoleUI consoleUI, string selectedTrip){
        
        string noteName = consoleUI.AskForTrackItemString("Please enter the name of the note: ");
        string noteDescription = consoleUI.AskForTrackItemString(Environment.NewLine + "Please enter a description");
        string noteSource = consoleUI.AskForTrackItemString(Environment.NewLine + "Please enter the source of the information: ");
        string noteDateTimeStamp = DateTime.Now.ToString();
        Note newNote = new Note(noteName, noteDescription, noteSource, noteDateTimeStamp);
        foreach(Trip trip in Trips){
            if(selectedTrip == trip.Name){
                trip.Notes.Add(newNote);
                string tripFileName = selectedTrip + ".txt";
                fileSaver.SyncTripData(selectedTrip,Trips);
            }
        }
    }

    public void ReadSavedTrips(){
        if(File.Exists("trips.txt")){
            Trips.Clear();
            var tripsFileContent = File.ReadAllLines("trips.txt");
            foreach(var tripName in tripsFileContent){
                Trips.Add(new Trip(tripName));
            }
        }
    }

    public void ReadTripData(){
        foreach(Trip trip in Trips){
            string fileName = trip.Name + ".txt";
            if (File.Exists(fileName)){
                var fileData = File.ReadAllLines(fileName);
                foreach(var line in fileData)
                {
                    var splitted = line.Split(",",StringSplitOptions.RemoveEmptyEntries);
                    string trackingType = splitted[0];
                    
                    if(trackingType == "Photo"){
                        string photoName = splitted[1];
                        string photoLocation = splitted[2];
                        string photoTimeOfDay = splitted[3];
                        string photoDateTimeStamp = splitted[4];
                        Photo readPhoto = new Photo(photoName,photoLocation,photoTimeOfDay,photoDateTimeStamp);
                        trip.Photos.Add(readPhoto);

                    }else if(trackingType == "Cost"){
                        string costDescription = splitted[1];
                        double costPrice = double.Parse(splitted[2]);
                        string costLocation = splitted[3];
                        string costDateTimeStamp = splitted[4];
                        Cost readCost = new Cost(costDescription,costPrice,costLocation, costDateTimeStamp);
                        trip.Costs.Add(readCost);

                    }else if(trackingType == "Note"){
                        string noteName = splitted[1];
                        string noteDescription = splitted[2];
                        string noteSource = splitted[3];
                        string noteDateTimeStamp = splitted[4];
                        Note readNote = new Note(noteName,noteDescription,noteSource, noteDateTimeStamp);
                        trip.Notes.Add(readNote);
                    }
                }
            }
        }
    }
}