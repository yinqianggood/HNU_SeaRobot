﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotControl : MonoBehaviour
{
    public GameObject ROV;
    public Rigidbody _rb;
    public GameObject TMS;
    public GameObject armNote1;
    public GameObject armNode2;
    public GameObject armNode3;
    public GameObject armNodeRote;
    public GameObject armFinger1;
    public GameObject armFinger2;

    public List<Transform> H_Propellers = new List<Transform>();
    public List<Transform> V_Propellers = new List<Transform>();
    private float mPropSpeed = 0;

    public List<GameObject> lamp_port_STBD;
    public List<GameObject> lamp_bullet_PT;
    public List<GameObject> lamp_bottomPT;
    public Light light_bullet;
    public Light light_pt;
    public Light light_pt_bottom;
    public Light light_port;
    public Light light_STBD;
  


    public GameObject PT_RoteteH;
    public GameObject PT_RoteteV;
    public GameObject PT_BottomRoteteH;
    public GameObject PT_BottomRotateV;

    public Rigidbody TMS_Tether;

    public GameObject flotPan;

    public Light Directional_light_Sun;

    //作为单例类
    private static RobotControl instance;
    public static RobotControl Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new RobotControl();
            }
            return instance;
        }
    }

    public enum DIR
    { 
       None=0,
       Foward=1,
       Back=2,
       Left=3,
       Right=4,
       Up=5,
       Down=6,
       TurnL=7,
       TurnR=8
    }

    public enum PROPDIR 
    {
      Horizontal=1,
      Vertical=2,
      Both=3

    }

    public enum ARMDIR 
    {
      Left=1,
      Right=2,
      Up=3,
      Down=4,
      Long=5,
      Short=6,
      In=7,
      Out=8,
      TurnL = 9,
      TurnR = 10,
    }
   // private Rigidbody _rb ;
    private string mDataStr;
    bool isDoing = true;
    private void Awake()
    {
      //  _rb = ROV.GetComponent<Rigidbody>();
       // _rb.freezeRotation = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        // LampControl(true,false,true,true);
        //lamp_bottomPT[0].gameObject.GetComponent<MeshRenderer>().sharedMaterial.DisableKeyword("_EMISSION");

        for (int i = 0; i < H_Propellers.Count; i++)
        {
           H_Propellers[i].transform.Rotate(new Vector3(0, 5, 0));
        }
    }
    private float _propSpeed=1;
    // Update is called once per frame
    void Update()
    {
       // ThrusterControl(mPropSpeed, PROPDIR.Both);
        mDataStr = UDPServer.instance.recvStr;
        //  if(isDoing)
        if (string.IsNullOrEmpty(mDataStr)) return;
        if (mDataStr.Contains("rov_"))
        {
            MoveROV(mDataStr);
            
        }
           
        else if (mDataStr.Contains("arm_"))
            MoveArm(mDataStr);
        else if (mDataStr.Contains("cam_"))
            MoveCamera(mDataStr);
        else if (mDataStr.Contains("flot_"))
            MoveFlot(mDataStr);
        else if (mDataStr.Contains("lamp_"))
            LampControl(mDataStr);
        else if (mDataStr.Contains("tether"))
            TMSTetherControl(mDataStr);
        else
        {
            return;

        }
        _rb.transform.localEulerAngles = new Vector3(0, _rb.transform.localEulerAngles.y, 0);
       

    }
    private void FixedUpdate()
    {
        FreezedRotate();
    }
    /// <summary>
    /// socket receive
    /// </summary>
    /// <param name="dirStr"></param>
    /// <param name="speed"></param>
    /// <param name="isPress"></param>
    public void MoveROV(string dirStr, float speed = 1, Boolean isPress = true)
    {

        ControlData.Instance.cur_Velocity = _rb.velocity;
        ControlData.Instance.cur_EularVelocity = _rb.angularVelocity;
       // mPropSpeed = 20;
        if (dirStr==NetConfig.rov_left_on)
        {
            MoveROV(DIR.Left);
            mPropSpeed = 20;
        }
        else if (dirStr == NetConfig.rov_left_off)
        {
            mPropSpeed = 1;
        }
        else if(dirStr==NetConfig.rov_right_on)
        {
            MoveROV(DIR.Right);
            mPropSpeed = -20;
        }
        else if (dirStr == NetConfig.rov_right_off)
        {
           
            mPropSpeed = -1;
        }
        else if (dirStr == NetConfig.rov_up_on)
        {
            MoveROV(DIR.Up);
            mPropSpeed = 20;
        }
        else if (dirStr == NetConfig.rov_up_off)
        {
           
            mPropSpeed = 1;
        }
        else if (dirStr == NetConfig.rov_down_on)
        {
            MoveROV(DIR.Down);
            mPropSpeed = -20;
        }
        else if (dirStr == NetConfig.rov_down_off)
        {
           
            mPropSpeed = -21;
        }
        else if (dirStr == NetConfig.rov_foward_on)
        {
            MoveROV(DIR.Foward);
            mPropSpeed = 20;
        }
        else if (dirStr == NetConfig.rov_foward_off)
        {
          
            mPropSpeed = 1;
        }
        else if (dirStr == NetConfig.rov_back_on)
        {
            MoveROV(DIR.Back);
            mPropSpeed = -20;
        }
        else if (dirStr == NetConfig.rov_back_off)
        {
           
            mPropSpeed = -1;
        }
        else if (dirStr == NetConfig.rov_turn_left_on)
        {
            MoveROV(DIR.TurnL);
            mPropSpeed = -20;
        }
        else if (dirStr == NetConfig.rov_turn_left_on)
        {
            
            mPropSpeed = -1;
        }
        else if (dirStr == NetConfig.rov_turn_right_on)
        {
            MoveROV(DIR.TurnR);
            mPropSpeed = 20;
        }
        else if (dirStr == NetConfig.rov_turn_right_off)
        {
           
            mPropSpeed = 1;
        }
        else
        {
           // mPropSpeed = 0.5f;
           // return;
        }
      ThrusterControl(mPropSpeed, PROPDIR.Both);
        // isDoing = false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dataStr"></param>
    public void MoveArm(string dataStr)
    {
        if(dataStr==NetConfig.arm_foward_on)
        {
            ArmControl(ARMDIR.Long);
        }
        else if(dataStr==NetConfig.arm_back_on)
        {
            ArmControl(ARMDIR.Short);
        }
        else if (dataStr == NetConfig.arm_left_on)
        {
            ArmControl(ARMDIR.Left);
        }
        else if (dataStr == NetConfig.arm_right_on)
        {
            ArmControl(ARMDIR.Right);
        }
        else if (dataStr == NetConfig.arm_up_on)
        {
            ArmControl(ARMDIR.Up);
        }
        else if (dataStr == NetConfig.arm_down_on)
        {
            ArmControl(ARMDIR.Down);
        }
        else if (dataStr == NetConfig.arm_turn_left_on)
        {
            ArmControl(ARMDIR.TurnL);
        }
        else if (dataStr == NetConfig.arm_turn_right_on)
        {
            ArmControl(ARMDIR.TurnR);
        }
        else if (dataStr == NetConfig.arm_open_on)
        {
            ArmControl(ARMDIR.Out);
        }
        else if (dataStr == NetConfig.arm_close_on)
        {
            ArmControl(ARMDIR.In);
        }
        else
        {
          //  Debug.LogWarning("unvaliad arm direction message:" + dataStr);
            return;
        }
       
    }

    public void MoveCamera(string dataStr)
    {
        if(dataStr==NetConfig.cam_up_on)
        {
            CameraRote(DIR.Foward);
        }
        else if(dataStr==NetConfig.cam_down_on)
        {
            CameraRote(DIR.Back);
        }
        else if (dataStr == NetConfig.cam_left_on)
        {
            CameraRote(DIR.Left);
        }
        else if (dataStr == NetConfig.cam_right_on)
        {
            CameraRote(DIR.Right);
        }
        else
        {
          //  Debug.LogWarning("unvaliad camera direction message:" + dataStr);
        }
    }

    public void MoveFlot(string dataStr)
    {
        int speed = 0;
        if(dataStr== NetConfig.flot_back_on)
        {
            speed = -1;
        }
        else if(dataStr==NetConfig.flot_foward_on)
        {
            speed = 1;
        }
        else
        {
            speed = 0;
        }
        FlotControl(speed);
    }
    bool tg1_isOn, tg2_isOn, tg3_isOn, tgAll_isOn = false;
    public void LampControl(string dataStr)
    {
        
        if(dataStr==NetConfig.lamp_port_stbd_on)
        {
            tg1_isOn = true;
        }
        else if (dataStr == NetConfig.lamp_port_stbd_off)
        {
            tg1_isOn = false;
        }
        else if (dataStr == NetConfig.lamp_bullet_pt_on)
        {
            tg2_isOn = true;
        }
        else if (dataStr == NetConfig.lamp_bullet_pt_off)
        {
            tg2_isOn = false;
        }
        else if (dataStr == NetConfig.lamp_bottom_pt_on)
        {
            tg3_isOn = true;
        }
        else if (dataStr == NetConfig.lamp_bottom_pt_off)
        {
            tg3_isOn = false;
        }
        else if (dataStr == NetConfig.lamp_all_on)
        {
            tgAll_isOn = true;
        }
        else if (dataStr == NetConfig.lamp_all_off)
        {
            tgAll_isOn = false;
        }
        else
        {
           // Debug.LogWarning("unvaliad lamp toggle message:" + dataStr);
        }
        LampControl(tg1_isOn, tg2_isOn, tg3_isOn, tgAll_isOn);

    }
    public void MoveROV(DIR dir, float speed=30000,Boolean isPress=true)
    {

     
        //DIR dir = (DIR)direct;
        switch (dir)
        {
            case DIR.Left:
                // ROV.transform.Translate(new Vector3(-speed * Time.deltaTime, 0, 0));
                ThrusterControl(mPropSpeed, PROPDIR.Horizontal);
                if (Mathf.Abs(_rb.velocity.x) > 1) return;
                _rb.AddForce(new Vector3(-speed*Time.deltaTime, 0), ForceMode.Force);
                MsgMng.Instance.Send(MessageName.MSG_MOVE_LEFT, new MessageData((int)DIR.Left));
                break;
            case DIR.Right:
                ThrusterControl(-mPropSpeed, PROPDIR.Horizontal);
                if (Mathf.Abs(_rb.velocity.x) > 1) return;
                //ROV.transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
                _rb.AddForce(new Vector3(speed * Time.deltaTime, 0), ForceMode.Force);
                MsgMng.Instance.Send(MessageName.MSG_MOVE_RIGHT, new MessageData((int)DIR.Right));
                break;
            case DIR.Foward:
                ThrusterControl(mPropSpeed, PROPDIR.Horizontal);
                if (Mathf.Abs(_rb.velocity.z) > 1) return;
                //ROV.transform.Translate(new Vector3(0,0,speed * Time.deltaTime));
                _rb.AddForce(new Vector3(0,0,speed*Time.deltaTime), ForceMode.Force);
                MsgMng.Instance.Send(MessageName.MSG_MOVE_FWD, new MessageData((int)DIR.Foward));
                break;
            case DIR.Back:
                ThrusterControl(-mPropSpeed, PROPDIR.Horizontal);
                if (Mathf.Abs(_rb.velocity.z) > 1) return;
                // ROV.transform.Translate(new Vector3(0, 0, -speed * Time.deltaTime));
                _rb.AddForce(new Vector3(0, 0, -speed * Time.deltaTime), ForceMode.Force);
                MsgMng.Instance.Send(MessageName.MSG_MOVE_BWD, new MessageData((int)DIR.Back));
                break;
            case DIR.Up:
                ThrusterControl(mPropSpeed, PROPDIR.Vertical);
                if (Mathf.Abs(_rb.velocity.y) > 1) return;
                //ROV.transform.Translate(new Vector3(0, speed * Time.deltaTime,0));
                _rb.AddForce(new Vector3(0,speed * Time.deltaTime,0), ForceMode.Force);
                MsgMng.Instance.Send(MessageName.MSG_MOVE_UP, new MessageData((int)DIR.Up));
                break;
            case DIR.Down:
                ThrusterControl(-mPropSpeed, PROPDIR.Vertical);
                if (Mathf.Abs(_rb.velocity.y) > 1) return;
                _rb.AddForce(new Vector3(0, -speed * Time.deltaTime, 0), ForceMode.Force);
                MsgMng.Instance.Send(MessageName.MSG_MOVE_DOWN, new MessageData((int)DIR.Down));
                break;
            case DIR.TurnL:
                ThrusterControl(-mPropSpeed/5, PROPDIR.Horizontal);
                if (Mathf.Abs(_rb.angularVelocity.y) > 1) return;
                _rb.AddRelativeTorque(new Vector3(0, -speed/10 * Time.deltaTime, 0), ForceMode.Force);
                //ROV.transform.Rotate(new Vector3(0, -speed * Time.deltaTime, 0));
                MsgMng.Instance.Send(MessageName.MSG_MOVE_TURN_L, new MessageData((int)DIR.TurnL));
                break;
            case DIR.TurnR:
                ThrusterControl(mPropSpeed/5, PROPDIR.Horizontal);
                if (Mathf.Abs(_rb.angularVelocity.y) > 1) return;
                _rb.AddRelativeTorque(new Vector3(0, speed/10 * Time.deltaTime, 0), ForceMode.Force);
                // ROV.transform.Rotate(new Vector3(0, speed * Time.deltaTime, 0));
                MsgMng.Instance.Send(MessageName.MSG_MOVE_TURN_R, new MessageData((int)DIR.TurnR));
                break;
            default:
              
                break;
        }
    }

    public void CameraRote(DIR dir)
    {
        GameObject ptH = ControlData.Instance.curPT == 1 ? PT_RoteteH : PT_BottomRoteteH;
        GameObject ptV = ControlData.Instance.curPT == 1 ? PT_RoteteV : PT_BottomRotateV;
       // DIR dir = (DIR)direct;
        switch (dir)
        {
            case DIR.Foward:
                ptV.transform.Rotate(new Vector3(5 * Time.deltaTime, 0, 0));
                break;
            case DIR.Back:
                ptV.transform.Rotate(new Vector3(-5 * Time.deltaTime, 0, 0));
                break;
            case DIR.Left:
                ptH.transform.Rotate(new Vector3(0, -5 * Time.deltaTime, 0));
                break;
            case DIR.Right:
                ptH.transform.Rotate(new Vector3(0,  5 * Time.deltaTime, 0));
                break;
            default:
                break;
        }
    }
    public void ArmControl(ARMDIR dir)
    {
        switch (dir)
        {
            case ARMDIR.Left:
                armNote1.transform.Rotate(new Vector3(0, -10 * Time.deltaTime,0));
                break;
            case ARMDIR.Right:
                armNote1.transform.Rotate(new Vector3(0, 10 * Time.deltaTime, 0));
                break;
            case ARMDIR.Up:
                armNode2.transform.Rotate(new Vector3(0, 0,10 * Time.deltaTime));
                break;
            case ARMDIR.Down:
                armNode2.transform.Rotate(new Vector3(0, 0, -10 * Time.deltaTime));
                break;
            case ARMDIR.Long:
                if(armNode3.transform.localPosition.y<0.65f)
                {
                    armNode3.transform.Translate(0, 0.05f * Time.deltaTime, 0);
                }
                break;
            case ARMDIR.Short:
                if (armNode3.transform.localPosition.y >0.4f)
                {
                    armNode3.transform.Translate(0, -0.05f * Time.deltaTime, 0);
                }
                break;
            case ARMDIR.In:
                Vector3 v3 = getAngle(armFinger1.transform);
                Debug.Log("v3.x:" + v3.x);
                //if (v3.x >25f) return;
                armFinger1.transform.Rotate(new Vector3(-10f * Time.deltaTime, 0, 0));
                armFinger2.transform.Rotate(new Vector3(-10f * Time.deltaTime, 0, 0));
                break;
            case ARMDIR.Out:
                Vector3 v = getAngle(armFinger1.transform);
                if (v.x < -30f) return;
                armFinger1.transform.Rotate(new Vector3(10f * Time.deltaTime, 0, 0));
                armFinger2.transform.Rotate(new Vector3(10f * Time.deltaTime, 0, 0));
                break;
            case ARMDIR.TurnL:
                armNodeRote.transform.Rotate(new Vector3(0,-10*Time.deltaTime,0));
                break;
            case ARMDIR.TurnR:
                armNodeRote.transform.Rotate(new Vector3(0, 10 * Time.deltaTime, 0));
                break;
            default:
                break;
        }
    }
    private float tether_meters = 20;
    //TMS滚轮控制.
    void TMSTetherControl(string mDataStr)
    {
        if(mDataStr==NetConfig.tether_out_on)
        {
            TMS_Tether.AddTorque(0, 0, 1);
            tether_meters += Time.deltaTime * 10;
        }
        else if(mDataStr==NetConfig.tether_in_on)
        {
            TMS_Tether.AddTorque(0, 0, -1);
            tether_meters -= Time.deltaTime * 10;
        }
        else
        {

        }
        ControlData.Instance.TMS_meters = tether_meters;
    }


    Vector3 getAngle(Transform t)
    {
        Vector3 angle = t.eulerAngles;
        float x = angle.x;
        float y = angle.y;
        float z = angle.z;
        if (Vector3.Dot(transform.up,Vector3.up)>=0f)
        {
            if(angle.x>=0f&&angle.x<=90f)
            {
                x = angle.x;
            }
            if(angle.x>=270f && angle.x<=360f)
            {
                x = angle.x - 360f;
            }
        }
        if (Vector3.Dot(transform.up, Vector3.up) < 0)
        {
            if (angle.x >= 0f && angle.x <= 90f)
            {
                x = 180f - angle.x;
            }
            if (angle.x >= 270f && angle.x <= 360f)
            {
                x = 180f - angle.x;
            }
        }
        if (angle.y > 180)
        {
            y = angle.y - 360f;
        }
        if (angle.z > 180)
        {
            y = angle.z - 360f;
        }
        return new Vector3(x, y, z);
    }

    private void ThrusterControl(float speed,PROPDIR pd)
    {
        if(pd!=PROPDIR.Vertical)
        {
            for (int i = 0; i < H_Propellers.Count; i++)
            {
              //  H_Propellers[i].GetComponent<Rigidbody>().AddRelativeTorque(Vector3.*100);
              H_Propellers[i].transform.Rotate(new Vector3(0,speed,0));
                // H_Propellers[i].GetComponent<Rigidbody>().AddTorque(0,0,speed*100);
               // Debug.LogWarning(H_Propellers[i].GetComponent<Rigidbody>().angularVelocity);

            }
        }
        
        if(pd!=PROPDIR.Horizontal)
        {
            for (int i = 0; i < V_Propellers.Count; i++)
            {
                //H_Propellers[i].GetComponent<Rigidbody>().AddRelativeTorque(Vector3.forward * 10);
                //V_Propellers[i].GetComponent<Rigidbody>().AddTorque(0,speed*100,0);
               V_Propellers[i].transform.Rotate(new Vector3( 0,speed,0));
            }
        }
       
    }

    public  void LampControl(bool showPortSTBD,bool showBulletPT,bool showBottomPT,bool showAll)
    {
        Directional_light_Sun.intensity= (showPortSTBD || showAll) ? 0.5f : 0;
        light_port.intensity=(showPortSTBD || showAll) ? 10 :0;
        light_STBD.intensity=(showPortSTBD || showAll)? 10:0;
        light_bullet.intensity=(showBulletPT || showAll) ? 10:0;
        light_pt.intensity=(showBulletPT || showAll)? 10:0;
        light_pt_bottom.intensity=(showBottomPT || showAll)? 10:0;
        for (int i = 0; i < lamp_port_STBD.Count; i++)
            {
                if (showPortSTBD || showAll)
                    lamp_port_STBD[i].gameObject.GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
                else
                    lamp_port_STBD[i].gameObject.GetComponent<MeshRenderer>().material.DisableKeyword("_EMISSION");
            }

            for (int i = 0; i < lamp_bullet_PT.Count; i++)
            {
                
                if (showBulletPT || showAll)
                    lamp_bullet_PT[i].gameObject.GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
                else
                    lamp_bullet_PT[i].gameObject.GetComponent<MeshRenderer>().material.DisableKeyword("_EMISSION");
            }

            for (int i = 0; i < lamp_bottomPT.Count; i++)
            {
               
                if(showBottomPT || showAll)
                    lamp_bottomPT[i].gameObject.GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
                else
                    lamp_bottomPT[i].gameObject.GetComponent<MeshRenderer>().material.DisableKeyword("_EMISSION");
            }
        }

    public void FlotControl(int speed)
    {
      //float speed=isIn ? -1:1;
        if (flotPan.transform.localPosition.z > 1)
        {
            flotPan.transform.localPosition = new Vector3(flotPan.transform.localPosition.x, flotPan.transform.localPosition.y, 1);
        }
        else if (flotPan.transform.localPosition.z < 0)
        {
            flotPan.transform.localPosition = new Vector3(flotPan.transform.localPosition.x, flotPan.transform.localPosition.y, 0);
        }
        else
            flotPan.transform.Translate(new Vector3(0, 0, speed * Time.deltaTime));
    }

    public void FreezedRotate()
    {
        //_rb.constraints = ~RigidbodyConstraints.FreezeRotationY;
        // _rb.constraints = RigidbodyConstraints.FreezeRotationZ;
        _rb.ResetCenterOfMass();
    }

       
    }





