using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using System.Collections;
using System;

public class ObjectLauncher : MonoBehaviour
{
    #region References
    public event Action OnFireLauncher;
    [SerializeField] private AimProjection aimProjection;
    [SerializeField] private Transform launcherObject;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Transform CharacterContainer;
    [SerializeField] private float force = 15;
    [SerializeField] private float characterMass = 7;
    #endregion

    #region Attributes
    private List<CharacterHandler> characterToLaunch;
    private float launcherHorizontalLimit = 0.6f;
    private float LauncherVerticalLimit = 1f;
    private int currentIndex = 0;
    private Vector3 screenPosition;
    private Vector3 worldPosition;
    private Vector3 startPosition;
    private bool isLaunching = false;
    private bool canApplyForce = false;
    private bool canFire = true;
    private const string LAUNCHER_TAG = "Launcher";

    #endregion


    private void Start()
    {
        AddObjectsToList();     
        StartCoroutine(WaitForFirstCharacterToClimb());
        startPosition = launcherObject != null ? launcherObject.position : new Vector3(0, 4, 0);

    }
    private void Update()
    {

        if (!canFire) return;

        if (Input.GetMouseButton(0))
        {
         
            PrepareLauncher();

        }
        if (Input.GetMouseButtonUp(0))
        {
            
            FireLauncher();
            
            StartCoroutine(TryGetNextObjectToLaunch());
        }
    }
    private void FixedUpdate()
    {
        AddForceToObject();
    }
    
    private void PrepareLauncher()
    {
        if (isLaunching || characterToLaunch[currentIndex].IsDead()) return;


        aimProjection.ShowAimProjection(firePoint.position, firePoint.forward * force/ characterMass);
        screenPosition = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, 10))
        {
            if (!hitInfo.transform.CompareTag(LAUNCHER_TAG)) return;
          
            GetMouseWorldPosition(hitInfo.point);
            SetLauncherObjectPosition(new Vector3(worldPosition.x, launcherObject.transform.position.y, worldPosition.z), 0.1f);

            characterToLaunch[currentIndex].transform.DOMove(firePoint.position, 0.1f);
            characterToLaunch[currentIndex].SetRigidBodyKinematic(true);
            characterToLaunch[currentIndex].ApplyGravity(false);
        }
    }
    private void FireLauncher()
    {
        if (isLaunching) return;
        aimProjection.HideAimProjection();
        characterToLaunch[currentIndex].GetComponent<CharacterAnimations>().FloatOnAir();

        SetLauncherObjectPosition(startPosition, 0.8f);


        characterToLaunch[currentIndex].SetRigidBodyKinematic(false);
        characterToLaunch[currentIndex].ApplyGravity(true);
        isLaunching = true;
        canApplyForce = true;
        OnFireLauncher?.Invoke();
    }
    private void AddForceToObject()
    {
        if (canApplyForce)
        {
            float forceMultiplier = (-1 * launcherObject.transform.position.z);

            characterToLaunch[currentIndex].GetComponent<Rigidbody>().AddForce(characterToLaunch[currentIndex].transform.forward * (forceMultiplier * force),ForceMode.Impulse);
               
            canApplyForce = false;
        }
    }
    private void SetLauncherObjectPosition(Vector3 position, float duration)
    {
        launcherObject.DOMove(position, duration);
    }

    private IEnumerator TryGetNextObjectToLaunch()
    {

        if (currentIndex != characterToLaunch.Count - 1)
        {
            if (characterToLaunch[currentIndex].IsDead())
            {
                currentIndex++;
                characterToLaunch[currentIndex].GetComponent<CharacterAnimations>().ClimbToSling(firePoint);
                yield return new WaitForSecondsRealtime(characterToLaunch[currentIndex].GetComponent<CharacterAnimations>().GetAnimationLength());
                isLaunching = false;
            }
        }

    }
    private IEnumerator WaitForFirstCharacterToClimb()
    {
        canFire = false;
        characterToLaunch[currentIndex].GetComponent<CharacterAnimations>().ClimbToSling(firePoint);
        yield return new WaitForSecondsRealtime(characterToLaunch[currentIndex].GetComponent<CharacterAnimations>().GetAnimationLength());
        canFire = true;
    }

    private void GetMouseWorldPosition(Vector3 point)
    {
        worldPosition = point;
        worldPosition.x = Mathf.Clamp(worldPosition.x, -launcherHorizontalLimit, launcherHorizontalLimit);
        worldPosition.z = Mathf.Clamp(worldPosition.z, -LauncherVerticalLimit, 0);
    }

    private void AddObjectsToList()
    {
        characterToLaunch = new List<CharacterHandler>();

        foreach (Transform child in CharacterContainer.transform)
        {
            if (child.TryGetComponent<CharacterHandler>(out CharacterHandler characterHandler))
            {
                characterToLaunch.Add(characterHandler);
            }


        }
    }
    public void DisableController()
    {
        canFire = false;
    }

}
