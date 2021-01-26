using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Printing;
using System.Security.Cryptography.X509Certificates;
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

namespace Shop
{
    public class Product
    {
        public string ImageFileName;
        public string ProductTitle;
        public string ProductText;
        public decimal ProductPrice;
    }
    public partial class MainWindow : Window
    {
        private Grid mainGrid;
        private Image mainImage;
        private Label mainTitle;
        private Grid productsHeaderGrid;
        private static List<Product> productList;

        private Dictionary<Product, int> cart;
        private static string cartFilePath = @"C:\Windows\Temp\cart.csv";
        private Grid cartGrid;
        private decimal sumPrice, amount;
        private Label cartLabel, cartSummary;
        private static List<string> cartBoxIndexList = new List<string>();
        private ListBox cartBox;
        private Button subButton, clearButton, saveButton, orderButton;

        private static string couponFilePath = @"C:\Windows\Temp\couponcodes.csv";
        private Dictionary<string, decimal> couponDictionary;
        private Label couponLabel;
        private TextBox couponBox;
        private Button addCouponButton, eraseCouponButton;
        private decimal priceMultiplier = 1;
        private decimal discountAmount;

        private static string productFilePath = @"C:\Windows\Temp\products.csv";
        private Grid mainProductGrid;
        private Label titleLabel, priceLabel;
        private TextBox textBox;
        private Button addToCartButton;

        public MainWindow()
        {
            InitializeComponent();
            Start();
        }

