namespace TripTrackerProject;

public class Photo {
    public string Name {get;}
    public string Location {get;}
    public string TimeOfDay {get;}

    public Photo(string name, string location, string timeOfDay){
        this.Name = name;
        this.Location = location;
        this.TimeOfDay = timeOfDay;
    }

    public override string ToString()
    {
        return this.Name;
    }
}

public class Cost {
    public string Description {get;}
    public double Price {get;}
    public string Location {get;}

    public Cost(string description, double price, string location){
        this.Description = description;
        this.Price = price;
        this.Location = location;
    }

    public override string ToString()
    {
        return this.Description;
    }
}


public class Trip {
    public string Name {get;}
    public List<Photo> Photos {get;}
    public List<Cost> Costs {get;}

    public Trip(string name){
        this.Name = name;
        this.Photos = new List<Photo>();
        this.Costs = new List<Cost>();
    }

    public override string ToString(){
        return this.Name;
    }
}