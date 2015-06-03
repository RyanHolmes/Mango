using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class player : MonoBehaviour {

	public List <GameObject> blocks;

	//RYANTODO: 
	//1) no duplicate blocks
	//4) blocks fall when deleted
	//2) color change back on delete mode
	//3) building glitches - building in front

	//Game Objects
	public Camera cam;
	public GameObject cube;
	public GameObject guide;

	public Canvas can;

	public int count = 0;
	public int deleteCount = 0;
	
	//Mouse Crap
	public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
	public RotationAxes axes = RotationAxes.MouseXAndY;
	public float sensitivityX = 10F;
	public float sensitivityY = 10F;
	public float minimumX = -360F;
	public float maximumX = 360F;
	public float minimumY = -60F;
	public float maximumY = 60F;
	float rotationY = 0F;
	
	//pngs
	public Texture2D crosshairTexture;
	
	//booleans
	public bool canPress; //disables walking ability
	public bool canDrawCH; //enables crosshair drawing
	public bool canDrawMenu; //Enables menu drawing
	public bool canJump; //Enables player jump movement when they rreturn to ground
	public bool canShoot; //disable shooting when in menu mode
	public bool canBuild;
	public bool startGame;

	public string currentState = "build";

	void Start () {
		canDrawCH = false;
		canPress = true;
		canDrawMenu = false;
		canJump = false;
		canShoot = false;
		canBuild = true;
		blocks =  new List<GameObject>();
		Cursor.visible = true; 
		//make measure block way under map with the intention of moving it around
		GameObject g = (GameObject)Instantiate (guide, new Vector3(-10,-10,-10), Quaternion.identity);
		g.name = "guide";
		g.tag = "guide";
	}
	
	// Update is called once per frame
	void Update () {
		if (startGame) {
			//mouse stuff
			if (axes == RotationAxes.MouseX) {
				cam.transform.Rotate (0, Input.GetAxis ("Mouse X") * sensitivityX, 0);
			} else if (axes == RotationAxes.MouseXAndY && canPress) {
				float rotationX = transform.localEulerAngles.y + Input.GetAxis ("Mouse X") * sensitivityX;
				
				rotationY += Input.GetAxis ("Mouse Y") * sensitivityY;
				rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
				
				transform.localEulerAngles = new Vector3 (-rotationY, rotationX, 0);
			} else if (canPress) {
				rotationY += Input.GetAxis ("Mouse Y") * sensitivityY;
				rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
				
				cam.transform.localEulerAngles = new Vector3 (-rotationY, transform.localEulerAngles.y, 0);
			}
			
			Vector3 forward = cam.transform.forward;
			forward.y = 0; // this should be changed to being relative to the ground. later.
			if (Input.GetKey (KeyCode.W) && canPress) {
				this.transform.position = this.transform.position + forward * 0.1f;
			}
			if (Input.GetKey (KeyCode.S) && canPress) {
				this.transform.position = this.transform.position - forward * 0.1f;
			}
			if (Input.GetKey (KeyCode.D) && canPress) {
				//transform.Rotate (Vector3.up * Time.deltaTime * 100);
				this.transform.Translate (Vector3.right * 0.1f);
			}
			if (Input.GetKey (KeyCode.A) && canPress) {
				//transform.Rotate (Vector3.down * Time.deltaTime * 100);
				this.transform.Translate (Vector3.left * 0.1f);
			}

			if (Input.GetKey (KeyCode.Space) && canJump) {
				GetComponent <Rigidbody> ().AddForce (Vector3.up * 250f);
				canJump = false;
			}

			// ##########################    RAY STUFF   #########################################################

			if (canBuild) {
				canDrawCH = true;
				//if you can build, enable guide
				GameObject.Find ("guide").GetComponent<Renderer> ().enabled = true;

				//move guide to correct position
				GameObject.Find ("guide").transform.position = cam.transform.position + (cam.transform.forward * 10);

				//ray objects
				RaycastHit hit;
				Ray target = new Ray (cam.transform.position, cam.transform.forward);
				Debug.DrawRay (cam.transform.position, cam.transform.forward);


				if (Physics.Raycast (target, out hit, 10.0f)) {
					GameObject.Find ("guide").transform.position = hit.point;
					if (hit.collider.tag.Contains("BLOCK")) {
						//if below center point of target, stay on same y plane
						//if above center point of target, got up a y plane
						if (hit.point.y <= 0) {
							GameObject.Find ("guide").SendMessage ("setY", hit.collider.transform.position.y);
						} else if (hit.point.y > 0) {
							GameObject.Find ("guide").SendMessage ("setY", hit.collider.transform.position.y + 1);
						}
					} else {
						//if not on a block, set guide's y to zero
						GameObject.Find ("guide").SendMessage ("setY", 0, SendMessageOptions.DontRequireReceiver);
					}
				} else {
					GameObject.Find ("guide").SendMessage ("setY", 0, SendMessageOptions.DontRequireReceiver);
				}

				//place block 
				if (Input.GetKeyDown (KeyCode.Mouse0) && canBuild) {
					Vector3 newBlockPosition = GameObject.Find("guide").GetComponent<guide>().pos;
					Block newBlock = new Block(newBlockPosition.x,newBlockPosition.y,newBlockPosition.z);
					BlockMap.getInstance().AddNewBlockOfType(newBlock,BasicBlock.tag);
				}
			}
			//IN DELETE MODE
			else {
				GameObject.Find ("guide").GetComponent<Renderer> ().enabled = false;
				//ray objects
				RaycastHit hit;
				Ray target = new Ray (cam.transform.position, cam.transform.forward);
				Debug.DrawRay (cam.transform.position, cam.transform.forward);
				
				
				if (Physics.Raycast (target, out hit, 10.0f)) {
					if (hit.collider.tag.Contains("BLOCK")) {
						hit.collider.gameObject.GetComponent<Renderer> ().material.color = Color.red; // change back??
						if (Input.GetKeyDown (KeyCode.Mouse0)) {
							Vector3 blockToDestroyPosition = hit.collider.gameObject.transform.position;
							BlockMap.getInstance().Delete(blockToDestroyPosition);
							//GameObject.Destroy (hit.collider.gameObject);
							//blocks.Remove (hit.collider.gameObject);
						}
					}
				}
			}

			//if key == e, enter delete mode or exit it
			if (Input.GetKeyDown (KeyCode.E)) {
				if (deleteCount % 2 == 0) {
					canBuild = false;
				} else {
					canBuild = true;
				}
				deleteCount++;
			}
			// #####################################################################################################
		}
	}

	void OnGUI(){
		//if not paused
		if (Time.timeScale != 0) {
			if (crosshairTexture != null && canDrawCH)
				GUI.DrawTexture (new Rect ((Screen.width - crosshairTexture.width * 1) / 2, (Screen.height - crosshairTexture.height * 1) / 2, crosshairTexture.width * 1, crosshairTexture.height * 1), crosshairTexture);
		}
	}

	void OnCollisionEnter(Collision c){
		if(c.gameObject.tag == "floor" || c.gameObject.tag == "block"){
			canJump = true;
			
		}
	}

	void startTheGame(){
		startGame = true;
		can.enabled = false;
		Cursor.visible = false;
	}
	
}
