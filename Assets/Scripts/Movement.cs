using UnityEngine;

public class Movement : MonoBehaviour
{
    private const string Speed = "Speed";
    private const string Jump = "Jump";

    [SerializeField] private float _speed = 4;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask _ground;
    [SerializeField] private float _jumpForce = 400;

    private Rigidbody2D _playerRigidbody;
    private bool _isGrounded;
    private float _groundRadius = 0.1f;
    private Animator _animator;

    private void Start()
    {
        _playerRigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        _isGrounded = Physics2D.OverlapCircle(groundCheck.position, _groundRadius, _ground);

        if (!_isGrounded)
        {
            return;
        }
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        float move = Input.GetAxis("Horizontal");
        _animator.SetFloat(Speed, Mathf.Abs(move));
        _playerRigidbody.velocity = new Vector2(move * _speed, _playerRigidbody.velocity.y);

        if (_isGrounded && Input.GetKeyDown(KeyCode.W))
        {
            _animator.SetTrigger(Jump);
            _playerRigidbody.AddForce(new Vector2(0, _jumpForce));
        }
    }
}