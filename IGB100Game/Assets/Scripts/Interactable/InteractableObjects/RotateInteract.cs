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

    public int locks;

    private void Awake()
    {
        baseRotation = transform.parent.localRotation.eulerAngles;
    }

    public IEnumerator Interact()
    {
        if (locks > 0)
            yield break;

        float t = 0;
        hasRotated = !hasRotated;

        while (t < rotationTime)
        {
            var newRot = (!hasRotated) ? baseRotation : newRotation; 

            t += Time.deltaTime;

            transform.parent.localRotation = Quaternion.Euler(newRot);//RotateTowards(transform.parent.localRotation, Quaternion.Euler(newRot), t/rotationTime);

            if (transform.parent.localRotation == Quaternion.Euler(newRot))
                yield break;

            yield return null;
        }
    }

    public void Unlock()
    {
        locks -= 1;

        if (locks == 0)
            AudioManager.i.PlaySFX(unlockSound);
    }
}
