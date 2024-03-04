using UnityEngine;
using UnityEngine.InputSystem;

namespace FS.Cores.Players
{
    [DefaultExecutionOrder(ScriptOrders.PLAYER_INPIUT_ORDER)]
    public class PlayerInputAction : MonoBehaviour
    {
        public static PlayerInputAction Instance;
        [SerializeField] private InputActionAsset m_InputAsset;
        
        public Vector2 MoveInput { get; private set; } = Vector2.zero;

        private void Awake()
        {
            Instance = this;
        }

        private void OnEnable()
        {
            m_InputAsset["W"].performed += SetW;
            m_InputAsset["W"].canceled += ClearMove;

            m_InputAsset["A"].performed += SetA;
            m_InputAsset["A"].canceled += ClearMove;

            m_InputAsset["S"].performed += SetS;
            m_InputAsset["S"].canceled += ClearMove;

            m_InputAsset["D"].performed += SetD;
            m_InputAsset["D"].canceled += ClearMove;

            m_InputAsset["Q"].performed += SetQ;
            m_InputAsset["Q"].canceled += SetQ;

            m_InputAsset["E"].performed += SetE;
            m_InputAsset["E"].canceled += SetE;
        }

        private void OnDisable()
        {
            m_InputAsset["W"].performed -= SetW;
            m_InputAsset["W"].canceled -= ClearMove;

            m_InputAsset["A"].performed -= SetA;
            m_InputAsset["A"].canceled -= ClearMove;

            m_InputAsset["S"].performed -= SetS;
            m_InputAsset["S"].canceled -= ClearMove;

            m_InputAsset["D"].performed -= SetD;
            m_InputAsset["D"].canceled -= ClearMove;

            m_InputAsset["Q"].performed -= SetQ;
            m_InputAsset["Q"].canceled -= SetQ;

            m_InputAsset["E"].performed -= SetE;
            m_InputAsset["E"].canceled -= SetE;
        }

        private void ClearMove(InputAction.CallbackContext context)
        {
            MoveInput = Vector2.zero;
        }

        private void SetW(InputAction.CallbackContext context)
        {
            MoveInput = new Vector2(0, 1);
        }

        private void SetA(InputAction.CallbackContext context)
        {
            MoveInput = new Vector2(-1, 0);
        }

        private void SetS(InputAction.CallbackContext context)
        {
            MoveInput = new Vector2(0, -1);
        }

        private void SetD(InputAction.CallbackContext context)
        {
            MoveInput = new Vector2(1, 0);
        }

        private void SetQ(InputAction.CallbackContext context)
        {

        }

        private void SetE(InputAction.CallbackContext context)
        {

        }
    }
}