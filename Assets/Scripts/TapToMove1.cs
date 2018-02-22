using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class TapToMove1 : MonoBehaviour
{
    bool collecting;
    GameObject objectToCollect;

    void Start()
    {
    }

    void Update()
    {
        //check if the screen is touched / clicked   
        NavMeshAgent agent = GetComponent<NavMeshAgent>();

        if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || (Input.GetMouseButtonDown(1)))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.name == "Tree(Clone)") {
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
            Debug.Log(Player.defaultPlayer.currency);
        }

    }
    void Destroy() {

    }
}
