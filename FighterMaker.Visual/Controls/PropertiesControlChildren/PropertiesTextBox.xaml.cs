using System.Windows.Controls;
using System.Windows.Input;

namespace FighterMaker.Visual.Controls.PropertiesControlChildren
{
    /// <summary>
    /// Representa um controle com um nome de exibição e uma caixa de texto.
    /// </summary>
    public partial class PropertiesTextBox : UserControl
    {
        static readonly Dictionary<Type, Func<string, bool>> KnownTypesParseFuncs = new Dictionary<Type, Func<string, bool>>
        {
            { typeof(char), (s) => char.TryParse(s, out _) },
            { typeof(byte), (s) => byte.TryParse(s, out _) },
            { typeof(sbyte), (s) => sbyte.TryParse(s, out _) },
            { typeof(short), (s) => short.TryParse(s, out _) },
            { typeof(ushort), (s) => ushort.TryParse(s, out _) },
            { typeof(int), (s) => int.TryParse(s, out _) },
            { typeof(uint), (s) => uint.TryParse(s, out _) },
            { typeof(long), (s) => long.TryParse(s, out _) },
            { typeof(ulong), (s) => ulong.TryParse(s, out _) },
            { typeof(double), (s) => double.TryParse(s, out _) },
            { typeof(float), (s) => float.TryParse(s, out _) }
        };

        private string? initialValue = null;
        private Type? validType;

        /// <summary>
        /// Obtém o controle Label.
        /// </summary>
        public Label LabelControl { get => DisplayLabel; }
        /// <summary>
        /// Obtém o controle TextBox.
        /// </summary>
        public TextBox TextBoxControl { get => ValueTextBox; }
        /// <summary>
        /// Obtém ou define o tipo a ser validado ao ser mudado o texto no TextBox.
        /// </summary>
        public Type? ValidType { get => validType; set => validType = value; }
        /// <summary>
        /// O valor inicial do TextBox.
        /// </summary>
        public string? InitialValue
        {
            get => initialValue;
            set
            {
                initialValue = value;
                ValueTextBox.Text = value;
            }
        }        

        /// <summary>
        /// Invocado quando o controle perde o foco.
        /// </summary>
        public event EventHandler<string>? TextChanged;

        public PropertiesTextBox()
        {
            InitializeComponent();
        }        

        private void ValueTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Tab)
            {
                if (ValidType != null && KnownTypesParseFuncs.TryGetValue(ValidType, out var func))
                {
                    if (!func.Invoke(TextBoxControl.Text))
                    {
                        TextBoxControl.Text = initialValue;
                    }
                }

                if (initialValue != null && TextBoxControl.Text == initialValue)
                    return;

                TextChanged?.Invoke(sender, ValueTextBox.Text);
            }
        }
    }
}
