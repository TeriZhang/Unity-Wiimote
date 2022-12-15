using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text;
using System;
using WiimoteApi;

public class WiimoteDemo1 : MonoBehaviour {

    public WiimoteModel model;
    public RectTransform[] ir_dots;
    public RectTransform[] ir_bb;
    public RectTransform ir_pointer;

    private Quaternion initial_rotation;

    [SerializeField]
    private Wiimote wiimote2;
    

    private Vector2 scrollPosition;

    private Vector3 wmpOffset = Vector3.zero;

    void Start() {
        initial_rotation = model.rot.localRotation;
    }

	void Update () 
    {
        RaycastHit target;
        Vector3 rayStart = transform.position;
        Vector3 rayDir = transform.forward;

        if (Physics.Raycast(rayStart, rayDir, out target, Mathf.Infinity))
        {
            Debug.Log("123321");
            Debug.DrawRay(rayStart, rayDir * target.distance, Color.yellow);

        }
        else
        {

        }

        if (!WiimoteManager.HasWiimote()) { return; }

        wiimote2 = WiimoteManager.Wiimotes[1];
        

        int ret;
        do
        {
            ret = wiimote2.ReadWiimoteData();

            if (ret > 0 && wiimote2.current_ext == ExtensionController.MOTIONPLUS) {
                Vector3 offset = new Vector3(  -wiimote2.MotionPlus.PitchSpeed,
                                                wiimote2.MotionPlus.YawSpeed,
                                                wiimote2.MotionPlus.RollSpeed) / 95f; // Divide by 95Hz (average updates per second from wiimote)
                wmpOffset += offset;

                model.rot.Rotate(offset, Space.Self);
            }
        } while (ret > 0);

        model.a.enabled = wiimote2.Button.a;
        model.b.enabled = wiimote2.Button.b;
        model.one.enabled = wiimote2.Button.one;
        model.two.enabled = wiimote2.Button.two;
        model.d_up.enabled = wiimote2.Button.d_up;
        model.d_down.enabled = wiimote2.Button.d_down;
        model.d_left.enabled = wiimote2.Button.d_left;
        model.d_right.enabled = wiimote2.Button.d_right;
        model.plus.enabled = wiimote2.Button.plus;
        model.minus.enabled = wiimote2.Button.minus;
        model.home.enabled = wiimote2.Button.home;

        if (wiimote2.current_ext != ExtensionController.MOTIONPLUS)
            model.rot.localRotation = initial_rotation;

        if (ir_dots.Length < 4) return;

        float[,] ir = wiimote2.Ir.GetProbableSensorBarIR();
        for (int i = 0; i < 2; i++)
        {
            float x = (float)ir[i, 0] / 1023f;
            float y = (float)ir[i, 1] / 767f;
            if (x == -1 || y == -1) {
                ir_dots[i].anchorMin = new Vector2(0, 0);
                ir_dots[i].anchorMax = new Vector2(0, 0);
            }

            ir_dots[i].anchorMin = new Vector2(x, y);
            ir_dots[i].anchorMax = new Vector2(x, y);

            if (ir[i, 2] != -1)
            {
                int index = (int)ir[i,2];
                float xmin = (float)wiimote2.Ir.ir[index,3] / 127f;
                float ymin = (float)wiimote2.Ir.ir[index,4] / 127f;
                float xmax = (float)wiimote2.Ir.ir[index,5] / 127f;
                float ymax = (float)wiimote2.Ir.ir[index,6] / 127f;
                ir_bb[i].anchorMin = new Vector2(xmin, ymin);
                ir_bb[i].anchorMax = new Vector2(xmax, ymax);
            }
        }

        float[] pointer = wiimote2.Ir.GetPointingPosition();
        ir_pointer.anchorMin = new Vector2(pointer[0], pointer[1]);
        ir_pointer.anchorMax = new Vector2(pointer[0], pointer[1]);

        
    }

