using UnityEngine;

namespace BlueGravity.Core
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float m_moveSpeed = 3f;
        [SerializeField] private Animator m_animator = null;

        private Rigidbody2D m_rigidBody2D;

        private PlayerInput m_playerInput;

        private int m_animatorSpeedHash = 0;

        bool m_canMove = true;
        public void SetCanMove(bool value)
        {
            if (!value) Move(Vector2.zero);
            m_canMove = value;
        }

        void Start()
        {
            m_animatorSpeedHash = Animator.StringToHash("Speed");
            m_rigidBody2D = GetComponent<Rigidbody2D>();
            m_playerInput = GetComponent<PlayerInput>();
            m_playerInput.inputControl.Ground.Move.performed += move => Move(move.ReadValue<Vector2>());
            m_playerInput.inputControl.Ground.Move.canceled += move => Move(Vector2.zero);
        }

        private void Move(Vector2 _move)
        {
            if (!m_canMove) return;
            if (_move != Vector2.zero) Flip(_move.x);
            m_rigidBody2D.velocity = new Vector2(_move.x * m_moveSpeed, _move.y * m_moveSpeed);
            m_animator.SetFloat(m_animatorSpeedHash, _move.magnitude);
        }

        private void Flip(float _xValue)
        {
            var _actualScale = m_animator.transform.localScale;
            float newX = _xValue > 0 ? Mathf.Abs(_actualScale.x) : -Mathf.Abs(_actualScale.x);
            m_animator.transform.localScale = new Vector3(newX, _actualScale.y, _actualScale.z);
        }
    }
}

