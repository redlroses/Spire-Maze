namespace CodeBase.Services.Sound
{
    public class SoundLocker
    {
        private readonly string _lockKey;

        public SoundLocker(string lockKey)
        {
            _lockKey = lockKey;
        }

        public override int GetHashCode() =>
            _lockKey.GetHashCode();

        public override bool Equals(object obj) =>
            obj is SoundLocker locker && locker._lockKey == _lockKey;

        public static implicit operator bool(SoundLocker locker) => locker != null;

        public static bool operator ==(SoundLocker left, SoundLocker right) =>
            Equals(left, right);

        public static bool operator !=(SoundLocker left, SoundLocker right) =>
            !Equals(left, right);
    }
}