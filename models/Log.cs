namespace StormyLand.Models;

public class Log{

    public int LogId {get; set;}
    public DateTime Timestamp {get; set;}
    public string Player {get; set;}
    public string Action {get; set;}
    public string Block {get; set;}
    public string Location {get; set;}
    public string Dimension {get; set;}
}