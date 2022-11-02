using System;
using System.Collections;
using System.Collections.Generic;
using Player.Driver;
using UnityEngine;

public class ChargePointUi : MonoBehaviour
{
    [SerializeField] RectTransform _rectTransform;
    [SerializeField] float _delvierySpeed = 1.0f;




    public IEnumerator DeliverPoint(Vector3 startPos, RectTransform endTransform)
    {
        Vector3 from = startPos;
        Vector3 to = endTransform.position;
        float percent = 0;
        while (percent <1)
        {
            percent += Time.deltaTime * _delvierySpeed;
            _rectTransform.position = Vector3.Lerp(from, to, percent);
            yield return null;
        }
        
        Destroy(this.gameObject);
    }


}
