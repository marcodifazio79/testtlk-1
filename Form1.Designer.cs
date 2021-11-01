
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
            this.SuspendLayout();
            // 
            // btnDelSingleMachine
            // 
            this.btnDelSingleMachine.Location = new System.Drawing.Point(149, 22);
            this.btnDelSingleMachine.Name = "btnDelSingleMachine";
            this.btnDelSingleMachine.Size = new System.Drawing.Size(165, 23);
            this.btnDelSingleMachine.TabIndex = 0;
            this.btnDelSingleMachine.Text = "Delete Single Machine";
            this.btnDelSingleMachine.UseVisualStyleBackColor = true;
            this.btnDelSingleMachine.Click += new System.EventHandler(this.btnDelSingleMachine_Click);
            // 
            // txtIdtoDelete
            // 
            this.txtIdtoDelete.Location = new System.Drawing.Point(31, 25);
            this.txtIdtoDelete.Name = "txtIdtoDelete";
            this.txtIdtoDelete.Size = new System.Drawing.Size(100, 20);
            this.txtIdtoDelete.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(38, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "ID Machine";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
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
    }
}

