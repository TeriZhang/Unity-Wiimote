using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WiimoteApi;

public class test : MonoBehaviour
{
    public Wiimote wiimote1;
    public Wiimote wiimote2;
    // Start is called before the first frame update
    void Start()
    {
        WiimoteManager.FindWiimotes();
        wiimote2 = WiimoteManager.Wiimotes[1];
        wiimote1 = WiimoteManager.Wiimotes[0];
        
    }

    // Update is called once per frame
    void Update()
    {
        if(wiimote2 == null)
        {
            Debug.Log("Unable to Find Wiimote");
        }
        wiimote1.SendDataReportMode(InputDataType.REPORT_BUTTONS_ACCEL);
        Debug.Log(WiimoteManager.Wiimotes.Count);
    }

    void OnDrawGizmos()
    {
        if (wiimote1 == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.rotation * GetAccelVector() * 2);
    }

    private Vector3 GetAccelVector()
    {
        float accel_x;
        float accel_y;
        float accel_z;

        float[] accel = wiimote1.Accel.GetCalibratedAccelData();
        accel_x = accel[0];
        accel_y = -accel[2];
        accel_z = -accel[1];

        return new Vector3(accel_x, accel_y, accel_z).normalized;
    }
}
