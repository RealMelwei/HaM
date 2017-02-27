using UnityEngine;
using System.Collections;
using System;
using UnityEngine.EventSystems;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.Text;

public class PlayerDataScript : MonoBehaviour {
	public int Silber = 0;
	public int Gold = 0;
	public int EP = 1;
	public int Level = 1;
	public struct Kreatur{
		public int Klasse;
		public int NummerInKlasse;
		public int Level;
		public int Anzahl;
	}
	public struct KreaturenDaten{
		public int Klasse;
		public int Nummer;
		public int Level;
		public string Name;
		public int[] Skills;
		public int Leben;
		public int Angriff;
	}
	public struct SkillInfo{
		public string Name;
		public string Description;
		public int n;
	}
	public Kreatur[] Besitz = new Kreatur[6*4*6];
	public Kreatur[] Deck = new Kreatur[6*4*6];
	public int AnzahlKlassen = 6;
	public int AnzahlLevel = 4;
	public int AnzahlproKlasse = 6;
	public KreaturenDaten[] Creatures = new KreaturenDaten[6*4*6];
	public int SkillNumber=14;
	public SkillInfo[] Skills = new SkillInfo[15];

	public int StandardGold=50;
	public int StandardSilber=1000;
	public int StandardEP=1;
	public Kreatur[] StandardDeck;
	public Kreatur[] StandardBesitz;
	public bool SetValuesToStandard=false;
	public bool LogMyDeck=false;
	public bool LogBesitz=false;

