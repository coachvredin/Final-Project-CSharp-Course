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
using Shop;

namespace ShopAdministration
{
    public class Coupon
    {
        public string Code;
        public decimal Discount;
    }

    public partial class MainWindow : Window
    {
        //private List<string> cartBoxIndexList = new List<string>();

        private Grid mainGrid;
        private Label mainTitle;

        private List<Product> productList;
        private string productFilePath = @"C:\Windows\Temp\products.csv";
        private string couponFilePath = @"C:\Windows\Temp\couponcodes.csv";
        private Grid titleGrid, productsGrid;
        private Label titleLabel, priceLabel;
        private TextBox textBox;
        private Button removeProductButton;

        private Grid rightGrid;
        private List<Coupon> couponList;
        private ComboBox couponComboBox;
        private Label couponHeader, couponCodeLabel, couponDiscountLabel, emptyRow;
        public static TextBox couponCodeBox, couponDiscountBox;
        private Button saveCouponButton, newCouponButton, removeCouponButton;
        private int couponIndex;

        private Label changeProductHeader, productImageLabel, productTitleLabel, productDescriptionLabel, productPriceLabel;
        private TextBox productTitleBox, productDescriptionBox, productPriceBox;
        private ComboBox productsBox, productImageBox;
        private Button saveProductButton, addProductButton;
        private int productIndex;


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

            productList = ReadProductFile(productFilePath);

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
            mainTitle = CreateLabel("Admin Imperial Shop Program");
            mainTitle.FontSize = 30;
            mainTitle.FontWeight = FontWeights.Bold;
            mainGrid.Children.Add(mainTitle);
            Grid.SetColumn(mainTitle, 0);

            // ---------- Products Grid ----------
            titleGrid = new Grid();
            mainGrid.Children.Add(titleGrid);
            Grid.SetRow(titleGrid, 2);
            Grid.SetColumn(titleGrid, 0);

