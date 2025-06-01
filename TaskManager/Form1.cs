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
                _logger?.LogInformation("Загрузка формы началась");

                comboBoxStatus.SelectedIndex = 0;
                comboBoxPriority.SelectedIndex = 1;
                dataGridViewTasks.ReadOnly = true;
                dataGridViewTasks.AllowUserToAddRows = false;
                dataGridViewTasks.AllowUserToDeleteRows = false;
                dataGridViewTasks.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridViewTasks.MultiSelect = false;
                dataGridViewTasks.EditMode = DataGridViewEditMode.EditProgrammatically;
                dataGridViewTasks.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Title", HeaderText = "Название", Width = 150 });
                dataGridViewTasks.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Description", HeaderText = "Описание", Width = 200 });
                dataGridViewTasks.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Deadline", HeaderText = "Дедлайн", Width = 120 });
                dataGridViewTasks.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Status", HeaderText = "Статус", Width = 100 });
                dataGridViewTasks.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Priority", HeaderText = "Приоритет", Width = 100 });

                await LoadTasks();

                _logger?.LogInformation("Форма успешно загружена");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Ошибка при загрузке формы");
                MessageBox.Show($"Ошибка загрузки задач: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                _logger?.LogInformation("Задачи загружены");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Ошибка загрузки задач");
                MessageBox.Show($"Ошибка загрузки задач: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    MessageBox.Show("Название не может быть пустым.", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrEmpty(status) || string.IsNullOrEmpty(priority))
                {
                    MessageBox.Show("Выберите статус и приоритет.", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                _logger?.LogInformation("Добавлена новая задача");

                ClearInputFields();
                await LoadTasks();

                MessageBox.Show("Задача успешно добавлена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Ошибка при добавлении задачи");
                MessageBox.Show($"Ошибка при добавлении задачи: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void buttonEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewTasks.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Выберите задачу для редактирования.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    MessageBox.Show("Название не может быть пустым.", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrEmpty(status) || string.IsNullOrEmpty(priority))
                {
                    MessageBox.Show("Выберите статус и приоритет.", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var task = await _taskService.GetTaskByIdAsync(taskId);
                task.Title = title;
                task.Description = description;
                task.Deadline = deadline;
                task.Status = status switch
                {
                    "К выполнению" => "To Do",
                    "В процессе" => "In Progress",
                    "Готово" => "Done",
                    _ => "To Do"
                };
                task.Priority = priority switch
                {
                    "Низкий" => "Low",
                    "Средний" => "Medium",
                    "Высокий" => "High",
                    _ => "Medium"
                };

                await _taskService.UpdateTaskAsync(task);
                _logger?.LogInformation("Задача обновлена");

                ClearInputFields();
                await LoadTasks();

                MessageBox.Show("Задача успешно обновлена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Ошибка при редактировании задачи");
                MessageBox.Show($"Ошибка при редактировании задачи: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void buttonDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewTasks.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Выберите задачу для удаления.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var confirm = MessageBox.Show("Вы действительно хотите удалить выбранную задачу?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm == DialogResult.Yes)
                {
                    var selectedRow = dataGridViewTasks.SelectedRows[0];
                    var taskId = (int)selectedRow.Cells["Id"].Value;

                    await _taskService.DeleteTaskAsync(taskId);
                    _logger?.LogInformation("Задача удалена");

                    ClearInputFields();
                    await LoadTasks();

                    MessageBox.Show("Задача успешно удалена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Ошибка при удалении задачи");
                MessageBox.Show($"Ошибка при удалении задачи: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearInputFields()
        {
            textBoxTitle.Clear();
            textBoxDescription.Clear();
            dateTimePickerDeadline.Value = DateTime.Now;
            comboBoxStatus.SelectedIndex = 0; 
            comboBoxPriority.SelectedIndex = 1;

            _logger?.LogInformation("Поля ввода очищены");
        }
    }
}
