using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Molecule : MonoBehaviour {

    public VSEPR vsepr;

    // Use this for initialization
    void Start() {
        ColorizeMe();
    }
    public static void ColorizeAtomAs(GameObject gameObject, string symbol)
    {
       var color = HexToColor(VSEPR.valencyElectrones[symbol].color);
        Colorize(gameObject, color);
    }
	static void Colorize(GameObject gobj, Color color){
		//Set the main Color of the Material to green
		Renderer rend = gobj.GetComponent<Renderer>();
		rend.material.color = color;
	}
	private List<LigandAndObject> ligands= new List<LigandAndObject>();

	class LigandAndObject{
		public GameObject gameObject;
		public string symbol;
	}

	public static Color HexToColor(string hex)
	{
		hex = hex.Replace ("0x", "");//in case the string is formatted 0xFFFFFF
		hex = hex.Replace ("#", "");//in case the string is formatted #FFFFFF
		byte a = 255;//assume fully visible unless specified in hex
		byte r = byte.Parse(hex.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
		byte g = byte.Parse(hex.Substring(2,2), System.Globalization.NumberStyles.HexNumber);
		byte b = byte.Parse(hex.Substring(4,2), System.Globalization.NumberStyles.HexNumber);
		//Only use alpha if the string has enough characters
		if(hex.Length == 8){
			a = byte.Parse(hex.Substring(6,2), System.Globalization.NumberStyles.HexNumber);
		}
		return new Color32(r,g,b,a);
	}
	void ColorizeMe(){
		if(vsepr!=null){
			
		var color = HexToColor(vsepr.CentralAtom.info.color);
		Colorize (gameObject, color);
			var ligandsX = vsepr.Ligands.Where (l => l.Type == VSEPR.Ligand.LigandType.X).GetEnumerator();

			foreach(Renderer variableName in GetComponentsInChildren<Renderer>())
			{
				if (variableName.transform.parent) {
					
					var parentName = variableName.transform.parent.gameObject.transform.name;
					if (parentName == "X" && variableName.transform.name == "Core") {
						ligandsX.MoveNext ();
						var symbol = VSEPR.valencyElectrones [ligandsX.Current.Symbol].color;
						var ligandColor = HexToColor (symbol);
						variableName.material.color = ligandColor;
						ligands.Add (new LigandAndObject (){ gameObject = variableName.gameObject, symbol = ligandsX.Current.Symbol });

					}
				}
			}
	}
	}




	void OnGUI(){
		var recta = new Rect (0, 0, 200, 40);
		var pointa = Camera.main.WorldToScreenPoint (gameObject.transform.position);
		recta.x = pointa.x-20;
		recta.y = Screen.height - pointa.y - recta.height; // bottom left corner set to the 3D point
		if (vsepr != null) {
			GUI.Label (recta, vsepr.CentralAtom.symbol, new GUIStyle() { fontSize=35}); // display its name, or other string
		}
		foreach (var ligand in ligands) {
			var rect = new Rect (0, 0, 200, 40);
			var point = Camera.main.WorldToScreenPoint (ligand.gameObject.transform.position);
			rect.x = point.x-20;
			rect.y = Screen.height - point.y - rect.height; // bottom left corner set to the 3D point
			if (vsepr != null) {
				GUI.Label (rect, ligand.symbol, new GUIStyle() { fontSize = 35 }); // display its name, or other string
			}
		}
	}
		
	// Update is called once per frame
	void Update () {
		transform.Rotate (Vector3.up*Time.deltaTime*30, Space.World);
	}
}
