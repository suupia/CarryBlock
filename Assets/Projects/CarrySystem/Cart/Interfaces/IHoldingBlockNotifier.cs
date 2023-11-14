namespace Projects.CarrySystem.Cart.Interfaces
{
    public interface IHoldingBlockNotifier
    {
        public void ShowReachableText();

        public void ShowMoveToCartText();

        public void HideReachableText();

        public void HideMoveToCartText();
    }
}