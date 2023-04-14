using TMPro;
using UnityEngine;
using static UnityEditor.PlayerSettings;

namespace StarterAssets
{
	[RequireComponent(typeof(CharacterController))]
	public class FirstPersonController : MonoBehaviour
	{
		[Header("Player")]
		[Tooltip("Move speed of the character in m/s")]
		public float MoveSpeed = 4.0f;
		[Tooltip("Rotation speed of the character")]
		public float RotationSpeed = 1.0f;

		[Space(10)]
		[Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
		public float GravityValue = -15.0f;

		[Space(10)]
		[Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
		public float FallTimeout = 0.15f;

		[Header("Player Grounded")]
		[Tooltip("If the character is grounded or not.")]
		public bool Grounded = true;
		[Tooltip("Useful for rough ground")]
		public float GroundedOffset = -0.14f;
		[Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
		public float GroundedRadius = 0.5f;

		[Header("Layers")]
		[SerializeField] LayerMask groundLayer;
		[SerializeField] LayerMask interactLayer;

        [Header("Camera")]
		[Tooltip("How far in degrees can you move the camera up")]
		public float TopClamp = 90.0f;
		[Tooltip("How far in degrees can you move the camera down")]
		public float BottomClamp = -90.0f;

		// camera
		private float _targetPitch;

		// player
		private float _speed;
		private float _rotationVelocity;
		private float _verticalVelocity;

		private CharacterController _controller;
		private GameObject _mainCamera;

		private const float _threshold = 0.01f;

		private void Awake()
		{
			// get a reference to our main camera
			if (_mainCamera == null)
				_mainCamera = GameObject.FindGameObjectWithTag("MainCamera");

			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}

		private void Start()
		{
			_controller = GetComponent<CharacterController>();
		}

		public void HandleUpdate()
		{
			Gravity();
			GroundedCheck();
			Move();

			if(Input.GetButtonDown("Interact"))
				HandleInteractInput();

            CameraRotation();
        }

		void HandleInteractInput()
		{
			//Shoot ray out from camera, if hits interactable object then interacts
            Ray ray = new(_mainCamera.transform.position, _mainCamera.transform.forward);

            if (Physics.Raycast(ray, out RaycastHit hitInfo, 3f, interactLayer))
                StartCoroutine(hitInfo.collider.GetComponent<Interactable>().Interact());
        }

		private void GroundedCheck()
		{
			// set sphere position, with offset
			Vector3 spherePosition = new(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
			Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, groundLayer, QueryTriggerInteraction.Ignore);
		}

		private void CameraRotation()
		{
            Vector2 _mouseInput = new(Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"));
            
			// if there is an input
            if (_mouseInput.sqrMagnitude >= _threshold)
			{
				_targetPitch += _mouseInput.y * RotationSpeed;
				_rotationVelocity = _mouseInput.x * RotationSpeed;

				// clamp pitch rotation
                if (_targetPitch < -360f) _targetPitch += 360f;
                if (_targetPitch > 360f) _targetPitch -= 360f;
                _targetPitch = Mathf.Clamp(_targetPitch, BottomClamp, TopClamp);

                // Update camera target pitch
                _mainCamera.transform.localRotation = Quaternion.Euler(_targetPitch, 0.0f, 0.0f);

				// rotate the player left and right
				transform.Rotate(Vector3.up * _rotationVelocity);
			}
		}

		private void Move()
		{
			Vector2 _input = new(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            // if there is no input, set the target speed to 0
            _speed = (_input != Vector2.zero) ? MoveSpeed : 0.0f;

            // normalise input direction
            Vector3 inputDirection = new Vector3(_input.x, 0.0f, _input.y).normalized;

			// if there is a move input rotate player when the player is moving
			if (_input != Vector2.zero)
			{
				// move
				inputDirection = transform.right * _input.x + transform.forward * _input.y;
			}

			// move the player
			_controller.Move(inputDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
		}

		private void Gravity()
		{
			if (Grounded)
			{
				// stop our velocity dropping infinitely when grounded
				if (_verticalVelocity < 0.0f)
					_verticalVelocity = -2f;
			}
			else
                // apply gravity over time (multiply by delta time twice to linearly speed up over time)
                _verticalVelocity += GravityValue * Time.deltaTime;
		}

		private void OnDrawGizmosSelected()
		{
			Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
			Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

			if (Grounded) Gizmos.color = transparentGreen;
			else Gizmos.color = transparentRed;

			// when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
			Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z), GroundedRadius);
		}
	}
}