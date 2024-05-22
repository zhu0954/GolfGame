using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private OriginalPosition player,ball;
    [SerializeField] private Movement movement;
    [SerializeField] private Goal goal;

    private int Par ;
    public int par
    {
        get
        {
            return Par;
        } 
    }

    private int Kicks ;
    public int kicks
    {
        get
        {
            return Kicks;
        } 
    }

    private int Level;
    public int level
    {
        get
        {
            return Level;
        } 
    }

    void Start()
    {
        movement.canMove = false;
    }

    void Update()
    {
        Scene currentScene = SceneManager.GetActiveScene ();
        int buildIndex = currentScene.buildIndex;
        if( buildIndex == 0)
        {
            Level = 1;
            Par = 3;
        }
        else if( buildIndex == 1)
        {
            Level = 2;
            Par = 5;
        }
        else if(buildIndex == 2)
        {
            Level = 3;
            Par = 5;
        }
    }

    public void kicksAdd()
    {
        Kicks++;
    }

    public void kicksReset()
    {
        player.ObjectReset();
        ball.ObjectReset();
        goal.scored = false;
        Kicks = 0;
    }

    public void DoGame()
    {
        movement.canMove = true;
    }
}
