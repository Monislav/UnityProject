using UnityEngine;
using System.Collections;

public class CarScript : MonoBehaviour
{
	public static bool IsAlive;

    public GuiManager GuiManager;
	public GameObject Explosion;
    private float _moveSpeed = 0.05f;
	private float _zMax = 7.19f;
	private float _zMin = 1.61f;
	private float _xMin = 10.53f;
	private float _xMax = 18.36f;
	private float _yCoordinate = 0.415f;
    public int Score;
    float _gameTime;
    AudioSource coinSound;
    Vector3 currPos;

	// Use this for initialization
	void Start ()
    {
        coinSound = gameObject.GetComponent<AudioSource>();
        IsAlive = true;
        Score = PlayerPrefs.GetInt("Score");
        GuiManager.ScoreLbl.text = string.Format("Score : {0}", Score.ToString());
        _gameTime = 0f;
    }
	
	// Update is called once per frame
	void Update () 
    {
        GuiManager.ScoreLbl.text = "Score : " + PlayerPrefs.GetInt("Score");
        if (IsAlive)
        {
            ManageInput();
            ManageLimitations();
            ManageSensitivity();
        }
	}

    void ManageInput()
    {
        transform.position += new Vector3(Input.GetAxis("Horizontal") * _moveSpeed , 0, Input.GetAxis("Vertical") * _moveSpeed);
    }

    void ManageLimitations()
    {
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, _xMin, _xMax),_yCoordinate, Mathf.Clamp(transform.position.z, _zMin, _zMax));
    }

    void ManageSensitivity()
    {
        if (Time.timeSinceLevelLoad > (_gameTime + 15))
        {
            _gameTime += 15;
            _moveSpeed += 0.01f;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Enemy")
        {
            Crash();
            currPos = gameObject.transform.position;
			col.gameObject.SetActive(false);
            gameObject.SetActive(false);
            Explosion.transform.position = currPos;
            Explosion.SetActive(true);
        }
        else if (col.tag == "Coin")
        {
            coinSound.Play();
            Score++;
            PlayerPrefs.SetInt("Score", Score);
            col.gameObject.SetActive(false);
        }
        else if (col.tag == "GreenCoin")
        {
            coinSound.Play();
            Score += 2;
            PlayerPrefs.SetInt("Score", Score);
            col.gameObject.SetActive(false);
        }
    }

    void Crash()
    {
        IsAlive = false;
        GuiManager.ShowDeadText();
    }

}
