using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIComponent;
using UnityEngine.UI;

namespace Module
{
    public class Left : LContainer
    {
        private ScrollRect _scrollRect;
        private LList _list;
        private HorizontalLayoutGroup _layoutGroup;


        private List<object> _data;

        protected override void Awake()
        {
            _scrollRect = GetChildComponent<ScrollRect>("LeftTop/Head");
            _scrollRect.vertical = false;
            _list = AddChildComponent<LList>("LeftTop/Head/mask/Content");
            _layoutGroup = GetChildComponent<HorizontalLayoutGroup>("LeftTop/Head/mask/Content");
            _layoutGroup.spacing = 300f;
        }

        protected override void Start()
        {
            _data = new List<object>();
            _data.Add("hello list0");
            _data.Add("hello list1");
            _data.Add("hello list2");
            _list.SetData<Head>(_data);
        }

        class Head : LList.Item
        {
            private Image _icon;
            private Text _desc;

            protected override void Awake()
            {
                _icon = GetChildComponent<Image>("heroHead");
                _desc = GetChildComponent<Text>("heroDesc");
            }

            public override object Data
            {
                set
                {
                    _desc.text = value.ToString();
                }
            }

        }
    }
}
