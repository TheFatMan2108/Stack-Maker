using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] float speed = 10f;
    float x, z;
  
    void Start()
    {
       x= transform.position.x; z= transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 target = new Vector3(player.transform.position.x+x,transform.position.y,player.transform.position.z+z);
        transform.position = Vector3.Lerp(transform.position,target,speed*Time.deltaTime);
    }
}
