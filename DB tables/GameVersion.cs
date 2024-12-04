using UnityEngine;
using SQLite4Unity3d;
using System.Text.RegularExpressions;

public class GameVersion
{
    [PrimaryKey]
    public int ID {get; set; }
    public string _GameVersion {get; set; }

    public GameVersion(){

    }
    
    public GameVersion(int ID, string _GameVersion){
        setID(ID);
        setGameVersion(_GameVersion);
    }

    public void setID(int a)
    {
        ID = a;
    }

    public void setGameVersion(string a)
    {
        string pattern = @"^\d\.\d\.\d$";
        Debug.Assert(Regex.IsMatch(a, pattern));
        _GameVersion = a;
    }
}
