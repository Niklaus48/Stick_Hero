
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public static Timer Instance;

    private void Awake()
    {
        Instance = this;
    }

     float MinTime;
     float MaxTime;
     public bool IsTimerActive;
    void Update()
    {
        //kjvhjgcjhgchj
        if (IsTimerActive)
        {
            gameObject.GetComponent<Image>().fillAmount = MinTime / MaxTime;
            MinTime += Time.deltaTime;
        }  
    }

    public void ResetTimer()
    {
        MaxTime = GroundLifeTime.instance.GetAge();
        MinTime = 0;
    }
}
