using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class EnemyScript : MonoBehaviour
{
    public float initx;
    public float inity;
    public float angSpeed = 5f;
    public float velocity = 1.0f;
    public float minSpeed = 1.0f;
    public float maxSpeed = 4.0f;
    public float stop = 1f;
    public float breaking = 4.0f;
    public GameObject player;
    public float distanceVel = 0f;
    public float separateThreshold = 0f;
    public float pathThreshold = 0f;
    public float collitionThreshold = 0f;

    private WeaponScript weapon;
    private PlayerScript playerS;
    public int switcher = 0;
    private float canTurn = 0.0f;
    private float maxAngle = 45.0f;
    private float angle = 0f;
    public List<GameObject> path;
    private GameObject[] closers;
    private Vector3 direction, target;
    private NavMeshScript navmeshScript;

    private void Start()
    {
        initx = transform.localScale.x;
        inity = transform.localScale.y;
        // we need to know where player is
        player = GameObject.Find("Player");
        playerS = player.GetComponent<PlayerScript>();
        // we need to know where enemies are
        closers = GameObject.FindGameObjectsWithTag("Enemies");
        // deactivate the attached weapon
        weapon = GetComponentInChildren<WeaponScript>();
        weapon.enabled = false;
        // render triangle
        navmeshScript = GetComponent<NavMeshScript>();
    }

    private void Update()
    {
        // if not path following, choose behaviors
        if (switcher != 9){
            target = player.transform.position;
            direction = target - transform.position;
        }

        // behaviors by key input
        if (Input.GetKey(KeyCode.LeftControl)){
            if (Input.GetKey(KeyCode.Q)){
                switcher = 2;
                //"Encarar";
            } else if (Input.GetKey(KeyCode.W)){
                switcher = 3;
                //"Buscar y encarar";
            } else if (Input.GetKey(KeyCode.E)){
                switcher = 4;
                //"Merodear";
            } else if (Input.GetKey(KeyCode.A)){
                switcher = 1;
                //"Alinear";
            } else if (Input.GetKeyDown(KeyCode.T)){
                weapon.enabled = !weapon.enabled;
            } else if (Input.GetKey(KeyCode.D)){
                switcher = 0;
                //"Estacionario";
            } else if (Input.GetKey(KeyCode.Z)){
                switcher = 6;
                //"Busqueda dinámica";
            } else if (Input.GetKey(KeyCode.X)){
                switcher = 7;
                //"Imitar";
            } else if (Input.GetKey(KeyCode.C)){
                switcher = 8;
                //"Pursue";
            } else if (Input.GetKey(KeyCode.G)){
                switcher = 9;
                //"Follow Path";
            }
        }
        // weapon activated, shoot
        if (weapon.enabled){
            shoot();
        }

        separation();
        controlVelocity(direction);
        //collitionRays(direction);

        // call to behavior choose
        switch (switcher){
            case 9:
                pathFollowing();
                break;
            case 8:
                pursue(direction);
                break;
            case 7:
                align();
                alignVeloc();
                break;
            case 6:
                faceTo(direction);
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
                kinematicSeek(direction);
                faceTo(direction);
                break;
            case 2:
                faceTo(direction);
                break;
            case 1:
                align();
                break;
            default:
                break;
        }
    }

    // Wander movement
    private void wander()
    {
        velocity = 2;
        if (canTurn > 0.0f){
            canTurn -= Time.deltaTime;
        } else{
            // cambiar dirección
            angle = Mathf.Deg2Rad * Random.Range(-maxAngle, maxAngle);
            canTurn = 2.0f;
        }
        faceTo(new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * Time.deltaTime);
        transform.position += transform.up * velocity * Time.deltaTime;
    }

    // Shoot mechanism
    private void shoot(){
        if (weapon != null && weapon.enabled && weapon.canAttack){
            weapon.Attack(true);
        }
    }

    // Dynamic seek movement
    private void dynamicSeek(){
        transform.position += transform.up * velocity * Time.deltaTime;
    }

    // Dynamic flee movement
    private void dynamicFlee(){
        transform.position += -transform.up * velocity * Time.deltaTime;
    }

    // Kinematic seek movement
    private void kinematicSeek(Vector3 direction){
        if (direction.magnitude >= stop){
            transform.position += direction * Time.deltaTime;
        }
    }

    // Face to 
    private void faceTo(Vector3 direction){
        float angle = Mathf.Atan2(-direction.x, direction.y) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * angSpeed);
    }

    // Simple Move
    private void simpleMove(Vector3 move){
        transform.position += move * velocity * Time.deltaTime;
    }

    // Control velocity for character
    private void controlVelocity(Vector3 faceToDir){
        if (faceToDir.magnitude > breaking){
            faceToDir.Normalize();
            velocity += 1 * Time.deltaTime;
            if (velocity > maxSpeed){
                velocity = maxSpeed;
            }
        } else if (faceToDir.magnitude > stop){
            faceToDir.Normalize();
            velocity -= 1 * Time.deltaTime;
            if (velocity < minSpeed){
                velocity = minSpeed;
            }
        } else {
            if (velocity <= 0) {
                velocity = 0.0f;
            } else {
                velocity -= 2 * Time.deltaTime;
            }
        }
    }

    // faceTo align to target
    private void align(){
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            player.transform.rotation,
            Time.deltaTime * angSpeed
        );
    }

    // Velocity align to target
    private void alignVeloc(){
        transform.position += playerS.velocity * Time.deltaTime;
    }

    // Pursue movement
    private void pursue(Vector3 direction){
        float distance = direction.magnitude;
        float maxPrediction = 3;
        float prediction;
        Vector3 newPos;
        //Check if speed is too small to give a reasonable prediction time
        if (velocity <= distance / maxPrediction){
            prediction = maxPrediction;
            //Otherwise calculate the prediction time
        } else {
            prediction = distance / velocity;
        }
        //Put the target together

        newPos = player.transform.position + playerS.velocity * prediction;
        direction = newPos - transform.position;
        //Delegate to seek  
        faceTo(direction);
        dynamicSeek();
    }

    public void pathFollowing(){
        if (path.Count > 0){
            target = path[0].transform.position;
            direction = target - transform.position;
            if (direction.magnitude > pathThreshold){
                faceTo(direction);
                dynamicSeek();
            } else {
                if (path.Count > 0){
                    path[0].GetComponent<NodeScript>().active = true;
                    path.RemoveAt(0);
                }
                navmeshScript.removeFirstInPath();
            }
        }
    }

    private void separation(){
        float strength = 0f;
        float distance = 0f;
        Vector3 direction = new Vector3(0,0,0);
        for (var i = 0; i < closers.Length; i++){
            if (closers[i].transform != transform){
                direction = closers[i].transform.position - transform.position;
                distance = direction.magnitude;
                if (distance < separateThreshold){
                    strength = Mathf.Min(distanceVel / (distance * distance), maxSpeed);
                    direction.Normalize();
                    //faceTo(-direction);
                    transform.position += -direction * strength * Time.deltaTime;

                }
            }
        }
    }

    private void separateFromTarget(Vector3 target){
        float strength = 0f;
        Vector3 direction = target - transform.position;
        float distance = direction.magnitude;
        if (distance < collitionThreshold){
            strength = Mathf.Min(distanceVel / (distance * distance), maxSpeed);
            direction.Normalize();
            faceTo(-direction);
            transform.position += -direction * strength * Time.deltaTime;
        }
    }

    // Collition detection
    private void collitionRays(Vector3 direction){
        float cosadd = 0.349066f, sinadd = 0.349066f;
        int lm = 1 << LayerMask.NameToLayer("Objects");
        Vector3 leftD, rightD;
        RaycastHit hitl, hit, hitr;

        // Ajuste de angulos para los rayos de colision
        if (transform.up.x < 0 && transform.up.y > 0){
            leftD = new Vector3(Mathf.Cos(Mathf.Acos(transform.up.x) + cosadd),
                Mathf.Sin(Mathf.Asin(transform.up.y) - sinadd), 0);
            rightD = new Vector3(Mathf.Cos(Mathf.Acos(transform.up.x) - cosadd),
                Mathf.Sin(Mathf.Asin(transform.up.y) + sinadd), 0);
        } else if (transform.up.y < 0 && transform.up.x > 0) {
            leftD = new Vector3(Mathf.Cos(Mathf.Acos(transform.up.x) - cosadd),
                Mathf.Sin(Mathf.Asin(transform.up.y) + sinadd), 0);
            rightD = new Vector3(Mathf.Cos(Mathf.Acos(transform.up.x) + cosadd),
                Mathf.Sin(Mathf.Asin(transform.up.y) - sinadd), 0);
        } else if (transform.up.x <= 0) {
            leftD = new Vector3(Mathf.Cos(Mathf.Acos(transform.up.x) - cosadd),
                Mathf.Sin(Mathf.Asin(transform.up.y) - sinadd), 0);
            rightD = new Vector3(Mathf.Cos(Mathf.Acos(transform.up.x) + cosadd),
                Mathf.Sin(Mathf.Asin(transform.up.y) + sinadd), 0);
        } else {
            leftD = new Vector3(Mathf.Cos(Mathf.Acos(transform.up.x) + cosadd),
                Mathf.Sin(Mathf.Asin(transform.up.y) + sinadd), 0);
            rightD = new Vector3(Mathf.Cos(Mathf.Acos(transform.up.x) - cosadd),
                Mathf.Sin(Mathf.Asin(transform.up.y) - sinadd), 0);
        }

        // Left ray collides
        if (Physics.Raycast(transform.position, leftD, out hitl, 2f, lm)){
            Debug.DrawRay(transform.position, leftD * 2, Color.red);
            Debug.DrawRay(transform.position, hitl.normal * 2.5f, Color.blue);
            separateFromTarget(hitl.point);
        } else {
            Debug.DrawRay(transform.position, leftD * 2, Color.green);
        }

        // Rigth ray collides
        if (Physics.Raycast(transform.position, rightD, out hitr, 2f, lm)){
            Debug.DrawRay(transform.position, rightD * 2, Color.red);
            Debug.DrawRay(transform.position, hitr.normal * 2.5f, Color.blue);
            separateFromTarget(hitr.point);
        } else {
            Debug.DrawRay(transform.position, rightD * 2, Color.green);
        }

        // Center ray collides
        if (Physics.Raycast(transform.position, transform.up, out hit, 2.5f, lm)) {
            Debug.DrawRay(transform.position, transform.up * 2.5f, Color.red);
            Debug.DrawRay(transform.position, hit.normal * 2.5f, Color.blue);
            separateFromTarget(hit.point);
        } else {
            Debug.DrawRay(transform.position, transform.up * 2.5f, Color.green);
        }
    }
}