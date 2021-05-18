using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
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

            if (_data == null)
                _data = new List<dynamic>();

            if (_image == null)
                _image = null;

            if (_imageActive == null)
                _imageActive = null;

            #region Grid
            if (_fieldsGrid == null || _fieldsGrid.Count == 0)
                _fieldsGrid = new List<Field>();

            if (_backgroundColorGrid == null)
                _backgroundColorGrid = Color.White;

            if (_colorGrid == null)
                _colorGrid = Color.Black;

            if (_backgroundColorHeaderGrid == null)
                _backgroundColorHeaderGrid = Color.White;

            if (_colorHeaderGrid == null)
                _colorHeaderGrid = Color.Black;

            if (_headersHeightGrid == 0)
                _headersHeightGrid = 30;
            #endregion

            #region Element           
            if (_widthElement == 0)
                _widthElement = 100;
            if (_heightElement == 0)
                 _heightElement = 150;
            #endregion

            #region Bar                      
            if (_heightBar == 0)
                _heightBar = 50;
            #endregion
        }

        IndexMode _mode = IndexMode.Elements;
        List<dynamic> _data;
        Image _image;
        Image _imageActive;

        #region Grid
        List<Field> _fieldsGrid;
        Color _backgroundColorGrid;
        Color _colorGrid;
        Color _backgroundColorHeaderGrid;
        Color _colorHeaderGrid;
        Font _fontGrid;
        int _headersHeightGrid;
        #endregion

        #region Element
        bool _withDescriptionElement;
        string _nameElement;
        string _descriptionElement; 
        int _widthElement;
        int _heightElement;
        #endregion

        #region Element       
        string _nameBar;
        string _descriptionBar;
        int _heightBar;
        #endregion

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
        public Image image
        {
            get { return _image; }
            set { _image = value; }
        }

        [Category("Org")]
        public Image imageActive
        {
            get { return _imageActive; }
            set { _imageActive = value; }
        }                  

        #region Grid
        [Category("Org.Grid")]
        public List<Field> FieldsGrid
        {
            get { return _fieldsGrid; }
            set
            {
                _fieldsGrid = value;
                ChangeMode();
            }
        }

        [Category("Org.Grid")]
        public Color BackGroundColorGrid
        {
            get { return _backgroundColorGrid; }
            set
            {
                _backgroundColorGrid = value;
                ChangeMode();
            }
        }

        [Category("Org.Grid")]
        public Color ColorGrid
        {
            get { return _colorGrid; }
            set
            {
                _colorGrid = value;
                ChangeMode();
            }
        }

        [Category("Org.Grid")]
        public int HeadersHeightGrid
        {
            get { return _headersHeightGrid; }
            set
            {
                _headersHeightGrid = value;
                ChangeMode();
            }
        }

        [Category("Org.Grid")]
        public Color BackGroundColorHeaderGrid
        {
            get { return _backgroundColorHeaderGrid; }
            set
            {
                _backgroundColorHeaderGrid = value;
                ChangeMode();
            }
        }

        [Category("Org.Grid")]
        public Color ColorHeaderGrid
        {
            get { return _colorHeaderGrid; }
            set
            {
                _colorHeaderGrid = value;
                ChangeMode();
            }
        }

        [Category("Org.Grid")]
        public Font FontGrid
        {
            get { return _fontGrid; }
            set
            {
                _fontGrid = value;
                ChangeMode();
            }
        }
        #endregion

        #region Element
        [Category("Org.Element")]
        public int WidthElement
        {
            get { return _widthElement; }
            set { _widthElement = value; }
        }

        [Category("Org.Element")]
        public int HeightElement
        {
            get { return _heightElement; }
            set { _heightElement = value; }
        }

        [Category("Org.Element")]
        public string NameElement
        {
            get { return _nameElement; }
            set { _nameElement = value; }
        }

        [Category("Org.Element")]
        public string DescriptionElement
        {
            get { return _descriptionElement; }
            set { _descriptionElement = value; }
        }

        [Category("Org.Element")]
        public bool WithDescriptionElement
        {
            get { return _withDescriptionElement; }
            set { _withDescriptionElement = value; }
        }
        #endregion

        #region Bar       
        [Category("Org.Bar")]
        public int HeightBar
        {
            get { return _heightBar; }
            set { _heightBar = value; }
        }

        [Category("Org.Bar")]
        public string NameBar
        {
            get { return _nameBar; }
            set { _nameBar = value; }
        }

        [Category("Org.Bar")]
        public string DescriptionBar
        {
            get { return _descriptionBar; }
            set { _descriptionBar = value; }
        }
        #endregion       

        public void DataSource<t>(List<t> data)
        {
            _data = new List<dynamic>();
            foreach (var d in data)
                _data.Add(d);
            ChangeMode();
        }

        public void ChangeMode()
        {
            flpIndexPanelBody.Controls.Clear();
            switch (_mode)
            {
                case IndexMode.Elements:
                    flpIndexPanelBody.Controls.AddRange(ElementMode());
                    break;
                case IndexMode.Grids:
                    flpIndexPanelBody.Controls.Add(GrideMode());
                    break;
                case IndexMode.Bars:
                    BarMode();
                    break;
                default:
                    break;
            }
        }

        public Element[] ElementMode()
        {
            List<Element> list = new List<Element>();
            foreach (var ob in _data)
            {
                Element element = new Element();
                element.WithDescription = _withDescriptionElement;
                element.Width = _widthElement;
                element.Height = _heightElement;
                element.image = _image;
                element.imageActive = _imageActive;

                Type t = ob.GetType();
                PropertyInfo p = t.GetProperty(_nameElement);
                if (p != null)
                    element.Title = p.GetValue(ob, null).ToString();

                if (_withDescriptionElement)
                {
                    Type t1 = ob.GetType();
                    PropertyInfo p1 = t1.GetProperty(_descriptionElement);
                    if (p1 != null)
                        element.Description = p1.GetValue(ob, null).ToString();
                }
                list.Add(element);
            }
            return list.ToArray();
        }

        public DataGridView GrideMode()
        {
            DataGridView grid = new DataGridView();
            grid.AutoGenerateColumns = false;
            grid.Columns.Clear();
            grid.Dock = DockStyle.Fill;
            grid.BorderStyle = BorderStyle.None;
            grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            grid.RowHeadersVisible = false;
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            grid.EnableHeadersVisualStyles = false;

            grid.ColumnHeadersHeight = _headersHeightGrid;
            grid.BackgroundColor = _backgroundColorGrid;
            grid.ColumnHeadersDefaultCellStyle.BackColor = _backgroundColorHeaderGrid;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = _colorHeaderGrid;
            grid.Font = _fontGrid;
            grid.ForeColor = _colorGrid;

            foreach (var field in _fieldsGrid.ToList())
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

        private void GridMode_Click(object sender, EventArgs e)
        {
            Mode = IndexMode.Grids;
        }

        private void ElementModel_Click(object sender, EventArgs e)
        {
            Mode = IndexMode.Elements;
        }
        
        private void BarMode_Click(object sender, EventArgs e)
        {
            Mode = IndexMode.Bars;
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
    }
}