using UnityEngine;

public class Car : MonoBehaviour {

	public Rigidbody2D rb;

	public float minSpeed = 8f;
	public float maxSpeed = 12f;
	float _speed = 1f;
	
	public float Timer = 10;

	void Start ()
	{
		_speed = Random.Range(minSpeed, maxSpeed);
	}

	void FixedUpdate ()
	{
		Timer -= Time.deltaTime;
		
		var car = new Vector2(transform.right.x, transform.right.y);
		transform.position += transform.right * _speed * Time.deltaTime;
		
		if (Timer <= 0)
		{
			Destroy(gameObject);
			//PhotonNetwork.Destroy(gameObject);
		}
	}
	


}
