namespace InputReader
{
    public interface IEntityInputSource
    {
        float HorizontalDirection { get; }
        bool Jumped { get; }
        bool AttackStarted { get; }

        void ResetOneTimeActions();
    }
}