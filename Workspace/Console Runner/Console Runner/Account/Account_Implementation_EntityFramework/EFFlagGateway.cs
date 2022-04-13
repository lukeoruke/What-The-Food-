﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Runner.AccountService
{ 
    public class EFFlagGateway : IFlagGateway
    {
        private ContextAccountDB _efContext;

        public EFFlagGateway()
        {
            _efContext = new ContextAccountDB();
        }
        public async Task<bool> AccountHasFlagAsync(int userID, int ingredientID)
        {
            //FoodFlag foodFlag = new(userID, ingredientID);
            return await _efContext.FoodFlags.FindAsync(userID, ingredientID) != null;
        }

        public async Task<bool> AddFlagAsync(FoodFlag flag)
        {
            try
            {
               await _efContext.FoodFlags.AddAsync(flag);
               await _efContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<List<FoodFlag>> GetNAccountFlags(int userID, int skip, int take)
        {
            List<FoodFlag> results =  await _efContext.FoodFlags.Where(x => x.UserID == userID).
                OrderBy(x => x.IngredientID).Skip(skip).Take(take).ToListAsync();
            return results;
        }
        public List<FoodFlag> GetAllAccountFlags(int userID)
        {
            List<FoodFlag> flagList = new List<FoodFlag>();
            foreach (var flag in _efContext.FoodFlags)
            {
                if (flag.UserID == userID)
                {
                    flagList.Add(flag);
                }
            }
            return flagList;
        }

        public async Task<bool> RemoveFoodFlagAsync(int userID, int ingredientID)
        {
            try
            {
                FoodFlag foodFlag = new(userID, ingredientID);
                _efContext.FoodFlags.Remove(foodFlag);
                await _efContext.SaveChangesAsync();
                return true;
            }catch(Exception ex)
            {
                throw new Exception("Remove flag failed: " + ex.ToString);
            }
                

        }
    }
}