	public bool AddToDeck=false;
	public int AddToDeckID=1;
	public int AddToDeckAnzahl=1;
	//Fach 1: Klasse, Fach 2: Truppenzahl in Klasse, Fach 3: Level, Fach 4: Fähigkeitsnummer, Eingabe: Fähigkeitswert
	/*Fähigkeitsnummern:
	0:Regeneration
	1:Siegessicherheit
	2:Explosiv
	3:Rüstung
	4:Blockade
	5:Bogen
	6:Auge um Auge
	7:Giftig
	8:Kavallerie
	9:Rachsucht
	10:Schneller Schlag
	11:Todesstoß
	12:Verflucht
	13:Wildheit
	*/
	void Start () {
		Array.Resize (ref Skills, SkillNumber + 1);
		Array.Resize (ref Besitz, AnzahlKlassen * AnzahlLevel * AnzahlproKlasse);
		Array.Resize (ref Deck, AnzahlKlassen * AnzahlLevel * AnzahlproKlasse);
		Array.Resize (ref Creatures, AnzahlKlassen * AnzahlLevel * AnzahlproKlasse);
		
		Array.Resize (ref StandardBesitz, AnzahlKlassen * AnzahlLevel * AnzahlproKlasse);
		Array.Resize (ref StandardDeck, AnzahlKlassen * AnzahlLevel * AnzahlproKlasse);
		for (int i=1; i<=AnzahlLevel*AnzahlKlassen*AnzahlproKlasse; i++) {
			Besitz [i - 1].Klasse = Mathf.CeilToInt ((i - 1) / (AnzahlLevel * AnzahlproKlasse)) + 1;
			Besitz [i - 1].Level = (i - 1) % AnzahlLevel + 1;
			Besitz [i - 1].NummerInKlasse = ((i - 1) / AnzahlLevel) % AnzahlproKlasse + 1;
			Besitz[i-1].Anzahl=0;
		}
		for (int i=1; i<=AnzahlLevel*AnzahlKlassen*AnzahlproKlasse; i++) {
			Deck [i - 1].Klasse = Mathf.CeilToInt ((i - 1) / (AnzahlLevel * AnzahlproKlasse)) + 1;
			Deck [i - 1].Level = (i - 1) % AnzahlLevel + 1;
			Deck [i - 1].NummerInKlasse = ((i - 1) / AnzahlLevel) % AnzahlproKlasse + 1;
			Deck[i-1].Anzahl=0;
		}
        for (int i = 1; i <= AnzahlLevel * AnzahlKlassen * AnzahlproKlasse; i++)
        {
            StandardBesitz[i - 1].Klasse = Mathf.CeilToInt((i - 1) / (AnzahlLevel * AnzahlproKlasse)) + 1;
            StandardBesitz[i - 1].Level = (i - 1) % AnzahlLevel + 1;
            StandardBesitz[i - 1].NummerInKlasse = ((i - 1) / AnzahlLevel) % AnzahlproKlasse + 1;
            StandardBesitz[i - 1].Anzahl = 0;
        }
        for (int i = 1; i <= AnzahlLevel * AnzahlKlassen * AnzahlproKlasse; i++)
        {
            StandardDeck[i - 1].Klasse = Mathf.CeilToInt((i - 1) / (AnzahlLevel * AnzahlproKlasse)) + 1;
            StandardDeck[i - 1].Level = (i - 1) % AnzahlLevel + 1;
            StandardDeck[i - 1].NummerInKlasse = ((i - 1) / AnzahlLevel) % AnzahlproKlasse + 1;
            StandardDeck[i - 1].Anzahl = 0;
        }

        StandardBesitz [0].Anzahl = 8;
		StandardBesitz [1].Anzahl=2;
		StandardBesitz [2].Anzahl = 2;
		StandardBesitz [3].Anzahl=2;
		StandardBesitz [4].Anzahl = 2;
		StandardBesitz [19].Anzahl = 2;
		StandardDeck [0].Anzahl = 2;
		StandardDeck [1].Anzahl=1;
		StandardDeck [4].Anzahl = 2;
		LoadPlayerData ();

		for (int i=1; i<=AnzahlLevel*AnzahlKlassen*AnzahlproKlasse; i++) {
			Creatures [i - 1].Klasse = Mathf.CeilToInt ((i-1)/(AnzahlLevel*AnzahlproKlasse))+1;
			Creatures [i - 1].Level = (i-1)%AnzahlLevel+1;
			Creatures [i - 1].Nummer = ((i-1)/AnzahlLevel)%AnzahlproKlasse+1;
			Creatures[i-1].Skills = new int[15] {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};
			switch(100*Creatures[i-1].Klasse+10*Creatures[i-1].Nummer+Creatures[i-1].Level){
			case 111:
				//Soldat Level 1, ID 1, ArrayPlace 0
				Creatures[i-1].Name = "Soldat";
				Creatures[i-1].Leben = 4;
				Creatures[i-1].Angriff = 2;
				break;
			case 112:
				//Soldat Level 2, ID 2, ArrayPlace 1
				Creatures[i-1].Name = "Soldat";
				Creatures[i-1].Leben = 4;
				Creatures[i-1].Angriff = 3;
				break;
			case 113:
				//Soldat Level 3, ID 3, ArrayPlace 2
				Creatures[i-1].Name = "Soldat";
				Creatures[i-1].Leben = 7;
				Creatures[i-1].Angriff = 3;
				break;
			case 114:
				//Soldat Level 4, ID 4, ArrayPlace 3
				Creatures[i-1].Name = "Soldat";
				Creatures[i-1].Leben = 9;
				Creatures[i-1].Angriff = 4;
				break;
			case 121:
				//Schildträger Level 1, ID 5, ArrayPlace 4
				Creatures[i-1].Name = "Schildträger";
				Creatures[i-1].Leben = 4;
				Creatures[i-1].Angriff = 2;
				Creatures[i-1].Skills[3] = 1;
				break;
			case 122:
				//Schildträger Level 2, ID 6, ArrayPlace 5
				Creatures[i-1].Name = "Schildträger";
				Creatures[i-1].Leben = 6;
				Creatures[i-1].Angriff = 2;
				Creatures[i-1].Skills[3] = 1;
				break;
			case 123:
				//Schildträger Level 3, ID 7, ArrayPlace 6
				Creatures[i-1].Name = "Schildträger";
				Creatures[i-1].Leben = 6;
				Creatures[i-1].Angriff = 2;
				Creatures[i-1].Skills[3] = 2;
				break;
			case 124:
				//Schildträger Level 4, ID 8, ArrayPlace 7
				Creatures[i-1].Name = "Schildträger";
				Creatures[i-1].Leben = 8;
				Creatures[i-1].Angriff = 2;
				Creatures[i-1].Skills[4] = 1;
				break;
			case 131:
				//Schütze Level 1, ID 9, ArrayPlace 8
				Creatures[i-1].Name = "Schütze";
				Creatures[i-1].Leben = 4;
				Creatures[i-1].Angriff = 2;
				Creatures[i-1].Skills[5] = 1;
				break;
			case 132:
				//Schütze Level 2, ID 10, ArrayPlace 9
				Creatures[i-1].Name = "Schütze";
				Creatures[i-1].Leben = 5;
				Creatures[i-1].Angriff = 3;
				Creatures[i-1].Skills[5] = 1;
				break;
			case 133:
				//Schütze Level 3, ID 11, ArrayPlace 10
				Creatures[i-1].Name = "Schütze";
				Creatures[i-1].Leben = 6;
				Creatures[i-1].Angriff = 3;
				Creatures[i-1].Skills[5] = 2;
				break;
			case 134:
				//Schütze Level 4, ID 12, ArrayPlace 11
				Creatures[i-1].Name = "Schütze";
				Creatures[i-1].Leben = 8;
				Creatures[i-1].Angriff = 2;
				Creatures[i-1].Skills[5] = 2;
				Creatures[i-1].Skills[13]=2;
				break;
			case 141:
				//Reiter Level 1, ID 13, ArrayPlace 12
				Creatures[i-1].Name = "Reiter";
				Creatures[i-1].Leben = 2;
				Creatures[i-1].Angriff = 3;
				Creatures[i-1].Skills[8] = 2;
				break;
			case 142:
				//Reiter Level 2, ID 14, ArrayPlace 13
				Creatures[i-1].Name = "Reiter";
				Creatures[i-1].Leben = 3;
				Creatures[i-1].Angriff = 4;
				Creatures[i-1].Skills[8] = 2;
				break;
			case 143:
				//Reiter Level 3, ID 15, ArrayPlace 14
				Creatures[i-1].Name = "Reiter";
				Creatures[i-1].Leben = 4;
				Creatures[i-1].Angriff = 4;
				Creatures[i-1].Skills[8] = 2;
				Creatures[i-1].Skills[11] = 1;
				break;
			case 144:
				//Reiter Level 4, ID 16, ArrayPlace 15
				Creatures[i-1].Name = "Reiter";
				Creatures[i-1].Leben = 6;
				Creatures[i-1].Angriff = 4;
				Creatures[i-1].Skills[8] = 2;
				Creatures[i-1].Skills[10]=1;
				Creatures[i-1].Skills[11] = 1;
				break;
			case 151:
				//Krieger Level 1, ID 17, ArrayPlace 16
				Creatures[i-1].Name = "Krieger";
				Creatures[i-1].Leben = 4;
				Creatures[i-1].Angriff = 2;
				Creatures[i-1].Skills[13] = 2;
				break;
			case 152:
				//Krieger Level 2, ID 18, ArrayPlace 17
				Creatures[i-1].Name = "Krieger";
				Creatures[i-1].Leben = 5;
				Creatures[i-1].Angriff = 2;
				Creatures[i-1].Skills[10] = 1;
				Creatures[i-1].Skills[13] = 2;
				break;
			case 153:
				//Krieger Level 3, ID 19, ArrayPlace 18
				Creatures[i-1].Name = "Krieger";
				Creatures[i-1].Leben = 6;
				Creatures[i-1].Angriff = 3;
				Creatures[i-1].Skills[10] = 1;
				Creatures[i-1].Skills[13] = 2;
				break;
			case 154:
				//Krieger Level 4, ID 20, ArrayPlace 19
				Creatures[i-1].Name = "Krieger";
				Creatures[i-1].Leben = 8;
				Creatures[i-1].Angriff = 3;
				Creatures[i-1].Skills[9] = 1;
				Creatures[i-1].Skills[10]=1;
				Creatures[i-1].Skills[13] = 2;
				break;
			}

			//Debug.Log("Kreatur "+(i-1).ToString()+" hat Klasse "+Creatures [i - 1].Klasse.ToString()+" und Level "+Creatures [i - 1].Level.ToString()+" und Nummer "+Creatures [i-1].Nummer.ToString());
		}
		LogDeck (Deck);
	}
	
