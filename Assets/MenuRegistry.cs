using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//RYANTODO: Create all menu items (In Flash)
//RYANTODO: test add and delete functions for menu items
//RYANTODO: merge with Hannes

public class MenuRegistry : MonoBehaviour{

	public Camera cam; //player camera
	public GameObject player;

	public GameObject menu_default;
	public GameObject menu_delete;
	public GameObject menu_red_gun;
	public GameObject menu_green_gun;
	public GameObject menu_red_block;
	public GameObject menu_green_block;
	public GameObject md;

	//list of all menu card items
	public List<GameObject> blocks;
	public List<GameObject> guns;
	public string currentMode = "shoot"; //build, delete

	public int lastx = 0; //last postion defaults to default
	public int lasty = 0;
	public int curx = 0; //current x position of menu -> -1 is gun and -2 is block, 0 is default
	public int gcury = 0;//y position of guns 
	public int bcury = 0;//y position of blocks

	//constructor
	void Start() {
		blocks = new List <GameObject>();
		guns = new List<GameObject>();

		//set positions of menu items to begin with
		md = (GameObject)Instantiate (menu_default, new Vector3(-100,-100,-100), Quaternion.identity);
		md.tag = "menu_default";
		md.GetComponent<MenuItem>().setx(0);
		md.GetComponent<MenuItem>().sety(0);

		GameObject h = (GameObject)Instantiate (menu_delete, new Vector3(-100,-100,-100), Quaternion.identity);
		h.tag = "menu_delete";
		h.GetComponent<MenuItem>().setx(2);
		h.GetComponent<MenuItem>().sety(-1);
		blocks.Add (h);

		GameObject i = (GameObject)Instantiate (menu_red_gun, new Vector3(-100,-100,-100), Quaternion.identity);
		i.tag = "menu_red_gun";
		i.GetComponent<MenuItem>().setx(1);
		i.GetComponent<MenuItem>().sety(0);
		guns.Add (i);

		GameObject j = (GameObject)Instantiate (menu_green_gun, new Vector3(-100,-100,-100), Quaternion.identity);
		j.tag = "menu_green_gun";
		j.GetComponent<MenuItem>().setx(1);
		j.GetComponent<MenuItem>().sety(1);
		guns.Add (j);

		GameObject k = (GameObject)Instantiate (menu_red_block, new Vector3(-100,-100,-100), Quaternion.identity);
		k.tag = "menu_red_block";
		k.GetComponent<MenuItem>().setx(2);
		k.GetComponent<MenuItem>().sety(0);
		blocks.Add (k);

		GameObject l = (GameObject)Instantiate (menu_green_block, new Vector3(-100,-100,-100), Quaternion.identity);
		l.tag = "menu_green_block";
		l.GetComponent<MenuItem>().setx(2);
		l.GetComponent<MenuItem>().sety(1);
		blocks.Add (l);

	}

	void Update(){
		if (Input.GetKey (KeyCode.Q)) {
			//GameObject.Find("Player").SendMessage("setFalse", "canShoot");
			this.GetComponent<player>().canPress = false;
			this.GetComponent<player>().canJump = false;
			//GameObject.Find("gun_basic").GetComponent<MeshRenderer>().enabled = false;
			drawMenu();

			if (Input.GetKeyDown (KeyCode.W)) {
				//if its a gun item 
				if(curx == -1 && (gcury * -1) < guns.Count - 1){
					shiftUp();

				}
				//if its a block item
				if(curx == -2 && (bcury * -1) < blocks.Count - 2){
					shiftUp();
				}

			} else if (Input.GetKeyDown (KeyCode.S)) {
				if(curx == -1 && gcury != 0){
					shiftDown();
				}
				if(curx == -2 && bcury != 1){
					shiftDown();
				}

			} else if (Input.GetKeyDown (KeyCode.A)) {
				if(curx == -1){ 
					gcury = 0;
					shiftLeft();
				}
				if(curx == -2){
					bcury = 0;
					shiftLeft();
				}

			} else if (Input.GetKeyDown (KeyCode.D)) {
				if(curx == 0){ 
					shiftRight();
				}
				else if(curx == -1){
					gcury = 0;
					shiftRight();
				}
			}

		} else {
			hideMenu ();
			//GameObject.Find("Player").SendMessage("setTrue", "canShoot");
			this.GetComponent<player>().canPress = true;
			this.GetComponent<player>().canJump = true;
			//GameObject.Find("gun_basic").GetComponent<MeshRenderer>().enabled = true; 
			if(curx == -1){
				currentMode = "shoot";
			}
			else if(curx == -2){
				if(bcury == 1){
					currentMode = "delete";
				}
				else{
					currentMode = "build";
				}
			}
			curx = 0;
			bcury = 0;
			gcury = 0;
		}
	}



