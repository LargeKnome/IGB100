using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RotateInteract : MonoBehaviour, Interactable
{
    [SerializeField] Vector3 newRotation;
    [SerializeField] float rotationTime;

    [SerializeField] AudioClip doorSound;

    Vector3 baseRotation;

    bool hasRotated = false;
    bool isRotating = false;

    public int locks;

    BoxCollider collider;

    private void Awake()
    {
        baseRotation = transform.parent.localRotation.eulerAngles;

        collider = GetComponent<BoxCollider>();
    }

    public IEnumerator Interact()
    {
        if (locks > 0 || isRotating)
            yield break;

        if(doorSound != null)
            AudioManager.i.PlaySFX(doorSound);

        isRotating = true;

        collider.enabled = false;

        float t = 0;
        hasRotated = !hasRotated;

        var prevRotation = transform.parent.localRotation;

        while (t < rotationTime)
        {
            var newRot = (!hasRotated) ? baseRotation : newRotation; 

            t += Time.deltaTime;

            transform.parent.localRotation = Quaternion.Slerp(prevRotation, Quaternion.Euler(newRot), t/rotationTime);

            yield return null;
        }

        if(t >= rotationTime)
            transform.parent.localRotation = (!hasRotated) ? Quaternion.Euler(baseRotation) : Quaternion.Euler(newRotation);

        isRotating = false;
        collider.enabled = true;
    }

    public void Unlock()
    {
        locks -= 1;
    }
}
