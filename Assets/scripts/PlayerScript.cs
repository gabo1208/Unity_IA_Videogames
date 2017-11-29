using UnityEngine;

public class PlayerScript : MonoBehaviour {
    public float speed = 1.0f;
    public float jumpSpeed = 10.0f;
    public float initx;
    public float inity;
    public Vector3 velocity;
    public bool jumping = false;
    private Vector3 gravity;
    public Rigidbody rb;
    public bool visible = true;

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
        // modify vector values by input in kew arrows
        var move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        // turn 'face' of player in mouse location
        var v3 = Input.mousePosition;
        v3.z = 0;

        var target = Camera.main.ScreenToWorldPoint(v3);
        velocity = move * speed;
        // move player in 'move' direction in given speed
        transform.position += move * speed * Time.deltaTime;

        // Turn face to target
        Vector3 facing = target - transform.position;
        float angulo = Mathf.Atan2(-facing.x, facing.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angulo, Vector3.forward);
    }

    void FixedUpdate(){
        // change gravity orientation
        Physics.gravity = gravity;

        // jump conditions
        if (Input.GetKeyDown(KeyCode.Space) && !jumping){
            jumping = true;
            // if junping --> go down
            rb.velocity += jumpSpeed * -new Vector3(0, 0, 1);
        }else if (transform.position.z > -0.6){
            jumping = false;
            transform.localScale = new Vector3(initx, inity, 1);
        }

        if (transform.position.z >= -1.6 && jumping){
            transform.localScale = new Vector3(
                initx - transform.position.z / 2.5f,
                inity - transform.position.z / 2.5f,
                1
            );
        }
    }
}