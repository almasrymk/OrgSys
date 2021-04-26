using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Utility;

namespace OrgSysApp.Tools
{
    public partial class IndexPanel : UserControl
    {
        public IndexPanel()
        {
            InitializeComponent();
        }

        IndexMode _mode = IndexMode.Elements;
        List<Field> _fields = new List<Field>();
        List<dynamic> _data = new List<dynamic>();

        [Category("Org")]
        public IndexMode Mode
        {
            get { return _mode; }
            set
            {
                _mode = value;
                ChangeMode();
            }
        }

        [Category("Org")]
        public List<Field> Fields
        {
            get { return _fields; }
            set
            {
                _fields = value;
                ChangeMode();
            }
        }
       
        public void DataSource<t>(List<t> data)
        {
            _data = new List<dynamic>();
            foreach (var d in data)
                _data.Add(d);
            ChangeMode();
        }

        public void ChangeMode()
        {
            this.Controls.Clear();
            switch (_mode)
            {
                case IndexMode.Elements:
                    ElementMode();
                    break;
                case IndexMode.Grids:
                    this.Controls.Add(GrideMode());
                    break;
                case IndexMode.Bars:
                    BarMode();
                    break;
                default:
                    break;
            }
        }

        public void ElementMode()
        {
            
        }

        public DataGridView GrideMode()
        {
            DataGridView grid = new DataGridView();
            grid.AutoGenerateColumns = false;
            grid.Columns.Clear();
            grid.Dock = DockStyle.Fill;
            foreach (var field in _fields.Where(e=>e.ShowIn == ShowInIndexMode.All || e.ShowIn == ShowInIndexMode.Grids).ToList())
            {
                DataGridViewColumn column = new DataGridViewColumn();
                DataGridViewCell cell = new DataGridViewTextBoxCell();
                column.CellTemplate = cell;
                column.DataPropertyName = field.Name;
                column.Name = "col" + field.Name;
                column.Width = field.Width;
                column.Visible = field.Visible;
                column.HeaderText = field.Title;
                column.DefaultCellStyle.ForeColor = field.color;
                column.DefaultCellStyle.BackColor = field.Backgroundcolor;
                column.DefaultCellStyle.Font = field.font;
                grid.Columns.Add(column);
            }
            if (_data != null)
                grid.DataSource = _data;
            return grid;
        }

        public void BarMode()
        {
            
        }
    }

    public class Field
    {      
        public string Name { get; set; }
        public string Title { get; set; }
        public string TitleCulture { get; set; }
        public int Width { get; set; }
        public Color color { get; set; }
        public Color colorFocus { get; set; }
        public Color Backgroundcolor { get; set; }
        public Color BackgroundcolorFocus { get; set; }
        public Font font { get; set; }
        public bool Visible { get; set; } = true;
        public ShowInIndexMode ShowIn { get; set; }
    }
}