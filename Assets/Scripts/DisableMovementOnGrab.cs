using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DisableMovementOnGrab : MonoBehaviour
{
    public ActionBasedContinuousMoveProvider moveProvider;
    public XRRayInteractor rayInteractor;

    private void OnEnable()
    {
        rayInteractor.selectEntered.AddListener(OnGrab);
        rayInteractor.selectExited.AddListener(OnRelease);
    }

    private void OnDisable()
    {
        rayInteractor.selectEntered.RemoveListener(OnGrab);
        rayInteractor.selectExited.RemoveListener(OnRelease);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        if (moveProvider != null)
            moveProvider.enabled = false;
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        if (moveProvider != null)
            moveProvider.enabled = true;
    }
}
