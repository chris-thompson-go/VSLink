using System;
using System.Drawing;
using System.Windows.Forms;

namespace VsLink
{
	public class BorderedTextBox : UserControl
	{
		public BorderedTextBox()
		{
			textBox = new TextBox()
			{
				BorderStyle = BorderStyle.FixedSingle,
				Location = new Point(-1, -1),
				Anchor = AnchorStyles.Top | AnchorStyles.Bottom |
						 AnchorStyles.Left | AnchorStyles.Right
			};
			Control container = new ContainerControl()
			{
				Dock = DockStyle.Fill,
				Padding = new Padding(-1)
			};
			container.Controls.Add(textBox);
			this.Controls.Add(container);

			DefaultBorderColor = SystemColors.ControlDark;
			FocusedBorderColor = SystemColors.ControlDark;
			BackColor = DefaultBorderColor;
			Padding = new Padding(1);
			Size = textBox.Size;
		}

		public new event EventHandler TextChanged
		{
			add { textBox.TextChanged += value; }
			remove { textBox.TextChanged -= value; }
		}

		public Color DefaultBorderColor { get; set; }

		public Color FocusedBorderColor { get; set; }

		public bool CanUndo
		{
			get { return textBox.CanUndo; }
		}

		public bool CanRedo
		{
			get { return false; }
		}

		public bool CanPaste
		{
			get { return true; }
		}

		public void Undo()
		{
			textBox.Undo();
		}

		public void Cut()
		{
			textBox.Cut();
		}

		public void Copy()
		{
			textBox.Copy();
		}

		public void Paste()
		{
			textBox.Paste();
		}

		public override string Text
		{
			get { return textBox.Text; }
			set { textBox.Text = value; }
		}

		public void SetBorderColor(Color color)
		{
			BackColor = color;
			DefaultBorderColor = color;
			FocusedBorderColor = color;
		}

		protected override void OnEnter(EventArgs e)
		{
			BackColor = FocusedBorderColor;
			base.OnEnter(e);
		}

		protected override void OnLeave(EventArgs e)
		{
			BackColor = DefaultBorderColor;
			base.OnLeave(e);
		}

		protected override void SetBoundsCore(int x, int y,
			int width, int height, BoundsSpecified specified)
		{
			base.SetBoundsCore(x, y, width, textBox.PreferredHeight, specified);
		}

		private TextBox textBox;
	}
}