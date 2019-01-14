using System;
using System.Collections.Generic;

namespace DomainDriven
{
    [Serializable]
    public abstract class PersistentObject<TKey>
        : IComparable<PersistentObject<TKey>>, IEquatable<PersistentObject<TKey>>, IPersistentObject
        where TKey : IComparable
    {
        public const string ID = "Id";

        public virtual TKey Id { get; set; }
        object IPersistentObject.Id { get { return Id; } }

        public virtual bool IsPersistent
        {
            get
            {
                return (!Equals(Id, default(TKey)));
            }
        }

        public virtual bool IsTransient
        {
            get { return !IsPersistent; }
        }

        public virtual int CompareTo(PersistentObject<TKey> other)
        {
            if (IsPersistent)
            {
                return Id.CompareTo(other.Id);
            }
            return 0;
        }

        public override bool Equals(object obj)
        {
            if (IsPersistent)
            {
                var persistentObject = obj as PersistentObject<TKey>;
                return (persistentObject != null) && (Equals(Id, persistentObject.Id));
            }

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return IsPersistent ? Id.GetHashCode() : base.GetHashCode();
        }

        public virtual bool Equals(PersistentObject<TKey> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return EqualityComparer<TKey>.Default.Equals(Id, other.Id);
        }

        public static bool operator ==(PersistentObject<TKey> left, PersistentObject<TKey> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(PersistentObject<TKey> left, PersistentObject<TKey> right)
        {
            return !Equals(left, right);
        }


    }
}