   // void OnGUI()
   // {
   //     GUI.Box(new Rect(0,0,320,Screen.height), "");

   //     GUILayout.BeginVertical(GUILayout.Width(300));
   //     GUILayout.Label("Wiimote Found: " + WiimoteManager.HasWiimote());
   //     if (GUILayout.Button("Find Wiimote"))
   //         WiimoteManager.FindWiimotes();

   //     if (GUILayout.Button("Cleanup"))
   //     {
   //         WiimoteManager.Cleanup(wiimote2);
   //         wiimote2 = null;
   //     }

   //     if (wiimote2 == null)
   //         return;

   //     GUILayout.Label("Extension: " + wiimote2.current_ext.ToString());

   //     GUILayout.Label("LED Test:");
   //     GUILayout.BeginHorizontal();
   //     for (int x = 0; x < 4;x++ )
   //         if (GUILayout.Button(""+x, GUILayout.Width(300/4)))
   //             wiimote2.SendPlayerLED(x == 0, x == 1, x == 2, x == 3);
   //     GUILayout.EndHorizontal();

   //     GUILayout.Label("Set Report:");
   //     GUILayout.BeginHorizontal();
   //     if(GUILayout.Button("But/Acc", GUILayout.Width(300/4)))
   //         wiimote2.SendDataReportMode(InputDataType.REPORT_BUTTONS_ACCEL);
   //     if(GUILayout.Button("But/Ext8", GUILayout.Width(300/4)))
   //         wiimote2.SendDataReportMode(InputDataType.REPORT_BUTTONS_EXT8);
   //     if(GUILayout.Button("B/A/Ext16", GUILayout.Width(300/4)))
   //         wiimote2.SendDataReportMode(InputDataType.REPORT_BUTTONS_ACCEL_EXT16);
   //     if(GUILayout.Button("Ext21", GUILayout.Width(300/4)))
   //         wiimote2.SendDataReportMode(InputDataType.REPORT_EXT21);
   //     GUILayout.EndHorizontal();

   //     if (GUILayout.Button("Request Status Report"))
   //         wiimote2.SendStatusInfoRequest();

   //     GUILayout.Label("IR Setup Sequence:");
   //     GUILayout.BeginHorizontal();
   //     if(GUILayout.Button("Basic", GUILayout.Width(100)))
   //         wiimote2.SetupIRCamera(IRDataType.BASIC);
   //     if(GUILayout.Button("Extended", GUILayout.Width(100)))
   //         wiimote2.SetupIRCamera(IRDataType.EXTENDED);
   //     if(GUILayout.Button("Full", GUILayout.Width(100)))
   //         wiimote2.SetupIRCamera(IRDataType.FULL);
   //     GUILayout.EndHorizontal();

   //     GUILayout.Label("WMP Attached: " + wiimote2.wmp_attached);
   //     if (GUILayout.Button("Request Identify WMP"))
   //         wiimote2.RequestIdentifyWiiMotionPlus();
   //     if ((wiimote2.wmp_attached || wiimote2.Type == WiimoteType.PROCONTROLLER) && GUILayout.Button("Activate WMP"))
   //         wiimote2.ActivateWiiMotionPlus();
   //     if ((wiimote2.current_ext == ExtensionController.MOTIONPLUS ||
   //         wiimote2.current_ext == ExtensionController.MOTIONPLUS_CLASSIC ||
   //         wiimote2.current_ext == ExtensionController.MOTIONPLUS_NUNCHUCK) && GUILayout.Button("Deactivate WMP"))
   //         wiimote2.DeactivateWiiMotionPlus();

   //     GUILayout.Label("Calibrate Accelerometer");
   //     GUILayout.BeginHorizontal();
   //     for (int x = 0; x < 3; x++)
   //     {
   //         AccelCalibrationStep step = (AccelCalibrationStep)x;
   //         if (GUILayout.Button(step.ToString(), GUILayout.Width(100)))
   //             wiimote2.Accel.CalibrateAccel(step);
   //     }
   //     GUILayout.EndHorizontal();

