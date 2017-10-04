using UnityEngine;

public class PlayerScript : MonoBehaviour {
    public float speed = 1.0f;
    public float jumpSpeed = 10.0f;
    public float initx;
    public float inity;
    public float time = 1.0f;
    public float jumpRate = 0.5f;
    public Vector3 velocity;
    private bool jumping = false;
    private Vector3 gravity;
    public Rigidbody rb;

    // Use this for initialization
    void Start () {
        initx = transform.localScale.x;
        inity = transform.localScale.y;
        rb = GetComponent<Rigidbody>();
        gravity = Physics.gravity;
        gravity.z = -gravity.y;
        gravity.x = 0;
        gravity.y = 0;
        Physics.gravity = gravity;
    }

    void Update(){
        var move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        var v3 = Input.mousePosition;
        v3.z = 0;
        var target = Camera.main.ScreenToWorldPoint(v3);
        velocity = move * speed;
        transform.position += move * speed * Time.deltaTime;

        Vector3 facing = target - transform.position;
        // Formula para facing
        float angulo = Mathf.Atan2(-facing.x, facing.y) * Mathf.Rad2Deg;
        // Orientar hacia el target
        transform.rotation = Quaternion.AngleAxis(angulo, Vector3.forward);
    }

    void FixedUpdate(){
        Physics.gravity = gravity;
        if (Input.GetKeyDown(KeyCode.Space) && !jumping){
            jumping = true;
            rb.velocity += jumpSpeed * -new Vector3(0, 0, 1);
        }

        if (transform.position.z > -0.6){
            jumping = false;
            transform.localScale = new Vector3(initx, inity, 1);
        }

        transform.localScale = new Vector3(initx - transform.position.z / 2.5f, inity - transform.position.z / 2.5f, 1);

        if (transform.position.z <= -2.5){
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, 10);
        }
    }
}
