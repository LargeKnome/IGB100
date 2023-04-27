using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SlideInteract : MonoBehaviour, Interactable
{
    [SerializeField] Vector3 newPosition;
    [SerializeField] float slideTime;

    Vector3 basePosition;

    bool hasSlid = false;

    public int locks;

    private void Awake()
    {
        basePosition = transform.parent.localPosition;
        newPosition += transform.parent.localPosition;
    }

    public IEnumerator Interact()
    {

        if (locks > 0)
            yield break;

        float t = 0;
        hasSlid = !hasSlid;
        

        while (t < slideTime)
        {
            var newPos = (!hasSlid) ? basePosition : newPosition;
            t += Time.deltaTime;
            transform.parent.localPosition = Vector3.Lerp(transform.parent.localPosition, newPos, t/slideTime);

            if (transform.parent.localPosition == newPos)
                yield break;

            yield return null;
        }
    }

    public void unlock()
    {
        locks -= 1;
    }
}