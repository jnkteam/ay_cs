namespace KuaiCard.SysConfig.Menu
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class MenuItem
    {
        private List<MenuItem> _items = new List<MenuItem>();
        private string _link = string.Empty;
        private bool _onlyForAdmin = false;
        private int[] _requirePowers = new int[0];
        private int _systemId = 0;
        private string _text = string.Empty;

        public List<MenuItem> Items
        {
            get
            {
                return this._items;
            }
        }

        public string Link
        {
            get
            {
                return this._link;
            }
            set
            {
                this._link = value;
            }
        }

        public bool OnlyForAdmin
        {
            get
            {
                return this._onlyForAdmin;
            }
            set
            {
                this._onlyForAdmin = value;
            }
        }

        public int[] RequirePowers
        {
            get
            {
                return this._requirePowers;
            }
            set
            {
                this._requirePowers = value;
            }
        }

        public int SystemId
        {
            get
            {
                return this._systemId;
            }
            set
            {
                this._systemId = value;
            }
        }

        public string Text
        {
            get
            {
                return this._text;
            }
            set
            {
                this._text = value;
            }
        }
    }
}

