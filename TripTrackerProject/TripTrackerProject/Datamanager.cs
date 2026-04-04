namespace TripTrackerProject;

using Microsoft.VisualBasic;
using Spectre.Console;
using System.IO;

public class Datamanager{

    public List <Trip> Trips {get;}

    public Datamanager(){
        Trips = new List<Trip>();

        if(File.Exists("trips.txt")){
            var tripsFileContent = File.ReadAllLines("trips.txt");
            foreach(var tripName in tripsFileContent){
                Trips.Add(new Trip(tripName));
            }
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

    public void SyncTrips(List<Trip>Trips){
        File.Delete("trips.txt");
        foreach(Trip enteredTrip in Trips)
        {
            File.AppendAllText("trips.txt", enteredTrip + Environment.NewLine);
        }
        Console.WriteLine("Trips have been synchronized to the text file");
    }

    public void SyncTripData(string targetTrip, List<Trip>Trips){
        foreach (Trip trip in Trips){
            if (trip.Name == targetTrip){
                string fileName = trip.Name + ".txt";
                File.Delete(fileName);
                foreach (Photo photo in trip.Photos){
                    File.AppendAllText(fileName,"Photo," + photo.Name + "," + photo.Location + "," + photo.TimeOfDay + "," + photo.DateTimeStamp + Environment.NewLine);
                }
                Console.WriteLine("Photos Synced for: " + trip.Name);

                foreach (Cost cost in trip.Costs){
                    File.AppendAllText(fileName,"Cost," + cost.Description + "," + cost.Price + "," + cost.Location + "," + cost.DateTimeStamp + Environment.NewLine);
                }
                Console.WriteLine("Costs Synced for: " + trip.Name);
                
                foreach (Note note in trip.Notes){
                    File.AppendAllText(fileName,"Note," + note.Name + "," + note.Description + "," + note.Source + "," + note.DateTimeStamp + Environment.NewLine);
                }
                Console.WriteLine("Notes Synced for: " + trip.Name);
            }
        }
    }
}