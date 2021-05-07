using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    private bool IsSetting;

    public GameObject[] Buttons;

    public GameObject CreditPanel;

    private bool IsButtonPress;

    // Start is called before the first frame update
    void Start()
    {
        IsSetting = false;
        IsButtonPress = false;
        //GameManager.instance.IsGameStart = true;
        //BGMManager.instance.Play(1);
        //BGMManager.instance.FadeInMusic();
    }
    private void OnEnable()
    {
        BGMManager.instance.Play(1);
        BGMManager.instance.FadeInMusic();
    }

    public void StartButton()
    {
        if (!DataManager.instance.StageClear[0])
        {
            DataManager.instance.StageClear[0] = true;
            Tutorial();
            return;
        }

        /*
        AsyncOperation oper = new AsyncOperation();      
        oper = SceneManager.LoadSceneAsync("StageSelect");
        oper.allowSceneActivation = true;
        */
        //BGMManager.instance.FadeOutMusic();        
        BGMManager.instance.FadeInMusic();
        BGMManager.instance.Play(0);        

        SoundManager.instance.Play(2);
        TitlePrefab.instance.gameObject.SetActive(false);
        StageSelectPrefab.instance.gameObject.SetActive(true);
    }

    public void Tutorial()
    {        
        if (!IsButtonPress)
        {
            IsButtonPress = true;
        }
        else
            return;

        DataManager.instance.IsTuto = true;
        AsyncOperation oper = new AsyncOperation();
        oper = SceneManager.LoadSceneAsync("Loading");
        
        oper.allowSceneActivation = true;
    }

    public void SettingButton()
    {
        SoundManager.instance.Play(2);

        IsSetting = !IsSetting;        
        if(IsSetting)
        {
            StopAllCoroutines();

            for (int i = 0; i < Buttons.Length; i++)
                Buttons[i].SetActive(true);

            StartCoroutine(Spread());
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(SpreadBack());            
        }
    }
    IEnumerator Spread()
    {
        int[] Y_spacing = new int[Buttons.Length];
        bool[] SpreadDone = new bool[Buttons.Length];
        bool EverySprite = false;

        Y_spacing[0] = 140;
        Y_spacing[1] = 280;
        //Y_spacing[3] = 560;        

        Vector2[] GoalPos = new Vector2[Buttons.Length];

        //GoalPos[0].x = 0;
        //GoalPos[0].y = -Y_spacing[0];
        //GoalPos[1].x = 0;
        //GoalPos[1].y = -Y_spacing[1];
        //GoalPos[2].x = 0;
        //GoalPos[2].y = -Y_spacing[2];
        //GoalPos[3].x = 0;
        //GoalPos[3].y = -Y_spacing[3];

        GoalPos[0].x = -Y_spacing[0];
        GoalPos[0].y = 0;
        GoalPos[1].x = -Y_spacing[1];
        GoalPos[1].y = 0;
        //GoalPos[3].x = -Y_spacing[3];
        //GoalPos[3].y = 0;

        while (true)
        {
            for (int i = 0; i < Buttons.Length; i++)
            {
                if (SpreadDone[i]) continue;

                if (Buttons[i].transform.localPosition.y < -Y_spacing[i])
                    SpreadDone[i] = true;

                Buttons[i].transform.localPosition = Vector2.Lerp(Buttons[i].transform.localPosition, GoalPos[i], 15*Time.deltaTime);
                //Buttons[i].transform.Translate(Vector2.down * 15* Time.deltaTime);
            }
            EverySprite = true;
            for (int i=0;i<Buttons.Length;i++)
            {
                EverySprite = EverySprite&SpreadDone[i];
            }
            if (EverySprite) break;
            yield return null;
        }
    }
    IEnumerator SpreadBack()
    {
        int Y_spacing = 140;
        bool[] SpreadDone = new bool[Buttons.Length];
        bool EverySprite = false;
        while (true)
        {
            for (int i = 0; i < Buttons.Length; i++)
            {
                if (SpreadDone[i]) continue;

                if (Buttons[i].transform.localPosition.x > -Y_spacing)
                {
                    Buttons[i].SetActive(false);
                    SpreadDone[i] = true;
                }
                Buttons[i].transform.Translate(Vector2.right * 45* Time.deltaTime);
            }
            EverySprite = true;
            for (int i = 0; i < Buttons.Length; i++)
            {
                EverySprite = EverySprite & SpreadDone[i];
            }
            if (EverySprite) break;
            yield return null;
        }
        for (int i = 0; i < Buttons.Length; i++)
            Buttons[i].SetActive(false);
    }

    public void InfoButton()
    {
        SoundManager.instance.Play(2);

        Application.OpenURL("https://antiquegame2020.tistory.com/");

    }
    public void CreditButton()
    {
        SoundManager.instance.Play(2);

        CreditPanel.SetActive(true);
    }
    public void CreditExit()
    {
        SoundManager.instance.Play(2);

        CreditPanel.SetActive(false);
    }

    public void SettingPopup()
    {
        SoundManager.instance.Play(2);

        DataManager.instance.PAUSEDPanel.SetActive(true);
    }
}
