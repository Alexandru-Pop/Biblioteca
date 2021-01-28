using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.Entity;
using AutoLotModel;
using System.Data;

namespace Biblioteca
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    enum ActionState
    {
        New,
        Edit,
        Delete,
        Nothing
    }


    public partial class MainWindow : Window
    {
        ActionState action = ActionState.Nothing;

        AutoLotEntitiesModel ctx = new AutoLotEntitiesModel();

        CollectionViewSource customerViewSource;
        CollectionViewSource inventoryViewSource;

        CollectionViewSource customerOrdersViewSource;

        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            inventoryViewSource = (System.Windows.Data.CollectionViewSource)(this.FindResource("inventoryViewSource"));
            inventoryViewSource.Source = ctx.Inventories.Local;

            customerViewSource = (CollectionViewSource)FindResource("customerViewSource");
            customerViewSource.Source = ctx.Customers.Local;


            customerOrdersViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("customerOrdersViewSource")));

            ctx.Customers.Load();
            ctx.Orders.Load();
            ctx.Inventories.Load();

            cmbCustomers.ItemsSource = ctx.Customers.Local;
            cmbCustomers.SelectedValuePath = "CustId";

            cmbInventory.ItemsSource = ctx.Inventories.Local;
            cmbInventory.SelectedValuePath = "BookId";

            BindDataGrid();
        }

        private void BindDataGrid()
        {
            var queryOrder = from ord in ctx.Orders
                             join cust in ctx.Customers on ord.CustId equals
                             cust.CustId
                             join inv in ctx.Inventories on ord.BookId
                 equals inv.BookId
                             select new
                             {
                                 ord.OrderId,
                                 ord.BookId,
                                 ord.CustId,
                                 cust.FirstName,
                                 cust.LastName,
                                 inv.BookTitle,
                                 inv.BookFormat
                             };
            customerOrdersViewSource.Source = queryOrder.ToList();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Edit;

            BindingOperations.ClearBinding(firstNameTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(lastNameTextBox, TextBox.TextProperty);
            SetValidationBinding();
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.New;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Delete;
        }

        private void btnEdit1_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Edit;
        }

        private void btnNew1_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.New;
        }

        private void btnDelete1_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Delete;
        }


        // Codul pentru Customers

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Se salveaza modificarea solicitata. Va rugam asteptati!");
            Customer customer = null;
            if (action == ActionState.New)
            {
                MessageBox.Show("Inregistrarea a fost salvata cu succes!");
                try
                {
                    //Instantiem Customer Entity
                    customer = new Customer()
                    {
                        CustId = int.Parse(custIdTextBox.Text.Trim()),
                        FirstName = firstNameTextBox.Text.Trim(),
                        LastName = lastNameTextBox.Text.Trim(),
                        Age = ageTextBox.Text.Trim(),
                        University = universityTextBox.Text.Trim()
                    };

                    //Adaugam Entitatea nou creata in context
                    ctx.Customers.Add(customer);
                    customerViewSource.View.Refresh();

                    //Salvam modificarile
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else if (action == ActionState.Edit)
            {
                MessageBox.Show("Se editeaza inregistrarea.");
                try
                {
                    customer = (Customer)customerDataGrid.SelectedItem;
                    customer.FirstName = firstNameTextBox.Text.Trim();
                    customer.LastName = lastNameTextBox.Text.Trim();
                    customer.Age = ageTextBox.Text.Trim();
                    customer.University = universityTextBox.Text.Trim();

                    //Salvam modificarile
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }

                customerViewSource.View.Refresh();

                // Pozitionam pe item-ul curent
                customerViewSource.View.MoveCurrentTo(customer);
            }
            else if (action == ActionState.Delete)
            {
                MessageBox.Show("Inregistrarea a fost stearsa cu succes!");
                try
                {
                    customer = (Customer)customerDataGrid.SelectedItem;
                    ctx.Customers.Remove(customer);
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                customerViewSource.View.Refresh();
            }

            action = ActionState.Nothing;
        }


        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            customerViewSource.View.MoveCurrentToNext();
        }

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            customerViewSource.View.MoveCurrentToPrevious();
        }



        // Codul pentru Inventory


        private void btnSave1_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Se salveaza modificarea solicitata. Va rugam asteptati!");
            Inventory inventory = null;
            if (action == ActionState.New)
            {
                MessageBox.Show("Inregistrarea a fost salvata cu succes!");
                try
                {
                    //Instantiem Inventory Entity
                    inventory = new Inventory()
                    {
                        BookId = int.Parse(bookIdTextBox.Text.Trim()),
                        BookTitle = booktitleTextBox.Text.Trim(),
                        BookFormat = bookformatTextBox.Text.Trim(),
                        Author = authorTextBox.Text.Trim(),
                        PublishingYear = publishingyearTextBox.Text.Trim()
                    };

                    //Adaugam Entitatea nou creata in context
                    ctx.Inventories.Add(inventory);
                    inventoryViewSource.View.Refresh();

                    //Salvam modificarile
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else if (action == ActionState.Edit)
            {
                MessageBox.Show("Se editeaza inregistrarea.");
                try
                {
                    inventory = (Inventory)inventoryDataGrid.SelectedItem;
                    inventory.BookId = int.Parse(bookIdTextBox.Text.Trim());
                    inventory.BookTitle = booktitleTextBox.Text.Trim();
                    inventory.BookFormat = bookformatTextBox.Text.Trim();
                    inventory.Author = authorTextBox.Text.Trim();
                    inventory.PublishingYear = publishingyearTextBox.Text.Trim();

                    //Salvam modificarile
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }

                inventoryViewSource.View.Refresh();

                inventoryViewSource.View.MoveCurrentTo(inventory);
                // Pozitionam pe item-ul curent
            }
            else if (action == ActionState.Delete)
            {
                MessageBox.Show("Inregistrarea a fost stearsa cu succes!");
                try
                {
                    inventory = (Inventory)inventoryDataGrid.SelectedItem;
                    ctx.Inventories.Remove(inventory);
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                inventoryViewSource.View.Refresh();
            }

            action = ActionState.Nothing;
        }

        private void btnNext1_Click(object sender, RoutedEventArgs e)
        {
            inventoryViewSource.View.MoveCurrentToNext();
        }

        private void btnPrevious1_Click(object sender, RoutedEventArgs e)
        {
            inventoryViewSource.View.MoveCurrentToPrevious();
        }


        // Codul pentru Orders



        private void btnSave2_Click(object sender, RoutedEventArgs e)
        {
            Order order = null;
            if (action == ActionState.New)
            {
                MessageBox.Show("Comanda a fost inregistrata cu succes. Pentru a o vedea va rugam reporniti programul!");
                try
                {
                    Customer customer = (Customer)cmbCustomers.SelectedItem;
                    Inventory inventory = (Inventory)cmbInventory.SelectedItem;
                    //Instantiem Order Entity
                    order = new Order()
                    {

                        CustId = customer.CustId,
                        BookId = inventory.BookId
                    };
                    //Adaugam Entitatea nou creata in context
                    ctx.Orders.Add(order);
                    customerOrdersViewSource.View.Refresh();
                    //Salvam modificarile
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
            else if (action == ActionState.Edit)
            {
                dynamic selectedOrder = ordersDataGrid.SelectedItem;
                try
                {
                    int curr_id = selectedOrder.OrderId;
                    var editedOrder = ctx.Orders.FirstOrDefault(s => s.OrderId == curr_id);
                    if (editedOrder != null)
                    {
                        editedOrder.CustId = Int32.Parse(cmbCustomers.SelectedValue.ToString());
                        editedOrder.BookId = Convert.ToInt32(cmbInventory.SelectedValue.ToString());
                        //Salvam modificarile
                        ctx.SaveChanges();
                    }
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                BindDataGrid();
                // Pozitionam pe item-ul curent
                customerViewSource.View.MoveCurrentTo(selectedOrder);
            }
            else if (action == ActionState.Delete)
            {
                try
                {
                    dynamic selectedOrder = ordersDataGrid.SelectedItem;
                    int curr_id = selectedOrder.OrderId;
                    var deletedOrder = ctx.Orders.FirstOrDefault(s => s.OrderId == curr_id);
                    if (deletedOrder != null)
                    {
                        ctx.Orders.Remove(deletedOrder);
                        ctx.SaveChanges();
                        MessageBox.Show("Comanda a fost scoasa din inregistrari cu succes!");
                        BindDataGrid();
                    }
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }



        }

        private void btnNext2_Click(object sender, RoutedEventArgs e)
        {
            customerOrdersViewSource.View.MoveCurrentToNext();
        }

        private void btnPrevious2_Click(object sender, RoutedEventArgs e)
        {
            customerOrdersViewSource.View.MoveCurrentToPrevious();
        }

        //In cele ce urmeaza se regaseste codul de validare a datelor de intrare

        private void SetValidationBinding()
        {
            //Vom incepe cu validarea campurilor Age si BookFormat (string-urile introduse nu pot fi nule)

            //Validarea campului Age

            Binding ageValidationBinding = new Binding();
            ageValidationBinding.Source = customerViewSource;
            ageValidationBinding.Path = new PropertyPath("Age");
            ageValidationBinding.NotifyOnValidationError = true;
            ageValidationBinding.Mode = BindingMode.TwoWay;
            ageValidationBinding.UpdateSourceTrigger =
           UpdateSourceTrigger.PropertyChanged;
            //Este necesar string-ul sa nu fie nul
            ageValidationBinding.ValidationRules.Add(new StringNotEmpty());
            ageTextBox.SetBinding(TextBox.TextProperty,
           ageValidationBinding);

            //Validarea campului BookFormat

            Binding bookFormatValidationBinding = new Binding();
            bookFormatValidationBinding.Source = inventoryViewSource;
            bookFormatValidationBinding.Path = new PropertyPath("BookFormat");
            bookFormatValidationBinding.NotifyOnValidationError = true;
            bookFormatValidationBinding.Mode = BindingMode.TwoWay;
            bookFormatValidationBinding.UpdateSourceTrigger =
           UpdateSourceTrigger.PropertyChanged;
            //Este necesar string-ul sa nu fie nul
            bookFormatValidationBinding.ValidationRules.Add(new StringNotEmpty());
            bookformatTextBox.SetBinding(TextBox.TextProperty,
           bookFormatValidationBinding);



            //Continuam cu validarea campurilor FirstName, LastName, University, BookTitle, Author, Publishing Year (string-urile introduse trebuie sa aiba minim 4 caractere)

            //Validarea campului FirstName

            Binding firstNameValidationBinding = new Binding();
            firstNameValidationBinding.Source = customerViewSource;
            firstNameValidationBinding.Path = new PropertyPath("FirstName");
            firstNameValidationBinding.NotifyOnValidationError = true;
            firstNameValidationBinding.Mode = BindingMode.TwoWay;
            firstNameValidationBinding.UpdateSourceTrigger =
           UpdateSourceTrigger.PropertyChanged;
            //Este necesar ca string-ul sa aiba minim 4 caractere
            firstNameValidationBinding.ValidationRules.Add(new StringMinLengthValidator());
            firstNameTextBox.SetBinding(TextBox.TextProperty,
           firstNameValidationBinding);

            //Validarea campului LastName

            Binding lastNameValidationBinding = new Binding();
            lastNameValidationBinding.Source = customerViewSource;
            lastNameValidationBinding.Path = new PropertyPath("LastName");
            lastNameValidationBinding.NotifyOnValidationError = true;
            lastNameValidationBinding.Mode = BindingMode.TwoWay;
            lastNameValidationBinding.UpdateSourceTrigger =
           UpdateSourceTrigger.PropertyChanged;
            //Este necesar ca string-ul sa aiba minim 4 caractere
            lastNameValidationBinding.ValidationRules.Add(new StringMinLengthValidator());
            lastNameTextBox.SetBinding(TextBox.TextProperty,
           lastNameValidationBinding);

            //Validarea campului University

            Binding universityValidationBinding = new Binding();
            universityValidationBinding.Source = customerViewSource;
            universityValidationBinding.Path = new PropertyPath("University");
            universityValidationBinding.NotifyOnValidationError = true;
            universityValidationBinding.Mode = BindingMode.TwoWay;
            universityValidationBinding.UpdateSourceTrigger =
           UpdateSourceTrigger.PropertyChanged;
            //Este necesar ca string-ul sa aiba minim 4 caractere
            universityValidationBinding.ValidationRules.Add(new StringMinLengthValidator());
            universityTextBox.SetBinding(TextBox.TextProperty,
           universityValidationBinding);

            //Validarea campului BookTitle

            Binding bookTitleValidationBinding = new Binding();
            bookTitleValidationBinding.Source = inventoryViewSource;
            bookTitleValidationBinding.Path = new PropertyPath("BookTitle");
            bookTitleValidationBinding.NotifyOnValidationError = true;
            bookTitleValidationBinding.Mode = BindingMode.TwoWay;
            bookTitleValidationBinding.UpdateSourceTrigger =
           UpdateSourceTrigger.PropertyChanged;
            //Este necesar ca string-ul sa aiba minim 4 caractere
            bookTitleValidationBinding.ValidationRules.Add(new StringMinLengthValidator());
            booktitleTextBox.SetBinding(TextBox.TextProperty,
           bookTitleValidationBinding);

            //Validarea campului Author

            Binding authorValidationBinding = new Binding();
            authorValidationBinding.Source = inventoryViewSource;
            authorValidationBinding.Path = new PropertyPath("Author");
            authorValidationBinding.NotifyOnValidationError = true;
            authorValidationBinding.Mode = BindingMode.TwoWay;
            authorValidationBinding.UpdateSourceTrigger =
           UpdateSourceTrigger.PropertyChanged;
            //Este necesar ca string-ul sa aiba minim 4 caractere
            authorValidationBinding.ValidationRules.Add(new StringMinLengthValidator());
            authorTextBox.SetBinding(TextBox.TextProperty,
           authorValidationBinding);

            //Validarea campului PublishingYear

            Binding publishingYearValidationBinding = new Binding();
            publishingYearValidationBinding.Source = inventoryViewSource;
            publishingYearValidationBinding.Path = new PropertyPath("PublishingYear");
            publishingYearValidationBinding.NotifyOnValidationError = true;
            publishingYearValidationBinding.Mode = BindingMode.TwoWay;
            publishingYearValidationBinding.UpdateSourceTrigger =
           UpdateSourceTrigger.PropertyChanged;
            //Este necesar ca string-ul sa aiba minim 4 caractere
            publishingYearValidationBinding.ValidationRules.Add(new StringMinLengthValidator());
            publishingyearTextBox.SetBinding(TextBox.TextProperty,
           publishingYearValidationBinding);
        }

    }

 }
