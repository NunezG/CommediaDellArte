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

    private float _timer;

	// Use this for initialization
	void Start () {
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
    }

    public void initializeWalker(int pathIndex, int spriteIndex)
    {
        WalkerScript walker = Instantiate(walkerPrefab, Vector3.zero, Quaternion.identity) as WalkerScript;
        walker.transform.parent = this.transform;
        walker.Initialisation();
        walker.setSprite(_spriteArray[spriteIndex]);
        walker.startWalking(_pathArray[pathIndex]);
    }

    void OnDrawGizmos()
    {
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
