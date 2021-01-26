using System;
using System.IO;
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

namespace ShopAdministration
{
    public class Product
    {
        public string ImageFileName;
        public string ProductTitle;
        public string ProductText;
        public decimal ProductPrice;
    }

    public class Coupon
    {
        public string Code;
        public decimal Discount;
    }



    public partial class MainWindow : Window
    {
        private List<string> cartBoxIndexList = new List<string>();

        private Grid mainGrid;
        private Label mainTitle;

        private List<Product> productList;
        private Grid productsGrid;
        private Label titleLabel, priceLabel;
        private TextBox textBox;
        private Button changeProductButton;


        private Grid rightGrid;
        private Dictionary<string, decimal> couponDictionary;
        private ComboBox couponComboBox;
        private Label couponHeader, couponCodeLabel, couponIntLabel, extraCouponRow, emptyRow;
        private TextBox couponCodeBox, couponIntBox;
        private Button saveCouponButton, newCouponButton;

        private Label changeProductHeader, productImageLabel, productTitleLabel, productDescriptionLabel, productPriceLabel;
        private TextBox productImageBox, productTitleBox, productDescriptionBox, productPriceBox;
        private Button saveProductButton, addProductButton;


        public MainWindow()
        {
            InitializeComponent();
            Start();
        }

