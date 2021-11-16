using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HudIcon : MonoBehaviour
{
    private GameModel Model;

    [Header("Elements")]
    public Button Button;
    public Text HotkeyText;
    public UI_Window Window;

    [Header("Misc")]
    public KeyCode Hotkey;

    public bool IsWindowActive;

    public void Init(GameModel model)
    {
        Model = model;
        HotkeyText.text = Hotkey.ToString();
        Button.onClick.AddListener(ToggleWindow);
        Window.gameObject.SetActive(false);
        IsWindowActive = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(Hotkey)) ToggleWindow();
    }

    private void ToggleWindow()
    {
        IsWindowActive = !IsWindowActive;
        Window.gameObject.SetActive(IsWindowActive);

        if (IsWindowActive) Window.Init(Model);
    }
}
