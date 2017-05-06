using UnityEngine;
using UnityEngine.UI;

public class TankShooting : MonoBehaviour
{
    public int playerNumber = 1;       
    public Rigidbody shell; 
    public Transform fireTransform;    
    public Slider aimSlider;           
    public AudioSource shootingAudio;  
    public AudioClip chargingClip;     
    public AudioClip fireClip;   

    public float minLaunchForce = 15f; 
    public float maxLaunchForce = 30f; 
    public float maxChargeTime = 0.75f;
    
    private string fireButton;         
    private float currentLaunchForce;  
    private float chargeSpeed;         
    private bool fired;                


    private void OnEnable()
    {
        currentLaunchForce = minLaunchForce;
        aimSlider.value = minLaunchForce;
    }


    private void Start()
    {
        fireButton = "Fire" + playerNumber;

        chargeSpeed = (maxLaunchForce - minLaunchForce) / maxChargeTime;
    }
    
	// Track the current state of the fire button and make decisions based on the current launch force.
    private void Update()
    {
		// The slider should have a default value of the minimum launch force.
		aimSlider.value = minLaunchForce;

		// If the max force has been exceeded and the shell hasn't yet been launched...
		if (currentLaunchForce >= maxLaunchForce && !fired)
		{
			// ... use the max force and launch the shell.
			currentLaunchForce = maxLaunchForce;
			Fire ();
		}
		// Otherwise, if the fire button has just started being pressed...
		else if (Input.GetButtonDown (fireButton))
		{
			// ... reset the fired flag and reset the launch force.
			fired = false;
			currentLaunchForce = minLaunchForce;

			// Change the clip to the charging clip and start it playing.
			shootingAudio.clip = chargingClip;
			shootingAudio.Play ();
		}
		// Otherwise, if the fire button is being held and the shell hasn't been launched yet...
		else if (Input.GetButton (fireButton) && !fired)
		{
			// Increment the launch force and update the slider.
			currentLaunchForce += chargeSpeed * Time.deltaTime;

			aimSlider.value = currentLaunchForce;
		}
		// Otherwise, if the fire button is released and the shell hasn't been launched yet...
		else if (Input.GetButtonUp (fireButton) && !fired)
		{
			// ... launch the shell.
			Fire ();
		}
    }

	// Instantiate and launch the shell.
    private void Fire()
    {
		// Set the fired flag so only Fire is only called once.
		fired = true;

		// Create an instance of the shell and store a reference to it's rigidbody.
		Rigidbody shellInstance = Instantiate (shell, fireTransform.position, fireTransform.rotation) as Rigidbody;

		// Set the shell's velocity to the launch force in the fire position's forward direction.
		shellInstance.velocity = currentLaunchForce * fireTransform.forward;

		// Change the clip to the firing clip and play it.
		shootingAudio.clip = fireClip;
		shootingAudio.Play ();

		// Reset the launch force.  This is a precaution in case of missing button events.
		currentLaunchForce = minLaunchForce;
    }
}