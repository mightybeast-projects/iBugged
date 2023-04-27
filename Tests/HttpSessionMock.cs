using System.Diagnostics.CodeAnalysis;

namespace iBugged.Tests;

public class HttpSessionMock : ISession
{
    Dictionary<string, object> sessionStorage
        = new Dictionary<string, object>();

    public object this[string name]
    {
        get { return sessionStorage[name]; }
        set { sessionStorage[name] = value; }
    }

    public IEnumerable<string> Keys { get { return sessionStorage.Keys; } }

    public void Clear() => sessionStorage.Clear();

    public void Remove(string key) => sessionStorage.Remove(key);

    public void Set(string key, byte[] value)
        => sessionStorage[key] = value;

    public bool TryGetValue(string key, [NotNullWhen(true)] out byte[]? value)
    {
        if (sessionStorage[key] != null)
        {
            value = (byte[])sessionStorage[key];
            return true;
        }

        value = null;
        return false;
    }

    public bool IsAvailable => throw new NotImplementedException();

    public string Id => throw new NotImplementedException();

    public Task CommitAsync(CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    public Task LoadAsync(CancellationToken cancellationToken = default)
        => throw new NotImplementedException();
}