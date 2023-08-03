namespace FH.UI {
    public interface IView {
        void Init();
    }

    public abstract class ViewBase : IView {
        public abstract void Init();
    }
}