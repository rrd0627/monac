using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Unit : MonoBehaviour
{
    private Vector2 dir;

    public Vector3Int Goal;

    public Vector3Int Next;

    public Vector3Int Cur;

    public Vector3Int Post;

    private Animator _ani;

    public int Master;

    private int Damage;

    public int Amount;

    public List<GameObject> Enemy;
    private int EnemyNum;
    private int EnemyNum_temp;

    private GameObject prefab_Barrack;
    private GameObject prefab_Fight;
    private GameObject prefab_Obstacle;
    private GameObject prefab_Triangle;

    private Color _color;

    private float timer;
    private float fightingtimer;
    private Vector3Int vec3_temp;
    private Vector3 vec3temp;

    public int UnitState;

    private Collider2D[] _col;

    private bool IsYChanged;

    //private List<GameObject> CharMerge;

    public bool IsOrder;

    public bool IsStop;

    public bool IsStand;

    public bool IsChase;

    public bool IsBridge;

    public bool IsMoveOrder;

    private Vector3Int BridgeLocation;

    public Vector3Int BridgePosition;

    public bool IsBarrigate;

    public bool IsCoroutine;

    public GameObject ChaseUnit;

    public GameObject BarrigateClicked;

    public GameObject SelectArrow;

    private GameObject MergeEffectPrefab;

    private GameObject MergeSmallEffectPrefab;

    private GameObject GhostEffectPrefab;
    
    private GameObject BuffEffectPrefab;

    private WaitForSeconds waittime;

    public GameObject[] Star;

    public GameObject EffectBar;

    private int HalfMapSize;

    public int UnitLevel;

    private const float ChaseSpeed = 1.2f;
    private const float LevelSpeed = 1.4f;

    private float ConquestSpeed = 1.3f;

    private float CharSpeed = 1f;    

    private float CharSorting = 2.5f;
    

    private const float SpearSpeed = 0.8f;
    private const float SpearAttack = 1f;
    private const float SpearArmor = 0.5f;   
    private const int SpearHeal = 2;

    private const float BowSpeed = 1.35f;    
    private const float BowAttack = 1.13f;
    private const float BowArmor = 1.3f;
    private const int BowHeal = 1;

    private const float SwordSpeed = 1f;
    private const float SwordAttack = 0.6f;
    private const float SwordArmor = 0.85f;
    private const int SwordHeal = 15;

    private const float MageSpeed = 1.04f;
    private const float MageAttack = 1.35f;
    private const float MageArmor = 1f;
    private const int MageHeal = 0;
    

    private int CharNum;

    private int CharUnitNum;

    enum ActState
    {
        run,
        stand,
        merge,
        attack,
        build,
        chase,
        barrigate,
        bridge,
    }

    void Start()
    {
        UnitLevel = 0;
        ConquestSpeed =1;
        CharSpeed = 1;
        prefab_Fight = null;
        UnitState = (int)ActState.run;
        waittime = new WaitForSeconds(0.1f);
        IsBarrigate = false; IsChase = false; IsBridge = false; IsCoroutine = false; IsStop = false; IsMoveOrder = false; IsOrder = false;
        //IsStand = false;
        vec3temp = new Vector3(0, 0.25f, 0);
        transform.position = GameManager.instance.map.CellToWorld(Cur)+Vector3.forward*CharSorting;
        transform.localScale = Vector3.one * 0.6f;
        Goal = Cur;

        _ani = GetComponent<Animator>();
        if(Amount==0)
            Amount = 100;
        CharUnitNum = Amount/100;

        HalfMapSize = GameManager.instance.mapsize / 2;
        GameManager.instance.UnitPos[Cur.x + HalfMapSize, Cur.y + HalfMapSize] = this.gameObject;
                

        if (Master == 1)
        {
            CharNum = DataManager.instance.SelectedChar;
            if (DataManager.instance.SelectedChar == 1)
            {
                CharSpeed = SpearSpeed;
                this.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animation/Blue") as RuntimeAnimatorController;
                _color = DataManager.instance.color1;
            }                
            else if (DataManager.instance.SelectedChar == 2)
            {
                CharSpeed = BowSpeed;
                this.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animation/Green") as RuntimeAnimatorController;
                _color = DataManager.instance.color2;
            }                
            else if (DataManager.instance.SelectedChar == 3)
            {
                CharSpeed = SwordSpeed;

                this.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animation/Yellow") as RuntimeAnimatorController;

                _color = DataManager.instance.color3;
            }                
            else
            {
                CharSpeed = MageSpeed;

                this.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animation/Red") as RuntimeAnimatorController;

                _color = DataManager.instance.color4;
            }
                
        }
        else if (Master == 2)
        {
            CharNum = 2;
            CharSpeed = BowSpeed;

            _color = DataManager.instance.color2;
            this.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animation/Green") as RuntimeAnimatorController;
            if (DataManager.instance.SelectedChar == 2)
            {
                CharSpeed = SpearSpeed;

                CharNum = 1;
                _color = DataManager.instance.color1;
                this.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animation/Blue") as RuntimeAnimatorController;
            }
        }
        else if (Master == 3)
        {
            CharSpeed = SwordSpeed;

            CharNum = 3;
            _color = DataManager.instance.color3;
            this.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animation/Yellow") as RuntimeAnimatorController;
            if (DataManager.instance.SelectedChar == 3)
            {
                CharSpeed = SpearSpeed;

                CharNum = 1;
                _color = DataManager.instance.color1;
                this.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animation/Blue") as RuntimeAnimatorController;
            }
        }
        else if (Master == 4)
        {
            CharSpeed = MageSpeed;

            CharNum = 4;
            _color = DataManager.instance.color4;
            this.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animation/Red") as RuntimeAnimatorController;
            if (DataManager.instance.SelectedChar == 4)
            {
                CharSpeed = SpearSpeed;

                CharNum = 1;
                _color = DataManager.instance.color1;
                this.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animation/Blue") as RuntimeAnimatorController;
            }
        }

        if (!GameManager.instance.IsGameStart)
        {
            switch (Master)
            {
                case 1:
                    GameManager.instance.Unit1.Add(this.gameObject);

                    prefab_Triangle = GameManager.instance.TrianglePrefabPooling();
                    prefab_Triangle.transform.position = transform.position + 1.2f * Vector3.up;
                    prefab_Triangle.SetActive(true);
                    prefab_Triangle.GetComponent<SpriteRenderer>().color = _color;                   

                    break;
                case 2:
                    GameManager.instance.Unit2.Add(this.gameObject);
                    break;
                case 3:
                    GameManager.instance.Unit3.Add(this.gameObject);
                    break;
                case 4:
                    GameManager.instance.Unit4.Add(this.gameObject);
                    break;
            }
        }

        //GetComponent<SpriteRenderer>().color = _color;

        this.GetComponent<PolygonCollider2D>().enabled = true;
        //transform.Translate(vec3temp * Cur.z);
        StartCoroutine(Move(Cur));
    }

    private void OnEnable()
    {
        if (!GameManager.instance.IsGameStart) return;

        switch (Master)
        {
            case 1:
                GameManager.instance.Unit1.Add(this.gameObject);
                break;
            case 2:
                GameManager.instance.Unit2.Add(this.gameObject);
                break;
            case 3:
                GameManager.instance.Unit3.Add(this.gameObject);
                break;
            case 4:
                GameManager.instance.Unit4.Add(this.gameObject);
                break;
        }

        UnitLevel = 0;        
        Star[0].SetActive(false);
        Star[1].SetActive(false);
        Star[2].SetActive(false);

        prefab_Fight = null;
        ConquestSpeed = 1;
        CharSpeed = 1;

        Enemy.RemoveRange(0, Enemy.Count);
        UnitState = (int)ActState.run;
        waittime = new WaitForSeconds(0.1f);
        IsBarrigate = false; IsStand = false; IsChase = false; IsBridge = false; IsCoroutine = false; IsStop = false; IsMoveOrder = false; IsOrder = false;
        vec3temp = new Vector3(0, 0.25f, 0);
        transform.position = GameManager.instance.map.CellToWorld(Cur) + Vector3.forward * CharSorting;
        transform.localScale = Vector3.one * 0.6f;
        Goal = Cur;
        _ani = GetComponent<Animator>();
        Amount = 100;
        CharUnitNum = Amount / 100;

        HalfMapSize = GameManager.instance.mapsize / 2;
        GameManager.instance.UnitPos[Cur.x + HalfMapSize, Cur.y + HalfMapSize] = this.gameObject;

        if (Master == 1)
        {
            CharNum = DataManager.instance.SelectedChar;

            if (DataManager.instance.SelectedChar == 1)
            {
                this.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animation/Blue") as RuntimeAnimatorController;
                CharSpeed = SpearSpeed;

                _color = DataManager.instance.color1;
            }
            else if (DataManager.instance.SelectedChar == 2)
            {
                this.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animation/Green") as RuntimeAnimatorController;
                CharSpeed = BowSpeed;
                _color = DataManager.instance.color2;
            }
            else if (DataManager.instance.SelectedChar == 3)
            {
                this.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animation/Yellow") as RuntimeAnimatorController;
                CharSpeed = SwordSpeed;

                _color = DataManager.instance.color3;
            }
            else
            {
                this.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animation/Red") as RuntimeAnimatorController;
                CharSpeed = MageSpeed;

                _color = DataManager.instance.color4;
            }

        }
        else if (Master == 2)
        {
            CharNum = 2;
            _color = DataManager.instance.color2;
            CharSpeed = BowSpeed;

            this.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animation/Green") as RuntimeAnimatorController;
            if (DataManager.instance.SelectedChar == 2)
            {
                CharSpeed = SpearSpeed;

                CharNum = 1;
                _color = DataManager.instance.color1;
                this.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animation/Blue") as RuntimeAnimatorController;
            }
        }
        else if (Master == 3)
        {
            CharSpeed = SwordSpeed;

            CharNum = 3;
            _color = DataManager.instance.color3;
            this.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animation/Yellow") as RuntimeAnimatorController;
            if (DataManager.instance.SelectedChar == 3)
            {
                CharSpeed = SpearSpeed;

                CharNum = 1;
                _color = DataManager.instance.color1;
                this.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animation/Blue") as RuntimeAnimatorController;
            }
        }
        else if (Master == 4)
        {
            CharSpeed = MageSpeed;

            CharNum = 4;
            _color = DataManager.instance.color4;
            this.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animation/Red") as RuntimeAnimatorController;
            if (DataManager.instance.SelectedChar == 4)
            {
                CharSpeed = SpearSpeed;

                CharNum = 1;
                _color = DataManager.instance.color1;
                this.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animation/Blue") as RuntimeAnimatorController;
            }
        }

        //GetComponent<SpriteRenderer>().color = _color;
        //switch (Master)
        //{
        //    case 1:
        //        GameManager.instance.Unit1.Add(this.gameObject);
        //        CharMerge = GameManager.instance.TileChar;
        //        break;
        //    case 2:
        //        GameManager.instance.Unit2.Add(this.gameObject);
        //        CharMerge = GameManager.instance.TileChar1;
        //        break;
        //    case 3:
        //        GameManager.instance.Unit3.Add(this.gameObject);
        //        CharMerge = GameManager.instance.TileChar2;
        //        break;
        //    case 4:
        //        GameManager.instance.Unit4.Add(this.gameObject);
        //        CharMerge = GameManager.instance.TileChar3;
        //        break;
        //}
        this.GetComponent<PolygonCollider2D>().enabled = true;
        //transform.Translate(vec3temp * Cur.z);
        //StartCoroutine(Move(Cur));
    }

    private void OnDisable()
    {
        UnitLevel = 0;

        if (BuffEffectPrefab != null)
            Destroy(BuffEffectPrefab);

        StopAllCoroutines();
    }

    void Update()
    {
        if (Amount <= 0)
        {
            switch (Master)
            {
                case 1:
                    GameManager.instance.Unit1.Remove(this.gameObject);
                    break;
                case 2:
                    GameManager.instance.Unit2.Remove(this.gameObject);
                    break;
                case 3:
                    GameManager.instance.Unit3.Remove(this.gameObject);
                    break;
                case 4:
                    GameManager.instance.Unit4.Remove(this.gameObject);
                    break;
            }
            //CharMerge.Remove(this.gameObject);
            //Destroy(this.gameObject);

            GhostEffect();
            if(Master==1)
                SoundManager.instance.Play(4);

            _col = Physics2D.OverlapCircleAll(transform.position, 2);
            for (int i = 0; i < _col.Length; i++)
            {
                if (_col[i].CompareTag("Unit") && _col[i].gameObject != this.gameObject)
                {
                    if (_col[i].GetComponent<Unit>().Enemy.Contains(this.gameObject))
                    {
                        _col[i].GetComponent<Unit>().Enemy.Remove(this.gameObject);
                    }
                }
            }
            this.gameObject.SetActive(false);
        }
        else if(Amount<500)
        {
            //transform.localScale = Vector3.one * 0.6f;
        }
        else if(Amount<2000)
        {
            if(UnitLevel==0)
            {
                UnitLevel = 1;
                MergeEffect();
                Star[0].SetActive(true);
            }
            //transform.localScale = Vector3.one * 0.72f;
        }
        else if(Amount<5000)
        {
            if (UnitLevel <= 1)
            {
                UnitLevel = 2;
                MergeEffect();
                Star[0].SetActive(false);
                Star[1].SetActive(true);
                BuffEffectPrefab = Instantiate(GameManager.instance.prefabs[7], transform);
            }
            //transform.localScale = Vector3.one * 0.78f;
        }
        else
        {
            if(UnitLevel <=2)
            {
                UnitLevel = 3;
                MergeEffect();
                Star[0].SetActive(false);
                Star[1].SetActive(false);
                Star[2].SetActive(true);
                if (BuffEffectPrefab == null)
                {
                    BuffEffectPrefab = Instantiate(GameManager.instance.prefabs[7], transform);
                }
                BuffEffectPrefab.transform.localScale *= 1.5f;
                ParticleSystem.MainModule ps = BuffEffectPrefab.GetComponentInChildren<ParticleSystem>().main;
                ps.startColor = new Color(239/255f,66/255f,162/255f);
            }            
        }

        if(UnitLevel == 0)
        {
            CharSorting = 2.5f;
            transform.localScale = 0.6f * Vector3.one;
            //transform.localScale = 1f * Vector3.one;
        }
        else if(UnitLevel == 1)
        {
            CharSorting = 2.7f;
            transform.localScale = 0.72f * Vector3.one;
        }
        else if(UnitLevel == 2)
        {
            CharSorting = 2.9f;
            transform.localScale = 0.8f *Vector3.one;
        }
        else
        {
            CharSorting = 3.1f;
            transform.localScale = 1f *Vector3.one;
        }

        if (((float)GameManager.instance.Mapnumber[Master] / GameManager.instance.NumMapTile) * 100 > 70 && ConquestSpeed == 1)
        {
            ConquestSpeed = 1.3f;
        }
        else if(ConquestSpeed!=1.3f)
            ConquestSpeed = 1;

        timer += Time.deltaTime;
        if (timer >= 0.25f) // 0.25초마다 변하도록
        {
            if (CharNum == 1 && Amount < CharUnitNum * 100)
            {
                if (Amount + SpearHeal * (UnitLevel + 1) > CharUnitNum * 100)
                    Amount = CharUnitNum * 100;
                else
                {
                    Amount += SpearHeal * (UnitLevel+1);
                }
            }
            else if (CharNum == 2 && Amount < CharUnitNum * 100)
            {
                if (Amount + BowHeal * (UnitLevel + 1) > CharUnitNum * 100)
                    Amount = CharUnitNum * 100;
                else
                {
                    Amount += BowHeal * (UnitLevel + 1);
                }
            }
            else if (CharNum == 3 && Amount < CharUnitNum * 100)
            {
                if (Amount + SwordHeal * (UnitLevel + 1) > CharUnitNum * 100)
                    Amount = CharUnitNum * 100;
                else
                {
                    Amount += SwordHeal * (UnitLevel + 1);
                }
            }
            else if (CharNum == 4 && Amount < CharUnitNum * 100)
            {
                if (Amount + MageHeal * (UnitLevel + 1) > CharUnitNum * 100)
                    Amount = CharUnitNum * 100;
                else
                {
                    Amount += MageHeal * (UnitLevel + 1);
                }
            }

            timer = 0;
            
            if(Amount<1000)
            {
                if (GameManager.instance.money[Master] - (1 + Mathf.CeilToInt(Amount / 100f)) < 0) //돈부족할때
                {                    
                    GameManager.instance.money[Master] = 0;
                }
                else
                {
                    GameManager.instance.money[Master] -= 1 + Mathf.CeilToInt(Amount / 100f); //2에서 11
                }
            }
            else if(Amount<10000)
            {
                if(GameManager.instance.money[Master] - (2* Mathf.CeilToInt(Amount / 100f)) <0) 
                {
                    GameManager.instance.money[Master] = 0;
                }
                else
                {
                    GameManager.instance.money[Master] -= 2 * Mathf.CeilToInt(Amount / 100f);//20 에서 200

                }
            }
            else
            {
                if (GameManager.instance.money[Master] - (-100 + 5 * Mathf.CeilToInt(Amount / 100f)) < 0)
                {
                    GameManager.instance.money[Master] = 0;
                }
                else
                {
                    GameManager.instance.money[Master] -= -100 + 5 * Mathf.CeilToInt(Amount / 100f); //400 ~
                }
            }            
        }
        switch (UnitState)
        {
            case (int)ActState.stand:
                _ani.SetBool("Run", false);
                _ani.SetBool("Attack", false);
                if (IsBarrigate)
                {
                    UnitState = (int)ActState.barrigate;
                    break;
                }                    
                else if (IsStand)
                {
                    IsOrder = false;
                }
                else
                {
                    if(UnitLevel>=2)
                    {
                        if(IsStringEnemyAround())
                        {
                            break;
                        }
                        if(IsBarrackAround())
                        { 
                            break;
                        }
                        if(IsEnemyAround())
                        {
                            break;
                        }
                    }
                    else if(UnitLevel ==1)
                    {
                        if (IsEnemyAround())
                        {
                            break;
                        }
                        if (IsBarrackAround())
                        {
                            break;
                        }
                    }
                    else
                    {
                        if(IsAllyAround())
                        {
                            break;
                        }
                        if (IsEnemyAround())
                        {
                            break;
                        }
                        if (IsBarrackAround())
                        {
                            break;
                        }
                    }



                    //else if(IsBarrackNextTile())
                    //{
                    //    UnitState = (int)ActState.attack;
                    //    break;
                    //}
                    if (Goal == Cur)
                    {
                        Goal = ChooseGoal();
                    }
                    if (Goal == Cur) //주변에 점령할 것이 없는경우 또는 맨처음!
                    {
                        Goal = RandMove();
                        //if (CharMerge.Contains(this.gameObject))
                        //{
                        //    CharMerge.Remove(this.gameObject);
                        //}
                    }
                    //else
                    //{
                    //    if (!CharMerge.Contains(this.gameObject))
                    //        CharMerge.Add(this.gameObject);
                    //}
                }
                IsCoroutine = false;
                StopAllCoroutines();
                StartCoroutine(Move(Goal));
                break;

            /*case (int)ActState.merge:
                if (CharMerge.Count ==0)
                {                    
                    UnitState = (int)ActState.stand;
                    break;
                }
                if (CharMerge.Contains(this.gameObject))
                {
                    CharMerge.Remove(this.gameObject);
                }
                Goal = CharMerge[Random.Range(0, CharMerge.Count)].GetComponent<Unit>().Cur;
                IsCoroutine = false;
                StopAllCoroutines();
                StartCoroutine(Move(Goal));
                UnitState = (int)ActState.run;
                break;

                */
            case (int)ActState.run:
                if(Enemy.Count>0)
                    UnitState = (int)ActState.attack;

                if (!IsCoroutine && !IsStand)
                {
                    UnitState = (int)ActState.stand;
                }
                if(!IsCoroutine && IsStand)
                {
                    IsStop = true;
                    if(IsEnemyNextTile())
                    {
                        UnitState = (int)ActState.attack;
                    }                        
                }
                if(IsBarrigate && IsNextTile(Cur,Goal))
                {
                    if (GameManager.instance.TileMaster[Goal.x + HalfMapSize, Goal.y + HalfMapSize] == 5) //이미 완성되어있으면
                    {
                        IsBarrigate = false;
                        Goal = Cur;
                        UnitState = (int)ActState.stand;
                    }
                }                    
                break;
            case (int)ActState.attack:                
                if (Time.timeScale == 0)
                    return;

                _ani.SetBool("Run", false);
                _ani.SetBool("Attack", true);

                EnemyNum_temp = 0;
                for (int i = Enemy.Count - 1; i >= 0; i--)
                {
                    if (!Enemy[i].activeInHierarchy)
                        Enemy.RemoveAt(i);
                    else
                    {
                        if(Enemy[i].GetComponent<Unit>()!=null)
                        {
                            //if (Vector3Int.Distance(Enemy[i].GetComponent<Unit>().Cur, Cur) >= 1)

                            if (Enemy[i].GetComponent<Unit>().Cur.z > Cur.z) //적이 나보다 높을때
                            {
                                if (Enemy[i].GetComponent<Unit>().CharNum == 1)
                                {
                                    EnemyNum_temp += (int)(Enemy[i].GetComponent<Unit>().Amount * 1.2f * SpearAttack);
                                }
                                else if (Enemy[i].GetComponent<Unit>().CharNum == 2)
                                {
                                    EnemyNum_temp += (int)(Enemy[i].GetComponent<Unit>().Amount * 1.2f * BowAttack);
                                }
                                else if (Enemy[i].GetComponent<Unit>().CharNum == 3)
                                {
                                    EnemyNum_temp += (int)(Enemy[i].GetComponent<Unit>().Amount * 1.2f * SwordAttack);
                                }
                                else
                                {
                                    EnemyNum_temp += (int)(Enemy[i].GetComponent<Unit>().Amount * 1.2f * MageAttack);
                                }
                            }
                            else
                            {
                                if (Enemy[i].GetComponent<Unit>().CharNum == 1)
                                {
                                    EnemyNum_temp += (int)(Enemy[i].GetComponent<Unit>().Amount * SpearAttack);
                                }
                                else if (Enemy[i].GetComponent<Unit>().CharNum == 2)
                                {
                                    EnemyNum_temp += (int)(Enemy[i].GetComponent<Unit>().Amount * BowAttack);
                                }
                                else if (Enemy[i].GetComponent<Unit>().CharNum == 3)
                                {
                                    EnemyNum_temp += (int)(Enemy[i].GetComponent<Unit>().Amount * SwordAttack);
                                }
                                else
                                {
                                    EnemyNum_temp += (int)(Enemy[i].GetComponent<Unit>().Amount * MageAttack);
                                }
                            }
                            if (i==0)
                            {
                                if (Enemy[0].GetComponent<Unit>()._ani.GetFloat("DirX") == -_ani.GetFloat("DirX") && Enemy[0].GetComponent<Unit>()._ani.GetFloat("DirY") == -_ani.GetFloat("DirY")) //적이랑 나랑 마주볼때
                                {
                                    //마주봄
                                }
                                else
                                {
                                    //내가 상대 옆을 때리는경우
                                    EnemyNum_temp = Mathf.CeilToInt(EnemyNum_temp * 0.3f);
                                }
                            }
                        }                       
                    }
                }
                if(CharNum == 1)
                {
                    EnemyNum = (int)(EnemyNum_temp * SpearArmor);
                }
                else if(CharNum == 2)
                {
                    EnemyNum = (int)(EnemyNum_temp * BowArmor);
                }
                else if(CharNum == 3)
                {
                    EnemyNum = (int)(EnemyNum_temp * SwordArmor);
                }
                else
                {
                    EnemyNum = (int)(EnemyNum_temp * MageArmor);
                }
                
                if (Enemy.Count==0)
                {
                    _ani.SetBool("Attack", false);
                    if(IsChase && ChaseUnit != this.gameObject)
                    {
                        IsCoroutine = false;
                        StopAllCoroutines();
                        StartCoroutine(Chase(ChaseUnit));
                    }
                    else
                    {
                        IsCoroutine = false;
                        StopAllCoroutines();
                        if (IsBarrigate)
                        {
                            UnitState = (int)ActState.barrigate;
                        }
                        else
                            StartCoroutine(Move(Goal));
                    }                        
                    break;
                }
                if(prefab_Fight==null)
                {
                    prefab_Fight = GameManager.instance.FightPrefabPooling();
                    prefab_Fight.transform.position = (transform.position + Enemy[0].transform.position) *0.5f;
                    prefab_Fight.SetActive(true);

                    Invoke("DestroyFightPrefab", 0.5f);

                    //prefab_Fight = Instantiate(GameManager.instance.prefabs[0], (transform.position + Enemy[0].transform.position)/2, Quaternion.identity);
                    //Destroy(prefab_Fight, 0.5f);
                }
                _ani.SetFloat("DirX", Enemy[0].transform.position.x - transform.position.x);
                _ani.SetFloat("DirY", Enemy[0].transform.position.y - transform.position.y);
                
                if (EnemyNum<1000)
                {
                    Damage = Mathf.CeilToInt(EnemyNum / 100f); //1부터 10까지
                }
                else if(EnemyNum<10000)
                {
                    Damage = -1 + Mathf.CeilToInt(EnemyNum / 75f); //13부터 ~ 134까지
                }
                else
                {
                    Damage = Mathf.CeilToInt(EnemyNum / 50f) - 49; //151부터 ~
                }

                if(Enemy.Count == 1)
                {
                    ;
                }
                else if (Enemy.Count == 2)
                {
                    Damage = Mathf.CeilToInt(Damage * 1.2f);
                }
                else if (Enemy.Count == 3)
                {
                    Damage = Mathf.CeilToInt(Damage * 1.5f);
                }
                else if(Enemy.Count >=4)
                {
                    Damage = Mathf.CeilToInt(Damage * 2f);
                }
                Amount -= Damage;
                
                if (Enemy[0].GetComponent<Node>() != null) //적이 배럭이면
                {
                    Enemy[0].GetComponent<Node>().HP-=2;
                    Amount--;
                }
                else if(Enemy[0].GetComponent<TreeAndBush>() != null) //적이 방해물이면
                {
                    Enemy[0].GetComponent<TreeAndBush>().HP--;
                    Amount--;
                }
                break;

            case (int)ActState.build:
                //가장 가까운 빌드 할수있는곳 찾아서 move하고 move 한뒤 그곳에 빌드 할수 있는지 체크하고 지음
                break;

            case (int)ActState.chase:
                if(!ChaseUnit.activeInHierarchy || ChaseUnit == this.gameObject)
                {
                    IsChase = false;
                    UnitState = (int)ActState.stand;
                    break;
                }
                IsCoroutine = false;
                StopAllCoroutines();
                StartCoroutine(Chase(ChaseUnit));
                //케릭터를 누르고 버튼을 눌러 그 캐릭터의 cur 위치로 move하도록 함
                break;
            case (int)ActState.barrigate:
                if(Goal==Cur)
                {
                    IsCoroutine = false;
                    StopAllCoroutines();
                    StartCoroutine(Move(Post));
                    UnitState = (int)ActState.run;
                    break;
                }
                if (IsNextTile(Cur,Goal)) //바로옆이면
                {
                    IsCoroutine = false;
                    StopAllCoroutines();
                    StartCoroutine(Move(Cur));

                    bool IsOktoMake = true;
                    if(GameManager.instance.TileMaster[Goal.x + HalfMapSize, Goal.y + HalfMapSize] == 0)
                    {
                        _col = Physics2D.OverlapCircleAll(transform.position, 3);
                        for (int i = 0; i < _col.Length; i++)
                        {
                            if (_col[i].CompareTag("Unit") && _col[i].gameObject != this.gameObject)
                            {
                                if(_col[i].GetComponent<Unit>().Cur == Goal || _col[i].GetComponent<Unit>().Next == Goal)
                                {
                                    IsOktoMake = false;
                                }
                            }
                        }                
                    }
                    else if(GameManager.instance.TileMaster[Goal.x + HalfMapSize, Goal.y + HalfMapSize] == 5 || GameManager.instance.BarricateLeft <= 0)
                    {//이미 그곳에 지었거나 더이상 남은게 없을때 원래대로 돌아가기
                        Goal = Cur;
                        IsBarrigate = false;
                        UnitState = (int)ActState.stand;
                        break;
                    }
                    else if(GameManager.instance.TileMaster[Goal.x + HalfMapSize, Goal.y + HalfMapSize] >=2 && GameManager.instance.TileMaster[Goal.x + HalfMapSize, Goal.y + HalfMapSize] <= 4)
                    //상대배럭이 지어졌으면
                    {
                        IsCoroutine = false;
                        StopAllCoroutines();
                        StartCoroutine(Move(Goal));
                        UnitState = (int)ActState.run;
                        break;
                    }
                    else
                    {
                        IsOktoMake = false;
                        IsBarrigate = false;
                        UnitState = (int)ActState.stand;
                    }

                    IsCoroutine = false;
                    StopAllCoroutines();
                    _ani.SetBool("Run", false);
                    if (IsOktoMake && GameManager.instance.BarricateLeft > 0)
                    {
                        IsBarrigate = false;
                        //prefab_Obstacle = Instantiate(GameManager.instance.Obstacle, GameManager.instance.change.CellToWorld(Goal), Quaternion.identity);
                        prefab_Obstacle = GameManager.instance.BarricatePrefabPooling();
                        prefab_Obstacle.GetComponent<TreeAndBush>().Pos = Goal;
                        prefab_Obstacle.SetActive(true);
                        Next = Cur;
                        Goal = Cur;
                        _ani.SetFloat("DirX", prefab_Obstacle.transform.position.x - transform.position.x);
                        _ani.SetFloat("DirY", prefab_Obstacle.transform.position.y - transform.position.y);
                        this.transform.position = GameManager.instance.map.CellToWorld(Cur);
                        transform.position += Vector3.forward * (-transform.position.z + Cur.z + CharSorting);
                        //GameManager.instance.TileMaster[Goal.x + GameManager.instance.mapsize / 2, Goal.y + GameManager.instance.mapsize / 2] = 5;
                        //GameManager.instance.change.SetTile(Goal, _tile);
                        
                        if(GameManager.instance.BarricateLeft == 2)
                            GameManager.instance.ObstacleCooltime_Cur = 0;
                        GameManager.instance.ObstacleAble = false;

                        GameManager.instance.BarricateLeft--;
                        GameManager.instance.BarricateText.text = GameManager.instance.BarricateLeft.ToString();

                    }
                    UnitState = (int)ActState.stand;
                }
                else
                {
                    IsCoroutine = false;
                    StopAllCoroutines();
                    StartCoroutine(Move(Goal));
                    //UnitState = (int)ActState.run;
                }            
                break;
            case (int)ActState.bridge:
                //가장 가까운 빌드 할수있는곳 찾아서 move하고 move 한뒤 그곳에 빌드 할수 있는지 체크하고 지음

                if (Goal == Cur)
                {

                    float distance = Vector2.Distance(this.transform.position, GameManager.instance.map.CellToWorld(Goal));
                    if (distance < 0.1f)
                    {

                        if (GameManager.instance.BridgeLeft <= 0)
                        {
                            IsBridge = false;
                            UnitState = (int)ActState.stand;
                            break;
                        }
                        IsBridge = false;
                        Tile _tile;
                        if (BridgeLocation.x == Cur.x)
                        {
                            _tile = Resources.Load<Tile>("Tile/BridgeLU"); //지형에 따라 RU
                        }
                        else
                        {
                            _tile = Resources.Load<Tile>("Tile/BridgeRU"); //지형에 따라 RU
                        }
                        if(!IsObstacleThere(BridgeLocation))
                        {

                            GameManager.instance.map.SetTile(BridgeLocation, _tile);
                            GameManager.instance.MapMaster[BridgeLocation.x + HalfMapSize, BridgeLocation.y + HalfMapSize] = 7;
                            GameManager.instance.MapZ[BridgeLocation.x + HalfMapSize, BridgeLocation.y + HalfMapSize] = BridgeLocation.z;
                            GameManager.instance.BridgeCooltime_Cur = 0;
                            GameManager.instance.BridgeAble = false;

                            GameManager.instance.BridgeLeft--;
                            GameManager.instance.BridgeText.text = GameManager.instance.BridgeLeft.ToString();
                        }                        
                        UnitState = (int)ActState.stand;
                    }
                }
                else
                {
                    IsCoroutine = false;
                    StopAllCoroutines();
                    StartCoroutine(Move(Goal));
                    UnitState = (int)ActState.run;
                }
                break;
        }
    }
    private bool IsObstacleThere(Vector3Int pos)
    {
        bool ret = false;
        Vector3Int vec3;
        for(int i=0;i<=20;i++)
        {
            vec3 = pos;
            vec3.z = i;
            if(GameManager.instance.obstacle.GetTile(vec3)!=null||GameManager.instance.map.GetTile(vec3)!=null) 
            {
                ret = true;
                break;
            }
        }
        return ret;
    }
    public class Astar
    {
        public int f;
        public int h;        
        public int g;
        public Vector3Int pos;
        public Astar parent;


        public bool Compare(int x, int y)
        {
            // TODO: Handle x or y being null, or them not having names
            return x < y;
        }

        public Astar()
        {
            pos = Vector3Int.zero;
            parent = null;
            f = 0;
            g = 0;
            h = 0;
        }

        public Astar Clone()
        {
            Astar astar = new Astar();
            astar.pos = this.pos;
            astar.parent = this.parent;
            astar.f = this.f;
            astar.g = this.g;
            astar.h = this.h;
            return astar;
        }
    }
    List<Astar> neighbor = new List<Astar>();
    private List<Astar> GetNeighborTiles(Astar cur)
    {
        neighbor.Clear();
        Astar origin = new Astar();
        origin.parent = cur;
        origin.g = cur.g;

        Astar temp1 = origin.Clone();
        Astar temp2 = origin.Clone();
        Astar temp3 = origin.Clone();
        Astar temp4 = origin.Clone();
        /*
        Astar temp5 = origin.Clone();
        Astar temp6 = origin.Clone();
        Astar temp7 = origin.Clone();
        Astar temp8 = origin.Clone();

        Astar temp9 = origin.Clone();
        Astar temp10 = origin.Clone();
        Astar temp11 = origin.Clone();
        Astar temp12 = origin.Clone();
        */
        Vector3Int curpos = cur.pos;
        
        temp1.pos = curpos + Vector3Int.right;
        neighbor.Add(temp1);
        temp2.pos = curpos + Vector3Int.left;
        neighbor.Add(temp2);
        temp3.pos = curpos + Vector3Int.up;
        neighbor.Add(temp3);
        temp4.pos = curpos + Vector3Int.down;
        neighbor.Add(temp4);
        /*
        Vector3Int temp = new Vector3Int(0, 0, 1);

        temp5.pos = curpos + Vector3Int.right + temp;
        neighbors.Add(temp5);
        temp6.pos = curpos + Vector3Int.left + temp;
        neighbors.Add(temp6);
        temp7.pos = curpos + Vector3Int.up + temp;
        neighbors.Add(temp7);
        temp8.pos = curpos + Vector3Int.down + temp;
        neighbors.Add(temp8);

        temp9.pos = curpos + Vector3Int.right - temp;
        neighbors.Add(temp9);
        temp10.pos = curpos + Vector3Int.left - temp;
        neighbors.Add(temp10);
        temp11.pos = curpos + Vector3Int.up - temp;
        neighbors.Add(temp11);
        temp12.pos = curpos + Vector3Int.down - temp;
        neighbors.Add(temp12);
        */
        int rand;
        List<Astar> ret = new List<Astar>();
        while (neighbor.Count>0)
        {
            rand = Random.Range(0, neighbor.Count);
            ret.Add(neighbor[rand]);
            neighbor.RemoveAt(rand);
        }
        return ret;
        //return neighbors;
    }

    private Astar FindNextTile(List<Astar> openList)
    {
        if (openList.Count == 0) return null;

        Astar result = openList[0];

        foreach (Astar tile in openList)
        {
            if (tile.f < result.f) result = tile;
        }

        return result;

    }


    private List<Vector3Int> PathFinder(Vector3Int Start , Vector3Int Target) //걸을수 없는곳 나중에 추가
    {
        List<Vector3Int> path = new List<Vector3Int>();
        List<Astar> openList = new List<Astar>();
        List<Vector2Int> closedList = new List<Vector2Int>();
        Astar currentTile = new Astar();
        currentTile.pos = Start;
        List<Astar> neighbors;
        bool InOpenlist;
        //if (!GameManager.instance.CanGo(Target)) return null; //누른 위치가 못가는 곳인경우
        if (GameManager.instance.MapMaster[Target.x + HalfMapSize, Target.y + HalfMapSize] == 6) return null; //누른 위치가 못가는 곳인경우
        else
        {
            while (true)
            {
                neighbors = GetNeighborTiles(currentTile);
                for (int i = 0; i < neighbors.Count; i++)
                {
                    //if (!GameManager.instance.CanGo(neighbors[i].pos)) continue; //못가는 곳인경우 또는 이미 끝인경우
                    if (GameManager.instance.MapMaster[neighbors[i].pos.x+HalfMapSize, neighbors[i].pos.y+HalfMapSize]==6) continue; //못가는 곳인경우
                    if (Mathf.Abs(GameManager.instance.MapZ[neighbors[i].pos.x + HalfMapSize, neighbors[i].pos.y + HalfMapSize] - currentTile.pos.z) > 1) continue; //1이상 차이나면 못감
                    if (closedList.Contains((Vector2Int)neighbors[i].pos) == true) continue; //이미 검색했던 곳이면

                    neighbors[i].pos.z = GameManager.instance.MapZ[neighbors[i].pos.x + HalfMapSize, neighbors[i].pos.y + HalfMapSize];

                    if (GameManager.instance.IshereObstacle((Vector2Int)neighbors[i].pos)) //거기에 장애물 있으면
                    {
                        //양이 충분하지 않고 Ai 면
                        if (GameManager.instance.ObstaclePos[neighbors[i].pos.x + HalfMapSize, neighbors[i].pos.y + HalfMapSize].GetComponent<TreeAndBush>().HP >= Amount && Master != 1)
                        {//넘어감
                            continue;
                        }
                        if (Master == 1 && neighbors[i].pos == Target && UnitState != (int)ActState.barrigate)
                        {//내 캐릭터인경우 그곳을 지정한 경우만 //부심
                            ;
                        }
                        else if (Master == 1)
                        { //그 외에는 넘어감
                            continue;
                        }
                    }

                    InOpenlist = false;
                    for (int j = 0; j < openList.Count; j++)
                    {
                        if (openList[j].pos == neighbors[i].pos)
                        {
                            InOpenlist = true;
                            break;
                        }
                    }

                    if (InOpenlist)
                    {
                        Astar oldParent = neighbors[i].parent;
                        int oldG = neighbors[i].g;
                        neighbors[i].parent = currentTile;
                        neighbors[i].g = currentTile.g + 1;
                        neighbors[i].h = Mathf.Abs(Target.x - neighbors[i].pos.x) + Mathf.Abs(Target.y - neighbors[i].pos.y);
                        neighbors[i].f = neighbors[i].g + neighbors[i].h;
                        if (neighbors[i].g >= oldG)
                        {
                            neighbors[i].parent = oldParent;
                            neighbors[i].g = oldParent.g + 1;
                            neighbors[i].h = Mathf.Abs(Target.x - neighbors[i].pos.x) + Mathf.Abs(Target.y - neighbors[i].pos.y);
                            neighbors[i].f = neighbors[i].g + neighbors[i].h;
                        }
                    }
                    else
                    {
                        neighbors[i].parent = currentTile;
                        neighbors[i].g = currentTile.g + 1;
                        neighbors[i].h = Mathf.Abs(Target.x - neighbors[i].pos.x) + Mathf.Abs(Target.y - neighbors[i].pos.y);
                        neighbors[i].f = neighbors[i].g + neighbors[i].h;
                        openList.Add(neighbors[i]);
                    }
                }

                closedList.Add((Vector2Int)currentTile.pos);

                openList.Remove(currentTile);

                if (closedList.Contains((Vector2Int)Target) == true)
                    break;

                currentTile = FindNextTile(openList);



                if (currentTile == null)
                {
                    UnitState = (int)ActState.run;
                    return null;
                }
            }
        }

        
        while (currentTile.pos != Start)
        {
            path.Add(currentTile.pos);
            currentTile = currentTile.parent;
        }
        path.Reverse();

        return path;
    }


    private List<Vector3Int> PathEnemyBarrackFinder(Vector3Int Start, Vector3Int Target) //걸을수 없는곳 나중에 추가
    {
        List<Vector3Int> path = new List<Vector3Int>();
        List<Astar> openList = new List<Astar>();
        List<Vector2Int> closedList = new List<Vector2Int>();
        Astar currentTile = new Astar();
        currentTile.pos = Start;
        List<Astar> neighbors;
        bool InOpenlist;
        //if (!GameManager.instance.CanGo(Target)) return null; //누른 위치가 못가는 곳인경우
        if (GameManager.instance.MapMaster[Target.x + HalfMapSize, Target.y + HalfMapSize] == 6) return null; //누른 위치가 못가는 곳인경우
        else
        {
            while (true)
            {
                neighbors = GetNeighborTiles(currentTile);
                for (int i = 0; i < neighbors.Count; i++)
                {
                    //if (!GameManager.instance.CanGo(neighbors[i].pos)) continue; //못가는 곳인경우 또는 이미 끝인경우
                    if (GameManager.instance.MapMaster[neighbors[i].pos.x + HalfMapSize, neighbors[i].pos.y + HalfMapSize] == 6) continue; //못가는 곳인경우
                    if (Mathf.Abs(GameManager.instance.MapZ[neighbors[i].pos.x + HalfMapSize, neighbors[i].pos.y + HalfMapSize] - currentTile.pos.z) > 1) continue; //1이상 차이나면 못감
                    if (closedList.Contains((Vector2Int)neighbors[i].pos) == true) continue; //이미 검색했던 곳이면

                    neighbors[i].pos.z = GameManager.instance.MapZ[neighbors[i].pos.x + HalfMapSize, neighbors[i].pos.y + HalfMapSize];

                    if (GameManager.instance.IshereObstacle((Vector2Int)neighbors[i].pos)) //거기에 장애물 있으면
                    {
                        //양이 충분하지 않고 Ai 면
                        if (GameManager.instance.ObstaclePos[neighbors[i].pos.x + HalfMapSize, neighbors[i].pos.y + HalfMapSize].GetComponent<TreeAndBush>().HP >= Amount && Master != 1)
                        {//넘어감
                            continue;
                        }
                        if (Master == 1 && neighbors[i].pos == Target && UnitState != (int)ActState.barrigate)
                        {//내 캐릭터인경우 그곳을 지정한 경우만 //부심
                            ;
                        }
                        else if (Master == 1)
                        { //그 외에는 넘어감
                            continue;
                        }
                    }

                    InOpenlist = false;
                    for (int j = 0; j < openList.Count; j++)
                    {
                        if (openList[j].pos == neighbors[i].pos)
                        {
                            InOpenlist = true;
                            break;
                        }
                    }

                    if (InOpenlist)
                    {
                        Astar oldParent = neighbors[i].parent;
                        int oldG = neighbors[i].g;
                        neighbors[i].parent = currentTile;
                        neighbors[i].g = currentTile.g + 1;
                        neighbors[i].h = Mathf.Abs(Target.x - neighbors[i].pos.x) + Mathf.Abs(Target.y - neighbors[i].pos.y);
                        neighbors[i].f = neighbors[i].g + neighbors[i].h;
                        if (neighbors[i].g >= oldG)
                        {
                            neighbors[i].parent = oldParent;
                            neighbors[i].g = oldParent.g + 1;
                            neighbors[i].h = Mathf.Abs(Target.x - neighbors[i].pos.x) + Mathf.Abs(Target.y - neighbors[i].pos.y);
                            neighbors[i].f = neighbors[i].g + neighbors[i].h;
                        }
                    }
                    else
                    {
                        neighbors[i].parent = currentTile;
                        neighbors[i].g = currentTile.g + 1;
                        neighbors[i].h = Mathf.Abs(Target.x - neighbors[i].pos.x) + Mathf.Abs(Target.y - neighbors[i].pos.y);
                        neighbors[i].f = neighbors[i].g + neighbors[i].h;
                        openList.Add(neighbors[i]);
                    }
                }

                closedList.Add((Vector2Int)currentTile.pos);

                openList.Remove(currentTile);

                if (closedList.Contains((Vector2Int)Target) == true)
                    break;

                currentTile = FindNextTile(openList);



                if (currentTile == null)
                {
                    UnitState = (int)ActState.run;
                    return null;
                }

                if (closedList.Count > 41)
                    return null;

            }
        }


        while (currentTile.pos != Start)
        {
            path.Add(currentTile.pos);
            currentTile = currentTile.parent;
        }
        path.Reverse();

        return path;
    }



    IEnumerator Chase(GameObject targetUnit)
    {
        IsStop = false;
        IsCoroutine = true;
        UnitState = (int)ActState.run;
        Vector2 pos;
        Vector2 dir;

        Vector3Int target;

        if (targetUnit.activeInHierarchy)
        {
            target = targetUnit.GetComponent<Unit>().Cur;
        }
        else
        {
            target = RandMove();
        }
        if (target == Cur)
        {
            target = RandMove();
        }
        Goal = target;

        

        float distance;
        float distance_temp;

        float _distance;
        Vector2 post_pos;
        List<Vector3Int> path = new List<Vector3Int>();//A STAR 를 통해 target까지 가는 vec3int  List

        path = PathFinder(Cur, target);

        dir = GameManager.instance.map.CellToWorld(Cur) - transform.position;
        dir.Normalize();
        distance = Vector2.Distance(this.transform.position, GameManager.instance.map.CellToWorld(Cur));
        distance_temp = 99999999;
        _ani.SetBool("Run", true);

        if (distance > 0.1f) //처음에 위치 맞추기
        {
            _ani.SetFloat("DirX", dir.x);
            _ani.SetFloat("DirY", dir.y);

            while (true)
            {
                if(UnitLevel==3)
                    this.transform.Translate(CharSpeed * ConquestSpeed * LevelSpeed * ChaseSpeed * dir * Time.deltaTime);                
                else
                    this.transform.Translate(CharSpeed * ConquestSpeed * ChaseSpeed * dir * Time.deltaTime);
                distance = Vector2.Distance(this.transform.position, GameManager.instance.map.CellToWorld(Cur));
                yield return null;

                if (distance > distance_temp || distance < 0.01f) //dist가 작아지다가 커지면
                    break;

                distance_temp = distance;
                post_pos = this.transform.position;
            }
        }
        Tile _tile = Resources.Load<Tile>("Tile/1");

        if (GameManager.instance.IsTileChangeOK(Cur))
            GameManager.instance.map.SetTile(Cur, _tile);
        if (GameManager.instance.IsTileColorOK(Cur))
        {
            GameManager.instance.Mapnumber[whosMap(GameManager.instance.map.GetColor(Cur))]--;
            GameManager.instance.Mapnumber[Master]++;

            GameManager.instance.map.SetTileFlags(Cur, TileFlags.None);
            GameManager.instance.map.SetColor(Cur, _color);
            GameManager.instance.MapMaster[Cur.x + HalfMapSize, Cur.y + HalfMapSize] = Master;
        }



        if (path == null)
        {
            if (IsChase)
            {
                SoundManager.instance.Play(7);

                GameManager.instance.InfoTexting("길이 없습니다");
            }
                

            Goal = ChooseGoal();

            IsBridge = false;
            IsBarrigate = false;

            target = Goal;

            path = PathFinder(Cur, target);

            IsChase = false;
            ChaseUnit = null;
            IsOrder = false;
            UnitState = (int)ActState.stand;
        }
        else if (path.Count == 0)
        {
            if (GameManager.instance.IsTileChangeOK(Cur))
                GameManager.instance.map.SetTile(Cur, _tile);
            if(GameManager.instance.IsTileColorOK(Cur))
            {
                GameManager.instance.Mapnumber[whosMap(GameManager.instance.map.GetColor(Cur))]--;
                GameManager.instance.Mapnumber[Master]++;

                GameManager.instance.map.SetTileFlags(Cur, TileFlags.None);
                GameManager.instance.map.SetColor(Cur, _color);
                GameManager.instance.MapMaster[Cur.x + HalfMapSize, Cur.y + HalfMapSize] = Master;
            }  

            transform.position += Vector3.forward * (-transform.position.z + Cur.z + CharSorting);

            dir = GameManager.instance.map.CellToWorld(Cur) - transform.position;
            dir.Normalize();

            distance_temp = 99999999;
            distance = Vector2.Distance(this.transform.position, GameManager.instance.map.CellToWorld(Cur));

            if (distance > 0.1f)
            {
                _ani.SetFloat("DirX", dir.x);
                _ani.SetFloat("DirY", dir.y);
            }
            while (true)
            {
                if (UnitLevel == 3)
                    this.transform.Translate(CharSpeed * ConquestSpeed * LevelSpeed * ChaseSpeed * dir * Time.deltaTime);
                else
                    this.transform.Translate(CharSpeed * ConquestSpeed * ChaseSpeed * dir * Time.deltaTime);
                distance = Vector2.Distance(this.transform.position, GameManager.instance.map.CellToWorld(Cur));
                yield return null;

                if (distance > distance_temp || distance < 0.01f) //dist가 작아지다가 커지면
                    break;

                distance_temp = distance;
                post_pos = this.transform.position;
            }
            UnitState = (int)ActState.chase;
        }
        else
        {
            Next = path[0];


            vec3_temp = path[0] - Cur;
            if (vec3_temp.x == 0 && vec3_temp.y == 1)
            {
                dir.x = -1f;
                dir.y = 0.5f;
            }
            else if (vec3_temp.x == 0 && vec3_temp.y == -1)
            {
                dir.x = 1f;
                dir.y = -0.5f;
            }
            else if (vec3_temp.x == 1 && vec3_temp.y == 0)
            {
                dir.x = 1f;
                dir.y = 0.5f;
            }
            else
            {
                dir.x = -1f;
                dir.y = -0.5f;
            }
            pos = GameManager.instance.map.CellToWorld(Next);
            dir.Normalize();

            _ani.SetFloat("DirX", dir.x);
            _ani.SetFloat("DirY", dir.y);
            distance_temp = 99999999;
            _distance = 0;
            post_pos = transform.position;
            IsYChanged = false;
            while (true)
            {
                if(UnitLevel==3)
                    this.transform.Translate(CharSpeed * ConquestSpeed * LevelSpeed * ChaseSpeed * dir * Time.deltaTime);                
                else
                    this.transform.Translate(CharSpeed * ConquestSpeed * ChaseSpeed * dir * Time.deltaTime);
                
                distance = Vector2.Distance(this.transform.position, pos);
                _distance += Vector2.Distance(this.transform.position, post_pos);
                yield return null;

                if (distance > distance_temp || distance < 0.01f) //dist가 작아지다가 커지면
                    break;

                if(!IsYChanged && _distance > 0.2f) //거의다 도착했을때
                {
                    IsYChanged = true;
                    transform.position += Vector3.up * (Next.z - Cur.z) * 0.25f;

                    if (Next.z - Cur.z < 0 && GameManager.instance.map.CellToWorld(Next).y - GameManager.instance.map.CellToWorld(Cur).y == 0)
                    {
                        transform.position += Vector3.forward * -0.5f;
                    }

                    transform.position += Vector3.forward * (Next.z - Cur.z);
                    distance_temp = 99999999;
                    Post = Cur;
                    Cur = Next;
                    continue;
                }
                distance_temp = distance;
                post_pos = this.transform.position;
            }
            Cur = Next;

            GameManager.instance.UnitPos[Post.x + HalfMapSize, Post.y + HalfMapSize]=null;
            GameManager.instance.UnitPos[Cur.x + HalfMapSize, Cur.y + HalfMapSize]=this.gameObject;

            this.transform.position = pos;
            transform.position += Vector3.forward * (-transform.position.z + Cur.z + CharSorting);
            if (GameManager.instance.IsTileChangeOK(Cur))
                GameManager.instance.map.SetTile(Cur, _tile);
            if (GameManager.instance.IsTileColorOK(Cur))
            {
                GameManager.instance.Mapnumber[whosMap(GameManager.instance.map.GetColor(Cur))]--;
                GameManager.instance.Mapnumber[Master]++;

                GameManager.instance.map.SetTileFlags(Cur, TileFlags.None);
                GameManager.instance.map.SetColor(Cur, _color);
                GameManager.instance.MapMaster[Cur.x + HalfMapSize, Cur.y + HalfMapSize] = Master;
            }
            UnitState = (int)ActState.chase;
        }

        yield return null;
        IsMoveOrder = false;
        IsCoroutine = false;
    }

    IEnumerator Move(Vector3Int target)
    {
        IsStop = false;
        IsCoroutine = true;
        UnitState = (int)ActState.run;
        _ani.SetBool("Attack", false);
        _ani.SetBool("Run", true);
        Vector2 pos;
        Vector2 dir;
        float distance;
        float distance_temp;

        float _distance;
        Vector2 post_pos;
        List<Vector3Int>path = new List<Vector3Int>();//A STAR 를 통해 target까지 가는 vec3int  List

        path = PathFinder(Cur, target);
        dir = GameManager.instance.map.CellToWorld(Cur) - transform.position;
        dir.Normalize();
        distance = Vector2.Distance(this.transform.position, GameManager.instance.map.CellToWorld(Cur));
        distance_temp = 99999999;

        Tile _tile = Resources.Load<Tile>("Tile/1");

        if (distance > 0.01f) //처음에 위치 맞추기
        {
            _ani.SetFloat("DirX", dir.x);
            _ani.SetFloat("DirY", dir.y);

            while (true)
            {
                if (UnitLevel == 3)
                    this.transform.Translate(CharSpeed * ConquestSpeed * LevelSpeed * dir * Time.deltaTime);
                else
                    this.transform.Translate(CharSpeed * ConquestSpeed * dir * Time.deltaTime);
                distance = Vector2.Distance(this.transform.position, GameManager.instance.map.CellToWorld(Cur));
                yield return null;

                if (distance > distance_temp || distance < 0.01f) //dist가 작아지다가 커지면
                    break;

                distance_temp = distance;
                post_pos = this.transform.position;
            }
            _ani.SetBool("Run", false);
        }

        if (GameManager.instance.IsTileChangeOK(Cur))
            GameManager.instance.map.SetTile(Cur, _tile);
        if (GameManager.instance.IsTileColorOK(Cur))
        {
            GameManager.instance.Mapnumber[whosMap(GameManager.instance.map.GetColor(Cur))]--;
            GameManager.instance.Mapnumber[Master]++;

            GameManager.instance.map.SetTileFlags(Cur, TileFlags.None);
            GameManager.instance.map.SetColor(Cur, _color);
            GameManager.instance.MapMaster[Cur.x + HalfMapSize, Cur.y + HalfMapSize] = Master;
        }

        if (path==null)
        {
            Goal = ChooseGoal();
            target = Goal;

            IsBridge = false;
            IsBarrigate = false;

            path = PathFinder(Cur, target);
            if(IsMoveOrder || IsBarrigate)
            {
                SoundManager.instance.Play(7);
                GameManager.instance.InfoTexting("길이 없습니다");
            }


            //갈수없습니다.
            IsBarrigate = false;
            IsBridge = false;
        }       
        else if (path.Count == 0) 
        {

            //if (GameManager.instance.IsTileChangeOK(Cur))
            //    GameManager.instance.map.SetTile(Cur, _tile);
            //if (GameManager.instance.IsTileColorOK(Cur))
            //{
            //    GameManager.instance.Mapnumber[whosMap(GameManager.instance.map.GetColor(Cur))]--;
            //    GameManager.instance.Mapnumber[Master]++;

            //    GameManager.instance.map.SetTileFlags(Cur, TileFlags.None);
            //    GameManager.instance.map.SetColor(Cur, _color);
            //}

            transform.position += Vector3.forward * (-transform.position.z + Cur.z + CharSorting);

            dir = GameManager.instance.map.CellToWorld(Cur) - transform.position;
            dir.Normalize();

            distance_temp = 99999999;
            distance = Vector2.Distance(this.transform.position, GameManager.instance.map.CellToWorld(Cur));

            if (distance > 0.1f)
            {
                _ani.SetBool("Run", true);
                _ani.SetFloat("DirX", dir.x);
                _ani.SetFloat("DirY", dir.y);
            }
            while (true)
            {
                if (distance > distance_temp || distance < 0.1f) //dist가 작아지다가 커지면
                    break;
                if (UnitLevel == 3)
                    this.transform.Translate(CharSpeed * ConquestSpeed * LevelSpeed * dir * Time.deltaTime);
                else
                    this.transform.Translate(CharSpeed * ConquestSpeed * dir * Time.deltaTime);
                distance = Vector2.Distance(this.transform.position, GameManager.instance.map.CellToWorld(Cur));
                yield return null;
                distance_temp = distance;
                post_pos = this.transform.position;
            }

            if (GameManager.instance.IsBarrackOK(Cur))//주변에 건물 있는지
            {
                _ani.SetBool("Run", false);
                yield return waittime;
                yield return waittime;
            }
            if (GameManager.instance.IsBarrackOK(Cur))//주변에 건물 있는지
            {
                //prefab_Barrack = Instantiate(GameManager.instance.Barrack, GameManager.instance.change.CellToWorld(Cur), Quaternion.identity);
                prefab_Barrack = GameManager.instance.BarrackPrefabPooling();
                prefab_Barrack.GetComponent<Node>().Master = Master;
                prefab_Barrack.GetComponent<Node>().Pos = Cur;
                prefab_Barrack.SetActive(true);
                GameManager.instance.TileMaster[Cur.x + HalfMapSize, Cur.y + HalfMapSize] = Master;
            }
            _ani.SetBool("Run", false);
        }
        else //path 긴경우
        {
            while (path.Count>0)
            {
                if (Master == 1 && GameManager.instance.MapMaster[path[0].x + HalfMapSize, path[0].y + HalfMapSize] == 5)
                {
                    if (!IsMoveOrder)
                        break;
                    if (path[0] != Goal) //중간에 걸리면
                    {
                        Next = Cur;
                        break;
                    }
                        
                }

                Next = path[0];

                    vec3_temp = path[0] - Cur;
                if (vec3_temp.x == 0 && vec3_temp.y == 1)
                {
                    dir.x = -1f;
                    dir.y = 0.5f;
                }
                else if (vec3_temp.x == 0 && vec3_temp.y == -1)
                {
                    dir.x = 1f;
                    dir.y = -0.5f;
                }
                else if (vec3_temp.x == 1 && vec3_temp.y == 0)
                {
                    dir.x = 1f;
                    dir.y = 0.5f;
                }
                else
                {
                    dir.x = -1f;
                    dir.y = -0.5f;
                }
                pos = GameManager.instance.map.CellToWorld(Next);
                dir.Normalize();
                _ani.SetBool("Run", true);
                _ani.SetFloat("DirX", dir.x);
                _ani.SetFloat("DirY", dir.y);
                distance_temp = 99999999;
                _distance = 0;
                post_pos = transform.position;
                IsYChanged = false;
                while (true)
                {
                    if (UnitLevel == 3)
                        this.transform.Translate(CharSpeed * ConquestSpeed * LevelSpeed * dir * Time.deltaTime);
                    else
                        this.transform.Translate(CharSpeed * ConquestSpeed * dir * Time.deltaTime);
                    distance = Vector2.Distance(this.transform.position, pos);
                    _distance += Vector2.Distance(this.transform.position, post_pos);
                    yield return null;

                    if (distance > distance_temp || distance < 0.01f) //dist가 작아지다가 커지면
                        break;

                    if (!IsYChanged && _distance > 0.2f) //거의다 도착했을때
                    {
                        IsYChanged = true;
                        transform.position += Vector3.up * (Next.z - Cur.z) * 0.25f;

                        if (Next.z - Cur.z < 0 && GameManager.instance.map.CellToWorld(Next).y - GameManager.instance.map.CellToWorld(Cur).y == 0)
                        {
                            transform.position += Vector3.forward * -0.5f;
                        }
                        transform.position += Vector3.forward * (Next.z - Cur.z);
                        Post = Cur;
                        Cur = Next;
                        continue;
                    }
                    distance_temp = distance;
                    post_pos = this.transform.position;
                }
                Cur = Next;

                GameManager.instance.UnitPos[Post.x + HalfMapSize, Post.y + HalfMapSize] = null;
                GameManager.instance.UnitPos[Cur.x + HalfMapSize, Cur.y + HalfMapSize] = this.gameObject;

                if (IsBarrigate)
                {
                    if (IsNextTile(Cur, Goal))
                    {
                        break;
                    }
                }

                path.RemoveAt(0);
                if (path.Count > 0)
                {
                    if (GameManager.instance.MapMaster[path[0].x + HalfMapSize, path[0].y + HalfMapSize] == 5 && !IsOrder && Master==1)
                    {
                        break;
                    }
                    Next = path[0];
                }
                this.transform.position = pos;
                transform.position += Vector3.forward * (-transform.position.z + Cur.z + CharSorting);
                if (GameManager.instance.IsTileChangeOK(Cur))
                    GameManager.instance.map.SetTile(Cur, _tile);
                if (GameManager.instance.IsTileColorOK(Cur))
                {
                    GameManager.instance.Mapnumber[whosMap(GameManager.instance.map.GetColor(Cur))]--;
                    GameManager.instance.Mapnumber[Master]++;

                    GameManager.instance.map.SetTileFlags(Cur, TileFlags.None);
                    GameManager.instance.map.SetColor(Cur, _color);
                    GameManager.instance.MapMaster[Cur.x + HalfMapSize, Cur.y + HalfMapSize] = Master;
                }                

                if (GameManager.instance.IsBarrackOK(Cur))
                {
                    _ani.SetBool("Run", false);
                    yield return waittime;
                    yield return waittime;
                }
                if (GameManager.instance.IsBarrackOK(Cur))//주변에 건물 있는지
                {
                    //prefab_Barrack = Instantiate(GameManager.instance.Barrack, GameManager.instance.change.CellToWorld(Cur), Quaternion.identity);

                    if(Master==1)
                        SoundManager.instance.Play(0);
                    prefab_Barrack = GameManager.instance.BarrackPrefabPooling();
                    prefab_Barrack.transform.position += Vector3.forward;
                    prefab_Barrack.GetComponent<Node>().Master = Master;
                    prefab_Barrack.GetComponent<Node>().Pos = Cur;
                    prefab_Barrack.SetActive(true);
                    GameManager.instance.TileMaster[Cur.x + HalfMapSize, Cur.y + HalfMapSize] = Master;
                }
            }         
        }
        //_ani.SetBool("Run", false);
        //싸움 끝난뒤
        if (IsStand && Goal ==Cur)
        {
            UnitState = (int)ActState.run;
            _ani.SetBool("Run", false);
            //IsStand = false;
        }
        else
        {
            UnitState = (int)ActState.stand;
        }
        if (IsChase)
        {
            UnitState = (int)ActState.chase;
        }
        else if (IsBarrigate)
        {
            UnitState = (int)ActState.barrigate;
        }
        else if (IsBridge)
        {
            UnitState = (int)ActState.bridge;
        }
        IsMoveOrder = false;
        yield return waittime;
        
        IsCoroutine = false;
    }

    /*
    private Vector3Int ChooseGoal()
    {
        Vector3Int vec3ret = Cur;
        Vector3Int vec3temp;

        List<Vector3Int> vec3list = new List<Vector3Int>();
        List<Vector3Int> visited = new List<Vector3Int>();

        Vector3Int temp = new Vector3Int(0, 0, 1);

        int count = 0;

        //넓혀가는식으로 가장 가까운 것 찾음
        vec3list.Add(Cur);
        while(vec3list.Count!=0)
        {
            vec3temp = vec3list[0];
            vec3list.RemoveAt(0);
            
            if (!GameManager.instance.CanGo(vec3temp))
                continue;
            if (visited.Contains(vec3temp) == true)
                continue;
            if (GameManager.instance.IshereObstacle(vec3temp)) //거기에 방해물있으면
            {
                //부실수 있으면
                if (GameManager.instance.ObstaclePos[vec3temp.x + HalfMapSize, vec3temp.y + HalfMapSize].GetComponent<TreeAndBush>().HP<=Amount && Master != 1) 
                {
                    ;
                }
                else //내가 죽으면
                    continue;
            }
            if (count++ > 41)
                break;

            if (GameManager.instance.map.GetColor(vec3temp) != _color)
            {
                if (GameManager.instance.map.GetTile(vec3temp).name == "BridgeLU" || GameManager.instance.map.GetTile(vec3temp).name == "BridgeRU")
                {
                    //다리인경우
                }
                else
                {
                    vec3ret = vec3temp;
                    break;
                }                
            }
            visited.Add(vec3temp);

            int rand = Random.Range(0, 4);

            switch(rand)
            {
                case 0:
                    vec3list.Add(vec3temp + Vector3Int.up);
                    vec3list.Add(vec3temp + Vector3Int.down);
                    vec3list.Add(vec3temp + Vector3Int.right);
                    vec3list.Add(vec3temp + Vector3Int.left);
                    vec3list.Add(vec3temp + Vector3Int.up + temp);
                    vec3list.Add(vec3temp + Vector3Int.down + temp);
                    vec3list.Add(vec3temp + Vector3Int.right + temp);
                    vec3list.Add(vec3temp + Vector3Int.left + temp);
                    vec3list.Add(vec3temp + Vector3Int.up - temp);
                    vec3list.Add(vec3temp + Vector3Int.down - temp);
                    vec3list.Add(vec3temp + Vector3Int.right - temp);
                    vec3list.Add(vec3temp + Vector3Int.left - temp);
                    break;
                case 1:
                    vec3list.Add(vec3temp + Vector3Int.down);
                    vec3list.Add(vec3temp + Vector3Int.right);
                    vec3list.Add(vec3temp + Vector3Int.left);
                    vec3list.Add(vec3temp + Vector3Int.up);
                    vec3list.Add(vec3temp + Vector3Int.down + temp);
                    vec3list.Add(vec3temp + Vector3Int.right + temp);
                    vec3list.Add(vec3temp + Vector3Int.left + temp);
                    vec3list.Add(vec3temp + Vector3Int.up + temp);
                    vec3list.Add(vec3temp + Vector3Int.down - temp);
                    vec3list.Add(vec3temp + Vector3Int.right - temp);
                    vec3list.Add(vec3temp + Vector3Int.left - temp);
                    vec3list.Add(vec3temp + Vector3Int.up - temp);                    
                    break;
                case 2:
                    vec3list.Add(vec3temp + Vector3Int.right);
                    vec3list.Add(vec3temp + Vector3Int.left);
                    vec3list.Add(vec3temp + Vector3Int.up);
                    vec3list.Add(vec3temp + Vector3Int.down);
                    vec3list.Add(vec3temp + Vector3Int.right + temp);
                    vec3list.Add(vec3temp + Vector3Int.left + temp);
                    vec3list.Add(vec3temp + Vector3Int.up + temp);
                    vec3list.Add(vec3temp + Vector3Int.down + temp);                    
                    vec3list.Add(vec3temp + Vector3Int.right - temp);
                    vec3list.Add(vec3temp + Vector3Int.left - temp);
                    vec3list.Add(vec3temp + Vector3Int.up - temp);
                    vec3list.Add(vec3temp + Vector3Int.down - temp);
                    break;
                case 3:                   
                    vec3list.Add(vec3temp + Vector3Int.left);
                    vec3list.Add(vec3temp + Vector3Int.up);
                    vec3list.Add(vec3temp + Vector3Int.down);
                    vec3list.Add(vec3temp + Vector3Int.right);                    
                    vec3list.Add(vec3temp + Vector3Int.left + temp);
                    vec3list.Add(vec3temp + Vector3Int.up + temp);
                    vec3list.Add(vec3temp + Vector3Int.down + temp);
                    vec3list.Add(vec3temp + Vector3Int.right + temp);                    
                    vec3list.Add(vec3temp + Vector3Int.left - temp);
                    vec3list.Add(vec3temp + Vector3Int.up - temp);
                    vec3list.Add(vec3temp + Vector3Int.down - temp);
                    vec3list.Add(vec3temp + Vector3Int.right - temp);
                    break;
            }            
        }
        return vec3ret;
    }
    */
    private Vector3Int ChooseGoal()
    {
        Vector3Int vec3ret = Cur;
        Vector3Int CharPostemp = Cur;
        Vector3Int Vec3Temp = new Vector3Int();
        List<Vector2Int> vec2list = new List<Vector2Int>();
        List<Vector2Int> visited = new List<Vector2Int>();
        Vector2Int vec2temp = new Vector2Int();
        int count = 0;        

        //넓혀가는식으로 가장 가까운 것 찾음
        vec2list.Add((Vector2Int)Cur);
        while (vec2list.Count != 0)
        {
            vec2temp = vec2list[0];
            vec2list.RemoveAt(0);

            if (GameManager.instance.MapMaster[vec2temp.x + HalfMapSize,vec2temp.y+ HalfMapSize] == 6) //못가는곳이면
                continue;
            if (visited.Contains(vec2temp) == true)
                continue;
            if (GameManager.instance.MapMaster[vec2temp.x + HalfMapSize, vec2temp.y + HalfMapSize] == 5) //거기에 방해물있으면
            {
                //부실수 있으면
                if (GameManager.instance.ObstaclePos[vec2temp.x + HalfMapSize, vec2temp.y + HalfMapSize].GetComponent<TreeAndBush>().HP < Amount && Master != 1)
                {
                    ;
                }
                else //내가 죽으면
                    continue;
            }
            /*if (count++ > 41)
                break;
                */

            count++;
            if(UnitLevel==0)
            {
                if (count > 25)
                {
                    if (CharPostemp != Cur)
                        vec3ret = CharPostemp;
                    break;
                }
            }
            else if(UnitLevel ==1)
            {
                if (count > 100)
                {
                    if (CharPostemp != Cur)
                        vec3ret = CharPostemp;
                    break;
                }
            }
            else if(UnitLevel ==2)
            {
                if (count > 150)
                {
                    if (CharPostemp != Cur)
                        vec3ret = CharPostemp;
                    break;
                }
            }
            else if(UnitLevel ==3)
            {
                if (count > 200)
                {
                    if (CharPostemp != Cur)
                        vec3ret = CharPostemp;
                    break;
                }
            }

            if(GameManager.instance.UnitPos[vec2temp.x + HalfMapSize, vec2temp.y + HalfMapSize]!=null)
            {
                for (int i = 20; i >= 0; i--)
                {
                    CharPostemp.x = vec2temp.x;
                    CharPostemp.y = vec2temp.y;
                    CharPostemp.z = i;
                    if (GameManager.instance.map.GetTile(CharPostemp) != null)
                    {
                        break;
                    }
                }
            }

            

            if (GameManager.instance.MapMaster[vec2temp.x + HalfMapSize, vec2temp.y + HalfMapSize] != Master) //다른색이면
            {
                if (GameManager.instance.MapMaster[vec2temp.x + HalfMapSize, vec2temp.y + HalfMapSize] == 7 && GameManager.instance.MapMaster[vec2temp.x + HalfMapSize, vec2temp.y + HalfMapSize] != 5)
                {
                    ;//다리인경우   장애물은 없는 경우
                }
                else
                {
                    for(int i=20;i>=0;i--)
                    {
                        Vec3Temp.x = vec2temp.x;
                        Vec3Temp.y = vec2temp.y;
                        Vec3Temp.z = i;
                        if (GameManager.instance.map.GetTile(Vec3Temp)!=null)
                        {
                            vec3ret = Vec3Temp;
                            break;
                        }
                    }
                    break;
                }
            }
            visited.Add(vec2temp);

            int rand = Random.Range(0, 4);

            switch (rand)
            {
                case 0:
                    if (Mathf.Abs(GameManager.instance.MapZ[vec2temp.x + HalfMapSize, vec2temp.y + HalfMapSize] - GameManager.instance.MapZ[vec2temp.x + HalfMapSize, vec2temp.y + HalfMapSize + 1] ) <= 1)
                        vec2list.Add(vec2temp + Vector2Int.up);
                    if (Mathf.Abs(GameManager.instance.MapZ[vec2temp.x + HalfMapSize, vec2temp.y + HalfMapSize] - GameManager.instance.MapZ[vec2temp.x + HalfMapSize, vec2temp.y + HalfMapSize - 1] ) <= 1)
                        vec2list.Add(vec2temp + Vector2Int.down);
                    if (Mathf.Abs(GameManager.instance.MapZ[vec2temp.x + HalfMapSize, vec2temp.y + HalfMapSize] - GameManager.instance.MapZ[vec2temp.x + HalfMapSize + 1, vec2temp.y + HalfMapSize]) <= 1)
                        vec2list.Add(vec2temp + Vector2Int.right);
                    if (Mathf.Abs(GameManager.instance.MapZ[vec2temp.x + HalfMapSize, vec2temp.y + HalfMapSize] - GameManager.instance.MapZ[vec2temp.x + HalfMapSize - 1, vec2temp.y + HalfMapSize]) <= 1)
                        vec2list.Add(vec2temp + Vector2Int.left);
                    break;
                case 1:
                    if (Mathf.Abs(GameManager.instance.MapZ[vec2temp.x + HalfMapSize, vec2temp.y + HalfMapSize] - GameManager.instance.MapZ[vec2temp.x + HalfMapSize, vec2temp.y + HalfMapSize - 1] ) <= 1)
                        vec2list.Add(vec2temp + Vector2Int.down);
                    if (Mathf.Abs(GameManager.instance.MapZ[vec2temp.x + HalfMapSize, vec2temp.y + HalfMapSize] - GameManager.instance.MapZ[vec2temp.x + HalfMapSize + 1, vec2temp.y + HalfMapSize]) <= 1)
                        vec2list.Add(vec2temp + Vector2Int.right);
                    if (Mathf.Abs(GameManager.instance.MapZ[vec2temp.x + HalfMapSize, vec2temp.y + HalfMapSize] - GameManager.instance.MapZ[vec2temp.x + HalfMapSize - 1, vec2temp.y + HalfMapSize]) <= 1)
                        vec2list.Add(vec2temp + Vector2Int.left);
                    if (Mathf.Abs(GameManager.instance.MapZ[vec2temp.x + HalfMapSize, vec2temp.y + HalfMapSize] - GameManager.instance.MapZ[vec2temp.x + HalfMapSize, vec2temp.y + HalfMapSize + 1] ) <= 1)
                        vec2list.Add(vec2temp + Vector2Int.up);
                    break;
                case 2:
                    if (Mathf.Abs(GameManager.instance.MapZ[vec2temp.x + HalfMapSize, vec2temp.y + HalfMapSize] - GameManager.instance.MapZ[vec2temp.x + HalfMapSize + 1, vec2temp.y + HalfMapSize]) <= 1)
                        vec2list.Add(vec2temp + Vector2Int.right);
                    if (Mathf.Abs(GameManager.instance.MapZ[vec2temp.x + HalfMapSize, vec2temp.y + HalfMapSize] - GameManager.instance.MapZ[vec2temp.x + HalfMapSize - 1, vec2temp.y + HalfMapSize]) <= 1)
                        vec2list.Add(vec2temp + Vector2Int.left);
                    if (Mathf.Abs(GameManager.instance.MapZ[vec2temp.x + HalfMapSize, vec2temp.y + HalfMapSize] - GameManager.instance.MapZ[vec2temp.x + HalfMapSize, vec2temp.y + HalfMapSize + 1] ) <= 1)
                        vec2list.Add(vec2temp + Vector2Int.up);
                    if (Mathf.Abs(GameManager.instance.MapZ[vec2temp.x + HalfMapSize, vec2temp.y + HalfMapSize] - GameManager.instance.MapZ[vec2temp.x + HalfMapSize, vec2temp.y + HalfMapSize - 1]) <= 1)
                        vec2list.Add(vec2temp + Vector2Int.down);
                    break;
                case 3:
                    if (Mathf.Abs(GameManager.instance.MapZ[vec2temp.x + HalfMapSize, vec2temp.y + HalfMapSize] - GameManager.instance.MapZ[vec2temp.x + HalfMapSize - 1, vec2temp.y + HalfMapSize]) <= 1)
                        vec2list.Add(vec2temp + Vector2Int.left);
                    if (Mathf.Abs(GameManager.instance.MapZ[vec2temp.x + HalfMapSize, vec2temp.y + HalfMapSize] - GameManager.instance.MapZ[vec2temp.x + HalfMapSize, vec2temp.y + HalfMapSize + 1] ) <= 1)
                        vec2list.Add(vec2temp + Vector2Int.up);
                    if (Mathf.Abs(GameManager.instance.MapZ[vec2temp.x + HalfMapSize, vec2temp.y + HalfMapSize] - GameManager.instance.MapZ[vec2temp.x + HalfMapSize, vec2temp.y + HalfMapSize - 1] ) <= 1)
                        vec2list.Add(vec2temp + Vector2Int.down);
                    if (Mathf.Abs(GameManager.instance.MapZ[vec2temp.x + HalfMapSize, vec2temp.y + HalfMapSize] - GameManager.instance.MapZ[vec2temp.x + HalfMapSize + 1, vec2temp.y + HalfMapSize]) <= 1)
                        vec2list.Add(vec2temp + Vector2Int.right);
                    break;
            }
        }
        return vec3ret;
    }

    List<Vector2Int> vec2list_Rand = new List<Vector2Int>();
    Vector3Int vec3IntTemp = new Vector3Int();
    public Vector3Int RandMove()
    {
        Vector3Int vec3ret = Cur;
        
        Vector2Int vec2temp = (Vector2Int)Cur;

        vec2list_Rand.Clear();

        //Vector3Int temp = new Vector3Int(0, 0, 1);

        int rand = Random.Range(0, 4);

        switch (rand)
        {
            case 0:
                vec2list_Rand.Add(vec2temp + Vector2Int.up);
                vec2list_Rand.Add(vec2temp + Vector2Int.down);
                vec2list_Rand.Add(vec2temp + Vector2Int.right);
                vec2list_Rand.Add(vec2temp + Vector2Int.left);
                //vec3list.Add(vec3temp + Vector3Int.up + temp);
                //vec3list.Add(vec3temp + Vector3Int.down + temp);
                //vec3list.Add(vec3temp + Vector3Int.right + temp);
                //vec3list.Add(vec3temp + Vector3Int.left + temp);
                //vec3list.Add(vec3temp + Vector3Int.up - temp);
                //vec3list.Add(vec3temp + Vector3Int.down - temp);
                //vec3list.Add(vec3temp + Vector3Int.right - temp);
                //vec3list.Add(vec3temp + Vector3Int.left - temp);
                break;
            case 1:
                vec2list_Rand.Add(vec2temp + Vector2Int.down);
                vec2list_Rand.Add(vec2temp + Vector2Int.right);
                vec2list_Rand.Add(vec2temp + Vector2Int.left);
                vec2list_Rand.Add(vec2temp + Vector2Int.up);
                //vec3list.Add(vec3temp + Vector3Int.down + temp);
                //vec3list.Add(vec3temp + Vector3Int.right + temp);
                //vec3list.Add(vec3temp + Vector3Int.left + temp);
                //vec3list.Add(vec3temp + Vector3Int.up + temp);
                //vec3list.Add(vec3temp + Vector3Int.down - temp);
                //vec3list.Add(vec3temp + Vector3Int.right - temp);
                //vec3list.Add(vec3temp + Vector3Int.left - temp);
                //vec3list.Add(vec3temp + Vector3Int.up - temp);
                break;
            case 2:
                vec2list_Rand.Add(vec2temp + Vector2Int.right);
                vec2list_Rand.Add(vec2temp + Vector2Int.left);
                vec2list_Rand.Add(vec2temp + Vector2Int.up);
                vec2list_Rand.Add(vec2temp + Vector2Int.down);
                //vec3list.Add(vec3temp + Vector3Int.right + temp);
                //vec3list.Add(vec3temp + Vector3Int.left + temp);
                //vec3list.Add(vec3temp + Vector3Int.up + temp);
                //vec3list.Add(vec3temp + Vector3Int.down + temp);
                //vec3list.Add(vec3temp + Vector3Int.right - temp);
                //vec3list.Add(vec3temp + Vector3Int.left - temp);
                //vec3list.Add(vec3temp + Vector3Int.up - temp);
                //vec3list.Add(vec3temp + Vector3Int.down - temp);
                break;
            case 3:
                vec2list_Rand.Add(vec2temp + Vector2Int.left);
                vec2list_Rand.Add(vec2temp + Vector2Int.up);
                vec2list_Rand.Add(vec2temp + Vector2Int.down);
                vec2list_Rand.Add(vec2temp + Vector2Int.right);
                //vec3list.Add(vec3temp + Vector3Int.left + temp);
                //vec3list.Add(vec3temp + Vector3Int.up + temp);
                //vec3list.Add(vec3temp + Vector3Int.down + temp);
                //vec3list.Add(vec3temp + Vector3Int.right + temp);
                //vec3list.Add(vec3temp + Vector3Int.left - temp);
                //vec3list.Add(vec3temp + Vector3Int.up - temp);
                //vec3list.Add(vec3temp + Vector3Int.down - temp);
                //vec3list.Add(vec3temp + Vector3Int.right - temp);
                break;
        }
        for (int i = 0; i < vec2list_Rand.Count; i++)
        {
            if(GameManager.instance.CanGo(Cur,vec2list_Rand[i]) && !GameManager.instance.IshereObstacle(vec2list_Rand[i]))
            {
                vec3IntTemp.x = vec2list_Rand[i].x;
                vec3IntTemp.y = vec2list_Rand[i].y;
                vec3IntTemp.z = GameManager.instance.MapZ[vec2list_Rand[i].x + HalfMapSize, vec2list_Rand[i].y + HalfMapSize];
                vec3ret = vec3IntTemp;
                return vec3ret;
            }
        }        
        return vec3ret;   
    }

    public void GoOrder(Vector3Int goPosition)
    {
        IsOrder = true;
        IsMoveOrder = true;
        UnitState = (int)ActState.run;
        IsCoroutine = false;
        StopAllCoroutines();
        Goal = goPosition;
        IsBridge = false;
        IsBarrigate = false;
        if (this.gameObject.activeSelf)
            StartCoroutine(Move(goPosition));
    }


    private Vector3Int ClickWaterTile()
    {
        Vector3Int v3Int = new Vector3Int(0, 0, -1);
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(Input.touchCount>0)
        {
            pos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
        }
        Ray2D ray = new Ray2D(pos, Vector2.zero);
        int dist = 0;
        int min = 99999999;
        Vector3Int ret = new Vector3Int(0, 0, -1);
        for (int i = 20; i >= 0; i--)
        {
            pos.z = i;
            v3Int = GameManager.instance.map.WorldToCell(pos);
            //v3Int += Vector3Int.up + Vector3Int.right;
            dist = IsnextmapExist(v3Int);
            if (GameManager.instance.map.GetTile(v3Int) == null)
            {
                if(dist >= 0) //그 주변에 갈수있는 곳이 있는지
                {
                    if(dist <min)
                    {
                        min = dist;
                        ret = v3Int;
                    }
                }

                /*if (GameManager.instance.map.GetTile(v3Int).name == "Pillar")
                    continue;
                GameManager.instance.map.SetTileFlags(v3Int, TileFlags.None);

                if (GameManager.instance.map.GetColor(v3Int) == Color.red)
                {
                    GameManager.instance.map.SetColor(v3Int, (Color.blue));
                }
                else
                    GameManager.instance.map.SetColor(v3Int, (Color.red));
                    */
            }
        }
        return ret;
    }
    private int IsnextmapExist(Vector3Int pos)
    {
        List<Vector3Int> path_temp;
        Vector3Int pos_temp;
        int ret =9999999;
        int temp;
        pos_temp = pos + Vector3Int.up;
        if (GameManager.instance.map.GetTile(pos_temp) != null)
        {
            path_temp = PathFinder(Cur, pos_temp);
            if (path_temp != null)
            {
                temp = path_temp.Count;
                if (temp < ret)
                    ret = temp;
            }
        }
        pos_temp = pos + Vector3Int.down;
        if (GameManager.instance.map.GetTile(pos_temp) != null)
        {
            path_temp = PathFinder(Cur, pos_temp);
            if (path_temp != null)
            {
                temp = path_temp.Count;
                if (temp < ret)
                    ret = temp;
            }
        }
        pos_temp = pos + Vector3Int.right;
        if (GameManager.instance.map.GetTile(pos_temp) != null)
        {
            path_temp = PathFinder(Cur, pos_temp);
            if (path_temp!=null)
            {
                temp = path_temp.Count;
                if (temp < ret)
                    ret = temp;
            }
            
        }
        pos_temp = pos + Vector3Int.left;
        if (GameManager.instance.map.GetTile(pos_temp) != null)
        {
            path_temp = PathFinder(Cur, pos_temp);
            if (path_temp != null)
            {
                temp = path_temp.Count;
                if (temp < ret)
                    ret = temp;
            }
        }
        return ret;
    }
    public void GoBridgeOrder(Vector3Int goPosition)
    {
        IsOrder = true;

        BridgePosition = goPosition;

        BridgeLocation = BridgePosition;
        List<Vector3Int> path = new List<Vector3Int>();
        List<Vector3Int> path_temp = new List<Vector3Int>();
        int index = -1;
        path = PathFinder(Cur, BridgePosition + Vector3Int.up);
        
        if(path!=null)
        {
            index = 0;
        }
        path_temp = PathFinder(Cur, BridgePosition + Vector3Int.down);
        if (path_temp != null)
        {
            if (path == null)
            {
                path = path_temp;
                index = 1;
            }
            else if (path_temp.Count < path.Count)
            {
                path = path_temp;
                index = 1;
            }
        }
        path_temp = PathFinder(Cur, BridgePosition + Vector3Int.right);

        if (path_temp != null)
        {
            if (path == null)
            {
                path = path_temp;
                index = 2;
            }
            else if (path_temp.Count < path.Count)
            {
                path = path_temp;
                index = 2;
            }
        }
        path_temp = PathFinder(Cur, BridgePosition + Vector3Int.left);
        if (path_temp != null)
        {
            if (path == null)
            {
                path = path_temp;
                index = 3;
            }
            else if (path_temp.Count < path.Count)
            {
                path = path_temp;
                index = 3;
            }
        }
        if(index!=-1)
        {
            if (index == 0)
                Goal = BridgePosition + Vector3Int.up;
            else if (index == 1)
                Goal = BridgePosition + Vector3Int.down;
            else if (index == 2)
                Goal = BridgePosition + Vector3Int.right;
            else if (index == 3)
                Goal = BridgePosition + Vector3Int.left;

            Goal.z = GameManager.instance.MapZ[Goal.x + HalfMapSize, Goal.y + HalfMapSize];

            UnitState = (int)ActState.bridge;
            IsCoroutine = false;
            StopAllCoroutines();
            IsBridge = true;
            StartCoroutine(Move(Goal));
        }
        else
        {
            IsOrder = false;
            IsBridge = false;
            GameManager.instance.InfoTexting("길이 없습니다");
        }
    }
    public void ChaseOrder(GameObject _ChaseUnit)
    {
        IsOrder = true;

        if (_ChaseUnit == this.gameObject)
        {
            //UnitState = (int)ActState.run;
            return;
        }
        IsCoroutine = false;
        StopAllCoroutines();
        ChaseUnit = _ChaseUnit;
        IsChase = true;
        UnitState = (int)ActState.chase;
    }
    public void ObstacleOrder(Vector3Int goPosition)
    {
        IsOrder = true;

        IsBarrigate = true;
        Goal = goPosition;
        UnitState = (int)ActState.barrigate;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Unit") && collision.GetComponent<Unit>().Cur ==Cur)
        {
            if (collision.GetComponent<Unit>().Master == Master) //우리팀이랑 만났을떄
            {
                if(collision.GetComponent<Unit>().UnitLevel > UnitLevel) //상대가 더 레벨이 높으면
                {                    
                    collision.GetComponent<Unit>().Amount += Amount;
                    //CharMerge.Remove(this.gameObject);
                    switch (Master)
                    {
                        case 1:
                            GameManager.instance.Unit1.Remove(this.gameObject);
                            break;
                        case 2:
                            GameManager.instance.Unit2.Remove(this.gameObject);
                            break;
                        case 3:
                            GameManager.instance.Unit3.Remove(this.gameObject);
                            break;
                        case 4:
                            GameManager.instance.Unit4.Remove(this.gameObject);
                            break;
                    }
                    //Destroy(this.gameObject);
                    collision.GetComponent<Unit>().MergeEffectHpBar();
                    collision.GetComponent<Unit>().MergeSmallEffect();
                    if (collision.GetComponent<Unit>().Unitpriority() >= Unitpriority())//상대 priority 가 높거나 같으면 상관 x
                    {
                        ;
                    }
                    else //상대 prioirty가 더 낮으면
                    {
                        JobSend(collision.gameObject); //내 일을 전해줌
                    }
                    this.gameObject.SetActive(false);
                    return;
                }
                else if(collision.GetComponent<Unit>().UnitLevel == UnitLevel)
                {
                    if (collision.GetComponent<Unit>().Unitpriority() > Unitpriority())
                    {
                        collision.GetComponent<Unit>().Amount += Amount;
                        //CharMerge.Remove(this.gameObject);
                        switch (Master)
                        {
                            case 1:
                                GameManager.instance.Unit1.Remove(this.gameObject);
                                break;
                            case 2:
                                GameManager.instance.Unit2.Remove(this.gameObject);
                                break;
                            case 3:
                                GameManager.instance.Unit3.Remove(this.gameObject);
                                break;
                            case 4:
                                GameManager.instance.Unit4.Remove(this.gameObject);
                                break;
                        }
                        //Destroy(this.gameObject);
                        collision.GetComponent<Unit>().MergeEffectHpBar();
                        collision.GetComponent<Unit>().MergeSmallEffect();
                        this.gameObject.SetActive(false);
                        return;
                    }
                    else if (collision.GetComponent<Unit>().Unitpriority() == Unitpriority())
                    {
                        if (collision.GetComponent<Unit>().Amount > Amount)
                        {
                            collision.GetComponent<Unit>().Amount += Amount;
                            //CharMerge.Remove(this.gameObject);
                            switch (Master)
                            {
                                case 1:
                                    GameManager.instance.Unit1.Remove(this.gameObject);
                                    break;
                                case 2:
                                    GameManager.instance.Unit2.Remove(this.gameObject);
                                    break;
                                case 3:
                                    GameManager.instance.Unit3.Remove(this.gameObject);
                                    break;
                                case 4:
                                    GameManager.instance.Unit4.Remove(this.gameObject);
                                    break;
                            }
                            //Destroy(this.gameObject);
                            collision.GetComponent<Unit>().MergeEffectHpBar();
                            collision.GetComponent<Unit>().MergeSmallEffect();
                            this.gameObject.SetActive(false);
                            return;
                        }
                        else if (collision.GetComponent<Unit>().Amount == Amount)
                        {
                            if (collision.transform.position.y < transform.position.y)
                            {
                                collision.GetComponent<Unit>().Amount += Amount;
                                //CharMerge.Remove(this.gameObject);
                                switch (Master)
                                {
                                    case 1:
                                        GameManager.instance.Unit1.Remove(this.gameObject);
                                        break;
                                    case 2:
                                        GameManager.instance.Unit2.Remove(this.gameObject);
                                        break;
                                    case 3:
                                        GameManager.instance.Unit3.Remove(this.gameObject);
                                        break;
                                    case 4:
                                        GameManager.instance.Unit4.Remove(this.gameObject);
                                        break;
                                }
                                //Destroy(this.gameObject);
                                collision.GetComponent<Unit>().MergeEffectHpBar();
                                collision.GetComponent<Unit>().MergeSmallEffect();
                                this.gameObject.SetActive(false);
                                return;
                            }
                        }
                    }                    
                }                
            }
        }
        //상대팀과 만났을떄
        if (collision.CompareTag("Unit")  && !collision.GetComponent<Unit>().Enemy.Contains(this.gameObject) && collision.GetComponent<Unit>().Master != Master)
        {
            if(collision.GetComponent<Unit>().Cur == Next || collision.GetComponent<Unit>().Next == Cur || collision.GetComponent<Unit>().Cur == Cur)
            {
                IsCoroutine = false;
                StopAllCoroutines(); //멈추고
                _ani.SetBool("Run", false);
                _ani.SetBool("Attack", true);

                collision.GetComponent<Unit>().Enemy.Add(this.gameObject);

                UnitState = (int)ActState.attack;
            }
        }

        //멀리있는 우리팀이랑 만날때
        if (collision.CompareTag("Unit") && collision.GetComponent<Unit>().Next == Cur && collision.GetComponent<Unit>().Cur == Next)
        {
            if (collision.GetComponent<Unit>().Master == Master) //우리팀이랑 만났을떄
            {
                if (collision.GetComponent<Unit>().UnitLevel > UnitLevel) //상대가 더 레벨이 높으면
                {                    
                    collision.GetComponent<Unit>().Amount += Amount;
                    //CharMerge.Remove(this.gameObject);
                    switch (Master)
                    {
                        case 1:
                            GameManager.instance.Unit1.Remove(this.gameObject);
                            break;
                        case 2:
                            GameManager.instance.Unit2.Remove(this.gameObject);
                            break;
                        case 3:
                            GameManager.instance.Unit3.Remove(this.gameObject);
                            break;
                        case 4:
                            GameManager.instance.Unit4.Remove(this.gameObject);
                            break;
                    }
                    //Destroy(this.gameObject);
                    collision.GetComponent<Unit>().MergeEffectHpBar();
                    collision.GetComponent<Unit>().MergeSmallEffect();
                    if (collision.GetComponent<Unit>().Unitpriority() >= Unitpriority())//상대 priority 가 높거나 같으면 상관 x
                    {
                        ;
                    }
                    else //상대 prioirty가 더 낮으면
                    {
                        JobSend(collision.gameObject); //내 일을 전해줌
                    }
                    this.gameObject.SetActive(false);
                    return;
                }
                else if (collision.GetComponent<Unit>().UnitLevel == UnitLevel)
                {
                    if (collision.GetComponent<Unit>().Unitpriority() > Unitpriority())
                    {
                        collision.GetComponent<Unit>().Amount += Amount;
                        //CharMerge.Remove(this.gameObject);
                        switch (Master)
                        {
                            case 1:
                                GameManager.instance.Unit1.Remove(this.gameObject);
                                break;
                            case 2:
                                GameManager.instance.Unit2.Remove(this.gameObject);
                                break;
                            case 3:
                                GameManager.instance.Unit3.Remove(this.gameObject);
                                break;
                            case 4:
                                GameManager.instance.Unit4.Remove(this.gameObject);
                                break;
                        }
                        //Destroy(this.gameObject);
                        collision.GetComponent<Unit>().MergeEffectHpBar();
                        collision.GetComponent<Unit>().MergeSmallEffect();
                        this.gameObject.SetActive(false);
                        return;
                    }
                    else if (collision.GetComponent<Unit>().Unitpriority() == Unitpriority())
                    {
                        if (collision.GetComponent<Unit>().Amount > Amount)
                        {
                            collision.GetComponent<Unit>().Amount += Amount;
                            //CharMerge.Remove(this.gameObject);
                            switch (Master)
                            {
                                case 1:
                                    GameManager.instance.Unit1.Remove(this.gameObject);
                                    break;
                                case 2:
                                    GameManager.instance.Unit2.Remove(this.gameObject);
                                    break;
                                case 3:
                                    GameManager.instance.Unit3.Remove(this.gameObject);
                                    break;
                                case 4:
                                    GameManager.instance.Unit4.Remove(this.gameObject);
                                    break;
                            }
                            //Destroy(this.gameObject);
                            collision.GetComponent<Unit>().MergeEffectHpBar();
                            collision.GetComponent<Unit>().MergeSmallEffect();
                            this.gameObject.SetActive(false);
                            return;
                        }
                        else if (collision.GetComponent<Unit>().Amount == Amount)
                        {
                            if (collision.transform.position.y < transform.position.y)
                            {
                                collision.GetComponent<Unit>().Amount += Amount;
                                //CharMerge.Remove(this.gameObject);
                                switch (Master)
                                {
                                    case 1:
                                        GameManager.instance.Unit1.Remove(this.gameObject);
                                        break;
                                    case 2:
                                        GameManager.instance.Unit2.Remove(this.gameObject);
                                        break;
                                    case 3:
                                        GameManager.instance.Unit3.Remove(this.gameObject);
                                        break;
                                    case 4:
                                        GameManager.instance.Unit4.Remove(this.gameObject);
                                        break;
                                }
                                //Destroy(this.gameObject);
                                collision.GetComponent<Unit>().MergeEffectHpBar();
                                collision.GetComponent<Unit>().MergeSmallEffect();
                                this.gameObject.SetActive(false);
                                return;
                            }
                        }
                    }
                }
            }
        }

        if (collision.CompareTag("Node") && collision.GetComponent<Node>().Pos == Next)
        {
            if (collision.GetComponent<Node>().Master != Master) //만났는데 다른 팀 배럭이면
            {
                if (UnitState != (int)ActState.attack)
                {
                    IsCoroutine = false;
                    StopAllCoroutines(); //멈추고
                    _ani.SetBool("Run", false);
                    _ani.SetBool("Attack", true);

                    Enemy.Add(collision.gameObject);

                    UnitState = (int)ActState.attack;
                }                    
            }
        }
        if (collision.CompareTag("TreeAndBush") && collision.GetComponent<TreeAndBush>().Pos == Next)
        {
            if (UnitState != (int)ActState.attack)
            {
                IsCoroutine = false;
                StopAllCoroutines(); //멈추고
                _ani.SetBool("Run", false);
                _ani.SetBool("Attack", true);

                Enemy.Add(collision.gameObject);

                UnitState = (int)ActState.attack;
            }
        }
    }

    private int whosMap(Color color)
    {
        Color[] color_temp = new Color[5];

        color_temp[1] = DataManager.instance.color1;
        color_temp[2] = DataManager.instance.color2;
        color_temp[3] = DataManager.instance.color3;
        color_temp[4] = DataManager.instance.color4;


        if (DataManager.instance.SelectedChar == 1)
        {
            ;
        }
        else if (DataManager.instance.SelectedChar == 2)
        {
            color_temp[1] = DataManager.instance.color2;
            color_temp[2] = DataManager.instance.color1;
        }
        else if (DataManager.instance.SelectedChar == 3)
        {
            color_temp[3] = DataManager.instance.color1;
            color_temp[1] = DataManager.instance.color3;
        }
        else
        {
            color_temp[4] = DataManager.instance.color1;
            color_temp[1] = DataManager.instance.color4;
        }

        if (color == color_temp[1])
        {
            return 1;
        }
        else if(color == color_temp[2])
        {
            return 2;
        }
        else if (color == color_temp[3])
        {
            return 3;
        }
        else if (color == color_temp[4])
        {
            return 4;
        }
        else
        {
            return 0;
        }
    }

    public int Unitpriority()
    {        
        if (IsStop) return 10;
        else if (IsBarrigate) return 9;
        else if (IsBridge) return 7;
        else if (IsChase) return 6;
        else if (IsStand) return 5;
        else if (IsMoveOrder) return 4;
        else if (IsOrder) return 3;
        else return 2;               
    }

    public void JobSend(GameObject OtherChar)
    {
        if(UnitState == (int)ActState.attack || OtherChar.GetComponent<Unit>().UnitState == (int)ActState.attack)
        {
            if (IsStop)
            {                
                OtherChar.GetComponent<Unit>().IsStand = true;
            }
            else if (IsBarrigate)
            {
                OtherChar.GetComponent<Unit>().Goal = Goal;
                OtherChar.GetComponent<Unit>().IsBarrigate = true;
            }
            else if (IsBridge)
            {
                OtherChar.GetComponent<Unit>().Goal = BridgePosition;
                OtherChar.GetComponent<Unit>().IsBridge = true;                
            }
            else if (IsChase)
            {
                OtherChar.GetComponent<Unit>().IsChase = true;
                OtherChar.GetComponent<Unit>().ChaseUnit = ChaseUnit;
            }
            else if (IsStand)
            {
                OtherChar.GetComponent<Unit>().IsStand = true;
                OtherChar.GetComponent<Unit>().Goal = Goal;
            }
            return;
        }
        if (IsStop)
        {
            OtherChar.GetComponent<Unit>().StopAllCoroutines();
            OtherChar.GetComponent<Unit>().GoOrder(Goal);
            OtherChar.GetComponent<Unit>().IsStand = true;
        }
        else if(IsBarrigate)
        {
            OtherChar.GetComponent<Unit>().IsCoroutine = false;
            OtherChar.GetComponent<Unit>().StopAllCoroutines();
            OtherChar.GetComponent<Unit>().ObstacleOrder(Goal);
        }
        else if(IsBridge)
        {
            OtherChar.GetComponent<Unit>().IsCoroutine = false;
            OtherChar.GetComponent<Unit>().StopAllCoroutines();
            OtherChar.GetComponent<Unit>().GoBridgeOrder(BridgePosition);            
        }
        else if(IsChase)
        {
            OtherChar.GetComponent<Unit>().IsCoroutine = false;
            OtherChar.GetComponent<Unit>().StopAllCoroutines();
            OtherChar.GetComponent<Unit>().ChaseOrder(ChaseUnit);
        }
        else if(IsStand)
        {
            OtherChar.GetComponent<Unit>().IsCoroutine = false;
            OtherChar.GetComponent<Unit>().StopAllCoroutines();
            OtherChar.GetComponent<Unit>().IsStand = true;
            OtherChar.GetComponent<Unit>().Goal = Goal;
        }
    }

    private bool IsNextTile(Vector3Int a,Vector3Int b)
    {
        if (Vector2Int.Distance((Vector2Int)a,(Vector2Int)b) == 1 && Mathf.Abs(a.z - b.z) <= 1)
            return true;
        return false;

    }

    private bool IsEnemyNextTile()
    {
        _col = Physics2D.OverlapCircleAll(transform.position, 2);
        for (int i = 0; i < _col.Length; i++)
        {
            if (_col[i].CompareTag("Unit") && _col[i].gameObject != this.gameObject)
            {
                if (_col[i].GetComponent<Unit>().Master != Master && Vector2Int.Distance((Vector2Int)_col[i].GetComponent<Unit>().Cur, (Vector2Int)Cur) <= 1 && Mathf.Abs(_col[i].GetComponent<Unit>().Cur.z - Cur.z)<=1 )
                {
                    if (!_col[i].GetComponent<Unit>().Enemy.Contains(this.gameObject))
                    {
                        _col[i].GetComponent<Unit>().Enemy.Add(this.gameObject);
                        _col[i].GetComponent<Unit>().StopAllCoroutines();
                        _col[i].GetComponent<Unit>().UnitState = (int)ActState.attack;
                    }
                    if (!Enemy.Contains(_col[i].gameObject))
                        Enemy.Add(_col[i].gameObject);
                    return true;
                }
            }
        }
        return false;
    }


    private bool IsEnemyAround()
    {
        _col = Physics2D.OverlapCircleAll(transform.position, 2);
        for (int i = 0; i < _col.Length; i++)
        {
            if (_col[i].CompareTag("Unit") && _col[i].gameObject != this.gameObject)
            {
                if (_col[i].GetComponent<Unit>().Master != Master && Vector2Int.Distance((Vector2Int)_col[i].GetComponent<Unit>().Cur, (Vector2Int)Cur) <= 4)
                {
                    if(PathEnemyBarrackFinder(Cur, _col[i].GetComponent<Unit>().Cur) != null && PathEnemyBarrackFinder(Cur, _col[i].GetComponent<Unit>().Cur).Count <= 4)
                    {
                        ChaseOrder(_col[i].gameObject);
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private bool IsAllyAround()
    {
        _col = Physics2D.OverlapCircleAll(transform.position, 2);
        for (int i = 0; i < _col.Length; i++)
        {
            if (_col[i].CompareTag("Unit") && _col[i].gameObject != this.gameObject)
            {
                if (_col[i].GetComponent<Unit>().Master == Master && Vector2Int.Distance((Vector2Int)_col[i].GetComponent<Unit>().Cur, (Vector2Int)Cur) <= 1)
                {
                    if (PathEnemyBarrackFinder(Cur, _col[i].GetComponent<Unit>().Cur) != null && PathEnemyBarrackFinder(Cur, _col[i].GetComponent<Unit>().Cur).Count <= 1)
                    {
                        ChaseOrder(_col[i].gameObject);
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private bool IsStringEnemyAround()
    {
        _col = Physics2D.OverlapCircleAll(transform.position, 2);
        for (int i = 0; i < _col.Length; i++)
        {
            if (_col[i].CompareTag("Unit") && _col[i].gameObject != this.gameObject)
            {
                if (_col[i].GetComponent<Unit>().Master != Master && _col[i].GetComponent<Unit>().UnitLevel >= 2 && _col[i].GetComponent<Unit>().Amount <= Amount && Vector2Int.Distance((Vector2Int)_col[i].GetComponent<Unit>().Cur, (Vector2Int)Cur) <= 4)
                {
                    if (PathEnemyBarrackFinder(Cur, _col[i].GetComponent<Unit>().Cur) != null&& PathEnemyBarrackFinder(Cur, _col[i].GetComponent<Unit>().Cur).Count <=4)
                    {
                        ChaseOrder(_col[i].gameObject);
                        return true;
                    }
                }
            }
        }
        return false;
    }



    private bool IsBarrackNextTile()
    {
        _col = Physics2D.OverlapCircleAll(transform.position, 2);
        for (int i = 0; i < _col.Length; i++)
        {
            if (_col[i].CompareTag("Node"))
            {
                if (_col[i].GetComponent<Node>().Master != Master && Vector2Int.Distance((Vector2Int)_col[i].GetComponent<Node>().Pos, (Vector2Int)Cur) <= 1 && Mathf.Abs(_col[i].GetComponent<Node>().Pos.z - Cur.z) <= 1)
                {
                    if (!Enemy.Contains(_col[i].gameObject))
                        Enemy.Add(_col[i].gameObject);
                    return true;
                }
            }
        }
        return false;
    }

    private bool IsBarrackAround()
    {
        _col = Physics2D.OverlapCircleAll(transform.position, 2);
        for (int i = 0; i < _col.Length; i++)
        {
            if (_col[i].CompareTag("Node"))
            {
                if (_col[i].GetComponent<Node>().Master != Master && Vector2Int.Distance((Vector2Int)_col[i].GetComponent<Node>().Pos, (Vector2Int)Cur) <= 4)
                {
                    if (PathEnemyBarrackFinder(Cur, _col[i].GetComponent<Node>().Pos) != null && PathEnemyBarrackFinder(Cur, _col[i].GetComponent<Node>().Pos).Count <= 4)
                    {
                        GoOrder(_col[i].GetComponent<Node>().Pos);
                        return true;
                    }
                }
            }
        }
        return false;
    }


    private void DestroyFightPrefab()
    {
        prefab_Fight = null;
    }

    private void MergeEffect() // 레벨 변화시
    {
        MergeEffectPrefab = GameManager.instance.MergePrefabPooling();
        MergeEffectPrefab.transform.position = transform.position + Vector3.up;
        MergeEffectPrefab.transform.localScale = Vector3.one * UnitLevel*0.35f;
        MergeEffectPrefab.SetActive(true);
    }

    private void MergeSmallEffect() // 합칠때
    {
        MergeSmallEffectPrefab = GameManager.instance.MergeSmallPrefabPooling();
        MergeSmallEffectPrefab.transform.position = transform.position;
        MergeSmallEffectPrefab.SetActive(true);
    }

    private void MergeEffectHpBar()
    {
        if(this.gameObject.activeSelf)
            EffectBar.GetComponent<EffectBarBlink>().Blink();
    }

    private void GhostEffect() // 죽을때
    {
        GhostEffectPrefab = GameManager.instance.GhostPrefabPooling();
        GhostEffectPrefab.transform.position = transform.position + Vector3.up * 0.7f;        
        GhostEffectPrefab.SetActive(true);
    }

}
