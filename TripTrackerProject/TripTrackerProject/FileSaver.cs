namespace TripTrackerProject;

using Microsoft.VisualBasic;
using Spectre.Console;
using System.IO;

public class FileSaver {

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
                

                foreach (Cost cost in trip.Costs){
                    File.AppendAllText(fileName,"Cost," + cost.Description + "," + cost.Price + "," + cost.Location + "," + cost.DateTimeStamp + Environment.NewLine);
                }
                
                
                foreach (Note note in trip.Notes){
                    File.AppendAllText(fileName,"Note," + note.Name + "," + note.Description + "," + note.Source + "," + note.DateTimeStamp + Environment.NewLine);
                }
            }
            Console.WriteLine("Data Synced for: " + trip.Name);
        }
    }

}