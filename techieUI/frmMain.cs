using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace com.techphernalia.windows.forms.techieUI
{
    public partial class frmMain : Form
    {
        public string PrimaryLanguage { get; set; }
        private frmOutput Out = new frmOutput();
        private TreeData _current;
        public frmMain(string title,List<TreeDataList> items,string primaryLanguage)
        {
            InitializeComponent();
            this.PrimaryLanguage = primaryLanguage;
            this.Text = title + " : By Durgesh Chaudhary for techPhernalia.com";

            TreeNode rootNode = new TreeNode { Text = title, Tag = null , ImageKey = ImageKey.TOP , SelectedImageKey = ImageKey.TOP };
            treeResources.Nodes.Add(rootNode);
            foreach (TreeDataList item in items)
            {
                TreeNode node = new TreeNode { Text = item.Title , Tag = null , ImageKey = ImageKey.TOPIC , SelectedImageKey = ImageKey.TOPIC };
                TreeNode CategoryNode = null;
                string category = string.Empty;
                foreach (TreeData data in item)
                {
                    if (!data.Category.ToLower().Equals(category) 
                        || string.IsNullOrEmpty(category))
                    {
                        category = data.Category.ToLower();
                        CategoryNode = new TreeNode { Text = data.Category, Tag = null, ImageKey = ImageKey.CLOSE, SelectedImageKey = ImageKey.CLOSE };
                        node.Nodes.Add(CategoryNode);
                    }
                    TreeNode dataNode = new TreeNode { Text = data.Title, Tag = data, ImageKey = ImageKey.CLOSE, SelectedImageKey = ImageKey.CLOSE };
                    CategoryNode.Nodes.Add(dataNode);
                }
                treeResources.Nodes.Add(node);
                //treeResources.ExpandAll();
            }
        }
        private void treeResources_AfterSelect(object sender, TreeViewEventArgs e)
        {
            _current = ((TreeData)e.Node.Tag);
            if (_current != null)
            {
                txtDescription.Text = _current.Description;
                if (_current.Method != null)
                    btnRun.Enabled = true;
                if (_current.Code != null)
                {
                    if (_current.Code.Count > 0)
                    {
                        cmbLanguage.Enabled = true;
                        cmbLanguage.DataSource = _current.Code;
                        var x = from c in _current.Code where c.Lang.ToLower().Equals(PrimaryLanguage.ToLower()) select c;
                        if (x.Count() > 0)
                            cmbLanguage.SelectedItem = x.First();
                    }
                }
            }
            else
            {
                txtDescription.Text = "";
                txtCode.Text = "";
                btnRun.Enabled = false;
                cmbLanguage.Enabled = false;
            }
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            if (_current != null)
            {
                Out.T.Text = "";
                StreamWriter writer = _current.ParentList.ConsoleWriter;
                TextWriter oldConsoleOut = Console.Out;
                Console.SetOut(writer);
                MemoryStream stream = (MemoryStream)writer.BaseStream;
                stream.SetLength(0);
                _current.Execute();
                writer.Flush();
                Console.SetOut(oldConsoleOut);
                Out.T.Text += writer.Encoding.GetString(stream.ToArray());

                Out.ShowDialog();
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
        }

        private void cmbLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtCode.Text = ((CodeBlock)cmbLanguage.SelectedItem).Code;
        }

        private void treeResources_AfterExpand(object sender, TreeViewEventArgs e)
        {
            switch (e.Node.Level)
            {
                case 1:
                case 2:
                    e.Node.ImageKey = ImageKey.OPEN;
                    e.Node.SelectedImageKey = ImageKey.OPEN;
                    break;
            }
        }

        private void treeResources_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            switch (e.Node.Level)
            {
                case 1:
                case 2:
                    e.Node.ImageKey = ImageKey.CLOSE;
                    e.Node.SelectedImageKey = ImageKey.CLOSE;
                    break;
            }
        }
    }
    public static class ImageKey
    {
        public static string OPEN = "Open";
        public static string CLOSE = "Closed";
        public static string TOPIC = "Topic";
        public static string TOP = "Top";
    }
}
