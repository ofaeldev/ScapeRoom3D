using UnityEngine;
using UnityEngine.Events;

namespace PlayerInput
{
    public class InputBlocker : MonoBehaviour
    {
        // Evento disparado quando o input é bloqueado
        public UnityEvent OnBlocked;

        // Evento disparado quando o input é desbloqueado
        public UnityEvent OnUnblocked;

        // Se o input está atualmente bloqueado
        public bool IsBlocked { get; private set; } = false;

        /// <summary>
        /// Bloqueia o input do jogador.
        /// </summary>
        public void Block()
        {
            if (IsBlocked) return;

            IsBlocked = true;
            Debug.Log("[InputBlocker] Input bloqueado.");
            OnBlocked?.Invoke();
        }

        /// <summary>
        /// Desbloqueia o input do jogador.
        /// </summary>
        public void Unblock()
        {
            if (!IsBlocked) return;

            IsBlocked = false;
            Debug.Log("[InputBlocker] Input desbloqueado.");
            OnUnblocked?.Invoke();
        }

        /// <summary>
        /// Alterna entre bloqueado e desbloqueado.
        /// </summary>
        public void Toggle()
        {
            if (IsBlocked) Unblock();
            else Block();
        }
    }
}
