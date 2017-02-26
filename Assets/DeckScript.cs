using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class DeckScript : MonoBehaviour{
	public int Page = 1;
	public int Deckpage =1;
	public int DeckSize = 10;
	public int DeckSizeOld = 9;
	public PlayerDataScript.Kreatur[] Entwurf;
	public int EntwurfSize = 5;
	public int EntwurfSizeOld=5;
	public GameObject Button;
	public bool Ausgefuehrt = false;
	public Transform ButtonOnMouseOverPanel;

	public bool Level1Shown=true;
	public bool Level2Shown=true;
	public bool Level3Shown=true;
	public bool Level4Shown=true;
	
	public Vector3[] Kreas = new Vector3[50];
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {


		int maxDeckPage = 1;
		for (int i=0; i<GameObject.FindGameObjectsWithTag("DeckButton").Length; i++) {
			if(maxDeckPage<Mathf.Floor((GameObject.FindGameObjectsWithTag("DeckButton")[i].GetComponent<DeckButtonScript>().NummerInDeck-1)/24f)+1){
				maxDeckPage=Mathf.FloorToInt((GameObject.FindGameObjectsWithTag("DeckButton")[i].GetComponent<DeckButtonScript>().NummerInDeck-1)/24f+1);
			}
		}
		if (Deckpage>maxDeckPage) {
			Deckpage = maxDeckPage;
		}
		gameObject.transform.FindChild ("DeckPage").gameObject.GetComponent<UnityEngine.UI.Text> ().text = Deckpage.ToString () + "/" + maxDeckPage;
		int maxRestPage = 1;
		for (int i=0; i<GameObject.FindGameObjectsWithTag("DeckButton").Length; i++) {
			if(GameObject.FindGameObjectsWithTag("DeckButton")[i].GetComponent<DeckButtonScript>().NummerInDeck==0){
				if(maxRestPage<Mathf.Floor((GameObject.FindGameObjectsWithTag("DeckButton")[i].GetComponent<DeckButtonScript>().Nummer-1)/24f)+1){
					maxRestPage=Mathf.FloorToInt((GameObject.FindGameObjectsWithTag("DeckButton")[i].GetComponent<DeckButtonScript>().Nummer-1)/24f+1);
				}
			}
		}
		if (Page > maxRestPage) {
			Page = maxRestPage;
		}
		gameObject.transform.FindChild ("RestPage").gameObject.GetComponent<UnityEngine.UI.Text> ().text = Page.ToString () + "/" + maxRestPage;
		EntwurfSize = 0;
		for (int i=0; i<GameObject.FindGameObjectsWithTag("DeckButton").Length; i++) {
			if(GameObject.FindGameObjectsWithTag("DeckButton")[i].GetComponent<DeckButtonScript>().NummerInDeck!=0){
				EntwurfSize++;
			}
		}
		transform.FindChild ("CardCount").GetComponent<UnityEngine.UI.Text> ().text = EntwurfSize.ToString() + "/" + DeckSize.ToString();

		if (GameObject.Find ("PlayerData").GetComponent<PlayerDataScript> ().Level <= 20) {
			DeckSize=GameObject.Find ("PlayerData").GetComponent<PlayerDataScript> ().Level+10;
		} else {
			DeckSize=30;
		}
		if (DeckSize != DeckSizeOld) {
			//Array.Resize(ref GameObject.Find ("PlayerData").GetComponent<PlayerDataScript> ().Deck,DeckSize);
		}
		if (!Ausgefuehrt) {
			for (int i=0; i<GameObject.Find("PlayerData").GetComponent<PlayerDataScript>().Besitz.Length; i++) {
				if(GameObject.Find("PlayerData").GetComponent<PlayerDataScript>().Besitz[i].Anzahl>GameObject.Find("PlayerData").GetComponent<PlayerDataScript>().Deck[i].Anzahl){
					Debug.Log("Test");
					for(int x=1; x<=GameObject.Find("PlayerData").GetComponent<PlayerDataScript>().Besitz[i].Anzahl-GameObject.Find("PlayerData").GetComponent<PlayerDataScript>().Deck[i].Anzahl;x++){
						//Debug.Log("Im Besitz, aber nicht im Deck: " + i.ToString());
						GameObject KnopfObjekt = Instantiate(Button) as GameObject;
						KnopfObjekt.transform.SetParent(GameObject.Find("Deck").transform);
						KnopfObjekt.transform.localScale=new Vector3(KnopfObjekt.transform.localScale.x*Screen.width/1500f*1.5f, KnopfObjekt.transform.localScale.y*Screen.height/600f*1.3f);
						KnopfObjekt.GetComponent<DeckButtonScript>().NummerInDeck=1;
						KnopfObjekt.tag="DeckButton";
						//EventSystem Eve = KnopfObjekt.AddComponent<UnityEngine.EventSystems.EventSystem>();
						KnopfObjekt.GetComponent<DeckButtonScript>().OnMouseOverPanel=ButtonOnMouseOverPanel;
						KnopfObjekt.GetComponent<DeckButtonScript>().ChangeDeck();
						KnopfObjekt.GetComponent<DeckButtonScript>().Kreatur=GameObject.Find("PlayerData").GetComponent<PlayerDataScript>().Besitz[i];
                        Debug.Log("Klasse: "+GameObject.Find("PlayerData").GetComponent<PlayerDataScript>().Besitz[i].Klasse+"; Nummer: "+ GameObject.Find("PlayerData").GetComponent<PlayerDataScript>().Besitz[i].NummerInKlasse+"; Level:"+ GameObject.Find("PlayerData").GetComponent<PlayerDataScript>().Besitz[i].Level);
						KnopfObjekt.transform.FindChild("Text").gameObject.GetComponent<UnityEngine.UI.Text>().text=GameObject.Find("PlayerData").GetComponent<PlayerDataScript>().Creatures[i].Name;
						switch (GameObject.Find("PlayerData").GetComponent<PlayerDataScript>().Creatures[i].Level){
						case 1:
							KnopfObjekt.GetComponent<UnityEngine.UI.Button>().image.color=Color.white;
							break;
						case 2:
							KnopfObjekt.GetComponent<UnityEngine.UI.Button>().image.color=Color.green;
							break;
						case 3:
							KnopfObjekt.GetComponent<UnityEngine.UI.Button>().image.color=Color.magenta;
							break;
						case 4:
							KnopfObjekt.GetComponent<UnityEngine.UI.Button>().image.color=new Color(251f/255f,166f/255f,54f/255f);
							break;
						}
					}
				}
				if(GameObject.Find("PlayerData").GetComponent<PlayerDataScript>().Deck[i].Anzahl>0){
					for(int x=1; x<=GameObject.Find("PlayerData").GetComponent<PlayerDataScript>().Deck[i].Anzahl; x++){
						//Debug.Log("Im Deck: "+i);
						GameObject KnopfObjekt = Instantiate(Button) as GameObject;
						KnopfObjekt.transform.SetParent(GameObject.Find("Deck").transform);
						KnopfObjekt.GetComponent<DeckButtonScript>().NummerInDeck=0;
						KnopfObjekt.tag="DeckButton";
						KnopfObjekt.transform.localScale=new Vector3(KnopfObjekt.transform.localScale.x*Screen.width/1500f*1.5f, KnopfObjekt.transform.localScale.y*Screen.height/600f*1.3f);
						KnopfObjekt.GetComponent<DeckButtonScript>().OnMouseOverPanel=ButtonOnMouseOverPanel;
						/*EventTrigger trigg=KnopfObjekt.AddComponent<EventTrigger>();
						Debug.Log(trigg.gameObject.name);
						EventTrigger.Entry entry = new EventTrigger.Entry();
						entry.eventID=EventTriggerType.PointerEnter;
						entry.callback.AddListener( (eventData) => { KnopfObjekt.GetComponent<DeckButtonScript>().MouseEnter(); } );
						trigg.OnPointerEnter(entry);*/
						
						//EventSystem Eve = KnopfObjekt.AddComponent<UnityEngine.EventSystems.EventSystem>();


						KnopfObjekt.GetComponent<DeckButtonScript>().ChangeDeck();
						KnopfObjekt.GetComponent<DeckButtonScript>().Kreatur=GameObject.Find("PlayerData").GetComponent<PlayerDataScript>().Deck[i];
						KnopfObjekt.transform.FindChild("Text").gameObject.GetComponent<UnityEngine.UI.Text>().text=GameObject.Find("PlayerData").GetComponent<PlayerDataScript>().Creatures[i].Name;
						//KnopfObjekt.transform.SetParent(GameObject.Find("Canvas").transform);
						switch (GameObject.Find("PlayerData").GetComponent<PlayerDataScript>().Creatures[i].Level){
						case 1:
							KnopfObjekt.GetComponent<UnityEngine.UI.Button>().image.color=Color.white;
							break;
						case 2:
							KnopfObjekt.GetComponent<UnityEngine.UI.Button>().image.color=Color.green;
							break;
						case 3:
							KnopfObjekt.GetComponent<UnityEngine.UI.Button>().image.color=Color.magenta;
							break;
						case 4:
							KnopfObjekt.GetComponent<UnityEngine.UI.Button>().image.color=new Color((251),(166),(54));
							break;
						}
					}
					GameObject.Find("Canvas").GetComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;

				}
			}
			Ausgefuehrt=true;
		}
	}
	public void DeckpageChange(int Number){
		int maxDeckPage = 1;
		for (int i=0; i<GameObject.FindGameObjectsWithTag("DeckButton").Length; i++) {
			if(maxDeckPage<Mathf.Floor((GameObject.FindGameObjectsWithTag("DeckButton")[i].GetComponent<DeckButtonScript>().NummerInDeck-1)/24f)+1){
				maxDeckPage=Mathf.FloorToInt((GameObject.FindGameObjectsWithTag("DeckButton")[i].GetComponent<DeckButtonScript>().NummerInDeck-1)/24f+1);
			}
		}
		if (Deckpage + Number > 0 && Deckpage + Number <= maxDeckPage) {
			Deckpage+=Number;
		}
	}
	public void RestpageChange(int Number){
		int maxRestPage=1;
		for (int i=0; i<GameObject.FindGameObjectsWithTag("DeckButton").Length; i++) {
			if(GameObject.FindGameObjectsWithTag("DeckButton")[i].GetComponent<DeckButtonScript>().NummerInDeck==0){
				if(maxRestPage<Mathf.Floor((GameObject.FindGameObjectsWithTag("DeckButton")[i].GetComponent<DeckButtonScript>().Nummer-1)/24f)+1){
					maxRestPage=Mathf.FloorToInt((GameObject.FindGameObjectsWithTag("DeckButton")[i].GetComponent<DeckButtonScript>().Nummer-1)/24f+1);
				}
			}
		}
		if (Page + Number > 0 && Page + Number <= maxRestPage) {
			Page+=Number;
		}
	}
	public void SaveDeck(){
		if (EntwurfSize > 0) {
			/*int Counter = 0;
			PlayerDataScript PD = GameObject.Find("PlayerData").GetComponent<PlayerDataScript>();
			Array.Resize (ref GameObject.Find ("PlayerData").GetComponent<PlayerDataScript> ().Deck, EntwurfSize);
			for (int i=0; i<GameObject.FindGameObjectsWithTag("DeckButton").Length; i++) {
				GameObject obj = GameObject.FindGameObjectsWithTag("DeckButton")[i];
				if(obj.GetComponent<DeckButtonScript>().NummerInDeck>0){
					Kreas[Counter]= new Vector3(obj.GetComponent<DeckButtonScript>().Kreatur.Klasse,obj.GetComponent<DeckButtonScript>().Kreatur.NummerInKlasse,obj.GetComponent<DeckButtonScript>().Kreatur.Level);
					bool Existing = false;
					int Index=0;
					for(int j=0; j<PD.Deck.Length; j++){
						if(PD.Deck[j].Klasse==PD.Besitz[i].Klasse&&PD.Deck[j].Level==PD.Besitz[i].Level&&PD.Deck[j].NummerInKlasse==PD.Besitz[i].NummerInKlasse){
							Existing=true;
							Index = j;
						}
					}
					if(!Existing){
						Debug.Log(obj.GetComponent<DeckButtonScript>().NummerInDeck-1+","+i);
						PD.Deck[obj.GetComponent<DeckButtonScript>().NummerInDeck-1]=PD.Besitz[i];
					} else {
						PD.Deck[Index].Anzahl++;
						Array.Resize(ref PD.Deck, PD.Deck.Length-1);
					}
				}
			}
			for(int i=0; i<PD.Deck.Length; i++){
				Debug.Log("Klasse "+PD.Deck[i].Klasse.ToString() + ", Nummer "+PD.Deck[i].NummerInKlasse.ToString()+", Level " + PD.Deck[i].Level.ToString()+", Anzahl " + PD.Deck[i].Anzahl.ToString());
			}*/
			GameObject.Find("PlayerData").GetComponent<PlayerDataScript>().Deck=new PlayerDataScript.Kreatur[4*6*4];
			for(int i=0;i<GameObject.FindGameObjectsWithTag("DeckButton").Length;i++){
				DeckButtonScript Button = GameObject.FindGameObjectsWithTag("DeckButton")[i].GetComponent<DeckButtonScript>();
				if(Button.NummerInDeck!=0){
					/*for(int j=0;j<GameObject.Find("PlayerData").GetComponent<PlayerDataScript>().Deck.Length;j++){
						Debug.Log(Button.Kreatur.Klasse+","+Button.Kreatur.NummerInKlasse+","+Button.Kreatur.Level+" | "+GameObject.Find("PlayerData").GetComponent<PlayerDataScript>().Deck[j].Klasse+","+GameObject.Find("PlayerData").GetComponent<PlayerDataScript>().Deck[j].NummerInKlasse+","+GameObject.Find("PlayerData").GetComponent<PlayerDataScript>().Deck[j].Level);
						if(GameObject.Find("PlayerData").GetComponent<PlayerDataScript>().Deck[j].Klasse==Button.Kreatur.Klasse&&GameObject.Find("PlayerData").GetComponent<PlayerDataScript>().Deck[j].NummerInKlasse==Button.Kreatur.NummerInKlasse&&GameObject.Find("PlayerData").GetComponent<PlayerDataScript>().Deck[j].Level==Button.Kreatur.Level){
							GameObject.Find("PlayerData").GetComponent<PlayerDataScript>().Deck[i].Anzahl+=1;
						}
					}*/
					int AKlassen = GameObject.Find("PlayerData").GetComponent<PlayerDataScript>().AnzahlKlassen;
					int ALevel = GameObject.Find("PlayerData").GetComponent<PlayerDataScript>().AnzahlLevel;
					int AproKlasse = GameObject.Find("PlayerData").GetComponent<PlayerDataScript>().AnzahlproKlasse;
					//Debug.Log("Klasse "+Button.Kreatur.Klasse);
					//Debug.Log("Nummer "+Button.Kreatur.NummerInKlasse);
					//Debug.Log("Level "+Button.Kreatur.Level);
					int DeckID=((Button.Kreatur.Klasse-1)*ALevel*AproKlasse+(Button.Kreatur.NummerInKlasse-1)*ALevel+Button.Kreatur.Level)-1;
					//Debug.Log("DeckID: "+DeckID);
					GameObject.Find("PlayerData").GetComponent<PlayerDataScript>().Deck[DeckID].Anzahl+=1;
				}
			}
			GameObject.Find("PlayerData").GetComponent<PlayerDataScript>().LogDeck(GameObject.Find("PlayerData").GetComponent<PlayerDataScript>().Deck);
		}
	}
	public void SwitchVisibility(int Level){
		switch (Level) {
		case 1:
			if(Level1Shown){
				for(int i=0;i<GameObject.FindGameObjectsWithTag("DeckButton").Length;i++){
					if(GameObject.FindGameObjectsWithTag("DeckButton")[i].GetComponent<DeckButtonScript>().Kreatur.Level==1&&GameObject.FindGameObjectsWithTag("DeckButton")[i].GetComponent<DeckButtonScript>().NummerInDeck==0){
						GameObject go = GameObject.FindGameObjectsWithTag("DeckButton")[i];
						go.GetComponent<DeckButtonScript>().Activated=false;
						go.transform.localPosition = new Vector3 (10000, 10000);
						//go.GetComponent<DeckButtonScript>().Nummer=0;
					}
				}
				int RunningInt=0;
				for(int i=0;i<GameObject.FindGameObjectsWithTag("DeckButton").Length;i++){
					if(GameObject.FindGameObjectsWithTag("DeckButton")[RunningInt].GetComponent<DeckButtonScript>().Kreatur.Level==1&&GameObject.FindGameObjectsWithTag("DeckButton")[RunningInt].GetComponent<DeckButtonScript>().NummerInDeck==0){
						GameObject.FindGameObjectsWithTag("DeckButton")[RunningInt].tag="DisabledDeckButton";
					} else {
						RunningInt++;
					}
				}
				GameObject.Find("Disable/EnableLevel1").transform.FindChild("Text").GetComponent<Text>().text=" ";
			} else {
				for(int i=0;i<GameObject.FindGameObjectsWithTag("DisabledDeckButton").Length;i++){
					if(GameObject.FindGameObjectsWithTag("DisabledDeckButton")[i].GetComponent<DeckButtonScript>().Kreatur.Level==1){
						GameObject.FindGameObjectsWithTag("DisabledDeckButton")[i].GetComponent<DeckButtonScript>().Activated=true;
					}
				}
				int RunningInt=0;
				for(int i=0;i<GameObject.FindGameObjectsWithTag("DisabledDeckButton").Length;i++){
					if(GameObject.FindGameObjectsWithTag("DisabledDeckButton")[RunningInt].GetComponent<DeckButtonScript>().Kreatur.Level==1){
						Debug.Log(RunningInt);
						GameObject.FindGameObjectsWithTag("DisabledDeckButton")[RunningInt].tag="DeckButton";
					} else {
						RunningInt++;
					}
				}
				GameObject.Find("Disable/EnableLevel1").transform.FindChild("Text").GetComponent<Text>().text="X";
			}




			Level1Shown = !Level1Shown;
			break;




		case 2:
			if(Level2Shown){
				for(int i=0;i<GameObject.FindGameObjectsWithTag("DeckButton").Length;i++){
					if(GameObject.FindGameObjectsWithTag("DeckButton")[i].GetComponent<DeckButtonScript>().Kreatur.Level==2&&GameObject.FindGameObjectsWithTag("DeckButton")[i].GetComponent<DeckButtonScript>().NummerInDeck==0){
						GameObject go = GameObject.FindGameObjectsWithTag("DeckButton")[i];
						go.GetComponent<DeckButtonScript>().Activated=false;
						go.transform.localPosition = new Vector3 (10000, 10000);
						//go.GetComponent<DeckButtonScript>().Nummer=0;
					}
				}
				int RunningInt=0;
				for(int i=0;i<GameObject.FindGameObjectsWithTag("DeckButton").Length;i++){
					if(GameObject.FindGameObjectsWithTag("DeckButton")[RunningInt].GetComponent<DeckButtonScript>().Kreatur.Level==2&&GameObject.FindGameObjectsWithTag("DeckButton")[RunningInt].GetComponent<DeckButtonScript>().NummerInDeck==0){
						GameObject.FindGameObjectsWithTag("DeckButton")[RunningInt].tag="DisabledDeckButton";
					} else {
						RunningInt++;
					}
				}
				GameObject.Find("Disable/EnableLevel2").transform.FindChild("Text").GetComponent<Text>().text=" ";
			} else {
				for(int i=0;i<GameObject.FindGameObjectsWithTag("DisabledDeckButton").Length;i++){
					if(GameObject.FindGameObjectsWithTag("DisabledDeckButton")[i].GetComponent<DeckButtonScript>().Kreatur.Level==2){
						GameObject.FindGameObjectsWithTag("DisabledDeckButton")[i].GetComponent<DeckButtonScript>().Activated=true;
					}
				}
				int RunningInt=0;
				for(int i=0;i<GameObject.FindGameObjectsWithTag("DisabledDeckButton").Length;i++){
					if(GameObject.FindGameObjectsWithTag("DisabledDeckButton")[RunningInt].GetComponent<DeckButtonScript>().Kreatur.Level==2){
						Debug.Log(RunningInt);
						GameObject.FindGameObjectsWithTag("DisabledDeckButton")[RunningInt].tag="DeckButton";
					} else {
						RunningInt++;
					}
				}
				GameObject.Find("Disable/EnableLevel2").transform.FindChild("Text").GetComponent<Text>().text="X";
			}
			Level2Shown=!Level2Shown;
			break;



		case 3:
			if(Level3Shown){
				for(int i=0;i<GameObject.FindGameObjectsWithTag("DeckButton").Length;i++){
					if(GameObject.FindGameObjectsWithTag("DeckButton")[i].GetComponent<DeckButtonScript>().Kreatur.Level==3&&GameObject.FindGameObjectsWithTag("DeckButton")[i].GetComponent<DeckButtonScript>().NummerInDeck==0){
						GameObject go = GameObject.FindGameObjectsWithTag("DeckButton")[i];
						go.GetComponent<DeckButtonScript>().Activated=false;
						go.transform.localPosition = new Vector3 (10000, 10000);
						//go.GetComponent<DeckButtonScript>().Nummer=0;
					}
				}
				int RunningInt=0;
				for(int i=0;i<GameObject.FindGameObjectsWithTag("DeckButton").Length;i++){
					if(GameObject.FindGameObjectsWithTag("DeckButton")[RunningInt].GetComponent<DeckButtonScript>().Kreatur.Level==3&&GameObject.FindGameObjectsWithTag("DeckButton")[RunningInt].GetComponent<DeckButtonScript>().NummerInDeck==0){
						GameObject.FindGameObjectsWithTag("DeckButton")[RunningInt].tag="DisabledDeckButton";
					} else {
						RunningInt++;
					}
				}
				GameObject.Find("Disable/EnableLevel3").transform.FindChild("Text").GetComponent<Text>().text=" ";
			} else {
				for(int i=0;i<GameObject.FindGameObjectsWithTag("DisabledDeckButton").Length;i++){
					if(GameObject.FindGameObjectsWithTag("DisabledDeckButton")[i].GetComponent<DeckButtonScript>().Kreatur.Level==3){
						GameObject.FindGameObjectsWithTag("DisabledDeckButton")[i].GetComponent<DeckButtonScript>().Activated=true;
					}
				}
				int RunningInt=0;
				for(int i=0;i<GameObject.FindGameObjectsWithTag("DisabledDeckButton").Length;i++){
					if(GameObject.FindGameObjectsWithTag("DisabledDeckButton")[RunningInt].GetComponent<DeckButtonScript>().Kreatur.Level==3){
						Debug.Log(RunningInt);
						GameObject.FindGameObjectsWithTag("DisabledDeckButton")[RunningInt].tag="DeckButton";
					} else {
						RunningInt++;
					}
				}
				GameObject.Find("Disable/EnableLevel3").transform.FindChild("Text").GetComponent<Text>().text="X";
			}

			Level3Shown = !Level3Shown;
			break;




		case 4:
			if(Level4Shown){
				for(int i=0;i<GameObject.FindGameObjectsWithTag("DeckButton").Length;i++){
					if(GameObject.FindGameObjectsWithTag("DeckButton")[i].GetComponent<DeckButtonScript>().Kreatur.Level==4&&GameObject.FindGameObjectsWithTag("DeckButton")[i].GetComponent<DeckButtonScript>().NummerInDeck==0){
						GameObject go = GameObject.FindGameObjectsWithTag("DeckButton")[i];
						go.GetComponent<DeckButtonScript>().Activated=false;
						go.transform.localPosition = new Vector3 (10000, 10000);
						//go.GetComponent<DeckButtonScript>().Nummer=0;
					}
				}
				int RunningInt=0;
				for(int i=0;i<GameObject.FindGameObjectsWithTag("DeckButton").Length;i++){
					if(GameObject.FindGameObjectsWithTag("DeckButton")[RunningInt].GetComponent<DeckButtonScript>().Kreatur.Level==4&&GameObject.FindGameObjectsWithTag("DeckButton")[RunningInt].GetComponent<DeckButtonScript>().NummerInDeck==0){
						GameObject.FindGameObjectsWithTag("DeckButton")[RunningInt].tag="DisabledDeckButton";
					} else {
						RunningInt++;
					}
				}
				GameObject.Find("Disable/EnableLevel4").transform.FindChild("Text").GetComponent<Text>().text=" ";
			} else {
				for(int i=0;i<GameObject.FindGameObjectsWithTag("DisabledDeckButton").Length;i++){
					if(GameObject.FindGameObjectsWithTag("DisabledDeckButton")[i].GetComponent<DeckButtonScript>().Kreatur.Level==4){
						GameObject.FindGameObjectsWithTag("DisabledDeckButton")[i].GetComponent<DeckButtonScript>().Activated=true;
					}
				}
				int RunningInt=0;
				for(int i=0;i<GameObject.FindGameObjectsWithTag("DisabledDeckButton").Length;i++){
					if(GameObject.FindGameObjectsWithTag("DisabledDeckButton")[RunningInt].GetComponent<DeckButtonScript>().Kreatur.Level==4){
						Debug.Log(RunningInt);
						GameObject.FindGameObjectsWithTag("DisabledDeckButton")[RunningInt].tag="DeckButton";
					} else {
						RunningInt++;
					}
				}
				GameObject.Find("Disable/EnableLevel4").transform.FindChild("Text").GetComponent<Text>().text="X";
			}
			Level4Shown = !Level4Shown;
			break;
		}
		StartCoroutine (TimePauseForDeckButtonSortAfterDisableLevel());
	}
	public PlayerDataScript.Kreatur[] AddToDeck(int KreaID, int AnzahlNeu, PlayerDataScript.Kreatur[] Deck){
		PlayerDataScript.Kreatur[] Deckx = Deck;
		if (Deckx [KreaID].Anzahl + AnzahlNeu >= 0) {
			Deckx [KreaID].Anzahl += AnzahlNeu;
		} else {
			Deckx [KreaID].Anzahl = 0;
		}
		return Deckx;
	}
	IEnumerator TimePauseForDeckButtonSortAfterDisableLevel(){
		yield return new WaitForSeconds (0.01f);
		int NummerCounter = 1;
		int DeckNummerCounter = 1;
		for (int i=0; i<GameObject.FindGameObjectsWithTag("DeckButton").Length; i++) {
			DeckButtonScript Bt = GameObject.FindGameObjectsWithTag("DeckButton")[i].GetComponent<DeckButtonScript>();
			if(Bt.NummerInDeck==0){
				Bt.Nummer=NummerCounter;
				NummerCounter++;
			} else {
				Bt.NummerInDeck=DeckNummerCounter;
				DeckNummerCounter++;
			}
		}
	}
}
