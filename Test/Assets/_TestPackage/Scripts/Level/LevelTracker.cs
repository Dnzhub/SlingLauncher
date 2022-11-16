using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTracker : MonoBehaviour
{
    #region References
    public event Action<float> OnGetPercentage;    
    private ShotCounter shotCounter;
    private ObjectLauncher objectLauncher;
    [SerializeField] private Transform obstaclesContainer;
    [SerializeField] private LevelStatistics statistics;
    #endregion

    #region Attributes
    private float totalObstacle;
    private float maxPercentage = 100f;
    private float currentObstacle = 0;
    private float levelPassLimitPercentage = 0.90f;
    private float statisticsShowTimer = 5f;
    private int currentLevel = 1;
    #endregion
    private void Awake()
    {
        shotCounter = GetComponentInParent<ShotCounter>();
        objectLauncher = GetComponentInParent<ObjectLauncher>();
    }
    void Start()
    {
        totalObstacle = CountObstacleNumber();
    }
    private void OnEnable()
    {
        shotCounter.OnDecreaseShot += NotAvailableShot;
    }
    private void OnDisable()
    {
        shotCounter.OnDecreaseShot -= NotAvailableShot;
    }
    private void Update()
    {
        CompleteLevel();
    }
    private void NotAvailableShot()
    {
        StartCoroutine(ShowLevelStatistics());

    }
    private void CompleteLevel()
    {
        if (CanCompleteLevel())
        {
            StartCoroutine(ShowLevelStatistics());
        }
    }
    private bool CanCompleteLevel()
    {
        return GetPercentage() > maxPercentage * levelPassLimitPercentage;
    }
    private IEnumerator ShowLevelStatistics()
    {
        objectLauncher.DisableController();
        yield return new WaitForSeconds(statisticsShowTimer);
        statistics.SetStatisticsInfo(currentLevel, CanCompleteLevel(), GetPercentage());
    }
    public int GetCurrentLevel()
    {
        return currentLevel;
    }
    public float GetPercentage()
    {
        return Mathf.Min(((currentObstacle / totalObstacle) * 100), maxPercentage);
    }

    private float CountObstacleNumber()
    {
        return obstaclesContainer.childCount - 1;

    }
    public float GetMaxPercentage()
    {
        return maxPercentage;
    }
    public void RestartLevel()
    {
        DOTween.KillAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            currentObstacle++;
            OnGetPercentage?.Invoke(GetPercentage());
            Destroy(other.gameObject);
        }
    }

}
