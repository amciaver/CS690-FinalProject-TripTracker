namespace TripTrackerProject.Test;

using TripTrackerProject;
using Xunit.Sdk;

public class ReporterTests{


    [Fact]
    public void Total_Trip_Cost_Test(){
        var manager = new Datamanager();
        var reporter = new Reporter();

        var testTrip = new Trip("Florida 2025");
        manager.Trips.Add(testTrip);

        testTrip.Costs.Add(new Cost("Water Bottle", 5.50, "Disney", "20260405"));
        testTrip.Costs.Add(new Cost("Ice Cream", 4.50, "Disney", "20260405"));

        
        var result = reporter.TotalTripCost(manager, "Florida 2025");
        Assert.Equal(10, result);

        string expectedFileName = "Florida 2025.txt";
        if (File.Exists(expectedFileName)){
            File.Delete(expectedFileName);
        }
    }
}
