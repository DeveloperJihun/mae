using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Extensions;
using Firebase.Database;
using Unity.VisualScripting;
using TMPro;


public class DataManage : MonoBehaviour
{
    public enum SortType
    {
        Name,
        Price,
        Like
    }
    DatabaseReference reference;
    public List<Menu> menus = new List<Menu>(); // �޴� Ŭ���� �迭
    void Start()
    {

        reference = FirebaseDatabase.DefaultInstance.RootReference;
        GetMenu();
        //for (int i = 1; i <= 100; i++)
        //{
        //    string menuName = "Menu" + i.ToString();
        //    int price = UnityEngine.Random.Range(100, 10001);
        //    string description = "�޴�" + i.ToString() + "�� ����޴�" + i.ToString() + "�� ����޴�" + i.ToString() + "�� ����޴�" + i.ToString() + "�� ����";
        //    int like = UnityEngine.Random.Range(0, 11);

        //    AddNewMenu(menuName, price, description, like);
        //}
    }
    private void Update()
    {
        
        
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    for (int i = 1; i <= 100; i++)
        //    {
        //        string menuName = "Menu" + i.ToString();
        //        int price = UnityEngine.Random.Range(100, 10001);
        //        string description = "�޴�" + i.ToString() + "�� ����޴�" + i.ToString() + "�� ����޴�" + i.ToString() + "�� ����޴�" + i.ToString() + "�� ����";
        //        int like = UnityEngine.Random.Range(0, 11);

        //        AddNewMenu(menuName, price, description, like);
        //    }
        //}
        if (Input.GetKeyDown(KeyCode.R))
        {
           menus.Clear();
           GetMenu();
        }
    }
    void GetMenu()
    {
        // menus ���� ��ü �о����
        reference.Child("menus").GetValueAsync().ContinueWith(task =>
        {
        if (task.IsFaulted)
        {
            Debug.LogError("Failed to fetch menus: " + task.Exception);
        }
        else if (task.IsCompleted)
        {
            DataSnapshot snapshot = task.Result;
                Debug.Log("getmenu����");


            // �� �޴� �����Ϳ� ���� �ݺ��Ͽ� ����Ʈ�� �߰�
            foreach (DataSnapshot menuSnapshot in snapshot.Children)
            {
                Menu menu = new Menu();
                    menu.name = menuSnapshot.Child("name").Value.ToString();
                    menu.description = menuSnapshot.Child("description").Value.ToString();
                    menu.like = int.Parse(menuSnapshot.Child("like").Value.ToString());
                    menu.price = int.Parse(menuSnapshot.Child("price").Value.ToString());

                    menus.Add(menu);
                }

                // ������ �޴� ����Ʈ�� Ȱ���Ͽ� �۾� ����
                //Debug.Log("Fetched menus:");
                //foreach (Menu menu in menus)
                //{
                //    Debug.Log("Menu Name: " + menu.name);
                //    Debug.Log("Menu Description: " + menu.description);
                //    Debug.Log("Menu Like: " + menu.like);
                //    Debug.Log("Menu Price: " + menu.price);
                //}
            }
        });
    }


    void AddNewMenu(string menuName, int price, string description, int like )
    {
        Menu newMenu = new Menu();
        newMenu.name = menuName;
        newMenu.description = description;
        newMenu.like = like;
        newMenu.price = price;

        string json = JsonUtility.ToJson(newMenu);

        // ���̾�̽��� ������ �߰�
        reference.Child("menus").Child(menuName).SetRawJsonValueAsync(json).ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Failed to add new menu: " + task.Exception);
            }
            else if (task.IsCompleted)
            {
                Debug.Log("New menu added successfully.");
            }
        });
    }

    //void CreateMenuUIFromList(List<Menu> menus)
    //{
    //    // UI�� �ִ� ��� �޴� ����
    //    foreach (Transform child in menuPanel)
    //    {
    //        Destroy(child.gameObject);
    //    }
    //    // menus ����Ʈ�� �ִ� �� �޴� ������ ����Ͽ� UI�� �޴��� ����
    //    if(menus.Count <= 0)
    //    {
    //        Debug.Log("������ �����Ͱ� �����ϴ�.");
    //    }
    //    foreach (Menu menu in menus)
    //    {
    //        // �޴� �������� �����Ͽ� UI�� �߰�
    //        GameObject newMenu = Instantiate(menuPrefab, menuPanel);
    //        GameObject Vertical_layout = newMenu.transform.Find("Veritcal_layout").gameObject;
    //        GameObject Horizontal_layout = Vertical_layout.transform.Find("Horizontal_layout").gameObject;
    //        // UI ��ҿ��� Text ������Ʈ�� ������
    //        TextMeshProUGUI nameText = Vertical_layout.transform.Find("NameText").GetComponent<TextMeshProUGUI>();
    //        TextMeshProUGUI priceText = Vertical_layout.transform.Find("PriceText").GetComponent<TextMeshProUGUI>();
    //        TextMeshProUGUI likeText = Horizontal_layout.transform.Find("LikeText").GetComponent<TextMeshProUGUI>();

    //        // �޴� ������ �ؽ�Ʈ�� ����
    //        nameText.text = menu.name;
    //        priceText.text = menu.price.ToString() + "��";
    //        likeText.text =  menu.like.ToString();
    //    }
    //}
    

}