	// Update is called once per frame
	void Update () {
		Level = Mathf.FloorToInt (Mathf.Sqrt (EP));
		if (EP > 10000) {
			EP = 10000;
		}
		if (Application.loadedLevelName == "Menu") {
			GameObject.Find ("MenuSilver").GetComponent<UnityEngine.UI.Text> ().text = Silber.ToString();
			GameObject.Find ("MenuGold").GetComponent<UnityEngine.UI.Text> ().text = Gold.ToString();
		}
		SavePlayerData ();
		if (SetValuesToStandard) {
			Silber=StandardSilber;
			Gold=StandardGold;
			EP = StandardEP;

            for(int i = 0; i < StandardDeck.Length;i++)
            {
                Deck[i].Anzahl = StandardDeck[i].Anzahl;
                Deck[i].Klasse = StandardDeck[i].Klasse;
                Deck[i].Level = StandardDeck[i].Level;
                Deck[i].NummerInKlasse = StandardDeck[i].NummerInKlasse;
            }
            for (int i = 0; i < StandardDeck.Length;i++)
            {
                Besitz[i].Anzahl = StandardBesitz[i].Anzahl;
                Besitz[i].Klasse = StandardBesitz[i].Klasse;
                Besitz[i].Level = StandardBesitz[i].Level;
                Besitz[i].NummerInKlasse = StandardBesitz[i].NummerInKlasse;
                Debug.Log(Besitz[i].Anzahl + "Anz, " + Besitz[i].Klasse + "Kla, " + Besitz[i].Level + "Lvl, " + Besitz[i].NummerInKlasse+"Nr");
            }
            
			SetValuesToStandard=false;
		}
		if (LogMyDeck) {
			LogDeck (Deck);
			LogMyDeck = false;
		}
		if (LogBesitz) {
			LogDeck (Besitz);
			LogBesitz = false;
		}
		if (AddToDeck) {
            if (GameObject.Find("Deck"))
            {
                if (GameObject.Find("Deck").GetComponent<DeckScript>())
                {
                    Besitz = GameObject.Find("Deck").GetComponent<DeckScript>().AddToDeck(AddToDeckID, AddToDeckAnzahl, Besitz);
                    AddToDeck = false;
                    GameObject.Find("Deck").GetComponent<DeckScript>().Ausgefuehrt = false;
                }
            }
		}
	}
	void Awake(){
		DontDestroyOnLoad (gameObject);
	}
	public void LogDeck(Kreatur[] InputDeck){
		Debug.LogWarning ("LogStart");
		for (int i=0; i<InputDeck.Length; i++) {
			if(InputDeck[i].Anzahl>0){
				for(int x=1;x<=InputDeck[i].Anzahl;x++){
					Debug.Log(Creatures[i].Name+", Level "+Creatures[i].Level+": "+Creatures[i].Angriff+" Angriff, "+Creatures[i].Leben+" Leben");
				}
			}
		}
		Debug.LogWarning ("LogEnd");
	}
	public string[] GetDescriptionOfSkill(int SkillID, int Value){
		string[] Out = new string[2];
		switch(SkillID){
			/*Fähigkeitsnummern:
			 * 1:Regeneration
			 * 2:Siegessicherheit
			 * 3:Explosiv
			 * 4:Rüstung
			 * 5:Blockade
			 * 6:Bogen
			 * 7:Auge um Auge
			 * 8:Giftig
			 * 9:Kavallerie
			 * 10:Rachsucht
			 * 11:Schneller Schlag
			 * 12:Todesstoß
			 * 13:Verflucht
			 * 14:Wildheit
			 */
		case 1:
			Out[0]="Regeneration "+Value;
			Out[1]="Regeneriert jede Runde "+Value+" Leben";
					break;
		case 2:
			Out[0]="Siegessicherheit "+Value;
			Out[1]="Wenn eine gegnerische Einheit stirbt, erhält diese Einheit +"+Value+"Leben, +"+Value+"Angriff";			
					break;
		case 3:
			Out[0]="Explosiv "+Value;
			Out[1]="Wenn diese Einheit stirbt, fügt sie allen angrenzenden Einheiten "+Value+" Schaden zu";
					break;
		case 4:
			Out[0]="Rüstung "+Value;
			Out[1]="Reduziert erlittenen Schaden um "+Value;
					break;
		case 5:
			Out[0]="Blockade";
			Out[1]="Reduziert mit 50% Wahrscheinlichkeit erlittenen Schaden auf 0";
					break;
		case 6:
			Out[0]="Bogen "+Value;
			Out[1]="Reichweite: "+Value;
					break;
		case 7:
			Out[0]="Auge um Auge";
			Out[1]="Fügt erlittenen Schaden auch dem Gegner zu";
					break;
		case 8:
			Out[0]="Giftig "+Value;
			Out[1]="Getroffene Einheiten erhalten für 3 Runden 'Vergiftet "+Value+"'";
					break;
		case 9:
			Out[0]="Kavallerie "+Value;
			Out[1]="Tempo: "+Value;
					break;
		case 10:
			Out[0]="Rachsucht "+Value;
			Out[1]="Wenn diese Einheit Schaden erleidet, erhält sie +"+Value+"Angriff";
					break;
		case 11:
			Out[0]="Schneller Schlag";
			Out[1]="Kann sich auch nach dem Angriff noch bewegen";
					break;
		case 12:
			Out[0]="Todesstoß";
			Out[1]="Tötet den ersten getroffenen Gegner sofort";
					break;
		case 13:
			Out[0]="Verflucht";
			Out[1]="Erleidet zugefügten Schaden auch selbst";
					break;
		case 14:
			Out[0]="Wildheit "+Value;
			Out[1]="Kann "+Value+" mal angreifen";
					break;
		}
		return Out;
	}

	
	void SavePlayerData(){
		string NormData = Mathf.FloorToInt(Silber/100000f).ToString()+" "+Silber%100000 + " " + Gold.ToString ()+" "+EP.ToString();
		string DeckData = "";
		string HaveData = "";
		for (int i=0; i<Deck.Length; i++) {
			DeckData+= " "+Deck[i].Anzahl;
		}
		for (int i=0; i<Besitz.Length; i++) {
			HaveData+= " "+Besitz[i].Anzahl;
		}

		
		// Must be 64 bits, 8 bytes.
		// Distribute this key to the user who will decrypt this file.
		string sSecretKey;
		
		// Get the Key for the file to Encrypt.
		sSecretKey = GenerateKey();
		string s2 = GenerateKey ();
		string s3 = GenerateKey ();
		// For additional security Pin the key.
		GCHandle gch = GCHandle.Alloc( sSecretKey,GCHandleType.Pinned );
		

		

		File.WriteAllText(Application.persistentDataPath + "/Key", sSecretKey+"j@~~@/(/)"+s2+"|$/$|"+s3);
		File.WriteAllText(Application.persistentDataPath + "/SpielerDatenNoCrypt", NormData); //Normale Spielerdaten: Silber, Gold, EP.
		File.WriteAllText (Application.persistentDataPath + "/DeckDatenNoCrypt", DeckData); //Das gesamte Deck
		File.WriteAllText (Application.persistentDataPath + "/BesitzDatenNoCrypt", HaveData); //Der gesamte Besitz
		string path1 = Application.persistentDataPath + "/SpielerDatenNoCrypt";
		string path1_2 = Application.persistentDataPath + "/SpielerDaten";
		string path2 = Application.persistentDataPath + "/DeckDatenNoCrypt";
		string path2_2 = Application.persistentDataPath + "/DeckDaten";
		string path3 = Application.persistentDataPath + "/BesitzDatenNoCrypt";
		string path3_2 = Application.persistentDataPath + "/BesitzDaten";
		// Encrypt the file.        
		EncryptFile(@path1, 
		            @path1_2, 
		            sSecretKey);
		EncryptFile(@path2, 
		            @path2_2, 
		            sSecretKey);
		EncryptFile(@path3, 
		            @path3_2, 
		            sSecretKey);

		File.Delete (path1);
		File.Delete (path2);
		File.Delete (path3);
		// Remove the Key from memory. 
		ZeroMemory(gch.AddrOfPinnedObject(), sSecretKey.Length * 2);
		gch.Free();
	}

