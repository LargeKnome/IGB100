using System;
using TMPro;
//using UnityEditor.PackageManager;
using UnityEngine;
//using static UnityEditor.PlayerSettings;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
	[Header("Player")]
	[Tooltip("Move speed of the character in m/s")]
	[SerializeField] float moveSpeed = 4.0f;
	[Tooltip("Rotation speed of the character")]
	[SerializeField] float rotationSpeed = 1.0f;
	[Tooltip("The distance in m the character can interact from")]
	[SerializeField] float interactionDistance;

	[Space(10)]
	[Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
	public float gravityValue = -15.0f;

	bool grounded = true;

	//Camera
	float topClamp = 90.0f;
	float bottomClamp = -90.0f;
	float targetPitch;

	// player
	float speed;
	float rotationVelocity;
	float verticalVelocity;

	bool visionActivated = false;

	CharacterController controller;
	GameObject mainCamera;

	Inventory inventory;

	public Inventory Inventory => inventory;

	public event Action<bool> OnVisionActivate;

	private void Start()
	{
		mainCamera = GameController.i.MainCamera.gameObject;
		inventory = GetComponent<Inventory>();
		controller = GetComponent<CharacterController>();
	}

	public void HandleUpdate()
	{
		Gravity();
		GroundedCheck();
		Move();

		if (Input.GetButtonDown("Interact"))
			HandleInteractInput();

		if (Input.GetKeyDown(KeyCode.Tab))
			GameController.i.StateMachine.Push(InventoryState.i);

		if (Input.GetKeyDown(KeyCode.F))
		{
			visionActivated = !visionActivated;
			OnVisionActivate.Invoke(visionActivated);
		}

		CameraRotation();
	}

	void HandleInteractInput()
	{
		//Shoot ray out from camera, if hits interactable object then interacts
		Ray ray = new(mainCamera.transform.position, mainCamera.transform.forward);

		if (Physics.Raycast(ray, out RaycastHit hitInfo, interactionDistance))
			if (1 << hitInfo.collider.gameObject.layer == LayerManager.i.InteractLayer.value)
				StartCoroutine(hitInfo.collider.GetComponent<Interactable>().Interact());
	}

	public bool Interactable()
	{
		//Returns true if looking at interactable object
		if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out RaycastHit hitInfo, interactionDistance))
			return 1 << hitInfo.collider.gameObject.layer == LayerManager.i.InteractLayer.value;

		return false;

	}

	public string GetInteractableName()
	{
		//Returns true if looking at interactable object
		if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out RaycastHit hitInfo, interactionDistance))
			return hitInfo.collider.gameObject.name;

		return "";
	}

	private void GroundedCheck()
	{
		// set sphere position
		Vector3 spherePosition = transform.position;
		grounded = Physics.CheckSphere(spherePosition, controller.radius, LayerManager.i.GroundLayer, QueryTriggerInteraction.Ignore);
	}

	private void CameraRotation()
	{
		Vector2 _mouseInput = new(Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"));

		// if there is an input
		if (_mouseInput.sqrMagnitude >= Mathf.Epsilon)
		{
			targetPitch += _mouseInput.y * rotationSpeed;
			rotationVelocity = _mouseInput.x * rotationSpeed;

			// clamp pitch rotation
			if (targetPitch < -360f) targetPitch += 360f;
			if (targetPitch > 360f) targetPitch -= 360f;
			targetPitch = Mathf.Clamp(targetPitch, bottomClamp, topClamp);

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
		speed = (_input != Vector2.zero) ? moveSpeed : 0.0f;

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
		if (grounded)
		{
			// stop our velocity dropping infinitely when grounded
			if (verticalVelocity < 0.0f)
				verticalVelocity = -2f;
		}
		else
			// apply gravity over time (multiply by delta time twice to linearly speed up over time)
			verticalVelocity += gravityValue * Time.deltaTime;
	}

	private void OnDrawGizmosSelected()
	{
		Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
		Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

		if (grounded) Gizmos.color = transparentGreen;
		else Gizmos.color = transparentRed;

		// when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
		Gizmos.DrawSphere(transform.position, GetComponent<CharacterController>().radius);
	}
}