using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
using UnityEngine.UI;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;
		public bool aim;

        [SerializeField] private float maxStamina = 100f;
        [SerializeField] private float currentStamina = 100f;

        public float staminaDrainRate = 20f;
        public float staminaRegenRate = 10f;

        private bool canSprint = true;

        [Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

		public GameManager gm;
		public Slider slider;


#if ENABLE_INPUT_SYSTEM
        public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}

		public void OnAim(InputValue value)
		{
			AimInput(value.isPressed);
		}
#endif


		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

        public void SprintInput(bool newSprintState)
        {
            sprint = canSprint && newSprintState;
        }
        public void AimInput(bool newAimState)
        {
            aim = newAimState;
        }

        private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}

        private void Update()
        {
            bool isMoving = move.sqrMagnitude > 0.01f;

            if (canSprint && sprint && isMoving)
            {
                currentStamina -= staminaDrainRate * Time.deltaTime;

                if (currentStamina <= 5f)
                {
                    canSprint = false;
                    sprint = false;
				}
            }
            else
            {
                currentStamina += staminaRegenRate * Time.deltaTime;

                if (currentStamina >= 40f)
                {
                    canSprint = true;
                }
            }

            currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
			
			slider.value = currentStamina / maxStamina; 
        }
    }
	
}