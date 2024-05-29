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
    public List<Menu> menus = new List<Menu>(); // 메뉴 클래스 배열
    void Start()
    {

        reference = FirebaseDatabase.DefaultInstance.RootReference;
        GetMenu();
        //for (int i = 1; i <= 100; i++)
        //{
        //    string menuName = "Menu" + i.ToString();
        //    int price = UnityEngine.Random.Range(100, 10001);
        //    string description = "메뉴" + i.ToString() + "의 설명메뉴" + i.ToString() + "의 설명메뉴" + i.ToString() + "의 설명메뉴" + i.ToString() + "의 설명";
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
        //        string description = "메뉴" + i.ToString() + "의 설명메뉴" + i.ToString() + "의 설명메뉴" + i.ToString() + "의 설명메뉴" + i.ToString() + "의 설명";
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
        // menus 하위 객체 읽어오기
        reference.Child("menus").GetValueAsync().ContinueWith(task =>
        {
        if (task.IsFaulted)
        {
            Debug.LogError("Failed to fetch menus: " + task.Exception);
        }
        else if (task.IsCompleted)
        {
            DataSnapshot snapshot = task.Result;
                Debug.Log("getmenu성공");


            // 각 메뉴 데이터에 대해 반복하여 리스트에 추가
            foreach (DataSnapshot menuSnapshot in snapshot.Children)
            {
                Menu menu = new Menu();
                    menu.name = menuSnapshot.Child("name").Value.ToString();
                    menu.description = menuSnapshot.Child("description").Value.ToString();
                    menu.like = int.Parse(menuSnapshot.Child("like").Value.ToString());
                    menu.price = int.Parse(menuSnapshot.Child("price").Value.ToString());

                    menus.Add(menu);
                }

                // 가져온 메뉴 리스트를 활용하여 작업 수행
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

        // 파이어베이스에 데이터 추가
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
    //    // UI에 있는 모든 메뉴 제거
    //    foreach (Transform child in menuPanel)
    //    {
    //        Destroy(child.gameObject);
    //    }
    //    // menus 리스트에 있는 각 메뉴 정보를 사용하여 UI에 메뉴를 생성
    //    if(menus.Count <= 0)
    //    {
    //        Debug.Log("가져올 데이터가 없습니다.");
    //    }
    //    foreach (Menu menu in menus)
    //    {
    //        // 메뉴 프리팹을 복제하여 UI에 추가
    //        GameObject newMenu = Instantiate(menuPrefab, menuPanel);
    //        GameObject Vertical_layout = newMenu.transform.Find("Veritcal_layout").gameObject;
    //        GameObject Horizontal_layout = Vertical_layout.transform.Find("Horizontal_layout").gameObject;
    //        // UI 요소에서 Text 컴포넌트를 가져옴
    //        TextMeshProUGUI nameText = Vertical_layout.transform.Find("NameText").GetComponent<TextMeshProUGUI>();
    //        TextMeshProUGUI priceText = Vertical_layout.transform.Find("PriceText").GetComponent<TextMeshProUGUI>();
    //        TextMeshProUGUI likeText = Horizontal_layout.transform.Find("LikeText").GetComponent<TextMeshProUGUI>();

    //        // 메뉴 정보를 텍스트로 설정
    //        nameText.text = menu.name;
    //        priceText.text = menu.price.ToString() + "원";
    //        likeText.text =  menu.like.ToString();
    //    }
    //}
    

}