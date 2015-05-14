using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour {


	public GameObject loadingScreen;
	public GameObject loadingPercentage;

	private int percentage = 0;

	// Use this for initialization
	void Start () {
		loadingScreen.SetActive (false);
	}

	
	// Update is called once per frame
	void Update () {
	
	}

	public void loadLevel(string levelName){
		StartCoroutine (displayLoadingScreen (levelName));
	}

	 IEnumerator displayLoadingScreen(string levelName){

		//affichage de l'ecran de chargement
		loadingScreen.SetActive (true);
		//Initialisation du poucentage a 0%
		loadingPercentage.GetComponent<UnityEngine.UI.Text> ().text = percentage.ToString() + "%";

		AsyncOperation async = Application.LoadLevelAsync (levelName);

		//tant que le chargement de la scene n'est pas finis
		while (!async.isDone) {
			//on recupere la progression du la tache
			percentage =  (int) ( async.progress * 100 );
			loadingPercentage.GetComponent<UnityEngine.UI.Text> ().text = percentage.ToString() + "%";

			yield return null;
		}

	}

	 
}
