using UnityEngine;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour {
    public float initx;
    public float inity;
    public float angSpeed = 5f;
    public float velocity = 1.0f;
    public float minSpeed = 1.0f;
    public float maxSpeed = 4.0f;
    public float stop = 1f;
    public float breaking = 4.0f;
    public GameObject player;
    public Text comportamiento;

    private WeaponScript weapon;
    private PlayerScript playerS;
    private int switcher = 0;
    private float canTurn = 0.0f;
    private float maxAngle = 45.0f;
    private float angle = 0f;

    private void Start(){
        initx = transform.localScale.x;
        inity = transform.localScale.y;
        player = GameObject.Find("Player");

        playerS = player.GetComponent<PlayerScript>();
        weapon = GetComponentInChildren<WeaponScript>();
        weapon.enabled = false;

    }

    private void OnTriggerEnter2D(Collider2D collision){
        collision.gameObject.transform.localScale = new Vector3(initx + 0.3f, inity + 0.3f,0f);
    }

    private void OnTriggerExit2D(Collider2D collision){
        collision.gameObject.transform.localScale = new Vector3(initx, inity, 0f);
    }

    private void Update(){
        var target = player.transform.position;
        Vector3 distance = target - transform.position;

        if (Input.GetKey(KeyCode.LeftControl)){
            if (Input.GetKey(KeyCode.Q)){
                switcher = 5;
                comportamiento.text = "Movimiento simple";
            }else if(Input.GetKey(KeyCode.W)){
                switcher = 2;
                comportamiento.text = "Encarar";
            }
            else if (Input.GetKey(KeyCode.E)){
                switcher = 3;
                comportamiento.text = "Buscar y encarar";
            }
            else if (Input.GetKey(KeyCode.T)){
                switcher = 4;
                comportamiento.text = "Merodear";
            }
            else if (Input.GetKey(KeyCode.A)){
                switcher = 1;
                comportamiento.text = "Alinear";
            }
            else if (Input.GetKeyDown(KeyCode.U)){
                weapon.enabled = !weapon.enabled;
            }else if (Input.GetKey(KeyCode.D)){
                switcher = 0;
                comportamiento.text = "Estacionario";
            }
            else if (Input.GetKey(KeyCode.I)){
                switcher = 6;
                comportamiento.text = "Busqueda dinámica";
            }
            else if (Input.GetKey(KeyCode.Z)){
                switcher = 7;
                comportamiento.text = "Imitar";
            }
        }

        if (weapon.enabled){
            shoot();
        }

        collitionRays(distance);

        switch (switcher){
            case 7:
                align();
                alignVeloc();
                break;
            case 6:
                facing(distance);
                dynamicSeek();
                break;
            case 5:
                var move = new Vector3(1.0f, 1.0f, 0);
                simpleMove(move);
                break;
            case 4:
                wander();
                break;
            case 3:
                kinematicSeek(distance);
                facing(distance);
                break;
            case 2:
                facing(distance);
                break;
            case 1:
                align();
                break;
            default:
                break;
        }
    }

    private void wander(){
        velocity = 2;
        if (canTurn > 0.0f) {
            canTurn -= Time.deltaTime;
        } else {
            // cambiar dirección
            angle = Mathf.Deg2Rad * Random.Range(-maxAngle, maxAngle);
            canTurn = 2.0f;
        }
        transform.up += new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * velocity * Time.deltaTime;
        transform.position += transform.up * velocity * Time.deltaTime;
    }
    private void shoot(){
        if (weapon != null && weapon.enabled && weapon.canAttack){
            weapon.Attack(true);
        }
    }

    private void dynamicSeek(){
        transform.position += transform.up * velocity * Time.deltaTime;
    }

    private void kinematicSeek(Vector3 distance){
        
        if (distance.magnitude >= stop){
            transform.position += distance * Time.deltaTime;
        }
    }

    private void facing(Vector3 distance){
        float angle = Mathf.Atan2(-distance.x, distance.y) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * angSpeed);
    }
    private void simpleMove(Vector3 move){
        transform.position += move * velocity * Time.deltaTime;
    }

    private void controlVelocity(Vector3 facingDir){
        if (facingDir.magnitude > breaking){
            facingDir.Normalize();
            velocity += 1;
            if (velocity > maxSpeed){
                velocity = maxSpeed;
            }
        }else if (facingDir.magnitude > stop){
            facingDir.Normalize();
            velocity -= 1;
            if (velocity < minSpeed){
                velocity = minSpeed;
            }
        }else{
            velocity = 0.0f;
        }
    }

    private void align(){
        transform.rotation = Quaternion.Slerp(transform.rotation, player.transform.rotation, Time.deltaTime * angSpeed);
    }

    private void alignVeloc(){
        transform.position += playerS.velocity * Time.deltaTime;
    }

    private void collitionRays(Vector3 distance) {
        float cosadd = 0.349066f, sinadd = 0.349066f;
        int lm = 1 << LayerMask.NameToLayer("Objects"), dir = 0, dir2 = 0;
        Vector3 leftD, rightD;
        RaycastHit hitl, hit, hitr;

        // Ajuste de angulos para los rayos de colision
        if (transform.up.x < 0 && transform.up.y > 0) {
            leftD = new Vector3(Mathf.Cos(Mathf.Acos(transform.up.x) + cosadd),
                Mathf.Sin(Mathf.Asin(transform.up.y) - sinadd), 0);
            rightD = new Vector3(Mathf.Cos(Mathf.Acos(transform.up.x) - cosadd),
                Mathf.Sin(Mathf.Asin(transform.up.y) + sinadd), 0);
        }else if(transform.up.y < 0 && transform.up.x > 0) {
            leftD = new Vector3(Mathf.Cos(Mathf.Acos(transform.up.x) - cosadd),
                Mathf.Sin(Mathf.Asin(transform.up.y) + sinadd), 0);
            rightD = new Vector3(Mathf.Cos(Mathf.Acos(transform.up.x) + cosadd),
                Mathf.Sin(Mathf.Asin(transform.up.y) - sinadd), 0);
        }else if (transform.up.x <= 0) {
            leftD = new Vector3(Mathf.Cos(Mathf.Acos(transform.up.x) - cosadd),
                Mathf.Sin(Mathf.Asin(transform.up.y) - sinadd), 0);
            rightD = new Vector3(Mathf.Cos(Mathf.Acos(transform.up.x) + cosadd),
                Mathf.Sin(Mathf.Asin(transform.up.y) + sinadd), 0);
        }else{
            leftD = new Vector3(Mathf.Cos(Mathf.Acos(transform.up.x) + cosadd),
                Mathf.Sin(Mathf.Asin(transform.up.y) + sinadd), 0);
            rightD = new Vector3(Mathf.Cos(Mathf.Acos(transform.up.x) - cosadd),
                Mathf.Sin(Mathf.Asin(transform.up.y) - sinadd), 0);
        }

        // Left ray collides
        if (Physics.Raycast(transform.position, leftD, out hitl, 2f, lm)){
            Debug.DrawRay(transform.position, leftD * 2, Color.red);
            Debug.DrawRay(transform.position, hitl.normal * 2.5f, Color.blue);
            dir2 = -1;
        }else{
            Debug.DrawRay(transform.position, leftD * 2, Color.green);
            dir = 1;
        }

        // Rigth ray collides
        if (Physics.Raycast(transform.position, rightD, out hitr, 2f, lm)){
            Debug.DrawRay(transform.position, rightD * 2, Color.red);
            Debug.DrawRay(transform.position, hitr.normal * 2.5f, Color.blue);
            dir2 = -2;
        }else{
            Debug.DrawRay(transform.position, rightD * 2, Color.green);
            dir = 2;
        }
        
        // Center ray collides
        if (Physics.Raycast(transform.position, transform.up, out hit, 2.5f, lm)){
            Debug.DrawRay(transform.position, transform.up * 2.5f, Color.red);
            Debug.DrawRay(transform.position, hit.normal * 2.5f, Color.blue);
            if(dir == 0){
                dir = -3;
            }
        }else{
            Debug.DrawRay(transform.position, transform.up * 2.5f, Color.green);
            dir = 3;
        }
       
        if (dir == 1 || dir2 == -2){
            angSpeed = 0;
            transform.up = leftD * Time.deltaTime;
            transform.position += transform.up * 2 * Time.deltaTime;
            velocity = 0;
        }
        else if(dir == 2 || dir2 == -1){
            angSpeed = 0;
            transform.up = rightD * Time.deltaTime;
            transform.position += transform.up *2 * Time.deltaTime;
            velocity = 0;
        }else if (dir < 0){
            velocity = 0;
        }else {
            angSpeed = 3;
            if (switcher > 4){
                controlVelocity(distance);
            }
        }
    }

    private void behaviors(){
        //var move = new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f), 0);
        //transform.position += move * speed * Time.deltaTime;
        // Orientar hacia el target Inmediata
        //transform.rotation = Quaternion.AngleAxis(angulo, Vector3.forward);
    }
}
