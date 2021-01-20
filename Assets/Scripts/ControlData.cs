
//记录各个界面控件操作数据.
using UnityEngine;

public class ControlData 
{
    private static ControlData _instance;
    private static object _lock = new object();

    public static ControlData Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                        _instance = new ControlData();
                }
            }
            return _instance;
        }
    }
    public int ROVPOD_isOn = 0;
    public int ROVMOTOR_isOn = 0;
    public int ALLROVLAMP_isOn = 0;
    public int Release_Sequence_isOn = 0;
    public int LoadPump_isOn = 0;
    public int ThrustEnabled_isOn = 0;
    public int STBDMainipulator_isOm= 0;
    public int SystemPressureValue_isOn = 0;
    

    public int curPT = 2;//当前操作云台 1--上方云台 2--下方云台.
    public float curDepth=20;//当前深度.
    public Vector3 cur_Velocity = Vector3.zero;
    public Vector3 cur_EularVelocity = Vector3.zero;

    public float TMS_meters = 0;
   

   // public float curSpeed = cur_Velocity.y;//0.1m/s.
  

    //判断是否具备启动条件
    public bool GetReady
    {
        get {
            int sum = ROVPOD_isOn + ROVMOTOR_isOn + ALLROVLAMP_isOn + Release_Sequence_isOn + LoadPump_isOn + ThrustEnabled_isOn + STBDMainipulator_isOm + SystemPressureValue_isOn;
            return sum == 8;
        }
    }

    //前后运动速度.
    public float GetSpeedSurge
    {
        get{ return (float)System.Math.Round(cur_Velocity.z, 2); }
        set{ }
    }
    //左右速度.
    public float GetSpeedSway
    {
        get { return (float)System.Math.Round(cur_Velocity.x, 2); }
        set { }
    }
    //上下速度.
    public float GetSpeedHeave
    {
        get { return (float)System.Math.Round(cur_Velocity.y, 2); }
        set { }
    }

   
    //前后Trim百分比.
    public float GetTrim_Fwd_Aft
    {
        get {
           
            return 
                (float)System.Math.Round(cur_Velocity.z * 100, 2);
        }
        set { }
    }

    //左右Trim百分比.
    public float GetTrim_Port_Stbd
    {
        get { return (float)System.Math.Round(cur_Velocity.x * 100, 2); }
        set { }
    }

    //上下Trim百分比.
    public float GetTrim_UP_Down
    {
        get { return   (float)System.Math.Round(cur_Velocity.y * 100, 2); }
    }

    //右上螺旋桨百分比.
    public float GetPropRightUp
    {
        get { return (float)System.Math.Round(cur_Velocity.x * 58.6f, 1); }
    }


    //右下螺旋桨百分比.
    public float GetPropRightDown
    {
        get { return  (float)System.Math.Round(cur_Velocity.x * 58.6f,1); }
    }

    //左上螺旋桨百分比.
    public float GetPropLeftUp
    {
        get { return (float)System.Math.Round(cur_Velocity.x * 58.6f, 1); }
    }


    //左下螺旋桨百分比.
    public float GetPropLeftDown
    {
        get { return (float)System.Math.Round(cur_Velocity.x * 58.6f, 1); }
    }

    //
    public float GetTMSBigRoll
    {
        get { return (float)System.Math.Round(cur_Velocity.x * 58.6f, 1); }
    }

    public float GetTMSBigSmallRoll
    {
        get { return (float)System.Math.Round(cur_Velocity.x * 58.6f, 1); }
    }

    public float getTMSMeters
    {
        get { return TMS_meters; }
    }
   
}
