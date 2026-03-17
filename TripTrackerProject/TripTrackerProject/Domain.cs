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

public class Trip {
    public string Name {get;}
    public List<Photo> Photos {get;}

    public Trip(string name){
        this.Name = name;
        this.Photos = new List<Photo>();
    }

    public override string ToString(){
        return this.Name;
    }
}