
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;

    [Header("Positions:")]
    [SerializeField] float Space;
    [SerializeField] float MinXPos, MaxXPos;
    [SerializeField] float YPos;

    [Header("Camera Setting:")]
    [SerializeField] float CameraSpeed;

    [Header("ScaleSetting:")]
    [SerializeField] float MinXScale;
    [SerializeField] float MaxXScale;
    [SerializeField] float YScale;

    float DistanceBetwenGrounds;
    float CameraCorrectLocation;
    bool AlowCameraToMove;
    bool IsGameOver;

    [Header(" ")]
    public GameObject Ground, StartGround;
    public GameObject Cam;

    [Header("Dont Care About This One")]
    public List<GameObject> grounds = new List<GameObject>();


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        grounds.Add(StartGround);
        CreateGround(Random.Range(MinXScale, MaxXScale),YScale);
    }


    private void Update()
    {
        if (!IsGameOver)
        {
            if (AlowCameraToMove)
            {
                Cam.transform.Translate(CameraSpeed * Time.deltaTime, 0, 0);

                if (Cam.transform.position.x >= CameraCorrectLocation)
                {
                    AlowCameraToMove = false;
                    GameManager.instance.SetBridgeToNull();
                    GroundLifeTime.instance.SetAgeForGround();
                    Timer.Instance.ResetTimer();
                    Timer.Instance.IsTimerActive = true;
                }
            }
        } 
    }


    public void CreateGround(float XScale , float YScale)
    {
        DistanceBetwenGrounds = Random.Range(MinXPos, MaxXPos);
        float CurentGroundPos = grounds[0].transform.position.x;
        float CurentGroundScale = grounds[0].transform.localScale.x / 2;
        float NextGroundXPos = CurentGroundPos + CurentGroundScale + Space + DistanceBetwenGrounds + XScale/2;

        GameObject newGround = Instantiate(Ground, new Vector2(NextGroundXPos, YPos), Quaternion.identity);
        newGround.transform.localScale = new Vector2(XScale, YScale);
        grounds.Add(newGround);
    }

    public void DeleteGround()
    {
        CameraCorrectLocation += grounds[1].transform.position.x - grounds[0].transform.position.x;
        Destroy(grounds[0],1);
        grounds.RemoveAt(0);
    }

    public void MoveCamera()
    {
        DeleteGround();
        StartCoroutine(DelayCreatingGround());
        AlowCameraToMove = true;
    }

    public void IncreaseCamSpeed(float increase)
    {
        CameraSpeed += increase;
    }

    public void SetGameOver()
    {
        IsGameOver = true;
    }

    IEnumerator DelayCreatingGround()
    {
        CreateGround(Random.Range(MinXScale, MaxXScale), YScale);
        yield return new WaitForSeconds(1);
        
    }

}
