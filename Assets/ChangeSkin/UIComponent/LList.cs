using System.Collections.Generic;
using UnityEngine;

namespace UIComponent
{
    public class LList : LContainer
    {
        private const string TEMPLET_NAME = "item";
        private GameObject _templet;

        private List<Item> _itemList = new List<Item>();

        protected override void Awake()
        {
            _templet = transform.FindChild(TEMPLET_NAME).gameObject;
            _templet.SetActive(false);
        }

        public void SetData<T>(IList<object> _data) where T : Item
        {
            _itemList.Clear();

            for(int i = 0; i < _data.Count; i++)
            {
                GameObject go = Instantiate(_templet) as GameObject;
                go.SetActive(true);
                go.transform.SetParent(transform, false);
                T item = go.AddComponent<T>();
                item.Data = _data[i];
                _itemList.Add(item);
            }
        }

        public class Item : LContainer
        {
            private object _data;

            public virtual object Data
            {
                set
                {
                    _data = value;
                }
            }
        }
    }


}
