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

    public void DeleteTrackedEntry(string selectedTrip){
        foreach(Trip trip in Trips){
            if(selectedTrip == trip.Name){
                
                Dictionary<int,string> entries = new Dictionary<int,string>();
                int entryNumber = 0;
                int photoCount = -1;
                int costCount = -1;
                int noteCount = -1;

                foreach(Photo photo in trip.Photos){
                    entryNumber += 1;
                    photoCount +=1;
                    entries.Add(entryNumber,$"photo,{photoCount}");
                    Console.WriteLine($"Entry {entryNumber}) {photo.Name},{photo.Location},{photo.TimeOfDay},{photo.DateTimeStamp}");
                }

                foreach(Cost cost in trip.Costs){
                    entryNumber += 1;
                    costCount += 1;               
                    entries.Add(entryNumber,$"cost,{costCount}");
                    Console.WriteLine($"Entry {entryNumber}) {cost.Description},{cost.Price},{cost.Location},{cost.DateTimeStamp}");
                }   

                foreach(Note note in trip.Notes){
                    entryNumber += 1;
                    noteCount += 1;
                    entries.Add(entryNumber,$"note,{noteCount}");
                    Console.WriteLine($"Entry {entryNumber}) {note.Name},{note.Description},{note.Source},{note.DateTimeStamp}");
                }

                Console.WriteLine("Please enter the entry number to delete: ");
                int input = int.Parse(Console.ReadLine()); //make separate function to check int later
                string deleteEntry = entries[input];
                var splitDeleteEntry = deleteEntry.Split(",");
                string itemType = splitDeleteEntry[0];
                int itemIndex = int.Parse(splitDeleteEntry[1]);
                
                if(itemType == "photo"){
                    trip.Photos.RemoveAt(itemIndex);
                }else if(itemType == "cost"){
                    trip.Costs.RemoveAt(itemIndex);
                }else if (itemType == "note"){
                    trip.Notes.RemoveAt(itemIndex);
                }
                fileSaver.SyncTripData(selectedTrip, Trips);
            }
        }
    }
}