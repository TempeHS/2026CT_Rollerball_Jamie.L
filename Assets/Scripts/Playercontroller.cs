using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
 private Rigidbody rb; 

 private int count;

 private float movementX;
 private float movementY;

 public float jumpForce = 20;

 public float speed = 0;

 public TextMeshProUGUI countText;

 public GameObject winTextObject;

 private bool IsGrounded;

 void Start()
    {
        rb = GetComponent<Rigidbody>();

        count = 0;

        SetCountText();

        winTextObject.SetActive(false);
    }
 
 void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x; 
        movementY = movementVector.y; 
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
        Destroy(gameObject); 
 
        winTextObject.gameObject.SetActive(true);
        winTextObject.GetComponent<TextMeshProUGUI>().text = "you lose ha u suck";
 
    }
    if (collision.gameObject.CompareTag("Ground"))
        {
             IsGrounded = true;
        }

}

private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            IsGrounded = false;
        }
    }

}
