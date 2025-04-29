using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField] private float moveSpeed = 8f;      

    private Rigidbody rb;
    private bool isMoving;
    private Vector3 direction;                       


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
            Debug.LogError($"{name}: Rigidbody component is missing!");

        direction = transform.forward.normalized;
    }

    private void FixedUpdate()
    {
        if (isMoving)
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
    }


    private void OnMouseDown()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager.Instance is null!");
            return;
        }

        if (GameManager.Instance.InputLocked) return;

        isMoving = true;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!isMoving) return;

        isMoving = false;                   

        if (other.CompareTag("Car"))
        {
            GameManager.Instance.CarCrashed();
            GameManager.Instance.LoseLife();

            AudioManager.Instance?.PlaySfx("Crash");
            Destroy(gameObject);
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Boundary"))
        {
            GameManager.Instance.RegisterExit();
            Destroy(gameObject);
        }

        rb.MovePosition(rb.position - direction * 0.2f);
    }


    public void Init(Vector3 dir)
    {
        direction = dir.normalized;
        transform.rotation = Quaternion.LookRotation(direction);
    }
}
