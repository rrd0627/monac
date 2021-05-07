using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeBar : MonoBehaviour
{
    private Image Timebar;
    
    public GameObject[] Star;
    public TimeScript timescript;
    private int Curstage;
    
    private float Star3timer;
    private float Star2timer;
    private float Star1timer;


    private bool Star3Gone;
    private bool Star2Gone;
    private bool Star1Gone;

    // Start is called before the first frame update
    void Start()
    {
        Curstage = DataManager.instance.LastStage;
        
        Timebar = GetComponent<Image>();


        if(DataManager.instance.IsHard[Curstage])
        {
            Star3timer = DataManager.instance.StarHardTimer[Curstage, 2];
            Star2timer = DataManager.instance.StarHardTimer[Curstage, 1];
            Star1timer = DataManager.instance.StarHardTimer[Curstage, 0];
        }
        else
        {
            Star3timer = DataManager.instance.StarTimer[Curstage, 2];
            Star2timer = DataManager.instance.StarTimer[Curstage, 1];
            Star1timer = DataManager.instance.StarTimer[Curstage, 0];
        }

        Star3Gone = false;
        Star2Gone = false;
        Star1Gone = false;
    }



    // Update is called once per frame
    void Update()
    {
        if(Timebar.fillAmount>0.66f)
        {
            Timebar.fillAmount = ((Star3timer - GameManager.instance.TimeForText) / Star3timer) * 0.34f + 0.66f;
        }
        else if (Timebar.fillAmount > 0.33f)
        {
            Timebar.fillAmount = (((Star2timer-Star3timer) - (GameManager.instance.TimeForText-Star3timer)) / (Star2timer-Star3timer)) * 0.33f + 0.33f;            
        }
        else
        {
            Timebar.fillAmount = (((Star1timer-Star2timer) - (GameManager.instance.TimeForText - Star2timer)) / (Star1timer - Star2timer)) * 0.33f;            
        }
        
        if(GameManager.instance.TimeForText>Star3timer && !Star3Gone)
        {
            Star3Gone = true;
            StartCoroutine(Vanish(Star[0]));            
        }
        else if (GameManager.instance.TimeForText > Star2timer && !Star2Gone)
        {
            Star2Gone = true;
            StartCoroutine(Vanish(Star[1]));
        }
        else if(GameManager.instance.TimeForText > Star1timer && !Star1Gone)
        {
            Star1Gone = true;
            StartCoroutine(Vanish(Star[2]));

            if (!GameManager.instance.DefeatPanel.activeSelf && !GameManager.instance.VictoryPanel.activeSelf)
                GameManager.instance.DefeatGame();
        }
    }

    IEnumerator Vanish(GameObject Obj)
    {
        while(true)
        {
            Obj.transform.localScale *= 0.9f;
            if (Obj.transform.localScale.x <= 0.1f)
            {
                Obj.SetActive(false);
                break;
            }
            yield return null;
        }
    }

}
