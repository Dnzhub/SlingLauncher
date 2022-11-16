using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class LevelDisplay : MonoBehaviour
{
    [SerializeField] private LevelTracker levelTracker;
    [SerializeField] private TMP_Text levelPercentageText;
    [SerializeField] private TMP_Text currentLevelText;
    [SerializeField] private TMP_Text nextLevelText;
    [SerializeField] private Slider levelSlider;

    private int currentLevel;



    private void OnEnable()
    {
        levelTracker.OnGetPercentage += SetLevelPercentageText;
        levelTracker.OnGetPercentage += SetSliderValue;
    }

    private void OnDisable()
    {
        levelTracker.OnGetPercentage -= SetLevelPercentageText;
        levelTracker.OnGetPercentage -= SetSliderValue;
    }
    private void Start()
    {
        SetSliderMaxPercentage();
        currentLevel = levelTracker.GetCurrentLevel();
        SetLevelText();
    }
    private void SetSliderMaxPercentage()
    {
        levelSlider.maxValue = levelTracker.GetMaxPercentage();
        levelSlider.value = 0;
    }

    public void SetSliderValue(float percentage)
    {
        levelSlider.value = percentage;
    }
    private void SetLevelPercentageText(float percentage)
    {
        levelPercentageText.text = String.Format("%{0:0}", percentage);
    }
    private void SetLevelText()
    {
        currentLevelText.text = currentLevel.ToString();
        nextLevelText.text = (currentLevel + 1).ToString();
    }

}
