using UnityEngine;

public class ActivateCamera : MonoBehaviour
{
    [SerializeField]
    private GameObject quad;

    [SerializeField]
    private PasswordPool passwordPool;

    private Material quadMaterial;

    public PasswordPool _passwordPool { get { return passwordPool; } }

    private void Start()
    {
        quadMaterial = quad.GetComponent<MeshRenderer>().material;

        if (passwordPool == null)
            throw new UnassignedReferenceException();
    }

    public void EnabledCamera(int numberCamera, bool active)
    {
        Abbreviation(numberCamera, active);
    }

    private void Abbreviation(int value, bool active)
    {
        quad.SetActive(active);

        passwordPool._pool[value].camera.gameObject.SetActive(active);

        if (active == true) 
            quadMaterial.mainTexture = passwordPool._pool[value].camera.targetTexture;
        else 
            quadMaterial.mainTexture = null;
    }
}