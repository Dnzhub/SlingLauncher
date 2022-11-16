using UnityEngine;
using TMPro;
using System;

public class ShotCounter : MonoBehaviour
{
    public event Action OnDecreaseShot;
    [SerializeField] private TMP_Text shotCounterText;
    [SerializeField] private Transform characterContainer;

    private int currentShot;

    private void OnEnable()
    {
        GetComponent<ObjectLauncher>().OnFireLauncher += DecreaseCurrentShot;

    }
    private void OnDisable()
    {
        GetComponent<ObjectLauncher>().OnFireLauncher -= DecreaseCurrentShot;
    }
    private void Start()
    {
        GetTotalShot();
        SetCurrentShotText();
    }

    private void SetCurrentShotText()
    {
        if (HasShot())
        {
            shotCounterText.text = String.Format("{0:0} Shot Left", currentShot);
        }
        else
        {
            shotCounterText.text = "No More Shots";
        }

    }
    private bool HasShot()
    {
        if (currentShot > 0) return true;
        else { return false; }
      
    }
    private void DecreaseCurrentShot()
    {
        currentShot--;
        SetCurrentShotText();
        
        if (!HasShot())
        {
            OnDecreaseShot?.Invoke();
        }
     
    }
    private void GetTotalShot()
    {
        foreach (Transform child in characterContainer.transform)
        {
            currentShot++;
        }
    }

}
