using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearAfterXseconds : MonoBehaviour
{
    private void OnEnable()
    {
        Invoke("DisableSelf", 1.5f);
    }

    private void DisableSelf()
    {
        gameObject.SetActive(false);
    }
}
