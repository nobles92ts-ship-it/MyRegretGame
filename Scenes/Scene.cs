public abstract class Scene
{
    // 들어왔다 나갔다  갱신하고 출력 해야함
    public virtual void Enter()
    {}
    public virtual void Exit()
    {}
    public abstract void Update();
    public abstract void Render();
    
}