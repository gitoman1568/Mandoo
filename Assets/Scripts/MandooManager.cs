using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MandooManager : MonoBehaviour
{
    [Header("만두 베이스")]
    public GameObject mandoo;

    [Space(5)]
    [Header("상태 정보")]
    public int count = 0;
    public long score;
    public float time;
    public Text t_Score;
    public Text t_time;
    public Image timeBar;
    public MandooController bowlMandoo;

    [Space(5)]
    [Header("생성 된 만두 리스트")]
    public List<MandooController> mandooLeftList;
    public List<MandooController> mandooRightList;

    [Space(5)]
    [Header("레일")]
    public Transform mandooLeftParent;
    public Transform mandooRightParent;
    public Transform bowl;
    public Transform[] LeftRails;
    public Transform[] RightRails;

    [Space(5)]
    [Header("만두 이미지")]
    public Sprite meatMandooSprite;
    public Sprite gimchiMandooSprite;

    [Space(5)]
    [Header("왼손 애니메이션")]
    IEnumerator meat_Hand_Animation;
    public Image left_Hand_Image;
    public Sprite meatMandooHandSprite_Base;
    public Sprite meatMandooHandSprite;
    public Sprite meatMandooHandSprite_2;

    [Space(5)]
    [Header("오른손 애니메이션")]
    IEnumerator gimchi_Hand_Animation;
    public Image right_Hand_Image;
    public Sprite gimchiMandooHandSprite_Base;
    public Sprite gimchiMandooHandSprite;
    public Sprite gimchiMandooHandSprite_2;

    [Space(5)]
    [Header("왼쪽 요리사 애니메이션")]
    IEnumerator left_Cooker_Animation;
    public Image left_Cooker_Image;
    public Sprite left_Cooker_Sprite;
    public Sprite left_Cooker_Sprite2;

    [Space(5)]
    [Header("오른쪽 요리사 애니메이션")]
    IEnumerator right_Cooker_Animation;
    public Image right_Cooker_Image;
    public Sprite right_Cooker_Sprite;
    public Sprite right_Cooker_Sprite2;

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

    //이 전 게임 초기화
    public void Initialize()
    {
        if (mandooLeftList.Count > 0)
        {
            mandooLeftList.Clear();
        }

        if (mandooRightList.Count > 0)
        {
            mandooRightList.Clear();
        }

        count = 0;
        score = 0;
        time = 60;
    }

    public void GameStart()
    {
        //이 전 게임 초기화
        Initialize();
        StartCoroutine(TimeStart());

        //왼쪽 만두 생성
        for (int i = 0; i < 8; i++)
        {
            GameObject _mandoo = Instantiate(mandoo, mandooLeftParent);
            MandooController mandooController = _mandoo.GetComponent<MandooController>();

            //만두 첫 포지션 결정
            _mandoo.transform.position = LeftRails[i].position;

            if (i > LeftRails.Length - 4)
            {
                _mandoo.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            }
            else
            {
                float value = 1 - (i * 0.18f);
                _mandoo.transform.localScale = new Vector3(value, value, value);
            }

            mandooLeftList.Add(mandooController);

            mandooController.TypeRandomSetting();
        }

        //오른쪽 만두 생성
        for (int i = 0; i < 8; i++)
        {
            GameObject _mandoo = Instantiate(mandoo, mandooRightParent);
            MandooController mandooController = _mandoo.GetComponent<MandooController>();

            //만두 첫 포지션 결정
            _mandoo.transform.position = RightRails[i].position;

            if (i > RightRails.Length - 4)
            {
                _mandoo.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            }
            else
            {
                float value = 1 - (i * 0.18f);
                _mandoo.transform.localScale = new Vector3(value, value, value);
            }

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
                bowlMandoo.gameObject.SetActive(false);

                //다음 만두 애니메이션
                Tweener next_MandooAnim = nextMandoo.transform.DOMove(bowl.position, 0.1f);
                nextMandoo.transform.DOScale(new Vector3(2, 1.83f, 2), 0.1f);
                next_MandooAnim.OnComplete(() =>
                {
                    bowlMandoo.gameObject.SetActive(true);
                    Destroy(nextMandoo.gameObject);

                    //요리사 애니메이션
                    if (left_Cooker_Animation != null)
                    {
                        StopCoroutine(left_Cooker_Animation);
                    }

                    left_Cooker_Animation = LeftCookerAnimation();
                    StartCoroutine(left_Cooker_Animation);
                });

                //기존 만두 애니메이션
                for (int i = 0; i < mandooLeftList.Count; i++)
                {
                    mandooLeftList[i].transform.DOMove(LeftRails[i].position, 0.1f);

                    if (i > LeftRails.Length - 4)
                    {
                        mandooLeftList[i].transform.DOScale(new Vector3(0.1f, 0.1f, 0.1f), 0.1f);
                    }
                    else
                    {
                        float value = 1 - (i * 0.18f);
                        mandooLeftList[i].transform.DOScale(new Vector3(value, value, value), 0.1f);
                    }
                }

                //새로운 만두 생성
                GameObject _mandoo = Instantiate(mandoo, mandooLeftParent);
                MandooController mandooController = _mandoo.GetComponent<MandooController>();

                //만두 첫 포지션 설정
                _mandoo.transform.position = LeftRails[LeftRails.Length - 1].position;
                _mandoo.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

                //만두 리스트에 추가
                mandooLeftList.Add(mandooController);

                //만두 종류 설정
                mandooController.TypeRandomSetting();
            }
            else
            {
                //그릇 위 만두 세팅
                MandooController nextMandoo = mandooRightList[0];
                bowlMandoo.TypeSetting(nextMandoo);
                mandooRightList.Remove(nextMandoo);
                bowlMandoo.gameObject.SetActive(false);

                //다음 만두 애니메이션
                Tweener next_MandooAnim = nextMandoo.transform.DOMove(bowl.position, 0.1f);
                nextMandoo.transform.DOScale(new Vector3(2, 1.83f, 2), 0.1f);
                next_MandooAnim.OnComplete(() =>
                {
                    bowlMandoo.gameObject.SetActive(true);
                    Destroy(nextMandoo.gameObject);

                    //요리사 애니메이션
                    if (right_Cooker_Animation != null)
                    {
                        StopCoroutine(right_Cooker_Animation);
                    }

                    right_Cooker_Animation = RightCookerAnimation();
                    StartCoroutine(right_Cooker_Animation);
                });

                //기존 만두 애니메이션
                for (int i = 0; i < mandooRightList.Count; i++)
                {
                    mandooRightList[i].transform.DOMove(RightRails[i].position, 0.1f);

                    if (i > RightRails.Length - 4)
                    {
                        mandooRightList[i].transform.DOScale(new Vector3(0.1f, 0.1f, 0.1f), 0.1f);
                    }
                    else
                    {
                        float value = 1 - (i * 0.18f);
                        mandooRightList[i].transform.DOScale(new Vector3(value, value, value), 0.1f);
                    }
                }

                //새로운 만두 생성
                GameObject _mandoo = Instantiate(mandoo, mandooRightParent);
                MandooController mandooController = _mandoo.GetComponent<MandooController>();

                //만두 첫 포지션 설정
                _mandoo.transform.position = RightRails[RightRails.Length - 1].position;
                _mandoo.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

                mandooRightList.Add(mandooController);

                mandooController.TypeRandomSetting();
            }

            count++;

            //손 애니메이션
            if (type.Equals(0))
            {
                if (meat_Hand_Animation != null)
                {
                    StopCoroutine(meat_Hand_Animation);
                }

                meat_Hand_Animation = MeatMandooAnimation();
                StartCoroutine(meat_Hand_Animation);
            }
            else
            {
                if (gimchi_Hand_Animation != null)
                {
                    StopCoroutine(gimchi_Hand_Animation);
                }

                gimchi_Hand_Animation = GimchiMandooAnimation();
                StartCoroutine(gimchi_Hand_Animation);
            }

            //스코어 추가
            score += 500;

            //시간 추가
            if (time < 59.5f)
            {
                time += 0.5f;
            }
            else
            {
                time = 60;
            }

            //UI 새로고침
            UIRefresh();
        }
        else
        {
            //실패
            time -= 1;
            TimeRefresh();
        }
    }

    public void UIRefresh()
    {
        t_Score.text = Comma_Set(score) + "원";

        TimeRefresh();
    }

    public void TimeRefresh()
    {
        timeBar.fillAmount = time / 60;

        t_time.text = time.ToString("f0");
    }

    IEnumerator MeatMandooAnimation()
    {
        left_Hand_Image.sprite = meatMandooHandSprite;

        yield return new WaitForSeconds(0.1f);

        left_Hand_Image.sprite = meatMandooHandSprite_2;

        yield return new WaitForSeconds(0.1f);

        left_Hand_Image.sprite = meatMandooHandSprite_Base;
    }

    IEnumerator GimchiMandooAnimation()
    {
        right_Hand_Image.sprite = gimchiMandooHandSprite;

        yield return new WaitForSeconds(0.1f);

        right_Hand_Image.sprite = gimchiMandooHandSprite_2;

        yield return new WaitForSeconds(0.1f);

        right_Hand_Image.sprite = gimchiMandooHandSprite_Base;
    }

    IEnumerator LeftCookerAnimation()
    {
        left_Cooker_Image.sprite = left_Cooker_Sprite;

        yield return new WaitForSeconds(0.2f);

        left_Cooker_Image.sprite = left_Cooker_Sprite2;
    }

    IEnumerator RightCookerAnimation()
    {
        right_Cooker_Image.sprite = right_Cooker_Sprite;

        yield return new WaitForSeconds(0.2f);

        right_Cooker_Image.sprite = right_Cooker_Sprite2;
    }

    IEnumerator TimeStart()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);

            time -= 0.1f + ((count * 0.1f) * 0.05f);

            TimeRefresh();

            if (time <= 0)
            {
                //게임 오버
                LobbyManager.instance.ReturnRobby();
                break;
            }
        }
    }

    /// <summary>
    /// 1000단위 콤마 포맷 적용 해주는 메소드
    /// </summary>
    public string Comma_Set(float value)
    {
        return (string.Format("{0:#,###0}", value));
    }
}
