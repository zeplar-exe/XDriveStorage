using System.Collections;
using System.Diagnostics.CodeAnalysis;

using XDriveStorage.Users;

namespace XDriveStorage.Configuration;

public class UserContainer : IEnumerable<User>
{
    private Dictionary<string, User> Users { get; }
    
    public int Count => Users.Count;

    public UserContainer()
    {
        Users = new Dictionary<string, User>();
    }

    public bool Add(User user)
    {
        if (Users.ContainsKey(user.Id))
            return false;
        
        Users.Add(user.Id, user);

        return true;
    }

    public bool TryGet(string name, [NotNullWhen(true)] out User? user)
    {
        return Users.TryGetValue(name, out user);
    }

    public bool Exists(string id)
    {
        return Users.ContainsKey(id);
    }

    public bool Remove(string id)
    {
        return Users.Remove(id);
    }

    public IEnumerator<User> GetEnumerator()
    {
        return Users.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}