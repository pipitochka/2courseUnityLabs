using FluentAssertions;
using NUnit.Framework;
using TMPro;
using UnityEditor.SceneManagement;
using UnityEngine;
using Singletons;
using Objects;

namespace Tests.EditorTests
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
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            GameObject fieldGameObject = new GameObject("FieldObject");
            field = fieldGameObject.AddComponent<Field>();
            
            var rt = fieldGameObject.AddComponent<RectTransform>(); 
            var cellViewPrefab = new GameObject("CellViewPrefab").AddComponent<CellView>(); 
            var saveLoadManager = new GameObject("SaveLoadManager").AddComponent<SaveLoadManager>(); 
            SaveLoadManager.Instance = saveLoadManager;
            
            field.setRt(rt); 
            field.setCellView(cellViewPrefab);
            field.setSaveLoadManager(saveLoadManager);

            field.FieldSize = 4;
            field.CellSize = 1f;
            field.Splacing = 0.1f;
            field.StartCellsCount = 2;
            field.isTest = true;
            
            Field.Instance = field;
            
            GameObject gameControllerObject = new GameObject("GameController");
            gameController = gameControllerObject.AddComponent<GameControlller>();
            GameControlller.Instance = gameController;
            var pointsText = new GameObject("PointsText").AddComponent<TextMeshProUGUI>();
            var gameResultText = new GameObject("GameResultText").AddComponent<TextMeshProUGUI>();
            
            gameController.setPointsText(pointsText);
            gameController.setGameResult(gameResultText);
            
            
            
            GameObject inputContollerObject = new GameObject("InputController");
            inputController = inputContollerObject.AddComponent<InputController>();

            GameObject colorManagerObject = new GameObject("Color Manager");
            colorManager = colorManagerObject.AddComponent<ColorManager>();
            ColorManager.Instance = colorManager;
            
            gameController.StartGame();
            field.CreateField();
        }

        [Test]
        public void EmptyTest()
        {
            int a = 2;
            int b = 3;
            (a + b).Should().Be(5);
        }

        [Test]
        public void TestFieldIsNotNull()
        {
            field.Should().NotBeNull();
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

            dataIn.Should().BeEquivalentTo(dataOut);
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

            dataOut.Should().BeEquivalentTo(dataOutReal);
        }

        [Test]
        public void ScoreAndMovementTest()
        {
            gameController.SetPoints(0);
            
            int[,] dataIn = new int[,]
            {
                { 2, 2, 0, 0 },
                { 4, 2, 4, 0 },
                { 2, 2, 4, 4 },
                { 16, 0, 16, 0 }
            };
            field.setField(dataIn);
            
            field.OnInput(Vector2.left);

            int[,] dataOut1 = new int[,]
            {
                { 4, 0, 0, 0 },
                { 4, 2, 4, 0 },
                { 4, 8, 0, 0 },
                { 32, 0, 0, 0 }
            };

            int[,] dataOutReal1 = field.getField();
            
            dataOut1.Should().BeEquivalentTo(dataOutReal1);
            
            gameController.GetPoints().Should().Be(48);
            
            field.OnInput(Vector2.down);
            
            int[,] dataOut2 = new int[,]
            {
                { 0, 0, 0, 0 },
                { 4, 0, 0, 0 },
                { 8, 2, 0, 0 },
                { 32, 8, 4, 0 }
            };

            int[,] dataOutReal2 = field.getField();
            
            dataOut1.Should().BeEquivalentTo(dataOutReal1);
            
            gameController.GetPoints().Should().Be(56);
        }
        
        
        
    }
}

