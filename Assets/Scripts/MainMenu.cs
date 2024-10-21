using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject[] infoFill;
    public Button leftBtn, rightBtn;
    private int pageNum;
    public int maxPage;

    private void Start()
    {
        Application.targetFrameRate = 30;
        Screen.SetResolution(1920, 1080, true);
        pageNum = 0;
        InfoBtnToggle();
        maxPage = infoFill.Length - 1;
    }

    public void PressPlay()
    {
        SceneManager.LoadScene(1);
    }

    public void PressExit()
    {
        Application.Quit();
    }

    private void InfoBtnToggle()
    {
        if(pageNum == 0)
        {
            leftBtn.interactable = false;
            rightBtn.interactable = true;
            infoFill[0].SetActive(true);
        }
        else if(pageNum > 0 && pageNum < maxPage)
        {
            leftBtn.interactable = true;
            rightBtn.interactable = true;
            infoFill[pageNum].SetActive(true);
        }
        else if(pageNum ==  maxPage)
        {
            leftBtn.interactable = true;
            rightBtn.interactable = false;
            infoFill[maxPage].SetActive(true);
        }
    }

    public void LeftInfoPress()
    {
        infoFill[pageNum].SetActive(false);
        pageNum -= 1;
        InfoBtnToggle();
    }

    public void RightInfoPress()
    {
        infoFill[pageNum].SetActive(false);
        pageNum += 1;
        InfoBtnToggle();
    }
}
