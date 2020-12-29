using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class MinionStackable
{
    public string message;
    public Transform MinionCameraPosition;

    public MinionStackable(string m, Transform t)
    {
        message = m;
        MinionCameraPosition = t;
    }
}

public class MinionCameraManager : MonoBehaviour
{
    public static MinionCameraManager SharedInstance;

    public Camera MinionCamera;
    public Transform Target;
    public GameObject CameraDisplay;
    public TextMeshProUGUI MinionArriveText;
    public float AlertDuration = 5f;
    public float AlertDisplaySpeed = 5f;

    private List<MinionStackable> minionStack = new List<MinionStackable>();

    private bool isPlayingAlert;
    private float currentTimer = 0f;
    private Vector2 windowScale = Vector2.zero;
    private RectTransform cameraDisplayTransform;

    private void Awake()
    {
        SharedInstance = this;
        cameraDisplayTransform = CameraDisplay.GetComponent<RectTransform>();
    }

    void Update()
    {
        // Match camera position to newest spawn
        if (Target)
        {
            MinionCamera.transform.position = Target.position;
            MinionCamera.transform.rotation = Target.rotation;
        }

        // Lerp scale of the minion spawn ui element
        cameraDisplayTransform.localScale = Vector2.Lerp(cameraDisplayTransform.localScale, windowScale, Time.deltaTime * AlertDisplaySpeed);

        // Manage timer and displaying minion from stack
        if (isPlayingAlert)
        {
            if (currentTimer > 0f)
            {
                currentTimer -= Time.deltaTime;
            }
            else
            {
                Target = null;
                isPlayingAlert = false;

                windowScale.x = 0f;
                windowScale.y = 0f;
            }
        }
        else
        {
            if (minionStack.Count > 0)
            {
                MinionStackable minion = minionStack[0];

                isPlayingAlert = true;
                currentTimer = AlertDuration;
                MinionArriveText.text = minion.message;
                Target = minion.MinionCameraPosition;

                StartCoroutine(SetSpawnDisplayWithDelay(1f));

                minionStack.RemoveAt(0);
            }
        }

    }

    public IEnumerator SetSpawnDisplayWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        windowScale.x = 1f;
        windowScale.y = 1f;
    }

    public void AddMinionToStack(Transform target, string message)
    {
        MinionStackable minion = new MinionStackable(message, target);
        minionStack.Add(minion);
    }
}
