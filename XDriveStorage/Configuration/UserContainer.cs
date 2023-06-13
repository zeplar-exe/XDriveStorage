using System.Collections;

using XDriveStorage.Users;

namespace XDriveStorage.Configuration;

public class UserContainer : IEnumerable<User>
{
    private Dictionary<string, User> Users { get; }

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

    public bool TryGet(string name, out User user)
    {
        return Users.TryGetValue(name, out user);
    }

    public bool Exists(string name)
    {
        return Users.ContainsKey(name);
    }

    public bool Remove(string name)
    {
        return Users.Remove(name);
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