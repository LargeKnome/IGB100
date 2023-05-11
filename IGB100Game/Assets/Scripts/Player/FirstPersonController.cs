using System;
using TMPro;
//using UnityEditor.PackageManager;
using UnityEngine;
//using static UnityEditor.PlayerSettings;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
	[Header("Player")]
	[SerializeField] float moveSpeed = 4.0f;
	[SerializeField] float rotationSpeed = 1.0f;
	[SerializeField] float interactionDistance;

	//Camera
	float topClamp = 90.0f;
	float bottomClamp = -90.0f;
	float targetPitch;

	// player
	float rotationVelocity;

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
			if (hitInfo.collider.gameObject.tag != "HiddenInteract")
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
		Vector2 movementVector = new(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

		if (movementVector == Vector2.zero)
			return;

		// Change input direction depending on where the player is facing
		var inputDirection = transform.right * movementVector.x + transform.forward * movementVector.y;

		// move the player
		controller.Move(moveSpeed * Time.deltaTime * inputDirection.normalized);
	}
}