        private void Start()
        {
            // Window options
            Title = "Imperial Shop Staff Program";
            Width = 1500;
            Height = 800;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            //Read product file

            productList = ReadProductFile("Products.csv");

            // Main grid
            mainGrid = new Grid();
            Content = mainGrid;
            mainGrid.ColumnDefinitions.Add(new ColumnDefinition());
            mainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.3, GridUnitType.Star) });
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            mainGrid.RowDefinitions.Add(new RowDefinition());

            // ---------- Main Title ----------
            mainTitle = CreateLabel("Imperial Shop Staff Program");
            mainTitle.FontSize = 30;
            mainTitle.FontWeight = FontWeights.Bold;
            mainGrid.Children.Add(mainTitle);
            Grid.SetColumn(mainTitle, 0);

            // ---------- Products Grid ----------
            productsGrid = new Grid();
            mainGrid.Children.Add(productsGrid);
            Grid.SetRow(productsGrid, 2);
            Grid.SetColumn(productsGrid, 0);

            productsGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            productsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.2, GridUnitType.Star) });
            productsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.2, GridUnitType.Star) });
            productsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.2, GridUnitType.Star) });
            productsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.2, GridUnitType.Star) });
            productsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.2, GridUnitType.Star) });

            Label productImage = CreateLabel("Bild");
            productsGrid.Children.Add(productImage);
            Grid.SetColumn(productImage, 0);

            Label productTitle = CreateLabel("Titel");
            productsGrid.Children.Add(productTitle);
            Grid.SetColumn(productTitle, 1);

            Label productDesc = CreateLabel("Info");
            productsGrid.Children.Add(productDesc);
            Grid.SetColumn(productDesc, 2);

            Label productPrice = CreateLabel("Pris");
            productsGrid.Children.Add(productPrice);
            Grid.SetColumn(productPrice, 3);

            // Scrolling Products grid
            ScrollViewer root = new ScrollViewer();
            root.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            mainGrid.Children.Add(root);

            productsGrid = new Grid
            {
                Margin = new Thickness(5),
            };
            root.Content = productsGrid;

            Grid.SetRow(root, 3);
            Grid.SetColumn(root, 0);
            for (int i = 0; i < 5; i++)
            {
                productsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.3, GridUnitType.Star) });
            };
            productsGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            CreateProducts();


            // *********************** Right Column ***********************

            
            



            // Right Grid
            rightGrid = new Grid()
            {
                Margin = new Thickness(5)
            };
            mainGrid.Children.Add(rightGrid);
            Grid.SetColumn(rightGrid, 1);
            Grid.SetRow(rightGrid, 3);
            rightGrid.ColumnDefinitions.Add(new ColumnDefinition());
            rightGrid.ColumnDefinitions.Add(new ColumnDefinition());
            rightGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            rightGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            rightGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            rightGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            rightGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            rightGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            rightGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            rightGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            rightGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            rightGrid.RowDefinitions.Add(new RowDefinition ());
            rightGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            rightGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            rightGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            // Coupon Header
            couponHeader = CreateLabel("Change or Add new coupon");
            mainGrid.Children.Add(couponHeader);
            Grid.SetRow(couponHeader, 1);
            Grid.SetColumn(couponHeader, 1);

            // Coupon combobox
            couponComboBox = new ComboBox { Margin = new Thickness(5) };
            mainGrid.Children.Add(couponComboBox);
            Grid.SetRow(couponComboBox, 2);
            Grid.SetColumn(couponComboBox, 1);
            couponComboBox.SelectionChanged += Handle_CouponComboBox;

            string[] couponArray = File.ReadAllLines("Couponcodes.csv");
            foreach (string c in couponArray)
            {
                string[] splitCouponLines = c.Split('|');

                couponComboBox.Items.Add($"Rabattkod: {splitCouponLines[0]} Rabatt: {Math.Round(decimal.Parse(splitCouponLines[1]) * 100, 0)}%");
            }



            //Coupon Code Label
            couponCodeLabel = CreateLabel("Coupon code:");
            couponCodeLabel.FontSize = 14;
            rightGrid.Children.Add(couponCodeLabel);

            //Coupon Code Textbox
            couponCodeBox = new TextBox { Margin = new Thickness(5) };
            rightGrid.Children.Add(couponCodeBox);
            Grid.SetColumn(couponCodeBox, 1);

            //Coupon Int label
            couponIntLabel = CreateLabel("Coupon int:");
            couponIntLabel.FontSize = 14;
            rightGrid.Children.Add(couponIntLabel);
            Grid.SetRow(couponIntLabel, 1);

            //Coupon Int Textbox
            couponIntBox = new TextBox { Margin = new Thickness(5) };
            rightGrid.Children.Add(couponIntBox);
            Grid.SetRow(couponIntBox, 1);
            Grid.SetColumn(couponIntBox, 1);

            //New coupon button
            newCouponButton = new Button
            {
                Content = "New coupon",
                Margin = new Thickness(5)
            };
            rightGrid.Children.Add(newCouponButton);
            Grid.SetRow(newCouponButton, 2);
            newCouponButton.Click += HandleNewCouponButton;

            //Save coupon button
            saveCouponButton = new Button
            {
                Content = "Save coupon",
                IsDefault = true,
                Margin = new Thickness(5)
            };
            rightGrid.Children.Add(saveCouponButton);
            Grid.SetRow(saveCouponButton, 2);
            Grid.SetColumn(newCouponButton, 1);
            saveCouponButton.Click += HandleSaveCouponButton;


            //Extra coupon row  
            extraCouponRow = CreateLabel("(Extra coupon row to use)");
            extraCouponRow.FontSize = 14;
            extraCouponRow.FontWeight = FontWeights.Normal;
            rightGrid.Children.Add(extraCouponRow);
            Grid.SetRow(extraCouponRow, 3);
            Grid.SetColumnSpan(extraCouponRow, 2);

            //Empty row
            emptyRow = CreateLabel("");
            emptyRow.FontSize = 14;
            rightGrid.Children.Add(emptyRow);
            Grid.SetRow(emptyRow, 4);
            Grid.SetColumnSpan(emptyRow, 2);

            //Change Product Header
            changeProductHeader = CreateLabel("Change or Add new product:");
            changeProductHeader.FontSize = 14;
            rightGrid.Children.Add(changeProductHeader);
            Grid.SetRow(changeProductHeader, 5);
            Grid.SetColumnSpan(changeProductHeader, 2);

            //Product Image Label
            productImageLabel = CreateLabel("Image filename:");
            productImageLabel.FontSize = 14;
            rightGrid.Children.Add(productImageLabel);
            Grid.SetRow(productImageLabel, 6);
            //Product Image Textbox
            productImageBox = new TextBox { Margin = new Thickness(5) };
            rightGrid.Children.Add(productImageBox);
            Grid.SetRow(productImageBox, 6);
            Grid.SetColumn(productImageBox, 1);

            //Product Image Label
            productTitleLabel = CreateLabel("Title:");
            productTitleLabel.FontSize = 14;
            rightGrid.Children.Add(productTitleLabel);
            Grid.SetRow(productTitleLabel, 7);
            //Product Image Textbox
            productTitleBox = new TextBox { Margin = new Thickness(5) };
            rightGrid.Children.Add(productTitleBox);
            Grid.SetRow(productTitleBox, 7);
            Grid.SetColumn(productTitleBox, 1);

            //Product Description Label
            productDescriptionLabel = CreateLabel("Description:");
            productDescriptionLabel.FontSize = 14;
            rightGrid.Children.Add(productDescriptionLabel);
            Grid.SetRow(productDescriptionLabel, 8);
            //Product Image Textbox
            productDescriptionBox = new TextBox 
            { 
                Margin = new Thickness(5), 
                TextWrapping = TextWrapping.Wrap, 
            };
            rightGrid.Children.Add(productDescriptionBox);
            Grid.SetRow(productDescriptionBox, 9);
            Grid.SetRowSpan(productDescriptionBox, 2);
            Grid.SetColumnSpan(productDescriptionBox, 2);

            //Product Price Label
            productPriceLabel = CreateLabel("Price:");
            productPriceLabel.FontSize = 14;
            rightGrid.Children.Add(productPriceLabel);
            Grid.SetRow(productPriceLabel, 11);
            //Product Price Textbox
            productPriceBox = new TextBox { Margin = new Thickness(5) };
            rightGrid.Children.Add(productPriceBox);
            Grid.SetRow(productPriceBox, 11);
            Grid.SetColumn(productPriceBox, 1);

            //Save Product Button
            saveProductButton = new Button
            {
                Content = "Save changes",
                Margin = new Thickness(5)
            };
            rightGrid.Children.Add(saveProductButton);
            Grid.SetRow(saveProductButton, 12);
            saveProductButton.Click += HandleSaveProductButton;

            //Add Product Button
            addProductButton = new Button
            {
                Content = "Add product",
                Margin = new Thickness(5)
            };
            rightGrid.Children.Add(addProductButton);
            Grid.SetRow(addProductButton, 12);
            Grid.SetColumn(addProductButton, 1);
            addProductButton.Click += HandleAddProductButton;




        }

        private void Handle_CouponComboBox(object sender, SelectionChangedEventArgs e)
        {
            int index = couponComboBox.SelectedIndex;

            string fullLine = couponComboBox.SelectedItem.ToString();

            int couponCodeLength = fullLine.Substring(11)


            string couponCode = fullLine.Substring(11, fullLine.IndexOf(" "));

            int discountIndexStart = fullLine.LastIndexOf(" ") + 1;
            int discountIndexEnd = fullLine.LastIndexOf("%") - 1;
            string discountNumber = fullLine.Substring(discountIndexStart, discountIndexEnd);

            MessageBox.Show($"{couponCode} + {discountNumber}");


            // couponComboBox.Items.Add($"Rabattkod: {splitCouponLines[0]} Rabatt: {Math.Round(decimal.Parse(splitCouponLines[1]) * 100, 0)}%");
        }

        private Label CreateLabel(string content)
        {
            Label createLabel = new Label
            {
                Content = content,
                FontSize = 16,
                FontWeight = FontWeights.Bold,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            return createLabel;
        }

        private void HandleSaveCouponButton(object sender, RoutedEventArgs e)
        {
                MessageBox.Show("Test");
        }

        private void HandleNewCouponButton(object sender, RoutedEventArgs e)
        {

        }

        private void HandleSaveProductButton(object sender, RoutedEventArgs e)
        {

        }

        private void HandleAddProductButton(object sender, RoutedEventArgs e)
        {

        }

        private void CreateProducts()
        {
            int productCounter = 0;

            foreach (Product p in productList)
            {
                // Write image
                ImageSource newSource = new BitmapImage(new Uri($@"Images\products\{p.ImageFileName}", UriKind.Relative));
                Image newImage = new Image
                {
                    Source = newSource,
                    Height = 150,
                    Width = 150,
                    Margin = new Thickness(10, 0, 0, 10)
                };
                productsGrid.Children.Add(newImage);
                Grid.SetRow(newImage, productCounter);
                RenderOptions.SetBitmapScalingMode(newImage, BitmapScalingMode.HighQuality);

                // Write title
                titleLabel = CreateLabel(p.ProductTitle);

                titleLabel.FontSize = 14;
                productsGrid.Children.Add(titleLabel);
                Grid.SetRow(titleLabel, productCounter);
                Grid.SetColumn(titleLabel, 1);

                // Write description
                textBox = new TextBox
                {
                    Text = p.ProductText,
                    FontSize = 12,
                    BorderThickness = new Thickness(0),
                    TextWrapping = TextWrapping.Wrap,
                    IsReadOnly = true,
                    VerticalAlignment = VerticalAlignment.Center
                };
                productsGrid.Children.Add(textBox);
                Grid.SetRow(textBox, productCounter);
                Grid.SetColumn(textBox, 2);

                // Write price
                priceLabel = CreateLabel($"${p.ProductPrice}");
                priceLabel.FontSize = 12;
                productsGrid.Children.Add(priceLabel);
                Grid.SetRow(priceLabel, productCounter);
                Grid.SetColumn(priceLabel, 3);

                // Write "add-to-cart" button
                changeProductButton = CreateButton("Edit", p.ProductTitle);
                changeProductButton.Width = 100;
                changeProductButton.Height = 30;
                productsGrid.Children.Add(changeProductButton);
                Grid.SetRow(changeProductButton, productCounter);
                Grid.SetColumn(changeProductButton, 4);
                changeProductButton.Click += HandleChangeProductButton;

                productsGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                productCounter++;
            }
        }

        private void HandleChangeProductButton(object sender, RoutedEventArgs e)
        {
           
        }

        private Button CreateButton(string content, string tag)
        {
            Button createButton = new Button
            {
                Content = content,
                Padding = new Thickness(5),
                Margin = new Thickness(5),
                FontSize = 14,
                Tag = tag,
            };
            return createButton;
        }

        public List<Product> ReadProductFile(string productFile)
        {
            List<Product> newProductList = new List<Product>();

            string[] productArray = File.ReadAllLines(productFile);

            foreach (string line in productArray)
            {
                string[] splitLines = line.Split('|');

                Product newProduct = new Product
                {
                    ImageFileName = splitLines[0],
                    ProductTitle = splitLines[1],
                    ProductText = splitLines[2],
                    ProductPrice = decimal.Parse(splitLines[3])
                };

                newProductList.Add(newProduct);
            }

            return newProductList;
        }


    }
}
