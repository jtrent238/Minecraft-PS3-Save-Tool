using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Minecraft.NBT;

namespace Minecraft_PS3_Save_Tool
{
	public partial class ItemDataForm : Form
	{
		ItemSlot slot;
		
		public ItemDataForm()
		{
			InitializeComponent();
		}
		
		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);
			CenterToParent();
		}
		
		public void Update(ItemSlot slot)
		{
			boxName.TextChanged -= BoxNameTextChanged;
			boxLore.TextChanged -= BoxLoreTextChanged;
			boxColor.TextChanged -= BoxColorTextChanged;
			boxPlayer.TextChanged -= BoxPlayerTextChanged;
			
			this.slot = slot;
			Item item = ((slot != null) ? slot.Item : null);
			
			boxName.Text = "";
			boxLore.Text = "";
			boxColor.Text = "";
			panelColor.BackColor = Color.Transparent;
			boxPlayer.Text = "";
			
			boxName.Enabled = (item != null);
			boxLore.Enabled = (item != null);
			boxColor.Enabled = (item != null && (item.ID == 298 || item.ID == 299 ||
			                                     item.ID == 300 || item.ID == 301));
			panelColor.Enabled = boxColor.Enabled;
			boxPlayer.Enabled = (item != null && item.ID == 397);
			
			if (item != null && item.tag.Contains("tag")) {
				var tag = item.tag["tag"];
				if (tag.Contains("display")) {
					var display = tag["display"];
					if (display.Contains("Name"))
						boxName.Text = (string)display["Name"];
					if (display.Contains("Lore"))
						boxLore.Lines = display["Lore"].Select((t) => (string)t).ToArray();
					if (boxColor.Enabled && display.Contains("color")) {
						int c = (int)display["color"];
						Color color = Color.FromArgb(c >> 16, (c >> 8) & 0xFF, c & 0xFF);
						boxColor.Text = (color.ToArgb() & 0xFFFFFF).ToString("X6");
						panelColor.BackColor = color;
					}
				}
				if (boxPlayer.Enabled && tag.Contains("SkullOwner"))
					boxPlayer.Text = (string)tag["SkullOwner"];
			}
			
			boxName.TextChanged += BoxNameTextChanged;
			boxLore.TextChanged += BoxLoreTextChanged;
			boxColor.TextChanged += BoxColorTextChanged;
			boxPlayer.TextChanged += BoxPlayerTextChanged;
		}
		
		void BtnHelpClick(object sender, EventArgs e)
		{
			var result = MessageBox.Show("There's a number of formatting codes which you can use\n" +
			                             "to make text in different colors and styles. Do you want\n" +
			                             "to go to the Minecraft Wiki page on formatting codes?",
			                             "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
			if (result == DialogResult.Yes)
				Process.Start("http://www.minecraftwiki.net/wiki/Formatting_codes");
		}
		
		void BtnHelp2Click(object sender, EventArgs e)
		{
			MessageBox.Show("When items are placed in the world, they lose their item NBT data.\n" +
			                "Player heads, when placed, will use the skin of a player if you entered\n" +
			                "their name here, but it will always drop a generic head when broken.",
			                "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}
		
		void BoxNameTextChanged(object sender, EventArgs e)
		{
			var tag = (slot.Item.tag.Contains("tag") ? slot.Item.tag["tag"] : null);
			var display = ((tag != null && tag.Contains("display")) ? tag["display"] : null);
			var name = boxName.Text;
			if (name != "") {
				if (tag == null) {
					tag = NbtTag.CreateCompound();
					slot.Item.tag.Add("tag", tag);
				}
				if (display == null) {
					display = NbtTag.CreateCompound();
					tag.Add("display", display);
				}
				if (display.Contains("Name"))
					display["Name"].Value = name;
				else display.Add("Name", name);
			} else display["Name"].Remove();
			slot.CallChanged();
		}
		
		void BoxLoreTextChanged(object sender, EventArgs e)
		{
			var tag = (slot.Item.tag.Contains("tag") ? slot.Item.tag["tag"] : null);
			var display = ((tag != null && tag.Contains("display")) ? tag["display"] : null);
			if (boxLore.Text != "") {
				if (tag == null) {
					tag = NbtTag.CreateCompound();
					slot.Item.tag.Add("tag", tag);
				}
				if (display == null) {
					display = NbtTag.CreateCompound();
					tag.Add("display", display);
				}
				var lore = (display.Contains("Lore") ? display["Lore"] : null);
				if (lore == null) {
					lore = NbtTag.CreateList(NbtTagType.String);
					display.Add("Lore", lore);
				} else lore.Clear();
				lore.Add(boxLore.Lines);
			} else display["Lore"].Remove();
			slot.CallChanged();
		}
		
		void PanelColorClick(object sender, EventArgs e)
		{
			if (colorDialog.ShowDialog() == DialogResult.Cancel) return;
			var color = colorDialog.Color;
			boxColor.Text = (color.ToArgb() & 0xFFFFFF).ToString("X6");
		}
		
		void BoxColorTextChanged(object sender, EventArgs e)
		{
			Color color;
			if (!ValidateColor(boxColor.Text, out color)) return;
			if (color == panelColor.BackColor) return;
			panelColor.BackColor = color;
			
			int c = color.ToArgb() & 0xFFFFFF;
			
			var tag = (slot.Item.tag.Contains("tag") ? slot.Item.tag["tag"] : null);
			var display = ((tag != null && tag.Contains("display")) ? tag["display"] : null);
			if (color != Color.Transparent) {
				if (tag == null) {
					tag = NbtTag.CreateCompound();
					slot.Item.tag.Add("tag", tag);
				}
				if (display == null) {
					display = NbtTag.CreateCompound();
					tag.Add("display", display);
				}
				if (display.Contains("color"))
					display["color"].Value = c;
				else display.Add("color", c);
			} else display["color"].Remove();
			slot.CallChanged();
		}
		
		void BoxPlayerTextChanged(object sender, EventArgs e)
		{
			var tag = (slot.Item.tag.Contains("tag") ? slot.Item.tag["tag"] : null);
			var player = boxPlayer.Text;
			if (player != "") {
				if (tag == null) {
					tag = NbtTag.CreateCompound();
					slot.Item.tag.Add("tag", tag);
				}
				if (tag.Contains("SkullOwner"))
					tag["SkullOwner"].Value = player;
				else tag.Add("SkullOwner", player);
			} else tag["SkullOwner"].Remove();
			slot.CallChanged();
		}
		
		bool ValidateColor(string str, out Color color)
		{
			color = Color.Transparent;
			if (str == "") return true;
			if (str.Length != 6) return false;
			int c;
			if (!int.TryParse(str, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out c)) return false;
			color = Color.FromArgb(c >> 16, (c >> 8) & 0xFF, c & 0xFF);
			return true;
		}
	}
}
