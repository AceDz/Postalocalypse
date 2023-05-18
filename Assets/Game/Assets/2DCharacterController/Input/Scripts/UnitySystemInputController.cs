#if ENABLE_INPUT_SYSTEM
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TeamTheDream
{
    [System.Serializable]
    public class UnitySystemInputController : IInputController
    {
        public enum EAction
        {
            PlayerMove,
            PlayerJump,
            PlayerDash,
            PlayerAttack,
            PlayerRangeAttack,
            PlayerParry,
            PlayerInteract
        }


        [SerializeField]
        private PlayerInput _input;

        [SerializeField]
        private float _runSensibility = 0f;

        [ShowInInspector, HideInEditorMode, BoxGroup("Game State")]
        private Vector2 _move;

        [ShowInInspector, HideInEditorMode, BoxGroup("Game State")]
        private bool _jumpDown;

        [ShowInInspector, HideInEditorMode, BoxGroup("Game State")]
        private bool _jump;

        [ShowInInspector, HideInEditorMode, BoxGroup("Game State")]
        private bool _interactDown;

        [ShowInInspector, HideInEditorMode, BoxGroup("Game State")]
        private bool _attackDown;

        public Vector2 Move => _move;
        public bool Crouch => _move.y < -0.5f;
        public bool MoveDown => _move.y < -0.2f;
        public bool MoveUp => _move.y > 0.2f;
        public bool MoveLeft => _move.x < -0.2f;
        public bool MoveRight => _move.x > 0.2f;
        public bool RunLeft => _move.x < -_runSensibility;
        public bool RunRight => _move.x > _runSensibility;

        public bool JumpDown => _jumpDown;
        public bool Jump => _jump;

        public bool InteractDown => _interactDown;

        public bool AttackDown => _attackDown;

        public void Init()
        {
            for (int i = 0; i < _input.actionEvents.Count; ++i)
            {
                Debug.Log(_input.actionEvents[i].actionName);
            }

            _input.actionEvents[(int)EAction.PlayerMove].AddListener(Input_OnMove);
            _input.actionEvents[(int)EAction.PlayerJump].AddListener(Input_OnJump);
            _input.actionEvents[(int)EAction.PlayerInteract].AddListener(Input_OnInteract);
            _input.actionEvents[(int)EAction.PlayerAttack].AddListener(Input_OnAttack);
        }

        public void Dispose()
        {
            for (int i = 0; i < _input.actionEvents.Count; ++i)
            {
                _input.actionEvents[i].RemoveAllListeners();
            }
        }

        public void CleanInput()
        {
            _move = Vector2.zero;
            _jumpDown = false;
            _jump = false;
            _interactDown = false;
            _attackDown = false;
        }

        public void UpdateInput()
        {
            InputSystem.Update();
            if (_jump)
            {
                _jumpDown = _input.actions["Jump"].WasPressedThisFrame();
            }
            if (_interactDown)
            {
                _interactDown = _input.actions["Interact"].WasPressedThisFrame();
            }
            if (_attackDown)
            {
                _attackDown = _input.actions["Attack"].WasPressedThisFrame();
            }
        }

        private void Input_OnMove(InputAction.CallbackContext action)
        {
            switch (action.phase)
            {
                case InputActionPhase.Started:
                    _move = action.action.ReadValue<Vector2>();
                    break;
                case InputActionPhase.Performed:
                    _move = action.action.ReadValue<Vector2>();
                    break;
                case InputActionPhase.Canceled:
                    _move = Vector2.zero;
                    break;
                case InputActionPhase.Disabled:
                    _move = Vector2.zero;
                    break;
            }
        }

        private void Input_OnJump(InputAction.CallbackContext action)
        {
            switch (action.phase)
            {
                case InputActionPhase.Started:
                    _jumpDown = true;
                    _jump = true;
                    break;
                case InputActionPhase.Canceled:
                    _jumpDown = false;
                    _jump = false;
                    break;
                case InputActionPhase.Disabled:
                    _jumpDown = false;
                    _jump = false;
                    break;
            }
        }

        private void Input_OnInteract(InputAction.CallbackContext action)
        {
            switch (action.phase)
            {
                case InputActionPhase.Started:
                    _interactDown = true;
                    break;
            }
        }

        private void Input_OnAttack(InputAction.CallbackContext action)
        {
            switch (action.phase)
            {
                case InputActionPhase.Started:
                    _attackDown = true;
                    break;
            }
        }
    }
}
#endif