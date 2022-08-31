using System;

namespace FakeAPI.Api;

public abstract class IdBase<T>
    where T : notnull
{
    private readonly T _id;
    public T Id => _id;
    public IdBase(T id)
    {
        _id = id;
    }

    public override bool Equals(object? obj)
    {
        if(obj is IdBase<T> id)
        {
            return this.Id.Equals(id.Id);
        }
        
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_id);
    }

    public override string ToString()
    {
        return Id.ToString() ?? "";
    }
}