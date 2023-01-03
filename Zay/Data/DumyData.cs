using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using Zay.Models;

namespace Zay.Data
{
    public  class  DumyData
    {
        
        public static async Task<Boolean> Initialize( UserManager<ApplicationUser> userManager)
        {
            
            string Name = "Zdm Information Technology";
            string userEmail = "zdm@gmail.com";
            string userPassword = "Zdm@gmail.comp@$$word007";
           
            if(await userManager.FindByEmailAsync(userEmail) == null)
            {

                var user = new ApplicationUser { Name = Name, UserName = userEmail, Email = userEmail };
                var result = await userManager.CreateAsync(user, userPassword);
                if(result.Succeeded)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
