using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class CharacterSelect : MonoBehaviour
{
    public GameObject CircleCenter;

    private Vector3 pos;
    private Collider[] _col;

    private Vector3 Pos_Down;
    private Vector3 Pos_Cur;
    private Vector3 Pos_Up;
    private float DirX;

    private bool IsSelecting;

    public GameObject[] Char;

    public Text Name;
    private bool IsButtonPress;

    public GameObject[] CharSelectImage;


    public GameObject[] Attack;
    public GameObject[] Defense;
    public GameObject[] Speed;
    public GameObject[] Heal;

    // Start is called before the first frame update
    private void OnEnable()
    {
        DataManager.instance.SelectedChar = 2;
        IsButtonPress = false;
        IsSelecting = false;
    }

    void Start()
    {
        DataManager.instance.SelectedChar = 2;
        IsButtonPress = false;
        IsSelecting = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                if (RaycastWorldUI())
                    return;

                Pos_Down = Camera.main.ScreenToViewportPoint(touch.position);

                IsSelecting = true;
            }
            if (touch.phase == TouchPhase.Moved && IsSelecting)
            {
                Pos_Cur = Camera.main.ScreenToViewportPoint(touch.position);

                DirX = (Pos_Down.x - Pos_Cur.x);

                CircleCenter.transform.Rotate(500 * Vector3.up * DirX);

                Pos_Down = Camera.main.ScreenToViewportPoint(touch.position);
            }
            if (touch.phase == TouchPhase.Ended)
            {
                Pos_Up = Camera.main.ScreenToViewportPoint(touch.position);

                IsSelecting = false;
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (RaycastWorldUI())
                    return;
                Pos_Down = Camera.main.ScreenToViewportPoint(Input.mousePosition);

                IsSelecting = true;
            }
            if (Input.GetMouseButton(0) && IsSelecting)
            {
                Pos_Cur = Camera.main.ScreenToViewportPoint(Input.mousePosition);

                DirX = (Pos_Down.x - Pos_Cur.x);

                CircleCenter.transform.Rotate(500 * Vector3.up * DirX);

                Pos_Down = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            }
            if (Input.GetMouseButtonUp(0))
            {
                Pos_Up = Camera.main.ScreenToViewportPoint(Input.mousePosition);

                IsSelecting = false;
            }
        }
        
        if(!IsSelecting)
        {
            if (CircleCenter.transform.rotation.eulerAngles.y > 315 || CircleCenter.transform.rotation.eulerAngles.y <= 45)
            {
                CircleCenter.transform.rotation = Quaternion.Lerp(CircleCenter.transform.rotation, Quaternion.Euler(0, 0, 0), 10 * Time.deltaTime);

                if (DataManager.instance.SelectedChar == 1) return;
                DataManager.instance.SelectedChar = 1;

                Char[0].GetComponent<Animator>().SetInteger("animation", 20);
                Char[1].GetComponent<Animator>().SetInteger("animation", 1);
                Char[2].GetComponent<Animator>().SetInteger("animation", 1);
                Char[3].GetComponent<Animator>().SetInteger("animation", 1);
                Name.text = "Direk";
                Name.color = DataManager.instance.color1;
                CharSelectImage[0].SetActive(true);
                CharSelectImage[1].SetActive(false);
                CharSelectImage[2].SetActive(false);
                CharSelectImage[3].SetActive(false);

                for (int i=0;i< Attack.Length;i++)
                {
                    Attack[i].SetActive(false);
                    Defense[i].SetActive(false);
                    Speed[i].SetActive(false);
                    Heal[i].SetActive(false);
                }
                for(int i=0;i< 4;i++)
                {
                    Attack[i].SetActive(true);
                }
                for (int i = 0; i < 9; i++)
                {
                    Defense[i].SetActive(true);
                }
                for (int i = 0; i < 2; i++)
                {
                    Speed[i].SetActive(true);
                }
                for (int i = 0; i < 6; i++)
                {
                    Heal[i].SetActive(true);
                }

            }
            else if (CircleCenter.transform.rotation.eulerAngles.y > 225 && CircleCenter.transform.rotation.eulerAngles.y <= 315)
            {
                CircleCenter.transform.rotation = Quaternion.Lerp(CircleCenter.transform.rotation, Quaternion.Euler(0, -90, 0), 10 * Time.deltaTime);
                if (DataManager.instance.SelectedChar == 2) return;
                DataManager.instance.SelectedChar = 2;
                Char[0].GetComponent<Animator>().SetInteger("animation", 1);
                Char[1].GetComponent<Animator>().SetInteger("animation", 14);
                Char[2].GetComponent<Animator>().SetInteger("animation", 1);
                Char[3].GetComponent<Animator>().SetInteger("animation", 1);
                Name.text = "Valk";
                Name.color = DataManager.instance.color2;
                CharSelectImage[0].SetActive(false);
                CharSelectImage[1].SetActive(true);
                CharSelectImage[2].SetActive(false);
                CharSelectImage[3].SetActive(false);

                for (int i = 0; i < Attack.Length; i++)
                {
                    Attack[i].SetActive(false);
                    Defense[i].SetActive(false);
                    Speed[i].SetActive(false);
                    Heal[i].SetActive(false);
                }
                for (int i = 0; i < 6; i++)
                {
                    Attack[i].SetActive(true);
                }
                for (int i = 0; i < 1; i++)
                {
                    Defense[i].SetActive(true);
                }
                for (int i = 0; i < 9; i++)
                {
                    Speed[i].SetActive(true);
                }
                for (int i = 0; i < 4; i++)
                {
                    Heal[i].SetActive(true);
                }
            }
            else if (CircleCenter.transform.rotation.eulerAngles.y > 45 && CircleCenter.transform.rotation.eulerAngles.y <= 135)
            {
                CircleCenter.transform.rotation = Quaternion.Lerp(CircleCenter.transform.rotation, Quaternion.Euler(0, 90, 0), 10 * Time.deltaTime);
                if (DataManager.instance.SelectedChar == 3) return;
                DataManager.instance.SelectedChar = 3;

                Char[0].GetComponent<Animator>().SetInteger("animation", 1);
                Char[1].GetComponent<Animator>().SetInteger("animation", 1);
                Char[2].GetComponent<Animator>().SetInteger("animation", 5);
                Char[3].GetComponent<Animator>().SetInteger("animation", 1);
                Name.text = "Solar";
                Name.color = DataManager.instance.color3;
                CharSelectImage[0].SetActive(false);
                CharSelectImage[1].SetActive(false);
                CharSelectImage[2].SetActive(true);
                CharSelectImage[3].SetActive(false);

                for (int i = 0; i < Attack.Length; i++)
                {
                    Attack[i].SetActive(false);
                    Defense[i].SetActive(false);
                    Speed[i].SetActive(false);
                    Heal[i].SetActive(false);
                }
                for (int i = 0; i < 2; i++)
                {
                    Attack[i].SetActive(true);
                }
                for (int i = 0; i < 6; i++)
                {
                    Defense[i].SetActive(true);
                }
                for (int i = 0; i < 4; i++)
                {
                    Speed[i].SetActive(true);
                }
                for (int i = 0; i < 9; i++)
                {
                    Heal[i].SetActive(true);
                }
            }
            else
            {
                CircleCenter.transform.rotation = Quaternion.Lerp(CircleCenter.transform.rotation, Quaternion.Euler(0, 180, 0), 10 * Time.deltaTime);
                if (DataManager.instance.SelectedChar == 4) return;
                DataManager.instance.SelectedChar = 4;
                Char[0].GetComponent<Animator>().SetInteger("animation", 1);
                Char[1].GetComponent<Animator>().SetInteger("animation", 1);
                Char[2].GetComponent<Animator>().SetInteger("animation", 1);
                Char[3].GetComponent<Animator>().SetInteger("animation", 24);
                Name.text = "Punan";
                Name.color = DataManager.instance.color4;
                CharSelectImage[0].SetActive(false);
                CharSelectImage[1].SetActive(false);
                CharSelectImage[2].SetActive(false);
                CharSelectImage[3].SetActive(true);

                for (int i = 0; i < Attack.Length; i++)
                {
                    Attack[i].SetActive(false);
                    Defense[i].SetActive(false);
                    Speed[i].SetActive(false);
                    Heal[i].SetActive(false);
                }
                for (int i = 0; i < 9; i++)
                {
                    Attack[i].SetActive(true);
                }
                for (int i = 0; i < 4; i++)
                {
                    Defense[i].SetActive(true);
                }
                for (int i = 0; i < 6; i++)
                {
                    Speed[i].SetActive(true);
                }
                for (int i = 0; i < 0; i++)
                {
                    Heal[i].SetActive(true);
                }

            }
        }        
    }
    public void GoBack()
    {
        SoundManager.instance.Play(2);

        StageSelectPrefab.instance.gameObject.SetActive(true);
        CharSelectPrefab.instance.gameObject.SetActive(false);
        /*
        AsyncOperation oper = new AsyncOperation();
        oper = SceneManager.LoadSceneAsync("StageSelect");
        oper.allowSceneActivation = true;
        */
    }

    public void Select_GoLoadnig()
    {
        if (!IsButtonPress)
        {
            IsButtonPress = true;
        }
        else
            return;
        SoundManager.instance.Play(2);
        AsyncOperation oper = new AsyncOperation();
        oper = SceneManager.LoadSceneAsync("Loading");
        oper.allowSceneActivation = true;
        StageSelectPrefab.instance.gameObject.SetActive(false);
        //CharSelectPrefab.instance.gameObject.SetActive(false);
    }


    private bool RaycastWorldUI()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current);

        if (Input.touchCount > 0)
        {
            pointerData.position = Input.GetTouch(0).position;
        }
        else
        {
            pointerData.position = Input.mousePosition;
        }

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        if (results.Count > 0)
        {
            if (results[0].gameObject.layer == LayerMask.NameToLayer("WorldUI"))
            {
                return true;
            }
        }
        return false;
    }

}
