﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Operational : MonoBehaviour
{
    public Text speedSurge;
    public Text speedHeave;
    public Text speedSway;
    public Text trimFwd_Aft;
    public Text trimPort_Stbd;
    public Text trimUp_Down;
    public Text prop_right_up;
    public Text prop_right_down;
    public Text prop_left_up;
    public Text prop_left_down;
    public Text txt_Altitude;
    public Text txt_Heading;
    public Text txt_Depth;
    public Text txt_Turns;
    public Text txt_flow_from;
    private float default_flow_from = 5.00f;
    float depth = ControlData.Instance.curDepth;
    float rovEuler = 0.0f;
    public Slider sl_Altitude;
    public Slider sl_Depth;
    public Text txt__TMS_BigRoll;
    public Text txt__TMS_SmallRoll;
    public Text txt_TMS_Meter;
    // Start is called before the first frame update
    void Start()
    {
        Debug.LogError(this.gameObject.name);
        MsgMng.Instance.Register(MessageName.MSG_MOVE_FWD, OnMove);
        MsgMng.Instance.Register(MessageName.MSG_MOVE_BWD, OnMove);
        MsgMng.Instance.Register(MessageName.MSG_MOVE_LEFT, OnMove);
        MsgMng.Instance.Register(MessageName.MSG_MOVE_RIGHT, OnMove);
        MsgMng.Instance.Register(MessageName.MSG_MOVE_UP, OnMove);
        MsgMng.Instance.Register(MessageName.MSG_MOVE_DOWN, OnMove);
        MsgMng.Instance.Register(MessageName.MSG_MOVE_STOP, OnMove);
        MsgMng.Instance.Register(MessageName.MSG_MOVE_TURN_L, OnMove);
        MsgMng.Instance.Register(MessageName.MSG_MOVE_TURN_R, OnMove);
        trimUp_Down.text = -ControlData.Instance.GetTrim_UP_Down + "%";
    }

    // Update is called once per frame
    void Update()
    {
        
        txt_flow_from.text = (default_flow_from+rovEuler).ToString("f2")+"Deg";
        OnShow();
        sl_Altitude.value = (30 - depth) / 30;
        sl_Depth.value = depth / 30;
        OnTMS();
    }
 
    RobotControl.DIR dir;
    void OnShow()
    {
        switch (dir)
        {
            case RobotControl.DIR.Foward:
                speedSurge.text = ControlData.Instance.GetSpeedSurge + "m/s";
                trimFwd_Aft.text = ControlData.Instance.GetTrim_Fwd_Aft + "%";
                trimPort_Stbd.text = "0.00%";
                prop_right_up.text = ControlData.Instance.GetPropRightUp + "%";
                prop_left_down.text = ControlData.Instance.GetPropLeftDown + "%";
                prop_right_down.text = ControlData.Instance.GetPropRightDown / 2.0f + "%";
                prop_left_up.text = ControlData.Instance.GetPropLeftUp / 2.0f + "%";
                break;
            case RobotControl.DIR.Back:
                speedSurge.text = ControlData.Instance.GetSpeedSurge + "m/s";
                trimFwd_Aft.text = ControlData.Instance.GetTrim_Fwd_Aft + "%";
                trimPort_Stbd.text = "0.00%";
                prop_right_up.text = -ControlData.Instance.GetPropRightUp + "%";
                prop_left_down.text = -ControlData.Instance.GetPropLeftDown + "%";
                prop_right_down.text = -ControlData.Instance.GetPropRightDown / 2.0f + "%";
                prop_left_up.text = -ControlData.Instance.GetPropLeftUp / 2.0f + "%";
                break;
            case RobotControl.DIR.Left:
                speedSway.text = -ControlData.Instance.GetSpeedSway + "m/s";
                trimPort_Stbd.text = ControlData.Instance.GetTrim_Port_Stbd + "%";
                trimFwd_Aft.text = "0.00%";
                prop_right_up.text = ControlData.Instance.GetPropRightUp / 2 + "%";
                prop_left_down.text = ControlData.Instance.GetPropLeftDown / 2 + "%";
                prop_right_down.text = -ControlData.Instance.GetPropRightDown + "%";
                prop_left_up.text = -ControlData.Instance.GetPropLeftUp + "%";
                break;
            case RobotControl.DIR.Right:
                speedSway.text = ControlData.Instance.GetSpeedSway + "m/s";
                trimPort_Stbd.text = ControlData.Instance.GetTrim_Port_Stbd + "%";
                trimFwd_Aft.text = "0.00%";
                prop_right_up.text = -ControlData.Instance.GetPropRightUp / 2 + "%";
                prop_left_down.text = -ControlData.Instance.GetPropLeftDown / 2 + "%";
                prop_right_down.text = ControlData.Instance.GetPropRightDown + "%";
                prop_left_up.text = ControlData.Instance.GetPropLeftUp + "%";
                break;
            case RobotControl.DIR.Up:
                speedHeave.text = ControlData.Instance.GetSpeedHeave + "m/s";
                depth -= ControlData.Instance.cur_Velocity.y * Time.deltaTime;
                txt_Depth.text = depth.ToString("f2") + "m";
                txt_Altitude.text = (30 - depth).ToString("0.00") + "m";
                break;
            case RobotControl.DIR.Down:
                speedHeave.text = ControlData.Instance.GetSpeedHeave + "m/s";
                depth -= ControlData.Instance.cur_Velocity.y * Time.deltaTime;
                txt_Depth.text = depth.ToString("f2") + "m";
                txt_Altitude.text = (30 - depth).ToString("0.00") + "m";
                break;
            case RobotControl.DIR.TurnL:
                float rot = ControlData.Instance.cur_EularVelocity.y * 50 * Time.deltaTime;
                if (rot > 360) rot -= 360;
                rovEuler -= rot;
                txt_Heading.text = rovEuler.ToString("f2") + "Deg";
                txt_Turns.text = (rovEuler / 360.0f).ToString("f2");

                break;
            case RobotControl.DIR.TurnR:
                float rot1 = ControlData.Instance.cur_EularVelocity.y * 50 * Time.deltaTime;
                if (rot1 > 360) rot1 -= 360;
                rovEuler += rot1;
                txt_Heading.text = rovEuler.ToString("f2") + "Deg";
                txt_Turns.text = (rovEuler / 360.0f).ToString("f2");

                break;
            default:
                break;
        }
       
    }

    private void OnTMS()
    {
        txt_TMS_Meter.text = ControlData.Instance.TMS_meters.ToString("f2") + "m";
        txt__TMS_BigRoll.text =((1000f- ControlData.Instance.TMS_meters)/ 10f).ToString("f2")+"%";
        txt__TMS_SmallRoll.text = ((ControlData.Instance.TMS_meters) / 100f).ToString("f2") + "%"; ;
    }
    private void OnMove(MessageData data)
    {
        
        if (data!=null)
        {
            
            dir = (RobotControl.DIR)data.valueInt;
           
        }
       
    }
}
