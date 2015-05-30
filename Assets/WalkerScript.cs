using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WalkerScript : MonoBehaviour {

    public float moveSpeed = 1.0f;
    public List<Path> pathList;
    public Sprite[] spriteArray;
    [HideInInspector]
    public bool isWalking = false;

    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private float timer;

	// Use this for initialization
	void Start () {
        _spriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
        _animator = this.GetComponentInChildren<Animator>();
        timer = Random.Range(0f, 2.0f);
	}
	
	// Update is called once per frame
	void Update () {      
        if (!isWalking)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                startWalking();
                timer = Random.Range(0f,2.0f); 
            }
        }
	}

    public void startWalking()
    {
        Debug.Log(Random.Range(0, spriteArray.Length));
        startWalking(Random.Range(0, pathList.Count), Random.Range(0, spriteArray.Length) );
    }
    public void startWalking(int pathIndex, int spriteIndex)
    {
        isWalking = true;
        _spriteRenderer.sprite = spriteArray[spriteIndex];
        this.transform.position = pathList[pathIndex]._start;
        StartCoroutine(walkingCoroutine( pathList[pathIndex]));
    }
    private IEnumerator walkingCoroutine(Path path)
    {
        Vector3 direction = (path._end - path._start).normalized;
        while (this.transform.position != path._end)
        {
            if (Vector3.Distance(this.transform.position, path._end) < (direction * (Time.deltaTime * moveSpeed)).magnitude )
            {
                this.transform.position = path._end;
            }
            else { 
                this.transform.position += direction *(Time.deltaTime * moveSpeed);
            }
            yield return null;
        }
        isWalking = false;
        yield break;
    }


    void OnDrawGizmos()
    {
        for (int i = 0; i < pathList.Count; i++)
        {
            Gizmos.color = new Color(1, 0, 0);
            Gizmos.DrawSphere(pathList[i]._start, 2.5f);
            Gizmos.color = new Color(0, 0, 1);
            Gizmos.DrawSphere(pathList[i]._end, 2.5f);
            Gizmos.color = new Color(0.5f, 0,0.5f );
            Gizmos.DrawLine(pathList[i]._start, pathList[i]._end);
        }
    }
}

[System.Serializable]
public class Path
{
    public Vector3 _start, _end ;
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