using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WalkerScript : MonoBehaviour {

    public float _moveSpeed,_gravity;
    [HideInInspector]
    public bool isWalking = false;

	public List<Scream> _screamList;

    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
	private AudioSource _audioSource;
	private int screamIndex = 0;

    public void Initialisation(){
        _spriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
        _animator = this.GetComponentInChildren<Animator>();
		_audioSource = this.GetComponent<AudioSource> ();
    }

	// Use this for initialization
	void Start () {
        _spriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
        _animator = this.GetComponentInChildren<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

    }
	public void grab(){
		screamIndex = Random.Range (0, _screamList.Count);
		_audioSource.clip = _screamList[screamIndex]._grabSound;
		_audioSource.loop = true;
		_audioSource.Play ();
	}

	public void throwAway(Vector2 force){
        Debug.Log(force.magnitude);

        if (force.magnitude < 500)
        {
            isWalking = true;
            return;
        }

		_audioSource.Stop ();
		_animator.SetTrigger ("thrown");

        Rigidbody body = this.GetComponent<Rigidbody>();
        body.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
        body.useGravity = true;

		//Rigidbody2D body = this.GetComponent<Rigidbody2D> ();
		//body.fixedAngle = false;
		//body.gravityScale = 10;

        body.AddForceAtPosition(new Vector3( force.x * 20, force.y * 20 ,0), Vector3.zero);

		_audioSource.loop = false;
		_audioSource.PlayOneShot (_screamList[screamIndex]._throwSound);

		Destroy (this, 3);
	}

    public void setSprite(Sprite sprite)
    {
        _spriteRenderer.sprite = sprite;
    }


    public void startWalking(Path path)
    {
        this.transform.position = path._start;
        isWalking = true;
        StartCoroutine(walkingCoroutine(path));
    }

    private IEnumerator walkingCoroutine(Path path)
    {
        Vector3 direction = (path._end - path._start).normalized;
        while ( (path._end - this.transform.position).magnitude > 5)
        {
            if (isWalking) {
                //gravity
                if (this.transform.position.y > path._end.y)
                {
                    if (this.transform.position.y - _gravity * Time.deltaTime < path._end.y)
                        this.transform.position = new Vector3(this.transform.position.x, path._end.y, this.transform.position.z);
                    else
                         this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - _gravity * Time.deltaTime, this.transform.position.z );
                }
                else
                {
                    if (Vector3.Distance(this.transform.position, path._end) <= (direction * (Time.deltaTime * _moveSpeed)).magnitude)
                    {
                        this.transform.position = path._end;
                        break;
                    }
                    else
                    {
                        this.transform.position += direction * (Time.deltaTime * _moveSpeed);
                    }
                }
            }
            yield return null;
        }
        isWalking = false;
        WalkerManager.currentWalker--;
        Destroy(this.gameObject);
        yield break;
    }
}

[System.Serializable]
public class Scream{
	public AudioClip _grabSound;
	public AudioClip _throwSound;
}
