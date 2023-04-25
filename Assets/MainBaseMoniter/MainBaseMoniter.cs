using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//カメラに矢印を収めたいため、Playerを中心に捉えるカメラにPointerをサブクラスにしてください。

public class MainBaseMoniter : MonoBehaviour
{
    [SerializeField] private Transform TargetOfMainBase;
    [SerializeField] private int PointerNumber = 0;
     private Renderer rend;
    // Start is called before the first frame update
    void Start()
    {
        // MainBaseModelのTransformをTargetOfMainBaseに設定する
        rend = GetComponent<Renderer>();
        TargetOfMainBase = GameObject.Find("MainBase").transform;
    }

    // Update is called once per frame
    void Update()
    {
        //MainBaseのある方に矢印を向けさせる。
        //矢印のパーツによって回転を変えている。
        this.gameObject.transform.LookAt(TargetOfMainBase);
        if(PointerNumber == 1)
        {
            this.gameObject.transform.rotation *= Quaternion.Euler(0, 45, 0);
        }
        if(PointerNumber == 2)
        {
            this.gameObject.transform.rotation *= Quaternion.Euler(0, -45, 0);
        }

        //MainBaseが見えているかいないかを判定して、矢印の表示を変更する。
        if (IsVisibleFrom(TargetOfMainBase.GetComponent<Renderer>(), Camera.main))
        {
            rend.enabled = false;
        }
        else
        {
            rend.enabled = true;
        }
    }

    //Rendererがカメラに写っているかどうかを判定する関数
    bool IsVisibleFrom(Renderer renderer, Camera camera)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
        return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
    }
}