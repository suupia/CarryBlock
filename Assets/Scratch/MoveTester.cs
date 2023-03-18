using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTester : MonoBehaviour
{
    [SerializeField] GameObject targetObj;
    float detectionRange = 12.0f;
    PickerInfo info;
    void Start()
    {
        var infoWrapper = new PlayerInfoWrapper(new PlayerInfo());

       info = new PickerInfo(this.gameObject,infoWrapper);

       info.pickerRd.velocity = new Vector3(10,0,10);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var endPos = targetObj.transform.position;
        TMoveToNormal(endPos);
    }

    protected void TMoveTo(Vector3 endPos, float acceleration, float maxVelocity, ForceMode forceMode)
    {
        var startPos = info.pickerObj.transform.position;
        var directionVec = Utility.SetYToZero(endPos - startPos).normalized;
        var distance = Utility.SetYToZero(endPos - startPos).magnitude;
        if (distance > info.decelerationRange)
        {
            info.pickerRd.AddForce(acceleration * directionVec, forceMode);
            if (info.pickerRd.velocity.magnitude >= maxVelocity) info.pickerRd.velocity = maxVelocity * info.pickerRd.velocity.normalized;
        }
        else
        {
            Debug.Log($"Œ¸‘¬‚µ‚Ü‚·");
            var weight = distance / info.decelerationRange;
            var accelerationVector = ((weight * directionVec - info.pickerRd.velocity) / Time.fixedDeltaTime); // The solution obtained by solving the equation.
            //var accelerationWeight = distance / info.decelerationRange;
            info.pickerRd.AddForce(accelerationVector, ForceMode.Acceleration); // Note that ForceMode has been changed to Force.
            //info.pickerRd.velocity =  maxVelocity * info.pickerRd.velocity.normalized;
            Debug.Log($"weight:{weight}, accelerationVector:{accelerationVector}");

        }
    }

    protected void TMoveToNormal(Vector3 endPos) { TMoveTo(endPos, info.normalAcceleration, info.normalMaxVelocity, ForceMode.Impulse); }


}
