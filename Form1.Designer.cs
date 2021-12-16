
namespace TestCasseTLK
{
    partial class Form1
    {
        /// <summary>
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Pulire le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione Windows Form

        /// <summary>
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnDelSingleMachine = new System.Windows.Forms.Button();
            this.txtIdtoDelete = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtIdtoUpdate = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnDelSingleMachine
            // 
            this.btnDelSingleMachine.Location = new System.Drawing.Point(318, 23);
            this.btnDelSingleMachine.Name = "btnDelSingleMachine";
            this.btnDelSingleMachine.Size = new System.Drawing.Size(165, 23);
            this.btnDelSingleMachine.TabIndex = 0;
            this.btnDelSingleMachine.Text = "Delete Single Machine";
            this.btnDelSingleMachine.UseVisualStyleBackColor = true;
            this.btnDelSingleMachine.Click += new System.EventHandler(this.btnDelSingleMachine_Click);
            // 
            // txtIdtoDelete
            // 
            this.txtIdtoDelete.Location = new System.Drawing.Point(31, 26);
            this.txtIdtoDelete.Name = "txtIdtoDelete";
            this.txtIdtoDelete.Size = new System.Drawing.Size(100, 20);
            this.txtIdtoDelete.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(38, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "ID To Delete ";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(189, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "ID To Udate";
            // 
            // txtIdtoUpdate
            // 
            this.txtIdtoUpdate.Location = new System.Drawing.Point(171, 26);
            this.txtIdtoUpdate.Name = "txtIdtoUpdate";
            this.txtIdtoUpdate.Size = new System.Drawing.Size(100, 20);
            this.txtIdtoUpdate.TabIndex = 4;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.txtIdtoUpdate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtIdtoDelete);
            this.Controls.Add(this.btnDelSingleMachine);
            this.Name = "Form1";
            this.Text = "Tool listener_DB ";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnDelSingleMachine;
        private System.Windows.Forms.TextBox txtIdtoDelete;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtIdtoUpdate;
    }
}

