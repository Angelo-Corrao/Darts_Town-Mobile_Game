using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/*
 * Manages the flight of the dart and ita landing on the Target
 */

public class Dart : MonoBehaviour
{
	public delegate void OnTargetHit(Collision collision);
	public static event OnTargetHit onTargetHit;

	private Rigidbody rb;
	private bool startTimer;

	private float alpha;
	private float timer;
	private float timeMax;

	private bool collisionEntered = false;

	private void OnEnable()
	{
		rb = gameObject.GetComponent<Rigidbody>();
		SwipeGesture.onSwipe += ThrowDart;
		collisionEntered = false;
	}

	private void OnDisable()
	{
		SwipeGesture.onSwipe -= ThrowDart;
	}

	private void Update()
	{
		if (startTimer)
		{
			timer += Time.deltaTime;
			transform.eulerAngles = Vector3.Lerp(new Vector3(-alpha, 0, 0), new Vector3(alpha, 0, 0), timer / timeMax);
			if (timer >= timeMax)
				startTimer = false;
		}

	}

	private void ThrowDart(float verticalOffset, float swipeOffsetX)
	{

		GetComponent<SwipeGesture>().enabled = false;
		rb.useGravity = true;
		timer = 0;
		startTimer = true;
		
		float r = verticalOffset >= 0 ? (GameManager.instance.target.position - transform.position).z + verticalOffset : (GameManager.instance.target.position - transform.position).z + verticalOffset;//distanza tra freccetta e bersaglio
		float g = -Physics.gravity.y;
		alpha = 20;
		float alphaRad = Mathf.Deg2Rad * alpha;


		float u = Mathf.Sqrt(r * g / Mathf.Sin(2 * alphaRad)); 
		timeMax = Mathf.Sin(alphaRad) * u * 2 / g;

		float c1 = DartGenerator.instance.image.localScale.x/2;

		float i = Mathf.Sqrt(c1 * c1 + r * r);

		float horizontalOffset = Mathf.Asin(c1 / i) * Mathf.Rad2Deg;

		float horizontalAngle = swipeOffsetX < 0 ? Mathf.Lerp(0, -horizontalOffset, Mathf.Abs(swipeOffsetX) / Screen.width * 2) : Mathf.Lerp(0, horizontalOffset, Mathf.Abs(swipeOffsetX) / Screen.width * 2);
		transform.eulerAngles = new Vector3(-alpha, horizontalAngle / 2, 0);
		rb.AddForce(transform.forward * u, ForceMode.Impulse);
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Target" || collision.gameObject.layer == 8)
		{
			rb.useGravity = false;
			rb.isKinematic = true;
			
			gameObject.GetComponent<Dart>().enabled = false;

			if (!collisionEntered)
			{
				collisionEntered = true;
				DartGenerator.instance.GenerateNewDart();
				onTargetHit?.Invoke(collision);
			}
		}

        if (GameManager.instance.tutorial)
        {
            if (TutorialGrandmaHouse.instance != null)
                TutorialGrandmaHouse.instance.ChangeText();

            if (TutorialPub.instance != null)
                TutorialPub.instance.ChangeText();
        }

    }
}