using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SCSM.Business;
using SCSM.Data;
using SCSM.Data.Respositories;



namespace SCMS
{
    public partial class Form1 : Form
    {

        SCSM.Business.DatabaseCalls DatabaseCall = new SCSM.Business.DatabaseCalls();

        public Form1()
        {
            InitializeComponent();
            LoadTable();
        }

        //the following button is pressed starts the initial event
        private void deleteStockButton_Click(object sender, EventArgs e)
        {
            //item code is the current selected cell on the table
            var itemCode = stockDataGrid.SelectedCells[0].Value.ToString();

            //create an instance of the mediator
            var mediator = new Mediator();

            //inititalise the intial event
            var startEvent = new DeleteStockEvent(mediator, "startEvent");
            //inititialise the processors sending the mediator the message and the object in the constructor
            var process1 = new CheckRevenueGenerateProcessor(mediator, "Process 1", itemCode);
            var process2 = new CancelCurrentOrdersProcessor(mediator, "Process 2", itemCode);
            var process3 = new ConfirmationProcessor(mediator, "Process 3", itemCode);
            var process4 = new StockItemProcessor(mediator, "Process 4", itemCode);
            var process5 = new NotificationProcessor(mediator, "Process 5", itemCode);

            //start the initial event passing a message which should tell processor one to start via the mediator send method
            startEvent.Send("delete");
          
            //Refresh the table
            ClearTable();
            LoadTable();
        }

        private void addNewStockButton_Click(object sender, EventArgs e)
        {
            //create an instance of the AddStockItem object
            var itemStock = new AddStockItem();

            //map the different values of the AddStockItem object via the form values the user inputs
            itemStock.itemCode = itemCodeTextBox.Text;
            itemStock.itemName = itemNameTextBox.Text;
            itemStock.itemPrice = itemPriceTextBox.Text;
            itemStock.itemQuantity = Convert.ToInt32(itemQuantityTextBox.Value);
            itemStock.stockArrivalDate = stockArrivalDateTextBox.Value;
            itemStock.minRequired = Convert.ToInt32(minimumRequiredTextBox.Value);
            itemStock.maxRequired = Convert.ToInt32(maximumRequiredTextBox.Value);

            //create an instance of the mediator
            var mediator = new Mediator();

            //inititalise the intial event
            var startEvent = new Processors.AddStockEvent(mediator,"startEvent");

            //initialise all the mediator steps
            var process1 = new Processors.ValidationProcessor(mediator, "Process 1", itemStock);
            var process2 = new Processors.ItemCheckProcessor(mediator, "Process 2", itemStock);
            var process3 = new Processors.BudgetProcessor(mediator, "Process 3", itemStock);
            var process4 = new StockItemProcessor(mediator, "Process 4", itemStock);
            var process5 = new NotificationProcessor(mediator, "Process 5", itemStock.itemCode);

            //start the initial event passing a message which should tell processor one to start via the mediator
            startEvent.Send("add");

            //Refresh the table
            ClearTable();
            LoadTable();

            //Go the switch to the main screen
            SwitchToMain();

            //Reset the forms
            ResetFields();
        }


        //In button click part of the initial event
        private void orderStockButton_Click(object sender, EventArgs e)
        {
            var itemCode = stockDataGrid.SelectedCells[0].Value.ToString();

            var itemOrder = new OrderItem();
            itemOrder.itemCode = itemCode;
            itemOrder.itemQuantity = Convert.ToInt32(orderStockQuanityTextBox.Value);
            itemOrder.stockArrivalDate = orderStocksSADTextBox.Value;

            orderStocksLabel.Text = itemCode;

            var mediator = new Mediator();

            //inititalise the intial event
            var startEvent = new OrderStockEvent(mediator, "startEvent");

            //initialise all the mediator steps
            var process1 = new CheckMinAndMaxProcessor(mediator, "Process 1", itemOrder);
            var process2 = new CheckRevenueGenerateProcessor(mediator, "Process 2", itemCode);
            var process3 = new Processors.CalculateCostProcessor(mediator, "Process 3", itemOrder);
            var process4 = new StockItemProcessor(mediator, "Process 4", itemOrder);
            var process5 = new NotificationProcessor(mediator, "Process 5", itemCode);

            //start the initial event passing a message which should tell processor one to start via the mediator send method
            startEvent.Send("order");

            //refresh the table 
            ClearTable();
            LoadTable();
            
        }
  
        ////////////////Helper functions///////////////////////////////////

        private void ClearTable()
        {
            stockDataGrid.ColumnCount = 7;
            stockDataGrid.Columns[0].Name = "Item Code";
            stockDataGrid.Columns[1].Name = "Item Name";
            stockDataGrid.Columns[2].Name = "Price";
            stockDataGrid.Columns[3].Name = "Item Quantity";
            stockDataGrid.Columns[4].Name = "Stock Arrival Date";
            stockDataGrid.Columns[5].Name = "Minimum required";
            stockDataGrid.Columns[6].Name = "Maximum required";

            stockDataGrid.Rows.Clear();
            stockDataGrid.Refresh();
        }

        private void LoadTable()
        {

            stockDataGrid.ColumnCount = 7;
            stockDataGrid.Columns[0].Name = "Item Code";
            stockDataGrid.Columns[1].Name = "Item Name";
            stockDataGrid.Columns[2].Name = "Price";
            stockDataGrid.Columns[3].Name = "Item Quantity";
            stockDataGrid.Columns[4].Name = "Stock Arrival Date";
            stockDataGrid.Columns[5].Name = "Minimum required";
            stockDataGrid.Columns[6].Name = "Maximum required";

            var row = DatabaseCall.GetData();

            for (int i = 0; i < row.itemCode.Count; i++)
            {
                stockDataGrid.Rows.Add(row.itemCode[i], row.itemName[i], row.price[i], row.itemQuantity[i], row.stockArrivalDate[i].Date, row.minRequired[i], row.maxRequired[i]);
            }

            stockDataGrid.Columns[1].Selected = false;

        }


        private void orderStockPanelButton_Click(object sender, EventArgs e)
        {
            orderStocksLabel.Text = stockDataGrid.SelectedCells[0].Value.ToString(); ;
            SwitchToOrderStocks();
        }

        private void addNewStockPanelButton_Click(object sender, EventArgs e)
        {
            SwitchToAddNewStocks();
        }

        private void SwitchToMain()
        {
            orderStocksPanel.Visible = false;
            addNewItemPanel.Visible = false;
            mainPanel.Visible = true;
        }

        private void SwitchToOrderStocks()
        {
            orderStocksPanel.Visible = true;
            addNewItemPanel.Visible = false;
            mainPanel.Visible = false;
        }

        private void SwitchToAddNewStocks()
        {
            orderStocksPanel.Visible = false;
            addNewItemPanel.Visible = true;
            mainPanel.Visible = false;
        }

        private void goToMainButton_Click(object sender, EventArgs e)
        {
            SwitchToMain();
        }

        private void goToMain2_Click(object sender, EventArgs e)
        {
            SwitchToMain();
        }

        private void ResetFields()
        {
            itemCodeTextBox.Text = "";
            itemNameTextBox.Text = "";
            itemPriceTextBox.Text = "";
            itemQuantityTextBox.Value = 0;
            stockArrivalDateTextBox.Value = DateTime.Now;
            minimumRequiredTextBox.Value = 0;
            maximumRequiredTextBox.Value = 0;
        }
    }
}
