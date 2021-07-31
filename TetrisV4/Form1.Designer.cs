
namespace TetrisV4
{
    partial class tetris_main_form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.game_tick_timer = new System.Windows.Forms.Timer(this.components);
            this.draw_refresh_timer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // game_tick_timer
            // 
            this.game_tick_timer.Interval = 1000;
            this.game_tick_timer.Tick += new System.EventHandler(this.game_tick_timer_Tick);
            // 
            // draw_refresh_timer
            // 
            this.draw_refresh_timer.Interval = 60;
            this.draw_refresh_timer.Tick += new System.EventHandler(this.draw_refresh_timer_Tick);
            // 
            // tetris_main_form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Name = "tetris_main_form";
            this.Text = "Tetris";
            this.Load += new System.EventHandler(this.tetris_main_form_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer game_tick_timer;
        private System.Windows.Forms.Timer draw_refresh_timer;
    }
}

