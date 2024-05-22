using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameManager gm ;
    [SerializeField] private Canvas startCanvas,retryCanvas;
    [SerializeField] public Button gameStart,retry,nextLevel;
    [SerializeField] private TextMeshProUGUI kickText,parText,startText,levelText,completeLevelText,kickParText,totalText;
    [SerializeField] private string sceneName = "Level 1";
    private string kicks = "Kicks: {0}/{1}";
    private string start = "Start";
    private string level = "Level:{0}";
    private string par = "Par:{0}";
    private string completeLevel = "Level {0} complete!";
    private string kickPar = "Kicks {0} / Par {1}";
    private string total = "Total: {0} over par";
    private int gmPar;
    private int gmLevel;
    private int gmKicks;

    void Start()
    {
        retryCanvas.enabled = false;
        gameStart.onClick.AddListener(gm.DoGame);
        gameStart.onClick.AddListener(StartOff);
        nextLevel.onClick.AddListener(LoadScene);
        retry.onClick.AddListener(gm.kicksReset);
        retry.onClick.AddListener(Reset);
        StartBox();
        KicksScore();
    }

    void Update()
    {
        gmLevel = gm.level;
        gmPar = gm.par;
        gmKicks = gm.kicks;
        KicksScore();
        StartBox();
    }

    void StartBox()
    {
        startText.text = start;
        parText.text = string.Format(par,gmPar);
        levelText.text = string.Format(level,gmLevel);
    }
    void KicksScore()
    {
        kickText.text = string.Format(kicks,gmKicks,gmPar);
    }
    
    public void RetryBox()
    {
        int sum = gmKicks - gmPar;
        completeLevelText.text = string.Format(completeLevel,gmLevel);
        kickParText.text = string.Format(kickPar,gmKicks,gmPar);
        if(sum >= 0)
        {
            totalText.text = string.Format(total,sum);
        }
        else
        {
            total = "Total: {0} under par";
            totalText.text = string.Format(total,sum);
        }
         retryCanvas.enabled = true;
        if(gm.level == 3)
        {
            nextLevel.interactable = false;
        }
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneName);
    }

    public void Reset()
    {
        retryCanvas.enabled = false;
    }

    void StartOff()
    {
        startCanvas.enabled = false;
    }

   
}