   //     if (GUILayout.Button("Print Calibration Data"))
   //     {
   //         StringBuilder str = new StringBuilder();
   //         for (int x = 0; x < 3; x++)
   //         {
   //             for (int y = 0; y < 3; y++)
   //             {
   //                 str.Append(wiimote2.Accel.accel_calib[y, x]).Append(" ");
   //             }
   //             str.Append("\n");
   //         }
   //         Debug.Log(str.ToString());
   //     }

   //     if (wiimote2 != null && wiimote2.current_ext != ExtensionController.NONE)
   //     {
   //         scrollPosition = GUILayout.BeginScrollView(scrollPosition);
   //         GUIStyle bold = new GUIStyle(GUI.skin.button);
   //         bold.fontStyle = FontStyle.Bold;
   //         if (wiimote2.current_ext == ExtensionController.NUNCHUCK) {
   //             GUILayout.Label("Nunchuck:", bold);
   //             NunchuckData data = wiimote2.Nunchuck;
   //             GUILayout.Label("Stick: " + data.stick[0] + ", " + data.stick[1]);
   //             GUILayout.Label("C: " + data.c);
   //             GUILayout.Label("Z: " + data.z);
   //         } else if (wiimote2.current_ext == ExtensionController.CLASSIC) {
   //             GUILayout.Label("Classic Controller:", bold);
   //             ClassicControllerData data = wiimote2.ClassicController;
   //             GUILayout.Label("Stick Left: " + data.lstick[0] + ", " + data.lstick[1]);
   //             GUILayout.Label("Stick Right: " + data.rstick[0] + ", " + data.rstick[1]);
   //             GUILayout.Label("Trigger Left: " + data.ltrigger_range);
   //             GUILayout.Label("Trigger Right: " + data.rtrigger_range);
   //             GUILayout.Label("Trigger Left Button: " + data.ltrigger_switch);
   //             GUILayout.Label("Trigger Right Button: " + data.rtrigger_switch);
   //             GUILayout.Label("A: " + data.a);
   //             GUILayout.Label("B: " + data.b);
   //             GUILayout.Label("X: " + data.x);
   //             GUILayout.Label("Y: " + data.y);
   //             GUILayout.Label("Plus: " + data.plus);
   //             GUILayout.Label("Minus: " + data.minus);
   //             GUILayout.Label("Home: " + data.home);
   //             GUILayout.Label("ZL: " + data.zl);
   //             GUILayout.Label("ZR: " + data.zr);
   //             GUILayout.Label("D-Up: " + data.dpad_up);
   //             GUILayout.Label("D-Down: " + data.dpad_down);
   //             GUILayout.Label("D-Left: " + data.dpad_left);
   //             GUILayout.Label("D-Right: " + data.dpad_right);
   //         }
   //         else if (wiimote2.current_ext == ExtensionController.MOTIONPLUS)
   //         {
   //             GUILayout.Label("Wii Motion Plus:", bold);
   //             MotionPlusData data = wiimote2.MotionPlus;
   //             GUILayout.Label("Pitch Speed: " + data.PitchSpeed);
   //             GUILayout.Label("Yaw Speed: " + data.YawSpeed);
   //             GUILayout.Label("Roll Speed: " + data.RollSpeed);
   //             GUILayout.Label("Pitch Slow: " + data.PitchSlow);
   //             GUILayout.Label("Yaw Slow: " + data.YawSlow);
   //             GUILayout.Label("Roll Slow: " + data.RollSlow);
   //             if (GUILayout.Button("Zero Out WMP"))
   //             {
   //                 data.SetZeroValues();
   //                 model.rot.rotation = Quaternion.FromToRotation(model.rot.rotation*GetAccelVector(), Vector3.up) * model.rot.rotation;
   //                 model.rot.rotation = Quaternion.FromToRotation(model.rot.forward, Vector3.forward) * model.rot.rotation;
   //             }
   //             if(GUILayout.Button("Reset Offset"))
   //                 wmpOffset = Vector3.zero;
   //             GUILayout.Label("Offset: " + wmpOffset.ToString());
   //         }
   //         else if (wiimote2.current_ext == ExtensionController.WIIU_PRO)
   //         {
   //             GUILayout.Label("Wii U Pro Controller:", bold);
   //             WiiUProData data = wiimote2.WiiUPro;
   //             GUILayout.Label("Stick Left: "+data.lstick[0]+", "+data.lstick[1]);
   //             GUILayout.Label("Stick Right: "+data.rstick[0]+", "+data.rstick[1]);
   //             GUILayout.Label("A: "+data.a);
   //             GUILayout.Label("B: "+data.b);
   //             GUILayout.Label("X: "+data.x);
   //             GUILayout.Label("Y: "+data.y);

