using UnityEngine;
using System.Collections;

public class DelayDestory : MonoBehaviour
{

    private float delayTime = 1.0f;

    public void Update()
    {
        this.delayTime -= Time.deltaTime;
        if (this.delayTime <= 0)
        {
            GameObject.Destroy(this.gameObject);
        }
    }

    
}
