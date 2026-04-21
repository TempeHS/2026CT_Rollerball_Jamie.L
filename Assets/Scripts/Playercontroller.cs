using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    private int gatchaCount = 0;
 private Rigidbody rb; 

 private int count;

 private int lives;

 public float hitCooldown = 0f;

 private float hitCooldownDuration = 1f;

 private float movementX;
 private float movementY;

 public float jumpForce = 20;

 public float speed = 0;

 public TextMeshProUGUI countText;

public TextMeshProUGUI LivesText;

 public GameObject winTextObject;

 private bool IsGrounded;

 public int pity;

private bool inGatchaZone = false;

 public int best;
public TextMeshProUGUI RewardText;
public string[] rewards = { "Light Cone", "Mini Herta", "FIREFLY" };
public int luck = 0;
 void Start()
    {
        rb = GetComponent<Rigidbody>();

        count = 0;

        SetCountText();

        lives = 3;

        SetLivesText();

        winTextObject.SetActive(false);

        pity = 0;

        best = 0;


    }
 
 void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x; 
        movementY = movementVector.y; 
    }

    public string RollGacha()
    {
        int LightConeChance = 50 - luck;
        int MiniHertaChance = 25 + (luck / 2);
        int FIREFLYChance = 25 + (luck / 2);

        int roll = Random.Range(1, 101);
        Debug.Log("Gatcha roll: " + roll);

        if (roll <= FIREFLYChance)
            return rewards[2];
        else if (roll <= FIREFLYChance + MiniHertaChance)
            return rewards[1];
        else
            return rewards[0];
    }
 private void FixedUpdate() 
    {
        Vector3 movement = new Vector3 (movementX, 0.0f, movementY);

        rb.AddForce(movement * speed); 
    }

 
void OnJump(InputValue jumpValue)
    {
        if (jumpValue.isPressed && IsGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
 

 void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText();
        }
        if (other.gameObject.CompareTag("SpeedPickUp"))
        {
            other.gameObject.SetActive(false);
            speed = speed + 1;
        }
        if (other.gameObject.CompareTag("RandomPickup"))
        {
            other.gameObject.SetActive(false);
            pity = pity + 1;
        }
        if (pity == 5)
        {
            best = best + 1;
        }
        if (best == 1)
        {
            jumpForce = jumpForce + 10;
            pity = 0;
            best = 0;
        }
        if (other.gameObject.CompareTag("gatcha"))
        {
            inGatchaZone = true;
            Debug.Log("Entered gatcha zone");
        }
    }

void OnTriggerStay(Collider other)
{
    // No action needed here for gatcha
}

 void SetCountText() 
    {

        countText.text = "Count: " + count.ToString();

 if (count >= 13)
        {
            winTextObject.SetActive(true);
    
            Destroy(GameObject.FindGameObjectWithTag("Enemy"));
        }
    }

private void OnCollisionEnter(Collision collision)
{
 if (collision.gameObject.CompareTag("Enemy"))
    {

            lives = lives - 1;

            SetLivesText();
            hitCooldown = hitCooldownDuration;
    }
    if (collision.gameObject.CompareTag("Ground"))
        {
             IsGrounded = true;
        }
    if (lives<=0)
        {
            Destroy(GameObject.FindGameObjectWithTag("Player"));
            winTextObject.SetActive(true);
             winTextObject.GetComponent<TextMeshProUGUI>().text = "you lose haha";
        }


}

private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            IsGrounded = false;
        }
    }
    void SetLivesText()
    {
        LivesText.text = "Lives: " + lives.ToString();
    }
void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("gatcha"))
        {
            inGatchaZone = false;
            Debug.Log("Exited gatcha zone");
        }
    }
    void Update()
    {
        if (hitCooldown > 0f)
            hitCooldown -= Time.deltaTime;

        Debug.Log($"Update running, inGatchaZone: {inGatchaZone}");

        if (inGatchaZone && (
            (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
            || Input.GetKeyDown(KeyCode.E)))
        {
            Debug.Log("E pressed in gatcha zone");
            string result = RollGacha();
            gatchaCount++;
            RewardText.text = $"You got: {result} ({gatchaCount})";
        }
    }
}