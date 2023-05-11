using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Rigidbody Oiram;
    public GameObject OiramBody;
    public int speed = 1500;
    public bool grounded = false;
    public GameObject cam;
    public GameObject hat;
    public Rigidbody hatRigid;
    public bool hatOnHead = true;
    public GameObject head;
    public Vector3 hatStart;
    public int maxSpeed = 10;
    public Animations animationScript;
    public bool canMove = true;
    // Start is called before the first frame update
    void Start()
    {
      speed = 1500;
     canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (hatOnHead){
            hatRigid.transform.position = new Vector3 (head.transform.position.x, head.transform.position.y+.15f, head.transform.position.z);
            hatRigid.velocity = new Vector3(0,0,0);
            hatRigid.constraints = RigidbodyConstraints.None;
        }
               hat.transform.localRotation = Quaternion.Euler(0,0,0);
        if (canMove){
            if (Input.GetKey("w")){
            Oiram.AddForce(cam.transform.forward*speed*Time.deltaTime);
             }
            if (Input.GetKey("s")){
            Oiram.AddForce(-cam.transform.forward*speed*Time.deltaTime);
             }
            if (Input.GetKey("a")){
            Oiram.AddForce(-cam.transform.right*speed*Time.deltaTime);
             }
            if (Input.GetKey("d")){
            Oiram.AddForce(cam.transform.right*speed*Time.deltaTime);
              }
               }
       
        if (Input.GetKeyDown("space") && grounded){
            if (Input.GetKey(KeyCode.LeftShift)){
                if (Oiram.velocity.magnitude<.1){
                Oiram.AddForce(transform.up*500);
                canMove = false;
                StartCoroutine("CrouchJumpWait");
                } else{
                    grounded=false;
                    Oiram.AddForce(transform.up*200);
                    Oiram.AddForce(transform.forward*900);
                }
            }
            else{
                Oiram.AddForce(transform.up*300);
            }
        }
        if (grounded){
            if (Oiram.velocity.x > maxSpeed){
            Oiram.velocity = new Vector3(maxSpeed,Oiram.velocity.y,Oiram.velocity.z);
        }
         if (Oiram.velocity.x < -maxSpeed){
            Oiram.velocity = new Vector3(-maxSpeed,Oiram.velocity.y,Oiram.velocity.z);
        } 
         if (Oiram.velocity.z < -maxSpeed){
            Oiram.velocity = new Vector3(Oiram.velocity.x,Oiram.velocity.y,-maxSpeed);
        } 
        if (Oiram.velocity.z > maxSpeed){
            Oiram.velocity = new Vector3(Oiram.velocity.x,Oiram.velocity.y,maxSpeed);
        } 
        } else{
              if (Oiram.velocity.x > maxSpeed+5){
            Oiram.velocity = new Vector3(maxSpeed+5,Oiram.velocity.y,Oiram.velocity.z);
        }
         if (Oiram.velocity.x < -maxSpeed-5){
            Oiram.velocity = new Vector3(-maxSpeed-5,Oiram.velocity.y,Oiram.velocity.z);
        } 
         if (Oiram.velocity.z < -maxSpeed-5){
            Oiram.velocity = new Vector3(Oiram.velocity.x,Oiram.velocity.y,-maxSpeed-5);
        } 
        if (Oiram.velocity.z > maxSpeed+5){
            Oiram.velocity = new Vector3(Oiram.velocity.x,Oiram.velocity.y,maxSpeed+5);
        } 
        }
       



        if (Input.GetKeyDown("r")){
            Oiram.transform.position = new Vector3(0,0,0);
        }

        cam.transform.position = new Vector3(OiramBody.transform.position.x, OiramBody.transform.position.y+1.5f, OiramBody.transform.position.z);
        
           if (Oiram.velocity.magnitude > .5f && new Vector3 (Oiram.velocity.x,0,Oiram.velocity.z) != Vector3.zero){
            OiramBody.transform.rotation = Quaternion.Slerp (OiramBody.transform.rotation, Quaternion.LookRotation (new Vector3 (Oiram.velocity.x,0,Oiram.velocity.z)), Time.deltaTime * 40f);
           } 
        //
        if (Input.GetKeyDown(KeyCode.Mouse0) && hatOnHead){
            StartCoroutine("hatThrow");

        }
        if (!hatOnHead){
            hatRigid.AddForce(-transform.up*4000*Time.deltaTime);
           // Debug.Log(hat.transform.position);
           // Debug.Log(hatStart);
             if (hat.transform.position.y < hatStart.y-1.2f)
                hat.transform.position = new Vector3(hat.transform.position.x, hatStart.y-1.2f, hat.transform.position.z);
        if ( ( Vector3.Distance(hat.transform.position,hatStart))>8){
            hatRigid.constraints = RigidbodyConstraints.FreezeAll;
        }
        }
       
              if (Input.GetKey(KeyCode.LeftShift) && grounded){
                    speed = 700;
              } else{
                speed = 1500;
              }

        
    }

     private void OnTriggerEnter(Collider other) {
        if (other.gameObject.name == "Hat" && !hatOnHead){
            animationScript.StopCoroutine("hover");
            animationScript.StopCoroutine("airJump");
            Oiram.velocity = new Vector3(0, 0, 0);
            Oiram.AddForce(transform.up*500);
            Debug.Log("Bounce");
            hatOnHead=true;
            StopCoroutine("hatThrow");
        }
    }
    private void FixedUpdate() {
         Oiram.velocity = new Vector3(Oiram.velocity.x*.95f,Oiram.velocity.y,Oiram.velocity.z*.95f);
    }
    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag== "ground"){
                grounded = true;
        }
    }
    private void OnCollisionExit(Collision other) {
        if (other.gameObject.tag == "ground"){
            StartCoroutine("groundExit");
            
        }
    }
    public IEnumerator CrouchJumpWait(){
            yield return new WaitForSeconds(.6f); 
            canMove=true;
    }
    public IEnumerator groundExit(){
        yield return new WaitForSeconds(.001f);
        grounded = false;
    }
    public IEnumerator hatThrow(){
        if (!grounded){
            Oiram.velocity = new Vector3(0, 2, 0);
        }
        OiramBody.transform.rotation = cam.transform.rotation;
        yield return new WaitForSeconds(.2f);
        hatStart =( hatRigid.transform.position);
        hatOnHead=false;
        hatRigid.AddForce(cam.transform.forward*900);
       
        yield return new WaitForSeconds(2f);
        hatOnHead=true;
       
    }
}
