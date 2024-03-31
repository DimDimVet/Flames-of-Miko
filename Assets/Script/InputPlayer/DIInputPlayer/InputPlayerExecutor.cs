using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    public enum Mode
    {
        Turn,
        AvtoRif
    }
    public class InputPlayerExecutor : IInputPlayerExecutor
    {
        private Mode[] modes = { Mode.Turn, Mode.AvtoRif };
        private int countMode = 0;
        private bool isTrigerClick = true;
        private InputData inputData;
        private InputActions inputActions;

        public Action<InputData> OnEventUpdata { get { return onEventUpdata; } set { onEventUpdata = value; } }
        private Action<InputData> onEventUpdata;
        //Дополнение
        public Action OnNormSpeed { get { return onNormSpeed; } set { onNormSpeed = value; } }
        private Action onNormSpeed;
        public void Enable()
        {
            inputData = new InputData();
            inputData.Modes = modes;
            inputActions = new InputActions();
            if (inputActions != null)
            {
                //Карта Key
                {
                    inputActions.KeyMap.WASD.started += contex => inputData.Move = contex.ReadValue<Vector2>();
                    inputActions.KeyMap.WASD.performed += contex => inputData.Move = contex.ReadValue<Vector2>();
                    inputActions.KeyMap.WASD.canceled += contex => inputData.Move = contex.ReadValue<Vector2>();

                    inputActions.KeyMap.Look.started += contex => { inputData.Mouse = contex.ReadValue<Vector2>(); inputData.MousePosition = Mouse.current.position.ReadValue(); };
                    inputActions.KeyMap.Look.performed += contex => { inputData.Mouse = contex.ReadValue<Vector2>(); inputData.MousePosition = Mouse.current.position.ReadValue(); };
                    inputActions.KeyMap.Look.canceled += contex => { inputData.Mouse = contex.ReadValue<Vector2>(); inputData.MousePosition = Mouse.current.position.ReadValue(); };

                    inputActions.KeyMap.MouseLeftButton.started += context => { inputData.MouseLeftButton = context.ReadValue<float>(); inputData.MousePosition = Mouse.current.position.ReadValue(); };
                    inputActions.KeyMap.MouseLeftButton.performed += context => { inputData.MouseLeftButton = context.ReadValue<float>(); inputData.MousePosition = Mouse.current.position.ReadValue(); };
                    inputActions.KeyMap.MouseLeftButton.canceled += context => { inputData.MouseLeftButton = context.ReadValue<float>(); inputData.MousePosition = Mouse.current.position.ReadValue(); };

                    inputActions.KeyMap.MouseMiddleButton.started += context => { inputData.MouseMiddleButton = context.ReadValue<float>(); inputData.MousePosition = Mouse.current.position.ReadValue(); };
                    inputActions.KeyMap.MouseMiddleButton.performed += context => { inputData.MouseMiddleButton = context.ReadValue<float>(); inputData.MousePosition = Mouse.current.position.ReadValue(); };
                    inputActions.KeyMap.MouseMiddleButton.canceled += context => { inputData.MouseMiddleButton = context.ReadValue<float>(); inputData.MousePosition = Mouse.current.position.ReadValue(); };

                    inputActions.KeyMap.MouseRightButton.started += context => { inputData.MouseRightButton = context.ReadValue<float>(); inputData.MousePosition = Mouse.current.position.ReadValue(); };
                    inputActions.KeyMap.MouseRightButton.performed += context => { inputData.MouseRightButton = context.ReadValue<float>(); inputData.MousePosition = Mouse.current.position.ReadValue(); };
                    inputActions.KeyMap.MouseRightButton.canceled += context => { inputData.MouseRightButton = context.ReadValue<float>(); inputData.MousePosition = Mouse.current.position.ReadValue(); };

                    inputActions.KeyMap.Executor.started += context => { inputData.Executor = context.ReadValue<float>(); EventUpdata(inputData); };
                    inputActions.KeyMap.Executor.performed += context => { inputData.Executor = context.ReadValue<float>(); };
                    inputActions.KeyMap.Executor.canceled += context => { inputData.Executor = context.ReadValue<float>(); EventUpdata(inputData); };

                    inputActions.KeyMap.Menu.started += context => { inputData.Menu = context.ReadValue<float>(); EventUpdata(inputData); };
                    inputActions.KeyMap.Menu.performed += context => { inputData.Menu = context.ReadValue<float>(); };
                    inputActions.KeyMap.Menu.canceled += context => { inputData.Menu = context.ReadValue<float>(); };

                    inputActions.KeyMap.Mode.started += context => { inputData.Mode = context.ReadValue<float>(); SelectMoveMode(); EventUpdata(inputData); };
                    inputActions.KeyMap.Mode.performed += context => { inputData.Mode = context.ReadValue<float>(); };
                    inputActions.KeyMap.Mode.canceled += context => { inputData.Mode = context.ReadValue<float>(); };
                }
                //Карта UI
                {
                    inputActions.UIMap.WASDUI.started += contex => inputData.Move = contex.ReadValue<Vector2>();
                    inputActions.UIMap.WASDUI.performed += contex => inputData.Move = contex.ReadValue<Vector2>();
                    inputActions.UIMap.WASDUI.canceled += contex => inputData.Move = contex.ReadValue<Vector2>();
                }

                inputActions.Enable();
                EventUpdata(inputData);
            }
        }
        public void OnDisable()
        {
            inputActions.Disable();
        }
        private void SelectMoveMode()
        {
            if (inputData.Mode != 0)
            {
                if (isTrigerClick)
                {
                    isTrigerClick = false;
                    countMode++;
                    if (countMode >= modes.Length) { countMode = 0; }

                    for (int i = 0; i < modes.Length; i++)
                    {
                        if ((int)modes[i] == countMode)
                        {
                            inputData.ModeAction = (Mode)countMode;
                        }
                    }
                    isTrigerClick = true;
                }
            }
        }
        public InputData Updata()
        {
            return inputData;
        }
        private void EventUpdata(InputData inputData)
        {
            onEventUpdata?.Invoke(inputData);
        }
        //Дополнение
        public void NormSpeed()
        {
            onNormSpeed?.Invoke();
        }
    }
}

