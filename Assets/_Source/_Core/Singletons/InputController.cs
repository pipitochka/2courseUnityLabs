using UnityEngine;
using UnityEngine.InputSystem;


public class InputController : MonoBehaviour
{
    private GameInput gameInput;
    private Field field;
    
    private Vector2 touchStartPosition;
    private bool isTouching = false;

    public void Awake()
    {
        gameInput = new GameInput();
        gameInput.Enable();
        
        field = GetComponent<Field>();

        gameInput.Gameplay.Move.performed += OnMove;
        gameInput.Gameplay.MouseSwipe.performed += OnMouseSwipe;
        gameInput.Gameplay.ScreenSwipe.performed += OnScreenSwipe;
        
        gameInput.Gameplay.MouseSwipe.canceled += OnMouseSwipeEnd;
        gameInput.Gameplay.ScreenSwipe.canceled += OnScreenSwipeEnd;

    }
    
    private void OnMouseSwipe(InputAction.CallbackContext context)
    {
        if (!isTouching)
        {
            touchStartPosition = Mouse.current.position.ReadValue();
            isTouching = true;
        }
    }
    
    private void OnScreenSwipe(InputAction.CallbackContext context)
    {
        if (!isTouching)
        {
            touchStartPosition = context.ReadValue<Vector2>();
            isTouching = true;
        }
    }
    
    
    private void OnMouseSwipeEnd(InputAction.CallbackContext context)
    {
        isTouching = false;  
        var touchEndPosition = Mouse.current.position.ReadValue();;
        var vec = touchEndPosition - touchStartPosition;
        if (vec.magnitude > 50)
        {
            if (Mathf.Abs(vec.x) > Mathf.Abs(vec.y))
            {
                if (field)
                {
                    Debug.Log($"Mouse {vec.x}");
                    field.OnInput(vec.x > 0 ? Vector2.right : Vector2.left);
                }
            }
            else
            {
                if (field)
                {
                    Debug.Log($"Mouse {vec.y}");
                    field.OnInput(vec.y > 0 ? Vector2.up : Vector2.down);
                }
            }
        }
    }

    private void OnScreenSwipeEnd(InputAction.CallbackContext context)
    {
        isTouching = false;  
        Vector2 touchEndPosition = context.ReadValue<Vector2>();
        var vec = touchEndPosition - touchStartPosition;
        if (vec.magnitude > 50)
        {
            if (Mathf.Abs(vec.x) > Mathf.Abs(vec.y))
            {
                if (field)
                {
                    Debug.Log($"Mouse {vec.x}");
                    field.OnInput(vec.x > 0 ? Vector2.left : Vector2.right);
                }
            }
            else
            {
                if (field)
                {
                    Debug.Log($"Mouse {vec.y}");
                    field.OnInput(vec.y > 0 ? Vector2.up : Vector2.down);
                }
            }
        }
    }
    
    private void OnMove(InputAction.CallbackContext context)
    {
        Vector2 inputVector = context.ReadValue<Vector2>();
        if (field)
        {
            Debug.Log($"Keyboard {inputVector}");
            field.OnInput(inputVector);
        }
    }

    private void OnDestroy()
    {
        gameInput.Gameplay.Move.performed -= OnMove;
        gameInput.Gameplay.MouseSwipe.performed -= OnMouseSwipe;
        gameInput.Gameplay.ScreenSwipe.performed -= OnScreenSwipe;

        gameInput.Gameplay.MouseSwipe.canceled -= OnMouseSwipeEnd;
        gameInput.Gameplay.ScreenSwipe.canceled -= OnScreenSwipeEnd;
    }

}
