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
        return iAnsi.Prompt(
            new SelectionPrompt<string>()
            .Title(message)
            .AddChoices(choices));
    }
    public void TrackPhoto(Datamanager datamanager, string selectedTrip){
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
                fileSaver.SyncTripData(selectedTrip,datamanager.Trips);
            }
        }
    }

    public void TrackCost(Datamanager datamanager, string selectedTrip){
        
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
                fileSaver.SyncTripData(selectedTrip,datamanager.Trips);
            }
        }
    }

    public void TrackNote(Datamanager datamanager, string selectedTrip){
        
        string noteName = AskForTrackItemString("Please enter the name of the note: ");
        string noteDescription = AskForTrackItemString(Environment.NewLine + "Please enter a description");
        string noteSource = AskForTrackItemString(Environment.NewLine + "Please enter the source of the information: ");
        string noteDateTimeStamp = DateTime.Now.ToString();
        Note newNote = new Note(noteName, noteDescription, noteSource, noteDateTimeStamp);
        foreach(Trip trip in datamanager.Trips){
            if(selectedTrip == trip.Name){
                trip.Notes.Add(newNote);
                string tripFileName = selectedTrip + ".txt";
                fileSaver.SyncTripData(selectedTrip,datamanager.Trips);
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