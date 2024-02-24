
using System.Collections;
using UnityEngine;

public class GroundLifeTime : MonoBehaviour
{
    public static GroundLifeTime instance;

    [SerializeField] float _Age;

    private Coroutine _AgeCoroutine;

    private void Awake()
    {
        instance = this;
    }


    public void DestroyGround()
    {
        Destroy(MapManager.Instance.grounds[0]);
        GameManager.instance.GameOver();
    }

    public void SetAgeForGround()
    {
        _AgeCoroutine = StartCoroutine(GroundsAge());
    }
    public void Stop()
    {
        if (_AgeCoroutine != null)
            StopCoroutine(_AgeCoroutine);
    }

    public void DecreaseAge(float decrease)
    {
        _Age -= decrease;
    }

    public float GetAge()
    {
        return _Age;
    }

    IEnumerator GroundsAge()
    {
        while (true)
        {
            yield return new WaitForSeconds(_Age);
            DestroyGround();
        }
    }
}
