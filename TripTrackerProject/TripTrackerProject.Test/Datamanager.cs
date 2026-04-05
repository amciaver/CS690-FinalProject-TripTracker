namespace TripTrackerProject.Test;

using TripTrackerProject;
using Xunit.Sdk;

public class Datamanagertests{


    [Fact]
    public void Read_Saved_Trips_Test()
    {
        var testTrip = new Trip("Florida 2025");
        var tripsList = new List<Trip> { testTrip };

        var fileSaver = new FileSaver();
        
        string expectedFileName = "trips.txt"; 
        fileSaver.SyncTrips(tripsList); 
       
        var datamanager = new Datamanager();
        datamanager.ReadSavedTrips(); 

        Assert.True(File.Exists(expectedFileName), "The trips.txt file was not found.");
        string fileContent = File.ReadAllText(expectedFileName);

        Assert.Contains("Florida 2025", fileContent);
        Assert.Contains(datamanager.Trips, t => t.Name == "Florida 2025");

        if (File.Exists(expectedFileName)){
            File.Delete(expectedFileName);
        }
    }
}