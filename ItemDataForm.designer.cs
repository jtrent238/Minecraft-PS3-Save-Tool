/*
 * Created by SharpDevelop.
 * User: copygirl
 * Date: 29.10.2012
 * Time: 13:18
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace Minecraft_PS3_Save_Tool
{
	partial class ItemDataForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ItemDataForm));
			this.boxLore = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.boxName = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.boxPlayer = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.boxColor = new System.Windows.Forms.TextBox();
			this.colorDialog = new System.Windows.Forms.ColorDialog();
			this.label5 = new System.Windows.Forms.Label();
			this.panelColor = new System.Windows.Forms.Panel();
			this.btnHelp = new System.Windows.Forms.Button();
			this.btnHelp2 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// boxLore
			// 
			this.boxLore.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.boxLore.Enabled = false;
			this.boxLore.Location = new System.Drawing.Point(57, 33);
			this.boxLore.Multiline = true;
			this.boxLore.Name = "boxLore";
			this.boxLore.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.boxLore.Size = new System.Drawing.Size(250, 107);
			this.boxLore.TabIndex = 4;
			this.boxLore.TextChanged += new System.EventHandler(this.BoxLoreTextChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(7, 33);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(46, 18);
			this.label1.TabIndex = 3;
			this.label1.Text = "Lore:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// boxName
			// 
			this.boxName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.boxName.Enabled = false;
			this.boxName.Location = new System.Drawing.Point(57, 7);
			this.boxName.Name = "boxName";
			this.boxName.Size = new System.Drawing.Size(220, 20);
			this.boxName.TabIndex = 1;
			this.boxName.TextChanged += new System.EventHandler(this.BoxNameTextChanged);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(7, 7);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(46, 18);
			this.label2.TabIndex = 0;
			this.label2.Text = "Name:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(7, 172);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(46, 18);
			this.label3.TabIndex = 9;
			this.label3.Text = "Player:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// boxPlayer
			// 
			this.boxPlayer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.boxPlayer.Enabled = false;
			this.boxPlayer.Location = new System.Drawing.Point(57, 172);
			this.boxPlayer.Name = "boxPlayer";
			this.boxPlayer.Size = new System.Drawing.Size(220, 20);
			this.boxPlayer.TabIndex = 10;
			this.boxPlayer.TextChanged += new System.EventHandler(this.BoxPlayerTextChanged);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(7, 146);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(46, 18);
			this.label4.TabIndex = 5;
			this.label4.Text = "Color:";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// boxColor
			// 
			this.boxColor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.boxColor.Enabled = false;
			this.boxColor.Font = new System.Drawing.Font("Courier New", 7.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.boxColor.Location = new System.Drawing.Point(241, 146);
			this.boxColor.MaxLength = 6;
			this.boxColor.Name = "boxColor";
			this.boxColor.Size = new System.Drawing.Size(66, 20);
			this.boxColor.TabIndex = 8;
			this.boxColor.TextChanged += new System.EventHandler(this.BoxColorTextChanged);
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(226, 146);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(17, 18);
			this.label5.TabIndex = 7;
			this.label5.Text = "#";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// panelColor
			// 
			this.panelColor.BackColor = System.Drawing.Color.Transparent;
			this.panelColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panelColor.Enabled = false;
			this.panelColor.Location = new System.Drawing.Point(57, 146);
			this.panelColor.Name = "panelColor";
			this.panelColor.Size = new System.Drawing.Size(169, 20);
			this.panelColor.TabIndex = 6;
			this.panelColor.Click += new System.EventHandler(this.PanelColorClick);
			// 
			// btnHelp
			// 
			this.btnHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnHelp.Image = ((System.Drawing.Image)(resources.GetObject("btnHelp.Image")));
			this.btnHelp.Location = new System.Drawing.Point(283, 4);
			this.btnHelp.Name = "btnHelp";
			this.btnHelp.Size = new System.Drawing.Size(26, 26);
			this.btnHelp.TabIndex = 2;
			this.btnHelp.UseVisualStyleBackColor = true;
			this.btnHelp.Click += new System.EventHandler(this.BtnHelpClick);
			// 
			// btnHelp2
			// 
			this.btnHelp2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnHelp2.Image = ((System.Drawing.Image)(resources.GetObject("btnHelp2.Image")));
			this.btnHelp2.Location = new System.Drawing.Point(283, 168);
			this.btnHelp2.Name = "btnHelp2";
			this.btnHelp2.Size = new System.Drawing.Size(26, 26);
			this.btnHelp2.TabIndex = 11;
			this.btnHelp2.UseVisualStyleBackColor = true;
			this.btnHelp2.Click += new System.EventHandler(this.BtnHelp2Click);
			// 
			// ItemDataForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(314, 199);
			this.Controls.Add(this.btnHelp2);
			this.Controls.Add(this.btnHelp);
			this.Controls.Add(this.boxColor);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.panelColor);
			this.Controls.Add(this.boxLore);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.boxPlayer);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.boxName);
			this.Controls.Add(this.label2);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ItemDataForm";
			this.ShowInTaskbar = false;
			this.Text = "Item Editor";
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.Button btnHelp2;
		private System.Windows.Forms.Button btnHelp;
		private System.Windows.Forms.Panel panelColor;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.ColorDialog colorDialog;
		private System.Windows.Forms.TextBox boxColor;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox boxPlayer;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox boxName;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox boxLore;
	}
}
