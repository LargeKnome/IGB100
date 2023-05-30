using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RotateInteract : MonoBehaviour, Interactable
{
    [SerializeField] Vector3 newRotation;
    [SerializeField] float rotationTime;

    [SerializeField] AudioClip unlockSound;

    Vector3 baseRotation;

    bool hasRotated = false;
    bool isRotating = false;

    public int locks;

    private void Awake()
    {
        baseRotation = transform.parent.localRotation.eulerAngles;
    }

    public IEnumerator Interact()
    {
        if (locks > 0 || isRotating)
            yield break;

        isRotating = true;

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
    }

    public void Unlock()
    {
        locks -= 1;

        if (locks == 0)
            AudioManager.i.PlaySFX(unlockSound);
    }
}
