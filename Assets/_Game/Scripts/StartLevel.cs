using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLevel : MonoBehaviour,IWinable
{
    public Transform startPoint;
   
    void Start()
    {
        OnEnable();
        GameManager.instance.StartWin(this);
    }
    private void OnEnable()
    {
        if (Player.Instance != null)
        Player.Instance.GoToStartPoint(startPoint.position);
    }

    public void OnStop()
    {
       gameObject.SetActive(false);
       Destroy(gameObject);
      
    }
    private void OnDestroy()
    {
        GameManager.instance.EndWin(this);
    }
    public void OnWin()
    {
       
    }

}
