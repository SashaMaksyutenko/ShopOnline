using System;
using Microsoft.EntityFrameworkCore;
using ShopOnline.Api.Data;
using ShopOnline.Api.Entities;
using ShopOnline.Api.Repositories.Contracts;
using ShopOnline.Models;
using ShopOnline.Models.Dtos;

namespace ShopOnline.Api.Repositories
{
    public class ShoppingCartRepository : IShoppingCartRepository 
    {
        private readonly ShopOnlineDBContext shopOnlineDBContext;

        public ShoppingCartRepository(ShopOnlineDBContext shopOnlineDBContext)
        {
            this.shopOnlineDBContext = shopOnlineDBContext;
        }
        private async Task<bool> CartItemExists(int cartId, int productId)
        {
            return await this.shopOnlineDBContext.CartItems.AnyAsync(c => c.CartId == cartId && c.ProductId == productId);
        } 
        public async Task<CartItem> AddItem(CartItemToAddDto cartItemToAddDto)
        {
            if (await CartItemExists(cartItemToAddDto.CartId, cartItemToAddDto.ProductId) == false)
            {
                var item = await (from product in this.shopOnlineDBContext.Products
                                  where product.Id == cartItemToAddDto.ProductId
                                  select new CartItem
                                  {
                                      CartId = cartItemToAddDto.CartId,
                                      ProductId = product.Id,
                                      Qty = cartItemToAddDto.Qty,

                                  }).SingleOrDefaultAsync();
                if (item != null)
                {
                    var result = await this.shopOnlineDBContext.CartItems.AddAsync(item);
                    await this.shopOnlineDBContext.SaveChangesAsync();
                    return result.Entity;
                }
            }
            return null;

        }

        public async Task<CartItem> DeleteItem(int id)
        {
            var item = await this.shopOnlineDBContext.CartItems.FindAsync(id);
            if (item != null)
            {
                this.shopOnlineDBContext.CartItems.Remove(item);
                await this.shopOnlineDBContext.SaveChangesAsync();
            }
            return item;
        }

        public async Task<CartItem> GetItem(int id)
        {
            return await (from cart in this.shopOnlineDBContext.Carts
                          join cartItem in this.shopOnlineDBContext.CartItems
                          on cart.Id equals cartItem.CartId
                          where cartItem.Id == id
                          select new CartItem
                          {
                              Id = cartItem.Id,
                              ProductId = cartItem.ProductId,
                              Qty = cartItem.Qty,
                              CartId = cartItem.CartId
                          }).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<CartItem>> GetItems(int userId)
        {
            return await (from cart in this.shopOnlineDBContext.Carts
                          join cartItem in this.shopOnlineDBContext.CartItems
                          on cart.Id equals cartItem.CartId
                          where cart.UserId == userId
                          select new CartItem
                          {
                              Id = cartItem.Id,
                              ProductId = cartItem.ProductId,
                              Qty = cartItem.Qty,
                              CartId = cartItem.CartId
                          }).ToListAsync();
        }

        public async Task<CartItem> UpdateQty(int id, CartItemQtyUpdateDto cartItemQtyUpdateDto)
        {
            var item = await this.shopOnlineDBContext.CartItems.FindAsync(id);
            if(item!=null)
            {
                item.Qty = cartItemQtyUpdateDto.Qty;
                await this.shopOnlineDBContext.SaveChangesAsync();
                return item;
            }
            return null;
        }
    }
}

