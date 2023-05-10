using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MouseController : MonoBehaviour
{
    private Rigidbody2D PlayerRb; // Player RigidBody2D Reference
    public float jetpackForce = 75.0f;
    public float forwardMovementSpeed = 3.0f;
    public Transform groundCheckTransform;
    public bool isGrounded;
    public LayerMask groundCheckLayerMask;
    public Animator mouseAnimator;
    public ParticleSystem jetpack;
    public bool isDead = false;
    public uint coins = 0;
    public bool check = false;

    public AudioClip coinCollectSound;

    public TextMeshProUGUI coinsCollectedLabel;

    public AudioSource jetpackAudio;
    public AudioSource footstepsAudio;

    public ParallaxScroll parallax;

    public Image RestartDialog;

    public Image ExitDialog;



    void Start()
    {
        PlayerRb = GetComponent<Rigidbody2D>();
        mouseAnimator = GetComponent<Animator>();

        // -----Getting Game Volume From Player Prefs----------
        float GameVol = PlayerPrefs.GetFloat("GameVolume");
        //--------Setting Game Volume Accordingly-------------
        footstepsAudio.volume = GameVol;
        Camera.main.GetComponent<AudioSource>().volume = GameVol;

    }

    //-----------Checking If Player Is Grounded Or Not-----------

    void UpdateGroundedStatus()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheckTransform.position, 1f, groundCheckLayerMask);
        mouseAnimator.SetBool("isGrounded", isGrounded);
    }

    //----------Back To Menu------------------

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    //-------------Exit game-----------------

    public void ExitGame()
    {
        RestartDialog.gameObject.SetActive(false);
        ExitDialog.gameObject.SetActive(true);
        check = true;
    }

    //----------On Yes-----------------

    public void onYes()
    {
        Application.Quit();
    }

    //---------On No-----------------

    public void OnNo()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void FixedUpdate()
    {

        bool jetpackActive = Input.GetButton("Fire1"); // Getting Fire Button Input
        jetpackActive = jetpackActive && !isDead; // setting jetPack active if player is not dead
        if (jetpackActive)
            PlayerRb.AddForce(new Vector2(0, jetpackForce)); // adding force on jetpack active
        if (!isDead)
        {
            Vector2 newVelocity = PlayerRb.velocity;  // adding velocity to player
            newVelocity.x = forwardMovementSpeed;
            PlayerRb.velocity = newVelocity;
        }

        if (isDead && isGrounded && !check)
        {

            RestartDialog.gameObject.SetActive(true);  // showing restart dialog when player is dead and falls on ground

        }

        UpdateGroundedStatus();
        AdjustJetpack(jetpackActive);
        AdjustFootstepsAndJetpackSound(jetpackActive);
        parallax.offset = transform.position.x;

    }

    //-------------------Restarting Game--------------------------

    public void RestartGame()
    {
        SceneManager.LoadScene("RocketMouse");
    }



    void AdjustFootstepsAndJetpackSound(bool jetpackActive)
    {
        footstepsAudio.enabled = !isDead && isGrounded;
        jetpackAudio.enabled = !isDead && !isGrounded;
        if (jetpackActive)
        {
            //jetpackAudio.volume = 1.0f;
            jetpackAudio.volume = PlayerPrefs.GetFloat("GameVolume");
        }
        else
        {
            //jetpackAudio.volume = 0.5f;
            jetpackAudio.volume = PlayerPrefs.GetFloat("GameVolume") * 0.5f;
        }
    }

    //--------------Adjusting JetPack Emission----------------

    void AdjustJetpack(bool jetpackActive)
    {
        var jetpackEmission = jetpack.emission;
        jetpackEmission.enabled = !isGrounded;
        if (jetpackActive)
        {
            jetpackEmission.rateOverTime = 300.0f;
        }
        else
        {
            jetpackEmission.rateOverTime = 75.0f;
        }


    }

    // ------------Coin Collection--------------------

    void CollectCoin(Collider2D coinCollider)
    {
        coins++;
        coinsCollectedLabel.text = coins.ToString();
        Destroy(coinCollider.gameObject);
        AudioSource.PlayClipAtPoint(coinCollectSound, transform.position, PlayerPrefs.GetFloat("GameVolume"));
    }

    //----------------Detecting Collisions On Trigger-------------------

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Coins"))
        {
            CollectCoin(collider);
        }
        else
            HitByLaser(collider);
    }

    //-------------Hitting By Laser----------------

    void HitByLaser(Collider2D laserCollider)
    {
        if (!isDead)
        {
            AudioSource laserZap = laserCollider.gameObject.GetComponent<AudioSource>();
            laserZap.volume = PlayerPrefs.GetFloat("GameVolume");
            laserZap.Play();
        }


        isDead = true;
        mouseAnimator.SetBool("isDead", true);

    }




}
