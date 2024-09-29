using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerWinner : MonoBehaviour, IBrickable, IWinable
{
    [SerializeField] GameObject openChest, closeChest,fx1,fx2;
    BoxCollider boxCollider;
    Player player;


    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
    }
    private void Start()
    {
        GameManager.instance.StartBrick(this);
        GameManager.instance.StartWin(this);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            if (player!=null)
            {
                this.player = player;
                GameManager.instance.OnWin();   
            }
            
        }
    }
    public void AddBrick()
    {
        boxCollider.size = boxCollider.size + new Vector3(0, 0.5f, 0);
    }

    public void ClearBrick()
    {
        
    }

    public void RemoveBrick()
    {
       
    }

    public void OnWin()
    {

        openChest.SetActive(true);
        closeChest.SetActive(false);
        fx1.SetActive(true);
        fx2.SetActive(true);
    }

    public void OnStop()
    {
        openChest.SetActive(false);
        closeChest.SetActive(true);
        fx1.SetActive(false);
        fx2.SetActive(false);
    }
    private void OnDestroy()
    {
        GameManager.instance.EndBrick(this);
        GameManager.instance.EndWin(this);
    }
}
