using System.Threading;

namespace ServerProject
{
public abstract class CustomThread
{
    private Thread _thread;

    protected CustomThread()
    {
        _thread = new Thread(new ThreadStart(this.RunThread));
    }

    // Thread methods / properties
    public void Start() => _thread.Start();
    public void Join() => _thread.Join();
    public bool IsAlive => _thread.IsAlive;

    // Override in base class
    public abstract void RunThread();
}
}