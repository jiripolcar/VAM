using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class DisplayScript : MonoBehaviour
{
    public Text console;

    // Use this for initialization
    void Start()
    {
        Debug.Log("displays connected: " + Display.displays.Length);
        // Display.displays[0] is the primary, default display and is always ON.
        // Check if additional displays are available and activate each.
        if (Display.displays.Length > 1)
            Display.displays[1].Activate();
        if (Display.displays.Length > 2)
            Display.displays[2].Activate();

    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.U))
            SwitchDisplays();
    }

    void SwitchDisplays()
    {
        console.text += "\n" + "Swtiching display";
        GameObject[] allObjs = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
        List<Camera> cams = new List<Camera>();
        foreach (GameObject obj in allObjs)
        {
            Camera rootCam = obj.GetComponent<Camera>();
            if (rootCam && !cams.Contains(rootCam))
                cams.Add(rootCam);

            Camera[] camsInObj = GetComponentsInChildren<Camera>(true);
            foreach (Camera objCam in camsInObj)
                if (!cams.Contains(objCam))
                    cams.Add(objCam);
        }
        foreach (Camera cam in cams)
        {
            console.text += "\n" + cam.name + " display: " + cam.targetDisplay;
            cam.targetDisplay = cam.targetDisplay == 1 ? 0 : 1;
        }
    }


}