namespace TaskManager.Repositories.Services
{
    public class JwtCheck
{
    private HashSet<string> tokens = new HashSet<string>();


    public void Add(string token)
    {
        tokens.Add(token);
    }

    public bool Check(string token)
    {
        var exist = tokens.Where(x => x.Equals(token)).Any();
        return exist;
    }

}
}