namespace TeamTheDream.Delivery
{
    public interface IPickable
    {
        event System.Action<int> OnAmountChanged;
        bool CanPick();
        void Pick();
    }
}