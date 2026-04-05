namespace TripTrackerProject;

using Microsoft.VisualBasic;
using Spectre.Console;
using System.IO;

public class ConsoleUI {
    Datamanager datamanager;
    Reporter reporter;

    public ConsoleUI() {
        datamanager = new Datamanager();
        reporter = new Reporter();

    }

    public void Show() {

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

                        reporter.DisplayRecords(datamanager, selectedTrip);
                        
                    }else if(trackEntryCommand == "Total Trip Cost"){
                        
                        reporter.TotalTripCost(datamanager, selectedTrip);

                    }
                }while (trackEntryCommand!= "Return To Home Menu");
            }
        }while(selectedTrip != "Exit Application");
    }
}
