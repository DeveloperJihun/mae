using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Extensions;
using Firebase.Database;
using Unity.VisualScripting;
using TMPro;
public class menu_detail : MonoBehaviour
{
    private void Start()
    {
        chat_buton.onClick.AddListener(() => Chat_button_Click());
    }
    public GameObject Community_manager;
    public GameObject UIManager;
    public TextMeshProUGUI menu_name;
    public TextMeshProUGUI menu_description;
    public TextMeshProUGUI menu_price;
    public Button back_button;
    public Button chat_buton;
    public void Set_UI()
    {
        menu_name.text = Global_data.selected_menu_name;

        menu_price.text = Global_data.selected_menu_price.ToString() + "¿ø";

        menu_description.text = Global_data.selected_menu_description;
    }

    public void back_button_click()
    {
        //Debug.Log("kj");
        UIManager.GetComponent<TabManager>().LookaroundScene();
    }
    public void Chat_button_Click()
    {
        Debug.Log(Community_manager);
        Community_manager.GetComponent<menu_community>().LoadReviews(Global_data.selected_menu_name);
    }
}
