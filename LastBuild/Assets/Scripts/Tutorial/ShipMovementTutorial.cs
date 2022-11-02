using Player.Driver;

namespace Tutorial
{
    public class ShipMovementTutorial : ShipMovement
    {
        public bool JumpLocked;
        public bool CanMove;

        protected override void JumpAction()
        {
            if (JumpLocked) return;

            base.JumpAction();
        }

        protected override void HandleMovement()
        {
            if (!CanMove) return;

            base.HandleMovement();
        }
    }
}