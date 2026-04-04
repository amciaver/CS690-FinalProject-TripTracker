namespace TripTrackerProject;

using Spectre.Console;

public class Reporter{

    private Datamanager datamanager = new Datamanager();
    
    public void DisplayRecords(Datamanager datamanager, string selectedTrip){
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
    }

    public void TotalTripCost(Datamanager datamanager, string selectedTrip){
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
}