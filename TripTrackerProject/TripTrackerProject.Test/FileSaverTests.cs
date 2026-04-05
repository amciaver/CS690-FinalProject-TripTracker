namespace TripTrackerProject.Test;

using TripTrackerProject;
using Xunit.Sdk;

public class FileSaverTests{


    [Fact]
    public void Sync_Trip_Data_Test(){

        var testTrip = new Trip("Florida 2025");
        testTrip.Notes.Add(new Note("Disney", "Disney is big", "Google Maps", "20260404")); 
        
        var tripsList = new List<Trip> {testTrip};
        var fileSaver = new FileSaver();
        string expectedFileName = "Florida 2025.txt";
        fileSaver.SyncTripData("Florida 2025", tripsList);
        string fileContent = File.ReadAllText(expectedFileName);

        Assert.True(File.Exists(expectedFileName));
        Assert.Contains("Note,Disney,Disney is big,Google Maps,20260404", fileContent);

        if (File.Exists(expectedFileName)){
            File.Delete(expectedFileName);
        }
    }
}
