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

    private void Update()
    {
        // Track the current state of the fire button and make decisions based on the current launch force.
        aimSlider.value = minLaunchForce;

        if (currentLaunchForce >= maxLaunchForce && !fired)
        {
            // at max charge, not yet fired
            currentLaunchForce = maxLaunchForce;
            Fire();
        }
        else if (Input.GetButtonDown(fireButton))
        {
            // have we pressed fire for the first time?
            fired = false;
            currentLaunchForce = minLaunchForce;

            shootingAudio.clip = chargingClip;
            shootingAudio.Play();
        }
        else if (Input.GetButton(fireButton) && !fired)
        {
            // Hilding the fire button, not yet fired
            currentLaunchForce += chargeSpeed * Time.deltaTime;

            aimSlider.value = currentLaunchForce;
        }
        else if (Input.GetButtonUp(fireButton) && !fired)
        {
            // we released the button, having not fired yet
            Fire();
        }
    }


    private void Fire()
    {
        // Instantiate and launch the shell.
        fired = true;

        Rigidbody shellInstance = Instantiate(shell, fireTransform.position, fireTransform.rotation) as Rigidbody;

        shellInstance.velocity = currentLaunchForce * fireTransform.forward;

        shootingAudio.clip = fireClip;
        shootingAudio.Play();

        currentLaunchForce = minLaunchForce;
    }
}