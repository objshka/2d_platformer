using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _speed;
    
    private float _minGroundNormalY = .65f; 
    private float _gravityModifier = 1f;
    private bool _grounded;
    private Vector2 _velocity;
    private Vector2 _targetVelocity;
    private Vector2 _groundNormal;
    private Rigidbody2D _playerRigidbody;
    private ContactFilter2D _contactFilter;
    private readonly RaycastHit2D[] _hitBuffer = new RaycastHit2D[16];
    private readonly List<RaycastHit2D> _hitBufferList = new List<RaycastHit2D>(16);
    private Animator _animator;

    private const float MinMoveDistance = 0.001f;
    private const float ShellRadius = 0.01f;

    void OnEnable()
    {
        _playerRigidbody = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        _contactFilter.useTriggers = false;
        _contactFilter.SetLayerMask(_layerMask);
        _contactFilter.useLayerMask = true;
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        _targetVelocity = new Vector2(Input.GetAxis("Horizontal") * _speed, 0);
        
        if (Input.GetKey(KeyCode.Space) && _grounded)
        {
            _velocity.y = 6;
        }
        
        bool isRunning = Mathf.Abs(Input.GetAxis("Horizontal")) > 0f;
        _animator.SetBool("isRunning", isRunning);
    }

    void FixedUpdate()
    {
        _velocity += _gravityModifier * Physics2D.gravity * Time.deltaTime;
        _velocity.x = _targetVelocity.x;
        
        _grounded = false;
        
        Vector2 deltaPosition = _velocity * Time.deltaTime;
        Vector2 moveAlongGround = new Vector2(_groundNormal.y, -_groundNormal.x);
        Vector2 move = moveAlongGround * deltaPosition.x;

        Move(move, false);

        move = Vector2.up * deltaPosition.y;

        Move(move, true);
    }

    void Move(Vector2 move, bool yMovement)
    {
        float distance = move.magnitude;

        if (distance > MinMoveDistance)
        {
            int count = _playerRigidbody.Cast(move, _contactFilter, _hitBuffer, distance + ShellRadius);

            _hitBufferList.Clear();

            for (int i = 0; i < count; i++)
            {
                _hitBufferList.Add(_hitBuffer[i]);
            }
            
            for (int i = 0; i < _hitBufferList.Count; i++)
            {
                Vector2 currentNormal = _hitBufferList[i].normal;
                if (currentNormal.y > _minGroundNormalY)
                {
                    _grounded = true;
                    if (yMovement)
                    {
                        _groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }
                }

                float projection = Vector2.Dot(_velocity, currentNormal);
                if (projection < 0)
                {
                    _velocity = _velocity - projection * currentNormal;
                }

                float modifiedDistance = _hitBufferList[i].distance - ShellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }
        }

        _playerRigidbody.position = _playerRigidbody.position + move.normalized * distance;
    }
}