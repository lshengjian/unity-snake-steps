public interface ICommand
{
    void Execute();
}
public static class CounterModel
{
    public static BindableProperty<int> Counter = new BindableProperty<int>()
    {
        Value = 0
    };
}
public struct IncreaseCountCommand : ICommand
{
    public void Execute()
    {
        CounterModel.Counter.Value++;
    }
}

public struct DecreaseCountCommand : ICommand
{
    public void Execute()
    {
        CounterModel.Counter.Value--;
    }
}