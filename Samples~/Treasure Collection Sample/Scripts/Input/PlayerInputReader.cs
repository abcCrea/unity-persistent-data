using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace abcCrea.PersistentData.Samples.Input
{
    public static class PlayerInputReader
    {
        public static Vector2 ReadMovement()
        {
            Vector2 input = ReadMovementInput();
            return Vector2.ClampMagnitude(input, 1f);
        }

        private static Vector2 ReadMovementInput()
        {
#if ENABLE_INPUT_SYSTEM
            if (Keyboard.current != null)
            {
                Vector2 input = Vector2.zero;

                if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) input.x -= 1f;
                if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) input.x += 1f;
                if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) input.y -= 1f;
                if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) input.y += 1f;

                return input;
            }
#endif

#if ENABLE_LEGACY_INPUT_MANAGER
            return new Vector2(UnityEngine.Input.GetAxisRaw("Horizontal"), UnityEngine.Input.GetAxisRaw("Vertical"));
#else
            return Vector2.zero;
#endif
        }
    }
}