using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using UnityEngine.UI;

public class TapToMove : MonoBehaviour
{
    bool collecting;
    GameObject objectToCollect;
    
    public Text woodText;

    void Start()
    {
        setWoodText();
    }

    void Update()
    {
        //check if the screen is touched / clicked   
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        SelectableUnitComponent selectable = GetComponent<SelectableUnitComponent>();

        if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || (Input.GetMouseButtonDown(1)) && selectable.selectionCircle != null)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (
                    hit.transform.name == "Tree(Clone)"
                    || hit.transform.name == "Rock(Clone)"
                ) {
                    collecting = true;
                    objectToCollect = hit.transform.gameObject;
                }

                agent.destination = hit.point;
            }
        }
        if (collecting && gameObject.transform.position == agent.destination)
        {
            collecting = false;
            Destroy(objectToCollect);
            Player.defaultPlayer.currency++;

            setWoodText();
            
        }

    }

    void setWoodText()
    {
        woodText.text = "Wood: " + Player.defaultPlayer.currency.ToString();
    }

}
