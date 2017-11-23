using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/// <summary>
/// 角色模型
/// </summary>
namespace OriginalStudio.Model
{
    public class ManageRoles
    {
        private int     _id;  
        private string  _module;
        private int     _type;
        private string  _title;
        private string  _description;
        private int     _status;
        private string  _rules;
        private string  _menu;


        public int Id { get => _id; set => _id = value; }
        public string Module { get => _module; set => _module = value; }
        public int Type { get => _type; set => _type = value; }
        public string Title { get => _title; set => _title = value; }
        public string Description { get => _description; set => _description = value; }
        public int Status { get => _status; set => _status = value; }
        public string Rules { get => _rules; set => _rules = value; }
        public string Menu { get => _menu; set => _menu = value; }
    }
}
