using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObject : MonoBehaviour
{
    public Vector3Int vec3;
    Vector2 Goal = new Vector2();
    Vector2 dir;

    float dist;
    float dist_temp;

    private void Start()
    {
        if(vec3 == new Vector3Int(21, 18, 1)||vec3 == new Vector3Int(17,11,1)|| vec3 == new Vector3Int(17, 14, 1)|| vec3 == new Vector3Int(40, 11, 1))
        {
            ;
        }
        else
        {
            vec3 = new Vector3Int(9, 8, 1);
        }

        Goal = GameManager.instance.map.CellToWorld(vec3);
        dist_temp = 9999999;
        dir = Goal - (Vector2)transform.position;
        dir.Normalize();
    }
    // Update is called once per frame
    void Update()
    {
        transform.Translate(dir *2* Time.fixedDeltaTime);

        dist = Vector2.Distance(transform.position, Goal);
        if (dist > dist_temp)
            Destroy(this.gameObject);
        dist_temp = Vector2.Distance(transform.position, Goal);
    }
}
