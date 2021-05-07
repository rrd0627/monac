using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    int state;

    public GameObject MyChar;
    public GameObject EnemyChar;
    public GameObject EnemyChar2;

    public GameObject WaitGoButton;
    public GameObject BridgeButton;
    public GameObject BarricateButton;

    public GameObject Info;

    public GameObject DragPrefab;
    private GameObject DragPrefab_temp;

    private Vector3Int goal;
    private Vector3Int goal1;
    private Vector3Int goal2;

    private bool IsBridgeOn;
    private bool IsMoveOn;
    private bool TextSwitch;

    public GameObject BlinkObject;
    public GameObject CharBlickObject;
    // Start is called before the first frame update

    private GameObject barricade;

    public GameObject Bridgetemp;
    public GameObject Barricadetemp;

    Touch touch;
    private void Awake()
    {
        MyChar.GetComponent<Unit>().IsStand = true;
        EnemyChar.GetComponent<Unit>().IsStand = true;
        EnemyChar2.GetComponent<Unit>().IsStand = true;

        DataManager.instance.SelectedChar = 2;
    }

    void Start()
    {
        DataManager.instance.CameraStartPos[0] = new Vector3(1, 2, 0);

        IsBridgeOn = false;
        IsMoveOn = false;
        TextSwitch = false;
        BlinkObject.SetActive(false);
        state = 0;
        goal = new Vector3Int(9, 8, 1);
        goal1 = new Vector3Int(9, 10, 1);
        goal2 = new Vector3Int(14, 8, 1);
        MyChar.GetComponent<Unit>().IsStand = true;
        EnemyChar.GetComponent<Unit>().IsStand = true;
        EnemyChar2.GetComponent<Unit>().IsStand = true;


        GameManager.instance.money[1] = 920627;
        GameManager.instance.money[2] = 920627;
        GameManager.instance.money[3] = 920627;
        GameManager.instance.money[4] = 920627;
        if(DataManager.instance.IsKorean)
            InfoTexting("게임 스타트버튼을 누르세요");
        else
        {
            InfoTexting("Press StartButton");
        }

        BGMManager.instance.FadeInMusic();
        BGMManager.instance.Play(7);
    }
    public void GameStart()
    {
        MyChar.GetComponent<Unit>().IsStand = true;
        EnemyChar.GetComponent<Unit>().IsStand = true;
        EnemyChar2.GetComponent<Unit>().IsStand = true;

        CharBlickObject.transform.position = WaitGoButton.transform.position + Vector3.up * 1.5f;
        CharBlickObject.transform.SetParent(WaitGoButton.transform);
        CharBlickObject.SetActive(true);
        Time.timeScale = 1;
        if (DataManager.instance.IsKorean)
            InfoTexting("왼쪽 WAIT 버튼을 눌러 <color=#00ff00>대기</color> 상태로 변경하세요.");
        else
        {
            InfoTexting("Press WAIT Button to <color=#00ff00>wait</color> state");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount>0)
        {
            touch = Input.GetTouch(0);
        }

        if (state ==0)
        {
            for (int i = 0; i < GameManager.instance.Unit1.Count; i++)
            {
                if (GameManager.instance.Unit1[i].GetComponent<Unit>().Next == new Vector3Int(5, 3, 1))
                {
                    GameManager.instance.Unit1[i].GetComponent<Unit>().Next = new Vector3Int(4, 3, 1);
                    GameManager.instance.Unit1[i].GetComponent<Unit>().IsStand = true;
                    GameManager.instance.Unit1[i].GetComponent<Unit>().StopAllCoroutines();
                    GameManager.instance.Unit1[i].GetComponent<Unit>().GoOrder(new Vector3Int(4, 3, 1));
                }
            }

            if (Input.touchCount > 0)
            {
                if (touch.phase == TouchPhase.Began && IsMoveOn)
                {
                    CharacterSelectPlease();
                    IsMoveOn = false;
                }
            }
            else if(Input.GetMouseButtonDown(0)&&IsMoveOn)
            {
                CharacterSelectPlease();
                IsMoveOn = false;
            }                
        }
        else if (state == 1)
        {
            if (GameManager.instance.GetComponent<Click>().ClickedChars.Count>0)
            {
                if (DataManager.instance.IsKorean)
                    InfoTexting("드래그 해서 부대를 이동하세요");
                else
                {
                    InfoTexting("Drag and Move Unit");
                }
                

                CharBlickObject.transform.position = GameManager.instance.map.CellToWorld(goal)+0.6f*Vector3.up;

                if (DragPrefab_temp == null)
                {
                    DragPrefab_temp = Instantiate(DragPrefab, GameManager.instance.GetComponent<Click>().ClickedChars[0].transform.position, Quaternion.identity);
                }
            }
            else
            {
                if(MyChar.GetComponent<Unit>().Goal == goal)
                {
                    if (DataManager.instance.IsKorean)
                        InfoTexting("좋습니다");
                    else
                    {
                        InfoTexting("Good");
                    }
                }
                else
                {
                    if (DataManager.instance.IsKorean)
                        InfoTexting("부대를 선택하세요");
                    else
                    {
                        InfoTexting("Touch Unit");
                    }
                }                    
            }

            if(MyChar.GetComponent<Unit>().Cur == goal)
            {
                MyChar.GetComponent<Unit>().StopAllCoroutines();
                MyChar.GetComponent<Unit>().Goal = goal;
                MyChar.GetComponent<Unit>().IsChase = false;
                MyChar.GetComponent<Unit>().GoOrder(goal);
                MyChar.GetComponent<Unit>().IsStand = true;

                state = 2;

                CharBlickObject.SetActive(false);
                BlinkObject.SetActive(true);

                if (DataManager.instance.IsKorean)
                    InfoTexting("드래그하여 목적지로 이동시킬 수 있습니다.");
                else
                {
                    InfoTexting("You can Move Unit to Destination with Drag");
                }
            }
        }
        else if(state == 2)
        {
            if (Input.touchCount > 0)
            {
                if (touch.phase == TouchPhase.Began && GameManager.instance.GetComponent<Click>().ClickedChars.Count == 0)
                {
                    IsBridgeOn = true;
                    BridgeOn();
                }
            }
            else if (Input.GetMouseButtonDown(0) && GameManager.instance.GetComponent<Click>().ClickedChars.Count == 0)
            {
                IsBridgeOn = true;
                BridgeOn();
            }

            if (GameManager.instance.GetComponent<Click>().ClickedChars.Count > 0 && !GameManager.instance.GetComponent<Click>().BridgePrefab.activeInHierarchy)
            {                
                CharBlickObject.transform.SetParent(BridgeButton.transform);
                CharBlickObject.transform.position = BridgeButton.transform.position + Vector3.up;
            }
            else if(GameManager.instance.GetComponent<Click>().ClickedChars.Count == 0)
            {
                //MyChar.GetComponent<Unit>().StopAllCoroutines();
                //MyChar.GetComponent<Unit>().IsBridge = false;
                MyChar.GetComponent<Unit>().IsStand = true;
                MyChar.GetComponent<Unit>().IsChase = false;
                MyChar.GetComponent<Unit>().GoOrder(goal);

                if (IsBridgeOn)
                    BridgeOn();
                CharBlickObject.transform.SetParent(null);
                CharBlickObject.transform.position = MyChar.transform.position +Vector3.up*1.5f;
            }
            if (GameManager.instance.GetComponent<Click>().BridgePrefab.activeInHierarchy)
            {
                //MyChar.GetComponent<Unit>().StopAllCoroutines();
                //MyChar.GetComponent<Unit>().IsBridge = false;
                MyChar.GetComponent<Unit>().IsStand = true;
                MyChar.GetComponent<Unit>().IsChase = false;
                MyChar.GetComponent<Unit>().GoOrder(goal);
                CharBlickObject.transform.SetParent(null);
                CharBlickObject.transform.position = Vector3.up * 0.5f + GameManager.instance.map.CellToWorld(new Vector3Int(9, 9, 1));
                Bridgetemp.SetActive(true);

                if (DataManager.instance.IsKorean)
                    InfoTexting("다리를 <color=#00ff00>드래그</color>하여 왼쪽 섬에 이어지도록 건설하세요");
                else
                {
                    InfoTexting("<color=#00ff00>Drag</color> the bridge to build it to the left island.");
                }
            }

            if(!GameManager.instance.BridgeAble)
            {
                MyChar.GetComponent<Unit>().IsStand = true;
                MyChar.GetComponent<Unit>().GoOrder(goal);
                CharBlickObject.SetActive(false);
                //CharBlickObject.transform.SetParent(WaitGoButton.transform);
                //CharBlickObject.transform.position = WaitGoButton.transform.position + 2 * Vector3.up;
                BridgeButton.GetComponent<Button>().enabled = false;
                Bridgetemp.SetActive(false);
                //WaitGoButton.GetComponent<Button>().enabled = true;
                //GameManager.instance.GetComponent<Click>().enabled = false;
                //MyChar.GetComponent<Unit>().Next = goal;
                state = 3;
            }
        }
        else if(state ==3)
        {
            A_PressedWait();
            state = 4;
        }
        else if(state ==4)
        {
            if(MyChar.GetComponent<Unit>().Next == new Vector3Int(10,8,1))
            {
                MyChar.GetComponent<Unit>().Next = goal;
                MyChar.GetComponent<Unit>().IsStand = true;
                MyChar.GetComponent<Unit>().IsChase = false;
                MyChar.GetComponent<Unit>().StopAllCoroutines();
                MyChar.GetComponent<Unit>().GoOrder(goal);
                if (DataManager.instance.IsKorean)
                    InfoTexting("섬으로 가세요!!");
                else
                {
                    InfoTexting("Go to Island");
                }
            }

            if(MyChar.GetComponent<Unit>().Cur == goal1)
            {
                GameManager.instance.GetComponent<Click>().enabled = false;
                
                if (DataManager.instance.IsKorean)
                    InfoTexting("부대는 <color=#00ff00>자동</color>으로 <color=#00ff00>병영</color>을 건설하고\n병영은 <color=#00ff00>부대를 생산</color>합니다");
                else
                {
                    InfoTexting("Unit build <color=#00ff00>Barracks automatically</color>\nAnd Barracks <color=#00ff00>produce Unit</color>");
                }
                BlinkObject.SetActive(true);
                CharBlickObject.SetActive(false);

                state = 17;
                TextSwitch = true;
            }
            for (int i = 0; i < GameManager.instance.Unit1.Count; i++)
            {
                if (GameManager.instance.Unit1[i].GetComponent<Unit>().Next == new Vector3Int(10, 8, 1))
                {
                    GameManager.instance.Unit1[i].GetComponent<Unit>().Next = goal;
                    GameManager.instance.Unit1[i].GetComponent<Unit>().IsStand = true;
                    GameManager.instance.Unit1[i].GetComponent<Unit>().StopAllCoroutines();
                    GameManager.instance.Unit1[i].GetComponent<Unit>().GoOrder(goal);
                }
            }
        }
        else if(state == 17)
        {
            for (int i = 0; i < GameManager.instance.Unit1.Count; i++)
            {
                if (GameManager.instance.Unit1[i].GetComponent<Unit>().Next == new Vector3Int(10, 8, 1))
                {
                    GameManager.instance.Unit1[i].GetComponent<Unit>().Next = goal;
                    GameManager.instance.Unit1[i].GetComponent<Unit>().IsStand = true;
                    GameManager.instance.Unit1[i].GetComponent<Unit>().StopAllCoroutines();
                    GameManager.instance.Unit1[i].GetComponent<Unit>().GoOrder(goal);
                }
            }

            if (Input.touchCount > 0)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    if (DataManager.instance.IsKorean)
                        InfoTexting("<color=#00ff00>평야</color>에는 병영을 건설할 수 있지만\n<color=#00ff00>암석</color>지대는 불가능합니다");
                    else
                    {
                        InfoTexting("You can build barracks on the <color=#00ff00>plains</color>\nBut you can't on the <color=#00ff00>rocky zone</color>");
                    }
                    state = 5;
                }
            }
            else if (Input.GetMouseButtonDown(0))
            {
                if (DataManager.instance.IsKorean)
                    InfoTexting("<color=#00ff00>평야</color>에는 병영을 건설할 수 있지만\n<color=#00ff00>암석</color>지대는 불가능합니다");
                else
                {
                    InfoTexting("You can build barracks on the <color=#00ff00>plains</color>\nBut you can't on the <color=#00ff00>rocky zone</color>");
                }
                state = 5;
            }
        }
        else if(state ==5)
        {
            if (TextSwitch && Input.GetMouseButtonDown(0))
            {
                TextSwitch = false;

                if (DataManager.instance.IsKorean)
                    InfoTexting("땅을 차지하면 일정시간 마다 <color=#00ff00>식량</color>을 수확합니다\n병사들은 식량을 소모합니다");
                else
                {
                    InfoTexting("If we take up the land, we harvest <color=#00ff00>food</color> every hour\nUnit consume food");
                }
            }
            if(Input.touchCount > 0)
            {
                if(TextSwitch && touch.phase == TouchPhase.Began)
                {
                    TextSwitch = false;
                    if (DataManager.instance.IsKorean)
                        InfoTexting("땅을 차지하면 일정시간 마다 <color=#00ff00>식량</color>을 수확합니다\n병사들은 식량을 소모합니다");
                    else
                    {
                        InfoTexting("If we take up the land, we harvest <color=#00ff00>food</color> every hour\nUnit consume food");
                    }
                }
            }


            for(int i=0;i<GameManager.instance.Unit1.Count;i++)
            {
                if (GameManager.instance.Unit1[i].GetComponent<Unit>().Next == new Vector3Int(10, 8, 1))
                {
                    GameManager.instance.Unit1[i].GetComponent<Unit>().Next = goal;
                    GameManager.instance.Unit1[i].GetComponent<Unit>().IsStand = true;
                    GameManager.instance.Unit1[i].GetComponent<Unit>().StopAllCoroutines();
                    GameManager.instance.Unit1[i].GetComponent<Unit>().GoOrder(goal);
                }
            }

            for (int i = 0; i < GameManager.instance.Unit1.Count; i++)
            {
                if (GameManager.instance.Unit1[i].GetComponent<Unit>().UnitLevel >= 1)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (DataManager.instance.IsKorean)
                            InfoTexting("병력이 모여 강화되었습니다");
                        else
                        {
                            InfoTexting("When Unit are combined they become stronger");
                        }
                        state = 6;
                        TextSwitch = true;
                    }
                    if (Input.touchCount > 0)
                    {
                        if (touch.phase == TouchPhase.Began)
                        {
                            if (DataManager.instance.IsKorean)
                                InfoTexting("병력이 모여 강화되었습니다");
                            else
                            {
                                InfoTexting("When Unit are combined they become stronger");
                            }
                            state = 6;
                            TextSwitch = true;
                        }
                    }
                }
            }
        }
        else if(state ==6)
        {
            for (int i = 0; i < GameManager.instance.Unit1.Count; i++)
            {
                if (GameManager.instance.Unit1[i].GetComponent<Unit>().Next == new Vector3Int(10, 8, 1))
                {
                    GameManager.instance.Unit1[i].GetComponent<Unit>().Next = goal;
                    GameManager.instance.Unit1[i].GetComponent<Unit>().IsStand = true;
                    GameManager.instance.Unit1[i].GetComponent<Unit>().StopAllCoroutines();
                    GameManager.instance.Unit1[i].GetComponent<Unit>().GoOrder(goal);
                }
            }

            if (TextSwitch && Input.GetMouseButtonDown(0))
            {
                if (DataManager.instance.IsKorean)
                    InfoTexting("<color=#00ff00>목책</color>은 부대의 이동을 <color=#00ff00>방해</color> 할 수 있습니다");
                else
                {
                    InfoTexting("<color=#00ff00>Barricade blocks</color> the movement of Unit");
                }
                BarricateOn();

                TextSwitch = true;

                state = 7;
            }
            if (Input.touchCount > 0)
            {
                if (TextSwitch && touch.phase == TouchPhase.Began)
                {
                    if (DataManager.instance.IsKorean)
                        InfoTexting("<color=#00ff00>목책</color>은 부대의 이동을 <color=#00ff00>방해</color> 할 수 있습니다");
                    else
                    {
                        InfoTexting("<color=#00ff00>Barricade blocks</color> the movement of Unit");
                    }

                    BarricateOn();

                    TextSwitch = true;

                    state = 7;
                }
            }
        }
        else if(state == 7)
        {
            if(TextSwitch && Input.GetMouseButtonDown(0))
            {
                GameManager.instance.GetComponent<Click>().enabled = true;
                GameManager.instance.GetComponent<Click>().IsStandPush = false;

                if (DataManager.instance.IsKorean)
                    InfoTexting("부대를 터치 후 목책 건설 버튼을 누르세요");
                else
                {
                    InfoTexting("Touch Unit and press the Barricade Button");
                }

                BlinkObject.SetActive(false);
                CharBlickObject.transform.SetParent(null);
                CharBlickObject.transform.position = GameManager.instance.Unit1[0].transform.position +  Vector3.up;
                CharBlickObject.SetActive(true);

                TextSwitch = false;
            }
            if (Input.touchCount > 0)
            {
                if (TextSwitch && touch.phase == TouchPhase.Began)
                {
                    GameManager.instance.GetComponent<Click>().enabled = true;
                    GameManager.instance.GetComponent<Click>().IsStandPush = false;
                    if (DataManager.instance.IsKorean)
                        InfoTexting("부대를 터치 후 목책 건설 버튼을 누르세요");
                    else
                    {
                        InfoTexting("Touch Unit and press the Barricade Button");
                    }

                    BlinkObject.SetActive(false);
                    CharBlickObject.transform.SetParent(null);
                    CharBlickObject.transform.position = GameManager.instance.Unit1[0].transform.position + Vector3.up;
                    CharBlickObject.SetActive(true);

                    TextSwitch = false;
                }
            }


            if (GameManager.instance.GetComponent<Click>().ObstaclePrefab.activeInHierarchy)
            {
                CharBlickObject.transform.SetParent(null);
                CharBlickObject.transform.position = GameManager.instance.map.CellToWorld(new Vector3Int(15, 8, 1)) + 0.6f * Vector3.up;
                Barricadetemp.SetActive(true);
                if (GameManager.instance.GetComponent<Click>().SelectedTile != new Vector3Int(15, 8, 1))
                {
                    GameManager.instance.GetComponent<Click>().ObstaclePrefab.GetComponentInChildren<Button>().gameObject.GetComponent<Image>().color = Color.gray * 1.5f;
                    GameManager.instance.GetComponent<Click>().ObstaclePrefab.GetComponentInChildren<Button>().enabled = false;
                    GameManager.instance.GetComponent<Click>().ObstaclePrefab.GetComponentInChildren<CircleCollider2D>().gameObject.GetComponent<SpriteRenderer>().color = Color.red / 2f;
                }
                else
                {
                    GameManager.instance.GetComponent<Click>().ObstaclePrefab.GetComponentInChildren<Button>().gameObject.GetComponent<Image>().color = Color.white;
                    GameManager.instance.GetComponent<Click>().ObstaclePrefab.GetComponentInChildren<Button>().enabled = true;
                    GameManager.instance.GetComponent<Click>().ObstaclePrefab.GetComponentInChildren<CircleCollider2D>().gameObject.GetComponent<SpriteRenderer>().color = Color.green / 2f;
                }
            }
            else if (GameManager.instance.GetComponent<Click>().ClickedChars.Count > 0)
            {
                CharBlickObject.transform.SetParent(BarricateButton.transform);
                CharBlickObject.transform.position = BarricateButton.transform.position + Vector3.up;
            }
            else
            {
                CharBlickObject.transform.SetParent(null);
                CharBlickObject.transform.position = GameManager.instance.Unit1[0].transform.position + 1.5f * Vector3.up;
            }


            for (int i = 0; i < GameManager.instance.Unit1.Count; i++)
            {
                if (GameManager.instance.Unit1[i].GetComponent<Unit>().Next == new Vector3Int(15, 8, 1))
                {
                    GameManager.instance.Unit1[i].GetComponent<Unit>().IsStand = true;
                    GameManager.instance.Unit1[i].GetComponent<Unit>().IsChase = false;
                    GameManager.instance.Unit1[i].GetComponent<Unit>().Next = goal2;
                    GameManager.instance.Unit1[i].GetComponent<Unit>().StopAllCoroutines();
                    GameManager.instance.Unit1[i].GetComponent<Unit>().GoOrder(goal2);
                    if (DataManager.instance.IsKorean)
                        InfoTexting("지정된 위치에 목책을 건설 하세요");
                    else
                    {
                        InfoTexting("Build Barricade at the location");
                    }

                }
            }

            if (GameManager.instance.ObstacleCooltime_Cur!=0)
            {
                barricade = GameObject.FindGameObjectWithTag("TreeAndBush");
                barricade.GetComponent<TreeAndBush>().HP = 200;

                MyChar.GetComponent<Unit>().IsStand = true;

                BarricateButton.GetComponent<Button>().enabled = false;
                Barricadetemp.SetActive(false);
                if (DataManager.instance.IsKorean)
                    InfoTexting("적군은 목책을 공격하거나 <color=#00ff00>회피</color>합니다");
                else
                {
                    InfoTexting("Enemy attacks Barricade or <color=#00ff00>avoids</color> it");
                }
                BlinkObject.SetActive(true);
                CharBlickObject.SetActive(false);

                EnemyChar.GetComponent<Unit>().Goal = goal;
                EnemyChar.GetComponent<Unit>().IsStand = false;
                TextSwitch = true;
                state = 8;
            }
        }
        else if(state ==8)
        {
            for (int i = 0; i < GameManager.instance.Unit1.Count; i++)
            {
                if (GameManager.instance.Unit1[i].GetComponent<Unit>().Next == new Vector3Int(15, 8, 1))
                {
                    GameManager.instance.Unit1[i].GetComponent<Unit>().Next = goal2;
                    GameManager.instance.Unit1[i].GetComponent<Unit>().IsStand = true;
                    GameManager.instance.Unit1[i].GetComponent<Unit>().StopAllCoroutines();
                    GameManager.instance.Unit1[i].GetComponent<Unit>().GoOrder(goal2);
                }
            }

            if (TextSwitch && Input.GetMouseButtonDown(0) && barricade.GetComponent<TreeAndBush>().HP <= 0)
            {
                if (DataManager.instance.IsKorean)
                    InfoTexting("<color=#00ff00>적군의 모든 병영과 부대가 없어지면 승리합니다</color>");
                else
                {
                    InfoTexting("<color=#00ff00>When all Barracks and Enemy are gone, You win</color>");
                }

                state = 9;
            }
            if (Input.touchCount > 0)
            {
                if (TextSwitch && touch.phase == TouchPhase.Began&& barricade.GetComponent<TreeAndBush>().HP <= 0)
                {
                    if (DataManager.instance.IsKorean)
                        InfoTexting("<color=#00ff00>적군의 모든 병영과 부대가 없어지면 승리합니다</color>");
                    else
                    {
                        InfoTexting("<color=#00ff00>When all Barracks and Enemy are gone, You win</color>");
                    }
                    state = 9;
                }
            }
        }
        else if(state ==9)
        {
            if (DragPrefab_temp == null && GameManager.instance.Unit1[0].GetComponent<Unit>().Goal != new Vector3Int(21, 18, 1))
            {
                DragPrefab_temp = Instantiate(DragPrefab, GameManager.instance.Unit1[0].transform.position, Quaternion.identity);
                DragPrefab_temp.GetComponent<DragObject>().vec3 = new Vector3Int(21, 18, 1);
            }
            if (TextSwitch && Input.GetMouseButtonDown(0))
            {
                BlinkObject.SetActive(false);

                if (DataManager.instance.IsKorean)
                    InfoTexting("모든 적군 부대와 병영을 공격하여 승리하세요");
                else
                {
                    InfoTexting("To win, Attack all enemy Unit and Barracks");
                }
            }
            if (Input.touchCount > 0)
            {
                if (TextSwitch && touch.phase == TouchPhase.Began)
                {
                    BlinkObject.SetActive(false);
                    if (DataManager.instance.IsKorean)
                        InfoTexting("모든 적군 부대와 병영을 공격하여 승리하세요");
                    else
                    {
                        InfoTexting("To win, Attack all enemy Unit and Barracks");
                    }
                }
            }
            if (GameManager.instance.VictoryPanel.activeInHierarchy)
            {
                state = 10;
                BlinkObject.SetActive(true);

                if (DataManager.instance.IsKorean)
                    InfoTexting("승리하였습니다!");
                else
                {
                    InfoTexting("Congratulation! You Win");
                }
            }
        }
        else if(state ==10)
        {

            if (TextSwitch && Input.GetMouseButtonDown(0))
            {
                if (DataManager.instance.IsKorean)
                    InfoTexting("다음 튜토리얼로 넘어갑니다");
                else
                {
                    InfoTexting("Go to next tutorial");
                }
                state = 11;
            }
            if (Input.touchCount > 0)
            {
                if (TextSwitch && touch.phase == TouchPhase.Began)
                {
                    if (DataManager.instance.IsKorean)
                        InfoTexting("다음 튜토리얼로 넘어갑니다");
                    else
                    {
                        InfoTexting("Go to next tutorial");
                    }
                }
                state = 11;
            }
        }
        else if (state == 11)
        {

            if (TextSwitch && Input.GetMouseButtonDown(0))
            {
                GoNextTuto();
            }
            if (Input.touchCount > 0)
            {
                if (TextSwitch && touch.phase == TouchPhase.Began)
                {
                    GoNextTuto();
                }
            }
        }
    }

    private void BridgeOn()
    {
        BlinkObject.SetActive(false);

        CharBlickObject.transform.position = MyChar.transform.position + Vector3.up * 1.5f;
        CharBlickObject.SetActive(true);

        BridgeButton.SetActive(true);

        if (DataManager.instance.IsKorean)
            InfoTexting("부대 터치 후 다리 건설 버튼을 누르세요");
        else
        {
            InfoTexting("Touch on unit and press the bridge button");
        }

    }
    public void BridgeButtonClick()
    {
        if(GameManager.instance.GetComponent<Click>().ClickedChars.Count > 0)
        {
            if (DataManager.instance.IsKorean)
                InfoTexting("다리를 <color=#00ff00>드래그</color>하여 왼쪽 섬으로 갈 수 있는 다리를 건설하세요");
            else
            {
                InfoTexting("<color=#00ff00>Drag</color> the bridge to build a bridge to the left island");
            }
        }
        else
        {
            if (DataManager.instance.IsKorean)
                InfoTexting("부대를 터치 후 다리 건설 버튼을 누르세요");
            else
            {
                InfoTexting("Touch on unit and press the bridge button");
            }
        }
    }


    private void BarricateOn()
    {
        BarricateButton.SetActive(true);
    }

    public void BarricateButtonClick()
    {
        if (GameManager.instance.GetComponent<Click>().ClickedChars.Count > 0)
        {
            if (DataManager.instance.IsKorean)
                InfoTexting("목책을 표시된 위치에 건설하세요");
            else
            {
                InfoTexting("Build the Barricade in the marked location");
            }
        }
        else
        {
            if (DataManager.instance.IsKorean)
                InfoTexting("부대를 터치 후 해당위치에 목책 건설 버튼을 누르세요");
            else
            {
                InfoTexting("Touch on the unit and press the Build Button for the location");
            }
        }
    }
    public void A_PressedWait()
    {
        if(state ==3)
        {
            WaitGoButton.GetComponent<Button>().enabled = false;
            GameManager.instance.GetComponent<Click>().enabled = true;
            GameManager.instance.GetComponent<Click>().IsStandPush = false;
            CharBlickObject.SetActive(true);
            CharBlickObject.transform.SetParent(null);
            CharBlickObject.transform.position = GameManager.instance.map.CellToWorld(new Vector3Int(9, 10, 1)) + Vector3.up * 0.5f;

            if (DataManager.instance.IsKorean)
                InfoTexting("섬으로 가세요");
            else
            {
                InfoTexting("Go to the island");
            }
            state = 4;
        }
        else
        {
            CharBlickObject.SetActive(false);
            BlinkObject.SetActive(true);
            if (DataManager.instance.IsKorean)
                InfoTexting("WAIT 상태이면 부대가 이동 후 <color=#00ff00>대기</color>합니다.\n상태는 버튼을 터치해서 변경 할 수 있습니다");
            else
            {
                InfoTexting("When in WAIT state, the unit will move and <color=#00ff00>wait</color>.\nYou can change the state by touching the button");
            }
            WaitGoButton.GetComponent<Button>().enabled = false;

            //캐릭터 환하게! 보여주기
            Time.timeScale = 1;

            IsMoveOn = true;
        }
    }

    public void CharacterSelectPlease()
    {
        GameManager.instance.GetComponent<Click>().enabled = true;
        GameManager.instance.GetComponent<Click>().IsStandPush = false;

        CharBlickObject.transform.SetParent(null);
        CharBlickObject.transform.position = MyChar.transform.position + Vector3.up * 1.5f;
        CharBlickObject.SetActive(true);
        BlinkObject.SetActive(false);

        state = 1;
    }



    public void InfoTexting(string _str)
    {
        Color color = new Color(1f, 1f, 1f, 0.5f);
        Info.GetComponent<Image>().color = color;
        Info.SetActive(true);
        Info.GetComponentInChildren<Text>().text = _str;        
    }


    public void RestartTuto()
    {
        AsyncOperation oper = new AsyncOperation();
        oper = SceneManager.LoadSceneAsync("Stage0");
        oper.allowSceneActivation = true;
    }


    public void GoNextTuto()
    {
        AsyncOperation oper = new AsyncOperation();
        oper = SceneManager.LoadSceneAsync("Stage0_1");
        oper.allowSceneActivation = true;
    }

}
