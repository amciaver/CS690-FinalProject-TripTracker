namespace TripTrackerProject;

using Microsoft.VisualBasic;
using Spectre.Console;
using System.IO;

class Program
{
    static void Main(string[] args)
    {   
        Console.WriteLine(Environment.NewLine + "Welcome to the TripTracker Application!");
        Datamanager datamanager = new Datamanager();

        List<string> tripSelectChoices = datamanager.Trips.ConvertAll(t => t.Name);
        tripSelectChoices.Add("Enter New Trip");
        tripSelectChoices.Add("Exit Application");
        
        string selectedTrip;
        do{    
            selectedTrip = Datamanager.AskForSelection("Please select a Trip", tripSelectChoices);
            Console.WriteLine("You have selected: " + selectedTrip);
            
            string tripName;
            if(selectedTrip == "Enter New Trip"){
                tripName = Datamanager.AskForInput("Enter new trip name: ");
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
                    trackEntryCommand = Datamanager.AskForSelection("Please select an action:", trackEntryCommandChoices);
                    Console.WriteLine(Environment.NewLine + "Selected Action = " + trackEntryCommand);
                    
                    if(trackEntryCommand == "Track Photo"){

                        Datamanager.TrackPhoto(datamanager, selectedTrip);

                    }else if(trackEntryCommand == "Track Cost"){

                        Datamanager.TrackCost(datamanager, selectedTrip);
                    
                    }else if(trackEntryCommand == "Track Note"){
                        
                        Datamanager.TrackNote(datamanager, selectedTrip);
    
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

    
}
