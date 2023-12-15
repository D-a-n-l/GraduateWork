using Cinemachine;

public class VirtualCameraControl
{
    public static void Control(CinemachineVirtualCamera virtualCamera, float value, float valueMin, float valueMax, bool wrap)
    {
        virtualCamera.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MinValue = valueMin;
        virtualCamera.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxValue = valueMax;

        virtualCamera.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.Value = value;

        virtualCamera.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_Wrap = wrap;
    }
}