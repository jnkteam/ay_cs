namespace KuaiCard.SysConfig.Menu
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class SystemInstance
    {
        private List<SystemInstance> _items = new List<SystemInstance>();
        private string _name = string.Empty;
        private bool _onlyForAdmin = false;
        private int _parentId = 0;
        private bool _release = true;
        private int _systemId = 0;
        private KuaiCard.SysConfig.Menu.SystemType _systemType = KuaiCard.SysConfig.Menu.SystemType.Custom;

        public List<SystemInstance> Items
        {
            get
            {
                return this._items;
            }
        }

        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
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

        public int ParentId
        {
            get
            {
                return this._parentId;
            }
            set
            {
                this._parentId = value;
            }
        }

        public bool Release
        {
            get
            {
                return this._release;
            }
            set
            {
                this._release = value;
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

        public KuaiCard.SysConfig.Menu.SystemType SystemType
        {
            get
            {
                return this._systemType;
            }
            set
            {
                this._systemType = value;
            }
        }
    }
}

