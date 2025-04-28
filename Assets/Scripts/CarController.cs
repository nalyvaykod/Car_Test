using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 8f;
    private Rigidbody rb;
    private bool isMoving;
    private Vector3 direction;

    public void Init(Vector3 forward)
    {
        direction = forward.normalized;
        transform.forward = direction;
    }

    void Awake() => rb = GetComponent<Rigidbody>();

    void FixedUpdate()
    {
        if (isMoving)
        {
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
        }   
    }

    void OnCollisionEnter(Collision other)
    {
        if (!isMoving) return;

        isMoving = false;

        if (other.gameObject.CompareTag("Car"))
        {
            //Will be lose of life
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Boundary"))
        {
            //Exit
        }

        rb.MovePosition(rb.position - direction * 0.2f);
    }
}
