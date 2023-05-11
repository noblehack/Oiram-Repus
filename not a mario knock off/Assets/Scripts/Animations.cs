using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animations : MonoBehaviour
{
    public Animator animator;
    public Rigidbody Oiram;
    public BoxCollider OiramCol;
    public Movement movementScript;
    public bool canGroundPound;
    public bool inGroundPound ;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

       // Walk.speed = (Mathf.Abs(Oiram.velocity.x)+Mathf.Abs(Oiram.velocity.z))/2;
        if (Mathf.Abs(Oiram.velocity.x) >.3 || Mathf.Abs(Oiram.velocity.z) >.3 ){
            animator.SetBool("Walk",true);
        } else {
            animator.SetBool("Walk",false);
        
        }
        if (Input.GetKeyDown(KeyCode.Mouse0)){
            animator.SetTrigger("Throw");
            StopCoroutine("hover");
        }

        if (Input.GetKey(KeyCode.LeftShift)){
            //OiramCol.size = new Vector3(8,16,4);
            animator.SetBool("Crouch",true);

        } else{
            animator.SetBool("Crouch",false);
            
           // OiramCol.size = new Vector3(8,24,4);
           
        }
        if (Input.GetKey(KeyCode.Space) && movementScript.grounded){

            animator.SetBool("Jump",true);
        } else{
             animator.SetBool("Jump",false);
        }
            if (movementScript.grounded){
                    canGroundPound = true;
            }
         if (!movementScript.grounded){
            if (Input.GetKeyDown(KeyCode.LeftShift) && canGroundPound){
                animator.SetTrigger("InAir");
                canGroundPound = false;
                inGroundPound = true;
                StartCoroutine("hover");
                animator.SetBool("AirJump",false);
            } 
                animator.SetBool("AirJump",false);
        } 
    
        if (Input.GetKeyDown(KeyCode.Space) && inGroundPound){
                Oiram.AddForce(Oiram.transform.forward*900);
                Oiram.AddForce(Oiram.transform.forward*200);
                animator.SetTrigger("AirJump");
                StopCoroutine("hover");
                StartCoroutine("airJump");
                inGroundPound = false;
        } 
        
        
    }
    public IEnumerator airJump(){
             for (int i = 0; i<50; i++){
                yield return new WaitForSeconds(.01f);
            Oiram.velocity = new Vector3(Oiram.velocity.x,0,Oiram.velocity.z);
             }

    }
    public IEnumerator hover(){
        for (int i = 0; i<50; i++){
            yield return new WaitForSeconds(.01f);
            Oiram.velocity = new Vector3(0,0,0);
        }
        inGroundPound = false;
    }
       private void OnTriggerEnter(Collider other) {
        if (other.gameObject.name == "Hat"){
           StopCoroutine("hover");
        }
    }
    //
}
 