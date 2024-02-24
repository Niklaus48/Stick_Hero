
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] float BridgeSpeed;
    [SerializeField] float PlayerSpeed;
    [SerializeField] float PlayerFallSpeed;
    float BridgeDropSpeed = 1;
    public GameObject BridgePrifab;
    public GameObject Player;

    bool MakeBridgeBigger;
    bool AllowPlayerToMove;
    bool IsBridgeDropCorrectly;
    bool IsGameOver;
    bool CanClick;

    GameObject Bridge = null;
    Animator BridgeAnimator;

    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (!IsGameOver)
        {
            if (Input.GetKeyDown(KeyCode.F) && Bridge == null)
            {
                Vector2 BridgePos = MapManager.Instance.grounds[0].transform.GetChild(0).transform.position;
                Bridge = Instantiate(BridgePrifab, BridgePos, Quaternion.identity);
                MakeBridgeBigger = true;
                CanClick = true;
            }
            if (Input.GetKeyUp(KeyCode.F) && CanClick)
            {
                DropBridge();
                SetIsBridgeDropCorrectly();
                StartCoroutine(PlayerMoveDelay());
                CanClick = false;
            }

            if (MakeBridgeBigger)
                CreateBridge();

            if (AllowPlayerToMove && IsBridgeDropCorrectly)
            {
                MovePlayerToTheCorrectPosition();
            }
            else if (AllowPlayerToMove && !IsBridgeDropCorrectly)
            {
                MovePlayerAlongTheBridge();
            }
        }
        else
        {
            Player.transform.Translate(0, -PlayerFallSpeed * Time.deltaTime, 0);
            Destroy(Player, 1);
        }
    }

    void CreateBridge()
    {
        float Yscale = Bridge.transform.localScale.y;
        Bridge.transform.localScale = new Vector2(Bridge.transform.localScale.x, Yscale + BridgeSpeed * Time.deltaTime);
    }

    void MovePlayerToTheCorrectPosition()
    {
        Player.transform.Translate(PlayerSpeed * Time.deltaTime, 0, 0);

        float DestinationGroundXPos = MapManager.Instance.grounds[1].transform.position.x;
        float DestinationGroundXScale = MapManager.Instance.grounds[1].transform.localScale.x;

        bool IsPlayerAtTheCorrectSpot = Player.transform.position.x > DestinationGroundXPos + (DestinationGroundXScale * 0.3f);

        if (IsPlayerAtTheCorrectSpot)
        {
            AllowPlayerToMove = false;
            MapManager.Instance.MoveCamera();
            UIManager.Instance.AddScore(1);
        }
    }

    void MovePlayerAlongTheBridge()
    {
        float EndOfBridge = Bridge.transform.position.x + Bridge.transform.localScale.y / 5;
        Player.transform.Translate(PlayerSpeed * Time.deltaTime, 0, 0);
        if (Player.transform.position.x >= EndOfBridge)
        {
            AllowPlayerToMove = false;
            Bridge.GetComponent<Animator>().SetBool("CanDrop", false);
            Bridge.GetComponent<Animator>().SetBool("IsDestroying", true);
            GameOver();
        }
    }

    void DropBridge()
    {
        BridgeAnimator = Bridge.GetComponent<Animator>();
        MakeBridgeBigger = false;
        BridgeAnimator.speed = BridgeDropSpeed;
        BridgeAnimator.SetBool("CanDrop", true);
    }

    public void SetBridgeToNull()
    {
        Destroy(Bridge);
        Bridge = null;
    }

    public void GameOver()
    {
        IsGameOver = true;
        MapManager.Instance.SetGameOver();
        UIManager.Instance.ActiveGameOverMenu();
        Timer.Instance.IsTimerActive = false;
    }

    public void SetIsBridgeDropCorrectly()
    {
        float EndOfBridge = Bridge.transform.position.x + Bridge.transform.localScale.y / 5;

        float DestinationGroundXPos = MapManager.Instance.grounds[1].transform.position.x;
        float DestinationGroundXScale = MapManager.Instance.grounds[1].transform.localScale.x;

        float MaxAllowedBridgeScale = DestinationGroundXPos + DestinationGroundXScale / 2;

        float MinAllowedBridgeScale = DestinationGroundXPos - DestinationGroundXScale / 2;

        if (EndOfBridge > MaxAllowedBridgeScale || EndOfBridge < MinAllowedBridgeScale)
        {
            IsBridgeDropCorrectly = false;
        }
        else
        {
            IsBridgeDropCorrectly = true;
        }
    }

    public void MakeGameHarder()
    {
        BridgeSpeed += 5;
        PlayerSpeed += 0.5f;
        BridgeDropSpeed += 0.1f;
        GroundLifeTime.instance.DecreaseAge(0.2f);
        MapManager.Instance.IncreaseCamSpeed(0.5f);
    }

    IEnumerator PlayerMoveDelay()
    {
        yield return new WaitUntil(() => BridgeAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1);
        if (IsBridgeDropCorrectly)
            Bridge.transform.GetChild(1).GetComponent<ParticleSystem>().Play();//Enable Smoke Particle System
        GroundLifeTime.instance.Stop();
        Timer.Instance.IsTimerActive = false;
        AllowPlayerToMove = true;
    }
}
