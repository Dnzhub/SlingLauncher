using System;
using TMPro;
using UnityEngine;

public class LevelStatistics : MonoBehaviour
{
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject statisticsPanel;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text completeText;
    [SerializeField] private TMP_Text percentageText;
  
    public void SetStatisticsInfo(int currentLevel,bool canCompleted,float percentage)
    {
        ActivateStatisticsPanel();
        levelText.text = "level " + currentLevel;
        percentageText.text = String.Format("%{0:0}", percentage);
        if (canCompleted)
        {
            completeText.text = "Complated";
            completeText.color = new Color(0.8018868f, 0.5514888f, 0f, 1f);
        }
        else
        {
            completeText.text = "Wasted";
            completeText.color = new Color(0.8f, 0.01549293f, 0f, 1f);
        }
        
    }

    private void ActivateStatisticsPanel()
    {
        gamePanel.SetActive(false);
        statisticsPanel.SetActive(true);
    }
}
