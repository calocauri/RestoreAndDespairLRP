public abstract class PoolableObject : UnityEngine.MonoBehaviour {

    protected GenericObjectPool pool;

    public void ReturnToPool() {
        pool.Return(this);
    }

    public virtual void OnCreatedFromPool(GenericObjectPool pool) {
        this.pool = pool;
    }

    public abstract void OnGetFromPool();
    public abstract void OnReturnToPool();

}