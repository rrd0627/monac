using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextScript : MonoBehaviour
{
    static public TextScript instance;

    public string[] str_kor;
    public string[] str_Eng;


    private void Awake()
    {
        //if (instance != null)
        //{
        //    Destroy(this.gameObject);
        //}
        //else
        //{
        //}
        instance = this;

    }
    // Start is called before the first frame update
    void Start()
    {
        str_kor = new string[100];
        str_Eng = new string[100];

        str_kor[0] = "메인 화면으로";
        str_Eng[0] = "Home";

        str_kor[1] = "\n메인화면으로 돌아가시겠습니까?\n\n진행중인 게임은 저장되지 않습니다";
        str_Eng[1] = "Return To Home Screen?\nYour Game won't be saved";

        str_kor[2] = "스테이지 화면으로";
        str_Eng[2] = "Map Stage";

        str_kor[3] = "\n스테이지 화면으로 돌아가시겠습니까?\n\n진행중인 게임은 저장되지 않습니다";
        str_Eng[3] = "Return to Map Stage?\nYour game won't be saved";

        str_kor[4] = "게임 재시작";
        str_Eng[4] = "Restart Game";

        str_kor[5] = "\n게임을 재시작 하시겠습니까?";
        str_Eng[5] = "Restart the Game?";

        str_kor[6] = "맵을 파악하고 시작하세요";
        str_Eng[6] = "Check the map and get started";
    }




}
