using Microsoft.AspNetCore.Http;
using System.Text;

namespace xUnitBackendTest;

public class MockHttpSession : ISession
{
    Dictionary<string, object> sessionStorage = new Dictionary<string, object>();

    public object this[string name]
    {
        get { return sessionStorage[name]; }
        set { sessionStorage[name] = value; }
    }

    void ISession.Set(string key, byte[] value)
    {
        sessionStorage[key] = value;
    }

    bool ISession.TryGetValue(string key, out byte[] value)
    {
        if (sessionStorage[key] != null)
        {
            value = Encoding.ASCII.GetBytes(sessionStorage[key].ToString());
            return true;
        }
        else
        {
            value = null;
            return false;
        }
    }

    // Not nessisary for mocking

    IEnumerable<string> ISession.Keys
    {
        get { throw new NotImplementedException(); }
    }

    string ISession.Id
    {
        get { throw new NotImplementedException(); }
    }

    bool ISession.IsAvailable
    {
        get { throw new NotImplementedException(); }
    }

    void ISession.Clear()
    {
        throw new NotImplementedException();
    }

    void ISession.Remove(string key)
    {
        throw new NotImplementedException();
    }

    Task ISession.CommitAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    Task ISession.LoadAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    } 
}