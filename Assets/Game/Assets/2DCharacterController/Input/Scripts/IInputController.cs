using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamTheDream
{
    public interface IInputController
    {
        void Init();
        void Dispose();
        void UpdateInput();
        void CleanInput();
        Vector2 Move { get; }
        bool Crouch { get; }
        bool MoveUp { get; }
        bool MoveDown { get; }
        bool MoveLeft { get; }
        bool MoveRight { get; }
        bool RunLeft { get; }
        bool RunRight { get; }
        bool JumpDown { get; }
        bool Jump { get; }
        bool InteractDown { get; }
        bool AttackDown { get; }
    }
}