using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class bulleInfoScript : MonoBehaviour {


	public float fadeSpeed;
    public List<Bubble> spriteList;

	private SpriteRenderer _spriteRenderer;
	// Use this for initialization
	void Start () {
		_spriteRenderer = GetComponent<SpriteRenderer> ();
		_spriteRenderer.color = new Color (1, 1, 1, 0);
	}
	
	// Update is called once per frame
	void Update () {}


	public void showBubble(float time, string name){
        _spriteRenderer.sprite = null;
        for (int i = 0; i < spriteList.Count; i++)
        {
            if (name == spriteList[i]._name)
                _spriteRenderer.sprite = spriteList[i]._sprite;
        }
        if (_spriteRenderer.sprite == null)
            return;
        StartCoroutine(showBubbleCoroutine(time));
	}

	IEnumerator showBubbleCoroutine(float time){

		while (_spriteRenderer.color.a != 1) {

			if(_spriteRenderer.color.a>=0.9)
				_spriteRenderer.color = new Color (1, 1, 1, 1);
			else
				_spriteRenderer.color = new Color (1, 1, 1, Mathf.Lerp (_spriteRenderer.color.a, 1, fadeSpeed * Time.deltaTime));
			yield return null;
		}

		yield return new WaitForSeconds(time);

		while (_spriteRenderer.color.a != 0) {

			if(_spriteRenderer.color.a<=0.1)
				_spriteRenderer.color = new Color (1, 1, 1, 0);
			else
				_spriteRenderer.color = new Color(1,1,1,Mathf.Lerp (_spriteRenderer.color.a, 0, fadeSpeed * Time.deltaTime));
			yield return null;
		}
				
		yield break;
	}

    [System.Serializable]
    public class Bubble
    {
        public Sprite _sprite;
        public string _name;
        public Bubble(string st, Sprite sp){
            _sprite = sp;
            _name = st;
        }

    }

}
