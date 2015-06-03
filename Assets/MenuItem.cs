using UnityEngine;
using System.Collections;

public class MenuItem : MonoBehaviour{
	//script goes on objects
	
	public int posx;
	public int posy;
	public string name; //same as tag
	public string type; //block, gun, delete, default

	//constructor - likely empty?
	public MenuItem(){

	}

	public void setx (int x) {
		posx = x;
	}

	public void sety (int y) {
		posy = y;
	}
}
