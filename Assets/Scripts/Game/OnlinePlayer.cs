using Astra;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class OnlinePlayer : NetworkBehaviour { 
    [SerializeField] GameObject leftHand;
    [SerializeField] GameObject rightHand;
    [SerializeField] private int sensitivity;

    public static OnlinePlayer LocalInstance;

    public override void OnNetworkSpawn() {
        if (IsOwner) {
            LocalInstance = this;
            
            if (UserDataManager.Instance.userRole == "Doctor") {
                DeactivateHandsServerRPC();
            } 
            else {
                if (sensitivity == 0) {
                    sensitivity = 1;
                }
            }
        }
    }

    [ServerRpc]
    private void DeactivateHandsServerRPC() {
        DeactivateHandsClientRPC();
    }

    [ClientRpc]
    private void DeactivateHandsClientRPC() {
        leftHand.SetActive(false);
        rightHand.SetActive(false);
    }

    void Update()
    {
        if (AstraSDKManager.Instance.Initialized && AstraSDKManager.Instance.IsBodyOn)
        {

            // Get the array of Body objects (you can track upto 5 bodies)
            Body[] bodies = AstraSDKManager.Instance.Bodies;

            if (bodies.Length > 0)
            {
                var body = bodies[0];

                if (body.Status == BodyStatus.Tracking)
                {
                    foreach (var joint in body.Joints)
                    {
                        if (joint.Type == JointType.LeftHand)
                        {
                            leftHand.transform.localPosition = new Vector3(joint.WorldPosition.X, joint.WorldPosition.Y, joint.WorldPosition.Z) / 100 * sensitivity;
                            if (body.HandPoseInfo.LeftHand == Astra.HandPose.Grip)
                            {
                                print("left hand closed");
                            }
                        }
                        else if (joint.Type == JointType.RightHand)
                        {
                            rightHand.transform.localPosition = new Vector3(joint.WorldPosition.X, joint.WorldPosition.Y, joint.WorldPosition.Z) / 100 * sensitivity;
                            if (body.HandPoseInfo.RightHand == Astra.HandPose.Grip)
                            {
                                print("right hand closed");
                            }
                        }
                    }
                }
            }
        }
    }
}