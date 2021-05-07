using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    static public GameManager instance;

    public Tilemap map;
    public Tilemap obstacle;
    public Tilemap change;

    public int[,] MapMaster;//0 기본   마스터 1 2 3 4      장애물 5    못가는곳 6   다리 7
    public int[,] MapZ; // 갈수있는곳의 Z축

    public GameObject Character;

    public GameObject Barrack;

    public GameObject Obstacle;

    public GameObject Ghost;

    public int[,] TileMaster;
    public GameObject[,] ObstaclePos;
    public GameObject[,] UnitPos;

    public List<GameObject> TileChar;
    public List<GameObject> TileChar1;
    public List<GameObject> TileChar2;
    public List<GameObject> TileChar3;

    public List<GameObject> Unit1;
    public List<GameObject> Unit2;
    public List<GameObject> Unit3;
    public List<GameObject> Unit4;


    public List<GameObject> FightPrefabs;
    public List<GameObject> UnitPrefabs;
    public List<GameObject> BarrackPrefabs;
    public List<GameObject> BarricatePrefabs;
    public List<GameObject> ProducePrefabs;
    public List<GameObject> MergePrefabs;
    public List<GameObject> MergeSmallPrefabs;
    public List<GameObject> GhostPrefabs;
    public List<GameObject> TrianglePrefabs;

    private int FightIndex;
    private int UnitInex;
    private int BarrackIndex;
    private int BarricateIndex;
    private int ProduceIndex;
    private int MergeIndex;
    private int MergeSmallIndex;
    private int GhostIndex;
    private int TriangleIndex;

    public int NumMapTile;
    public int[] Mapnumber;
    public int[] NodeNumber;

    private bool IsUnitFaster;

    public int mapsize = 200;

    public int[] money;
    public bool[] IsNoMoney;
    public int[] people;
    private int people_temp;

    public Text[] Amounttext;
    public Text[] Tiletext;
    public Text[] Foodtext;

    public Image[] LandImage;
    public Slider[] PeopleGraph;
    private int Biggest_people;
    private int biggest_people_temp;

    public GameObject InfoText;

    public GameObject FightPrefabTransform;
    public GameObject CharacterTransform;
    public GameObject BarrackTransform;
    public GameObject BarricateTransform;
    public GameObject ProduceTransform;
    public GameObject MergeTrasnform;
    public GameObject MergeSmallTrasnform;
    public GameObject GhostTrasnform;
    public GameObject TriangleTrasnform;

    public GameObject[] prefabs;
    public GameObject Obstacle_prefab;
    public GameObject Bridge_prefab;
    public GameObject TileClickDotPrefab;
    public GameObject TileClickPrefab;

    public float BridgeCooltime = 30f;
    public float BridgeCooltime_Cur;
    public float ObstacleCooltime = 3f;
    public float ObstacleCooltime_Cur;

    public int MoneyAmount = 100;

    public bool BridgeAble;
    public bool ObstacleAble;

    private float Foodtimer;
    private float GraphTimer;

    public GameObject VictoryPanel;
    public GameObject DefeatPanel;
    public GameObject SettingPanel;
    public GameObject HomePanel;
    public GameObject StagePanel;
    public GameObject RestartPanel;

    public GameObject[] VictoryStars;

    public Text UnitNumText;
    public Text FoodNumText;
    public Text ScoreText;
    public Text DefeatScoreText;
    public float TimeForText;

    public bool[] IsDead;

    public bool IsGameStart;

    private WaitForSeconds FightWaittime;

    public Slider BGMSlider;
    public Slider SfxSlider;


    public Text BridgeText;
    public Text BarricateText;
    public int BridgeLeft;
    public int BarricateLeft;

    private bool IsHard;

    public GameObject[] BigStartAppear;

    public Text CheckmapandStart;

    private bool[] Nofood;

    public GameObject[] NoFoodIcon;

    private void Awake()
    {
        map = GameObject.Find("Map").GetComponent<Tilemap>();
        obstacle = GameObject.Find("ObstacleMap").GetComponent<Tilemap>();
        change = GameObject.Find("ChangeMap").GetComponent<Tilemap>();
        Time.timeScale = 0;

        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            //DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        TileMaster = new int[mapsize, mapsize];
        MapMaster = new int[mapsize, mapsize];
        MapZ = new int[mapsize, mapsize];
        MapDefine();
        ObstaclePos = new GameObject[mapsize, mapsize];
        UnitPos = new GameObject[mapsize, mapsize];
        TileChar = new List<GameObject>();
        TileChar1 = new List<GameObject>();
        TileChar2 = new List<GameObject>();
        TileChar3 = new List<GameObject>();
        people = new int[5];
        Mapnumber = new int[5];
        NodeNumber = new int[5];

        FightPrefabs = new List<GameObject>();
        UnitPrefabs = new List<GameObject>();
        BarrackPrefabs = new List<GameObject>();
        BarricatePrefabs = new List<GameObject>();

        TrianglePrefabs = new List<GameObject>();

        FightIndex = 0;
        UnitInex = 0;
        BarrackIndex = 0;
        BarricateIndex = 0;
        ProduceIndex = 0;
        MergeIndex = 0;

        TimeForText = 0;
        IsGameStart = false;
        ObjectPooling();
    }
    private void ObjectPooling()
    {
        GameObject _gameobj_temp;
        for (int i = 0; i < 30; i++)
        {
            _gameobj_temp = Instantiate(prefabs[0]);
            _gameobj_temp.transform.SetParent(FightPrefabTransform.transform);
            _gameobj_temp.SetActive(false);
            FightPrefabs.Add(_gameobj_temp);
        }
        for (int i = 0; i < 200; i++)
        {
            _gameobj_temp = Instantiate(Character);
            _gameobj_temp.transform.SetParent(CharacterTransform.transform);
            _gameobj_temp.SetActive(false);
            UnitPrefabs.Add(_gameobj_temp);
        }
        for (int i = 0; i < 400; i++)
        {
            _gameobj_temp = Instantiate(Barrack);
            _gameobj_temp.transform.SetParent(BarrackTransform.transform);
            _gameobj_temp.SetActive(false);
            BarrackPrefabs.Add(_gameobj_temp);
        }
        for (int i = 0; i < 100; i++)
        {
            _gameobj_temp = Instantiate(Obstacle);
            _gameobj_temp.transform.SetParent(BarricateTransform.transform);
            _gameobj_temp.SetActive(false);
            BarricatePrefabs.Add(_gameobj_temp);
        }
        for (int i = 0; i < 30; i++)
        {
            _gameobj_temp = Instantiate(prefabs[2]);
            _gameobj_temp.transform.SetParent(ProduceTransform.transform);
            _gameobj_temp.SetActive(false);
            ProducePrefabs.Add(_gameobj_temp);
        }
        for (int i = 0; i < 20; i++)
        {
            _gameobj_temp = Instantiate(prefabs[3]);
            _gameobj_temp.transform.SetParent(MergeTrasnform.transform);
            _gameobj_temp.SetActive(false);
            MergePrefabs.Add(_gameobj_temp);
        }
        for (int i = 0; i < 30; i++)
        {
            _gameobj_temp = Instantiate(prefabs[6]);
            _gameobj_temp.transform.SetParent(MergeSmallTrasnform.transform);
            _gameobj_temp.SetActive(false);
            MergeSmallPrefabs.Add(_gameobj_temp);
        }
        for (int i = 0; i < 30; i++)
        {
            _gameobj_temp = Instantiate(Ghost);
            _gameobj_temp.transform.SetParent(GhostTrasnform.transform);
            _gameobj_temp.SetActive(false);
            GhostPrefabs.Add(_gameobj_temp);
        }
        for (int i = 0; i < 30; i++)
        {
            _gameobj_temp = Instantiate(prefabs[5]);
            _gameobj_temp.transform.SetParent(TriangleTrasnform.transform);
            _gameobj_temp.SetActive(false);
            TrianglePrefabs.Add(_gameobj_temp);
        }
    }

    public GameObject FightPrefabPooling()
    {
        while (true)
        {
            FightIndex++;
            if (FightIndex >= 30)
                FightIndex = 0;
            if (!FightPrefabs[FightIndex].activeInHierarchy)
                return FightPrefabs[FightIndex];
        }
    }
    public GameObject CharacterPrefabPooling()
    {
        while (true)
        {
            UnitInex++;
            if (UnitInex >= 200)
                UnitInex = 0;
            if (!UnitPrefabs[UnitInex].activeInHierarchy)
                return UnitPrefabs[UnitInex];

        }
    }
    public GameObject BarrackPrefabPooling()
    {
        while (true)
        {
            BarrackIndex++;
            if (BarrackIndex >= 400)
                BarrackIndex = 0;
            if (!BarrackPrefabs[BarrackIndex].activeInHierarchy)
                return BarrackPrefabs[BarrackIndex];

        }
    }
    public GameObject BarricatePrefabPooling()
    {
        while (true)
        {
            BarricateIndex++;
            if (BarricateIndex >= 100)
                BarricateIndex = 0;
            if (!BarricatePrefabs[BarricateIndex].activeInHierarchy)
                return BarricatePrefabs[BarricateIndex];
        }
    }
    public GameObject ProducePrefabPooling()
    {
        while (true)
        {
            ProduceIndex++;
            if (ProduceIndex >= 30)
                ProduceIndex = 0;
            if (!ProducePrefabs[ProduceIndex].activeInHierarchy)
                return ProducePrefabs[ProduceIndex];

        }
    }
    public GameObject MergePrefabPooling()
    {
        while (true)
        {
            MergeIndex++;
            if (MergeIndex >= 20)
                MergeIndex = 0;
            if (!MergePrefabs[MergeIndex].activeInHierarchy)
                return MergePrefabs[MergeIndex];
        }
    }
    public GameObject MergeSmallPrefabPooling()
    {
        while (true)
        {
            MergeSmallIndex++;
            if (MergeSmallIndex >= 30)
                MergeSmallIndex = 0;
            if (!MergeSmallPrefabs[MergeSmallIndex].activeInHierarchy)
                return MergeSmallPrefabs[MergeSmallIndex];
        }
    }
    public GameObject GhostPrefabPooling()
    {
        while (true)
        {
            GhostIndex++;
            if (GhostIndex >= 30)
                GhostIndex = 0;
            if (!GhostPrefabs[GhostIndex].activeInHierarchy)
                return GhostPrefabs[GhostIndex];
        }
    }
    public GameObject TrianglePrefabPooling()
    {
        while (true)
        {
            TriangleIndex++;
            if (TriangleIndex >= 30)
                TriangleIndex = 0;
            if (!TrianglePrefabs[TriangleIndex].activeInHierarchy)
                return TrianglePrefabs[TriangleIndex];
        }
    }
    private void MapDefine()
    {
        NumMapTile = 0;
        Vector3Int vec3inttemp = new Vector3Int();
        for (int i = -mapsize / 2; i < mapsize / 2; i++)
        {
            for (int j = -mapsize / 2; j < mapsize / 2; j++)
            {
                for (int k = 20; k >= 0; k--)
                {
                    vec3inttemp.x = i;
                    vec3inttemp.y = j;
                    vec3inttemp.z = k;

                    if (map.GetTile(vec3inttemp) != null)
                    {
                        if (map.GetTile(vec3inttemp).name == "BridgeLU" || map.GetTile(vec3inttemp).name == "BridgeRU")
                        {
                            MapMaster[i + mapsize / 2, j + mapsize / 2] = 7;
                            MapZ[i + mapsize / 2, j + mapsize / 2] = k;
                            NumMapTile++;
                            break;
                        }
                        if (map.GetTile(vec3inttemp).name == "Pillar")
                        {
                            ;
                        }
                        else
                        {
                            MapMaster[i + mapsize / 2, j + mapsize / 2] = 0;
                            MapZ[i + mapsize / 2, j + mapsize / 2] = k;
                            NumMapTile++;
                            break;
                        }
                    }
                    else
                        MapMaster[i + mapsize / 2, j + mapsize / 2] = 6;
                }
            }
        }
    }

    void Start()
    {
        if (DataManager.instance != null)
        {
            if (LandImage.Length > 0)
            {
                LandImage[0].color = DataManager.instance.color1;
                LandImage[1].color = DataManager.instance.color2;
                LandImage[2].color = DataManager.instance.color3;
                LandImage[3].color = DataManager.instance.color4;

                PeopleGraph[0].fillRect.GetComponent<Image>().color = DataManager.instance.color1;
                PeopleGraph[0].handleRect.GetComponent<Image>().color = DataManager.instance.color1;
                PeopleGraph[1].fillRect.GetComponent<Image>().color = DataManager.instance.color2;
                PeopleGraph[1].handleRect.GetComponent<Image>().color = DataManager.instance.color2;
                PeopleGraph[2].fillRect.GetComponent<Image>().color = DataManager.instance.color3;
                PeopleGraph[2].handleRect.GetComponent<Image>().color = DataManager.instance.color3;
                PeopleGraph[3].fillRect.GetComponent<Image>().color = DataManager.instance.color4;
                PeopleGraph[3].handleRect.GetComponent<Image>().color = DataManager.instance.color4;


                if (DataManager.instance.SelectedChar == 2)
                {
                    LandImage[0].color = DataManager.instance.color2;
                    LandImage[1].color = DataManager.instance.color1;
                    PeopleGraph[1].fillRect.GetComponent<Image>().color = DataManager.instance.color1;
                    PeopleGraph[1].handleRect.GetComponent<Image>().color = DataManager.instance.color1;

                    PeopleGraph[0].fillRect.GetComponent<Image>().color = DataManager.instance.color2;
                    PeopleGraph[0].handleRect.GetComponent<Image>().color = DataManager.instance.color2;
                }
                else if (DataManager.instance.SelectedChar == 3)
                {
                    LandImage[0].color = DataManager.instance.color3;
                    LandImage[2].color = DataManager.instance.color1;
                    PeopleGraph[2].fillRect.GetComponent<Image>().color = DataManager.instance.color1;
                    PeopleGraph[2].handleRect.GetComponent<Image>().color = DataManager.instance.color1;
                    PeopleGraph[0].fillRect.GetComponent<Image>().color = DataManager.instance.color3;
                    PeopleGraph[0].handleRect.GetComponent<Image>().color = DataManager.instance.color3;
                }
                else if (DataManager.instance.SelectedChar == 4)
                {
                    LandImage[0].color = DataManager.instance.color4;
                    LandImage[3].color = DataManager.instance.color1;
                    PeopleGraph[3].fillRect.GetComponent<Image>().color = DataManager.instance.color1;
                    PeopleGraph[3].handleRect.GetComponent<Image>().color = DataManager.instance.color1;
                    PeopleGraph[0].fillRect.GetComponent<Image>().color = DataManager.instance.color4;
                    PeopleGraph[0].handleRect.GetComponent<Image>().color = DataManager.instance.color4;
                }
            }

            BGMSlider.value = DataManager.instance.VolumeSettingValue;
            SfxSlider.value = DataManager.instance.SoundSettingValue;
        }

        if (DataManager.instance.IsKorean)
        {
            CheckmapandStart.text = TextScript.instance.str_kor[6];
        }
        else
        {
            CheckmapandStart.text = TextScript.instance.str_Eng[6];
        }
        money = new int[5];
        IsNoMoney = new bool[5];
        IsDead = new bool[4];
        Nofood = new bool[4];
        for (int i = 0; i < 5; i++)
        {
            money[i] = 1000;
            IsNoMoney[i] = false;

            if (DataManager.instance.LastStage == 10 && DataManager.instance.IsHard[10] || DataManager.instance.LastStage == 12)
                money[i] = 500000;
        }
        BridgeAble = false;
        ObstacleAble = false;
        IsGameStart = false;

        IsUnitFaster = false;

        BridgeCooltime_Cur = 9999;
        ObstacleCooltime_Cur = 0;

        Foodtimer = 0;
        GraphTimer = 2;
        FightWaittime = new WaitForSeconds(0.5f);

        BridgeLeft = 8;
        BridgeText.text = BridgeLeft.ToString();

        IsHard = DataManager.instance.IsHard[DataManager.instance.LastStage];

        if (IsHard)
        {
            BarricateLeft = 1;
        }
        else
        {
            BarricateLeft = 2;
        }
        BarricateText.text = BarricateLeft.ToString();
    }

    private void Update()
    {
        if (BridgeLeft > 0)
            BridgeCooltime_Cur += Time.deltaTime;

        if (IsHard)
        {
            if (BarricateLeft < 1)
            {
                ObstacleCooltime_Cur += Time.deltaTime;
                if (ObstacleCooltime <= ObstacleCooltime_Cur)
                {
                    BarricateLeft++;
                    BarricateText.text = BarricateLeft.ToString();
                    ObstacleCooltime_Cur = 0;
                }
            }
        }
        else
        {
            if (BarricateLeft < 2)
            {
                ObstacleCooltime_Cur += Time.deltaTime;
                if (ObstacleCooltime <= ObstacleCooltime_Cur)
                {
                    BarricateLeft++;
                    BarricateText.text = BarricateLeft.ToString();
                    ObstacleCooltime_Cur = 0;
                }
            }
        }
        if (!BridgeAble && BridgeCooltime <= BridgeCooltime_Cur)
        {
            BridgeAble = true;
        }

        if (!ObstacleAble && BarricateLeft > 0)
        {
            ObstacleAble = true;
        }

        people_temp = 0;
        for (int i = 0; i < Unit1.Count; i++)
        {
            people_temp += Unit1[i].GetComponent<Unit>().Amount;
        }
        people[1] = people_temp;
        people_temp = 0;
        for (int i = 0; i < Unit2.Count; i++)
        {
            people_temp += Unit2[i].GetComponent<Unit>().Amount;
        }
        people[2] = people_temp;
        people_temp = 0;
        for (int i = 0; i < Unit3.Count; i++)
        {
            people_temp += Unit3[i].GetComponent<Unit>().Amount;
        }
        people[3] = people_temp;
        people_temp = 0;
        for (int i = 0; i < Unit4.Count; i++)
        {
            people_temp += Unit4[i].GetComponent<Unit>().Amount;
        }
        people[4] = people_temp;

        for(int i=1;i<=4;i++)
        {
            if (people[i] < 0)
                people[i] = 0;
        }


        if (people[1] == 0 && NodeNumber[1] == 0 && !DefeatPanel.activeSelf && !VictoryPanel.activeSelf)
        {
            DefeatGame();
        }
        if (IsDead[1] && IsDead[2] && IsDead[3] && !VictoryPanel.activeSelf && !DefeatPanel.activeSelf)
        {
            UnitNumText.text = people[1].ToString();
            FoodNumText.text = money[1].ToString();

            string timeStr;
            int min = 0;
            float timer;


            min = (int)TimeForText / 60;
            timer = TimeForText - 60 * min;

            if (DataManager.instance.IsHard[DataManager.instance.LastStage])
            {
                if (TimeForText < DataManager.instance.StageScore[12 + DataManager.instance.LastStage] || DataManager.instance.StageScore[12 + DataManager.instance.LastStage] == 0)
                {
                    DataManager.instance.StageScore[12 + DataManager.instance.LastStage] = TimeForText;
                }
            }
            else
            {
                if (TimeForText < DataManager.instance.StageScore[DataManager.instance.LastStage] || DataManager.instance.StageScore[DataManager.instance.LastStage] == 0)
                {
                    DataManager.instance.StageScore[DataManager.instance.LastStage] = TimeForText;
                }
            }

            timeStr = min.ToString("00") + ":" + timer.ToString("00.00");
            timeStr = timeStr.Replace(".", ":");
            ScoreText.text = timeStr;


            if (DataManager.instance.IsHard[DataManager.instance.LastStage])
            {
                if (TimeForText < DataManager.instance.StarHardTimer[DataManager.instance.LastStage, 2])
                {
                    if (DataManager.instance.StageHardClearStar[DataManager.instance.LastStage]<3)
                    {
                        DataManager.instance.StageHardClearStar[DataManager.instance.LastStage] = 3;
                    }
                    StartCoroutine(StarAppear(3));
                }
                else if (TimeForText < DataManager.instance.StarHardTimer[DataManager.instance.LastStage, 1])
                {
                    if (DataManager.instance.StageHardClearStar[DataManager.instance.LastStage]<2)
                    {
                        DataManager.instance.StageHardClearStar[DataManager.instance.LastStage] = 2;
                    }
                    StartCoroutine(StarAppear(2));
                }
                else
                {
                    if (DataManager.instance.StageHardClearStar[DataManager.instance.LastStage] < 1)
                    {
                        DataManager.instance.StageHardClearStar[DataManager.instance.LastStage] = 1;
                    }
                    StartCoroutine(StarAppear(1));
                }
            }
            else
            {
                if (TimeForText < DataManager.instance.StarTimer[DataManager.instance.LastStage, 2])
                {
                    if (DataManager.instance.StageClearStar[DataManager.instance.LastStage] < 3)
                    {
                        DataManager.instance.StageClearStar[DataManager.instance.LastStage] = 3;
                    }
                    StartCoroutine(StarAppear(3));
                }
                else if (TimeForText < DataManager.instance.StarTimer[DataManager.instance.LastStage, 1])
                {
                    if (DataManager.instance.StageClearStar[DataManager.instance.LastStage] < 2)
                    {
                        DataManager.instance.StageClearStar[DataManager.instance.LastStage] = 2;
                    }
                    StartCoroutine(StarAppear(2));
                }
                else
                {
                    if (DataManager.instance.StageClearStar[DataManager.instance.LastStage] < 1)
                    {
                        DataManager.instance.StageClearStar[DataManager.instance.LastStage] = 1;
                    }
                    StartCoroutine(StarAppear(1));
                }
            }

            if (DataManager.instance.LastStage != 0)
                BGMManager.instance.Play(2);
            SoundManager.instance.Play(6);

            
            VictoryPanel.SetActive(true);

            if (people[0] + people[1] + people[2] + people[3] != 0)
            {
                DataManager.instance.StageClear[DataManager.instance.LastStage] = true;
            }

            if(DataManager.instance.LastStage!=0)
                DataManager.instance.LastStage++;


            DataManager.instance.SaveData();
            
            Time.timeScale = 1.0f;
            this.GetComponent<Click>().enabled = false;
        }
        else
        {

        }
        biggest_people_temp = 0;

        if (!VictoryPanel.activeSelf || !DefeatPanel.activeSelf)
        {
            GraphTimer += Time.deltaTime;
            if (GraphTimer > 0.25f)
            {
                GraphTimer = 0;
                for (int i = 0; i < 4; i++)
                {
                    Amounttext[i].text = people[i + 1].ToString();
                    Tiletext[i].text = Mapnumber[i + 1].ToString();
                    Foodtext[i].text = money[i + 1].ToString();
                }
            }
            for (int i = 0; i < 4; i++)
            {
                if (biggest_people_temp < people[i + 1])
                    biggest_people_temp = people[i + 1];

                if (people[i + 1] == 0 && NodeNumber[i + 1] == 0)
                {
                    money[i + 1] = 0;
                    IsDead[i] = true;
                    NoFoodIcon[i].SetActive(false);
                }
                else
                {
                    if(money[i+1] == 0)
                    {
                        Foodtext[i].color = Color.red;
                        Nofood[i] = true;
                        NoFoodIcon[i].SetActive(true);
                    }
                    else if(money[i+1]<1000 && Nofood[i])
                    {
                        ;
                    }
                    else if(money[i+1]>=1000 && Nofood[i])
                    {
                        Foodtext[i].color = Color.white;
                        Nofood[i] = false;
                        NoFoodIcon[i].SetActive(false);
                    }
                }
                if (Mapnumber[i + 1] / (float)NumMapTile * 100 > 70 && !IsUnitFaster)
                {
                    IsUnitFaster = true;

                    if (i == 0)
                    {
                        if (DataManager.instance.SelectedChar == 1)
                        {
                            if (DataManager.instance.IsKorean)
                            {
                                InfoTexting("<color=#395bb7>Direk 진영</color>이 70% 의 영토를 획득하여 이동속도가 빨라집니다");
                            }
                            else
                            {
                                InfoTexting("<color=#395bb7>Direk</color> got 70% of territory, speeding up its movement");
                            }
                        }
                        if (DataManager.instance.SelectedChar == 2)
                        {
                            if (DataManager.instance.IsKorean)
                            {
                                InfoTexting("<color=#1e824c>Valk 진영</color>이 70% 의 영토를 획득하여 이동속도가 빨라집니다.");
                            }
                            else
                            {
                                InfoTexting("<color=#1e824c>Valk</color> got 70% of territory, speeding up its movement");
                            }
                        }
                        else if (DataManager.instance.SelectedChar == 3)
                        {
                            if (DataManager.instance.IsKorean)
                            {
                                InfoTexting("<color=#ffc549>Solar 진영</color>이 70% 의 영토를 획득하여 이동속도가 빨라집니다.");
                            }
                            else
                            {
                                InfoTexting("<color=#ffc549>Solar</color> got 70% of territory, speeding up its movement");
                            }
                        }
                        else if (DataManager.instance.SelectedChar == 4)
                        {
                            if (DataManager.instance.IsKorean)
                            {
                                InfoTexting("<color=#c0392b>Punan 진영</color>이 70% 의 영토를 획득하여 이동속도가 빨라집니다.");
                            }
                            else
                            {
                                InfoTexting("<color=#c0392b>Punan</color> got 70% of territory, speeding up its movement");
                            }
                        }
                    }
                    else if (i == 1)
                    {
                        if (DataManager.instance.SelectedChar == 2)
                        {
                            if (DataManager.instance.IsKorean)
                            {
                                InfoTexting("<color=#395bb7>Direk 진영</color>이 70% 의 영토를 획득하여 이동속도가 빨라집니다.");
                            }
                            else
                            {
                                InfoTexting("<color=#395bb7>Direk</color> got 70% of territory, speeding up its movement");
                            }

                        }
                        else
                        {
                            if (DataManager.instance.IsKorean)
                            {
                                InfoTexting("<color=#1e824c>Valk 진영</color>이 70% 의 영토를 획득하여 이동속도가 빨라집니다.");
                            }
                            else
                            {
                                InfoTexting("<color=#1e824c>Valk</color> got 70% of territory, speeding up its movement");
                            }
                        }
                    }
                    else if (i == 2)
                    {
                        if (DataManager.instance.SelectedChar == 3)
                        {
                            if (DataManager.instance.IsKorean)
                            {
                                InfoTexting("<color=#395bb7>Direk 진영</color>이 70% 의 영토를 획득하여 이동속도가 빨라집니다.");
                            }
                            else
                            {
                                InfoTexting("<color=#395bb7>Direk</color> got 70% of territory, speeding up its movement");
                            }
                        }
                        else
                        {
                            if (DataManager.instance.IsKorean)
                            {
                                InfoTexting("<color=#ffc549>Solar 진영</color>이 70% 의 영토를 획득하여 이동속도가 빨라집니다.");
                            }
                            else
                            {
                                InfoTexting("<color=#ffc549>Solar</color> got 70% of territory, speeding up its movement");
                            }
                        }
                    }
                    else
                    {
                        if (DataManager.instance.SelectedChar == 4)
                        {
                            if (DataManager.instance.IsKorean)
                            {
                                InfoTexting("<color=#395bb7>Direk 진영</color>이 70% 의 영토를 획득하여 이동속도가 빨라집니다.");
                            }
                            else
                            {
                                InfoTexting("<color=#395bb7>Direk</color> got 70% of territory, speeding up its movement");
                            }
                        }
                        else
                        {
                            if (DataManager.instance.IsKorean)
                            {
                                InfoTexting("<color=#c0392b>Punan 진영</color>이 70% 의 영토를 획득하여 이동속도가 빨라집니다.");
                            }
                            else
                            {
                                InfoTexting("<color=#c0392b>Punan</color> got 70% of territory, speeding up its movement");
                            }
                        }
                    }
                }
            }
        }
        Biggest_people = biggest_people_temp;
        for (int i = 0; i < PeopleGraph.Length; i++)
        {
            PeopleGraph[i].value = people[i + 1] / (float)Biggest_people;
        }
        /*for(int i=1;i<5;i++)
        {
            if (IsNoMoney[i])
                MostBigPeople(i).GetComponent<Unit>().Amount -= Mathf.CeilToInt(MostBigPeople(i).GetComponent<Unit>().Amount * 0.003f);
        }*/


        Foodtimer += Time.deltaTime;
        if (Foodtimer >= 0.25f) //1초에 4씩 얻을수 있도록
        {
            Foodtimer = 0;
            for (int i = 1; i < 5; i++)
            {
                if (money[i] >= MoneyAmount)
                {
                    IsNoMoney[i] = false;
                }
                else
                {
                    IsNoMoney[i] = true;
                }
                if (IsNoMoney[i])
                {//가장 많은 유닛 줄이기
                    if (MostBigPeople(i).GetComponent<Unit>() != null)
                    {
                        MostBigPeople(i).GetComponent<Unit>().Amount -= Mathf.CeilToInt(MostBigPeople(i).GetComponent<Unit>().Amount * 0.03f);
                    }
                }
                money[i] += Mapnumber[i];
            }
        }
    }

    public bool IsBarrackOK(Vector3Int Cur)
    {
        Vector3Int vec3;
        vec3 = Cur;

        vec3.x += mapsize / 2;
        vec3.y += mapsize / 2;

        if (TileMaster[vec3.x + 1, vec3.y + 1] > 0 && TileMaster[vec3.x + 1, vec3.y + 1] <= 4)
            return false;
        if (TileMaster[vec3.x + 1, vec3.y] > 0 && TileMaster[vec3.x + 1, vec3.y] <= 4)
            return false;
        if (TileMaster[vec3.x + 1, vec3.y - 1] > 0 && TileMaster[vec3.x + 1, vec3.y - 1] <= 4)
            return false;
        if (TileMaster[vec3.x, vec3.y - 1] > 0 && TileMaster[vec3.x, vec3.y - 1] <= 4)
            return false;
        if (TileMaster[vec3.x - 1, vec3.y - 1] > 0 && TileMaster[vec3.x - 1, vec3.y - 1] <= 4)
            return false;
        if (TileMaster[vec3.x - 1, vec3.y] > 0 && TileMaster[vec3.x - 1, vec3.y] <= 4)
            return false;
        if (TileMaster[vec3.x - 1, vec3.y + 1] > 0 && TileMaster[vec3.x - 1, vec3.y + 1] <= 4)
            return false;
        if (TileMaster[vec3.x, vec3.y + 1] > 0 && TileMaster[vec3.x, vec3.y + 1] <= 4)
            return false;
        if (TileMaster[vec3.x, vec3.y] > 0 && TileMaster[vec3.x, vec3.y] <= 4)
            return false;
        if (TileMaster[vec3.x + 2, vec3.y] > 0 && TileMaster[vec3.x + 2, vec3.y] <= 4)
            return false;
        if (TileMaster[vec3.x, vec3.y + 2] > 0 && TileMaster[vec3.x, vec3.y + 2] <= 4)
            return false;
        if (TileMaster[vec3.x - 2, vec3.y] > 0 && TileMaster[vec3.x - 2, vec3.y] <= 4)
            return false;
        if (TileMaster[vec3.x, vec3.y - 2] > 0 && TileMaster[vec3.x, vec3.y - 2] <= 4)
            return false;



        if (
            //TileMaster[vec3.x + 1, vec3.y + 1] > 0 ||
            //TileMaster[vec3.x + 1, vec3.y] > 0 ||
            //TileMaster[vec3.x + 1, vec3.y - 1] > 0 ||
            //TileMaster[vec3.x, vec3.y - 1] > 0 ||
            //TileMaster[vec3.x - 1, vec3.y - 1] > 0 ||
            //TileMaster[vec3.x - 1, vec3.y] > 0 ||
            //TileMaster[vec3.x - 1, vec3.y + 1] > 0 ||
            //TileMaster[vec3.x, vec3.y + 1] > 0 ||
            //TileMaster[vec3.x, vec3.y] > 0||
            map.GetTile(Cur).name == "Dirt" ||
            map.GetTile(Cur).name == "BridgeLU" ||
            map.GetTile(Cur).name == "BridgeRU"
            )
            return false;
        return true;
    }

    public bool IsTileChangeOK(Vector3Int Cur)
    {
        if (
            map.GetTile(Cur).name == "Dirt" ||
            map.GetTile(Cur).name == "BridgeLU" ||
            map.GetTile(Cur).name == "BridgeRU"
            )
            return false;
        return true;
    }

    public bool IsTileColorOK(Vector3Int Cur)
    {
        if (
                map.GetTile(Cur).name == "BridgeLU" ||
                map.GetTile(Cur).name == "BridgeRU"
            )
            return false;
        return true;
    }

    public bool CanGo(Vector3Int pos)
    {
        if (map.GetTile(pos) == null || obstacle.GetTile(pos) != null || map.GetTile(pos).name == "Pillar")
        {
            return false;
        } //못가는 곳인경우 또는 이미 끝인경우
        return true;
    }
    public bool CanGo(Vector3Int Cur, Vector2Int pos)
    {
        if (MapMaster[pos.x + mapsize / 2, pos.y + mapsize / 2] == 6) return false; //못가는 곳인경우
        if (Mathf.Abs(MapZ[pos.x + mapsize / 2, pos.y + mapsize / 2] - Cur.z) > 1) return false; //1이상 차이나면 못감
        return true;
    }

    public bool IshereObstacle(Vector2Int pos)
    {
        if (TileMaster[pos.x + mapsize / 2, pos.y + mapsize / 2] == 5)
            return true;
        return false;
    }
    //public bool CanGoWithObstacle(Vector3Int pos)
    //{
    //    if (obstacle.GetTile(pos) != null || map.GetTile(pos) == null || map.GetTile(pos).name == "Pillar")
    //    {
    //        return false;
    //    } //못가는 곳인경우 또는 이미 끝인경우
    //    return true;
    //}

    public GameObject MostBigPeople(int master)
    {
        int max = 0;
        int temp = 0;
        GameObject ret = new GameObject();
        if (master == 1)
        {
            for (int i = 0; i < Unit1.Count; i++)
            {
                temp = Unit1[i].GetComponent<Unit>().Amount;
                if (max < temp)
                {
                    max = temp;
                    ret = Unit1[i];
                }
            }
        }
        else if (master == 2)
        {
            for (int i = 0; i < Unit2.Count; i++)
            {
                temp = Unit2[i].GetComponent<Unit>().Amount;
                if (max < temp)
                {
                    max = temp;
                    ret = Unit2[i];
                }
            }
        }
        else if (master == 3)
        {
            for (int i = 0; i < Unit3.Count; i++)
            {
                temp = Unit3[i].GetComponent<Unit>().Amount;
                if (max < temp)
                {
                    max = temp;
                    ret = Unit3[i];
                }
            }
        }
        else if (master == 4)
        {
            for (int i = 0; i < Unit4.Count; i++)
            {
                temp = Unit4[i].GetComponent<Unit>().Amount;
                if (max < temp)
                {
                    max = temp;
                    ret = Unit4[i];
                }
            }
        }
        return ret;
    }

    public void InfoTexting(string _str)
    {
        InfoText.GetComponentInChildren<Text>().text = _str;
        InfoText.SetActive(true);
        StartCoroutine(Vanish(InfoText));
    }


    IEnumerator Vanish(GameObject _target)
    {
        Color _color = new Color();
        Color _color_post = new Color();
        bool no_color = false;

        if (_target.GetComponent<Image>() != null)
        {
            _color_post = _target.GetComponent<Image>().color;
            _color = _target.GetComponent<Image>().color;
        }
        else if (_target.GetComponent<SpriteRenderer>() != null)
        {
            _color_post = _target.GetComponent<SpriteRenderer>().color;
            _color = _target.GetComponent<SpriteRenderer>().color;
        }
        else
            no_color = true;

        while (true && !no_color)
        {
            _color.a -= 0.003f;

            if (_target.GetComponent<Image>() != null)
                _target.GetComponent<Image>().color = _color;
            else if (_target.GetComponent<SpriteRenderer>() != null)
                _target.GetComponent<SpriteRenderer>().color = _color;

            if (_color.a <= 0)
                break;

            yield return null;
        }

        if (_target.GetComponent<Image>() != null)
            _target.GetComponent<Image>().color = _color_post;
        else if (_target.GetComponent<SpriteRenderer>() != null)
            _target.GetComponent<SpriteRenderer>().color = _color_post;

        _target.SetActive(false);
    }



    IEnumerator StarAppear(int starnum)
    {
        //GameObject[] starprefab = new GameObject[starnum];

        for (int i = 0; i < starnum; i++)
        {
            //starprefab[i] = Instantiate(prefabs[4], VictoryStars[i].transform.position,Quaternion.identity);

            BigStartAppear[i].SetActive(true);
            yield return new WaitForSeconds(0.5f);
            VictoryStars[i].SetActive(true);
        }
    }


    public void DefeatGame()
    {
        BGMManager.instance.Play(3);
        SoundManager.instance.Play(5);

        string timeStr;
        int min = 0;
        float timer;


        min = (int)TimeForText / 60;
        timer = TimeForText - 60 * min;

        timeStr = min.ToString("00") + ":" + timer.ToString("00.00");
        timeStr = timeStr.Replace(".", ":");
        DefeatScoreText.text = timeStr;

        DefeatPanel.SetActive(true);
        this.GetComponent<Click>().enabled = false;
    }
}
