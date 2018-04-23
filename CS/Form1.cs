using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Configuration;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using DevExpress.Xpo.Metadata.Helpers;
using DevExpress.Xpo.DB;

namespace ObjectProperties {
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class Form1 : System.Windows.Forms.Form {
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ListBox listBox1;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public Form1() {
            InitializeComponent();

        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing) {
            if(disposing) {
                if(components != null) {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.Location = new System.Drawing.Point(24, 8);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(272, 24);
            this.comboBox1.TabIndex = 0;
            this.comboBox1.SelectedValueChanged += new System.EventHandler(this.comboBox1_SelectedValueChanged);
            // 
            // listBox1
            // 
            this.listBox1.ItemHeight = 16;
            this.listBox1.Location = new System.Drawing.Point(24, 64);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(272, 260);
            this.listBox1.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.ClientSize = new System.Drawing.Size(312, 341);
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.listBox1,
																		  this.comboBox1});
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }
        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            string conn = MSSqlConnectionProvider.GetConnectionString("(local)", "Northwind");
            XpoDefault.DataLayer = XpoDefault.GetDataLayer(conn, AutoCreateOption.SchemaAlreadyExists);
            XpoDefault.Session.Dictionary.CollectClassInfos(typeof(Customer).Assembly);
            Application.Run(new Form1());
        }

        private void Form1_Load(object sender, System.EventArgs e) {
            foreach(XPClassInfo info in XpoDefault.Session.Dictionary.Classes) {
                if(info.IsPersistent && info.IsVisibleInDesignTime)
                    comboBox1.Items.Add(info);
            }
        }

        string[] GetObjectProperties(XPClassInfo classInfo) {
            if(classInfo != null)
                return GetObjectProperties(classInfo, new ArrayList());
            return new string[] { };
        }

        string[] GetObjectProperties(XPClassInfo xpoInfo, ArrayList processed) {
            if(processed.Contains(xpoInfo)) return new string[] { };
            processed.Add(xpoInfo);
            ArrayList result = new ArrayList();
            foreach(XPMemberInfo m in xpoInfo.PersistentProperties)
                if(!(m is ServiceField) && m.IsPersistent) {
                    result.Add(m.Name);
                    if(m.ReferenceType != null) {
                        string[] childProps = GetObjectProperties(m.ReferenceType, processed);
                        foreach(string child in childProps)
                            result.Add(string.Format("{0}.{1}", m.Name, child));
                    }
                }

            foreach(XPMemberInfo m in xpoInfo.CollectionProperties) {
                string[] childProps = GetObjectProperties(m.CollectionElementType, processed);
                foreach(string child in childProps)
                    result.Add(string.Format("{0}.{1}", m.Name, child));
            }
            return result.ToArray(typeof(string)) as string[];
        }

        private void comboBox1_SelectedValueChanged(object sender, System.EventArgs e) {
            listBox1.Items.Clear();
            listBox1.Items.AddRange(GetObjectProperties((XPClassInfo)comboBox1.SelectedItem));
        }
    }
}
