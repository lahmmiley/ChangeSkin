using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIComponent;
using UnityEngine.EventSystems;

namespace Module
{
    public class BattlePreparePanel : LContainer
    {
        private Left _left;

        protected override void Awake()
        {
            _left = AddChildComponent<Left>("Left");
        }
    }
}
