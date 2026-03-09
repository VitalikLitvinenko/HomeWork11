using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Speed = 5f;
    public float JumpForce = 12f;
    public float KnockbackForce = 8f; 
    public float KnockbackDuration = 0.2f;

    private Rigidbody2D _rigidbody;
    private SpriteRenderer _playerRenderer;
    private UIManager _uiManager;
    private bool _isGrounded;
    private bool _isKnockedBack;
    private float _knockbackTimer;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _playerRenderer = GetComponent<SpriteRenderer>();
        _uiManager = FindObjectOfType<UIManager>();
        _playerRenderer.color = Color.white;
        _playerRenderer.flipX = false;
    }

    private void Update()
    {
        if (_isKnockedBack)
        {
            _knockbackTimer -= Time.deltaTime;
            if (_knockbackTimer <= 0f)
                _isKnockedBack = false;
            return;
        }

        float x = Input.GetAxisRaw("Horizontal");
        float y = _rigidbody.linearVelocity.y;

        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) && _isGrounded)
        {
            y = JumpForce;
            _isGrounded = false;
        }

        _rigidbody.linearVelocity = new Vector2(x * Speed, y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGrounded = true;
            return;
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            _playerRenderer.color = Color.red;
            _uiManager.TakeDamage();

            Vector2 knockbackDirection = (transform.position - collision.transform.position).normalized;
            _rigidbody.linearVelocity = new Vector2(knockbackDirection.x * KnockbackForce, KnockbackForce * 0.5f);

            _isKnockedBack = true;
            _knockbackTimer = KnockbackDuration;

            Debug.Log("Столкновение с врагом: " + collision.gameObject.name, collision.gameObject);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGrounded = false;
            return;
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            _playerRenderer.color = Color.white;

            Debug.Log("Разлетелись с врагом: " + collision.gameObject.name, collision.gameObject);
        }
    }
}
