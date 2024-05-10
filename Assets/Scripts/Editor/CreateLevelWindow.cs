using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
public class CreateLevelWindow : EditorWindow
{
    private int searchWordsCount;
    private int height;
    private int width;
    private string[,] letters = new string[10, 10];
    private string[] words;

    [MenuItem("Custom Tool/CreateLevelWindow")]
    private static void ShowWindow()
    {
        var window = GetWindow<CreateLevelWindow>();
        window.titleContent = new GUIContent("Create Level Window");
        window.Show();
    }

    private void OnEnable()
    {
        Clear();
    }

    public void CreateGUI()
    {
        var wrapper = Container();


        wrapper.Add(LeftPanel());
        wrapper.Add(RightPanel());
    }

    private void CustomRepaint()
    {
        rootVisualElement.Clear();

        var wrapper = Container();


        wrapper.Add(LeftPanel());
        wrapper.Add(RightPanel());
    }
    private VisualElement Container()
    {
        StyleSO mainStyle = Resources.Load<StyleSO>("Style/StyleSO");
        rootVisualElement.styleSheets.Add(mainStyle.GetStyle(StyleType.CreateWindow));
        var container = rootVisualElement;


        return container;
    }

    private VisualElement LeftPanel()
    {
        var panel = Create<VisualElement>("LeftPanel");

        for (int y = 0; y < height; y++)
        {
            var horizontalProp = Create<VisualElement>("Property");
            for (int x = 0; x < width; x++)
            {
                var field = Create<TextField>("CustomTextField");
                field.maxLength = 1;
                field.value = letters[y, x];
                int xPos = x;
                int yPos = y;
                field.RegisterValueChangedCallback((e) =>
                {
                    letters[yPos, xPos] = e.newValue;
                });
                horizontalProp.Add(field);
            }
            panel.Add(horizontalProp);
        }

        return panel;
    }

    private VisualElement RightPanel()
    {
        var panel = Create<VisualElement>("RightPanel");

        #region Search Word Count
        var wordCountWrapper = Create<VisualElement>("Property");


        var label = Create<Label>();
        label.text = "SearchWordCount";


        var fieldCount = Create<IntegerField>();
        fieldCount.value = searchWordsCount;
        fieldCount.RegisterValueChangedCallback((x) =>
        {
            searchWordsCount = x.newValue;
            words = new string[searchWordsCount];
            for (int i = 0; i < searchWordsCount; i++)
            {
                words[i] = "";
            }
            CustomRepaint();
        });

        wordCountWrapper.Add(label);
        wordCountWrapper.Add(fieldCount);
        panel.Add(wordCountWrapper);

        panel.Add(SearchWordsPanel());

        #endregion

        #region Grid Size
        var widthProperty = Create<VisualElement>("Property");

        var widthTitle = Create<Label>();
        widthTitle.text = "Width";

        var widthSlider = Create<SliderInt>("CustomSlider");
        widthSlider.value = width;
        widthSlider.lowValue = 1;
        widthSlider.highValue = 10;
        widthSlider.RegisterValueChangedCallback((x) =>
        {
            width = x.newValue;
            Clear();
            CustomRepaint();
        });

        widthProperty.Add(widthTitle);
        widthProperty.Add(widthSlider);

        panel.Add(widthProperty);

        var heightProperty = Create<VisualElement>("Property");


        var heightTitle = Create<Label>();
        heightTitle.text = "Height";

        var heightSlider = Create<SliderInt>("CustomSlider");
        heightSlider.value = height;
        heightSlider.lowValue = 1;
        heightSlider.highValue = 10;

        heightSlider.RegisterValueChangedCallback((x) =>
        {
            height = x.newValue;
            Clear();
            CustomRepaint();
        });

        heightProperty.Add(heightTitle);
        heightProperty.Add(heightSlider);

        panel.Add(heightProperty);



        #endregion

        #region Time
        var timeProperty = Create<VisualElement>("Property");

        var timeTitle = Create<Label>();
        timeTitle.text = "Time";

        var timeField = Create<IntegerField>();

        timeProperty.Add(timeTitle);
        timeProperty.Add(timeField);

        panel.Add(timeProperty);
        #endregion

        var saveButton = Create<Button>("CustomButton");
        saveButton.text = "Save";
        saveButton.clicked += () =>
        {
            SaveLevel();
        };
        panel.Add(saveButton);

        var randomizeButton = Create<Button>("CustomButton");
        randomizeButton.text = "Randomize";
        randomizeButton.clicked += () =>
        {
            RandomizeGridLetters();
            CustomRepaint();
        };
        panel.Add(randomizeButton);

        var clearButton = Create<Button>("CustomButton");
        clearButton.text = "Clear";
        clearButton.clicked += () =>
        {
            Clear();
            CustomRepaint();
        };
        panel.Add(clearButton);


        return panel;
    }

    private void RandomizeGridLetters()
    {
        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
            {
                var errorCount = Regex.Matches(letters[y, x], @"[a-zA-Z0-9ğüşöçıİĞÜŞÖÇ]+$").Count;
                var turkishLetters = "ABCÇDEFGĞHİIJKLMNOÖPRSŞTUÜVYZ";

                int index = Random.Range(0, turkishLetters.Length);

                if (errorCount == 0)
                {
                    letters[y, x] = turkishLetters[index].ToString();
                }
            }
    }

    private void Clear()
    {
        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                letters[y, x] = "";
    }

    private void SaveLevel()
    {
        LevelSO newLevel = CreateInstance<LevelSO>();
        newLevel.grid = new Grid(width, height);

        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
            {
                newLevel.grid.columns[y].rows[x] = letters[y, x];
            }


        for (int i = 0; i < searchWordsCount; i++)
        {
            newLevel.searchWords.Add(words[i]);
        }

        AssetDatabase.CreateAsset(newLevel, $"Assets/Resources/Levels/Level{GetTotalLevelCount() + 1}.asset");
    }

    private VisualElement SearchWordsPanel()
    {
        var box = Create<VisualElement>();

        for (int i = 0; i < searchWordsCount; i++)
        {
            var field = Create<TextField>();
            field.value = words[i];
            int index = i;
            field.RegisterValueChangedCallback((e) =>
            {
                words[index]=e.newValue;
            });
            box.Add(field);
        }

        return box;
    }

    private int GetTotalLevelCount()
    {
        return Resources.LoadAll<LevelSO>("Levels").Count();
    }
    private T Create<T>(params string[] classNames) where T : VisualElement, new()
    {
        var ele = new T();
        foreach (var name in classNames)
        {
            ele.AddToClassList(name);
        }

        return ele;
    }
}
