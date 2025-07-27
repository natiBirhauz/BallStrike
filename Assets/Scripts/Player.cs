using UnityEngine;
using System.Collections;
using TMPro;
using Microsoft.Unity.VisualStudio.Editor;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public GameManager gm;
    public float forwardSpeed = 15f;
    public float baseSpeed = 15f;
    public float sideSpeed = 10f;
    public float maxZSpeed = 100f;
    public float leftLimit = -5f;
    public float rightLimit = 5f;

    private float hitCooldown = 0.5f;
    private float lastHitTime = 0f;
    private bool isInvulnerable = false;
    private Renderer rend;
    public GameObject HitAnimetion;
    public GameObject ballTextAnimation;
    public TextMeshProUGUI animetionText;
    public List<Material> materials;              // Assign your materials in the Inspector

    private int currentIndex = 0;

    private MaterialPropertyBlock propBlock;
    private void Awake()
    {
        rend = GetComponent<Renderer>();
        propBlock = new MaterialPropertyBlock();

        if (materials != null && materials.Count > 0 && rend != null)
        {
            rend.material = materials[0]; // Start with the first material
        }
    }

private IEnumerator OverlayColorTemporarily(Color overlayColor, float duration = 0.5f)
    {
        if (rend == null) yield break;

        rend.GetPropertyBlock(propBlock);

        // Set overlay tint
        propBlock.SetColor("_BaseColor", overlayColor); // Use _BaseColor in URP
        rend.SetPropertyBlock(propBlock);

        yield return new WaitForSeconds(duration);

        // Reset to original color (fully opaque)

    propBlock.SetColor("_BaseColor", new Color(1f, 1f, 1f, 1f));
        rend.SetPropertyBlock(propBlock);
    }

    void Start()
    {
        if (gm == null)
            gm = Object.FindFirstObjectByType<GameManager>();

        if (gm != null && gm.IsInitialized())
            gm.InitializePlayer(this);

        rend = GetComponent<Renderer>();
        // if (rend != null)
        //  originalColor = rend.material.color;
    }

    void FixedUpdate()
    {
        if (gm == null || !gm.IsInitialized() || !GameStateManager.Instance.IsPlaying()) return;

        Vector3 newPosition = transform.position;
        newPosition += Vector3.forward * forwardSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            newPosition += Vector3.left * sideSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            newPosition += Vector3.right * sideSpeed * Time.deltaTime;

        newPosition.x = Mathf.Clamp(newPosition.x, leftLimit, rightLimit);
        transform.position = newPosition;
    }
    public void ChangeMaterial()
    {
        if (materials == null || materials.Count == 0)
            return;

        currentIndex = (currentIndex + 1) % materials.Count;
        if (rend != null)
            rend.material = materials[currentIndex];

    }

    private void OnTriggerEnter(Collider other)
    {
        if (isInvulnerable) return;

        if (other.CompareTag("EnemyBall"))
        {
            if (Time.time - lastHitTime > hitCooldown)
            {
                var ball = other.GetComponent<EnemyBall>();
                if (ball != null)
                {
                    ball.Hit();
                    HitAnimetion.SetActive(true);
                    animetionText.text = "-1 health";
                    animetionText.color = Color.red;
                    ballTextAnimation.GetComponent<UnityEngine.UI.Image>().color = new Color32(255, 0, 0, 10);
                    ballTextAnimation.SetActive(true);
                    Invoke("SettTextAnimetionFalse", 0.5f);

                    //set back the hit animetion to false after 0.5 seconds
                    Invoke("SetHitAnimetionFalse", 0.5f);

                    lastHitTime = Time.time;

                    StartCoroutine(BecomeTemporarilyTransparent());
                }
            }
        }
        else if (other.CompareTag("HealthBall"))
        {
            var ball = other.GetComponent<HealthBall>();
            if (ball != null)
                ball.Hit();
            animetionText.text = "+1 health";
            animetionText.color = Color.yellow;
            ballTextAnimation.GetComponent<UnityEngine.UI.Image>().color = new Color32(255, 255, 0, 20);

            ballTextAnimation.SetActive(true);
            Invoke("SettTextAnimetionFalse", 0.5f);
        }
        else if (other.CompareTag("PointsBall"))
        {
            var ball = other.GetComponent<PointsBall>();
            if (ball != null)
                ball.Hit();
            animetionText.text = "+200 points";
            animetionText.color = Color.magenta;
            ballTextAnimation.GetComponent<UnityEngine.UI.Image>().color = new Color32(255, 0, 255, 20);

            ballTextAnimation.SetActive(true);
            Invoke("SettTextAnimetionFalse", 0.5f);
        }
        else if (other.CompareTag("IceBall"))
        {
            var ball = other.GetComponent<IceBall>();
            if (ball != null)
            {
                ball.Hit();
                animetionText.text = "slowed down";
                animetionText.color = Color.blue;
                ballTextAnimation.GetComponent<UnityEngine.UI.Image>().color = new Color32(0, 255, 255, 20);

                ballTextAnimation.SetActive(true);

                Invoke("SettTextAnimetionFalse", 0.5f);
                lastHitTime = Time.time;
                StartCoroutine(BecomeTemporarilyIce());
            }
        }
        else if (other.CompareTag("FireBall"))
        {
            var ball = other.GetComponent<FireBall>();
            if (ball != null)
            {
                ball.Hit();
                animetionText.text = "speed up";
                animetionText.color = Color.red;
                ballTextAnimation.GetComponent<UnityEngine.UI.Image>().color = new Color32(255, 100, 0, 20);

                ballTextAnimation.SetActive(true);
                Invoke("SettTextAnimetionFalse", 0.5f);
                lastHitTime = Time.time;
                StartCoroutine(BecomeTemporarilFire());
            }

        }
        else if (other.CompareTag("MagnetBall"))
        {
            var ball = other.GetComponent<MagnetBall>();
            if (ball != null)
            {
                ball.Hit();
                animetionText.text = "star magnet";
                animetionText.color = Color.green;
                ballTextAnimation.GetComponent<UnityEngine.UI.Image>().color = new Color32(0, 255, 0, 20);

                ballTextAnimation.SetActive(true);
                Invoke("SettTextAnimetionFalse", 0.5f);
                lastHitTime = Time.time;
                StartCoroutine(BecomeTemporarilyMagnet());
            }

        }
        else if (other.CompareTag("Star"))
        {
            var star = other.GetComponent<Star>();
            if (star != null)
            {
                star.Hit();
                animetionText.text = "+500 points";
                animetionText.color = Color.yellow;
                ballTextAnimation.GetComponent<UnityEngine.UI.Image>().color = new Color32(255, 255, 0, 20);
                ballTextAnimation.SetActive(true);
                Invoke("SettTextAnimetionFalse", 0.5f);

            }

        }
    }

    private IEnumerator BecomeTemporarilyTransparent() //enamy ball
    {

        yield return StartCoroutine(OverlayColorTemporarily(new Color(1f, 0f, 0f, 0.5f))); // gray

    }
    private IEnumerator BecomeTemporarilyIce()
    {
        yield return StartCoroutine(OverlayColorTemporarily(new Color(0f, 0.5f, 1f, 0.5f))); // semi-transparent blue
    }

    private IEnumerator BecomeTemporarilFire()
    {
        isInvulnerable = true;
        yield return StartCoroutine(OverlayColorTemporarily(new Color(0.5f, 0f, 0f, 0.5f))); // semi-transparent red
        isInvulnerable = false;
    }

    private IEnumerator BecomeTemporarilyMagnet()
    {
        yield return StartCoroutine(OverlayColorTemporarily(new Color(0.5f, 0.5f, 0.5f, 0.5f))); // gray
    }


    private void SetHitAnimetionFalse()
    {
        if (HitAnimetion != null)
            HitAnimetion.SetActive(false);
    }
    private void SettTextAnimetionFalse()
    {
        if (ballTextAnimation != null)
            ballTextAnimation.SetActive(false);
    }

}
