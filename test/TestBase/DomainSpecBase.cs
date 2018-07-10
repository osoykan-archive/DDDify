namespace TestBase
{
    public abstract class DomainSpecBase
    {
        protected T Random<T>() => TestBase.Random<T>._;
    }
}
