// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Text;

// using UnityEngine;

// public class TestPool : MonoBehaviour {

//     void Start () {
//         LVerticalScrollRect verScroll = GameObject.Find ("Canvas/DemoPoolWin/Window/Panel").GetComponent<LVerticalScrollRect> ();
//         GameObject cloner = GameObject.Instantiate (GameObject.Find ("Canvas").transform.FindChild ("Image").gameObject) as GameObject;
//         GameObject root = GameObject.Find ("PoolRoot");
//         verScroll.SetPoolInfo(500, GetLPoolData);

//         LHorizontalScrollRect hScroll = GameObject.Find ("Canvas/DemoPoolWin/Window/HPanel").GetComponent<LHorizontalScrollRect> ();
//         hScroll.SetPoolInfo(50, GetLPoolData);
//     }

//     private LPoolData GetLPoolData (int index) {
//         LPoolData data = new LPoolData ();
//         if (index % 2 == 0) {
//             data.name = "偶数";
//         } else {
//             data.name = "奇数";
//         }
//         data.lev = index;
//         return data;
//     }
// }
