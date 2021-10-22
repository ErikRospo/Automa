using HeathenEngineering.SteamAPI;
using Michsky.UI.ModernUIPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoinButton : MonoBehaviour
{
    public ButtonManagerBasic button;
    public Image image;
    private UserData userData;

    public void SetUserData(UserData userData)
    {
        this.userData = userData;
    }

    public void UpdateUserData()
    {
        button.buttonText = userData.DisplayName;
        //image.sprite = Sprite.Create(userData.avatar, image.sprite.rect, Vector2.zero);
        button.UpdateUI();
    }
}
