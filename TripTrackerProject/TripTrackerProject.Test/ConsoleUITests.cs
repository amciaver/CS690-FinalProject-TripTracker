namespace TripTrackerProject.Test;

using Spectre.Console.Testing;
using TripTrackerProject;

public class ConsoleUITests{

    ConsoleUI testUI = new ConsoleUI();
    Datamanager datamanager = new Datamanager();

    [Fact]
    public void UI_Show_Test(){
        
        string tripsFileName = "trips.txt";
        if (File.Exists(tripsFileName)){
            File.Delete(tripsFileName);
        }
        
        string tripDataFileName = "Florida 2025.txt";
        if (File.Exists(tripDataFileName))
        {
            File.Delete(tripDataFileName);
        }
        testUI.Show();

        var console = new TestConsole();
        var trips = new List<string> {"Florida 2025"};

        console.Input.PushKey(ConsoleKey.Enter);

        
        var result = datamanager.AskForSelection(console, trips);

        Assert.Equal("Florida 2026", result);
    }
}