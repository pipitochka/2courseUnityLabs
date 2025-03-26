using System.Collections;
using NUnit.Framework;
using Objects;
using UnityEngine;
using UnityEngine.TestTools;
using Singletons;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;
using Vector2 = System.Numerics.Vector2;

namespace Tests.PlayerTests
{
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

            var keyboardState = new KeyboardState(Key.W); 
            InputSystem.QueueStateEvent(keyboard, keyboardState); 
            InputSystem.Update(); 
            yield return null; 

            keyboardState = new KeyboardState(); 
            InputSystem.QueueStateEvent(keyboard, keyboardState); 
            InputSystem.Update(); 
            yield return null; 
            
            int[,] dataOut = new int[,]
            {
                { 8, 8, 8, 2 },
                { 0, 4, 4, 4 },
                { 0, 4, 0, 0 },
                { 0, 0, 0, 0 }
            };

            int[,] dataOutReal = field.getField();

            Assert.AreEqual(dataOut, dataOutReal);
            
            keyboardState = new KeyboardState(Key.D); 
            InputSystem.QueueStateEvent(keyboard, keyboardState); 
            InputSystem.Update(); 
            yield return null; 

            keyboardState = new KeyboardState(); 
            InputSystem.QueueStateEvent(keyboard, keyboardState); 
            InputSystem.Update(); 
            yield return null; 
            
            dataOut = new int[,]
            {
                { 0, 8, 16, 2 },
                { 0, 0, 4, 8 },
                { 0, 0, 0, 4 },
                { 0, 0, 0, 0 }
            };

            dataOutReal = field.getField();

            Assert.AreEqual(dataOut, dataOutReal);
        }
    }
}