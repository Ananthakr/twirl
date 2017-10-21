using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovementScript : MonoBehaviour {

	public Vector3 move;
	public Vector3 startingPosition;
	public Quaternion startingRotation;
	public float speed;
	public float turnSpeed = 300f;

	public int score;
	public Text scoreText;
	public Canvas finishGame;
	public Canvas startGame;
	public Text finalScore;
	bool grounded;
	bool moving;
	public Button start;
	public Button replay;

	public Vector3[] coinPositions;
	public Quaternion coinRotation;
	public GameObject coin;
	private Rigidbody rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		move = new Vector3 (0.0f, 0.0f, 1f);
		moving = false;
		startingPosition = rb.transform.position;
		startingRotation = rb.transform.rotation;
		coinRotation = new Quaternion (0.0f, 0.0f, 45f, 0.0f);
		coinPositions = new Vector3[]{ new Vector3(0f,1.25f,2f),new Vector3(0f,1.25f,5f),new Vector3(5f,1.25f,5f),new Vector3(7f,1.25f,5f),new Vector3(10f,1.25f,6f),new Vector3(10f,1.25f,10f)
			,new Vector3(8f,1.25f,10f),new Vector3(6f,1.25f,10f),new Vector3(5f,1.25f,12f),new Vector3(5f,1.25f,18f),new Vector3(2f,1.25f,20f),new Vector3(0f,1.25f,20f),new Vector3(0f,1.25f,13f),
			new Vector3(-2f,1.25f,10f),new Vector3(-5f,1.25f,8f),new Vector3(-5f,1.25f,4f),new Vector3(-2f,1.25f,0f)};
		start.onClick.AddListener (startGamePlay);
		replay.onClick.AddListener (startGamePlay);
	}
		

	
	// Update is called once per frame
	void FixedUpdate () {


		if(moving && !grounded){

			if (Input.GetKey (KeyCode.UpArrow)) {
				transform.Translate (Vector3.forward * speed * Time.deltaTime);

			}

			if (Input.GetKey (KeyCode.DownArrow)) {
				transform.Translate (-Vector3.forward * speed * Time.deltaTime);
			}

			if (Input.GetKey (KeyCode.LeftArrow)) {
				transform.Rotate(Vector3.up, -turnSpeed * Time.deltaTime);
			}

			if (Input.GetKey (KeyCode.RightArrow)) {
				transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime);
			}

			if (Input.touchCount > 0) {
				//handle touch
				foreach (Touch t in Input.touches) {

					if (t.position.x < Screen.width / 2) {
						transform.Rotate (Vector3.up, -turnSpeed * Time.deltaTime);
					} else if (t.position.x > Screen.width / 2) {
						transform.Rotate (Vector3.up, turnSpeed * Time.deltaTime);
					}

				}
			}
				
			transform.Translate (Vector3.forward * speed * Time.deltaTime);
			if (!grounded) {
				score = score + 1;//Mathf.CeilToInt(speed/300);
				SetScoreText ();
			}
		}
	}

	void OnTriggerEnter(Collider obj){
		
		if (obj.gameObject.CompareTag ("coin") && moving) {
			
			obj.gameObject.SetActive (false);
			if (!grounded) {
				score = score + 500;
				Debug.Log ("Coin collided" + score);
				SetScoreText ();
			}

			if (obj.transform.position.Equals( new Vector3 (-2f, 1.25f, 0f) ) ) {
				createCoins ();
			}
			Destroy (obj);

		}
			
	}

	void OnCollisionEnter(Collision hit){
		if (hit.gameObject.CompareTag ("Finish") && moving) {
			scoreText.enabled = false;
			int fscore = getFinalScore();
			//to remove continous score updation after first grounding
			if (fscore != 0) {
				finalScore.text = "Your Score \n" + fscore.ToString ();
				finishGame.gameObject.SetActive (true);
			}
		}

	}

	void SetScoreText(){
		scoreText.text = score.ToString ();
	}

	int getFinalScore (){
		if (!grounded) {
			grounded = true;
			moving = false;
			return score;
		} else {
			return 0;
		}
	}


	void createCoins(){
		foreach (Vector3 cp in coinPositions) {
			Instantiate (coin, cp, coinRotation);
		}
	}

	void startGamePlay(){

		//Debug.Log ("Game Started");
		startGame.gameObject.SetActive (false);
		finishGame.gameObject.SetActive (false);
		score = 0;
		rb.transform.SetPositionAndRotation (startingPosition,startingRotation);
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;
		grounded = false;
		moving = true;
		scoreText.enabled = true;
		SetScoreText ();
		createCoins ();

	}
}
