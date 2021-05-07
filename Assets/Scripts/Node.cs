using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Node : MonoBehaviour
{
    public int Master; //master 1  ==  유저
    private GameObject Unit;
    private GameObject Unit_prefab;
    private GameObject Produce_prefab;
    private Vector2 Pos_Node;
    public int HP;
    private int MaxHp;
    private Color _color;
    public Vector3Int Pos;
    private float timer;

    private Collider2D[] _col;

    private int Half_Mapsize;


    public int Level;

    public int UnitProduce;

    private const int BowBarrackHP = 30;
    private const int BarrackHP = 40;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = GameManager.instance.change.CellToWorld(Pos);
        transform.position += Vector3.forward * 1.12f;
        Level = 0;
        UnitProduce = 0;
        HP = 10;
        MaxHp = BarrackHP;
        
        Unit = GameManager.instance.Character;
        Half_Mapsize = GameManager.instance.mapsize / 2;
        if (Master == 1)
        {
            if (DataManager.instance.SelectedChar == 1)
            {
                _color = DataManager.instance.color1;
            }
            else if (DataManager.instance.SelectedChar == 2)
            {
                _color = DataManager.instance.color2;
            }
            else if (DataManager.instance.SelectedChar == 3)
            {
                _color = DataManager.instance.color3;
                MaxHp = BowBarrackHP;
            }
            else
            {
                _color = DataManager.instance.color4;
            }
        }
        else if (Master == 2)
        {
            _color = DataManager.instance.color2;
            if (DataManager.instance.SelectedChar == 2)
            {
                _color = DataManager.instance.color1;
            }
        }
        else if (Master == 3)
        {
            _color = DataManager.instance.color3;
            MaxHp = BowBarrackHP;
            if (DataManager.instance.SelectedChar == 3)
            {
                MaxHp = BarrackHP;
                _color = DataManager.instance.color1;
            }
        }
        else if (Master == 4)
        {
            _color = DataManager.instance.color4;
            if (DataManager.instance.SelectedChar == 4)
            {
                _color = DataManager.instance.color1;
            }
        }        
        GetComponent<SpriteRenderer>().color = _color;
        GameManager.instance.TileMaster[Pos.x + GameManager.instance.mapsize / 2, Pos.y + GameManager.instance.mapsize / 2] = Master;

        if(!GameManager.instance.IsGameStart)
            GameManager.instance.NodeNumber[Master]++;
    }

    private void OnEnable()
    {
        if (!GameManager.instance.IsGameStart) return;
        Level = 0;
        UnitProduce = 0;
        transform.position = GameManager.instance.change.CellToWorld(Pos);
        transform.position += Vector3.forward * 1.12f;

        HP = 10;
        MaxHp = BarrackHP;
        Unit = GameManager.instance.Character;
        Half_Mapsize = GameManager.instance.mapsize / 2;
        if (Master == 1)
        {
            if (DataManager.instance.SelectedChar == 1)
            {
                _color = DataManager.instance.color1;
            }
            else if (DataManager.instance.SelectedChar == 2)
            {
                _color = DataManager.instance.color2;
            }
            else if (DataManager.instance.SelectedChar == 3)
            {
                MaxHp = BowBarrackHP;
                _color = DataManager.instance.color3;
            }
            else
            {
                _color = DataManager.instance.color4;
            }
        }
        else if (Master == 2)
        {
            _color = DataManager.instance.color2;
            if (DataManager.instance.SelectedChar == 2)
            {
                _color = DataManager.instance.color1;
            }
        }
        else if (Master == 3)
        {
            _color = DataManager.instance.color3;
            MaxHp = BowBarrackHP;
            if (DataManager.instance.SelectedChar == 3)
            {
                MaxHp = BarrackHP;
                _color = DataManager.instance.color1;
            }
        }
        else if (Master == 4)
        {
            _color = DataManager.instance.color4;
            if (DataManager.instance.SelectedChar == 4)
            {
                _color = DataManager.instance.color1;
            }
        }
        GetComponent<SpriteRenderer>().color = _color;
        GameManager.instance.TileMaster[Pos.x + GameManager.instance.mapsize / 2, Pos.y + GameManager.instance.mapsize / 2] = Master;
        GameManager.instance.NodeNumber[Master]++;
    }
    void Update()
    {
        timer += Time.deltaTime;

        if (HP < 0)
        {
            GameManager.instance.TileMaster[Pos.x + GameManager.instance.mapsize / 2, Pos.y + GameManager.instance.mapsize / 2] = 0;
            GameManager.instance.NodeNumber[Master]--;
            //Destroy(this.gameObject);
            this.gameObject.SetActive(false);
        }
        if (timer >= 1f)
        {
            timer = 0;

            if(Level ==0)
            { //많이 소환   자원 생산 x
                ;
            }
            else if(Level ==1)
            {
                GameManager.instance.money[Master] += 10;
            }
            else if(Level ==2)
            {
                GameManager.instance.money[Master] += 30;
            }

            if (HP < MaxHp /*&& !GameManager.instance.IsNoMoney[Master]*/)
            {
                if(GameManager.instance.money[Master] >= 20000)
                {//4
                    HP += 10;
                }
                else if(GameManager.instance.money[Master] >= 15000)
                {//5
                    HP += 8;
                }
                else if(GameManager.instance.money[Master] >= 10000)
                {//6
                    HP += 7;
                }
                else if(GameManager.instance.money[Master] >= 5000)
                {//7
                    HP += 6;
                }
                else
                {//8
                    HP += 5;
                }
            }
                

            if (HP >= MaxHp /*&& !GameManager.instance.IsNoMoney[Master]*/)
            {
                //if(GameManager.instance.money[Master] - GameManager.instance.MoneyAmount > 0)
                //{
                //GameManager.instance.money[Master] -= GameManager.instance.MoneyAmount;
                if(Level!=2)
                {
                    switch(Master)
                    {
                        case 1:
                            if (GameManager.instance.Unit1.Count >= 15)
                                return;
                            break;
                        case 2:
                            if (GameManager.instance.Unit2.Count >= 15)
                                return;
                            break;
                        case 3:
                            if (GameManager.instance.Unit3.Count >= 15)
                                return;
                            break;
                        case 4:
                            if (GameManager.instance.Unit4.Count >= 15)
                                return;
                            break;
                    }

                    HP = 0;
                    Unit_prefab = GameManager.instance.CharacterPrefabPooling();
                    Unit_prefab.GetComponent<Unit>().Master = Master;
                    Unit_prefab.GetComponent<Unit>().Cur = Pos;
                    Unit_prefab.SetActive(true);
                    UnitProduce++;
                    Produce_prefab = GameManager.instance.ProducePrefabPooling();
                    Produce_prefab.transform.position = transform.position + Vector3.up*0.2f;
                    Produce_prefab.SetActive(true);
                }
                if (Level == 0 && UnitProduce>=4)
                {
                    if(ScanTile()) //주변에 남에땅 있으면
                    {
                        ;
                    }
                    else //모두 내땅!
                    {
                        UnitProduce = 0;
                        Level = 1;                        
                    }
                }
                if(Level == 1 && UnitProduce >= 2)
                {
                    if (ScanTile()) //주변에 남에땅 있으면
                    {
                        UnitProduce = 0;
                        Level = 0;
                    }
                    else
                    {
                        UnitProduce = 0;
                        Level = 2;
                    }
                }
                if(Level ==2)
                {
                    if(ScanTile())
                    {
                        UnitProduce = 0;
                        Level = 1;
                    }
                }

                /*
                Unit_prefab = Instantiate(Unit, this.transform.position, Quaternion.identity);
                Unit_prefab.GetComponent<Unit>().Master = Master;
                Unit_prefab.GetComponent<Unit>().Cur = Pos;*/


                //Unit_prefab.GetComponent<SpriteRenderer>().color = _color;
                //}
                //else
                //{
                //    GameManager.instance.money[Master] = 0;
                //    GameManager.instance.IsNoMoney[Master] = true;
                //}
            }
        }        
    }



    private bool ScanTile()
    {
        List<Vector2Int> vec2list = new List<Vector2Int>();
        List<Vector2Int> visited = new List<Vector2Int>();
        Vector2Int vec2temp = new Vector2Int();
        int count = 0;

        //넓혀가는식으로 가장 가까운 것 찾음
        vec2list.Add((Vector2Int)Pos);
        while (vec2list.Count != 0)
        {
            vec2temp = vec2list[0];
            vec2list.RemoveAt(0);

            if (GameManager.instance.MapMaster[vec2temp.x + Half_Mapsize, vec2temp.y + Half_Mapsize] == 6) //못가는곳이면
                continue;
            if (visited.Contains(vec2temp) == true)
                continue;

            count++;
            if (count > 100)
            {
                return false;
            }

            if (GameManager.instance.MapMaster[vec2temp.x + Half_Mapsize, vec2temp.y + Half_Mapsize] != Master) //다른색이면
            {
                if (GameManager.instance.MapMaster[vec2temp.x + Half_Mapsize, vec2temp.y + Half_Mapsize] == 7)
                {
                    ;//다리인경우
                }
                else
                {
                    return true;

                    //for (int i = 20; i >= 0; i--)
                    //{
                    //    Vec3Temp.x = vec2temp.x;
                    //    Vec3Temp.y = vec2temp.y;
                    //    Vec3Temp.z = i;
                    //    if (GameManager.instance.map.GetTile(Vec3Temp) != null)
                    //    {
                    //        return true;
                    //    }
                    //}
                }
            }
            visited.Add(vec2temp);

            int rand = Random.Range(0, 4);

            switch (rand)
            {
                case 0:
                    if(Mathf.Abs(GameManager.instance.MapZ[vec2temp.x + Half_Mapsize,vec2temp.y + Half_Mapsize]- GameManager.instance.MapZ[vec2temp.x + Half_Mapsize, vec2temp.y + Half_Mapsize + 1] )<=1)
                        vec2list.Add(vec2temp + Vector2Int.up);
                    if (Mathf.Abs(GameManager.instance.MapZ[vec2temp.x + Half_Mapsize, vec2temp.y + Half_Mapsize] - GameManager.instance.MapZ[vec2temp.x + Half_Mapsize, vec2temp.y + Half_Mapsize - 1] ) <= 1)
                        vec2list.Add(vec2temp + Vector2Int.down);
                    if (Mathf.Abs(GameManager.instance.MapZ[vec2temp.x + Half_Mapsize, vec2temp.y + Half_Mapsize] - GameManager.instance.MapZ[vec2temp.x + Half_Mapsize + 1, vec2temp.y + Half_Mapsize]) <= 1)
                        vec2list.Add(vec2temp + Vector2Int.right);
                    if (Mathf.Abs(GameManager.instance.MapZ[vec2temp.x + Half_Mapsize, vec2temp.y + Half_Mapsize] - GameManager.instance.MapZ[vec2temp.x + Half_Mapsize - 1, vec2temp.y + Half_Mapsize]) <= 1)
                        vec2list.Add(vec2temp + Vector2Int.left);
                    break;
                case 1:
                    if (Mathf.Abs(GameManager.instance.MapZ[vec2temp.x + Half_Mapsize, vec2temp.y + Half_Mapsize] - GameManager.instance.MapZ[vec2temp.x + Half_Mapsize, vec2temp.y + Half_Mapsize - 1] ) <= 1)
                        vec2list.Add(vec2temp + Vector2Int.down);
                    if (Mathf.Abs(GameManager.instance.MapZ[vec2temp.x + Half_Mapsize, vec2temp.y + Half_Mapsize] - GameManager.instance.MapZ[vec2temp.x + Half_Mapsize + 1, vec2temp.y + Half_Mapsize]) <= 1)
                        vec2list.Add(vec2temp + Vector2Int.right);
                    if (Mathf.Abs(GameManager.instance.MapZ[vec2temp.x + Half_Mapsize, vec2temp.y + Half_Mapsize] - GameManager.instance.MapZ[vec2temp.x + Half_Mapsize - 1, vec2temp.y + Half_Mapsize]) <= 1)
                        vec2list.Add(vec2temp + Vector2Int.left);
                    if (Mathf.Abs(GameManager.instance.MapZ[vec2temp.x + Half_Mapsize, vec2temp.y + Half_Mapsize] - GameManager.instance.MapZ[vec2temp.x + Half_Mapsize, vec2temp.y + Half_Mapsize + 1] ) <= 1)
                        vec2list.Add(vec2temp + Vector2Int.up);
                    break;
                case 2:
                    if (Mathf.Abs(GameManager.instance.MapZ[vec2temp.x + Half_Mapsize, vec2temp.y + Half_Mapsize] - GameManager.instance.MapZ[vec2temp.x + Half_Mapsize + 1, vec2temp.y + Half_Mapsize] ) <= 1)
                        vec2list.Add(vec2temp + Vector2Int.right);
                    if (Mathf.Abs(GameManager.instance.MapZ[vec2temp.x + Half_Mapsize, vec2temp.y + Half_Mapsize] - GameManager.instance.MapZ[vec2temp.x + Half_Mapsize - 1, vec2temp.y + Half_Mapsize]) <= 1)
                        vec2list.Add(vec2temp + Vector2Int.left);
                    if (Mathf.Abs(GameManager.instance.MapZ[vec2temp.x + Half_Mapsize, vec2temp.y + Half_Mapsize] - GameManager.instance.MapZ[vec2temp.x + Half_Mapsize, vec2temp.y + Half_Mapsize + 1] ) <= 1)
                        vec2list.Add(vec2temp + Vector2Int.up);
                    if (Mathf.Abs(GameManager.instance.MapZ[vec2temp.x + Half_Mapsize, vec2temp.y + Half_Mapsize] - GameManager.instance.MapZ[vec2temp.x + Half_Mapsize, vec2temp.y + Half_Mapsize - 1] ) <= 1)
                        vec2list.Add(vec2temp + Vector2Int.down);
                    break;
                case 3:
                    if (Mathf.Abs(GameManager.instance.MapZ[vec2temp.x + Half_Mapsize, vec2temp.y + Half_Mapsize] - GameManager.instance.MapZ[vec2temp.x + Half_Mapsize - 1, vec2temp.y + Half_Mapsize]) <= 1)
                        vec2list.Add(vec2temp + Vector2Int.left);
                    if (Mathf.Abs(GameManager.instance.MapZ[vec2temp.x + Half_Mapsize, vec2temp.y + Half_Mapsize] - GameManager.instance.MapZ[vec2temp.x + Half_Mapsize, vec2temp.y + Half_Mapsize + 1]) <= 1)
                        vec2list.Add(vec2temp + Vector2Int.up);
                    if (Mathf.Abs(GameManager.instance.MapZ[vec2temp.x + Half_Mapsize, vec2temp.y + Half_Mapsize] - GameManager.instance.MapZ[vec2temp.x + Half_Mapsize, vec2temp.y + Half_Mapsize - 1]) <= 1)
                        vec2list.Add(vec2temp + Vector2Int.down);
                    if (Mathf.Abs(GameManager.instance.MapZ[vec2temp.x + Half_Mapsize, vec2temp.y + Half_Mapsize] - GameManager.instance.MapZ[vec2temp.x + Half_Mapsize + 1, vec2temp.y + Half_Mapsize]) <= 1)
                        vec2list.Add(vec2temp + Vector2Int.right);
                    break;
            }
        }
        return false;
    }

}


