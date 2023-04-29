namespace InputReader
{
    public interface IEntityInputSource
    {
        float HorizontalDirection { get; }
        bool IsJumping { get; }
        bool IsAttacking { get; }

        void ResetOneTimeActions();
    }
}