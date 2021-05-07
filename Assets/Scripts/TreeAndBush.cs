using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeAndBush : MonoBehaviour
{
    public Vector3Int Pos;

    public int HP;

    private int HalfMapsize;
    // Start is called before the first frame update
    void Start()
    {
        if(HP<=0)
            HP = 500;

        HalfMapsize = GameManager.instance.mapsize / 2;
        GameManager.instance.MapMaster[HalfMapsize + Pos.x, HalfMapsize + Pos.y] = 5;
        GameManager.instance.TileMaster[Pos.x + HalfMapsize, Pos.y + HalfMapsize] = 5;
        GameManager.instance.ObstaclePos[Pos.x + HalfMapsize, Pos.y + HalfMapsize] = this.gameObject;

        this.transform.position = GameManager.instance.map.CellToWorld(Pos);
        transform.position += 2.5f * Vector3.forward + Vector3.up * 0.025f;
    }

    private void OnEnable()
    {
        if (!GameManager.instance.IsGameStart) return;

        if(HP<=0)
            HP = 500;

        HalfMapsize = GameManager.instance.mapsize / 2;
        GameManager.instance.MapMaster[HalfMapsize + Pos.x, HalfMapsize + Pos.y] = 5;
        GameManager.instance.TileMaster[Pos.x + HalfMapsize, Pos.y + HalfMapsize] = 5;
        GameManager.instance.ObstaclePos[Pos.x + HalfMapsize, Pos.y + HalfMapsize] = this.gameObject;

        this.transform.position = GameManager.instance.map.CellToWorld(Pos);
        transform.position += 2.9f * Vector3.forward + Vector3.up * 0.025f;
    }

    private void Update()
    {
        if(HP<=0)
        {
            GameManager.instance.MapMaster[HalfMapsize + Pos.x, HalfMapsize + Pos.y] = 0;
            if (GameManager.instance.map.GetTile(Pos).name == "BridgeRU"|| GameManager.instance.map.GetTile(Pos).name == "BridgeLU")
                GameManager.instance.MapMaster[HalfMapsize + Pos.x, HalfMapsize + Pos.y] = 7;

            GameManager.instance.TileMaster[Pos.x + HalfMapsize, Pos.y + HalfMapsize] = 0;
            GameManager.instance.ObstaclePos[Pos.x + HalfMapsize, Pos.y + HalfMapsize] = null;
            this.gameObject.SetActive(false);
            //Destroy(this.gameObject);
        }
    }
}
