namespace TestingTypedHttpClient.HttpClients;

public interface ISomeOtherDependency
{
    void DoNothig();
}

public class SomeOtherDependency : ISomeOtherDependency
{
    public void DoNothig() { }
}