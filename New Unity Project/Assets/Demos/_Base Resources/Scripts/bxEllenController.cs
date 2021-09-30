using UnityEngine;

namespace Blartenix.Demos
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class bxEllenController : MonoBehaviour
    {
        [SerializeField]
        private float walkSpeed = 1;
        [SerializeField]
        private float jumpForce = 1;
        [SerializeField]
        private float extraGravity = 10;
        [SerializeField]
        private LayerMask groundLayer = 1 << 0;
        [SerializeField]
        private float groundCheckDistance = 0.1f;
        [SerializeField]
        private bool airControl = false;

        public bool IsGrounded { get; private set; }
        public Vector2 MovingDir { get; private set; }

        private bool FacingLeft { get; set; }
        private bool MakeJump { get; set; }

        private Animator animator = null;
        private Rigidbody2D rb = null;
        private CapsuleCollider2D capsuleCollider = null;
        private SpriteRenderer spriteRenderer = null;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
            capsuleCollider = GetComponentInChildren<CapsuleCollider2D>();
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        private void Update()
        {
            UpdateAnimator();
        }

        private void FixedUpdate()
        {
            CheckGrounded();
            if(MakeJump)
            {
                MakeJump = false;
                rb.AddForce(Vector2.up * jumpForce * Time.fixedDeltaTime, ForceMode2D.Impulse);
            }

            rb.velocity = (Vector2.up * (rb.velocity.y - extraGravity)) + MovingDir * walkSpeed * Time.fixedDeltaTime;
        }

        private void UpdateAnimator()
        {
            animator.SetBool("Grounded", IsGrounded);
            animator.SetFloat("vSpeed", Mathf.Abs(MovingDir.x));
            animator.SetFloat("hSpeed", rb.velocity.y);
        }

        private void CheckGrounded()
        {
            Vector2 origin = capsuleCollider.bounds.center - new Vector3(capsuleCollider.bounds.extents.x, capsuleCollider.bounds.extents.y + 0.1f);
            Vector2 size = new Vector2(capsuleCollider.bounds.extents.x * 2, groundCheckDistance);

            RaycastHit2D hit = Physics2D.BoxCast(origin, size, 0, Vector2.down, 0, groundLayer);
            IsGrounded = hit;

            Debug.DrawRay(origin, Vector2.right * size.x, Color.red);
            Debug.DrawRay(origin, Vector2.down * size.y, Color.red);
            Debug.DrawRay(origin + Vector2.right * size.x, Vector2.down * size.y, Color.red);
            Debug.DrawRay(origin - Vector2.up * size.y, Vector2.right * size.x, Color.red);
        }

        public void Move(float horizontalInput)
        {
            if (IsGrounded || airControl)
                MovingDir = Vector2.right * horizontalInput;


            FacingLeft = horizontalInput < 0 || (FacingLeft && horizontalInput == 0);

            spriteRenderer.flipX = FacingLeft;
        }

        public void Jump()
        {
            if (!IsGrounded) return;

            MakeJump = true;
        }
    }
}