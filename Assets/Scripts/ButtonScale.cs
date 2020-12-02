using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ButtonScale : MonoBehaviour, IPointerUpHandler, IPointerDownHandler {

	public void OnPointerDown (PointerEventData data) {
        if (transform.parent != null) {
            transform.parent.DOScale(Vector3.one * 1.03f, 0.1f);
        }
	}

	public void OnPointerUp (PointerEventData data) {
        if (transform.parent != null)
        {
            transform.parent.DOScale(Vector3.one, 0.1f);
        }
	}

}
