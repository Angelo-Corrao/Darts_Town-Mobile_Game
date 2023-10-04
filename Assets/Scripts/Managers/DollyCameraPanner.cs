using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Cinemachine;

public class DollyCameraPanner : MonoBehaviour {

    private CinemachineComposer virtualCamera;
    [SerializeField]
    private Vector3 startingaAnchorLocation;
    [SerializeField]
    private float anchorDragSpeed;

    void Start()
    {
        CinemachineVirtualCamera freeLook;
        freeLook = gameObject.GetComponent<CinemachineVirtualCamera>();
        virtualCamera = freeLook.GetCinemachineComponent<CinemachineComposer>();
        virtualCamera.m_TrackedObjectOffset = startingaAnchorLocation;
    }

    private void Update()
    {
        if (virtualCamera.m_TrackedObjectOffset.z>0)
            virtualCamera.m_TrackedObjectOffset.z -= Time.deltaTime * anchorDragSpeed * (startingaAnchorLocation.magnitude)/4;

    }
}
