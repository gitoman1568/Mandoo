using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MandooManager : MonoBehaviour
{
    public GameObject mandoo;
    public Transform mandooLeftParent;
    public Transform mandooRightParent;
    public List<MandooController> mandooLeftList;
    public List<MandooController> mandooRightList;

    public int count = 0;
    public MandooController bowlMandoo;

    public Sprite meatMandooSprite;
    public Sprite gimchiMandooSprite;

    static MandooManager _instance;

    public static MandooManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<MandooManager>();
            }
            return _instance;
        }
    }

    void Start()
    {
        //왼쪽 만두 생성
        for (int i = 0; i < 5; i++)
        {
            GameObject _mandoo = Instantiate(mandoo, mandooLeftParent);
            MandooController mandooController = _mandoo.GetComponent<MandooController>();

            mandooLeftList.Add(mandooController);

            mandooController.TypeRandomSetting();
        }

        //오른쪽 만두 생성
        for (int i = 0; i < 5; i++)
        {
            GameObject _mandoo = Instantiate(mandoo, mandooRightParent);
            MandooController mandooController = _mandoo.GetComponent<MandooController>();

            mandooRightList.Add(mandooController);

            mandooController.TypeRandomSetting();
        }

        bowlMandoo.TypeRandomSetting();
    }

    public void MandooCategorize(int type)
    {
        MandooController.MandooType _type;

        if (type.Equals(0))
        {
            _type = MandooController.MandooType.고기만두;
        }
        else
        {
            _type = MandooController.MandooType.김치만두;
        }

        if (bowlMandoo.mandooType.Equals(_type))
        {
            //성공
            if (count % 2 > 0)
            {
                //그릇 위 만두 세팅
                MandooController nextMandoo = mandooLeftList[0];
                bowlMandoo.TypeSetting(nextMandoo);
                mandooLeftList.Remove(nextMandoo);

                Destroy(nextMandoo.gameObject);

                //새로운 만두 생성
                GameObject _mandoo = Instantiate(mandoo, mandooLeftParent);
                MandooController mandooController = _mandoo.GetComponent<MandooController>();

                mandooLeftList.Add(mandooController);

                mandooController.TypeRandomSetting();
            }
            else
            {
                //그릇 위 만두 세팅
                MandooController nextMandoo = mandooRightList[0];
                bowlMandoo.TypeSetting(nextMandoo);
                mandooRightList.Remove(nextMandoo);

                Destroy(nextMandoo.gameObject);

                //새로운 만두 생성
                GameObject _mandoo = Instantiate(mandoo, mandooRightParent);
                MandooController mandooController = _mandoo.GetComponent<MandooController>();

                mandooRightList.Add(mandooController);

                mandooController.TypeRandomSetting();
            }

            count++;
        }
        else
        {
            //실패
        }
    }
}
