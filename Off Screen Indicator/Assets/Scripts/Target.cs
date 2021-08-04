using UnityEngine;

/// <summary>
/// Attach this script to all the target game objects in the scene.
/// </summary>
[DefaultExecutionOrder(0)]
public class Target : MonoBehaviour
{
    [Tooltip("Change this color to change the indicators color for this target")]
    [SerializeField] private Color targetColor = Color.red;

    [Tooltip("Select if box indicator is required for this target")]
    [SerializeField] private bool needBoxIndicator = true;

    [Tooltip("Select if arrow indicator is required for this target")]
    [SerializeField] private bool needArrowIndicator = true;

    [Tooltip("Select if distance text is required for this target")]
    [SerializeField] private bool needDistanceText = true;

    [Tooltip("Specify target gameobject. If null, then attached object will be the target.")]
    [SerializeField] private GameObject targetGameObject = null;

    /// <summary>
    /// Please do not assign its value yourself without understanding its use.
    /// A reference to the target's indicator, 
    /// its value is assigned at runtime by the offscreen indicator script.
    /// </summary>
    [HideInInspector] public Indicator indicator;

    private Renderer[] renderers;

    /// <summary>
    /// Gets the color for the target indicator.
    /// </summary>
    public Color TargetColor
    {
        get
        {
            return targetColor;
        }
    }

    /// <summary>
    /// Gets if box indicator is required for the target.
    /// </summary>
    public bool NeedBoxIndicator
    {
        get
        {
            return needBoxIndicator;
        }
    }

    /// <summary>
    /// Gets if arrow indicator is required for the target.
    /// </summary>
    public bool NeedArrowIndicator
    {
        get
        {
            return needArrowIndicator;
        }
    }

    /// <summary>
    /// Gets if the distance text is required for the target.
    /// </summary>
    public bool NeedDistanceText
    {
        get
        {
            return needDistanceText;
        }
    }

    /// <summary>
    /// On enable add this target object to the targets list.
    /// </summary>
    private void OnEnable()
    {
        if(OffScreenIndicator.TargetStateChanged != null)
        {
            OffScreenIndicator.TargetStateChanged.Invoke(this, true);
        }
    }

    /// <summary>
    /// On disable remove this target object from the targets list.
    /// </summary>
    private void OnDisable()
    {
        if(OffScreenIndicator.TargetStateChanged != null)
        {
            OffScreenIndicator.TargetStateChanged.Invoke(this, false);
        }
    }

    private void Start() {
        if (targetGameObject == null) {
            targetGameObject = gameObject;
            renderers = targetGameObject.GetComponentsInChildren<Renderer>();
        }
    }

    /// <summary>
    /// Gets the distance between the camera and the target.
    /// </summary>
    /// <param name="cameraPosition">Camera position</param>
    /// <returns></returns>
    public float GetDistanceFromCamera(Vector3 cameraPosition)
    {
        float distanceFromCamera = Vector3.Distance(cameraPosition, transform.position);
        return distanceFromCamera;
    }

    public void ChangeTarget(GameObject target) {
        targetGameObject = target;
        renderers = targetGameObject.GetComponentsInChildren<Renderer>();
    }

    public Vector3 GetTargetCenter() {
        if (renderers == null) {
            return targetGameObject.transform.position;
        } else {
            return ComputeObjectCenter();
        }
    }

    private Vector3 ComputeObjectCenter() {
        Vector3 center = new Vector3();
        foreach (Renderer rend in renderers) {
            center += rend.bounds.center;
        }
        center /= renderers.Length;

        return center;
    }
}
