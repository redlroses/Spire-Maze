using System;

namespace CodeBase.Tools
{
    public class Locker
    {
        private readonly string _lockKey;

        public Locker(string lockKey)
        {
            if (string.IsNullOrWhiteSpace(lockKey))
                throw new ArgumentException($"Lock key must be unique: incorrect lock key {nameof(lockKey)}");

            _lockKey = lockKey;
        }

        public static implicit operator bool(Locker locker) =>
            locker != null;

        public static bool operator ==(Locker left, Locker right) =>
            Equals(left, right);

        public static bool operator !=(Locker left, Locker right) =>
            !Equals(left, right);

        public override int GetHashCode() =>
            _lockKey.GetHashCode();

        public override bool Equals(object obj) =>
            obj is Locker locker && locker._lockKey == _lockKey;
    }
}