   //             GUILayout.Label("D-Up: "   +data.dpad_up);
   //             GUILayout.Label("D-Down: " +data.dpad_down);
   //             GUILayout.Label("D-Left: " +data.dpad_left);
   //             GUILayout.Label("D-Right: "+data.dpad_right);

   //             GUILayout.Label("Plus: "+data.plus);
   //             GUILayout.Label("Minus: "+data.minus);
   //             GUILayout.Label("Home: "+data.home);

   //             GUILayout.Label("L: "+data.l);
   //             GUILayout.Label("R: "+data.r);
   //             GUILayout.Label("ZL: "+data.zl);
   //             GUILayout.Label("ZR: "+data.zr);
   //         }
			//else if (wiimote2.current_ext == ExtensionController.GUITAR) {
			//	GUILayout.Label ("Guitar", bold);
			//	GuitarData data = wiimote2.Guitar;
			//	float[] stick = data.GetStick01 ();
			//	GUILayout.Label ("Stick: " + stick [0] + ", " + stick [1]);
			//	GUILayout.Label ("Slider: " + (data.has_slider ? Convert.ToString (data.GetSlider01 ()) : "unsupported"));
			//	GUILayout.Label ("Green: " + data.green);
			//	GUILayout.Label ("Red: " + data.red);
			//	GUILayout.Label ("Yellow: " + data.yellow);
			//	GUILayout.Label ("Blue: " + data.blue);
			//	GUILayout.Label ("Orange: " + data.orange);
			//	GUILayout.Label ("Strum Up: " + data.strum_up);
			//	GUILayout.Label ("Strum Down: " + data.strum_down);
			//	GUILayout.Label ("Minus: " + data.minus);
			//	GUILayout.Label ("Plus: " + data.plus);
			//	GUILayout.Label ("Whammy: " + data.GetWhammy01());
			//}
   //         GUILayout.EndScrollView();
   //     } else {
   //         scrollPosition = Vector2.zero;
   //     }
   //     GUILayout.EndVertical();
   // }

    void OnDrawGizmos()
    {
        if (wiimote2 == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(model.rot.position, model.rot.position + model.rot.rotation*GetAccelVector()*2);
    }

    private Vector3 GetAccelVector()
    {
        float accel_x;
        float accel_y;
        float accel_z;

        float[] accel = wiimote2.Accel.GetCalibratedAccelData();
        accel_x = accel[0];
        accel_y = -accel[2];
        accel_z = -accel[1];

        return new Vector3(accel_x, accel_y, accel_z).normalized;
    }

    [System.Serializable]
    public class WiimoteModel
    {
        public Transform rot;
        public Renderer a;
        public Renderer b;
        public Renderer one;
        public Renderer two;
        public Renderer d_up;
        public Renderer d_down;
        public Renderer d_left;
        public Renderer d_right;
        public Renderer plus;
        public Renderer minus;
        public Renderer home;
    }

	void OnApplicationQuit() {
		if (wiimote2 != null) {
			WiimoteManager.Cleanup(wiimote2);
	        wiimote2 = null;
		}
	}
}
