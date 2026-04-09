namespace TripTrackerProject.Test;

using Spectre.Console.Testing;
using TripTrackerProject;

public class ConsoleUITests{

    ConsoleUI testUI = new ConsoleUI();
    Datamanager testManager = new Datamanager();

    [Fact]
    public void Ask_For_Selection_Test(){
        var testManager = new Datamanager();
        var testConsole = new TestConsole();
        testConsole.Interactive();
        testManager.iAnsi = testConsole; 
        testConsole.Input.PushKey(ConsoleKey.Enter);
        var result = testManager.AskForSelection("Please Select a trip", new List<string> { "Florida 2025" });
        Assert.Equal("Florida 2025", result);
    }
}