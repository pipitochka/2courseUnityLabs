using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;


public class GameTests
{
    private Field field;
    
    [SetUp]
    public void Setup()
    {
        // Создание новой сцены в редакторе с помощью EditorSceneManager
        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        // Создаем GameObject для Field и добавляем компонент Field
        GameObject fieldGameObject = new GameObject("FieldObject");
        field = fieldGameObject.AddComponent<Field>();
        
        var rt = fieldGameObject.AddComponent<RectTransform>();  // Добавление RectTransform
        var cellViewPrefab = new GameObject("CellViewPrefab").AddComponent<CellView>();  // Создание нового объекта для CellView
        var saveLoadManager = new GameObject("SaveLoadManager").AddComponent<SaveLoadManager>();  // Создание нового объекта для SaveLoadManager
        
        field.rt = rt;
        field.cellViewPrefab = cellViewPrefab;
        field.saveLoadManager = saveLoadManager;
        
        field.FieldSize = 4;
        field.CellSize = 1f;
        field.Splacing = 0.1f;
        field.StartCellsCount = 2;
        
        field.isTest = true;
        
        field.CreateField();
    }
    
    [Test] 
    public void EmptyTest()
    {
        int a = 2;
        int b = 3;
        Assert.AreEqual(5, a + b); 
    }
    
    [Test]
    public void TestFieldIsNotNull()
    {
        Assert.NotNull(field);
    }
    
    [Test] 
    public void SetTest()
    {
        int[,] dataIn = new int[,]
        {
            { 2, 2, 0, 0 },
            { 4, 0, 4, 0 },
            { 2, 2, 2, 2 },
            { 0, 0, 0, 0 }
        };
        
        field.setField(dataIn);

        int[,] dataOut;
        
        dataOut = field.getField();
        
        Assert.AreEqual(dataIn, dataOut);
    }

    [Test]
    public void MovementTest()
    {
        int[,] dataIn = new int[,]
        {
            { 2, 2, 0, 0 },
            { 4, 0, 4, 0 },
            { 2, 2, 2, 2 },
            { 0, 2, 0, 0 }
        };

        field.setField(dataIn);

        int[,] dataOut;

        field.Move(Vector2.right);

        dataOut = field.getField();

        int[,] dataOutReal = new int[,]
        {
            { 0, 0, 0, 4 },
            { 0, 0, 0, 8 },
            { 0, 0, 4, 4 },
            { 0, 0, 0, 2 }
        };

        Assert.AreEqual(dataOut, dataOutReal);
    }
    
}