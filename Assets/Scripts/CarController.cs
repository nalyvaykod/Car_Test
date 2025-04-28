using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 8f;
    private Rigidbody rb;
    private bool isMoving;
    private Vector3 direction;

    void Awake()
    {
        rb = this.GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody is missing on this object.");
        }

        direction = transform.forward;
        direction = direction.normalized;
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
        }
    }

    void OnMouseDown()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager.Instance is null!");
            return;
        }

        if (GameManager.Instance.InputLocked) return;
        isMoving = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isMoving) return;

        isMoving = false;

        if (other.gameObject.CompareTag("Car"))
        {
            GameManager.Instance.LoseLife();
            Destroy(gameObject);
            GameManager.Instance.carsLeft--;
            AudioManager.Instance.PlaySfx("Crash");
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
