using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Collections.Generic;

public class TapToMove1 : MonoBehaviour
{
    bool collecting;
    bool building;
    bool isIdle;
    bool isMoving;
    bool hasResource = false;
    GameObject objectToCollect;
    public GameObject idleText;
    public bool movingToResource = false;

    public bool collectingResource = false;
    public bool movingToStoreHouse = false;
    public bool storedResource = false;
    // Vector3 closestResource;
    Vector3 closestBuilding;
      
    public Text woodText, rockText;
    public Transform villager;
    public Transform resource;
    string currentResource;
    Transform[] resources;
    Vector3 townCenter = new Vector3(0, 0, 0);
    Vector3 closestResource;
    Vector3 pos;
    void Start()
    {
        setWoodText();
    }

    void Update()
    {
        if (isIdle) {
            idleText.SetActive(true);
        }
        else {
            idleText.SetActive(false);
        }
        //check if the screen is touched / clicked   
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        SelectableUnitComponent selectable = GetComponent<SelectableUnitComponent>();
        
        if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || (Input.GetMouseButtonDown(1)) && selectable.selectionCircle != null)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.name == "Tree(Clone)")
                {
                    collecting = true;
                    objectToCollect = hit.transform.gameObject;
                    currentResource = "Tree(Clone)";
                }
                else if (hit.transform.name == "Rock(Clone)")
                {
                    collecting = true;
                    objectToCollect = hit.transform.gameObject;
                    currentResource = "Rock(Clone)";
                }
            

                agent.destination = hit.point;
              
            }
        }
        if (collecting && gameObject.transform.position == agent.destination)
        {
            collecting = false;
            Destroy(objectToCollect);
            Player.defaultPlayer.currency++;

           

            
            movingToStoreHouse = true;
            hasResource = true;

           
        }
        if(hasResource==true && atStoreHouse()==false)
        {
            agent.destination = townCenter;
        }
        if (atStoreHouse() == true && hasResource==true)
        {

            if (currentResource == "Tree(Clone)")
            {
                Player.defaultPlayer.currency = Player.defaultPlayer.currency + 100;
                setWoodText();
            }
           
            hasResource = false;
            movingToStoreHouse = false;
            movingToResource = true;
             closestResource= FindClosestResource().transform.position;
            agent.destination = closestResource;
        }
        


    }
    GameObject FindClosestResource()
    {


        GameObject closest = null;
        var gos = GameObject.FindGameObjectsWithTag(currentResource);
        var distance = Mathf.Infinity;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - gameObject.transform.position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }

        }

        return closest;
    }
    bool atStoreHouse()
    {


        pos = gameObject.transform.position;
        if (Vector3.Distance(pos, townCenter) < 2.0f)
        {
            movingToStoreHouse = false;
            return true;
        }
        else
            return false;
    }


    void setWoodText()
    {
        woodText.text = "Wood: " + Player.defaultPlayer.currency.ToString();
    }
    void setRockText()
    {
        rockText.text = "Rock: " + Player.defaultPlayer.currency.ToString();
    }

}
