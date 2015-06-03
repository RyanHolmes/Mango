using UnityEngine;
using System.Collections;

public class guide : MonoBehaviour {

	public int yVal = 0;
	public Vector3 pos;

	// Use this for initialization
	void Start () {
		this.GetComponent<Renderer> ().material.color = new Color (0, 0, 0, 0.5f);
	}
	
	// Update is called once per frame
	void Update () {
		pos = new Vector3 ((int)this.transform.position.x, yVal, (int)this.transform.position.z);
		this.transform.position = pos;

	}
	void setY (int i) {
		yVal = i;
	}
	
}
