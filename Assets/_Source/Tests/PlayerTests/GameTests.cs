using System.Collections;
using NUnit.Framework;
using Objects;
using UnityEngine;
using UnityEngine.TestTools;
using Singletons;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class GameTests
{
    
    private Field field;
    private GameControlller gameController;
    private InputController inputController;
    private ColorManager colorManager;
    
    [SetUp]
        public void Setup()
        {
            SceneManager.LoadScene("SampleScene");
            
        }

    
    [UnityTest]
    public IEnumerator InputTest()
    {
        field = GameObject.FindObjectOfType<Field>();
        gameController = GameObject.FindObjectOfType<GameControlller>();
        inputController = GameObject.FindObjectOfType<InputController>();
        colorManager = GameObject.FindObjectOfType<ColorManager>();
        field.isTest = true;
        
        gameController.SetPoints(0);
        
        var keyboard = InputSystem.AddDevice<Keyboard>();
        var mouse = InputSystem.AddDevice<Mouse>();
        
        int[,] dataIn = new int[,]
        {
            { 0, 8, 8, 0 },
            { 4, 2, 4, 2 },
            { 0, 2, 0, 4 },
            { 4, 4, 0, 0 }
        };
    
        
        field.setField(dataIn);
    
        var keyboardState = new KeyboardState(Key.W); // Указываем, что клавиша W нажата
        InputSystem.QueueStateEvent(keyboard, keyboardState); // Передаем состояние клавиши
        InputSystem.Update();  // Обновляем систему ввода
        yield return null;  // Даем системе время для обработки

        keyboardState = new KeyboardState(); // Пустой конструктор - все клавиши отпущены
        InputSystem.QueueStateEvent(keyboard, keyboardState); // Передаем обновленное состояние клавиши
        InputSystem.Update();  // Обновляем систему ввода
        yield return null;  // Даем системе время для обработки


        
        int[,] dataOut = new int[,]
        {
            { 8, 8, 8, 2 },
            { 0, 4, 4, 4 },
            { 0, 4, 0, 0 },
            { 0, 0, 0, 0 }
        };
    
        int[,] dataOutReal = field.getField();
        
        Assert.AreEqual(dataOut, dataOutReal);
    }
}