	void LoadPlayerData(){
		/*string sSecretKey="Brabbel";
		if (File.Exists (Application.persistentDataPath + "/Key")) {
			sSecretKey = File.ReadAllText (Application.persistentDataPath + "/Key").Split (new string[]{"j@~~@/(/)"}, StringSplitOptions.None) [0];
		}
		string path1 = Application.persistentDataPath + "/SpielerDaten";
		string path1_2 = Application.persistentDataPath + "/SpielerDatenNoCrypt";
		string path2_2 = Application.persistentDataPath + "/DeckDatenNoCrypt";
		string path2 = Application.persistentDataPath + "/DeckDaten";
		string path3_2 = Application.persistentDataPath + "/BesitzDatenNoCrypt";
		string path3 = Application.persistentDataPath + "/BesitzDaten";
		if (File.Exists (Application.persistentDataPath + "/SpielerDaten")&&File.Exists(Application.persistentDataPath + "/DeckDaten")&&File.Exists(Application.persistentDataPath + "/BesitzDaten")){
			DecryptFile (@path1,
  					 	 @path1_2,
		             	 sSecretKey);
			string NormDataCrypt = File.ReadAllText (Application.persistentDataPath + "/SpielerDatenNoCrypt");
			File.Delete(@path1_2);
			string[] NormDataSplit = NormDataCrypt.Split (new string[]{" "}, StringSplitOptions.None);
			Silber = 100000 * int.Parse (NormDataSplit [0])+int.Parse (NormDataSplit [1]);
			Gold = int.Parse(NormDataSplit [2]);
			EP = int.Parse (NormDataSplit [3]);
			DecryptFile(
				@path2,
				@path2_2,
				sSecretKey);
			DecryptFile(
				@path3,
				@path3_2,
				sSecretKey);
			string DeckDataCrypt = File.ReadAllText (Application.persistentDataPath + "/DeckDatenNoCrypt");
			string HaveDataCrypt = File.ReadAllText (Application.persistentDataPath + "/BesitzDatenNoCrypt");
			string[] DeckDataSplit = DeckDataCrypt.Split(new string[] {" "}, StringSplitOptions.RemoveEmptyEntries);
			for(int i=1;i<Deck.Length;i++){
				if(i<DeckDataSplit.Length){
					Debug.Log(DeckDataSplit[i]);
					Deck[i].Anzahl = int.Parse(DeckDataSplit[i]);
				} else {
					Deck[i].Anzahl=0;
				}
			}*/
			/*string[] BesitzDataSplit = HaveDataCrypt.Split(new string[] {" "}, StringSplitOptions.None);
			for(int i=0;i<BesitzDataSplit.Length;i++){
				Besitz[i].Anzahl = int.Parse(BesitzDataSplit[i]);
			}*/
		/*} else {
			Silber=StandardSilber;
			Gold=StandardGold;
			EP=StandardEP;
			Deck=StandardDeck;
			Besitz=StandardBesitz;
		}*/


	}

