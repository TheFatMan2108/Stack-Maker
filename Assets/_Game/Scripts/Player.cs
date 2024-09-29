
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour,IBrickable,IWinable
{
    public static Player Instance { get; private set; }
    [SerializeField] LayerMask brickLayer;
    [SerializeField] float distanceCheck = 5,speed= 10f;
    [SerializeField] Transform pointCheck;
    [SerializeField] GameObject brick;
    [SerializeField] Animator animator;
    float brickHeight = 0.2f;
    ControllerPlayer controller;
    List<GameObject> listBrick = new List<GameObject>();
    InputAction swipe, mousePosition;
    Vector3 startPoint, endPoint, lastPoint;
    bool isGround, isMoving,iHaveBrick;
    Direct dir;
    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance == null) Instance=this;
        controller = new ControllerPlayer();
        swipe = controller.Player.Swipe;
        mousePosition = controller.Player.PositonMouse;
        mousePosition.Enable();
        swipe.Enable();
        swipe.started += StartSwipe;
        swipe.canceled += EndSwipe;
        dir = Direct.None;
    }
    void Start()
    {
        lastPoint = GetTargetPost();
        GameManager.instance.StartBrick(this);
        GameManager.instance.StartWin(this);

    }
    // Update is called once per frame
    void Update()
    {

        lastPoint = GetTargetPost();
        CheckGrounded();
        CheckMoving();
        Move(dir, lastPoint);
        GetBrick();


    }

    private void CheckMoving()
    {
        Vector3 endPosition = lastPoint;
        endPosition.y = transform.position.y;
        if (endPosition == transform.position) isMoving = false;
        else isMoving = true;
    }

    private void CheckGrounded()
    {
        if (Vector3.Distance(lastPoint, transform.position) < 0.00000001f) isGround = false;
        else isGround = true;
    }

    public void Move(Direct direct, Vector3 endPost)
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(endPost.x, transform.position.y, endPost.z), speed * Time.deltaTime);
        if (isMoving)
        {
            dir = Direct.None;
            return;
        }
        switch (direct)
        {
            case Direct.Left:
                pointCheck.localPosition = new Vector3(-1, pointCheck.localPosition.y, 0);
                Debug.Log("Trai");
                break;
            case Direct.Right:
                pointCheck.localPosition = new Vector3(1, pointCheck.localPosition.y, 0);
                Debug.Log("Phai");
                break;
            case Direct.Forward:
                pointCheck.localPosition = new Vector3(0, pointCheck.localPosition.y, 1);
                Debug.Log("Truoc");
                break;
            case Direct.Back:
                pointCheck.localPosition = new Vector3(0, pointCheck.localPosition.y, -1);
                Debug.Log("Sau");
                break;
            default:
                pointCheck.localPosition = new Vector3(0, pointCheck.localPosition.y, 0);
                break;
        }
        


    }
    public Direct SwipeDirect()
    {
        if (Vector3.Distance(startPoint, endPoint) > 100f)
        {
            Vector3 totalVector = endPoint - startPoint;
            float x = Mathf.Abs(totalVector.x);
            float y = Mathf.Abs(totalVector.y);
            if (x > y) return (totalVector.x > 0) ? Direct.Right : Direct.Left;
            if (x < y) return (totalVector.y > 0) ? Direct.Forward : Direct.Back;
        }
        return Direct.None;
    }
    public void AddBrick()
    {
        GameObject myBrick = Instantiate(brick,Vector3.up,Quaternion.identity,transform);
        listBrick.Add(myBrick);
        listBrick[listBrick.Count - 1].transform.SetParent(transform);
        
        myBrick.transform.Rotate(new Vector3(-90,0,0)); ;
        myBrick.transform.localPosition = new Vector3( 0,-(listBrick.Count*brickHeight),0);
        distanceCheck += brickHeight;
        transform.position = transform.position + new Vector3(0,brickHeight,0) ;
        animator.SetTrigger("Brick");
        iHaveBrick = true;
    }

    public void RemoveBrick()
    {
        if (listBrick.Count<1)
        {
            iHaveBrick = false;
            Debug.Log("het gach rui");
            return;
        }
        transform.position = transform.position - new Vector3(0, brickHeight, 0);
        Destroy(listBrick[listBrick.Count-1]);
        listBrick.RemoveAt(listBrick.Count - 1);
        animator.SetTrigger("Brick");
    }

    public void ClearBrick()
    {
        foreach (GameObject item in listBrick)
        {
            transform.position = transform.position - new Vector3(0, brickHeight, 0);
            Destroy(item);
            
        }
            listBrick.Clear();
    }
    public void StartSwipe(InputAction.CallbackContext callback)
    {
        startPoint = mousePosition.ReadValue<Vector2>();
        startPoint.z = 0;
    }

    public void EndSwipe(InputAction.CallbackContext callback)
    {
        endPoint = mousePosition.ReadValue<Vector2>();
        endPoint.z = 0;
        isMoving = true;
        dir = SwipeDirect();
    }
    public void OnInit()
    {
        // khoi tao cac gia tri o day
    }
    public Vector3 GetTargetPost()
    {
        Physics.Raycast(pointCheck.position, Vector3.down, out RaycastHit hit, distanceCheck, brickLayer);
        if (hit.transform != null&& hit.transform.name.Contains("Line")&&hit.transform.GetChild(0).gameObject.activeInHierarchy)
        {
            iHaveBrick = true;
        }
        if (hit.transform == null && !isGround||!iHaveBrick) return transform.position;
        if (hit.transform == null && isGround) return lastPoint;
        return hit.transform.position;
    }
    public void GetBrick()
    {
        Physics.Raycast(transform.position + Vector3.up, Vector3.down, out RaycastHit hit, distanceCheck, brickLayer);
        if (hit.transform == null|| hit.transform.childCount <= 0 || !hit.transform.GetChild(0).gameObject.activeInHierarchy) return;
        else if((hit.transform.GetComponentInChildren<PlayerBrick>()!=null))
        {
            hit.transform.GetChild(0).GetComponent<PlayerBrick>().GetBrick();
            GameManager.instance.AddBrick();
        }

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(pointCheck.position, pointCheck.position + new Vector3(0, distanceCheck * -1));
    }

    public void OnWin()
    {
       
        ClearBrick();
        Debug.Log("Da win");
        dir = Direct.None;
        StartCoroutine(NextLevelAuto(5));
        animator.SetTrigger("Win");
    }
    IEnumerator NextLevelAuto(float time)
    {
        yield return new WaitForSeconds(time);
        GameManager.instance.OnStop();
    }
    public void OnStop()
    {
        
    }
    public void GoToStartPoint(Vector3 point)
    {
        transform.position = point;
    }
}

public enum Direct
{
    Left, Forward, Right, Back, None
}
