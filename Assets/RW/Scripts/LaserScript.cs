using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour
{

    public Sprite laserOnSprite;
    public Sprite laserOffSprite;

    public float toggleInterval = 0.5f;
    public float rotationSpeed = 0.0f;

    private bool isLaserOn = true;
    private float timeUntilNextToggle;

    private Collider2D laserCollider;
    private SpriteRenderer laserRenderer;


    void Start()
    {
        timeUntilNextToggle = toggleInterval;
        laserCollider = GetComponent<Collider2D>();
        laserRenderer = GetComponent<SpriteRenderer>();
    }


    void Update()
    {

        timeUntilNextToggle -= Time.deltaTime;
        if (timeUntilNextToggle <= 0)
        {
            isLaserOn = !isLaserOn;
            laserCollider.enabled = isLaserOn;

            laserRenderer.sprite = (isLaserOn ? laserOnSprite : laserOffSprite);
            timeUntilNextToggle = toggleInterval;

        }



        transform.RotateAround(transform.position, transform.forward, rotationSpeed * Time.deltaTime);
    }
}