	//draw screen
	public void drawMenu(){
		//NOTE: THIS IS A FUCKSHOW - ASK ME ABOUT IT
		//draw menu items at current position
		//draw menu items at camera position + cam's forward vector + up and right vector * item posiition, * 0.25
		//md.GetComponent<MeshRenderer> ().enabled = true;

		md.transform.position = cam.transform.position + cam.transform.forward + (cam.transform.right * (0.25f * curx)) + (cam.transform.up * (0.25f * 0));// +
		md.transform.rotation = cam.transform.rotation;

		for(int i = 0; i < guns.Count; i++){
			guns[i].transform.position = cam.transform.position + cam.transform.forward + 
			cam.transform.right * (0.25f * (guns[i].GetComponent<MenuItem>().posx + curx)) + cam.transform.up * (0.25f * (guns[i].GetComponent<MenuItem>().posy + gcury));

			guns[i].transform.rotation = cam.transform.rotation;
		}

		for(int j = 0; j < blocks.Count; j++){
			blocks[j].transform.position = cam.transform.position + cam.transform.forward + 
			cam.transform.right * (0.25f * (blocks[j].GetComponent<MenuItem>().posx + curx)) + cam.transform.up * (0.25f * (blocks[j].GetComponent<MenuItem>().posy + bcury));
			blocks[j].transform.rotation = cam.transform.rotation;
		}
	}

	public void hideMenu(){
		//move items to under screen
		md.transform.position = new Vector3 (-100,-100,-100);
		//md.GetComponent<MeshRenderer> ().enabled = false;

		for(int i = 0; i < guns.Count; i++){
			guns[i].transform.position =  new Vector3 (-100,-100,-100);
		}

		for(int j = 0; j < blocks.Count; j++){
			blocks[j].transform.position = new Vector3 (-100,-100,-100);

		}
		
	}

	//resetMenu - used for respecing probably
	public void resetMenu(){
		blocks.Clear ();
		guns.Clear ();

		//re initialize
		blocks.Add(GameObject.Find ("delete_mode"));
		guns.Add (GameObject.Find ("red_gun"));
		guns.Add(GameObject.Find ("green_gun"));
		blocks.Add(GameObject.Find ("green_block"));
		blocks.Add(GameObject.Find ("red_block"));
	}

	//for movement leftward
	public void shiftLeft() {
		//shift left then draw menu again and update current centered position 
		curx += 1;
		this.drawMenu ();
	}

	//for movement rightward
	public void shiftRight() {
		curx -= 1;
		this.drawMenu ();
	}

	//for movement upward
	public void shiftUp() {
		if(curx == -1){
			gcury -= 1;
		}
		if(curx == -2){
			bcury -= 1;
		}
		this.drawMenu ();
	}

	//for movement downward
	public void shiftDown() {
		if(curx == -1){
			gcury += 1;
		}
		if(curx == -2){
			bcury += 1;
		}
		this.drawMenu ();
	}

	//add an item to menu
	public void addMenuItem (string type, string tag){
		switch (type) {
			
		case "block":
			blocks.Add(GameObject.Find (tag));
			GameObject.Find (tag).SendMessage ("setx", 2);
			GameObject.Find (tag).SendMessage ("sety", blocks.Count - 1);
			break;
			
		case "gun":
			guns.Add(GameObject.Find (tag));
			GameObject.Find (tag).SendMessage ("setx", 1);
			GameObject.Find (tag).SendMessage ("sety", guns.Count);
			break;
		}
		
	}
	
	//remove item
	public void removeMenuItem(string type, string tag){
		switch (type) {
		case "block":
			//remove from array list and place under map
			blocks.Remove(GameObject.Find (tag)); 
			GameObject.Find (tag).transform.Translate(new Vector3(-100,-100,-100));
			break;
			
		case "gun":
			guns.Remove(GameObject.Find (tag));
			GameObject.Find (tag).transform.Translate(new Vector3(-100,-100,-100));
			break;
		}
	}
}
