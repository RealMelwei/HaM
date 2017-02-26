using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DeckButtonScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler{
	public int Nummer;
	public int NummerInDeck=0;
	public int DeckSeite=1;
	public int Page = 1;
	public float Line=1;
	public int Spalte=1;
	public int Zeilen=6;
	public int Spalten = 3;
	public bool Activated = true;
	public PlayerDataScript.Kreatur Kreatur;
	public Transform OnMouseOverPanel;
	public UnityEngine.Transform MouseOverPanel;
	public bool ActivatedOld=true;
	public int Level=0;

	// Use this for initialization
	void Start () {


	}
	
	// Update is called once per frame
	void Update () {
		Level = Kreatur.Level;
		/*if (gameObject.GetComponent<UnityEngine.UI.Button>().image.color == gameObject.GetComponent<UnityEngine.UI.Button> ().colors.normalColor) {
			Debug.Log("TEST");
			MouseEnter ();
		} else {
			Debug.LogError("Noch nicht mal der normalen Farbe entsprechend....");
			//Debug.Log (GUI.color.r + " " + GUI.color.g + " " + GUI.color.b);
		}*/
		if (ActivatedOld && !Activated) {

		}

		if (Activated) {
			gameObject.tag = "DeckButton";
			if (NummerInDeck == 0) {
				int BpP = Zeilen * Spalten;
				Page = Mathf.FloorToInt ((Nummer - 1) / BpP) + 1;
				float NummerAufSeite = Nummer - BpP * (Page - 1);
				Line = Mathf.FloorToInt ((NummerAufSeite - 1) / Spalten) + 1;
				Spalte = (Nummer - 1) % Spalten + 1;
				if (Page == GameObject.Find ("Deck").GetComponent<DeckScript> ().Page) {
					transform.position = new Vector3 (Screen.width * 0.3f / Spalten * (Spalte - 1) + Screen.width * 0.55f, Screen.height * 0.75f - Screen.height * 0.5f / Zeilen * (Line - 1));
				} else {
					transform.position = new Vector3 (10000, 10000);
				}
			} else {
				int BpP = Zeilen * Spalten;
				DeckSeite = Mathf.FloorToInt ((NummerInDeck - 1) / BpP) + 1;
				float NummerAufSeite = NummerInDeck - BpP * (DeckSeite - 1);
				Line = Mathf.FloorToInt ((NummerAufSeite - 1) / Spalten) + 1;
				Spalte = (NummerInDeck - 1) % Spalten + 1;
				if (DeckSeite == GameObject.Find ("Deck").GetComponent<DeckScript> ().Deckpage) {
					transform.position = new Vector3 (Screen.width * 0.3f / Spalten * (Spalte - 1) + Screen.width * 0.25f, Screen.height * 0.75f - Screen.height * 0.5f / Zeilen * (Line - 1));
				} else {
					transform.position = new Vector3 (10000, 10000);
				}
			}
		} else {
			if(NummerInDeck==0){
				gameObject.tag = "DisabledDeckButton";
				gameObject.transform.localPosition = new Vector3 (10000, 10000);
				Nummer=0;
			} else {
				/*int BpP = Zeilen * Spalten;
				DeckSeite = Mathf.FloorToInt ((NummerInDeck - 1) / BpP) + 1;
				float NummerAufSeite = NummerInDeck - BpP * (DeckSeite - 1);
				Line = Mathf.FloorToInt ((NummerAufSeite - 1) / Spalten) + 1;
				Spalte = (NummerInDeck - 1) % Spalten + 1;
				if (DeckSeite == GameObject.Find ("Deck").GetComponent<DeckScript> ().Deckpage) {
					transform.position = new Vector3 (Screen.width * 0.3f / Spalten * (Spalte - 1) + Screen.width * 0.25f, Screen.height * 0.75f - Screen.height * 0.5f / Zeilen * (Line - 1));
				} else {
					transform.position = new Vector3 (10000, 10000);
				}*/
			}
		}
	}
	public void ChangeDeck(){
		if (NummerInDeck > 0) {
			NummerInDeck = 0;
			int DeckNummer = 1;
			int NichtDeckNummer = 1;
			for (int i=0; i<GameObject.FindGameObjectsWithTag("DeckButton").Length; i++) {
				if (GameObject.FindGameObjectsWithTag ("DeckButton") [i].GetComponent<DeckButtonScript> ().NummerInDeck > 0) {
					GameObject.FindGameObjectsWithTag ("DeckButton") [i].GetComponent<DeckButtonScript> ().NummerInDeck = DeckNummer;
					DeckNummer += 1;
				} else {
					GameObject.FindGameObjectsWithTag ("DeckButton") [i].GetComponent<DeckButtonScript> ().Nummer = NichtDeckNummer;
					NichtDeckNummer += 1;
				}
			}
		} else {
			if (GameObject.Find ("Deck").GetComponent<DeckScript> ().EntwurfSize + 1 <= GameObject.Find ("Deck").GetComponent<DeckScript> ().DeckSize) {
				NummerInDeck = 1;
				int DeckNummer = 1;
				int NichtDeckNummer = 1;
				for (int i=0; i<GameObject.FindGameObjectsWithTag("DeckButton").Length; i++) {
					if (GameObject.FindGameObjectsWithTag ("DeckButton") [i].GetComponent<DeckButtonScript> ().NummerInDeck > 0) {
						GameObject.FindGameObjectsWithTag ("DeckButton") [i].GetComponent<DeckButtonScript> ().NummerInDeck = DeckNummer;
						DeckNummer += 1;
					} else {
						GameObject.FindGameObjectsWithTag ("DeckButton") [i].GetComponent<DeckButtonScript> ().Nummer = NichtDeckNummer;
						NichtDeckNummer += 1;
					}
				}
			}
		}
	}
	public void MouseEnter(){
		//Debug.Log ("MouseOver!");
		MouseOverPanel = Instantiate (OnMouseOverPanel) as Transform;
		MouseOverPanel.SetParent(GameObject.Find ("Deck").transform);

		if (NummerInDeck == 0) {
			MouseOverPanel.gameObject.GetComponent<RectTransform> ().transform.localPosition = new Vector3 (160 + 75 * (Spalte - 1), 0, 0);
		} else {
			MouseOverPanel.gameObject.GetComponent<RectTransform> ().transform.localPosition = new Vector3 (75 * (Spalte - 2), 0, 0);
		}
		MouseOverPanel.localScale = new Vector3 (MouseOverPanel.localScale.x * Screen.width / 987, MouseOverPanel.localScale.x * Screen.height / 468);
		//MouseOverPanel.transform.position = new Vector3 (160, 0, 0);

		int AKlassen = GameObject.Find("PlayerData").GetComponent<PlayerDataScript>().AnzahlKlassen;
		int ALevel = GameObject.Find("PlayerData").GetComponent<PlayerDataScript>().AnzahlLevel;
		int AproKlasse = GameObject.Find("PlayerData").GetComponent<PlayerDataScript>().AnzahlproKlasse;
		Debug.Log("Klasse "+Kreatur.Klasse);
		Debug.Log("Nummer "+Kreatur.NummerInKlasse);
		Debug.Log("Level "+Kreatur.Level);
		int DeckID=((Kreatur.Klasse-1)*ALevel*AproKlasse+(Kreatur.NummerInKlasse-1)*ALevel+Kreatur.Level)-1;
		Debug.Log ("DecKID " + DeckID);

		MouseOverPanel.FindChild ("Name").GetComponent<Text> ().text = GameObject.Find ("PlayerData").GetComponent<PlayerDataScript> ().Creatures [DeckID].Name;

		Text Descript = MouseOverPanel.FindChild("Description").GetComponent<Text>();

		string Description = "\n\n";
		PlayerDataScript.KreaturenDaten Krea = GameObject.Find ("PlayerData").GetComponent<PlayerDataScript> ().Creatures [DeckID];
		for (int i=0; i<Krea.Skills.Length; i++) {
			if(Krea.Skills[i]>0){
				string[] NextSkill=GameObject.Find("PlayerData").GetComponent<PlayerDataScript>().GetDescriptionOfSkill(i+1,Krea.Skills[i]);
				Description+=NextSkill[0]+": "+NextSkill[1]+"\n\n";
			}
		}

		MouseOverPanel.FindChild ("Description").GetComponent<Text> ().text = Description;
		MouseOverPanel.FindChild ("Text").GetComponent<Text> ().text = "Angriff     " + Krea.Angriff + " | " + Krea.Leben + "     Leben";
		GameObject.Find("Canvas").GetComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
	}
	public void MouseExit(){
		//Destroy (MouseOverPanel.GetComponent<UnityEngine.UI.Image>());
		Destroy (MouseOverPanel.gameObject);
	}
	public void OnPointerEnter(PointerEventData eventdata){
		MouseEnter ();
	}
	public void OnPointerExit(PointerEventData eventdata){
		MouseExit ();
	}
}
