using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Horn_War_II.Spawn
{
    public partial class ValueSelect : Form
    {
        Type valueType;
        object CustControl;

        public object Selected;
        
        public ValueSelect(string Title, Type valueType)
        {
            InitializeComponent();
            this.Text = Title;
            this.valueType = valueType;

            var location = new Point(12, 13);
            var size = new Size(329, 21);

            if (valueType.IsEnum) // ENUM
            {
                var comboBox = new ComboBox()
                {
                    Parent = this,
                    Location = location,
                    Size = size,
                    Visible = true,
                };
                CustControl = comboBox;
                foreach (var EnumName in valueType.GetEnumNames())
                {
                    comboBox.Items.Add(EnumName);
                }
            }
            else if(IsNumericType(valueType)) // NUMBER
            {
                var numericInput = new NumericUpDown()
                {
                    Parent = this,
                    Location = location,
                    Size = size,
                    Visible = true,
                };
                CustControl = numericInput;
            }
            else if (valueType == typeof(string)) // STRING
            {
                var textInput = new TextBox()
                {
                    Parent = this,
                    Location = location,
                    Size = size,
                    Visible = true,
                };
                CustControl = textInput;
            }
            else if (valueType.IsValueType) // Structure
            {
                Selected = Activator.CreateInstance(valueType);

                var pgSize = 400;
                size.Height += pgSize;
                this.Height += pgSize;
                this.b_ok.Location =  new Point(this.b_ok.Location.X, this.b_ok.Location.Y + pgSize);
                this.b_abort.Location = new Point(this.b_abort.Location.X, this.b_abort.Location.Y + pgSize);

                var propertyGrid = new PropertyGrid()
                {
                    Parent = this,
                    Location = location,
                    Size = size,
                    Visible = true,
                    SelectedObject = Selected
                };
                CustControl = propertyGrid;
            }
            else
            {
                this.DialogResult = DialogResult.Abort;
                this.Close();
            }
        }

        private void b_ok_Click(object sender, EventArgs e)
        {

            if (valueType.IsEnum)
            {
                if (((ComboBox)CustControl).SelectedIndex < 0)
                {
                    MessageBox.Show("Please select an entry");
                    return;
                }

                Selected = Enum.Parse(valueType, ((ComboBox)CustControl).Text);
            }
            else if (IsNumericType(valueType))
            {
                Selected = Convert.ChangeType(((NumericUpDown)CustControl).Value, valueType);
            }
            else if (valueType == typeof(string)) // STRING
            {
                Selected = ((TextBox)CustControl).Text;
            }
            else if (valueType.IsValueType) // Struct
            {
                Selected = ((PropertyGrid)CustControl).SelectedObject;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void b_abort_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Abort;
            this.Close();
        }

        private bool IsNumericType(Type o)
        {
            switch (Type.GetTypeCode(o))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }
    }
}
