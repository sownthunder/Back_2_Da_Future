using UnityEngine;
using SurfaceDetection;


public class Spectator : MonoBehaviour
{
    [SerializeField]
    private KeyCode switchCursor = KeyCode.Tab;

    [SerializeField]
    private float mouceSpeed = 15f;

    [SerializeField]
    private float moveSpeed = 10f;

    [SerializeField]
    private float lookSmooth = .721f;
    [SerializeField]
    private float maxLookAngleY = 65f;


    Transform m_Transform;

    float rotationX, rotationY;
    Quaternion nativeRotation;

    bool cursorLocked = true;


    string m_SurfaceName;
    Material m_MeshMaterial;
    Texture m_TerrainTexture;

    Collider m_ObjCollider;



    // Use this for initialization
    void Start()
    {
        m_Transform = transform;
        m_Transform.position += m_Transform.up;

        nativeRotation = m_Transform.rotation;
        nativeRotation.eulerAngles = Vector3.up * m_Transform.localEulerAngles.y;
    }


    // Update
    void Update()
    {
        LockCursor();

        Movement();
        CameraLook();
    }


    // Fixed Update
    void FixedUpdate()
    {
        RefreshSurfaceInfo();
    }
    

    // LockCursor
    private void LockCursor()
    {
        if( Input.GetKeyDown( switchCursor ) || Input.GetKeyDown( KeyCode.Escape ) )
        {
            cursorLocked = !cursorLocked;
        }

        Cursor.visible = !cursorLocked;
        Cursor.lockState = cursorLocked ? CursorLockMode.Locked : CursorLockMode.None;
    }



    // RefreshSurfaceInfo
    private void RefreshSurfaceInfo()
    {
        RaycastHit hit;
        Physics.Raycast( m_Transform.position, m_Transform.forward, out hit );

        m_ObjCollider = hit.collider;

        m_SurfaceName = SurfaceDetector.UNKNOWN;
        m_TerrainTexture = null;

        if( hit.TryGetMaterial( out m_MeshMaterial ) )
        {
            m_SurfaceName = m_MeshMaterial.GetSurface();
        }
        else
        {
            if( hit.TryGetTerrainTexture( out m_TerrainTexture ) )
            {
                m_SurfaceName = m_TerrainTexture.GetSurface();
            }
        }

        //m_SurfaceName = hit.GetSurface();
    }


    // Movement
    private void Movement()
    {
        float horizontal = Input.GetAxis( "Horizontal" );
        float vertical = Input.GetAxis( "Vertical" );

        float speed = Input.GetKey( KeyCode.LeftShift ) ? moveSpeed * 2f : moveSpeed;

        Quaternion screenMovementSpace = Quaternion.Euler( m_Transform.eulerAngles );
        Vector3 forwardVector = screenMovementSpace * Vector3.forward * vertical;
        Vector3 rightVector = screenMovementSpace * Vector3.right * horizontal;
        Vector3 moveVector = forwardVector + rightVector;
        Vector3 moveDirection = m_Transform.position + moveVector * speed;

        if( Input.GetKey( KeyCode.E ) ) moveDirection.y += speed / 2;
        else if( Input.GetKey( KeyCode.Q ) ) moveDirection.y -= speed / 2;

        m_Transform.position = Vector3.Lerp( m_Transform.position, moveDirection, Time.smoothDeltaTime * 1.45f );
    }


    // Camera Look
    private void CameraLook()
    {
        rotationX += Input.GetAxis( "Mouse X" ) * mouceSpeed * Time.deltaTime * 10f;
        rotationY += Input.GetAxis( "Mouse Y" ) * mouceSpeed * Time.deltaTime * 10f;

        rotationY = Mathf.Clamp( rotationY, -maxLookAngleY, maxLookAngleY );

        Quaternion targetRotation = nativeRotation * Quaternion.Euler( -rotationY, rotationX, 0f );
        m_Transform.rotation = Quaternion.Slerp( m_Transform.rotation, targetRotation, lookSmooth * Time.deltaTime * 50f );
    }



    // OnGUI
    void OnGUI()
    {
        GUI.Box( new Rect( 5f, 5f, 250f, 110f ), string.Format( "<color=#ffa500ff>Info: (Obj) \"{0}\"</color>", GetName( m_ObjCollider ) ) );


        Rect rect = new Rect( 10f, 35f, 250f, 22f );
        GUI.Label( rect, string.Format( "<b>Surface (Name): \"{0}\"</b>", m_SurfaceName ) );

        rect.y += 25f;        
        GUI.Label( rect, string.Format( "Material (Mesh): \"{0}\"", GetName( m_MeshMaterial ) ) );

        rect.y += 25f;        
        GUI.Label( rect, string.Format( "Texture (Terrain): \"{0}\"", GetName( m_TerrainTexture ) ) );
    }

    
    static string GetName( Object obj )
    {
        return ( obj != null ) ? obj.name : "NULL";
    }
};
