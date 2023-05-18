namespace TeamTheDream.Delivery
{
    public interface IDeliveryInteractable
    {
        void CanInteract(PlayerMovementController player, bool can);
        void Interact(PlayerMovementController player);
    }
}