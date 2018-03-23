using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SelectionManager : MonoBehaviour {
    public GameObject selectionCirclePrefab;

    private bool isSelecting = false;
    private Vector3 mousePosition1;
    private Rect selectionBox;
    private List<GameObject> selectedUnits = new List<GameObject>();

    private const float CLICK_DELTA = 0.25f; // Maximum time between button press and release to be considered a click
    private float pressTime; // Time at which the mouse button was first pressed

	void Update () {
        // When left mouse button clicked, begin selection by storing first mouse position
		if(Input.GetMouseButtonDown(0)) {
            pressTime = Time.time;

            isSelecting = true;
            mousePosition1 = Input.mousePosition;

            // Remove selection circles from previously selected units
            foreach(var obj in FindObjectsOfType<Selectable>()) {
                if(obj.selectionCircle != null) {
                    Destroy(obj.selectionCircle);
                    obj.selectionCircle = null;
                }
            }
        }

        // While left mouse button down
        if(isSelecting) {
            // Define the selection box from the initial and current mouse positions
            selectionBox = PointsToRect(mousePosition1, Input.mousePosition);

            foreach(var obj in FindObjectsOfType<Selectable>()) {
                // Get position of object in screen coordinates
                Vector3 position = Camera.main.WorldToScreenPoint(obj.gameObject.transform.position);

                // If that position is within the selection box
                if(selectionBox.Contains(position)) {
                    if(obj.selectionCircle == null) {
                        obj.selectionCircle = Instantiate(selectionCirclePrefab);
                        obj.selectionCircle.transform.position = new Vector3(0, 0, 0);
                        obj.selectionCircle.transform.SetParent(obj.transform, false);
                    }
                } else {
                    // Remove circle from any previously highlighted objects
                    if(obj.selectionCircle != null) {
                        Destroy(obj.selectionCircle.gameObject);
                        obj.selectionCircle = null;
                    }
                }
            }
        }

        // When left mouse button released, end selection by adding the contained units to selectedUnits
        if(Input.GetMouseButtonUp(0)) {
            // Call OnDeselect for every unit and empty the list before replacing with new selections
            foreach(var unit in selectedUnits) {
                unit.GetComponent<Selectable>().OnDeselect();
            }
            selectedUnits.Clear();

            // If user drags the mouse
            if(Time.time - pressTime > CLICK_DELTA) {
                foreach(var obj in FindObjectsOfType<Selectable>()) {
                    // Get position of object in screen coordinates
                    Vector3 position = Camera.main.WorldToScreenPoint(obj.gameObject.transform.position);

                    if(selectionBox.Contains(position)) {
                        selectedUnits.Add(obj.gameObject);
                        obj.OnSelect();
                    }
                }
            } else { // If user clicks the mouse
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(mousePosition1);
                if(Physics.Raycast(ray, out hit)) {
                    if(hit.collider.gameObject.GetComponent<Selectable>() != null) {
                        GameObject obj = hit.collider.gameObject;
                        selectedUnits.Add(obj);
                        Selectable selectable = obj.GetComponent<Selectable>();
                        selectable.OnSelect();
                        selectable.selectionCircle = Instantiate(selectionCirclePrefab);
                        selectable.selectionCircle.transform.position = new Vector3(0, 0, 0);
                        selectable.selectionCircle.transform.SetParent(obj.transform, false);
                    } 
                }
            }
            
            isSelecting = false;
        }

        // When player right-clicks, send selected units to location they clicked
        if(Input.GetMouseButtonDown(1)) {
            foreach(var unit in selectedUnits) {
                NavMeshAgent agent = unit.GetComponent<NavMeshAgent>();
                if(agent == null) {
                    continue;
                }

                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if(Physics.Raycast(ray, out hit)) {
                    agent.destination = hit.point;
                }
            } 
        }
	}

    Rect PointsToRect(Vector3 point1, Vector3 point2) {
        float x = Mathf.Min(point1.x, point2.x);
        float y = Mathf.Min(point1.y, point2.y);
        float width = Mathf.Abs(point1.x - point2.x);
        float height = Mathf.Abs(point1.y - point2.y);
        return new Rect(x, y, width, height);
    }

    void OnGUI() {
        if(isSelecting) {
            // Create a rect from both mouse positions
            var rect = Utils.GetScreenRect(mousePosition1, Input.mousePosition);
            Utils.DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
            Utils.DrawScreenRectBorder(rect, 2, new Color(0.8f, 0.8f, 0.95f));
        }
    }
}
