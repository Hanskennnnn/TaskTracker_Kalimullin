namespace TaskManager
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DataGridView dataGridViewTasks;
        private System.Windows.Forms.TextBox textBoxTitle;
        private System.Windows.Forms.TextBox textBoxDescription;
        private System.Windows.Forms.DateTimePicker dateTimePickerDeadline;
        private System.Windows.Forms.ComboBox comboBoxStatus;
        private System.Windows.Forms.ComboBox comboBoxPriority;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonEdit;
        private System.Windows.Forms.Button buttonDelete;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            dataGridViewTasks = new DataGridView();
            textBoxTitle = new TextBox();
            textBoxDescription = new TextBox();
            dateTimePickerDeadline = new DateTimePicker();
            comboBoxStatus = new ComboBox();
            comboBoxPriority = new ComboBox();
            buttonAdd = new Button();
            buttonEdit = new Button();
            buttonDelete = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridViewTasks).BeginInit();
            SuspendLayout();
            // 
            // dataGridViewTasks
            // 
            dataGridViewTasks.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewTasks.Location = new Point(12, 12);
            dataGridViewTasks.MultiSelect = false;
            dataGridViewTasks.Name = "dataGridViewTasks";
            dataGridViewTasks.RowHeadersWidth = 51;
            dataGridViewTasks.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewTasks.Size = new Size(723, 533);
            dataGridViewTasks.TabIndex = 0;
            // 
            // textBoxTitle
            // 
            textBoxTitle.Location = new Point(776, 12);
            textBoxTitle.Name = "textBoxTitle";
            textBoxTitle.PlaceholderText = "Название";
            textBoxTitle.Size = new Size(262, 27);
            textBoxTitle.TabIndex = 1;
            // 
            // textBoxDescription
            // 
            textBoxDescription.Location = new Point(776, 45);
            textBoxDescription.Multiline = true;
            textBoxDescription.Name = "textBoxDescription";
            textBoxDescription.PlaceholderText = "Описание";
            textBoxDescription.Size = new Size(262, 110);
            textBoxDescription.TabIndex = 2;
            // 
            // dateTimePickerDeadline
            // 
            dateTimePickerDeadline.Location = new Point(776, 161);
            dateTimePickerDeadline.Name = "dateTimePickerDeadline";
            dateTimePickerDeadline.Size = new Size(262, 27);
            dateTimePickerDeadline.TabIndex = 3;
            // 
            // comboBoxStatus
            // 
            comboBoxStatus.Items.AddRange(new object[] { "В ожидании", "В процессе", "Завершено" });
            comboBoxStatus.Location = new Point(776, 194);
            comboBoxStatus.Name = "comboBoxStatus";
            comboBoxStatus.Size = new Size(262, 28);
            comboBoxStatus.TabIndex = 4;
            // 
            // comboBoxPriority
            // 
            comboBoxPriority.Items.AddRange(new object[] { "Низкий", "Средний", "Высокий" });
            comboBoxPriority.Location = new Point(776, 228);
            comboBoxPriority.Name = "comboBoxPriority";
            comboBoxPriority.Size = new Size(262, 28);
            comboBoxPriority.TabIndex = 5;
            // 
            // buttonAdd
            // 
            buttonAdd.Location = new Point(776, 389);
            buttonAdd.Name = "buttonAdd";
            buttonAdd.Size = new Size(262, 48);
            buttonAdd.TabIndex = 6;
            buttonAdd.Text = "Добавить";
            buttonAdd.Click += buttonAdd_Click;
            // 
            // buttonEdit
            // 
            buttonEdit.Location = new Point(776, 443);
            buttonEdit.Name = "buttonEdit";
            buttonEdit.Size = new Size(262, 48);
            buttonEdit.TabIndex = 7;
            buttonEdit.Text = "Редактировать";
            buttonEdit.Click += buttonEdit_Click;
            // 
            // buttonDelete
            // 
            buttonDelete.Location = new Point(776, 497);
            buttonDelete.Name = "buttonDelete";
            buttonDelete.Size = new Size(262, 48);
            buttonDelete.TabIndex = 8;
            buttonDelete.Text = "Удалить";
            buttonDelete.Click += buttonDelete_Click;
            // 
            // Form1
            // 
            ClientSize = new Size(1050, 557);
            Controls.Add(dataGridViewTasks);
            Controls.Add(textBoxTitle);
            Controls.Add(textBoxDescription);
            Controls.Add(dateTimePickerDeadline);
            Controls.Add(comboBoxStatus);
            Controls.Add(comboBoxPriority);
            Controls.Add(buttonAdd);
            Controls.Add(buttonEdit);
            Controls.Add(buttonDelete);
            Name = "Form1";
            Text = "TaskTracker";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridViewTasks).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
        #endregion
    }
}
