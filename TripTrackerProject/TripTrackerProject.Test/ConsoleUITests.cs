namespace TripTrackerProject.Test;

using Spectre.Console.Testing;
using TripTrackerProject;

public class ConsoleUITests{

    ConsoleUI testUI = new ConsoleUI();

    [Fact]
    public void Ask_For_Selection_Test(){
        
        var testConsole = new TestConsole();
        testConsole.Interactive();

        testUI.consoleAnsi = testConsole; 
        testConsole.Input.PushKey(ConsoleKey.Enter);

        var result = testUI.AskForSelection("Please Select a trip", new List<string> { "Florida 2025" });
        Assert.Equal("Florida 2025", result);
    }
}