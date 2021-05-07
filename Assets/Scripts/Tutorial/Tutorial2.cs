using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Tutorial2 : MonoBehaviour
{
    int state;

    public GameObject Info;

    public GameObject DragPrefab;
    private GameObject DragPrefab_temp;    

    public GameObject BlinkObject;
    public GameObject CharBlickObject;

    public GameObject[] MyChar;
    // Start is called before the first frame update
    public Button[] buttons;

    public GameObject Resource;

    public GameObject TutorialCircle;
    public GameObject ResourceBar;

    public GameObject Enemy;

    public GameObject barricade;

    public GameObject Gamespeed;
    private bool GamespeedButtonPress;
    void Start()
    {
        DataManager.instance.CameraStartPos[0] = new Vector3(4.3f,6.2f,0);
        GamespeedButtonPress = false;
        Gamespeed.GetComponent<Button>().enabled = false;
        BlinkObject.SetActive(false);
        CharBlickObject.SetActive(false);
        state = -1;

        GameManager.instance.money[1] = 920627;
        GameManager.instance.money[2] = 920627;
        GameManager.instance.money[3] = 920627;
        GameManager.instance.money[4] = 920627;

        

        if (DataManager.instance.IsKorean)
            InfoTexting("게임 스타트버튼을 누르세요");
        else
        {
            InfoTexting("Press StartButton");
        }
    }
    public void GameStart()
    {
        CharBlickObject.SetActive(true);
        CharBlickObject.transform.SetParent(null);
        CharBlickObject.transform.position = new Vector3(20,10,0);

        
        if (DataManager.instance.IsKorean)
            InfoTexting("<color=#00ff00>더블터치</color> 시 일정 영역의 부대가 모두 선택됩니다");
        else
        {
            InfoTexting("<color=#00ff00>Double touch</color> selects all units in a given area");
        }

        BlinkObject.SetActive(true);
        state = 0;
        buttons[0].enabled = false;
        buttons[1].enabled = false;
        buttons[2].enabled = false;

        for (int i = 0; i < GameManager.instance.Unit1.Count; i++)
        {
            GameManager.instance.Unit1[i].SetActive(false);
        }

        for (int i = 0; i < GameManager.instance.Unit1.Count; i++)
        {
            GameManager.instance.Unit1[i].SetActive(true);
            GameManager.instance.Unit1[i].GetComponent<Unit>().Amount = 400;
        }

        MyChar[0].GetComponent<Unit>().IsStand = true;
        MyChar[0].GetComponent<Unit>().IsStop = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(state ==0)
        {
            buttons[0].enabled = false;
            buttons[1].enabled = false;
            buttons[2].enabled = false;
            if (!GameManager.instance.GetComponent<Click>().ClickedChars.Contains(MyChar[1]))
                GameManager.instance.GetComponent<Click>().ClickedChars.Add(MyChar[1]);

            if (GameManager.instance.GetComponent<Click>().ClickedChars.Contains(MyChar[0]))
            {
                GameManager.instance.GetComponent<Click>().ClickedChars.Remove(MyChar[0]);
            }
            GameManager.instance.GetComponent<Click>().enabled = true;

            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    BlinkObject.SetActive(false);
                    TutorialCircle.SetActive(true);
                    if (DataManager.instance.IsKorean)
                        InfoTexting("<color=#00ff00>더블터치</color>로 해당 위치의 부대를 모두 선택하세요");
                    else
                    {
                        InfoTexting("<color=#00ff00>Double touch</color> and select all units in your location.");
                    }
                    state = 1;
                }
            }
            else if(Input.GetMouseButtonDown(0))
            {
                BlinkObject.SetActive(false);
                TutorialCircle.SetActive(true);
                if (DataManager.instance.IsKorean)
                    InfoTexting("<color=#00ff00>더블터치</color>로 해당 위치의 부대를 모두 선택하세요");
                else
                {
                    InfoTexting("<color=#00ff00>Double touch</color> and select all units in your location.");
                }
                state = 1;
            }
        }
        else if (state == 1)
        {
            buttons[0].enabled = false;
            buttons[1].enabled = false;
            buttons[2].enabled = false;
            GameManager.instance.GetComponent<Click>().enabled = true;

            if (GameManager.instance.GetComponent<Click>().ClickedChars.Contains(MyChar[0]))
            {
                GameManager.instance.GetComponent<Click>().ClickedChars.Remove(MyChar[0]);
            }

            if (!GameManager.instance.GetComponent<Click>().ClickedChars.Contains(MyChar[1]))
                GameManager.instance.GetComponent<Click>().ClickedChars.Add(MyChar[1]);
            if (GameManager.instance.GetComponent<Click>().ClickedChars.Count==7)
            {
                TutorialCircle.SetActive(false);
                state = 2;                
            }

        } //게임 정지 시키고 더블클릭해야한 다음으로 넘어가도록
        else if (state == 2)
        {
            if (DragPrefab_temp == null)
            {
                DragPrefab_temp = Instantiate(DragPrefab, GameManager.instance.Unit1[0].transform.position, Quaternion.identity);
                DragPrefab_temp.GetComponent<DragObject>().vec3 = new Vector3Int(17, 11, 1);
            }

            if (DataManager.instance.IsKorean)
                InfoTexting("드래그 하여 부대를 모으세요");
            else
            {
                InfoTexting("Drag and gather Units");
            }


            for(int i=1;i<MyChar.Length;i++)
            {
                if (MyChar[i].GetComponent<Unit>().Goal != new Vector3Int(17, 11, 1))
                {
                    break;
                }
                if(i == MyChar.Length-1)
                {
                    state = 3;
                    GameManager.instance.GetComponent<Click>().ClickedChars.Remove(MyChar[1]);
                    TutorialCircle.SetActive(false);
                    return;
                }                    
            }
            if (GameManager.instance.Unit1[0].GetComponent<Unit>().Amount >= 3200)
            {
                state = 3;
                TutorialCircle.SetActive(false);
                return;
            }
            if (GameManager.instance.GetComponent<Click>().ClickedChars.Contains(MyChar[0]))
            {
                GameManager.instance.GetComponent<Click>().ClickedChars.Remove(MyChar[0]);
            }
            if (!GameManager.instance.GetComponent<Click>().ClickedChars.Contains(MyChar[1]))
                GameManager.instance.GetComponent<Click>().ClickedChars.Add(MyChar[1]);
            if (GameManager.instance.GetComponent<Click>().ClickedChars.Count < 7)
            {
                TutorialCircle.SetActive(true);

                if (DataManager.instance.IsKorean)
                    InfoTexting("<color=#00ff00>더블터치</color>로 해당 위치의 부대를 모두 선택하세요");
                else
                {
                    InfoTexting("<color=#00ff00>Double touch</color> and select all units in your location.");
                }
            }
            else
            {
                TutorialCircle.SetActive(false);
            }
        }
        else if (state == 3)
        {
            GameManager.instance.GetComponent<Click>().enabled = false;
            if (GameManager.instance.Unit1[0].GetComponent<Unit>().Amount >= 3200)
            {

                if (DataManager.instance.IsKorean)
                    InfoTexting("모두 모였습니다");
                else
                {
                    InfoTexting("Good");
                }

                BlinkObject.SetActive(true);
                if (Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(0);
                    if (touch.phase == TouchPhase.Began)
                    {                        
                        state = 4;
                    }
                }
                else if (Input.GetMouseButtonDown(0))
                {                    
                    state = 4;
                }
            }
        } //목책앞으로 이동하게함
        if (state == 4)
        {
            if (DataManager.instance.IsKorean)
                InfoTexting("부대가 합쳐져 병력이 <color=#00ff00>많아지면 강해</color>집니다");
            else
            {
                InfoTexting("As the Unit <color=#00ff00>come together</color>, the forces become <color=#00ff00>stronger</color>");
            }
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    state = 5;
                    ResourceBar.SetActive(true);

                    CharBlickObject.transform.position = Resource.transform.position + Vector3.down * 1.5f + Vector3.left * 0.5f;
                    CharBlickObject.transform.SetParent(Resource.transform);
                    CharBlickObject.GetComponent<SpriteRenderer>().flipY = true;
                    CharBlickObject.SetActive(true);
                }
            }
            else if (Input.GetMouseButtonDown(0))
            {
                state = 5;
                ResourceBar.SetActive(true);

                CharBlickObject.transform.position = Resource.transform.position + Vector3.down * 1.5f + Vector3.left * 0.5f;
                CharBlickObject.transform.SetParent(Resource.transform);
                CharBlickObject.GetComponent<SpriteRenderer>().flipY = true;
                CharBlickObject.SetActive(true);
            }
        }//합쳐서 뭐 이펙트 이런거 보여주겟지
        else if(state ==5)
        {
            if (DataManager.instance.IsKorean)
                InfoTexting("강화된 부대는 <color=#00ff00>더많은 식량을 소모</color>합니다");
            else
            {
                InfoTexting("Large Unit <color=#00ff00>consume more food</color>");
            }

            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    GameManager.instance.money[2] = 0;
                    ResourceBar.transform.localPosition = new Vector3(-595, 402, 0);
                    CharBlickObject.transform.position = Resource.transform.position + Vector3.down * 1.7f + Vector3.right * 0.9f;

                    state = 6;                    
                }
            }
            else if (Input.GetMouseButtonDown(0))
            {
                GameManager.instance.money[2] = 0;
                ResourceBar.transform.localPosition = new Vector3(-595, 402, 0);
                CharBlickObject.transform.position = Resource.transform.position + Vector3.down * 1.7f + Vector3.right * 0.9f;


                state = 6;
            }
        }//우리 리소스 바 표시하기        
        else if (state == 6)
        {
            if (DataManager.instance.IsKorean)
                InfoTexting("<color=#00ff00>식량이 없을 경우</color> 가장 병력이 많은 부대의 <color=#00ff00>병력이 줄어</color>듭니다");
            else
            {
                InfoTexting("<color=#00ff00>Without food</color>, the number of Unit will be <color=#00ff00>reduced</color>");
            }

            if (Enemy.GetComponent<Unit>().Amount <2000)
            {
                Enemy.GetComponent<Unit>().Amount = 2000;
                GameManager.instance.money[2] = 920627;

                ResourceBar.SetActive(false);
                CharBlickObject.SetActive(false);
            }

            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    CharBlickObject.transform.position = GameManager.instance.map.CellToWorld(new Vector3Int(17,14,1)) + 0.6f * Vector3.up;
                    CharBlickObject.transform.SetParent(null);
                    CharBlickObject.GetComponent<SpriteRenderer>().flipY = false;
                    CharBlickObject.SetActive(true);
                    ResourceBar.SetActive(false);
                    BlinkObject.SetActive(false);
                    state = 7;
                }
            }
            else if (Input.GetMouseButtonDown(0))
            {
                CharBlickObject.transform.position = GameManager.instance.map.CellToWorld(new Vector3Int(17, 14, 1)) + 0.6f * Vector3.up;
                CharBlickObject.transform.SetParent(null);
                CharBlickObject.GetComponent<SpriteRenderer>().flipY = false;
                CharBlickObject.SetActive(true);
                ResourceBar.SetActive(false);
                BlinkObject.SetActive(false);
                state = 7;
            }
        }//상대방 리소스바 표시
        else if (state == 7)
        {
            GameManager.instance.GetComponent<Click>().enabled = true;

            if (GameManager.instance.money[2] == 0)
                GameManager.instance.money[2] = 920627;
            if (Enemy.GetComponent<Unit>().Amount != 2000)
                Enemy.GetComponent<Unit>().Amount = 2000;

            if (DataManager.instance.IsKorean)
                InfoTexting("목책을 직접 공격하여 부수세요");
            else
            {
                InfoTexting("Attack Barricade and break it down");
            }
            if (GameManager.instance.Unit1[0].GetComponent<Unit>().Goal == new Vector3Int(17,14,1))
            {
                GameManager.instance.GetComponent<Click>().ClickedChars.RemoveRange(0, GameManager.instance.GetComponent<Click>().ClickedChars.Count);
                CharBlickObject.SetActive(false);
                state = 8;
            }

            if (DragPrefab_temp == null)
            {
                DragPrefab_temp = Instantiate(DragPrefab, GameManager.instance.Unit1[0].transform.position, Quaternion.identity);
                DragPrefab_temp.GetComponent<DragObject>().vec3 = new Vector3Int(17, 14, 1);
            }            
        }
        else if (state == 8)
        {
            GameManager.instance.GetComponent<Click>().enabled = false;


            if (barricade.GetComponent<TreeAndBush>().HP <= 0)
            {
                if (DataManager.instance.IsKorean)
                    InfoTexting("잘하셨습니다");
                else
                {
                    InfoTexting("Good");
                }
                GameManager.instance.Unit1[0].GetComponent<Unit>().IsStand = true;

                BlinkObject.SetActive(true);
                state = 9;
            }            
        }
        else if (state == 9)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    BlinkObject.SetActive(true);

                    if (DataManager.instance.IsKorean)
                        InfoTexting("가속버튼을 누르면 게임진행이 빨라집니다");
                    else
                    {
                        InfoTexting("If you press the accelerate button, game faster");
                    }

                    Gamespeed.GetComponent<Button>().enabled = true;
                    CharBlickObject.transform.position = Gamespeed.transform.position + Vector3.down;
                    CharBlickObject.transform.SetParent(Gamespeed.transform);
                    CharBlickObject.GetComponent<SpriteRenderer>().flipY = true;
                    CharBlickObject.SetActive(true);


                    state = 10;
                }
            }
            else if (Input.GetMouseButtonDown(0))
            {
                BlinkObject.SetActive(true);
                if (DataManager.instance.IsKorean)
                    InfoTexting("가속버튼을 누르면 게임진행이 빨라집니다");
                else
                {
                    InfoTexting("If you press the accelerate button, game faster");
                }

                Gamespeed.GetComponent<Button>().enabled = true;
                CharBlickObject.transform.position = Gamespeed.transform.position + Vector3.down;
                CharBlickObject.transform.SetParent(Gamespeed.transform);
                CharBlickObject.GetComponent<SpriteRenderer>().flipY = true;
                CharBlickObject.SetActive(true);

                state = 10;
            }
        }
        else if (state == 10)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    if (DataManager.instance.IsKorean)
                        InfoTexting("가속버튼을 눌러 게임 진행을 빠르게하세요");
                    else
                    {
                        InfoTexting("Press the accelerator button to speed up the game");
                    }
                    state = 11;
                }
            }
            else if (Input.GetMouseButtonDown(0))
            {
                if (DataManager.instance.IsKorean)
                    InfoTexting("가속버튼을 눌러 게임 진행을 빠르게하세요");
                else
                {
                    InfoTexting("Press the accelerator button to speed up the game");
                }

                state = 11;
            }
        }
        else if (state == 11)
        {
            BlinkObject.SetActive(false);

            if (GamespeedButtonPress)
            {
                if (DataManager.instance.IsKorean)
                    InfoTexting("적을 공격하여 승리하세요");
                else
                {
                    InfoTexting("Attack the enemy and win");
                }
                if (DragPrefab_temp == null)
                {
                    DragPrefab_temp = Instantiate(DragPrefab, GameManager.instance.Unit1[0].transform.position, Quaternion.identity);
                    DragPrefab_temp.GetComponent<DragObject>().vec3 = new Vector3Int(40, 11, 1);
                }

                if(GameManager.instance.Unit1[0].GetComponent<Unit>().Goal == new Vector3Int(40,11,1))
                {
                    state = 12;
                }
            }
        }
        else if (state == 12)
        {
            if (DataManager.instance.IsKorean)
                InfoTexting("가고있습니다");
            else
            {
                InfoTexting("On my way");
            }
            GameManager.instance.GetComponent<Click>().enabled = false;

            if (GameManager.instance.VictoryPanel.activeInHierarchy)
            {
                state = 13;
            }
        }
        else if(state.Equals(13))
        {
            BlinkObject.SetActive(true);

            if (DataManager.instance.IsKorean)
            {
                InfoTexting("승리하였습니다!");
                DataManager.instance.StageClear[0] = true;
            }
            else
            {
                InfoTexting("Congratulation! You Win");
                DataManager.instance.StageClear[0] = true;
            }

            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {                    
                    state = 14;
                }
            }
            else if (Input.GetMouseButtonDown(0))
            {                
                state = 14;
            }
        }
        else if (state.Equals(14))
        {
            if (DataManager.instance.IsKorean)
            {
                InfoTexting("튜토리얼이 끝났습니다");
                DataManager.instance.StageClear[0] = true;
            }
            else
            {
                InfoTexting("Tutorial is over");
                DataManager.instance.StageClear[0] = true;
            }
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    state = 15;
                }
            }
            else if (Input.GetMouseButtonDown(0))
            {
                state = 15;
            }
        }
        else if (state.Equals(15))
        {
            if (DataManager.instance.IsKorean)
            {
                InfoTexting("발렌토스 대륙으로 떠납니다!");
                DataManager.instance.StageClear[0] = true;
            }
            else
            {
                InfoTexting("Go to Valentos Land!");
                DataManager.instance.StageClear[0] = true;
            }
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    state = 16;
                }
            }
            else if (Input.GetMouseButtonDown(0))
            {
                state = 16;
            }
        }
        else if (state.Equals(16))
        {
            WinTutorial();
            GameManager.instance.GetComponent<Click>().StageOK();
        }
    }


    public void GamespeedButton()
    {
        GamespeedButtonPress = true;
        GameManager.instance.GetComponent<Click>().enabled = true;
        Gamespeed.GetComponent<Button>().enabled = false;
        CharBlickObject.SetActive(false);
    }


    public void WinTutorial() //스테이지 1 열리도록                            
    {
        DataManager.instance.StageClear[0] = true;
    }


    public void InfoTexting(string _str)
    {
        Color color = new Color(1f, 1f, 1f, 0.5f);
        Info.GetComponent<Image>().color = color;
        Info.SetActive(true);
        Info.GetComponentInChildren<Text>().text = _str;
    }


    public void RestartTuto2()
    {
        AsyncOperation oper = new AsyncOperation();
        oper = SceneManager.LoadSceneAsync("Stage0_1");
        oper.allowSceneActivation = true;
    }

}
