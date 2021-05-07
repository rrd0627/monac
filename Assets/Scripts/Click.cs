using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Click : MonoBehaviour
{
    //카메라이동 케릭터 선택
    public List<GameObject> ClickedChars;
    private float DoubleClicktimer;
    private float DoubleClicktimer_temp;
    public bool IsDoubleClick;

    private Vector2 pos_down;

    private Vector2 pos_up;

    private Collider2D[] _col;

    public GameObject GameStartButton;

    public BoxCollider2D Boundary;

    public List<GameObject> SelectPrefab;

    public GameObject[] DragDrawing; //0 움직임 , 1 원군,2 싸움

    public Vector3Int SelectedTile;
    private Vector3Int SelectedTile_temp;

    private Vector2 pos;
    private RaycastHit2D[] hit;

    public bool IsMove;
    public bool IsMoveOK;
    public bool IsObstacleMove;
    public bool IsStand;
    public bool IsStandPush;
    private bool IsChase;
    private bool IsBridge;
    private bool IsObstacle;
    private bool bool_temp;

    public bool IsBarricateClicked;
    public bool IsBridgeClicked;

    private bool IsFast;

    private bool IsGameStart;
    private bool IsPressGoBack;


    public GameObject ObstaclePrefab;
    public GameObject BridgePrefab;
    private GameObject TileSelectDotPrefab;
    private GameObject TileSelectPrefab;
    private GameObject EnemyCircle;


    private Vector3 cameraPos;
    private GameObject ClickedUnit;
    private Vector3Int BridgePos;

    private Vector3 ClickAdjustPos = new Vector3(0, 170, 0);

    public GameObject StandMoveIcon;

    public GameObject GameSpeedIcon;

    public GameObject GradientPanel;

    private Vector2 Obstacle_AdjustDist;

    public GameObject BridgeButton;
    public GameObject BarricateButton;

    public GameObject ReadyText;
    public GameObject StartText;


    private const float TouchSize = 0.7f;
    private const float SelectTouchSize = 0.3f;


    public Text hometitletext;
    public Text hometext;
    public Text Stagetitletext;
    public Text Stagetext;
    public Text RestartTitletext;
    public Text Restarttext;

    private bool IsButtonPress;

    private Color _color_;

    public GameObject BlackPanel;

    private float GameSpeed;

    public GameObject MousePointer;

    // Start is called before the first frame update
    void Start()
    {        
        if (DataManager.instance.SelectedChar == 1)
        {
            _color_ = DataManager.instance.color1;
        }
        else if (DataManager.instance.SelectedChar == 2)
        {
            _color_ = DataManager.instance.color2;
        }
        else if (DataManager.instance.SelectedChar == 3)
        {
            _color_ = DataManager.instance.color3;
        }
        else
        {
            _color_ = DataManager.instance.color4;
        }        

        IsButtonPress = false;

        IsMove = false;
        IsMoveOK = false;
        IsObstacleMove = false;
        IsStand = false;
        IsStandPush = false;
        IsChase = false;
        IsBridge = false;
        IsObstacle = false;
        bool_temp = false;
        IsFast = false;
        hit = new RaycastHit2D[2];
        DoubleClicktimer = 0.4f;
        IsDoubleClick = false;
        SelectPrefab = new List<GameObject>();
        IsGameStart = false;
        IsPressGoBack = false;

        GameSpeed = 1.5f;

        IsBarricateClicked = false;
        IsBridgeClicked = false;

        ObstaclePrefab = Instantiate(GameManager.instance.Obstacle_prefab, new Vector2(0, 0), Quaternion.identity);
        ObstaclePrefab.SetActive(false);
        ObstaclePrefab.GetComponentInChildren<Button>().onClick.AddListener(ObstacleMake);
        BridgePrefab = Instantiate(GameManager.instance.Bridge_prefab, new Vector2(0, 0), Quaternion.identity);
        BridgePrefab.SetActive(false);
        BridgePrefab.GetComponentInChildren<Button>().onClick.AddListener(BridgeMake);
        TileSelectDotPrefab = Instantiate(GameManager.instance.TileClickDotPrefab, new Vector2(0, 0), Quaternion.identity);
        TileSelectDotPrefab.SetActive(false);
        EnemyCircle = Instantiate(GameManager.instance.prefabs[1], new Vector2(0, 0), Quaternion.identity);
        EnemyCircle.transform.localScale *= 0.6f;
        EnemyCircle.SetActive(false);

        //BarricateButton.SetActive(false);
        //BridgeButton.SetActive(false);
        //StandMoveIcon.SetActive(false);        

        BarricateButton.GetComponent<Button>().enabled = false;
        BridgeButton.GetComponent<Button>().enabled = false;
        StandMoveIcon.GetComponent<Button>().enabled = false;

        if(DataManager.instance.IsKorean)
        {
            hometitletext.text = TextScript.instance.str_kor[0];
            hometitletext.font = Resources.Load<Font>("Font/Recipe");
            hometext.text = TextScript.instance.str_kor[1];
            hometext.font = Resources.Load<Font>("Font/Recipe");
            Stagetitletext.text = TextScript.instance.str_kor[2];
            Stagetitletext.font = Resources.Load<Font>("Font/Recipe");
            Stagetext.text = TextScript.instance.str_kor[3];
            Stagetext.font = Resources.Load<Font>("Font/Recipe");
            RestartTitletext.text = TextScript.instance.str_kor[4];
            RestartTitletext.font = Resources.Load<Font>("Font/Recipe");
            Restarttext.text = TextScript.instance.str_kor[5];
            Restarttext.font = Resources.Load<Font>("Font/Recipe");
        }
        else
        {
            hometitletext.text = TextScript.instance.str_Eng[0];
            hometitletext.font = Resources.Load<Font>("Font/Changa-Bold");
            hometext.text = TextScript.instance.str_Eng[1];
            hometext.font = Resources.Load<Font>("Font/Changa-Bold");
            Stagetitletext.text = TextScript.instance.str_Eng[2];
            Stagetitletext.font = Resources.Load<Font>("Font/Changa-Bold");
            Stagetext.text = TextScript.instance.str_Eng[3];
            Stagetext.font = Resources.Load<Font>("Font/Changa-Bold");
            RestartTitletext.text = TextScript.instance.str_Eng[4];
            RestartTitletext.font = Resources.Load<Font>("Font/Changa-Bold");
            Restarttext.text = TextScript.instance.str_Eng[5];
            Restarttext.font = Resources.Load<Font>("Font/Changa-Bold");
        }
    }
    public void MoveOrder()
    {
        IsChase = false;
        IsBridge = false;
        IsObstacle = false;
        IsMove = false;

        for (int j=0;j<ClickedChars.Count;j++)
        {
            ClickedChars[j].GetComponent<Unit>().IsBarrigate = false;
            ClickedChars[j].GetComponent<Unit>().IsStand = IsStand;            
            ClickedChars[j].GetComponent<Unit>().IsChase = false;
            ClickedChars[j].GetComponent<Unit>().IsBridge = false;
            ClickedChars[j].GetComponent<Unit>().GoOrder(SelectedTile);
        }
        if(IsStand)
        {
            StandOrder();
        }
        
        ClickedChars.RemoveRange(0, ClickedChars.Count);
        for (int i = SelectPrefab.Count - 1; i >= 0; i--)
        {
            Destroy(SelectPrefab[i]);
        }
    }
    public void StandOrder()
    {
        SoundManager.instance.Play(2);

        IsStandPush = true;

        IsStand = !IsStand;

        if (IsStand)
        {
            StandMoveIcon.GetComponent<Image>().color = Color.gray;
            StandMoveIcon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/WaitPressed");
        }
        else
        {
            StandMoveIcon.GetComponent<Image>().color = Color.white;
            StandMoveIcon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/Wait");
        }

        if (IsBarricateClicked)
        {
            IsStandPush = false;
            ObstacleOrder();
        }            
        if (IsBridgeClicked)
        {
            IsStandPush = false;
            BridgeOrder();
        }
            

    }
    public void ChaseOrder()
    {        
        IsMove = false;
        IsBridge = false;
        IsObstacle = false;
        IsChase = false;

        for (int j = 0; j < ClickedChars.Count; j++)
        {
            ClickedChars[j].GetComponent<Unit>().IsBarrigate = false;
            ClickedChars[j].GetComponent<Unit>().IsStand = IsStand;
            ClickedChars[j].GetComponent<Unit>().IsBridge = false;
            ClickedChars[j].GetComponent<Unit>().ChaseOrder(ClickedUnit);
        }
        ClickedChars.RemoveRange(0, ClickedChars.Count);
        for (int i = SelectPrefab.Count - 1; i >= 0; i--)
        {
            Destroy(SelectPrefab[i]);
        }
        if (IsStand)
        {
            StandOrder();
        }
    }
    public void BridgeOrder()
    {
        SoundManager.instance.Play(2);

        if (BridgePrefab.activeSelf || IsBridgeClicked)
        {
            ObstaclePrefab.SetActive(false);
            BridgePrefab.SetActive(false);
            BarricateButton.GetComponentInChildren<Cooltime>().gameObject.GetComponent<Image>().color = Color.white;
            BridgeButton.GetComponentInChildren<Cooltime>().gameObject.GetComponent<Image>().color = Color.white;

            IsObstacle = false;
            IsBridge = false;
            IsBridgeClicked = false;
            TriangleOffEffect();
            for (int j = 0; j < SelectPrefab.Count; j++)
                Destroy(SelectPrefab[j]);
            ClickedChars.RemoveRange(0, ClickedChars.Count);
        }
        else if (GameManager.instance.BridgeAble && ClickedChars.Count>0)
        {
            
            ObstaclePrefab.SetActive(false);
            BridgePrefab.SetActive(true);

            BridgeButton.GetComponentInChildren<Cooltime>().gameObject.GetComponent<Image>().color = Color.gray;
            BarricateButton.GetComponentInChildren<Cooltime>().gameObject.GetComponent<Image>().color = Color.white;

            BridgePrefab.transform.position = GameManager.instance.change.CellToWorld(GameManager.instance.change.WorldToCell(Camera.main.transform.position + Vector3.forward * 50));

            BridgePrefab.GetComponentInChildren<CircleCollider2D>().isTrigger = false;

            IsMove = false;
            IsChase = false;
            IsObstacle = false;
            for (int j = 0; j < ClickedChars.Count; j++)
            {
                ClickedChars[j].GetComponent<Unit>().IsBarrigate = false;
                ClickedChars[j].GetComponent<Unit>().IsStand = IsStand;
                ClickedChars[j].GetComponent<Unit>().IsChase = false;
            }
            IsBridge = true;
            bool_temp = true;
        }
        else if (GameManager.instance.BridgeAble)
        {
            SoundManager.instance.Play(2);

            BridgeButton.GetComponentInChildren<Cooltime>().gameObject.GetComponent<Image>().color = Color.gray;
            BarricateButton.GetComponentInChildren<Cooltime>().gameObject.GetComponent<Image>().color = Color.white;
            TriangleEffect();

            GameManager.instance.InfoTexting("캐릭터를 선택해 주세요.");

            Time.timeScale = 0;
            GradientPanel.SetActive(true);

            IsBarricateClicked = false;
            IsBridgeClicked = true;

            IsMove = false;
            IsChase = false;
            IsBridge = false;
            IsObstacle = false;
            bool_temp = true;
        }
        else
        {
            SoundManager.instance.Play(7);

            GameManager.instance.InfoTexting("건설 할 수 없습니다.");
        }
    }
    public void BridgeMake()
    {
        SoundManager.instance.Play(2);
        if (GameManager.instance.BridgeAble)
        {
            BridgePrefab.SetActive(false);
            BridgeButton.GetComponentInChildren<Cooltime>().gameObject.GetComponent<Image>().color = Color.white;

            IsBridge = false;
            bool_temp = false;

            IsMove = false;
            IsChase = false;
            IsObstacle = false;
            for (int j = 0; j < ClickedChars.Count; j++)
            {
                ClickedChars[j].GetComponent<Unit>().IsBridge = true;
                ClickedChars[j].GetComponent<Unit>().IsStand = IsStand;
                ClickedChars[j].GetComponent<Unit>().IsChase = false;

                ClickedChars[j].GetComponent<Unit>().GoBridgeOrder(BridgePos);
            }
            ClickedChars.RemoveRange(0, ClickedChars.Count);
            for (int i = SelectPrefab.Count - 1; i >= 0; i--)
            {
                Destroy(SelectPrefab[i]);
            }
        }
        if (IsStand)
        {
            StandOrder();
        }
    }
    public void ObstacleOrder()
    {
        SoundManager.instance.Play(2);

        if (ObstaclePrefab.activeSelf|| IsBarricateClicked)
        {
            ObstaclePrefab.SetActive(false);
            BridgePrefab.SetActive(false);
            BarricateButton.GetComponentInChildren<Cooltime>().gameObject.GetComponent<Image>().color = Color.white;
            BridgeButton.GetComponentInChildren<Cooltime>().gameObject.GetComponent<Image>().color = Color.white;

            IsObstacle = false;
            IsBridge = false;
            IsBarricateClicked = false;
            TriangleOffEffect();
            for (int j = 0; j < SelectPrefab.Count; j++)
                Destroy(SelectPrefab[j]);
            ClickedChars.RemoveRange(0, ClickedChars.Count);
        }
        else if (GameManager.instance.ObstacleAble && ClickedChars.Count > 0)
        {
            BridgePrefab.SetActive(false);

            ObstaclePrefab.SetActive(true);

            BridgeButton.GetComponentInChildren<Cooltime>().gameObject.GetComponent<Image>().color = Color.white;
            BarricateButton.GetComponentInChildren<Cooltime>().gameObject.GetComponent<Image>().color = Color.gray;



            ObstaclePrefab.transform.position = GameManager.instance.change.CellToWorld(GameManager.instance.change.WorldToCell(Camera.main.transform.position + Vector3.forward *50));

            ObstaclePrefab.GetComponentInChildren<CircleCollider2D>().isTrigger = false;

            IsMove = false;
            IsChase = false;
            IsBridge = false;
            for (int j = 0; j < ClickedChars.Count; j++)
            {
                ClickedChars[j].GetComponent<Unit>().IsBarrigate = false;
                ClickedChars[j].GetComponent<Unit>().IsStand = IsStand;
                ClickedChars[j].GetComponent<Unit>().IsChase = false;
            }
            IsObstacle = true;
            bool_temp = true;
        }
        else if(GameManager.instance.ObstacleAble)
        {
            SoundManager.instance.Play(2);
            TriangleEffect();
            BridgeButton.GetComponentInChildren<Cooltime>().gameObject.GetComponent<Image>().color = Color.white;
            BarricateButton.GetComponentInChildren<Cooltime>().gameObject.GetComponent<Image>().color = Color.gray;

            GameManager.instance.InfoTexting("캐릭터를 선택해 주세요.");

            Time.timeScale = 0;
            GradientPanel.SetActive(true);

            IsBarricateClicked = true;
            IsBridgeClicked = false;
            IsMove = false;
            IsChase = false;
            IsBridge = false;
            IsObstacle = false;
            bool_temp = true;
        }
        else
        {
            SoundManager.instance.Play(7);

            GameManager.instance.InfoTexting("건설 할 수 없습니다.");
        }
    }
    public void ObstacleMake()
    {
        SoundManager.instance.Play(2);


        if (GameManager.instance.ObstacleAble)
        {
            ObstaclePrefab.SetActive(false);
            BarricateButton.GetComponentInChildren<Cooltime>().gameObject.GetComponent<Image>().color = Color.white;

            IsObstacle = false;
            bool_temp = false;

            IsMove = false;
            IsChase = false;
            IsBridge = false;
            
            for (int j = 0; j < ClickedChars.Count; j++)
            {
                ClickedChars[j].GetComponent<Unit>().IsBarrigate = true;
                ClickedChars[j].GetComponent<Unit>().IsStand = IsStand;
                ClickedChars[j].GetComponent<Unit>().IsChase = false;
                ClickedChars[j].GetComponent<Unit>().ObstacleOrder(SelectedTile);
            }
            ClickedChars.RemoveRange(0, ClickedChars.Count);
        
            for (int i = SelectPrefab.Count - 1; i >= 0; i--)
            {
                Destroy(SelectPrefab[i]);
            }
        }
        if (IsStand)
        {
            StandOrder();
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if (!IsGameStart || IsPressGoBack) return; //일시정지 또는 게임시작전에 실행 불가

        //화살표 없애기
        for (int i = SelectPrefab.Count-1; i >= 0; i--)
        {
            if (SelectPrefab[i] == null)
                SelectPrefab.RemoveAt(i);
        }

        //선택된 캐릭터가 있을때
        if (ClickedChars.Count!=0) 
        {
            for (int j = 0; j < ClickedChars.Count; j++) 
            {
                if(ClickedChars[j]==null)//선택된 캐릭터 멤버중 없어진 아이가 있을경우
                {
                    ClickedChars.RemoveAt(j);
                    j--;
                    continue;
                }

                if (ClickedChars[j].GetComponent<Unit>().Enemy.Count != 0) //유닛 attack 시에
                {
                    for (int i = 0; i < ClickedChars[j].GetComponent<Unit>().Enemy.Count; i++)
                    {
                        if (ClickedChars[j].GetComponent<Unit>().Enemy[i] != null)
                        {
                            if (ClickedChars[j].GetComponent<Unit>().Enemy[i].tag == "Unit") //유닛과 싸울시에
                            {
                                ClickedChars[j].GetComponent<LineRenderer>().enabled = false;
                                Destroy(SelectPrefab[j]);
                                ClickedChars.RemoveAt(j);
                                j--;
                                break;
                            }
                        }
                    }
                }
            }
            //Time.timeScale = 0.1f;
            Time.timeScale = 0f;
            GradientPanel.SetActive(true);
            //BridgeButton.SetActive(true);
            //BarricateButton.SetActive(true);
        }
        //선택된 캐릭터가 없을경우
        else
        {
            //BridgeButton.SetActive(false);
            //BarricateButton.SetActive(false);

            ObstaclePrefab.SetActive(false);
            BridgePrefab.SetActive(false);
            
            if(!IsBarricateClicked)
            {
                BarricateButton.GetComponentInChildren<Cooltime>().gameObject.GetComponent<Image>().color = Color.white;
            }
            if (!IsBridgeClicked)
            {
                BridgeButton.GetComponentInChildren<Cooltime>().gameObject.GetComponent<Image>().color = Color.white;
            }


            if (!IsBarricateClicked && !IsBridgeClicked)
            {
                GradientPanel.SetActive(false);

                if (IsFast)
                    Time.timeScale = GameSpeed;
                else
                    Time.timeScale = 1f;
            }            
        }

        //클릭하고 더블클릭 시간
        if (IsDoubleClick)
        {
            DoubleClicktimer_temp += Time.fixedDeltaTime;

            if(DoubleClicktimer < DoubleClicktimer_temp)
            {
                IsDoubleClick = false;
                DoubleClicktimer_temp = 0;
            }
        }
        
        //터치
        if(Input.touchCount>0)
        {
            Touch touch = Input.GetTouch(0);
            //터치 시작
            if (touch.phase == TouchPhase.Began)
            {
                if (RaycastWorldUI())
                {
                    return;
                }
                pos_down = Camera.main.ViewportToWorldPoint(touch.position);

                //장애물 설치하려고 장애물 등장했을때
                if (IsObstacle || IsBridge)
                {
                    ObstaclePrefab.GetComponentInChildren<CircleCollider2D>().isTrigger = true;
                    BridgePrefab.GetComponentInChildren<CircleCollider2D>().isTrigger = true;

                    pos = Camera.main.ScreenToWorldPoint(touch.position);
                    _col = Physics2D.OverlapCircleAll(pos, 0);

                    for (int i = 0; i < _col.Length; i++)
                    {
                        if (_col[i].CompareTag("InstallReady"))
                        {
                            IsObstacleMove = true;
                            GameManager.instance.GetComponent<FingersScript>().enabled = false;

                            Obstacle_AdjustDist = (Vector2)_col[i].transform.position - pos;
                            break;
                        }
                    }
                    bool_temp = false;
                }
                else
                {
                    int index = -1;
                    float dist = 99999999;
                    pos = Camera.main.ScreenToWorldPoint(touch.position);
                    if (ClickedChars.Count > 0)
                        _col = Physics2D.OverlapCircleAll(pos, SelectTouchSize);
                    else
                        _col = Physics2D.OverlapCircleAll(pos, TouchSize);
                    for (int i = 0; i < _col.Length; i++)
                    {
                        if (_col[i].CompareTag("Unit"))
                        {
                            if (ClickedChars.Count > 0 && ClickedChars.Contains(_col[i].gameObject)) //캐릭터 선택되어있고 그 캐릭터 누를때
                            {
                                index = i;
                                break;
                            }

                            if (_col[i].GetComponent<Unit>().Master == 1)
                            {
                                if (dist > Vector2.Distance(_col[i].transform.position, pos))
                                {
                                    index = i;
                                    dist = Vector2.Distance(_col[i].transform.position, pos);
                                }
                            }
                        }
                    }

                    //움직일때       캐릭터가 선택되어있고   선택되어있는 캐릭터를 다시 클릭 할때
                    if (index != -1 && ClickedChars.Count > 0 && ClickedChars.Contains(_col[index].gameObject))
                    {
                        IsMove = true;

                        SelectedTile = ClickedChars[0].GetComponent<Unit>().Cur;

                        TileSelectDotPrefab.transform.position = Vector3.forward * 100;
                        TileSelectDotPrefab.SetActive(true);

                        GameManager.instance.GetComponent<FingersScript>().enabled = false;

                        for (int i = 0; i < ClickedChars.Count; i++)
                            ClickedChars[i].GetComponent<LineRenderer>().enabled = true;

                        MousePointer.transform.position = new Vector3(0, 0, -100);
                        MousePointer.SetActive(true);

                        TileSelectPrefab = Instantiate(GameManager.instance.TileClickPrefab, new Vector3(0, 0, 100), Quaternion.identity);
                        EnemyCircle.transform.position = Vector3.forward * 100;
                        EnemyCircle.SetActive(true);
                    }
                    else if (index != -1) //그냥 캐릭터 바로 드래그할때
                    {
                        if (_col[index].GetComponent<Unit>().UnitState == 3)
                        {
                            GameManager.instance.InfoTexting("싸우는 대상은 선택할 수 없습니다.");
                        }
                        else
                        {
                            IsMove = true;

                            TriangleOffEffect();

                            TileSelectDotPrefab.transform.position = Vector3.forward * 100;
                            TileSelectDotPrefab.SetActive(true);

                            GameManager.instance.GetComponent<FingersScript>().enabled = false;

                            ClickedChars.RemoveRange(0, ClickedChars.Count);
                            for (int j = 0; j < SelectPrefab.Count; j++)
                                Destroy(SelectPrefab[j]);

                            ClickedChars.Add(_col[index].gameObject);
                            SelectPrefab.Add(Instantiate(GameManager.instance.prefabs[1], _col[index].gameObject.transform));
                            SelectedTile = ClickedChars[0].GetComponent<Unit>().Cur;
                            for (int i = 0; i < ClickedChars.Count; i++)
                                ClickedChars[i].GetComponent<LineRenderer>().enabled = true;
                            MousePointer.transform.position = new Vector3(0, 0, -100);
                            MousePointer.SetActive(true);
                            TileSelectPrefab = Instantiate(GameManager.instance.TileClickPrefab, new Vector3(0, 0, 100), Quaternion.identity);
                            EnemyCircle.transform.position = Vector3.forward * 100;
                            EnemyCircle.SetActive(true);
                        }
                    }
                }
            }
            //터치 중
            if (IsMove && touch.phase == TouchPhase.Moved)
            {
                //클릭여러번 그냥 하는것 제외함  드래그 할때만 의미있도록
                if (Vector2.Distance(pos_down, Camera.main.ViewportToWorldPoint(Input.mousePosition)) < 500) return;

                IsMoveOK = true;
                IsBarricateClicked = false;
                IsBridgeClicked = false;
                SelectedTile_temp = ClickTile();

                if (GameManager.instance.map.GetTile(SelectedTile_temp) != null && GameManager.instance.map.GetTile(SelectedTile_temp).name != "Pillar")
                {
                    SelectedTile = SelectedTile_temp;
                }

                TileSelectDotPrefab.transform.position = GameManager.instance.map.CellToWorld(SelectedTile) + 0.2f * Vector3.up;

                TileSelectPrefab.transform.position = GameManager.instance.map.CellToWorld(SelectedTile) + 0.7f * Vector3.up;


                cameraPos = Camera.main.transform.position;
                //클릭동안 카메라 움직임
                //위로
                if (Camera.main.ScreenToViewportPoint(touch.position).y > 0.8f)
                {
                    cameraPos.y = Mathf.Clamp(cameraPos.y + 0.1f, Boundary.bounds.min.y, Boundary.bounds.max.y);
                    Camera.main.transform.position = cameraPos;
                }
                //아래
                if (Camera.main.ScreenToViewportPoint(touch.position).y < 0.1f)
                {
                    cameraPos.y = Mathf.Clamp(cameraPos.y - 0.1f, Boundary.bounds.min.y, Boundary.bounds.max.y);
                    Camera.main.transform.position = cameraPos;
                }
                //오른쪽
                if (Camera.main.ScreenToViewportPoint(touch.position).x > 0.9f)
                {
                    cameraPos.x = Mathf.Clamp(cameraPos.x + 0.1f, Boundary.bounds.min.x, Boundary.bounds.max.x);
                    Camera.main.transform.position = cameraPos;
                }
                //왼쪽
                if (Camera.main.ScreenToViewportPoint(touch.position).x < 0.1f)
                {
                    cameraPos.x = Mathf.Clamp(cameraPos.x - 0.1f, Boundary.bounds.min.x, Boundary.bounds.max.x);
                    Camera.main.transform.position = cameraPos;
                }
                pos = Camera.main.ScreenToWorldPoint(touch.position + (Vector2)ClickAdjustPos);
                _col = Physics2D.OverlapCircleAll(pos, 0);
                int index = -1;
                float dist = 99999999;

                for (int i = 0; i < _col.Length; i++)
                {
                    if (_col[i].CompareTag("Untagged")|| _col[i].CompareTag("Node")) continue;

                    if (dist > Vector2.Distance(_col[i].transform.position, pos))
                    {
                        dist = Vector2.Distance(_col[i].transform.position, pos);
                        index = i;
                    }
                }

                //그 외에는      //그냥 타일에 대면 타일    move
                if (index == -1)
                {
                    TileSelectDotPrefab.SetActive(true);

                    IsChase = false;
                    EnemyCircle.transform.position = Vector3.forward * 100;
                    for (int i = 0; i < ClickedChars.Count; i++)
                    {
                        ClickedChars[i].GetComponent<LineRenderer>().SetPosition(0, ClickedChars[i].transform.position);
                        ClickedChars[i].GetComponent<LineRenderer>().SetPosition(1, pos);
                    }
                    MousePointer.transform.position = pos;

                    if (GameManager.instance.map.GetTile(SelectedTile) != null)
                    { //맵에 클릭했을때
                        TileSelectPrefab.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprite/Go");
                    }
                    else
                    {
                        TileSelectPrefab.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprite/Stop");
                    }
                    TileSelectPrefab.transform.SetParent(null);


                }
                else
                {
                    //캐릭터 위에 대면 캐릭터  chase                
                    if (_col[index].CompareTag("Unit"))
                    {
                        TileSelectDotPrefab.SetActive(false);

                        ClickedUnit = _col[index].gameObject;
                        //우리팀이면
                        if (_col[index].GetComponent<Unit>().Master == 1)
                        {
                            TileSelectPrefab.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprite/Help");
                            TileSelectPrefab.transform.position = _col[index].transform.position + Vector3.up * 1.5f;
                            TileSelectPrefab.transform.SetParent(_col[index].transform);
                        }
                        else
                        {
                            TileSelectPrefab.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprite/Fight");
                            TileSelectPrefab.transform.position = _col[index].transform.position + Vector3.up * 1.5f;
                            TileSelectPrefab.transform.SetParent(_col[index].transform);

                        }
                        IsChase = true;

                        for (int i = 0; i < ClickedChars.Count; i++)
                        {
                            ClickedChars[i].GetComponent<LineRenderer>().SetPosition(0, ClickedChars[i].transform.position);
                            ClickedChars[i].GetComponent<LineRenderer>().SetPosition(1, ClickedUnit.transform.position + 0.3f * Vector3.up);
                        }
                        MousePointer.transform.position = pos;

                        EnemyCircle.transform.position = ClickedUnit.transform.position + Vector3.up * 0.3f;
                        EnemyCircle.transform.SetParent(ClickedUnit.transform);
                    }
                    //장애물 위에 대면 장애물  장애물 부수기
                    else if (_col[index].CompareTag("TreeAndBush"))
                    {
                        IsChase = false;
                        TileSelectDotPrefab.SetActive(false);

                        for (int i = 0; i < ClickedChars.Count; i++)
                        {
                            ClickedChars[i].GetComponent<LineRenderer>().SetPosition(0, ClickedChars[i].transform.position);
                            ClickedChars[i].GetComponent<LineRenderer>().SetPosition(1, _col[index].transform.position + 0.3f * Vector3.up);
                        }
                        MousePointer.transform.position = pos;

                        TileSelectPrefab.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprite/Fight");
                        TileSelectPrefab.transform.position = _col[index].transform.position + Vector3.up * 1.5f;

                    }
                    //배러같은거 누르는 경우
                    else
                    {
                        TileSelectDotPrefab.SetActive(true);

                        IsChase = false;
                        EnemyCircle.transform.position = Vector3.forward * 100;
                        TileSelectPrefab.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprite/Go");

                        for (int i = 0; i < ClickedChars.Count; i++)
                        {
                            ClickedChars[i].GetComponent<LineRenderer>().SetPosition(0, ClickedChars[i].transform.position);
                            ClickedChars[i].GetComponent<LineRenderer>().SetPosition(1, pos);
                        }
                        MousePointer.transform.position = pos;

                    }
                }
            }

            if (IsObstacleMove && touch.phase == TouchPhase.Moved)
            {
                if (Vector2.Distance(pos_down, Camera.main.ViewportToWorldPoint(touch.position)) < 500) return;

                SelectedTile = ClickTile();

                if (IsObstacle)
                {
                    if (GameManager.instance.map.GetTile(SelectedTile) == null || GameManager.instance.TileMaster[GameManager.instance.mapsize / 2 + SelectedTile.x, GameManager.instance.mapsize / 2 + SelectedTile.y] > 0)
                    {
                        ObstaclePrefab.GetComponentInChildren<Button>().gameObject.GetComponent<Image>().color = Color.gray * 1.5f;
                        ObstaclePrefab.GetComponentInChildren<Button>().enabled = false;
                        ObstaclePrefab.GetComponentInChildren<CircleCollider2D>().gameObject.GetComponent<SpriteRenderer>().color = Color.red / 2f;
                    }
                    else
                    {
                        ObstaclePrefab.GetComponentInChildren<Button>().gameObject.GetComponent<Image>().color = Color.white;
                        ObstaclePrefab.GetComponentInChildren<Button>().enabled = true;
                        ObstaclePrefab.GetComponentInChildren<CircleCollider2D>().gameObject.GetComponent<SpriteRenderer>().color = Color.green / 2f;
                    }

                    ObstaclePrefab.transform.position = GameManager.instance.change.CellToWorld(SelectedTile) + Vector3.forward * 1.9f;
                    ObstaclePrefab.GetComponentInChildren<CircleCollider2D>().isTrigger = false;
                }
                else if (IsBridge)
                {
                    BridgePos = ClickWaterTile();

                    if (IsnextmapExist(BridgePos) && !IsMapHere(BridgePos))
                    {
                        BridgePrefab.GetComponentInChildren<Button>().gameObject.GetComponent<Image>().color = Color.white;
                        BridgePrefab.GetComponentInChildren<Button>().enabled = true;
                        BridgePrefab.GetComponentInChildren<CircleCollider2D>().gameObject.GetComponent<SpriteRenderer>().color = Color.green / 2f;
                    }
                    else
                    {
                        BridgePrefab.GetComponentInChildren<Button>().gameObject.GetComponent<Image>().color = Color.gray * 1.5f;
                        BridgePrefab.GetComponentInChildren<Button>().enabled = false;
                        BridgePrefab.GetComponentInChildren<CircleCollider2D>().gameObject.GetComponent<SpriteRenderer>().color = Color.red / 2f;
                    }
                    if (BridgePos == new Vector3Int(0, 0, -1))
                        BridgePrefab.transform.position = GameManager.instance.change.CellToWorld(SelectedTile) + Vector3.down * 0.15f + Vector3.forward * 1.5f;
                    else
                        BridgePrefab.transform.position = GameManager.instance.change.CellToWorld(BridgePos) + Vector3.down * 0.13f - Vector3.forward * 0.4f;

                    BridgePrefab.GetComponentInChildren<CircleCollider2D>().isTrigger = false;

                }

                cameraPos = Camera.main.transform.position;
                //클릭동안 카메라 움직임
                //위로
                if (Camera.main.ScreenToViewportPoint(touch.position).y > 0.8f)
                {
                    cameraPos.y = Mathf.Clamp(cameraPos.y + 0.1f, Boundary.bounds.min.y, Boundary.bounds.max.y);
                    Camera.main.transform.position = cameraPos;
                }
                //아래
                if (Camera.main.ScreenToViewportPoint(touch.position).y < 0.1f)
                {
                    cameraPos.y = Mathf.Clamp(cameraPos.y - 0.1f, Boundary.bounds.min.y, Boundary.bounds.max.y);
                    Camera.main.transform.position = cameraPos;
                }
                //오른쪽
                if (Camera.main.ScreenToViewportPoint(touch.position).x > 0.9f)
                {
                    cameraPos.x = Mathf.Clamp(cameraPos.x + 0.1f, Boundary.bounds.min.x, Boundary.bounds.max.x);
                    Camera.main.transform.position = cameraPos;
                }
                //왼쪽
                if (Camera.main.ScreenToViewportPoint(touch.position).x < 0.1f)
                {
                    cameraPos.x = Mathf.Clamp(cameraPos.x - 0.1f, Boundary.bounds.min.x, Boundary.bounds.max.x);
                    Camera.main.transform.position = cameraPos;
                }
            }

            //터치 끝
            if (touch.phase == TouchPhase.Ended)
            {
                ObstaclePrefab.GetComponentInChildren<CircleCollider2D>().isTrigger = true;
                BridgePrefab.GetComponentInChildren<CircleCollider2D>().isTrigger = true;

                pos_up = Camera.main.ViewportToWorldPoint(touch.position);
                IsMove = false;
                EnemyCircle.SetActive(false);

                //제대로 드래그 한 경우
                if (IsMoveOK)
                {
                    TileSelectPrefab.GetComponent<DestroySoon>().DestroyObject();
                    for (int i = 0; i < ClickedChars.Count; i++)
                    {
                        ClickedChars[i].GetComponent<LineRenderer>().SetPosition(0, ClickedChars[i].transform.position);
                        ClickedChars[i].GetComponent<LineRenderer>().SetPosition(1, ClickedChars[i].transform.position);
                        ClickedChars[i].GetComponent<LineRenderer>().enabled = false;
                    }
                    MousePointer.SetActive(false);

                    IsMoveOK = false;
                    //캐릭터위에서 땐 경우 Chase
                    if (IsChase)
                    {
                        ChaseOrder();
                        IsChase = false;
                    }
                    //그 외의 경우 Move
                    else
                    {
                        MoveOrder();
                    }

                    GameManager.instance.GetComponent<FingersScript>().enabled = true;
                    TileSelectDotPrefab.SetActive(false);

                    pos = Camera.main.ScreenToWorldPoint(touch.position + (Vector2)ClickAdjustPos);
                    _col = Physics2D.OverlapCircleAll(pos, 0);
                    for (int i = 0; i < _col.Length; i++)
                    {
                        if (_col[i].CompareTag("TreeAndBush"))
                        {
                            for (int j = 0; j < ClickedChars.Count; j++)
                                ClickedChars[j].GetComponent<Unit>().BarrigateClicked = _col[i].gameObject;
                            break;
                        }
                    }
                    return;
                }
                //장애물 설치하려고 temp나타나있는 경우
                if (IsObstacleMove)
                {
                    GameManager.instance.GetComponent<FingersScript>().enabled = true;
                    IsObstacleMove = false;
                }
                else
                {
                    if (Vector2.Distance(pos_down, pos_up) > 500)
                    {
                        IsDoubleClick = false;
                        return;
                    }
                    //드래그 없이 클릭완료한경우
                    else
                    {
                        if (IsObstacle || IsBridge)
                        {
                            //다른곳 눌러서 Obstacle 취소 할때
                            if (!bool_temp)
                            {
                                ObstaclePrefab.SetActive(false);
                                BridgePrefab.SetActive(false);
                                BarricateButton.GetComponentInChildren<Cooltime>().gameObject.GetComponent<Image>().color = Color.white;
                                BridgeButton.GetComponentInChildren<Cooltime>().gameObject.GetComponent<Image>().color = Color.white;
                                
                                IsObstacle = false;
                                IsBridge = false;
                                for (int j = 0; j < SelectPrefab.Count; j++)
                                    Destroy(SelectPrefab[j]);
                                ClickedChars.RemoveRange(0, ClickedChars.Count);
                            }

                        }
                        else if (IsStandPush)
                            IsStandPush = false;
                        else
                        {
                            pos_up = Camera.main.ScreenToWorldPoint(touch.position);
                            _col = Physics2D.OverlapCircleAll(pos_up, 1);
                            int index = -1;
                            float min_dist = 99999999;
                            for (int i = 0; i < _col.Length; i++)
                            {
                                if (_col[i].CompareTag("Unit"))
                                {
                                    if (_col[i].GetComponent<Unit>().Master == 1 && Vector2.Distance(pos_up, _col[i].transform.position) < min_dist)
                                    {
                                        min_dist = Vector2.Distance(pos_up, _col[i].transform.position);
                                        index = i;
                                    }
                                }
                            }
                            if (index != -1) //  뭔가를 찍었을때
                            {
                                if (_col[index].GetComponent<Unit>().UnitState == 3)
                                {
                                    GameManager.instance.InfoTexting("싸우는 대상은 선택할 수 없습니다.");
                                }
                                else
                                {
                                    for (int j = 0; j < SelectPrefab.Count; j++)
                                        Destroy(SelectPrefab[j]);


                                    ClickedChars.RemoveRange(0, ClickedChars.Count);
                                    ClickedChars.Add(_col[index].gameObject);

                                    SelectPrefab.Add(Instantiate(GameManager.instance.prefabs[1], _col[index].gameObject.transform));
                                    ClickedChars[0].GetComponent<Unit>().SelectArrow = SelectPrefab[0];

                                    if (IsBarricateClicked)
                                    {
                                        IsBarricateClicked = false;
                                        TriangleOffEffect();
                                        ObstacleOrder();

                                        IsMove = false;
                                    }
                                    else if (IsBridgeClicked)
                                    {
                                        IsBridgeClicked = false;
                                        TriangleOffEffect();
                                        BridgeOrder();

                                        IsMove = false;
                                    }
                                }
                            }
                            if (index == -1)   // 아무것도 안찍었을때
                            {
                                if (IsBarricateClicked && !RaycastWorldUI())
                                {
                                    IsBarricateClicked = false;
                                    TriangleOffEffect();
                                }
                                if (IsBridgeClicked && !RaycastWorldUI())
                                {
                                    IsBridgeClicked = false;
                                    TriangleOffEffect();
                                }

                                if (ClickedChars.Count != 0)
                                {
                                    for (int j = 0; j < ClickedChars.Count; j++)
                                    {
                                        Destroy(ClickedChars[j].GetComponent<Unit>().SelectArrow);
                                    }
                                    ClickedChars.RemoveRange(0, ClickedChars.Count);
                                    for (int j = 0; j < SelectPrefab.Count; j++)
                                    {
                                        Destroy(SelectPrefab[j]);
                                    }
                                }
                                GameManager.instance.GetComponent<FingersScript>().enabled = true;
                            }
                            else
                            {
                                //한마리만 찍은상태에서   더블클릭하려고 할때
                                if (ClickedChars.Count == 1 && IsDoubleClick && _col[index].gameObject == ClickedChars[0])
                                {
                                    DoubleClicktimer_temp = 0;

                                    ClickedChars = ObjectInCamera(ClickedChars[0].transform.position);
                                    for (int j = 0; j < ClickedChars.Count; j++)
                                    {
                                        Destroy(ClickedChars[j].GetComponent<Unit>().SelectArrow);
                                        SelectPrefab.Add(Instantiate(GameManager.instance.prefabs[1], ClickedChars[j].transform));
                                        ClickedChars[j].GetComponent<Unit>().SelectArrow = SelectPrefab[j];
                                    }
                                    GameManager.instance.GetComponent<FingersScript>().enabled = true;
                                }
                                //한마리만 찍는 상태일때
                                else if (ClickedChars.Count == 1)
                                {
                                    IsDoubleClick = true;
                                    DoubleClicktimer_temp = 0;
                                    GameManager.instance.GetComponent<FingersScript>().enabled = true;
                                }
                            }

                        }
                    }
                }
            }
        }
        //컴퓨터 용
        else
        {
            //테스트용 koki
            if(Input.GetKeyDown(KeyCode.A))
            {
                StandOrder();
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                ObstacleOrder();
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                BridgeOrder();
            }
            if(Input.GetKeyDown(KeyCode.F))
            {
                GameSpeed = 4;
            }

            //마우스 클릭
            if (Input.GetMouseButtonDown(0))
            {
                if(RaycastWorldUI())
                {
                    return;
                }
                pos_down = Camera.main.ViewportToWorldPoint(Input.mousePosition);

                //장애물 설치하려고 장애물 등장했을때
                if (IsObstacle || IsBridge)
                {
                    ObstaclePrefab.GetComponentInChildren<CircleCollider2D>().isTrigger = true;
                    BridgePrefab.GetComponentInChildren<CircleCollider2D>().isTrigger = true;

                    pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    _col = Physics2D.OverlapCircleAll(pos, 0);

                    for (int i = 0; i < _col.Length; i++)
                    {
                        if (_col[i].CompareTag("InstallReady"))
                        {
                            IsObstacleMove = true;
                            GameManager.instance.GetComponent<FingersScript>().enabled = false;

                            Obstacle_AdjustDist = (Vector2)_col[i].transform.position - pos;
                            break;
                        }
                    }
                    bool_temp = false;
                }
                else
                {
                    int index = -1;
                    float dist = 99999999;
                    pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                    if(ClickedChars.Count>0)
                        _col = Physics2D.OverlapCircleAll(pos, SelectTouchSize);
                    else
                        _col = Physics2D.OverlapCircleAll(pos, TouchSize);


                    for (int i = 0; i < _col.Length; i++)
                    {
                        if (_col[i].CompareTag("Unit"))
                        {
                            if(ClickedChars.Count>0 && ClickedChars.Contains(_col[i].gameObject)) //캐릭터 선택되어있고 그 캐릭터 누를때
                            {
                                index = i;
                                break;
                            }

                            if(_col[i].GetComponent<Unit>().Master == 1)
                            {
                                if (dist > Vector2.Distance(_col[i].transform.position, pos))
                                {
                                    index = i;
                                    dist = Vector2.Distance(_col[i].transform.position, pos);
                                }
                            }                            
                        }
                    }

                    //움직일때       캐릭터가 선택되어있고   선택되어있는 캐릭터를 다시 클릭 할때
                    if (index != -1 && ClickedChars.Count > 0 && ClickedChars.Contains(_col[index].gameObject))
                    {
                        IsMove = true;

                        TileSelectDotPrefab.transform.position = Vector3.forward * 100;
                        TileSelectDotPrefab.SetActive(true);
                        SelectedTile = ClickedChars[0].GetComponent<Unit>().Cur;
                        GameManager.instance.GetComponent<FingersScript>().enabled = false;
                        for (int i = 0; i < ClickedChars.Count; i++)
                            ClickedChars[i].GetComponent<LineRenderer>().enabled = true;

                        MousePointer.transform.position = new Vector3(0,0,-100);
                        MousePointer.SetActive(true);


                        TileSelectPrefab = Instantiate(GameManager.instance.TileClickPrefab, new Vector3(0,0,100), Quaternion.identity);
                        EnemyCircle.transform.position = Vector3.forward * 100;
                        EnemyCircle.SetActive(true);
                    }
                    else if(index != -1) //그냥 캐릭터 바로 드래그할때
                    {
                        if (_col[index].GetComponent<Unit>().UnitState == 3)
                        {
                            GameManager.instance.InfoTexting("싸우는 대상은 선택할 수 없습니다.");
                        }
                        else
                        {
                            IsMove = true;

                            TriangleOffEffect();

                            TileSelectDotPrefab.transform.position = Vector3.forward * 100;
                            TileSelectDotPrefab.SetActive(true);

                            GameManager.instance.GetComponent<FingersScript>().enabled = false;

                            ClickedChars.RemoveRange(0, ClickedChars.Count);
                            for (int j = 0; j < SelectPrefab.Count; j++)
                                Destroy(SelectPrefab[j]);

                            ClickedChars.Add(_col[index].gameObject);
                            SelectPrefab.Add(Instantiate(GameManager.instance.prefabs[1], _col[index].gameObject.transform));
                            SelectedTile = ClickedChars[0].GetComponent<Unit>().Cur;
                            for (int i = 0; i < ClickedChars.Count; i++)
                                ClickedChars[i].GetComponent<LineRenderer>().enabled = true;
                            MousePointer.transform.position = new Vector3(0, 0, -100);
                            MousePointer.SetActive(true);

                            TileSelectPrefab = Instantiate(GameManager.instance.TileClickPrefab, new Vector3(0, 0, 100), Quaternion.identity);
                            EnemyCircle.transform.position = Vector3.forward * 100;
                            EnemyCircle.SetActive(true);                            
                        }
                    }                    
                }                
            }

            //마우스 클릭중  캐릭터 움직임
            if (IsMove && Input.GetMouseButton(0))
            {               
                //클릭여러번 그냥 하는것 제외함  드래그 할때만 의미있도록
                if (Vector2.Distance(pos_down, Camera.main.ViewportToWorldPoint(Input.mousePosition)) < 500) return;
                
                IsMoveOK = true;
                IsBarricateClicked = false;
                IsBridgeClicked = false;
                SelectedTile_temp = ClickTile();

                if(GameManager.instance.map.GetTile(SelectedTile_temp)!=null && GameManager.instance.map.GetTile(SelectedTile_temp).name != "Pillar")
                {
                    SelectedTile = SelectedTile_temp;
                }

                TileSelectDotPrefab.transform.position = GameManager.instance.map.CellToWorld(SelectedTile) + 0.2f * Vector3.up;

                TileSelectPrefab.transform.position = GameManager.instance.map.CellToWorld(SelectedTile) + 0.7f * Vector3.up;


                cameraPos = Camera.main.transform.position;
                //클릭동안 카메라 움직임
                //위로
                if (Camera.main.ScreenToViewportPoint(Input.mousePosition).y > 0.8f)
                {
                    cameraPos.y = Mathf.Clamp(cameraPos.y+0.1f, Boundary.bounds.min.y, Boundary.bounds.max.y);                    
                    Camera.main.transform.position = cameraPos;
                }
                //아래
                if (Camera.main.ScreenToViewportPoint(Input.mousePosition).y < 0.1f)
                {
                    cameraPos.y = Mathf.Clamp(cameraPos.y - 0.1f, Boundary.bounds.min.y, Boundary.bounds.max.y);
                    Camera.main.transform.position = cameraPos;
                }
                //오른쪽
                if (Camera.main.ScreenToViewportPoint(Input.mousePosition).x > 0.9f)
                {
                    cameraPos.x = Mathf.Clamp(cameraPos.x + 0.1f, Boundary.bounds.min.x, Boundary.bounds.max.x);
                    Camera.main.transform.position = cameraPos;
                }
                //왼쪽
                if (Camera.main.ScreenToViewportPoint(Input.mousePosition).x < 0.1f)
                {
                    cameraPos.x = Mathf.Clamp(cameraPos.x - 0.1f, Boundary.bounds.min.x, Boundary.bounds.max.x);
                    Camera.main.transform.position = cameraPos;
                }
                pos = Camera.main.ScreenToWorldPoint(Input.mousePosition + ClickAdjustPos);
                _col = Physics2D.OverlapCircleAll(pos,0);
                int index = -1;
                float dist = 99999999;

                for (int i = 0; i < _col.Length; i++)
                {
                    if (_col[i].CompareTag("Untagged") || _col[i].CompareTag("Node")) continue;

                    if (dist > Vector2.Distance(_col[i].transform.position,pos))
                    {
                        dist = Vector2.Distance(_col[i].transform.position, pos);
                        index = i;
                    }
                }

                //그 외에는      //그냥 타일에 대면 타일    move
                if (index == -1)
                {
                    TileSelectDotPrefab.SetActive(true);

                    IsChase = false;
                    EnemyCircle.transform.position = Vector3.forward * 100;
                    for (int i = 0; i < ClickedChars.Count; i++)
                    {
                        ClickedChars[i].GetComponent<LineRenderer>().SetPosition(0, ClickedChars[i].transform.position);
                        ClickedChars[i].GetComponent<LineRenderer>().SetPosition(1, pos);
                        
                    }
                    MousePointer.transform.position = pos;

                    if (GameManager.instance.map.GetTile(SelectedTile)!=null)
                    { //맵에 클릭했을때
                        TileSelectPrefab.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprite/Go");
                    }
                    else
                    {
                        TileSelectPrefab.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprite/Stop");
                    }
                    TileSelectPrefab.transform.SetParent(null);


                }
                else
                {
                    //캐릭터 위에 대면 캐릭터  chase                
                    if (_col[index].CompareTag("Unit"))
                    {
                        TileSelectDotPrefab.SetActive(false);

                        ClickedUnit = _col[index].gameObject;
                        //우리팀이면
                        if (_col[index].GetComponent<Unit>().Master == 1)
                        {
                            TileSelectPrefab.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprite/Help");
                            TileSelectPrefab.transform.position = _col[index].transform.position + Vector3.up * 1.5f;
                            TileSelectPrefab.transform.SetParent(_col[index].transform);
                        }
                        else
                        {
                            TileSelectPrefab.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprite/Fight");
                            TileSelectPrefab.transform.position = _col[index].transform.position + Vector3.up * 1.5f;
                            TileSelectPrefab.transform.SetParent(_col[index].transform);

                        }
                        IsChase = true;

                        for (int i = 0; i < ClickedChars.Count; i++)
                        {
                            ClickedChars[i].GetComponent<LineRenderer>().SetPosition(0, ClickedChars[i].transform.position);
                            ClickedChars[i].GetComponent<LineRenderer>().SetPosition(1, ClickedUnit.transform.position + 0.3f * Vector3.up);
                        }
                        MousePointer.transform.position = pos;

                        EnemyCircle.transform.position = ClickedUnit.transform.position + Vector3.up * 0.3f;
                        EnemyCircle.transform.SetParent(ClickedUnit.transform);
                    }
                    //장애물 위에 대면 장애물  장애물 부수기
                    else if (_col[index].CompareTag("TreeAndBush"))
                    {
                        TileSelectDotPrefab.SetActive(false);
                        IsChase = false;

                        for (int i = 0; i < ClickedChars.Count; i++)
                        {
                            ClickedChars[i].GetComponent<LineRenderer>().SetPosition(0, ClickedChars[i].transform.position);
                            ClickedChars[i].GetComponent<LineRenderer>().SetPosition(1, _col[index].transform.position + 0.3f * Vector3.up);
                        }
                        MousePointer.transform.position = pos;

                        TileSelectPrefab.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprite/Fight");
                        TileSelectPrefab.transform.position = _col[index].transform.position + Vector3.up * 1.5f;
                    }
                    //배러같은거 누르는 경우
                    else
                    {
                        TileSelectDotPrefab.SetActive(true);
                        IsChase = false;
                        EnemyCircle.transform.position = Vector3.forward * 100;
                        TileSelectPrefab.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprite/Go");

                        for (int i = 0; i < ClickedChars.Count; i++)
                        {
                            ClickedChars[i].GetComponent<LineRenderer>().SetPosition(0, ClickedChars[i].transform.position);
                            ClickedChars[i].GetComponent<LineRenderer>().SetPosition(1, pos);
                        }
                        MousePointer.transform.position = pos;

                    }
                }
            }

            //장애물 클릭후 움직임
            if(IsObstacleMove && Input.GetMouseButton(0))
            {
                if (Vector2.Distance(pos_down, Camera.main.ViewportToWorldPoint(Input.mousePosition)) < 500) return;

                SelectedTile = ClickTile();

                if (IsObstacle)
                {
                    if (GameManager.instance.map.GetTile(SelectedTile) == null || GameManager.instance.TileMaster[GameManager.instance.mapsize / 2 + SelectedTile.x, GameManager.instance.mapsize / 2 + SelectedTile.y] > 0)
                    {
                        ObstaclePrefab.GetComponentInChildren<Button>().gameObject.GetComponent<Image>().color = Color.gray * 1.5f;
                        ObstaclePrefab.GetComponentInChildren<Button>().enabled = false;
                        ObstaclePrefab.GetComponentInChildren<CircleCollider2D>().gameObject.GetComponent<SpriteRenderer>().color = Color.red / 2f;
                    }
                    else
                    {
                        ObstaclePrefab.GetComponentInChildren<Button>().gameObject.GetComponent<Image>().color = Color.white;
                        ObstaclePrefab.GetComponentInChildren<Button>().enabled = true;
                        ObstaclePrefab.GetComponentInChildren<CircleCollider2D>().gameObject.GetComponent<SpriteRenderer>().color = Color.green / 2f;
                    }

                    ObstaclePrefab.transform.position = GameManager.instance.change.CellToWorld(SelectedTile) + Vector3.forward * 1.9f;
                    ObstaclePrefab.GetComponentInChildren<CircleCollider2D>().isTrigger = false;
                }
                else if(IsBridge)
                {
                    BridgePos = ClickWaterTile();

                    if (IsnextmapExist(BridgePos) && !IsMapHere(BridgePos))
                    {
                        //BridgePrefab.GetComponentInChildren<Button>().gameObject.GetComponent<Image>().enabled = true;
                        BridgePrefab.GetComponentInChildren<Button>().gameObject.GetComponent<Image>().color = Color.white;
                        BridgePrefab.GetComponentInChildren<Button>().enabled = true;
                        BridgePrefab.GetComponentInChildren<CircleCollider2D>().gameObject.GetComponent<SpriteRenderer>().color = Color.green / 2f;

                    }
                    else
                    {
                        //BridgePrefab.GetComponentInChildren<Button>().gameObject.GetComponent<Image>().enabled = false;
                        BridgePrefab.GetComponentInChildren<Button>().gameObject.GetComponent<Image>().color = Color.gray * 1.5f;
                        BridgePrefab.GetComponentInChildren<Button>().enabled = false;
                        BridgePrefab.GetComponentInChildren<CircleCollider2D>().gameObject.GetComponent<SpriteRenderer>().color = Color.red / 2f;

                    }
                    if (BridgePos == new Vector3Int(0,0,-1))
                        BridgePrefab.transform.position = GameManager.instance.change.CellToWorld(SelectedTile) + Vector3.down * 0.15f + Vector3.forward * 1.5f;
                    else
                        BridgePrefab.transform.position = GameManager.instance.change.CellToWorld(BridgePos) + Vector3.down * 0.13f - Vector3.forward * 0.4f;

                    BridgePrefab.GetComponentInChildren<CircleCollider2D>().isTrigger = false;

                }

                cameraPos = Camera.main.transform.position;
                //클릭동안 카메라 움직임
                //위로
                if (Camera.main.ScreenToViewportPoint(Input.mousePosition).y > 0.8f)
                {
                    cameraPos.y = Mathf.Clamp(cameraPos.y + 0.1f, Boundary.bounds.min.y, Boundary.bounds.max.y);
                    Camera.main.transform.position = cameraPos;
                }
                //아래
                if (Camera.main.ScreenToViewportPoint(Input.mousePosition).y < 0.1f)
                {
                    cameraPos.y = Mathf.Clamp(cameraPos.y - 0.1f, Boundary.bounds.min.y, Boundary.bounds.max.y);
                    Camera.main.transform.position = cameraPos;
                }
                //오른쪽
                if (Camera.main.ScreenToViewportPoint(Input.mousePosition).x > 0.9f)
                {
                    cameraPos.x = Mathf.Clamp(cameraPos.x + 0.1f, Boundary.bounds.min.x, Boundary.bounds.max.x);
                    Camera.main.transform.position = cameraPos;
                }
                //왼쪽
                if (Camera.main.ScreenToViewportPoint(Input.mousePosition).x < 0.1f)
                {
                    cameraPos.x = Mathf.Clamp(cameraPos.x - 0.1f, Boundary.bounds.min.x, Boundary.bounds.max.x);
                    Camera.main.transform.position = cameraPos;
                }                
            }
            //마우스 업
            if (Input.GetMouseButtonUp(0))
            {
                ObstaclePrefab.GetComponentInChildren<CircleCollider2D>().isTrigger = true;
                BridgePrefab.GetComponentInChildren<CircleCollider2D>().isTrigger = true;

                pos_up = Camera.main.ViewportToWorldPoint(Input.mousePosition);
                IsMove = false;
                EnemyCircle.SetActive(false);
                GameManager.instance.GetComponent<FingersScript>().enabled = true;

                //제대로 드래그 한 경우
                if (IsMoveOK)
                {
                    TileSelectPrefab.GetComponent<DestroySoon>().DestroyObject();

                    for (int i = 0; i < ClickedChars.Count; i++)
                    {
                        ClickedChars[i].GetComponent<LineRenderer>().SetPosition(0, ClickedChars[i].transform.position);
                        ClickedChars[i].GetComponent<LineRenderer>().SetPosition(1, ClickedChars[i].transform.position);
                        ClickedChars[i].GetComponent<LineRenderer>().enabled = false;
                    }
                    MousePointer.SetActive(false);

                    IsMoveOK = false;
                    //캐릭터위에서 땐 경우 Chase
                    if (IsChase)
                    {
                        ChaseOrder();
                        IsChase = false;
                    }
                    //그 외의 경우 Move
                    else
                    {
                        MoveOrder();                         
                    }

                    TileSelectDotPrefab.SetActive(false);

                    pos = Camera.main.ScreenToWorldPoint(Input.mousePosition + ClickAdjustPos);
                    _col = Physics2D.OverlapCircleAll(pos, 0);
                    for (int i = 0; i < _col.Length; i++)
                    {
                        if (_col[i].CompareTag("TreeAndBush"))
                        {
                            for (int j = 0; j < ClickedChars.Count; j++)
                                ClickedChars[j].GetComponent<Unit>().BarrigateClicked = _col[i].gameObject;
                            break;
                        }
                    }                    
                    return;
                }
                //장애물 설치하려고 temp나타나있는 경우
                if(IsObstacleMove)
                {
                    GameManager.instance.GetComponent<FingersScript>().enabled = true;
                    IsObstacleMove = false;
                }
                else
                {
                    if (Vector2.Distance(pos_down, pos_up) > 500)
                    {
                        IsDoubleClick = false;
                        return;
                    }
                    //드래그 없이 클릭완료한경우
                    else
                    {
                        if(IsObstacle || IsBridge)
                        {
                            //다른곳 눌러서 Obstacle 취소 할때
                            if (!bool_temp)
                            {
                                ObstaclePrefab.SetActive(false);
                                BridgePrefab.SetActive(false);
                                BarricateButton.GetComponentInChildren<Cooltime>().gameObject.GetComponent<Image>().color = Color.white;
                                BridgeButton.GetComponentInChildren<Cooltime>().gameObject.GetComponent<Image>().color = Color.white;

                                IsObstacle = false;
                                IsBridge = false;

                                for (int j = 0; j < SelectPrefab.Count; j++)
                                    Destroy(SelectPrefab[j]);
                                ClickedChars.RemoveRange(0, ClickedChars.Count);
                            }
                        }
                        else if (IsStandPush)
                            IsStandPush = false;
                        else
                        {
                            pos_up = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                            _col = Physics2D.OverlapCircleAll(pos_up, 1);
                            int index = -1;
                            float min_dist = 99999999;
                            for (int i = 0; i < _col.Length; i++)
                            {
                                if (_col[i].CompareTag("Unit"))
                                {
                                    if (_col[i].GetComponent<Unit>().Master == 1 && Vector2.Distance(pos_up, _col[i].transform.position) < min_dist)
                                    {
                                        min_dist = Vector2.Distance(pos_up, _col[i].transform.position);
                                        index = i;
                                    }
                                }
                            }
                            if (index != -1) //  뭔가를 찍었을때
                            {
                                if (_col[index].GetComponent<Unit>().UnitState == 3)
                                {
                                    GameManager.instance.InfoTexting("싸우는 대상은 선택할 수 없습니다.");
                                }
                                else
                                {
                                    for (int j = 0; j < SelectPrefab.Count; j++)
                                        Destroy(SelectPrefab[j]);

                                    ClickedChars.RemoveRange(0, ClickedChars.Count);
                                    ClickedChars.Add(_col[index].gameObject);

                                    SelectPrefab.Add(Instantiate(GameManager.instance.prefabs[1], _col[index].gameObject.transform));
                                    ClickedChars[0].GetComponent<Unit>().SelectArrow = SelectPrefab[0];

                                    if (IsBarricateClicked)
                                    {
                                        IsBarricateClicked = false;
                                        TriangleOffEffect();
                                        ObstacleOrder();

                                        IsMove = false;
                                    }
                                    else if(IsBridgeClicked)
                                    {
                                        IsBridgeClicked = false;
                                        TriangleOffEffect();
                                        BridgeOrder();

                                        IsMove = false;
                                    }
                                }
                            }
                            if (index == -1)   // 아무것도 안찍었을때
                            {              
                                if(IsBarricateClicked && !RaycastWorldUI())
                                {
                                    IsBarricateClicked = false;
                                    TriangleOffEffect();
                                }
                                if(IsBridgeClicked && !RaycastWorldUI())
                                {
                                    IsBridgeClicked = false;
                                    TriangleOffEffect();
                                }

                                if (ClickedChars.Count != 0)
                                {
                                    for (int j = 0; j < ClickedChars.Count; j++)
                                    {
                                        Destroy(ClickedChars[j].GetComponent<Unit>().SelectArrow);
                                    }
                                    ClickedChars.RemoveRange(0, ClickedChars.Count);
                                    for (int j = 0; j < SelectPrefab.Count; j++)
                                    {
                                        Destroy(SelectPrefab[j]);
                                    }
                                }                         
                                GameManager.instance.GetComponent<FingersScript>().enabled = true;
                            }
                            else
                            {
                                //한마리만 찍은상태에서   더블클릭하려고 할때
                                if (ClickedChars.Count == 1 && IsDoubleClick && _col[index].gameObject == ClickedChars[0])
                                {
                                    DoubleClicktimer_temp = 0;

                                    ClickedChars = ObjectInCamera(ClickedChars[0].transform.position);
                                    for (int j = 0; j < ClickedChars.Count; j++)
                                    {
                                        Destroy(ClickedChars[j].GetComponent<Unit>().SelectArrow);
                                        SelectPrefab.Add(Instantiate(GameManager.instance.prefabs[1], ClickedChars[j].transform));
                                        ClickedChars[j].GetComponent<Unit>().SelectArrow = SelectPrefab[j];
                                    }
                                    GameManager.instance.GetComponent<FingersScript>().enabled = true;
                                }
                                //한마리만 찍는 상태일때
                                else if (ClickedChars.Count == 1)
                                {
                                    IsDoubleClick = true;
                                    DoubleClicktimer_temp = 0;
                                    GameManager.instance.GetComponent<FingersScript>().enabled = true;
                                }
                            }
                            
                        }
                    }
                }                
            }
        }
    }

    //카메라내 케릭터 선택
    private List<GameObject> ObjectInCamera(Vector3 pos)
    {
        pos += 0.3f *Vector3.up;

        List<GameObject> ret = new List<GameObject>();

        _col = Physics2D.OverlapCircleAll(pos, 1.7f);

        for (int i = 0; i < _col.Length; i++)
        {
            if (_col[i].CompareTag("Unit")&&_col[i].GetComponent<Unit>().Master == 1&&!_col[i].GetComponent<Unit>().IsStand)
            {
                ret.Add(_col[i].gameObject);
            }
        }


        /*for (int i=0;i< GameManager.instance.Unit1.Count;i++)
        {


            //Vector3 pos = Camera.main.WorldToViewportPoint(GameManager.instance.Unit1[i].transform.position);

            if (pos.x < 0 || pos.x>1 || pos.y<0 || pos.y>0.8f)
            {
                ;
            }
            else
            {

                if(ret.Count<6)

            }
        }*/
        return ret;
    }

    //타일 선택
    private Vector3Int ClickTile()
    {
        Vector3Int v3Int  = new Vector3Int(0,0,-1);

        Vector3 pos;
        if (IsObstacleMove)
        {
            if (Input.touchCount > 0)
            {
                pos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            }
            else
            {
                pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
            pos += (Vector3)Obstacle_AdjustDist;
        }
        else
        {

            if (Input.touchCount > 0)
            {
                pos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position + (Vector2)ClickAdjustPos);
            }            
            else
            {
                pos = Camera.main.ScreenToWorldPoint(Input.mousePosition + ClickAdjustPos);
            }
        }

        Ray2D ray = new Ray2D(pos, Vector2.zero);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
        if (hit.transform != null)
        {
            for (int i = 20; i >= 0; i--)
            {
                pos.z = i;
                v3Int = GameManager.instance.map.WorldToCell(pos);
                if (GameManager.instance.map.GetTile(v3Int)!=null)
                {
                    if (GameManager.instance.map.GetTile(v3Int).name == "Pillar")
                    {
                        continue;
                    }
                    break;
                }
            }
        }
        return v3Int;
    }

    //물 또는 허공 선택
    private Vector3Int ClickWaterTile()
    {
        Vector3Int v3Int = new Vector3Int(0, 0, -1);
        Vector3 pos;

        if(IsObstacleMove)
        {

            if (Input.touchCount > 0)
            {
                pos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            }
            else
            {
                pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
            pos += (Vector3)Obstacle_AdjustDist;
        }
        else
        {
            if (Input.touchCount > 0)
            {
                pos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position + (Vector2)ClickAdjustPos);
            }
            else
            {
                pos = Camera.main.ScreenToWorldPoint(Input.mousePosition + ClickAdjustPos);
            }
        }
            
            
        
        Ray2D ray = new Ray2D(pos, Vector2.zero);
        Vector3Int ret = new Vector3Int(0, 0, -1);

        for (int i = 20; i >= 0; i--)
        {
            pos.z = i;
            v3Int = GameManager.instance.map.WorldToCell(pos);
            if (GameManager.instance.map.GetTile(v3Int) != null)
                continue;

            if (IsnextmapExist(v3Int))
            {
                ret = v3Int;
                break;
            }
        }
        return ret;
    }
    private bool IsnextmapExist(Vector3Int pos)
    {
        Vector3Int pos_temp;
        pos_temp = pos + Vector3Int.up;
        if (GameManager.instance.map.GetTile(pos_temp) != null && !(GameManager.instance.map.GetTile(pos_temp).name =="Pillar"))
            return true;        
        pos_temp = pos + Vector3Int.down;
        if (GameManager.instance.map.GetTile(pos_temp) != null && !(GameManager.instance.map.GetTile(pos_temp).name == "Pillar"))
            return true;
        pos_temp = pos + Vector3Int.right;
        if (GameManager.instance.map.GetTile(pos_temp) != null && !(GameManager.instance.map.GetTile(pos_temp).name == "Pillar"))
            return true;
        pos_temp = pos + Vector3Int.left;
        if (GameManager.instance.map.GetTile(pos_temp) != null && !(GameManager.instance.map.GetTile(pos_temp).name == "Pillar"))
            return true;
        return false;
    }

    private bool IsMapHere(Vector3Int pos)
    {
        for (int i = 20; i >= -5; i--)
        {
            pos.z = i;
            if(GameManager.instance.map.GetTile(pos)!=null || GameManager.instance.obstacle.GetTile(pos)!=null)
            {
                return true;
            }            
        }
        return false;
    }


    //게임시작 버튼
    public void GameStart()
    {
        //ready ~ 와함께 카메라 줌인
        ReadyText.SetActive(true);

        SoundManager.instance.Play(2);

        StartCoroutine(Zoomin());

    }

    IEnumerator Zoomin()
    {
        GameStartButton.SetActive(false);
        //zoomin 중
        
        this.gameObject.GetComponent<FingersScript>().enabled = false;
        
        while (true)
        {
            if (Camera.main.orthographicSize > 4)
                Camera.main.orthographicSize -= 0.1f;
            if (Vector2.Distance(Camera.main.transform.position, DataManager.instance.CameraStartPos[DataManager.instance.LastStage]) > 0.1f)
                Camera.main.transform.localPosition = Vector3.Lerp(Camera.main.transform.localPosition, DataManager.instance.CameraStartPos[DataManager.instance.LastStage] - Vector3.forward * 40, 2 * Time.fixedDeltaTime);
            yield return null;
            if (Vector2.Distance(Camera.main.transform.localPosition, DataManager.instance.CameraStartPos[DataManager.instance.LastStage]) <= 0.1f && Camera.main.orthographicSize <= 4)
            {
                break;
            }
        }       

        //zoomin  끝
        ReadyText.SetActive(false);
        StartText.SetActive(true);
        yield return null;

        if(DataManager.instance.LastStage == 0)
        {
            ;
        }
        else if (DataManager.instance.IsHard[DataManager.instance.LastStage])
        {
            BGMManager.instance.Play(5);
            BGMManager.instance.FadeInMusic();
        }
        else
        {
            BGMManager.instance.Play(4);
            BGMManager.instance.FadeInMusic();
        }
        SoundManager.instance.Play(1);
        IsGameStart = true;
        GameManager.instance.IsGameStart = true;

        //BarricateButton.SetActive(true);
        //BridgeButton.SetActive(true);
        //StandMoveIcon.SetActive(true);
        BarricateButton.GetComponent<Button>().enabled = true;
        BridgeButton.GetComponent<Button>().enabled = true;
        StandMoveIcon.GetComponent<Button>().enabled = true;

        for (int i = GameManager.instance.TrianglePrefabs.Count - 1; i >= 0; i--)
        {
            GameManager.instance.TrianglePrefabs[i].SetActive(false);
        }

        this.gameObject.GetComponent<FingersScript>().enabled = true;
    }


    //게임 끝나고 돌아가는 버튼
    public void GameEnd()
    {
        SoundManager.instance.Play(2);
        GameManager.instance.StagePanel.SetActive(true);
    }

    public void StageOK()
    {
        if (!IsButtonPress)
        {
            IsButtonPress = true;
        }
        else
            return;

        SoundManager.instance.Play(2);

        Time.timeScale = 1.0f;


        BlackPanel.SetActive(true);

        System.GC.Collect();
        BGMManager.instance.Play(0);
        BGMManager.instance.FadeInMusic();
        DataManager.instance.SaveData();


        AsyncOperation oper = new AsyncOperation();
        oper = SceneManager.LoadSceneAsync("Title");        
        StageSelectPrefab.instance.gameObject.SetActive(true);
        oper.allowSceneActivation = true;
    }

    //게임 끝나고 돌아가는 버튼
    public void EndToHome()
    {
        SoundManager.instance.Play(2);
        
        GameManager.instance.HomePanel.SetActive(true);
    }

    public void HomeOK()
    {
        if (!IsButtonPress)
        {
            IsButtonPress = true;
        }
        else
            return;


        Time.timeScale = 1.0f;
        SoundManager.instance.Play(2);
        BGMManager.instance.FadeOutMusic();
        System.GC.Collect();
        AsyncOperation oper = new AsyncOperation();
        oper = SceneManager.LoadSceneAsync("Title");
        BlackPanel.SetActive(true);
        TitlePrefab.instance.gameObject.SetActive(true);
        oper.allowSceneActivation = true;
    }

    //일시정지
    public void SettingButton()
    {
        SoundManager.instance.Play(2);

        IsPressGoBack = true;

        Time.timeScale = 0f;

        GameManager.instance.SettingPanel.SetActive(true);

        this.gameObject.GetComponent<Click>().enabled = false;
        this.gameObject.GetComponent<FingersScript>().enabled = false;
    }

    public void RestartPanelOn()
    {
        SoundManager.instance.Play(2);
        GameManager.instance.RestartPanel.SetActive(true);
    }
    public void RestartPanelOff()
    {
        SoundManager.instance.Play(2);
        GameManager.instance.RestartPanel.SetActive(false);
    }

    //일시정지
    public void ReStartOK()
    {
        if (!IsButtonPress)
        {
            IsButtonPress = true;
        }
        else
            return;

        SoundManager.instance.Play(2);

        Time.timeScale = 1.0f;

        AsyncOperation oper = new AsyncOperation();
        oper = SceneManager.LoadSceneAsync("Title");
        BlackPanel.SetActive(true);
        StageSelectPrefab.instance.gameObject.SetActive(true);
        oper.allowSceneActivation = true;

    }
    //일시정지
    public void SettingPanelExit()
    {
        SoundManager.instance.Play(2);

        IsPressGoBack = false;

        GameManager.instance.SettingPanel.SetActive(false);

        this.gameObject.GetComponent<Click>().enabled = true;
        this.gameObject.GetComponent<FingersScript>().enabled = true;

    }

    public void BGMSetting(Slider vol)
    {
        BGMManager.instance.SetGameVolume(vol);
    }
    public void SfxSetting(Slider vol)
    {
        SoundManager.instance.SetGameVolumn(vol);
    }
    public void GameRestart()
    {
        SoundManager.instance.Play(2);
        GameManager.instance.IsGameStart = false;

        DataManager.instance.SaveData();

        if(GameManager.instance.VictoryPanel.activeSelf)
        {
            DataManager.instance.LastStage--;
        }

        System.GC.Collect();
        SceneManager.LoadScene("Loading");
    }
    public void MapCancel()
    {
        SoundManager.instance.Play(2);

        GameManager.instance.StagePanel.SetActive(false);
    }
    public void HomeCancel()
    {
        SoundManager.instance.Play(2);

        GameManager.instance.HomePanel.SetActive(false);
    }

    public void GameSpeedButtonPress()
    {
        SoundManager.instance.Play(2);

        IsFast = !IsFast;

        Color _color = Color.white;
        _color.a = 0.3f;
            
        if (IsFast)
        {
            GameSpeedIcon.GetComponent<Image>().color = Color.white;
        }
        else
        {
            GameSpeedIcon.GetComponent<Image>().color = _color;

        }
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
        

    private void TriangleEffect()
    {
        for (int i = 0; i < GameManager.instance.TrianglePrefabs.Count; i++)
        {
            GameManager.instance.TrianglePrefabs[i].SetActive(false);
        }
        GameObject tri_prefab;
        for(int i=0;i<GameManager.instance.Unit1.Count;i++)
        {
            tri_prefab = GameManager.instance.TrianglePrefabPooling();
            tri_prefab.transform.position = GameManager.instance.Unit1[i].transform.position + 1.2f * Vector3.up;
            tri_prefab.GetComponent<SpriteRenderer>().color = _color_;
            tri_prefab.SetActive(true);
        }
    }

    private void TriangleOffEffect()
    {
        for (int i = 0; i < GameManager.instance.TrianglePrefabs.Count; i++)
        {
            GameManager.instance.TrianglePrefabs[i].SetActive(false);
        }
    }



    /*
    public bool BarricateUnitNext()
    {
        if (
        GameManager.instance.UnitPos[GameManager.instance.mapsize / 2 + SelectedTile.x, GameManager.instance.mapsize / 2 + SelectedTile.y] != null ||
            GameManager.instance.UnitPos[GameManager.instance.mapsize / 2 + SelectedTile.x + 1, GameManager.instance.mapsize / 2 + SelectedTile.y] != null ||
            GameManager.instance.UnitPos[GameManager.instance.mapsize / 2 + SelectedTile.x - 1, GameManager.instance.mapsize / 2 + SelectedTile.y] != null ||
            GameManager.instance.UnitPos[GameManager.instance.mapsize / 2 + SelectedTile.x, GameManager.instance.mapsize / 2 + SelectedTile.y + 1] != null ||
            GameManager.instance.UnitPos[GameManager.instance.mapsize / 2 + SelectedTile.x, GameManager.instance.mapsize / 2 + SelectedTile.y - 1] != null)
            return true;
        return false;
    }*/
}
