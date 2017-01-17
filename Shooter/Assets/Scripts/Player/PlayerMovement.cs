using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 6f;

    public float jumpSpeed = 1f;

    Vector3 movement;
    Animator anim;
    Rigidbody playerRigidbody;
    int floorMask;
    float camRayLength = 100f;
    bool jumping = false;

    public float gravity = 20.0F;

    private void Awake()
    {
        floorMask = LayerMask.GetMask("Floor");
        anim = GetComponent<Animator>(); //this just tells us that we're looking for an animator type of component
        playerRigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() //automatically called, mono-behaviour
    {
        float h = Input.GetAxisRaw("Horizontal"); //raw means that it only can have the values of -1,0 and 1. instead of the decimals in b/w that cause a slow build, this is a fast build
        float v = Input.GetAxisRaw("Vertical");

        if (Input.GetButtonDown("Jump"))
        {
            jumping = true;
        }
        Move(h, v);
        Turning();
        
        Animating(h, v);

    }
    

    void Move(float h, float v)
    {
        movement.Set(h, 0f, v);


        if (jumping == true)
        {
            transform.position += transform.up * jumpSpeed * Time.deltaTime;
        }


        if (playerRigidbody.position.y > 0.5)
        {
            jumping = false;
        }

        movement = movement.normalized * speed * Time.deltaTime; //this allows us to have the player move at the same speed regardless of the key-combination used 
        //delta time is the time b/w each update call

        playerRigidbody.MovePosition(transform.position + movement);
        
    }

    void Turning()
    {
        // Create a ray from the mouse cursor on screen in the direction of the camera.
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Create a RaycastHit variable to store information about what was hit by the ray.
        RaycastHit floorHit;

        // Perform the raycast and if it hits something on the floor layer...
        if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
        {
            // Create a vector from the player to the point on the floor the raycast from the mouse hit.
            Vector3 playerToMouse = floorHit.point - transform.position;

            // Ensure the vector is entirely along the floor plane.
            playerToMouse.y = 0f;

            // Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);

            // Set the player's rotation to this new rotation.
            playerRigidbody.MoveRotation(newRotation);
        }
        /*
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition); //takes a point ont the screen and casts a ray from that point towards the scene
                                                                        //by giving it the mouse position we're effecitvely telling it to create a ray from the point underneath the mouse if it's on the quad and send it back to the scene 
        RaycastHit floorHit; //this gets us the information from the raycast variable

        //now to cast the ray we've created. we have an imaginary invisible line from the mouse.pos back to the scene but now we need to actually make it able for that ray to actually hit something

        //raycast returns true if it hits something and false if it hasnt

        //if we hit something carry this out
        if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
        {
            Vector3 playerToMouse = floorHit.point - transform.position;
            playerToMouse.y = 0f;

            //Quaternion is a way of holding a rotation

            Quaternion newRotation = Quaternion.LookRotation(playerToMouse); //making this vector the forward facing one of the Quaternion
            playerRigidbody.MoveRotation(newRotation); //this is making the rotation = to the new one we've created based upon the position of the player mouse 

        }*/
    }

    void Animating(float h, float v)
    {
        bool walking = h != 0f || v != 0f; //basically this means that if we pressed either the horizontal or vertical axis. if we did we're walking and this is true. if we didn't we're not and this is false. 
        anim.SetBool("IsWalking", walking); //this sets it based upon if the axis are being used

    }
         
}
