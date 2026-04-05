namespace TripTrackerProject;

using Microsoft.VisualBasic;
using Spectre.Console;
using System.IO;

class Program
{
    static void Main(string[] args){   
        Console.WriteLine(Environment.NewLine + "Welcome to the TripTracker Application!");
        
        ConsoleUI theUI = new ConsoleUI();
        theUI.Show();    
    }    
}
