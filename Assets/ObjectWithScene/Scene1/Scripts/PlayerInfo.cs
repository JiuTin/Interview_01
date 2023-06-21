using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo 
{
    public int level=19;
    public int money=1000;
    public int value=0;
    public static PlayerInfo _instance;
    public static PlayerInfo Instance
    {
        get {
            if (_instance == null)
            {
                _instance = new PlayerInfo();
            }
            return _instance;
        }

    }
}
