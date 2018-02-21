using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class RtsManager : MonoBehaviour {
    public static RtsManager current = null;

    public List<PlayerInfo> players = new List<PlayerInfo>();
    public GameObject groundCollider;

    public Vector3? ScreenPointToMapPosition(Vector2 point) {
        var ray = Camera.main.ScreenPointToRay(point);
        RaycastHit hit;
        Collider newCollider = groundCollider.GetComponent<Collider>();
        if(!newCollider.Raycast(ray, out hit, Mathf.Infinity)) {
            return null;
        }

        return hit.point;
    }

    public bool IsGameObjectSafeToPlace(GameObject go) {
        var verts = go.GetComponent<MeshFilter>().mesh.vertices;
        var obstacles = GameObject.FindObjectsOfType<NavMeshObstacle>();
        var cols = new List<Collider>();
        foreach(var o in obstacles) {
            if(o.gameObject != go) {
                cols.Add(o.gameObject.GetComponent<Collider>());
            }
        }

        foreach(var v in verts) {
            NavMeshHit hit;
            var vReal = go.transform.TransformPoint(v);
            NavMesh.SamplePosition(vReal, out hit, 20, NavMesh.AllAreas);

            bool onXAxis = Mathf.Abs(hit.position.x - vReal.x) < 0.5f;
            bool onZAxis = Mathf.Abs(hit.position.z - vReal.z) < 0.5f;
            bool hitCollider = cols.Any(c => c.bounds.Contains(vReal));

            if(!onXAxis || !onZAxis || hitCollider) {
                return false;
            }
        }

        return true;
    }

	// Use this for initialization
	void Start () {
        current = this;
        
        foreach(var playerInfo in players) { // For each set of player info in players list
            foreach(var unit in playerInfo.startingUnits) { // Instantiate every unit in its starting units list
                var gameObject = (GameObject)GameObject.Instantiate(unit, playerInfo.location.position, playerInfo.location.rotation);
                var player = gameObject.AddComponent<Player>(); // Attach Player object to them with corresponding player info
                player.info = playerInfo;
                if(!playerInfo.isAI) {
                    if(Player.defaultPlayer == null) {
                        Player.defaultPlayer = playerInfo;
                    }
                }
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
