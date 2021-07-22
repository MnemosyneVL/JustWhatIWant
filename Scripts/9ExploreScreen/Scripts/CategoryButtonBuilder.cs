using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CategoryButtonBuilder : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField]
    private TextMeshProUGUI categoryName = default;
    [SerializeField]
    private Image _categoryImage = default;
    [SerializeField]
    private Button _categoryButton = default;
    // Other Fields
    private ExploreScreenCategory storedCategoryName = default;
    public Action<ExploreScreenCategory> onClickButtonAction; 
    public void ConstructCategoryButton(ExploreScreenCategory categoryObject)
    {
        storedCategoryName = categoryObject;
        categoryName.text = categoryObject.mainTag.tagName;
        if(categoryObject.categoryImage != null)
        {
            _categoryImage.sprite = categoryObject.categoryImage;
        }
        _categoryButton.onClick.AddListener(delegate { onClickButtonAction(storedCategoryName); });

    }
}
