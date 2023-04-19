using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RotateInteract : MonoBehaviour, Interactable
{
    [SerializeField] Vector3 newRotation;
    [SerializeField] float rotationTime;

    [SerializeField] Evidence requiredEvidence;

    Vector3 baseRotation;

    bool hasRotated = false;

    private void Awake()
    {
        baseRotation = transform.localRotation.eulerAngles;
    }

    public IEnumerator Interact()
    {
        if (requiredEvidence != null)
        {
            if (!GameController.i.Player.Inventory.EvidenceList.Contains(requiredEvidence))
                yield break;
        }

        float t = 0;
        hasRotated = !hasRotated;

        while (t < rotationTime)
        {
            var newRot = (!hasRotated) ? baseRotation : newRotation;

            t += Time.deltaTime;

            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(newRot), t/rotationTime);

            if (transform.rotation == Quaternion.Euler(newRot))
                yield break;

            yield return null;
        }
    }
}
