using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour {
    public GameObject selectionCirclePrefab;
    public GameObject unitMenu;
    public GameObject buildingMenu;
    public GameObject unitToTrain;
    public Text healthText;
    public Text testText;

    private bool isSelecting = false;
    private Vector3 oldMousePosition;
    private Rect selectionBox;
    private UnitAttribute lastUnit;
    private List<GameObject> selectedUnits = new List<GameObject>();

    private const float CLICK_DELTA = 0.25f; // Maximum time between button press and release to be considered a click
    private float pressTime; // Time at which the mouse button was first pressed

	void Update () {
        // If over a UI element and not finishing up a drag
        if(EventSystem.current.IsPointerOverGameObject() && !isSelecting) {
            return;
        }

        // When left mouse button clicked, begin selection by storing first mouse position
		if(Input.GetMouseButtonDown(0)) {
            pressTime = Time.time;

            isSelecting = true;
            oldMousePosition = Input.mousePosition;

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
            selectionBox = PointsToRect(oldMousePosition, Input.mousePosition);

            foreach(var obj in FindObjectsOfType<Selectable>()) {
                // Get position of object in screen coordinates
                Vector3 position = Camera.main.WorldToScreenPoint(obj.gameObject.transform.position);

                // If that position is within the selection box
                if(selectionBox.Contains(position)) {
                    if(obj.selectionCircle == null) {
                        obj.selectionCircle = Instantiate(selectionCirclePrefab);
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
                if (unit) unit.GetComponent<Selectable>().OnDeselect();
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
                Ray ray = Camera.main.ScreenPointToRay(oldMousePosition);
                if(Physics.Raycast(ray, out hit)) {
                    if(hit.collider.gameObject.GetComponent<Selectable>() != null) {
                        GameObject obj = hit.collider.gameObject;
                        selectedUnits.Add(obj);
                        Selectable selectable = obj.GetComponent<Selectable>();
                        selectable.OnSelect();
                        selectable.selectionCircle = Instantiate(selectionCirclePrefab);
                        selectable.selectionCircle.transform.position = new Vector3(0, 0, 0);
                        selectable.selectionCircle.transform.localScale = obj.transform.lossyScale * 1.75f;
                        selectable.selectionCircle.transform.SetParent(obj.transform, false);
                    } 
                }
            }

            isSelecting = false;
        }

        // When player right-clicks, send selected units to location they clicked
        if(Input.GetMouseButtonDown(1)) {
            //Generates an offset for template for each unit
            int width = 0;
            int depth = 0;
            Vector3 offsetGrid = new Vector3(width * 2, 0, depth * 2);

            foreach (var unit in selectedUnits) {
                NavMeshAgent agent = unit.GetComponent<NavMeshAgent>();
                if(agent == null) {
                    continue;
                }
                
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit)) {
                    agent.destination = hit.point + offsetGrid;
                }

                //increments the offset to provide a 5 x '#ofUnitsSelected/5' grid for the units to move to.
                if (width < 5) {
                    width++;
                }
                else {
                    width = 0;
                    depth++;
                }
            } 
        }

        // When selection is finished, sets up the relevant menu for the unit last selected
        if(selectedUnits.Count > 0) {
            lastUnit = selectedUnits[0].GetComponent<UnitAttribute>();

            if(lastUnit != null) {
                //Access Health
                float health = lastUnit.health;
                //Set unit menu to visible, display health value
                healthText.text = "Health: " + health;
                unitMenu.active = true;
            } else {
                buildingMenu.active = true;
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
            var rect = GetScreenRect(oldMousePosition, Input.mousePosition);
            DrawScreenRectWithBorder(rect, 2, new Color(0.8f, 0.8f, 0.95f, 0.25f), new Color(0.8f, 0.8f, 0.95f));
        }
    }

    public void BuildingUpgrade() {
        // lastUnit is set to null if the last thing in the selection was a building
        if(lastUnit == null) {
            testText.text = "successfully pulled";

            var buildingUnit = selectedUnits[0].GetComponent<UpgradeCapitol>();
            buildingUnit.version++;
            testText.text = "successfully updated";
        }
    }

    public void BuildingSell() {
        // lastUnit is set to null if the last thing in the selection was a building        
        if(lastUnit == null) {
            // Destroys the selected building
            Destroy(selectedUnits[0]);
        }
    }

    public void UnitUpgrade() {
        // lastUnit is not set to null if the last thing in the selection was a unit
        if(lastUnit != null) {
            // upgrades the unit's health to 30 from the default 10, then refreshes the health display
            lastUnit.health = 30;
            healthText.text = "Health: " + lastUnit.health;
        }
    }

    public void OnTrainUnitButtonClick() {
        Invoke("TrainUnit", 5);
    }

    public void TrainUnit() {
        Transform parent = selectedUnits[0].GetComponent<Transform>();
        var obj = Instantiate(unitToTrain, parent);
        obj.transform.position += new Vector3(5, 0, 0);
    }

    // nick 4/7: everything below was in Utils.cs, but I moved it here because
    // there a was a lot of unused code in utils
    static Texture2D _whiteTexture;
    public static Texture2D WhiteTexture
    {
        get
        {
            if( _whiteTexture == null )
            {
                _whiteTexture = new Texture2D( 1, 1 );
                _whiteTexture.SetPixel( 0, 0, Color.white );
                _whiteTexture.Apply();
            }

            return _whiteTexture;
        }
    }

    public static Rect GetScreenRect( Vector3 screenPosition1, Vector3 screenPosition2 ) {
        // Move origin from bottom left to top left
        screenPosition1.y = Screen.height - screenPosition1.y;
        screenPosition2.y = Screen.height - screenPosition2.y;
        // Calculate corners
        var topLeft = Vector3.Min( screenPosition1, screenPosition2 );
        var bottomRight = Vector3.Max( screenPosition1, screenPosition2 );
        // Create Rect
        return Rect.MinMaxRect( topLeft.x, topLeft.y, bottomRight.x, bottomRight.y );
    }

    public static void DrawScreenRectWithBorder(Rect rect, float thickness, Color mainColor, Color borderColor) {
        Rect top = new Rect(rect.xMin, rect.yMin, rect.width, thickness);
        Rect left = new Rect(rect.xMin, rect.yMin, thickness, rect.height);
        Rect right = new Rect(rect.xMax - thickness, rect.yMin, thickness, rect.height);
        Rect bottom = new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness);

        Color oldColor = GUI.color;

        GUI.color = mainColor;
        GUI.DrawTexture(rect, WhiteTexture);

        GUI.color = borderColor;
        GUI.DrawTexture(top, WhiteTexture);
        GUI.DrawTexture(left, WhiteTexture);
        GUI.DrawTexture(right, WhiteTexture);
        GUI.DrawTexture(bottom, WhiteTexture);

        GUI.color = oldColor;
    }
}
