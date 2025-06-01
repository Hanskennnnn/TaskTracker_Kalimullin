using Library.Interfaces;
using Library.Models;
using Microsoft.Extensions.Logging;

namespace TaskManager
{
    public partial class Form1 : Form
    {
        private readonly ITaskService _taskService;
        private readonly ILogger<Form1> _logger;

        public Form1(ITaskService taskService, ILogger<Form1> logger)
        {
            _taskService = taskService;
            _logger = logger;
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                _logger?.LogInformation("�������� ����� ��������");

                comboBoxStatus.SelectedIndex = 0;
                comboBoxPriority.SelectedIndex = 1;
                dataGridViewTasks.ReadOnly = true;
                dataGridViewTasks.AllowUserToAddRows = false;
                dataGridViewTasks.AllowUserToDeleteRows = false;
                dataGridViewTasks.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridViewTasks.MultiSelect = false;
                dataGridViewTasks.EditMode = DataGridViewEditMode.EditProgrammatically;
                dataGridViewTasks.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Title", HeaderText = "��������", Width = 150 });
                dataGridViewTasks.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Description", HeaderText = "��������", Width = 200 });
                dataGridViewTasks.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Deadline", HeaderText = "�������", Width = 120 });
                dataGridViewTasks.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Status", HeaderText = "������", Width = 100 });
                dataGridViewTasks.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Priority", HeaderText = "���������", Width = 100 });

                await LoadTasks();

                _logger?.LogInformation("����� ������� ���������");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "������ ��� �������� �����");
                MessageBox.Show($"������ �������� �����: {ex.Message}", "������", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadTasks()
        {
            try
            {
                var tasks = await _taskService.GetAllTasksAsync();

                var tasksForGrid = tasks.Select(t => new
                {
                    t.Id,
                    t.Title,
                    t.Description,
                    t.Deadline,
                    t.Status,
                    t.Priority
                }).ToList();

                dataGridViewTasks.DataSource = tasksForGrid;
                if (dataGridViewTasks.Columns.Contains("Id"))
                    dataGridViewTasks.Columns["Id"].Visible = false;

                _logger?.LogInformation("������ ���������");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "������ �������� �����");
                MessageBox.Show($"������ �������� �����: {ex.Message}", "������", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private async void buttonAdd_Click(object sender, EventArgs e)
        {
            try
            {
                var title = textBoxTitle.Text.Trim();
                var description = textBoxDescription.Text.Trim();
                var deadline = dateTimePickerDeadline.Value;
                var status = comboBoxStatus.SelectedItem?.ToString();
                var priority = comboBoxPriority.SelectedItem?.ToString();

                if (string.IsNullOrEmpty(title))
                {
                    MessageBox.Show("�������� �� ����� ���� ������.", "������ �����", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrEmpty(status) || string.IsNullOrEmpty(priority))
                {
                    MessageBox.Show("�������� ������ � ���������.", "������ �����", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var newTask = new TaskItem
                {
                    Title = title,
                    Description = description,
                    Deadline = deadline,
                    Status = status,
                    Priority = priority
                };

                await _taskService.AddTaskAsync(newTask);
                _logger?.LogInformation("��������� ����� ������");

                ClearInputFields();
                await LoadTasks();

                MessageBox.Show("������ ������� ���������!", "�����", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "������ ��� ���������� ������");
                MessageBox.Show($"������ ��� ���������� ������: {ex.Message}", "������", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void buttonEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewTasks.SelectedRows.Count == 0)
                {
                    MessageBox.Show("�������� ������ ��� ��������������.", "������", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var selectedRow = dataGridViewTasks.SelectedRows[0];
                var taskId = (int)selectedRow.Cells["Id"].Value;

                var title = textBoxTitle.Text.Trim();
                var description = textBoxDescription.Text.Trim();
                var deadline = dateTimePickerDeadline.Value;
                var status = comboBoxStatus.SelectedItem?.ToString();
                var priority = comboBoxPriority.SelectedItem?.ToString();

                if (string.IsNullOrEmpty(title))
                {
                    MessageBox.Show("�������� �� ����� ���� ������.", "������ �����", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrEmpty(status) || string.IsNullOrEmpty(priority))
                {
                    MessageBox.Show("�������� ������ � ���������.", "������ �����", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var task = await _taskService.GetTaskByIdAsync(taskId);
                task.Title = title;
                task.Description = description;
                task.Deadline = deadline;
                task.Status = status switch
                {
                    "� ����������" => "To Do",
                    "� ��������" => "In Progress",
                    "������" => "Done",
                    _ => "To Do"
                };
                task.Priority = priority switch
                {
                    "������" => "Low",
                    "�������" => "Medium",
                    "�������" => "High",
                    _ => "Medium"
                };

                await _taskService.UpdateTaskAsync(task);
                _logger?.LogInformation("������ ���������");

                ClearInputFields();
                await LoadTasks();

                MessageBox.Show("������ ������� ���������!", "�����", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "������ ��� �������������� ������");
                MessageBox.Show($"������ ��� �������������� ������: {ex.Message}", "������", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void buttonDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewTasks.SelectedRows.Count == 0)
                {
                    MessageBox.Show("�������� ������ ��� ��������.", "������", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var confirm = MessageBox.Show("�� ������������� ������ ������� ��������� ������?", "�������������", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm == DialogResult.Yes)
                {
                    var selectedRow = dataGridViewTasks.SelectedRows[0];
                    var taskId = (int)selectedRow.Cells["Id"].Value;

                    await _taskService.DeleteTaskAsync(taskId);
                    _logger?.LogInformation("������ �������");

                    ClearInputFields();
                    await LoadTasks();

                    MessageBox.Show("������ ������� �������!", "�����", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "������ ��� �������� ������");
                MessageBox.Show($"������ ��� �������� ������: {ex.Message}", "������", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearInputFields()
        {
            textBoxTitle.Clear();
            textBoxDescription.Clear();
            dateTimePickerDeadline.Value = DateTime.Now;
            comboBoxStatus.SelectedIndex = 0; 
            comboBoxPriority.SelectedIndex = 1;

            _logger?.LogInformation("���� ����� �������");
        }
    }
}