        //Start method includes the GUI and calls on methods. All logical functions will be handled and commented in respectively called method.
        private void Start()
        {
            // *********************** Window options ***********************
            Title = "Imperial War Shop";
            Width = 1500;
            Height = 800;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;


            // *********************** Main grid ***********************
            mainGrid = new Grid();
            Content = mainGrid;
            mainGrid.ColumnDefinitions.Add(new ColumnDefinition());
            mainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.3, GridUnitType.Star) });
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            mainGrid.RowDefinitions.Add(new RowDefinition());

            // ---------- Main Logo ----------
            ImageSource source1 = new BitmapImage(new Uri(@"Images\starwarslogo.png", UriKind.Relative));
            mainImage = new Image
            {
                Source = source1,
                Height = 100,
                Width = 100,
                Margin = new Thickness(10, 0, 0, 0)
            };
            mainGrid.Children.Add(mainImage);
            Grid.SetRow(mainImage, 0);
            Grid.SetColumn(mainImage, 1);
            RenderOptions.SetBitmapScalingMode(mainImage, BitmapScalingMode.HighQuality);

            // ---------- Main Title ----------
            mainTitle = CreateLabel("Welcome to the Imperial War Shop!");
            mainTitle.FontSize = 30;
            mainTitle.FontWeight = FontWeights.Bold;
            mainGrid.Children.Add(mainTitle);
            Grid.SetColumn(mainTitle, 0);

            // ******************** Products grid *********************
            productsHeaderGrid = new Grid();
            mainGrid.Children.Add(productsHeaderGrid);
            Grid.SetRow(productsHeaderGrid, 2);
            Grid.SetColumn(productsHeaderGrid, 0);

            productsHeaderGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            productsHeaderGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.2, GridUnitType.Star) });
            productsHeaderGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.2, GridUnitType.Star) });
            productsHeaderGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.2, GridUnitType.Star) });
            productsHeaderGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.2, GridUnitType.Star) });
            productsHeaderGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.2, GridUnitType.Star) });

            // ------- Product Labels ------
            Label productImage = CreateLabel("Picture");
            productsHeaderGrid.Children.Add(productImage);
            Grid.SetColumn(productImage, 0);

            Label productTitle = CreateLabel("Title");
            productsHeaderGrid.Children.Add(productTitle);
            Grid.SetColumn(productTitle, 1);

            Label productDesc = CreateLabel("Info");
            productsHeaderGrid.Children.Add(productDesc);
            Grid.SetColumn(productDesc, 2);

            Label productPrice = CreateLabel("Price");
            productsHeaderGrid.Children.Add(productPrice);
            Grid.SetColumn(productPrice, 3);

            // ---------- Scrolling --------
            ScrollViewer root = new ScrollViewer();
            root.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            mainGrid.Children.Add(root);

            mainProductGrid = new Grid
            {
                Margin = new Thickness(5),
            };
            root.Content = mainProductGrid;

            Grid.SetRow(root, 3);
            Grid.SetColumn(root, 0);
            for (int i = 0; i < 5; i++)
            {
                mainProductGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.3, GridUnitType.Star) });
            };

            // -------- Load & Create Products --------

            productList = ReadProductFile(productFilePath);

            CreateProducts();


            // *********************** Cart Column ***********************

            // ---------- Cart Banner ----------

            // Cart Header
            cartLabel = CreateLabel("Cart summary");
            mainGrid.Children.Add(cartLabel);
            Grid.SetRow(cartLabel, 1);
            Grid.SetColumn(cartLabel, 1);

            // Cart Summary
            cartSummary = CreateLabel($"Total: {sumPrice}");
            cartSummary.FontSize = 14;
            mainGrid.Children.Add(cartSummary);
            Grid.SetRow(cartSummary, 2);
            Grid.SetColumn(cartSummary, 1);

            // Cart Grid
            cartGrid = new Grid()
            {
                Margin = new Thickness(5)
            };
            mainGrid.Children.Add(cartGrid);
            Grid.SetColumn(cartGrid, 1);
            Grid.SetRow(cartGrid, 3);
            cartGrid.ColumnDefinitions.Add(new ColumnDefinition());
            cartGrid.ColumnDefinitions.Add(new ColumnDefinition());
            cartGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            cartGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            cartGrid.RowDefinitions.Add(new RowDefinition());
            cartGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            cartGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            cartGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            cartGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            // ---------- Coupon Label ----------
            couponLabel = CreateLabel("Coupon code:");
            couponLabel.FontSize = 14;
            cartGrid.Children.Add(couponLabel);

            // ---------- Coupon Textbox ---------
            couponBox = new TextBox { Margin = new Thickness(5) };
            cartGrid.Children.Add(couponBox);
            Grid.SetColumn(couponBox, 1);

            // ---------- Coupon Button ---------
            addCouponButton = new Button
            {
                Content = "Add coupon code",
                IsDefault = true,
                Margin = new Thickness(5)
            };
            cartGrid.Children.Add(addCouponButton);
            Grid.SetRow(addCouponButton, 1);
            addCouponButton.Click += HandleCouponButton;

            eraseCouponButton = new Button
            {
                Content = "Erase coupon code",
                Margin = new Thickness(5)
            };
            cartGrid.Children.Add(eraseCouponButton);
            Grid.SetColumn(eraseCouponButton, 1);
            Grid.SetRow(eraseCouponButton, 1);
            eraseCouponButton.Click += HandleEraseButton;

            couponDictionary = CreateCouponDictionary(couponFilePath);

            // ---------- Cart Listbox ---------
            cartBox = new ListBox { Margin = new Thickness(5) };
            cartGrid.Children.Add(cartBox);
            Grid.SetRow(cartBox, 2);
            Grid.SetColumnSpan(cartBox, 2);
            cartBox.Items.Add("Cart is empty");
            cartBox.SelectedIndex = 0;

            // ---------- Load Cart ----------
            cart = LoadCart(cartFilePath);
            UpdateCart();

            // ---------- Sub Button ----------
            subButton = CreateButton("Remove product");
            cartGrid.Children.Add(subButton);
            Grid.SetRow(subButton, 3);
            Grid.SetColumnSpan(subButton, 2);
            subButton.Click += HandleSubButton;

            // ---------- Clear Button ----------
            clearButton = CreateButton("Clear cart");
            cartGrid.Children.Add(clearButton);
            Grid.SetRow(clearButton, 4);
            Grid.SetColumnSpan(clearButton, 2);
            clearButton.Click += HandleClearButton;

            // ---------- Save Button ----------
            saveButton = CreateButton("Save cart");
            cartGrid.Children.Add(saveButton);
            Grid.SetRow(saveButton, 5);
            Grid.SetColumnSpan(saveButton, 2);
            saveButton.Click += HandleSaveButton;

            // ---------- Order Button ----------
            orderButton = CreateButton("Order cart");
            cartGrid.Children.Add(orderButton);
            Grid.SetRow(orderButton, 6);
            Grid.SetColumnSpan(orderButton, 2);
            orderButton.Click += HandleOrderButton;
        }
        
        // Reads user defined cart from file, or creates new empty cart.
        public static Dictionary<Product, int> LoadCart(string path)
        {
            Dictionary<Product, int> newCart = new Dictionary<Product, int>();

            if (File.Exists(cartFilePath))
            {
                //If a cart already exist, create an array where the cart will be converted into.
                string[] loadedCart = File.ReadAllLines(path);
                foreach (string line in loadedCart)
                {
                    //Split each line in loadedCart into specific properties in a new array.
                    string[] loadedProducts = line.Split('|');

                    foreach (Product p in productList)
                    {
                        //Compare if the title of any of the products in productList equals the specific product title in the array.  
                        if (p.ProductTitle == loadedProducts[0])
                        {
                            Product loadedProduct = p;

                            //If the loaded product can't be found in the productList, add it to the cart index-list.
                            if (!cartBoxIndexList.Contains(p.ProductTitle))
                            {
                                cartBoxIndexList.Add(p.ProductTitle);
                            }

                            //Adds founded product and amount to a new cart
                            newCart[loadedProduct] = int.Parse(loadedProducts[1]);

                            break;
                        }
                    }
                }
            }
            //Returning an empty cart if no one exist OR the cart that's already exist. 
            return newCart;
        }

        //Reads the product file and returns a products list used for:
        // * show assortment in the GUI
        // * read in products from the cart
        // * update the GUI when a product is added to the cart
        public static List<Product> ReadProductFile(string path)
        {
            //See if there's an existing product file on this device, otherwise use the one in this Project.  
            if (!File.Exists(productFilePath))
            {
                File.Copy("products.csv", productFilePath);
            }

            List<Product> newProductList = new List<Product>();

            //Separate each property from the file, to create a new product-object.
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
                
                //Add each product to the list
                newProductList.Add(newProduct);
            }

            return newProductList;
        }

        //Default label defenition
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

        //Default button defenition
        private Button CreateButton(string content)
        {
            Button createButton = new Button
            {
                Content = content,
                Padding = new Thickness(5),
                Margin = new Thickness(5),
                FontSize = 14,
            };
            return createButton;
        }

        //Overload used for "Add to cart"-button.
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

        // Creates the product GUI by looping through the "ProductList" and creating new rows in the product grid.
        private void CreateProducts()
        {
            int productCounter = 0;

            foreach (Product p in productList)
            {
                // Create image
                ImageSource newSource = new BitmapImage(new Uri($@"Images\products\{p.ImageFileName}", UriKind.Relative));
                Image newImage = new Image
                {
                    Source = newSource,
                    Height = 150,
                    Width = 150,
                    Margin = new Thickness(10, 0, 0, 10)
                };
                mainProductGrid.Children.Add(newImage);
                Grid.SetRow(newImage, productCounter);
                RenderOptions.SetBitmapScalingMode(newImage, BitmapScalingMode.HighQuality);

                // Create title
                titleLabel = CreateLabel(p.ProductTitle);

                titleLabel.FontSize = 14;
                mainProductGrid.Children.Add(titleLabel);
                Grid.SetRow(titleLabel, productCounter);
                Grid.SetColumn(titleLabel, 1);

                // Create description
                textBox = new TextBox
                {
                    Text = p.ProductText,
                    FontSize = 12,
                    BorderThickness = new Thickness(0),
                    TextWrapping = TextWrapping.Wrap,
                    IsReadOnly = true,
                    VerticalAlignment = VerticalAlignment.Center
                };
                mainProductGrid.Children.Add(textBox);
                Grid.SetRow(textBox, productCounter);
                Grid.SetColumn(textBox, 2);

                // Create price
                priceLabel = CreateLabel($"${p.ProductPrice}");
                priceLabel.FontSize = 12;
                mainProductGrid.Children.Add(priceLabel);
                Grid.SetRow(priceLabel, productCounter);
                Grid.SetColumn(priceLabel, 3);

                // Create "Add-to-cart" button
                addToCartButton = CreateButton("Add to cart", p.ProductTitle);
                addToCartButton.Width = 100;
                addToCartButton.Height = 30;
                mainProductGrid.Children.Add(addToCartButton);
                Grid.SetRow(addToCartButton, productCounter);
                Grid.SetColumn(addToCartButton, 4);
                addToCartButton.Click += HandleAddToCartButton;

                mainProductGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                productCounter++;
            }
        }

        //Copy default coupon file if no user defined coupon file exists. Creates dictionary from file with code and discount elements.
        public static Dictionary<string, decimal> CreateCouponDictionary(string path)
        {
            if (!File.Exists(couponFilePath))
            {
                File.Copy("couponcodes.csv", couponFilePath);
            }

            Dictionary<string, decimal> newDictionary = new Dictionary<string, decimal>();

            string[] couponArray = File.ReadAllLines(path);

            foreach (string c in couponArray)
            {
                string[] splitCouponLines = c.Split('|');

                newDictionary[splitCouponLines[0]] = decimal.Parse(splitCouponLines[1]);
            }

            return newDictionary;
        }

        private void HandleCouponButton(object sender, RoutedEventArgs e)
        {
            //The priceMultiplier is the regular price, ie 100%, so if it's not 1 then a coupon has been used.
            if (priceMultiplier != 1)
            {
                MessageBox.Show("You've already used a coupon.");
                return;
            }

            //Compare the users input with the existing coupon codes, if it's equal, set the priceMultiplier to the discount. If no matching coupon found, display error message.
            foreach (KeyValuePair<string, decimal> pair in couponDictionary)
            {
                if (couponBox.Text.ToLower() == pair.Key.ToLower())
                {
                    priceMultiplier = (1 - pair.Value);
                    UpdateCart();
                    MessageBox.Show($"Coupon code '{pair.Key}' gives {Math.Round(pair.Value * 100, 0)}% off and is now added to to the total!");
                    return;
                }
            }
            MessageBox.Show("Invalid coupon code!");
        }

        //Removes coupon discount, sets regular price multiplier.
        private void HandleEraseButton(object sender, RoutedEventArgs e)
        {
            priceMultiplier = 1;
            couponBox.Clear();

            UpdateCart();
        }

        //Adds product to cart related to associated tag and then update cart GUI.
        private void HandleAddToCartButton(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;

            foreach (Product p in productList)
            {
                if (p.ProductTitle == (string)clickedButton.Tag)
                {
                    //Adds product to cart or updates the amount if the product already exists in the cart dictionary.
                    if (cart.ContainsKey(p))
                    {
                        cart[p] += 1;
                    }
                    else
                    {
                        cart[p] = 1;
                    }

                    //We need a separated indexlist since the cart dictionary doesn't have any. Otherwise the cart box doesn't know the order ones the user wants to do changes in the cart.  
                    if (!cartBoxIndexList.Contains(p.ProductTitle))
                    {
                        cartBoxIndexList.Add(p.ProductTitle);
                    }

                    break;
                }
            }

            UpdateCart();
        }

        //Removes one unit of selected item in cart.
        private void HandleSubButton(object sender, RoutedEventArgs e)
        {
            //Return if no product is selected.
            if (cartBox.SelectedIndex == -1)
            {
                return;
            }

            int i = cartBox.SelectedIndex;
            string productToRemove = cartBoxIndexList[i];


            foreach (KeyValuePair<Product, int> pair in cart)
            {
                Product p = pair.Key;
                amount = pair.Value;

                if (productToRemove == p.ProductTitle)
                {
                    cart[p] -= 1;

                    if (cart[p] == 0)
                    {
                        cart.Remove(p);
                        cartBoxIndexList.Remove(p.ProductTitle);
                    }
                    break;
                }
            }

            UpdateCart();

            if (!cartBoxIndexList.Any())
            {
                cartSummary.Content = ($"Total: ${sumPrice}");
            }
        }

        //Clear cart GUI, cart dictionary and reset price. 
        private void HandleClearButton(object sender, RoutedEventArgs e)
        {
            cartBox.Items.Clear();
            cart.Clear();
            cartBoxIndexList.Clear();
            sumPrice = 0;
            cartSummary.Content = ($"Total: ${sumPrice}");

            if (File.Exists(cartFilePath))
            {
                File.Delete(cartFilePath);
            }
        }

        //Save cart to file in specified path.
        private void HandleSaveButton(object sender, RoutedEventArgs e)
        {
            if (cart.Any())
            {
                List<string> savedCart = new List<string>();
                foreach (KeyValuePair<Product, int> pair in cart)
                {
                    Product p = pair.Key;
                    int amount = pair.Value;

                    savedCart.Add(p.ProductTitle + "|" + amount);
                }
                File.WriteAllLines(cartFilePath, savedCart);

                MessageBox.Show("Cart saved!");
            }
            else
            {
                MessageBox.Show("You can't save an empty cart.");
            }
        }

        //Prints out the user's order, if it's empty a message is shown, without handling any payment process.
        private void HandleOrderButton(object sender, RoutedEventArgs e)
        {
            if (!cartBoxIndexList.Any())
            {
                MessageBox.Show("Cart is empty!");
                return;
            }

            string showOrder = "";

            //Since each line in cartbox includes multiple strings we neeed a list with only the product title to compare with the p.ProductTitle.
            foreach (string s in cartBoxIndexList)
            {
                //And then we can get the right item from the cart dictionary.
                foreach (KeyValuePair<Product, int> pair in cart)
                {
                    Product p = pair.Key;
                    amount = pair.Value;

                    if (p.ProductTitle == s)
                    {
                        showOrder += ($"{p.ProductTitle} x {amount} tot. ${amount * p.ProductPrice}\n");
                    }
                }
            }

            MessageBox.Show($"RECEIPT\n\n{showOrder}\nDiscount: ${discountAmount}\n\nTOTAL: ${sumPrice}\n\nThank you for your order!");

            cartBox.Items.Clear();
            cart.Clear();
            cartBoxIndexList.Clear();
            sumPrice = 0;
            cartSummary.Content = ($"Total: ${sumPrice}");
            priceMultiplier = 1;
        }

        //Clears and rebuild cartbox, total cost and discount that is showed in the GUI. 
        private void UpdateCart()
        {
            cartBox.Items.Clear();
            sumPrice = 0;
            discountAmount = 0;

            foreach (string s in cartBoxIndexList)
            {
                foreach (KeyValuePair<Product, int> pair in cart)
                {
                    Product p = pair.Key;
                    amount = pair.Value;

                    if (p.ProductTitle == s)
                    {
                        cartBox.Items.Add($"{p.ProductTitle} x {amount} tot. ${amount * p.ProductPrice}");

                        discountAmount += amount * p.ProductPrice * (1 - priceMultiplier);

                        sumPrice += amount * p.ProductPrice - amount * p.ProductPrice * (1 - priceMultiplier);

                        cartSummary.Content = ($"Total: ${sumPrice}");
                    }
                }
            }
        }
    }
}