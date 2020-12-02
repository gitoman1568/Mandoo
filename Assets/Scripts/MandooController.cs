using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MandooController : MonoBehaviour
{
    public Image image;
    public enum MandooType { 고기만두, 김치만두 };
    public MandooType mandooType;

    void Start()
    {
        TypeRandomSetting();
    }

    public void TypeSetting(MandooController mandoo)
    {
        image.sprite = mandoo.image.sprite;
        mandooType = mandoo.mandooType;
    }

    public void TypeRandomSetting()
    {
        if (Random.Range(0, 2).Equals(0))
        {
            mandooType = MandooType.고기만두;
            image.sprite = MandooManager.instance.meatMandooSprite;
        }
        else
        {
            mandooType = MandooType.김치만두;
            image.sprite = MandooManager.instance.gimchiMandooSprite;
        }
    }
}
