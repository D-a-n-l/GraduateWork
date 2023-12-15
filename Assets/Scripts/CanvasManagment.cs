using UnityEngine;

public class CanvasManagment : MonoBehaviour
{
    [SerializeField] 
    private GameObject mainPageLaptop;

    [SerializeField] 
    private GameObject cameraPageLaptop;

    [SerializeField] 
    private GameObject offlinePageLaptop;

    private void Start()
    {
        if (mainPageLaptop == null || cameraPageLaptop == null || offlinePageLaptop == null)
            throw new UnassignedReferenceException();
    }

    public void Enabled(bool mainPage, bool cameraPage, bool offlinePage)
    {
        mainPageLaptop.SetActive(mainPage);

        cameraPageLaptop.SetActive(cameraPage);

        offlinePageLaptop.SetActive(offlinePage);
    }
}