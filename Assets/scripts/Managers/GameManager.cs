using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public CharacterStats playerStats;
    List<IEndGameOberver> endGameObervers = new List<IEndGameOberver>();

    public void RigisterPlayer(CharacterStats player)
    {
        playerStats = player;
    }

    public void AddObserver(IEndGameOberver observer)
    {
        endGameObervers.Add(observer);
    }

    public void RemoveObserver(IEndGameOberver observer)
    {
        endGameObervers.Remove(observer);
    }

    public void NotifyObervers()
    {
        foreach (var observer in endGameObervers)
        {
            observer.EndNotify();
        }
    }
}
