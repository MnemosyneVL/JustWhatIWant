using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ExploreScreenCategory
{
    public CategoryTagItem mainTag;
    public Sprite categoryImage;
    public Sprite headerIllustration;
    public List<CategoryTagItem> subTags;
}

public class ExploreScreenManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private List<ExploreScreenCategory> categories = default;
    [Header("General References")]
    [SerializeField]
    RecommendationManagerMeta recommendationManager = default;

    [Header("CategoryEventsScreen References")]
    [SerializeField]
    private GameObject categoryEventsPage = default;
    [SerializeField]
    private Transform categoryEventsTransform = default;
    [SerializeField]
    private Image illustration = default; 
    [SerializeField]
    private TextMeshProUGUI categoryName = default;
    [SerializeField]
    private GameObject categoryListPrefab = default;

    [Header("CategoriesScreen References")]
    [SerializeField]
    private GameObject categoryButtonsPage = default;
    [SerializeField]
    private GameObject categoryButtonPrefab = default;
    [SerializeField]
    private Transform categoryButtonsContentTransform = default;

    private void Start()
    {
        EnableCategoryButtonsScreen();
        ConctructCategoryButtonsScreen();
    }

    #region CategoryScreen 
    public void EnableCategoryButtonsScreen()
    {
        categoryButtonsPage.SetActive(true);
        categoryEventsPage.SetActive(false);
    }
    private void ConctructCategoryButtonsScreen()
    {
        foreach (var category in categories)
        {
            GameObject instantiatedButton = Instantiate(categoryButtonPrefab, categoryButtonsContentTransform);
            CategoryButtonBuilder categoryButtonBuilder = instantiatedButton.GetComponent<CategoryButtonBuilder>();
            categoryButtonBuilder.ConstructCategoryButton(category);
            categoryButtonBuilder.onClickButtonAction += OnCategorySelected;
        }
    }

    #endregion

    #region CategoryEventsScreen

    public void OnCategorySelected(ExploreScreenCategory exploreScreenCategory)
    {
        EnableCategoriesScreen();
        ConstructCategoryEventsScreen(exploreScreenCategory);
    }
    private void EnableCategoriesScreen()
    {
        categoryButtonsPage.SetActive(false);
        categoryEventsPage.SetActive(true);
    }
    private void ConstructCategoryEventsScreen(ExploreScreenCategory exploreScreenCategory)
    {
        ClearCategoryEvent();
        illustration.sprite = exploreScreenCategory.headerIllustration;
        List<CategoryTagItem> categories = new List<CategoryTagItem>();
        categories.Add(exploreScreenCategory.mainTag);
        SetOfEvents allCategoryEvents = recommendationManager.RequestEvents(0, null, categories);
        categoryName.text = exploreScreenCategory.mainTag.tagName;
        foreach (var subTag in exploreScreenCategory.subTags)
        {
            List<CategoryTagItem> subCategories = new List<CategoryTagItem>();
            subCategories.Add(subTag);
            SetOfEvents eventsList = recommendationManager.RequestEvents(6, null, subCategories, allCategoryEvents.recommendedEvents);
            if(eventsList != null)
            {
                GameObject instantiatedEventsList = Instantiate(categoryListPrefab, categoryEventsTransform);
                CategoryListItemBuilder categoryEventBuilder = instantiatedEventsList.GetComponent<CategoryListItemBuilder>();
                categoryEventBuilder.ConstructCategoryItem(eventsList, subTag);
            }
            
        }
    }
    private void ClearCategoryEvent()
    {
        foreach (Transform child in categoryEventsTransform)
        {
            Destroy(child.gameObject);
        }
    }
    #endregion
}
