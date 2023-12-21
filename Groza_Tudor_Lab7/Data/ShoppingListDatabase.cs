using System;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;
using Groza_Tudor_Lab7.Models;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Groza_Tudor_Lab7.Data
{
    public class ShoppingListDatabase
    {
        readonly SQLiteAsyncConnection _database;
        private object product;

        public ShoppingListDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<ShopList>().Wait();
            _database.CreateTableAsync<Product>().Wait();
            _database.CreateTableAsync<ListProduct>().Wait();
            _database.CreateTableAsync<Shop>().Wait();
        }

        public Task<List<ShopList>> GetShopListsAsync()
        {
            return _database.Table<ShopList>().ToListAsync();
        }
        public Task<ShopList> GetShopListAsync(int id)
        {
            return _database.Table<ShopList>()
            .Where(i => i.ID == id)
           .FirstOrDefaultAsync();
        }
        public Task<int> SaveShopListAsync(ShopList slist)
        {
            if (slist.ID != 0)
            {
                return _database.UpdateAsync(slist);
            }
            else
            {
                return _database.InsertAsync(slist);
            }
        }

        public Task<int> SaveProductAsync(Product product)
        {
            if (product.ID != 0)
            {
                return _database.UpdateAsync(product);
            }
            else
            {
                return _database.InsertAsync(product);
            }
        }

        public Task<int> DeleteShopListAsync(ShopList slist)
        {
            return _database.DeleteAsync(product);

        }

        public Task<List<Product>> GetProductsAsync()
        {
            return _database.Table<Product>().ToListAsync();
        }

        public Task<int> SaveListProductAsync(ListProduct listp)
        {
            if (listp.ID != 0)
            {
                return _database.UpdateAsync(listp);
            }
            else
            {
                return _database.InsertAsync(listp);
            }
        }
        public Task<List<Product>> GetListProductsAsync(int shoplistid)
        {
            return _database.QueryAsync<Product>(
            "select P.ID, P.Description from Product P"
            + " inner join ListProduct LP"
            + " on P.ID = LP.ProductID where LP.ShopListID = ?",
            shoplistid);
        }

        internal Task DeleteProductAsync(Product product)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteItemFromShopListAsync(int productId, int shopListId)
        {
            return _database.Table<ListProduct>()
                .Where(lp => lp.ProductID == productId && lp.ShopListID == shopListId)
                .DeleteAsync();
        }

        public Task<List<Shop>> GetShopsAsync()
        {
            return _database.Table<Shop>().ToListAsync();
        }

        public Task<int> SaveShopAsync(Shop shop)
        {
            if (shop.ID != 0)
            {
                return _database.UpdateAsync(shop);
            }
            else
            {
                return _database.InsertAsync(shop);
            }
        }
    }
}