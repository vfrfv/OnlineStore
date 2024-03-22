using System;
using System.Collections.Generic;

namespace OnlineStore
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Good iPhone12 = new Good("IPhone 12");
            Good iPhone11 = new Good("IPhone 11");

            Warehouse warehouse = new Warehouse();

            Shop shop = new Shop(warehouse);

            warehouse.Delive(iPhone12, 10);
            warehouse.Delive(iPhone11, 1);

            warehouse.SoShowProductsStock();

            Cart cart = shop.Cart();
            cart.Add(iPhone12, 4);
            cart.Add(iPhone11, 3);

            cart.ShowProductsCart();

            Console.WriteLine(cart.Order().Paylink);

            cart.Add(iPhone12, 9); 
        }
    }

    class Shop
    {
        private Warehouse _warehouse;

        public Shop(Warehouse warehouse)
        {
            _warehouse = warehouse ?? throw new ArgumentNullException(nameof(warehouse));
        }

        public Cart Cart()
        {
            return new Cart(_warehouse);
        }
    }

    class Cart
    {
        private Dictionary<Good, int> _productsCart;
        private IWarehouse _warehouse;

        public Cart(IWarehouse warehouse)
        {
            _productsCart = new Dictionary<Good, int>();
            _warehouse = warehouse ?? throw new ArgumentNullException(nameof(warehouse));
        }

        public void Add(Good good, int count)
        {
            if (_warehouse.CheckAvailability(good, count) == false)
                throw new InvalidOperationException("Нет нужного количества товара на складе");

            if (_productsCart.ContainsKey(good))
            {
                _productsCart[good] += count;
            }
            else
            {
                _productsCart.Add(good, count);
            }
        }

        public Order Order()
        {
            foreach (var good in _productsCart)
            {
                _warehouse.GiveProduct(good.Key, good.Value);
            }

            return new Order();
        }

        public void ShowProductsCart()
        {
            Console.WriteLine("Товары в корзине:");

            foreach (var good in _productsCart)
            {
                Console.WriteLine($"Товар {good.Key.Name}. Количество {good.Value}");
            }
        }
    }

    class Order
    {
        public string Paylink { get; private set; }
    }

    class Warehouse : IWarehouse
    {
        private readonly Dictionary<Good, int> _goods = new Dictionary<Good, int>();

        public void Delive(Good good, int count)
        {
            _goods.Add(good, count);
        }

        public bool CheckAvailability(Good good, int count) => count <= _goods[good];

        public void GiveProduct(Good good, int count)
        {
            _goods[good] -= count;
        }

        public void SoShowProductsStock()
        {
            foreach (var good in _goods)
            {
                Console.WriteLine($"Товар {good.Key.Name}. Количество {good.Value}");
            }
        }
    }

    interface IWarehouse
    {
        void GiveProduct(Good good, int count);

        bool CheckAvailability(Good good, int count);
    }

    class Good
    {
        private string _name;

        public Good(string name)
        {
            _name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public string Name => _name;
    }
}
