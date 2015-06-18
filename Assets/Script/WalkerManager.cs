using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WalkerManager : MonoBehaviour {

    public static int currentWalker = 0;

    public WalkerScript walkerPrefab;
    public Path[] _pathArray;
    public Sprite[] _spriteArray;
    public int _maxWalker = 1;
    public float _timeBetweenSpawn = 1.0f;

	public float throwForce = 1;

    private float _timer;
    [HideInInspector]
    public static List<WalkerScript> walkerList;
    private WalkerScript dragWalker;
    private Vector3 oldMousePosition;


    //debug
    Vector3 from, to;

	// Use this for initialization
	void Start () {
        walkerList = new List<WalkerScript>();
	}
	
	// Update is called once per frame
	void Update () {

        _timer -= Time.deltaTime;
        while (currentWalker != _maxWalker && _timer <= 0)
        {
            initializeWalker(Random.Range(0, _pathArray.Length), Random.Range(0, _spriteArray.Length));
            _timer = _timeBetweenSpawn;
            currentWalker++;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1000)) - Camera.main.transform.position);
            RaycastHit hit;
            Physics.Raycast(ray, out hit, 1000);


            from = Camera.main.transform.position;
            to = (Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1000)) - Camera.main.transform.position) * 100;

            if (hit.collider != null && hit.collider.gameObject.GetComponent<WalkerScript>() != null)
            {
                dragWalker = hit.collider.gameObject.GetComponent<WalkerScript>();
                dragWalker.grab();
                oldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, dragWalker.transform.position.z));
                dragWalker.isWalking = false;
            }
            /*for (int i = 0; i < walkerList.Count; i++ )
            {
                if (walkerList[i] != null && walkerList[i].GetComponent<Collider2D>().overlapMouse())
                {
                    dragWalker = walkerList[i];
					dragWalker.grab();
                    oldMousePosition = Camera.main.ScreenToWorldPoint ( new Vector3 (Input.mousePosition.x, Input.mousePosition.y, dragWalker.transform.position.z));
                    dragWalker.isWalking = false;
                    break;
                }
            }*/
        }

        if (Input.GetButtonUp("Fire1") && dragWalker!= null)
        {
            //dragWalker.isWalking = true;
			Vector3 newPos = Camera.main.ScreenToWorldPoint ( new Vector3 (Input.mousePosition.x, Input.mousePosition.y, dragWalker.transform.position.z));
			Vector3 temp = newPos - oldMousePosition;
			dragWalker.throwAway(temp * throwForce);
            Destroy(dragWalker.gameObject, 1);
            dragWalker = null;
			currentWalker--;
        }

        if(dragWalker != null){
            Vector3 newPos = Camera.main.ScreenToWorldPoint ( new Vector3 (Input.mousePosition.x, Input.mousePosition.y, dragWalker.transform.position.z));

            dragWalker.transform.position += newPos - oldMousePosition;
            if (dragWalker.transform.position.y < -1.4f)
            {
                dragWalker.transform.position = new Vector3(dragWalker.transform.position.x, -1.4f, dragWalker.transform.position.z);
            }
            oldMousePosition = Camera.main.ScreenToWorldPoint ( new Vector3 (Input.mousePosition.x, Input.mousePosition.y, dragWalker.transform.position.z));

        }

    }

    public void initializeWalker(int pathIndex, int spriteIndex)
    {
        WalkerScript walker = Instantiate(walkerPrefab, Vector3.zero, Quaternion.identity) as WalkerScript;
        walker.transform.parent = this.transform;
        walker.Initialisation();
        walker.setSprite(_spriteArray[spriteIndex]);
        walker.startWalking(_pathArray[pathIndex]);
        walkerList.Add(walker);
    }

    void OnDrawGizmos()
    {

       // Gizmos.DrawRay(ray.origin, ray.direction);

        Gizmos.DrawLine(Camera.main.transform.position, Camera.main.ScreenToWorldPoint ( new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 1000)) );
        Gizmos.color = new Color(0, 0, 1);
        Gizmos.DrawLine(from, to);



        for (int i = 0; i < _pathArray.Length; i++)
        {
            Gizmos.color = new Color(1, 0, 0);
            Gizmos.DrawSphere(_pathArray[i]._start, 2f);
            Gizmos.color = new Color(0, 0, 1);
            Gizmos.DrawSphere(_pathArray[i]._end, 2f);
            Gizmos.color = new Color(0.5f, 0, 0.5f);
            Gizmos.DrawLine(_pathArray[i]._start, _pathArray[i]._end);
        }
    }
}


[System.Serializable]
public class Path
{
    public Vector3 _start, _end;
    public Path()
    {
        _start = Vector3.zero;
        _end = Vector3.zero;
    }
    public Path(Vector3 v1, Vector3 v2)
    {
        _start = v1;
        _end = v2;
    }
}
