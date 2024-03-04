using System.Collections;
using Microsoft.AspNetCore.Mvc;
using StormyLand.Models;

namespace StormyLand.Controllers;

[ApiController]
[Route("[controller]")]
public class LogsController : ControllerBase{
    private LogContext _logContext;
    string _key = "TO0rGSbRPTJAhw3cM65G9L8sTZXuNwaR";
    public LogsController(LogContext db){
        _logContext = db;
    }

    [HttpPost]
    public async Task AddLogsAsync(Log[] logs, string key){
        if(_key == key){
            for(int i = 0; i < logs.Length; i++){
            _logContext.Add(logs[i]);
        }
            await _logContext.SaveChangesAsync();
        }
    }

    [HttpDelete]
    public async Task<ActionResult> Wipe(string key){
        if(_key == key){
            DateTime currentDate = DateTime.Now;
            Log[] oldLogs = [.. _logContext.Logs.Where(l => l.Timestamp.AddDays(-30) > currentDate)];
            for(int i = 0; i <oldLogs.Length; i++){
                _logContext.Remove(oldLogs[i]);
            }
            await _logContext.SaveChangesAsync();
            }
            return NoContent();
        }
        

    [HttpGet("~/GetPlayers")]
    public IEnumerable<string> GetPlayers(){
        List<string> players = [];
        List<Log> logs = [.. _logContext.Logs];
        for(int i = 0; i < logs.Count; i++){
            if(!players.Contains(logs[i].Player))
            players.Add(logs[i].Player);
        }
        return players;
    }

    [HttpGet("~/GetBlocks")]
    public IEnumerable<string> GetBlocks(){
        List<string> blocks = [];
        List<Log> logs = [.. _logContext.Logs];
        for(int i = 0; i < logs.Count; i++){
            if(!blocks.Contains(logs[i].Block))
            blocks.Add(logs[i].Block);
        }
        return blocks;
    }

    [HttpGet("~/GetActions")]
    public IEnumerable<string> GetAction(){
        List<string> actions = [];
        List<Log> logs = [.. _logContext.Logs];
        for(int i = 0; i < logs.Count; i++){
            if(!actions.Contains(logs[i].Action))
            actions.Add(logs[i].Action);
        }
        return actions;
    }

    [HttpGet("~/GetDimensions")]
    public IEnumerable<string> GetDimension(){
        List<string> dimensions = [];
        List<Log> logs = [.. _logContext.Logs];
        for(int i = 0; i < logs.Count; i++){
            if(!dimensions.Contains(logs[i].Dimension))
            dimensions.Add(logs[i].Dimension);
        }
        return dimensions;
    }

    [HttpGet("~/Search")]
    public IEnumerable<Log> Search([FromQuery]string[] Player, [FromQuery]string[] Block, [FromQuery]string[] Action, string Location, [FromQuery]string[] Dimension){
        
        string playerString = string.Join("", Player);
        string blockString = string.Join("", Block);
        string actionString = string.Join("", Action);
        string DimensionString = string.Join("", Dimension);
        
        
        if(Player.Length == 1 && Player.Contains("0")){
            List<string> players = [];
            List<Log> slogs = [.. _logContext.Logs];
            for(int i = 0; i < slogs.Count; i++){
                if(!players.Contains(slogs[i].Player)){
                players.Add(slogs[i].Player);
                Console.WriteLine("Found Player: " + slogs[i].Player);
                }
            }
            playerString = string.Join("", players);

        }
        if(Block.Length == 1 && Block.Contains("0")){
            List<string> blocks = [];
            List<Log> slogs = [.. _logContext.Logs];
            for(int i = 0; i < slogs.Count; i++){
                if(!blocks.Contains(slogs[i].Block)){
                blocks.Add(slogs[i].Block);
                Console.WriteLine("Found Block: " + slogs[i].Block);
                }
            }
            blockString = string.Join("", blocks);
            
        }
        if(Action.Length == 1 && Action.Contains("0")){
            List<string> actions = [];
            List<Log> slogs = [.. _logContext.Logs];
            for(int i = 0; i < slogs.Count; i++){
                if(!actions.Contains(slogs[i].Action)){
                actions.Add(slogs[i].Action);
                Console.WriteLine("Found Action: " + slogs[i].Action);
                }
            }
            actionString = string.Join("", actions);
        }
        if(Dimension.Length == 1 && Dimension.Contains("0")){
            List<string> dimensions = [];
            List<Log> slogs = [.. _logContext.Logs];
            for(int i = 0; i < slogs.Count; i++){
                if(!dimensions.Contains(slogs[i].Dimension)){
                dimensions.Add(slogs[i].Dimension);
                Console.WriteLine("Found Dimension: " + slogs[i].Dimension);
                }
            }
            DimensionString = string.Join("", dimensions);
            
        }
        if(Location == "0"){
            IEnumerable<Log> logs = _logContext.Logs
            .Where(p => playerString.Contains(p.Player))
            .Where(b => blockString.Contains(b.Block))
            .Where(a => actionString.Contains(a.Action))
            .Where(d => DimensionString.Contains(d.Dimension))
            .OrderByDescending(l => l.Timestamp);
            Console.WriteLine(logs.Count() + " Results Found");
            if(logs.Count() > 100){
                Console.WriteLine("Only returning first 100 results");
                List<Log> modified = [];
                Log[] logsArray = logs.ToArray();
                for(int i = 0; i < 100; i++){
                    modified.Add(logsArray[i]);
                }
                return modified;
            }
            return logs;
        }
        else{
            IEnumerable<Log> logs = _logContext.Logs
            .Where(p => playerString.Contains(p.Player))
            .Where(b => blockString.Contains(b.Block))
            .Where(a => actionString.Contains(a.Action))
            .Where(d => DimensionString.Contains(d.Dimension))
            .Where(l => l.Location == Location)
            .OrderByDescending(l => l.Timestamp);
            Console.WriteLine(logs.Count() + " Results Found");
            if(logs.Count() > 100){
                Console.WriteLine("Only returning first 100 results");
                List<Log> modified = [];
                Log[] logsArray = logs.ToArray();
                for(int i = 0; i < 100; i++){
                    modified.Add(logsArray[i]);
                }
                return modified;
            }
            return logs;
        }
    }
}