            double titleColumnWith = 3;
            titleGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            titleGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(titleColumnWith, GridUnitType.Star) });
            titleGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(titleColumnWith, GridUnitType.Star) });
            titleGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(titleColumnWith, GridUnitType.Star) });
            titleGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(titleColumnWith, GridUnitType.Star) });

            for (int i = 0; i < 6; i++)
            {
                titleGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.2, GridUnitType.Star) });
            };

            Label productImage = CreateLabel("Picture");
            titleGrid.Children.Add(productImage);
            Grid.SetColumn(productImage, 0);

            Label productTitle = CreateLabel("Title");
            titleGrid.Children.Add(productTitle);
            Grid.SetColumn(productTitle, 1);

            Label productDesc = CreateLabel("Info");
            titleGrid.Children.Add(productDesc);
            Grid.SetColumn(productDesc, 2);

            Label productPrice = CreateLabel("Price");
            titleGrid.Children.Add(productPrice);
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
            double productsColumnWith = 3;
            productsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(productsColumnWith, GridUnitType.Star) });
            productsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(productsColumnWith, GridUnitType.Star) });
            productsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(productsColumnWith, GridUnitType.Star) });
            productsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(productsColumnWith, GridUnitType.Star) });

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
            rightGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            rightGrid.RowDefinitions.Add(new RowDefinition());
            rightGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            rightGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            rightGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            rightGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            // Coupon Header
            couponHeader = CreateLabel("Manage coupons");
            mainGrid.Children.Add(couponHeader);
            Grid.SetRow(couponHeader, 1);
            Grid.SetColumn(couponHeader, 1);

            // Coupon List
            couponList = CreateCouponList(couponFilePath);

            // Coupon combobox
            couponComboBox = new ComboBox { Margin = new Thickness(5) };
            mainGrid.Children.Add(couponComboBox);
            Grid.SetRow(couponComboBox, 2);
            Grid.SetColumn(couponComboBox, 1);
            couponComboBox.SelectionChanged += HandleCouponComboBox;

            UpdateCouponComboBox(couponList);

            //Coupon Code Label
            couponCodeLabel = CreateLabel("Coupon code:");
            couponCodeLabel.FontSize = 14;
            rightGrid.Children.Add(couponCodeLabel);

            //Coupon Code Textbox
            couponCodeBox = new TextBox { Margin = new Thickness(5) };
            rightGrid.Children.Add(couponCodeBox);
            Grid.SetColumn(couponCodeBox, 1);

            //Coupon Discount label
            couponDiscountLabel = CreateLabel("Coupon discount:");
            couponDiscountLabel.FontSize = 14;
            rightGrid.Children.Add(couponDiscountLabel);
            Grid.SetRow(couponDiscountLabel, 1);

            //Coupon Discount Textbox
            couponDiscountBox = new TextBox { Margin = new Thickness(5) };
            rightGrid.Children.Add(couponDiscountBox);
            Grid.SetRow(couponDiscountBox, 1);
            Grid.SetColumn(couponDiscountBox, 1);

            //New coupon button
            newCouponButton = new Button
            {
                Content = "Add new coupon",
                Margin = new Thickness(5)
            };
            rightGrid.Children.Add(newCouponButton);
            Grid.SetRow(newCouponButton, 2);
            newCouponButton.Click += HandleNewCouponButton;

            //Save coupon changes button
            saveCouponButton = new Button
            {
                Content = "Save changes",
                Margin = new Thickness(5)
            };
            rightGrid.Children.Add(saveCouponButton);
            Grid.SetRow(saveCouponButton, 2);
            Grid.SetColumn(newCouponButton, 1);
            saveCouponButton.Click += HandleSaveCouponButton;
            saveCouponButton.IsEnabled = false;

            //Erase coupon button
            removeCouponButton = new Button
            {
                Content = "Remove coupon",
                Margin = new Thickness(5)
            };
            rightGrid.Children.Add(removeCouponButton);
            Grid.SetRow(removeCouponButton, 3);
            removeCouponButton.Click += HandleRemoveCouponButton; ;
            removeCouponButton.IsEnabled = false;

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

            //Product Combobox showing product titles
            productsBox = new ComboBox { Margin = new Thickness(5) };
            rightGrid.Children.Add(productsBox);
            Grid.SetRow(productsBox, 6);
            Grid.SetColumnSpan(productsBox, 2);
            productsBox.SelectionChanged += HandleProductsBox;

            UpdateProductComboBox(productList);

            //Product Image Label
            productImageLabel = CreateLabel("Image filename:");
            productImageLabel.FontSize = 14;
            rightGrid.Children.Add(productImageLabel);
            Grid.SetRow(productImageLabel, 7);

            //Product Image Combobox
            productImageBox = new ComboBox { Margin = new Thickness(5) };
            rightGrid.Children.Add(productImageBox);
            Grid.SetRow(productImageBox, 7);
            Grid.SetColumn(productImageBox, 1);
            string[] fileEntries = Directory.GetFiles(@"Images\products");
            foreach (string fileName in fileEntries)
            {
                productImageBox.Items.Add($"{fileName.Substring(16)}");
            }

            //Product Image Label
            productTitleLabel = CreateLabel("Title:");
            productTitleLabel.FontSize = 14;
            rightGrid.Children.Add(productTitleLabel);
            Grid.SetRow(productTitleLabel, 8);

            //Product Image Textbox
            productTitleBox = new TextBox { Margin = new Thickness(5) };
            rightGrid.Children.Add(productTitleBox);
            Grid.SetRow(productTitleBox, 8);
            Grid.SetColumn(productTitleBox, 1);

            //Product Description Label
            productDescriptionLabel = CreateLabel("Description:");
            productDescriptionLabel.FontSize = 14;
            rightGrid.Children.Add(productDescriptionLabel);
            Grid.SetRow(productDescriptionLabel, 9);
            //Product Image Textbox
            productDescriptionBox = new TextBox
            {
                Margin = new Thickness(5),
                TextWrapping = TextWrapping.Wrap,
            };
            rightGrid.Children.Add(productDescriptionBox);
            Grid.SetRow(productDescriptionBox, 10);
            Grid.SetRowSpan(productDescriptionBox, 2);
            Grid.SetColumnSpan(productDescriptionBox, 2);

            //Product Price Label
            productPriceLabel = CreateLabel("Price:");
            productPriceLabel.FontSize = 14;
            rightGrid.Children.Add(productPriceLabel);
            Grid.SetRow(productPriceLabel, 12);
            //Product Price Textbox
            productPriceBox = new TextBox { Margin = new Thickness(5) };
            rightGrid.Children.Add(productPriceBox);
            Grid.SetRow(productPriceBox, 12);
            Grid.SetColumn(productPriceBox, 1);

            //Save Product Button
            saveProductButton = new Button
            {
                Content = "Save changes",
                Margin = new Thickness(5)
            };
            rightGrid.Children.Add(saveProductButton);
            Grid.SetRow(saveProductButton, 13);
            saveProductButton.Click += HandleSaveProductButton;
            saveProductButton.IsEnabled = false;

            //Add Product Button
            addProductButton = new Button
            {
                Content = "Add product",
                Margin = new Thickness(5)
            };
            rightGrid.Children.Add(addProductButton);
            Grid.SetRow(addProductButton, 13);
            Grid.SetColumn(addProductButton, 1);
            addProductButton.Click += HandleAddProductButton;

            //Remove Product button
            removeProductButton = new Button
            {
                Content = "Remove product",
                Margin = new Thickness(5)
            };
            rightGrid.Children.Add(removeProductButton);
            Grid.SetRow(removeProductButton, 14);
            removeProductButton.Click += HandleRemoveProductButton;
            removeProductButton.IsEnabled = false;

        }

        //Read properties from object into textboxes when product is selected in dropdown list, sets buttons states
        private void HandleProductsBox(object sender, SelectionChangedEventArgs e)
        {
            if (productsBox.SelectedIndex == -1)
            {
                return;
            }

            productIndex = productsBox.SelectedIndex;

            productImageBox.Text = productList[productIndex].ImageFileName;
            productTitleBox.Text = productList[productIndex].ProductTitle;
            productDescriptionBox.Text = productList[productIndex].ProductText;
            productPriceBox.Text = productList[productIndex].ProductPrice.ToString();

            addProductButton.IsEnabled = false;
            saveProductButton.IsEnabled = true;
            removeProductButton.IsEnabled = true;
        }

        //Read values from coupon-dictionary into textboxes when coupon is selected in dropdown list, sets buttons states
        private void HandleCouponComboBox(object sender, SelectionChangedEventArgs e)
        {
            if (couponComboBox.SelectedIndex == - 1)
            {
                return;
            }

            couponIndex = couponComboBox.SelectedIndex;

            couponCodeBox.Text = couponList[couponIndex].Code;
            couponDiscountBox.Text = couponList[couponIndex].Discount.ToString();

            newCouponButton.IsEnabled = false;
            saveCouponButton.IsEnabled = true;
            removeCouponButton.IsEnabled = true;
        }

        //Evaluates coupon rules, then updates coupon with data from textboxes, sets buttons states
        private void HandleSaveCouponButton(object sender, RoutedEventArgs e)
        {
            if (!CheckCouponRules())
            {
                return;
            }

            couponList[couponIndex].Code = couponCodeBox.Text;

            couponList[couponIndex].Discount = decimal.Parse(couponDiscountBox.Text);

            UpdateCouponFileAndBox(couponList);

            saveCouponButton.IsEnabled = false;
            removeCouponButton.IsEnabled = false;
            newCouponButton.IsEnabled = true;

            couponCodeBox.Text = "";
            couponDiscountBox.Text = "";
        }

        //Reads index of selected coupon and removes from coupon-dictionary, sets buttons states
        private void HandleRemoveCouponButton(object sender, RoutedEventArgs e)
        {
            couponList.RemoveAt(couponIndex);

            UpdateCouponFileAndBox(couponList);

            saveCouponButton.IsEnabled = false;
            removeCouponButton.IsEnabled = false;
            newCouponButton.IsEnabled = true;

            couponCodeBox.Text = "";
            couponDiscountBox.Text = "";
        }

        //Evaluates coupon rules, then creates new button with data from textboxes, sets buttons states and update GUI
        private void HandleNewCouponButton(object sender, RoutedEventArgs e)
        {
            if (!CheckCouponRules())
            {
                return;
            }

            Coupon newCoupon = new Coupon
            {
                Code = couponCodeBox.Text,
                Discount = decimal.Parse(couponDiscountBox.Text)
            };

            couponList.Add(newCoupon);

            UpdateCouponFileAndBox(couponList);

            couponCodeBox.Text = "";
            couponDiscountBox.Text = "";
        }

        //Checks if textboxes are empty, if values are in correct range and if code contains letters and digits only.
        public static bool CheckCouponRules()
        {
            if (couponCodeBox.Text == "" || couponDiscountBox.Text == "")
            {
                MessageBox.Show("Please enter code and discount for new coupon");
                return false;
            }

            if (decimal.Parse(couponDiscountBox.Text) <= 0 || decimal.Parse(couponDiscountBox.Text) >= 1)
            {
                MessageBox.Show("Please enter a discount between 0 - 1");
                return false;
            }

            if (couponCodeBox.Text.Count() < 3 || couponCodeBox.Text.Count() > 20)
            {
                MessageBox.Show("Please enter a code between 3 and 20 characters long");
                return false;
            }

            foreach (char c in couponCodeBox.Text)
            {
                if (!Char.IsLetterOrDigit(c))
                {
                    MessageBox.Show("Please enter only letter and digits");
                    return false;
                }
            };

            return true;
        }

        //Checks for valid input and updates properties of product at selected index. Updates GUI and sets button status.
        private void HandleSaveProductButton(object sender, RoutedEventArgs e)
        {
            if (productTitleBox.Text == "")
            {
                MessageBox.Show("Please enter a title.");
                return;
            }
            
            if (productDescriptionBox.Text == "")
            {
                MessageBox.Show("Please enter a description.");
                return;
            }
            
            if (productPriceBox.Text == "")
            {
                MessageBox.Show("Please enter a price.");
                return;
            }

            if (decimal.Parse(productPriceBox.Text) <= 0)
            {
                MessageBox.Show("Please enter a price larger than 0");
                return;
            }

            productList[productIndex].ImageFileName = productImageBox.Text;
            productList[productIndex].ProductTitle = productTitleBox.Text;
            productList[productIndex].ProductText = productDescriptionBox.Text;
            productList[productIndex].ProductPrice = decimal.Parse(productPriceBox.Text);

            UpdateProductFileAndBox(productList);

            saveProductButton.IsEnabled = false;
            removeProductButton.IsEnabled = false;
            addProductButton.IsEnabled = true;

            productImageBox.SelectedIndex = -1;
            productTitleBox.Text = "";
            productDescriptionBox.Text = "";
            productPriceBox.Text = "";
            
        }

        //Checks for valid input and creats new product object with data from textboxes and slected image. Updates GUI and sets button status.
        private void HandleAddProductButton(object sender, RoutedEventArgs e)
        {
            if (productTitleBox.Text == "")
            {
                MessageBox.Show("Please enter a title.");
                return;
            }

            if (productDescriptionBox.Text == "")
            {
                MessageBox.Show("Please enter a description.");
                return;
            }

            if (productPriceBox.Text == "")
            {
                MessageBox.Show("Please enter a price.");
                return;
            }

            if (decimal.Parse(productPriceBox.Text) <= 0)
            {
                MessageBox.Show("Please enter a price larger than 0");
                return;
            }

            Product newProduct = new Product
            {
                ImageFileName = productImageBox.Text,
                ProductTitle = productTitleBox.Text,
                ProductText = productDescriptionBox.Text,
                ProductPrice = decimal.Parse(productPriceBox.Text)
            };

            productList.Add(newProduct);

            UpdateProductFileAndBox(productList);

            productImageBox.SelectedIndex = -1;
            productTitleBox.Text = "";
            productDescriptionBox.Text = "";
            productPriceBox.Text = "";
        }

        //Default parameters for new button
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

        //Loops through product list generating product rows from properties for each product in the list
        private void CreateProducts()
        {
            int productCounter = 0;

            foreach (Product p in productList)
            {
                productsGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

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

                productCounter++;

            }
        }

        //Removes selected product from product list, and updates GUI.
        private void HandleRemoveProductButton(object sender, RoutedEventArgs e)
        {
            productList.RemoveAt(productIndex);

            UpdateProductFileAndBox(productList);

            saveProductButton.IsEnabled = false;
            removeProductButton.IsEnabled = false;
            addProductButton.IsEnabled = true;

            productTitleBox.Text = "";
            productDescriptionBox.Text = "";
            productPriceBox.Text = "";
        }

        //Reads the product file and returns a products list used to show assortment in the GUI and manage products
        public List<Product> ReadProductFile(string path)
        {
            if (!File.Exists(productFilePath))
            {
                File.Copy("products.csv", productFilePath);
            }

            List<Product> newProductList = new List<Product>();

            string[] productArray = File.ReadAllLines(path);

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

        //Creates a list with all coupons in custom file or from default file.
        public List<Coupon> CreateCouponList(string path)
        {
            if (!File.Exists(couponFilePath))
            {
                File.Copy("couponcodes.csv", couponFilePath);
            }

            List<Coupon> newList = new List<Coupon>();

            string[] couponArray = File.ReadAllLines(path);

            foreach (string c in couponArray)
            {
                string[] splitCouponLines = c.Split('|');

                Coupon newCoupon = new Coupon
                {
                    Code = splitCouponLines[0],
                    Discount = decimal.Parse(splitCouponLines[1])
                };

                newList.Add(newCoupon);
            }

            return newList;
        }

        //Clear and update the coupon GUI
        public void UpdateCouponComboBox(List<Coupon> inList)
        {
            couponComboBox.Items.Clear();

            foreach (Coupon c in inList)
            {
                couponComboBox.Items.Add($"Code: {c.Code} Discount: {Math.Round(100 * c.Discount, 0)}%");
            }
        }

        //Clear and update the dropdownbox for selecting a product in the GUI
        public void UpdateProductComboBox(List<Product> inList)
        {
            productsBox.Items.Clear();

            foreach (Product p in inList)
            {
                productsBox.Items.Add($"{p.ProductTitle}");
            }
        }

        //Writes coupon code csv file and updates GUI
        public void UpdateCouponFileAndBox(List<Coupon> inList)
        {
            List<string> codeDiscount = new List<string>();

            foreach (Coupon c in inList)
            {
                codeDiscount.Add($"{c.Code}|{c.Discount}");
            }

            File.WriteAllLines(@"C:\Windows\Temp\Couponcodes.csv", codeDiscount);

            UpdateCouponComboBox(couponList);
        }

        //Writes product csv file and updates GUI
        public void UpdateProductFileAndBox(List<Product> inList)
        {
            List<string> newproductList = new List<string>();

            foreach (Product p in inList)
            {
                newproductList.Add($"{p.ImageFileName}|{p.ProductTitle}|{p.ProductText}|{p.ProductPrice}");
            }

            File.WriteAllLines(@"C:\Windows\Temp\Products.csv", newproductList);

            UpdateProductComboBox(productList);

            productsGrid.Children.Clear();
            productsGrid.RowDefinitions.Clear();

            CreateProducts();
        }
    }
}
