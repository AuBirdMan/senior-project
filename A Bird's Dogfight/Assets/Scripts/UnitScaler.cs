using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitScaler : MonoBehaviour
{
    public Vector3 scaleUpAmount;
    public Vector3 scaleDownAmount;
    public float scaleSpeed;

    private Vector3 originalScale;
    private bool isScaling = false;

    private void Start()
    {
        originalScale = transform.localScale;
    }

    public void ScaleUp()
    {
        if (!isScaling)
        {
            isScaling = true;
            StartCoroutine(ScaleCoroutine(scaleUpAmount));
        }
    }

    public void ScaleDown()
    {
        if (!isScaling)
        {
            isScaling = true;
            StartCoroutine(ScaleCoroutine(scaleDownAmount));
        }
    }

    IEnumerator ScaleCoroutine(Vector3 targetScale)
    {
        while (transform.localScale != targetScale)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, targetScale, scaleSpeed * Time.deltaTime);
            yield return null;
        }
        isScaling = false;
    }

    public void ResetScale()
    {
        transform.localScale = originalScale;
    }
}
