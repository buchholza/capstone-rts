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
    public Text unitHealthText;
    public Text buildingHealthText;
    public Text unitTitle;
    public Text buildingTitle;

    public GameObject buildingUpgradePrefab;
    public GameObject crowdTargetPrefab;

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
            foreach(var obj in FindObjectsOfType<UnitAttribute>()) {
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

            foreach(var obj in FindObjectsOfType<UnitAttribute>()) {

                // we are only interested in selecting units we can control
                if (!obj.isPlayerControlled || !isPerson(obj)) continue;

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
                if (unit) unit.GetComponent<UnitAttribute>().OnDeselect();
            }
            selectedUnits.Clear();

            // when we deselect close any open menus
            buildingMenu.SetActive(false);
            unitMenu.SetActive(false);

            // If user drags the mouse
            if(Time.time - pressTime > CLICK_DELTA) {
                foreach(var obj in FindObjectsOfType<UnitAttribute>()) {
                    if (!obj.isPlayerControlled) continue;
                    // Get position of object in screen coordinates
                    Vector3 position = Camera.main.WorldToScreenPoint(obj.gameObject.transform.position);

                    if(selectionBox.Contains(position)) {
                        if (isPerson(obj)) {
                            selectedUnits.Add(obj.gameObject);
                            obj.OnSelect();
                        }
                    }
                }
            } else { // If user clicks the mouse
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(oldMousePosition);
                if(Physics.Raycast(ray, out hit)) {
                    GameObject hitObject = hit.collider.gameObject;
                    UnitAttribute hitUnit = hitObject.GetComponent<UnitAttribute>();

                    if(hitUnit != null && hitUnit.isPlayerControlled) {
                        selectedUnits.Add(hitObject);
                        UnitAttribute selectable = hitUnit;
                        selectable.OnSelect();
                        selectable.selectionCircle = Instantiate(selectionCirclePrefab);
                        selectable.selectionCircle.transform.position = new Vector3(0, 0, 0);
                        selectable.selectionCircle.transform.localScale = hitObject.transform.lossyScale * 1.75f;
                        selectable.selectionCircle.transform.SetParent(hitObject.transform, false);
                    } 
                }
            }

            isSelecting = false;
        }

        // When player right-clicks, send selected units to location they clicked

        if(Input.GetMouseButtonDown(1)) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            bool hitWorked = Physics.Raycast(ray, out hit);

            if (hitWorked) {
                // tell the crowd system what units were sending where
                var ct = Instantiate(crowdTargetPrefab, hit.point, Quaternion.identity).GetComponent<CrowdTarget>();
                ct.SetManagedUnits(new List<GameObject>(selectedUnits));

                for (int i = 0; i < selectedUnits.Count; i++) {
                    var unit = selectedUnits[i];

                    if (!unit) continue;

                    NavMeshAgent agent = unit.GetComponent<NavMeshAgent>();
                    if(agent == null) {
                        continue;
                    }
                    agent.destination = hit.point;
                    if (agent.isStopped) agent.isStopped = false;
                    
                    // if the unit was gathering resources before, now it should stop
                    CrowdMovement crowd = unit.GetComponent<CrowdMovement>();
                    GatherResource gather = unit.GetComponent<GatherResource>();

                    crowd.enabled = true;
                    gather.enabled = false;

                    var crowdMover = unit.GetComponent<CrowdMovement>();
                    if (crowdMover) crowdMover.SetCrowdTarget(ct, i);
                } 
            }
        }

        // When selection is finished, sets up the relevant menu for the unit last selected
        if(selectedUnits.Count > 0) {
            lastUnit = selectedUnits[0].GetComponent<UnitAttribute>();

            if (!lastUnit) {
                print("something is seriously messed up");
            }

            if(isPerson(lastUnit)) {
                //Access health
                float health = lastUnit.health;
                //Display health value
                unitHealthText.text = "Health: " + health;
                //Change the title of the unit menu based on unit's type
                var typeString = "";
                if (lastUnit.type == UnitAttribute.UnitType.NormalUnit) {
                    typeString = "Normal Unit";
                } else if (lastUnit.type == UnitAttribute.UnitType.PitchforkUnit) {
                    typeString = "Farmer";
                } else if (lastUnit.type == UnitAttribute.UnitType.SwordUnit) {
                    typeString = "Fighter";
                } else if (lastUnit.type == UnitAttribute.UnitType.SpartanUnit) {
                    typeString = "Spartan";
                }
                unitTitle.text = typeString;
                //Set unit menu active, deactivating building menu
                unitMenu.SetActive(true);
                buildingMenu.SetActive(false);
            } else {
                //Change the title of the building menu based on building's type
                var typeString = "";
                if (lastUnit.type == UnitAttribute.UnitType.Capitol) {
                    typeString = "Capitol";
                } else if (lastUnit.type == UnitAttribute.UnitType.Barracks) {
                    typeString = "Barracks";
                } else if (lastUnit.type == UnitAttribute.UnitType.Tower) {
                    typeString = "Tower";
                }
                //buildingTitle.text = typeString;
                //Set building menu active, deactivating unit menu
                unitMenu.SetActive(false);
                buildingMenu.SetActive(true);
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
        // if its not a unit its a building
        if(!isPerson(lastUnit)) {
            if(lastUnit.type == UnitAttribute.UnitType.Capitol) {
                var buildingUnit = selectedUnits[0].GetComponent<UpgradeCapitol>();
                buildingUnit.version++;
            } else { // assume its the old type of building (the cube one)
                var oldBuilding = selectedUnits[0];
                Destroy(oldBuilding);
                GameObject.Instantiate(buildingUpgradePrefab, oldBuilding.transform.position, oldBuilding.transform.rotation);
            }
        }
    }

    public void BuildingSell() {
        // if its not a unit its a building
        if(!isPerson(lastUnit)) {
            // Destroys the selected building
            Destroy(selectedUnits[0]);
        }
    }

    public void UnitUpgrade() {
        // test if its a unit
        if(isPerson(lastUnit)) {
            // upgrades the unit's health to 30 from the default 10, then refreshes the health display
            lastUnit.health = 30;
            lastUnit.maxHealth = 30;
            unitHealthText.text = "Health: " + lastUnit.health;
        }
    }

    public bool isPerson (UnitAttribute unit) {
        if (unit.type == UnitAttribute.UnitType.NormalUnit
            || unit.type == UnitAttribute.UnitType.PitchforkUnit
            || unit.type == UnitAttribute.UnitType.SwordUnit
            || unit.type == UnitAttribute.UnitType.SpartanUnit ) {
            return true;
        }
        return false;
    }

    public void UnitGather() {
        // test if its a unit
        if(isPerson(lastUnit)) {
            var gather = lastUnit.GetComponent<GatherResource>();
            if (gather) {
                if (gather.enabled == false) {
                    gather.enabled = true;
                    gather.Reset();

                    var crowd = lastUnit.GetComponent<CrowdMovement>();
                    if (crowd) crowd.enabled = false;

                    var agent = lastUnit.GetComponent<NavMeshAgent>();
                    if (agent) agent.isStopped = false;
                } else {
                    gather.enabled = false;

                    var crowd = lastUnit.GetComponent<CrowdMovement>();
                    if (crowd) crowd.enabled = true;

                    var agent = lastUnit.GetComponent<NavMeshAgent>();
                    if (agent) agent.isStopped = true;
                }
            }
        }
    }

    public void OnTrainUnitButtonClick() {
        Invoke("TrainUnit", 2);
    }

    public void TrainUnit() {
        Transform building = selectedUnits[0].GetComponent<Transform>();
        var obj = Instantiate(unitToTrain);
        obj.transform.position = building.transform.position + new Vector3(5, 0, 0);
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
