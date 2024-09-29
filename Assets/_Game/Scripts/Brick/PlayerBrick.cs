using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBrick : MonoBehaviour
{
    public void GetBrick()
    {
        if (gameObject.activeInHierarchy)
            gameObject.SetActive(false);
    }
}
