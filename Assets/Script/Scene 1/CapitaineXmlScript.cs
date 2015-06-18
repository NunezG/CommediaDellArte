using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class CapitaineXmlScript : MonoBehaviour
{
    public static int scaryValue = 0; 
	public List<AudioClip> _clipList;


    public Sprite _touchSprite2, _talkSprite2;
	public bool forVoiceOnly = false;

    private int _touchCount = 0, _talkCount = 0, _clipIndex = 0;
	private SoundController _soundController;
	private static Animator _animator;
	private static AudioSource _audioSource;
	private static IEnumerator resetCoroutine;
	private static CapitaineXmlScript _instance;

    private float timer = 0;

    // Use this for initialization
    void Start()
	{
		if (forVoiceOnly) {
			_animator = this.GetComponent <Animator> ();
			if (this.transform.parent != null){
				_audioSource =  this.GetComponent<AudioSource> ();
			}
			_instance = this;
		}
    }

    // Update is called once per frame
    void Update()
    {


        if (_animator.GetCurrentAnimatorStateInfo(0).shortNameHash == Animator.StringToHash("idle_base") && _animator.GetBool("postIntro"))
        {
            timer += Time.deltaTime;
            if (timer > 2)
            {
                _animator.SetTrigger("voiceEvent");
                timer = 0;
            }
        }
        else
        {
            timer = 0;
        }


    }

	public void ignoreEvent(){

	}

	public static void ignore(){
		
	}


	public void interruptEvent(){
		CapitaineXmlScript.interrupt ();
	}

	public static void interrupt(){
        if (resetCoroutine != null)
		    _instance.StopCoroutine (resetCoroutine);
		_audioSource.Stop();
	}

	public void voiceEvent(){

		_audioSource.PlayOneShot (_clipList[_clipIndex]);
		resetCoroutine = resetAnim (_clipList [_clipIndex].length);
		StartCoroutine (resetCoroutine);
		_clipIndex++;
		if (_clipIndex >= _clipList.Count)
			_clipIndex = 0;
	}

	IEnumerator resetAnim(float time){
	
		yield return new WaitForSeconds (time);
		_animator.SetTrigger ("end");
	}


    public void capitaineEvent(string eventName)
    {
        StartCoroutine(capitaineEventCoroutine(eventName));
    }



    public IEnumerator capitaineEventCoroutine(string eventName)
    {
		//interrupt ();

        if (eventName == "toucher_gentiment")
        {
            _touchCount++;
            if (_touchCount > 1)
            {
                yield return StartCoroutine(XmlManager.launchEventCoroutine("toucher_gentiment_2", "capitaine"));
                this.GetComponent<RadialMenuScript>().buttonList[1].GetComponent<RadialMenuScript>().buttonList[0].desactive();
                scaryValue += 20;
            }
            else
            {
                yield return StartCoroutine(XmlManager.launchEventCoroutine("toucher_gentiment", "capitaine"));
                this.GetComponent<RadialMenuScript>().buttonList[1].GetComponent<RadialMenuScript>().buttonList[0].gameObject.GetComponent<SpriteRenderer>().sprite = _touchSprite2;
            }
        }
        else if (eventName == "parler_moqueur")
        {
            _talkCount++;
            if (_talkCount > 1)
            {
                yield return StartCoroutine(XmlManager.launchEventCoroutine("parler_moqueur_2", "capitaine"));
                this.GetComponent<RadialMenuScript>().buttonList[0].GetComponent<RadialMenuScript>().buttonList[2].desactive();
                scaryValue += 20;
            }
            else
            {
                yield return StartCoroutine(XmlManager.launchEventCoroutine("parler_moqueur", "capitaine"));
                 this.GetComponent<RadialMenuScript>().buttonList[0].GetComponent<RadialMenuScript>().buttonList[2].gameObject.GetComponent<SpriteRenderer>().sprite = _talkSprite2;
            }
        }
        else if (eventName == "parler_mechamment")
        {
            yield return StartCoroutine(XmlManager.launchEventCoroutine("parler_mechamment", "capitaine"));
            scaryValue += 0;
        }
        else if (eventName == "parler_gentiment")
        {
            yield return StartCoroutine(XmlManager.launchEventCoroutine("parler_gentiment", "capitaine"));
            scaryValue += 0;
        }
        else if (eventName == "toucher_mechamment")
        {
            yield return StartCoroutine(XmlManager.launchEventCoroutine("toucher_mechamment", "capitaine"));
            scaryValue += 0;
        }
        else if (eventName == "faire_peur")
        {
            yield return StartCoroutine(XmlManager.launchEventCoroutine("faire_peur", "capitaine"));

            scaryValue += 60;

            if (scaryValue >= 100)
            {
                yield return StartCoroutine(XmlManager.launchEventCoroutine("fuite", "capitaine"));
                goAway();
            }
            else
            {
                yield return StartCoroutine(XmlManager.launchEventCoroutine("faire_peur_suite", "capitaine"));
                yield break;
            }
        }

        if (scaryValue >= 100)
        {
            yield return StartCoroutine(XmlManager.launchEventCoroutine("fuite", "capitaine"));
            goAway();
        }
        yield break;
    }




    public static void goAway()
    {
        LoadingScreen.instance.loadLevel("Scene 2 - Colombine");
    }
}