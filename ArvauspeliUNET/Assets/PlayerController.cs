using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using System.Collections.Generic;

public class PlayerController : NetworkBehaviour
{
    CanvasGroup activePlayerCanvasGroup;
    CanvasGroup inactivePlayerCanvasGroup;
    GraphicRaycaster gr;
    bool activePlayer = false;
    bool gameOn = false; // if we are showing instructions or some shit, the game is off then.
    PointerEventData ped = new PointerEventData(null);
    List<RaycastResult> results = new List<RaycastResult>();

    public void Start()
    {
        
    }
    public override void OnStartLocalPlayer()
    {
        //get if i'm the active one
        activePlayerCanvasGroup = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(0).GetComponent<CanvasGroup>();
        inactivePlayerCanvasGroup = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(1).GetComponent<CanvasGroup>();
        gr = activePlayerCanvasGroup.GetComponentInParent<GraphicRaycaster>();
    }

    void Update()
    {
        if (!isLocalPlayer)
            return;

        if (Input.touchCount > 0 && gameOn)
        {
            if(activePlayer)
            {
                //Read about the one you got right or wrong. Info screen that is.
            }
            else
            {
                // check if we touched right or wrong... Probably from just one inactive player.
                if(Input.GetTouch(0).phase == TouchPhase.Began)
                {                    
                    ped.position = Input.GetTouch(0).position;
                    results.Clear();
                    gr.Raycast(ped, results);
                }
                foreach(RaycastResult rr in results)
                {
                    Debug.Log("Hit: " + rr.gameObject.name);
                }
            }
        }
        // FOR DEBUG. DELETE LATER
        if (Input.GetMouseButtonDown(0))
        {
            /*GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
            foreach (GameObject go in allObjects)
                if (go.activeInHierarchy)
                    Debug.Log(go.name + " is an active object with tag " + go.tag);*/
            if (activePlayer)
            {
                //Read about the one you got right or wrong. Info screen that is.
            }
            else
            {
                    ped.position = Input.mousePosition;
                    results.Clear();
                    gr.Raycast(ped, results);
                
                foreach (RaycastResult rr in results)
                {
                    Debug.Log("Hit: " + rr.gameObject.name);
                    rr.gameObject.GetComponent<Image>().color = Color.red;
                }
            }
        }
    }
    /*
    [Command]
    void CmdFire()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 6;
        NetworkServer.Spawn(bullet);
        Destroy(bullet, 2.0f);
    }*/
}