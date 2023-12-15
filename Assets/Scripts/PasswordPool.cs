using UnityEngine;

public class PasswordPool : MonoBehaviour
{
    [SerializeField]
    private Pool[] pool;

    public Pool[] _pool { get { return pool; } }
}

[System.Serializable]
public class Pool
{
    public string ip;

    public string password;

    public string nameCamera;

    public Camera camera;
}