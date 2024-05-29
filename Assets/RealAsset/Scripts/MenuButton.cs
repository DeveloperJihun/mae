using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Extensions;
using Firebase.Database;
using Unity.VisualScripting;
using TMPro;
using System.Linq;

public class MenuButton : MonoBehaviour
{
    public enum SortType
    {
        Name,
        Price,
        Like
    }
    public GameObject Manager;
    public GameObject menuPrefab; // 메뉴를 생성할 프리팹
    public Transform menuPanel; // 메뉴를 생성할 부모 객체 (Menu Panel)
    public TMP_Dropdown sortDropdown;
    public GameObject UIManager;
    public GameObject community_manager;
    public GameObject search_button;
    public TMP_InputField search_field;
    public string SelectedMenu;
    public List<Menu> menus = new List<Menu>(); // 메뉴 클래스 배열
    public List<Menu> searched_menus = new List<Menu>();

    private void Start()
    {
        UnityEngine.UI.Button search_component = search_button.GetComponent<UnityEngine.UI.Button>();
        search_component.onClick.AddListener(() => search_button_click());
        sortDropdown.onValueChanged.AddListener(delegate {
            OnSortDropdownValueChanged(sortDropdown);
        });

        StartCoroutine(Delay_loading(0.5f));

    }
    public void SearchMenus(string searchString)
    {
        if (string.IsNullOrEmpty(searchString))
        {
            Debug.Log("Search string is empty or null.");
            return;
        }

        // 검색 로직
        searched_menus = menus.Where(menu => menu.name.Contains(searchString)).ToList();

        // 검색 결과 출력
        foreach (Menu menu in searched_menus)
        {
            Debug.Log("Found menu: " + menu.name);
        }

        CreateMenuUIFromList(searched_menus);
    }
    private IEnumerator Delay_loading(float delay)
    {
        yield return new WaitForSeconds(delay);

        menus = Manager.GetComponent<DataManage>().menus;
        CreateMenuUIFromList(menus);

        yield return new WaitForSeconds(delay);

        // 게임 오브젝트 비활성화
        //this.gameObject.SetActive(false);
    }
    public void SetSelectedMenu()
    {
        GameObject Vertical_layout = this.transform.Find("Veritcal_layout").gameObject;
        GameObject Horizontal_layout = Vertical_layout.transform.Find("Horizontal_layout").gameObject;
        // UI 요소에서 Text 컴포넌트를 가져옴
        TextMeshProUGUI nameText = Vertical_layout.transform.Find("NameText").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI priceText = Vertical_layout.transform.Find("PriceText").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI likeText = Horizontal_layout.transform.Find("LikeText").GetComponent<TextMeshProUGUI>();
        SelectedMenu = nameText.text;
        //UIManager.GetComponent<TabManager>()
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            menus = Manager.GetComponent<DataManage>().menus;
            CreateMenuUIFromList(menus);
        }
    }


    void CreateMenuUIFromList(List<Menu> menus)
    {
        // UI에 있는 모든 메뉴 제거
        foreach (Transform child in menuPanel)
        {
            Destroy(child.gameObject);
        }
        // menus 리스트에 있는 각 메뉴 정보를 사용하여 UI에 메뉴를 생성
        if (menus.Count <= 0)
        {
            Debug.Log("가져올 데이터가 없습니다.");

            //CreateMenuUIFromList(menus);
        }
        foreach (Menu menu in menus)
        {
            // 메뉴 프리팹을 복제하여 UI에 추가
            GameObject newMenu = Instantiate(menuPrefab, menuPanel);
            menu_number menuDetails = newMenu.GetComponent<menu_number>();
            Button menuButton = newMenu.GetComponent<Button>();
            menuButton.onClick.AddListener(() => menu_click(menuDetails));
            newMenu.GetComponent<menu_number>().menu_name = menu.name;
            newMenu.GetComponent<menu_number>().menu_price = menu.price;
            newMenu.GetComponent<menu_number>().menu_description = menu.description;
            GameObject Vertical_layout = newMenu.transform.Find("Veritcal_layout").gameObject;
            GameObject Horizontal_layout = Vertical_layout.transform.Find("Horizontal_layout").gameObject;
            // UI 요소에서 Text 컴포넌트를 가져옴
            TextMeshProUGUI nameText = Vertical_layout.transform.Find("NameText").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI priceText = Vertical_layout.transform.Find("PriceText").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI likeText = Horizontal_layout.transform.Find("LikeText").GetComponent<TextMeshProUGUI>();

            // 메뉴 정보를 텍스트로 설정
            nameText.text = menu.name;
            priceText.text = menu.price.ToString() + "원";
            likeText.text = menu.like.ToString();
        }
    }

    void SortMenu(SortType sortType)
    {
        switch (sortType)
        {
            case SortType.Name:
                // 이름을 기준으로 정렬
                for (int i = 0; i < menus.Count - 1; i++)
                {
                    for (int j = 0; j < menus.Count - 1 - i; j++)
                    {
                        // 메뉴 이름을 비교하여 정렬
                        if (string.Compare(menus[j].name, menus[j + 1].name) > 0)
                        {
                            // 메뉴를 교환
                            Menu temp = menus[j];
                            menus[j] = menus[j + 1];
                            menus[j + 1] = temp;
                        }
                    }
                }
                break;
            case SortType.Price:
                // 가격을 기준으로 정렬
                for (int i = 0; i < menus.Count - 1; i++)
                {
                    for (int j = 0; j < menus.Count - 1 - i; j++)
                    {
                        if (menus[j].price > menus[j + 1].price)
                        {
                            // 메뉴를 교환
                            Menu temp = menus[j];
                            menus[j] = menus[j + 1];
                            menus[j + 1] = temp;
                        }
                    }
                }
                break;
            case SortType.Like:
                // 좋아요 수를 기준으로 정렬
                for (int i = 0; i < menus.Count - 1; i++)
                {
                    for (int j = 0; j < menus.Count - 1 - i; j++)
                    {
                        if (menus[j].like > menus[j + 1].like)
                        {
                            // 메뉴를 교환
                            Menu temp = menus[j];
                            menus[j] = menus[j + 1];
                            menus[j + 1] = temp;
                        }
                    }
                }
                break;
        }

        // 정렬된 메뉴 리스트를 사용하여 UI를 업데이트
        CreateMenuUIFromList(menus);
    }

    void OnSortDropdownValueChanged(TMP_Dropdown change)
    {
        // 선택된 옵션에 따라 메뉴를 정렬
        switch (change.options[change.value].text)
        {
            case "Name":
                SortMenu(SortType.Name);
                break;
            case "Price":
                SortMenu(SortType.Price);
                break;
            case "Like":
                SortMenu(SortType.Like);
                break;
        }
    }

    public void menu_click(menu_number menuDetails)
    {
        if (menuDetails != null)
        {
            Global_data.selected_menu_name = menuDetails.menu_name;
            Global_data.selected_menu_description = menuDetails.menu_description;
            Global_data.selected_menu_price = menuDetails.menu_price;
            UIManager.GetComponent<TabManager>().MenuDetailScene();
            UIManager.GetComponent<menu_detail>().Set_UI();
            
            //Debug.Log(Global_data.selected_menu_name);
        }
        else
        {
            Debug.LogError("Menu details are not provided!");
        }
    }

    public void search_button_click()
    {
        SearchMenus(search_field.text);
    }
}





