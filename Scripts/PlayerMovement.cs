using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public float Move_Speed = 6f;//SPEED WHEN NOT HOLDING SPRINT
    [SerializeField] public float Max_AirSpeed = 6f;//MAXIMUM SPEED WHILE AIRBORNE
    [SerializeField] public float Air_Accel = 0.5f;
    [SerializeField] public float Sprint_Speed = 10f; //SPEED WHILE SPRINTING

    private float Player_Speed = 10f;

    [SerializeField] public float BackpedalMultiplier = 0.6f; //SPEED REDUCTION FOR BACKPEDALLING

    [SerializeField] public float Player_JumpHeight = 6f;
    [SerializeField] public float Player_Gravity = -13.0f; //GRAVITY KEEP NEGATIVE 
    public bool Player_GravityEnabled = true;
    public Vector3 Gravity_Direction;

    public float LandingAngle = 10f; // Angle that the camera dips down to when the player lands.
    public float LandingDuration = 0.4f;

    [SerializeField] bool ToggleSprint = true; // Whether the sprint button should be tapped or held.
    [SerializeField] public bool IsSprinting = false; //Useful for debugging, if not used later can be removed

    [SerializeField] [Range(0.0f, 0.5f)] float moveSmoothTime = 0.1f;

    Rigidbody rb = null; // Player Rigidbody
    private float FoV;
    [SerializeField] float SprintFoV = 1.1f;

    [SerializeField] public float PushPower = 2.0f; 
    private Vector3 pushDir;
    private Vector3 force;

    public Camera playerCam = null;
    public Camera altCam = null;
    public CapsuleCollider Player_Collider;
    public Animator PlayerAnimator;

    private float PlayerYBound; // used in IsGrounded()

    public bool AbleToWalk = true; //whether player can walk or not (used in GravityFieldClass for NoWalk)
    public bool AbleToJump = true;

    public bool Grounded;
    Wallrun wallrun;

    Vector2 currentDir = Vector2.zero;
    Vector2 currentDirVelocity = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        Gravity_Direction = transform.up;
        wallrun = GetComponent<Wallrun>();

        //playerCam = GetComponent<CameraController>().playerCam;
        FoV = playerCam.fieldOfView;
        SprintFoV = FoV * SprintFoV;

        rb = GetComponent<Rigidbody>();

        Player_Speed = Move_Speed;
        PlayerYBound = Player_Collider.bounds.extents.y;
    }

    void Update()
    {
        if (IsSprinting) { PlayerAnimator.SetBool("IsSprinting", true); } else { PlayerAnimator.SetBool("IsSprinting", false); }
        if (wallrun.isWallrunning) { PlayerAnimator.SetBool("IsWallrunning", true); if (wallrun.leftWall) { PlayerAnimator.SetFloat("Speed", 0.375f, 0.1f, Time.deltaTime); } else if (wallrun.rightWall) { PlayerAnimator.SetFloat("Speed", 0.5f, 0.1f, Time.deltaTime); } }
        else if (IsSprinting) { PlayerAnimator.SetFloat("Speed", 0.25f, 0.1f, Time.deltaTime); }
        else if (PlayerAnimator.GetBool("IsRunning")) { PlayerAnimator.SetFloat("Speed", 0.125f, 0.1f, Time.deltaTime); }
        else { PlayerAnimator.SetFloat("Speed", 0f, 0.1f, Time.deltaTime); }
        if (!wallrun.isWallrunning) { PlayerAnimator.SetBool("IsWallrunning", false); }
        altCam.fieldOfView = playerCam.fieldOfView;
        //if (Input.GetButtonDown("Jump")) { Debug.Log("Jump Button Pressed"); }

        if (IsGrounded()) { if (!Grounded) { Land(); Grounded = true; } } else { Grounded = false; }
        //Debug.Log(IsGrounded());

        //Debug.Log(IsGrounded()); //Check if player is grounded
        UpdateRotation();

        if (AbleToWalk) { Sprint(); }
        Jump();
    }
    private void FixedUpdate()
    {
        if (AbleToWalk) { UpdateMovement(); }
    }


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        if (body == null || body.isKinematic) { return; }
        if (hit.moveDirection.y < -0.3) { /*force = new Vector3(0, -0.5f, 0) * Player_Gravity * 3;*/ return; }
        else
        {
            Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
            force = pushDir * PushPower;
        }


        body.AddForceAtPosition(force, hit.point);
    }


    void UpdateMovement()
    {
        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (targetDir.magnitude > 0) { PlayerAnimator.SetBool("IsRunning",true); } else { PlayerAnimator.SetBool("IsRunning", false); }
        
        targetDir.Normalize();

        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);


        if (Player_GravityEnabled) { rb.AddForce(Player_Gravity * rb.mass * Gravity_Direction, ForceMode.Acceleration); }

        if (IsGrounded() && !(Input.GetAxisRaw("Jump") > 0))
        {
            //velocityY = 0;
        }
        var tempvel = (transform.forward * currentDir.y + transform.right * currentDir.x) * Player_Speed;
        //rb.velocity = new Vector3 (tempvel.x,rb.velocity.y,tempvel.z);
         // IF YOU WANT THE PLAYER TO USE FORCE WHILE IN THE AIR
        if (IsGrounded() && AbleToWalk)
        {
            //rb.velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * Player_Speed;
            rb.velocity = new Vector3(tempvel.x, rb.velocity.y, tempvel.z);
        }
        else
        {
            //var airforce = ((transform.forward * currentDir.y + transform.right * currentDir.x) * Player_Speed * 1.5f);
            //Debug.Log((transform.forward * currentDir.y + transform.right * currentDir.x) * Player_Speed * 1.5f);
            rb.velocity += new Vector3(tempvel.x * Air_Accel, 0, tempvel.z * Air_Accel);
            var lerpvel = Vector3.Lerp(rb.velocity, Vector3.zero, Time.deltaTime * 3);
            rb.velocity = new Vector3(lerpvel.x, rb.velocity.y, lerpvel.z);
            /*
            if (rb.velocity.x < Max_AirSpeed && tempvel.x>0)
            {
                //rb.AddForce(new Vector3(airforce.x, 0, 0));
                rb.velocity += new Vector3(tempvel.x *Air_Accel, 0, 0);
            }
            else if (rb.velocity.x > -Max_AirSpeed && tempvel.x < 0)
            {
                rb.velocity += new Vector3(tempvel.x * Air_Accel, 0, 0);
            }
            if (rb.velocity.z < Max_AirSpeed && tempvel.z > 0)
            {
                //rb.AddForce(new Vector3(0, 0, airforce.z));
                rb.velocity += new Vector3(0, 0, tempvel.z * Air_Accel);
            }
            else if (rb.velocity.z > -Max_AirSpeed && tempvel.z < 0)
            {
                rb.velocity += new Vector3(0, 0, tempvel.z * Air_Accel);
            }
            */
        }
        
        if (Player_Gravity == 0 && (Input.GetButtonDown("Jump")))
        {
            rb.AddForce((transform.up) * Player_Speed * 1.5f);
        }
        if (Player_Gravity == 0 && (Input.GetAxisRaw("Crouch") > 0))
        {
            rb.AddForce((-transform.up) * Player_Speed * 1.5f);
        }

         /* if (Input.GetAxisRaw("Vertical") < 0)
         {
             velocity = velocity * BackpedalMultiplier; //REDUCES SPEED WHILE BACKPEDALLING
         } */

         /* (Vector3.Dot(rb.velocity, Gravity_Direction))*/ /* + Gravity_Direction*velocityY*/;




        //controller.Move(velocity * Time.deltaTime);
    }
    void UpdateRotation()
    {
        if (transform.up != Gravity_Direction && Player_Gravity != 0)
        {
            //transform.up = Vector3.MoveTowards(transform.up, Gravity_Direction, 3*Time.deltaTime);

            //Quaternion lookrot = Quaternion.LookRotation(transform.up, Gravity_Direction);
            Quaternion rot = Quaternion.LookRotation(Gravity_Direction, -transform.forward);
            rot *= Quaternion.Euler(Vector3.right * 90f);
            //transform.rotation = Quaternion.LookRotation(Gravity_Direction, -transform.forward);
            //transform.Rotate(Vector3.right, 90f);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime);

            //transform.up = Gravity_Direction.normalized;


            //transform.up = Gravity_Direction;
            //transform.rotation = Quaternion.LookRotation(transform.up, Gravity_Direction);

            //transform.rotation = Quaternion.Euler(Gravity_Direction);

            //direction.y = 0;
            //transform.Rotate(new Vector3(Quaternion.LookRotation(transform.up, -Gravity_Direction).eulerAngles.x, 0, Quaternion.LookRotation(transform.up, -Gravity_Direction).eulerAngles.z),Space.Self);

            //transform.rotation = Quaternion.LookRotation(transform.up,Gravity_Direction);

            // Quaternion lookrot = Quaternion.Slerp(transform.rotation, Quaternion.FromToRotation(transform.up, Gravity_Direction),0.1f);
            //transform.rotation = lookrot;
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(Gravity_Direction),1);
        }

        if (Player_Gravity == 0 && (playerCam.transform.rotation != transform.rotation))
        {
            playerCam.transform.rotation = Quaternion.Lerp(playerCam.transform.rotation, transform.rotation, Time.deltaTime);
            //Debug.Log("P: "+ transform.localRotation +" C: "+ playerCam.transform.localRotation);
        }
    }

    void Sprint()
    {
        //Checks for backpedalling as player can't sprint while backwards is held
        if ((((!ToggleSprint && (Input.GetAxisRaw("Sprint") > 0)) || (ToggleSprint && Input.GetButtonDown("Sprint"))) && Player_Speed != Sprint_Speed && ((Input.GetAxisRaw("Vertical") >= 0))))
        {
            Player_Speed = Sprint_Speed;
            IsSprinting = true;
        }
        else if ((!ToggleSprint && Input.GetButtonUp("Sprint")) && IsSprinting)
        {
            Player_Speed = Move_Speed;
            IsSprinting = false;
        }
        else if ((((ToggleSprint && Input.GetButtonDown("Sprint")) || ((Input.GetAxisRaw("Horizontal") == 0) && (Input.GetAxisRaw("Vertical") == 0)) || ((Input.GetAxisRaw("Vertical") < 0)))) && IsSprinting)
        {
            Player_Speed = Move_Speed;
            IsSprinting = false;
        }

        if (IsSprinting && (playerCam.fieldOfView != SprintFoV))
        {
            playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, SprintFoV, 25.3f * Time.deltaTime);
        }
        else if (!IsSprinting && (playerCam.fieldOfView != FoV))
        {
            playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, FoV, 25.3f * Time.deltaTime);
        }

        //Debug.Log("H - " + Input.GetAxisRaw("Horizontal") + "  V - " + Input.GetAxisRaw("Vertical"));  //Debug for movement dir and sprinting
        //Debug.Log(IsSprinting);

    }
    void Jump()
    {
        if ((Input.GetButtonDown("Jump")) && (IsGrounded()/*||wallrun.isWallrunning*/) && AbleToJump)
        {
            PlayerAnimator.SetBool("IsJumping", true);
            //velocityY = Player_JumpHeight;
            Debug.Log("Jumping");
            rb.velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * Player_Speed + Gravity_Direction * transform.InverseTransformDirection(rb.velocity).y + (Gravity_Direction * Player_JumpHeight);
        }
    }

    void Land()
    {
        PlayerAnimator.SetBool("IsJumping", false); PlayerAnimator.SetTrigger("IsLanding");
        //Debug.Log("Player Landed");
        //playerCam.transform.localRotation *= Quaternion.Euler(new Vector3(LandingAngle, 0, 0));
        if (GetComponent<HeadBob>() != null) { GetComponent<HeadBob>().LandBob(); }
        if (GetComponent<Wallrun>() != null) { GetComponent<Wallrun>().RefreshWallrun(); }
    }

    public float isGroundedRayLength = 0.1f;
    public LayerMask isGroundedLayerMask;

    public bool IsGrounded()
    {
        Debug.DrawRay(transform.position - (transform.up * (PlayerYBound + isGroundedRayLength)), -transform.up * isGroundedRayLength);
        //return Physics.Raycast(transform.position, -transform.up, Player_Collider.bounds.extents.y + isGroundedRayLength, isGroundedLayerMask);
        return Physics.CheckSphere(transform.position - (transform.up * (PlayerYBound + isGroundedRayLength)), isGroundedRayLength, isGroundedLayerMask);
    }
}
