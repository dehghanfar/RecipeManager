using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RecipeManager.Models;


namespace RecipeManager
{
    public class MySession
    {
        public static List<RecipeModel> MySessionRecipeModel
        {
            get
            {
                var session =
                  (List < RecipeModel>)HttpContext.Current.Session["MyStoredSessionRecipeModel"];
                if (session == null)
                {
                    session = new List<RecipeModel>();
                    HttpContext.Current.Session["MyStoredSessionRecipeModel"] = session;
                }
                return session;
            }
        }
    }
}