	float Encode(int Inp, string Key){
		return Mathf.Pow (Inp, 1f/(Key.Length/Key.Split (new string[]{"a", "/", "_", "M"}, StringSplitOptions.None).Length));
	}
	int Decode(float Inp, string Key){
		float Out = Mathf.Pow (Inp, Key.Length/Key.Split (new string[]{"a", "/", "_", "M"}, StringSplitOptions.None).Length);
		if (Out - Mathf.Round (Out) < 0.05f) {
			return Mathf.RoundToInt (Out);
		} else {
			return -666;
		}
	}




















	//  Call this function to remove the key from memory after use for security
	[System.Runtime.InteropServices.DllImport("KERNEL32.DLL", EntryPoint="RtlZeroMemory")]
	public static extern bool ZeroMemory(IntPtr Destination, int Length);
	
	// Function to Generate a 64 bits Key.
	static string GenerateKey() 
	{
		// Create an instance of Symetric Algorithm. Key and IV is generated automatically.
		DESCryptoServiceProvider desCrypto =(DESCryptoServiceProvider)DESCryptoServiceProvider.Create();
		
		// Use the Automatically generated key for Encryption. 
		return ASCIIEncoding.ASCII.GetString(desCrypto.Key);
	}
	
	static void EncryptFile(string sInputFilename,
	                        string sOutputFilename, 
	                        string sKey) 
	{
		FileStream fsInput = new FileStream(sInputFilename, 
		                                    FileMode.Open, 
		                                    FileAccess.Read);
		
		FileStream fsEncrypted = new FileStream(sOutputFilename, 
		                                        FileMode.Create, 
		                                        FileAccess.Write);
		DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
		DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
		DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
		ICryptoTransform desencrypt = DES.CreateEncryptor();
		CryptoStream cryptostream = new CryptoStream(fsEncrypted, 
		                                             desencrypt, 
		                                             CryptoStreamMode.Write); 
		
		byte[] bytearrayinput = new byte[fsInput.Length];
		fsInput.Read(bytearrayinput, 0, bytearrayinput.Length);
		cryptostream.Write(bytearrayinput, 0, bytearrayinput.Length);
		cryptostream.Close();
		fsInput.Close();
		fsEncrypted.Close();
	}
	
	static void DecryptFile(string sInputFilename, 
	                        string sOutputFilename,
	                        string sKey)
	{
		DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
		//A 64 bit key and IV is required for this provider.
		//Set secret key For DES algorithm.
		DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
		//Set initialization vector.
		DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
		
		//Create a file stream to read the encrypted file back.
		FileStream fsread = new FileStream(sInputFilename, 
		                                   FileMode.Open, 
		                                   FileAccess.Read);
		//Create a DES decryptor from the DES instance.
		ICryptoTransform desdecrypt = DES.CreateDecryptor();
		//Create crypto stream set to read and do a 
		//DES decryption transform on incoming bytes.
		CryptoStream cryptostreamDecr = new CryptoStream(fsread, 
		                                                 desdecrypt,
		                                                 CryptoStreamMode.Read);
		//Print the contents of the decrypted file.
		StreamWriter fsDecrypted = new StreamWriter(sOutputFilename);
		fsDecrypted.Write(new StreamReader(cryptostreamDecr).ReadToEnd());
		fsDecrypted.Flush();
		fsDecrypted.Close();
	} 
}


























namespace CSEncryptDecrypt
{
	class Class1
	{

		
		static void Main()
		{
		}
	}
}
