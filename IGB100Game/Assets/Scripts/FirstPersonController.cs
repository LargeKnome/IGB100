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
        [Tooltip("The distance in m the character can interact from")]
        [SerializeField] float interactionDistance;

		[Space(10)]
		[Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
		public float GravityValue = -15.0f;

		[Header("Player Grounded")]
		[Tooltip("If the character is grounded or not.")]
		public bool Grounded = true;
		[Tooltip("Useful for rough ground")]
		public float GroundedOffset = -0.14f;
		[Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
		public float GroundedRadius = 0.5f;

        [Header("Camera")]
		[Tooltip("How far in degrees can you move the camera up")]
		public float TopClamp = 90.0f;
		[Tooltip("How far in degrees can you move the camera down")]
		public float BottomClamp = -90.0f;

		// camera
		private float targetPitch;

		// player
		private float speed;
		private float rotationVelocity;
		private float verticalVelocity;

		private CharacterController controller;
		private GameObject mainCamera;

		private void Awake()
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}

		private void Start()
		{
			mainCamera = GameController.i.MainCamera.gameObject;
			controller = GetComponent<CharacterController>();
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
            Ray ray = new(mainCamera.transform.position, mainCamera.transform.forward);

            if (Physics.Raycast(ray, out RaycastHit hitInfo, interactionDistance, LayerManager.i.InteractLayer))
                StartCoroutine(hitInfo.collider.GetComponent<Interactable>().Interact());
        }

		public bool Interactable()
		{
			return Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, interactionDistance, LayerManager.i.InteractLayer);
        }

		private void GroundedCheck()
		{
			// set sphere position, with offset
			Vector3 spherePosition = new(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
			Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, LayerManager.i.GroundLayer, QueryTriggerInteraction.Ignore);
		}

		private void CameraRotation()
		{
            Vector2 _mouseInput = new(Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"));
            
			// if there is an input
            if (_mouseInput.sqrMagnitude >= Mathf.Epsilon)
			{
				targetPitch += _mouseInput.y * RotationSpeed;
				rotationVelocity = _mouseInput.x * RotationSpeed;

				// clamp pitch rotation
                if (targetPitch < -360f) targetPitch += 360f;
                if (targetPitch > 360f) targetPitch -= 360f;
                targetPitch = Mathf.Clamp(targetPitch, BottomClamp, TopClamp);

                // Update camera target pitch
                mainCamera.transform.localRotation = Quaternion.Euler(targetPitch, 0.0f, 0.0f);

				// rotate the player left and right
				transform.Rotate(Vector3.up * rotationVelocity);
			}
		}

		private void Move()
		{
			Vector2 _input = new(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            // if there is no input, set the target speed to 0
            speed = (_input != Vector2.zero) ? MoveSpeed : 0.0f;

            // normalise input direction
            Vector3 inputDirection = new Vector3(_input.x, 0.0f, _input.y).normalized;

			// if there is a move input rotate player when the player is moving
			if (_input != Vector2.zero)
			{
				// move
				inputDirection = transform.right * _input.x + transform.forward * _input.y;
			}

			// move the player
			controller.Move(inputDirection.normalized * (speed * Time.deltaTime) + new Vector3(0.0f, verticalVelocity, 0.0f) * Time.deltaTime);
		}

		private void Gravity()
		{
			if (Grounded)
			{
				// stop our velocity dropping infinitely when grounded
				if (verticalVelocity < 0.0f)
					verticalVelocity = -2f;
			}
			else
                // apply gravity over time (multiply by delta time twice to linearly speed up over time)
                verticalVelocity += GravityValue * Time.deltaTime;
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