using Antlr.Runtime;
using Postgrest.Attributes;
using Postgrest.Models;
using Supabase;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web.Management;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MenuPlanner
{
    public partial class _Default : Page
    {
        public Client client;

        public static List<Meal> RandomizeList(List<Meal> list)
        {
            Random random = new Random();
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                // Swap the items at positions i and j
                Meal temp = list[i];
                list[i] = list[j];
                list[j] = temp;
            }

            return list;
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            var options = new Supabase.SupabaseOptions
            {
                AutoConnectRealtime = true
            };
            client = new Client(@"https://avmnzaqsnpbhciybmtrw.supabase.co", @"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImF2bW56YXFzbnBiaGNpeWJtdHJ3Iiwicm9sZSI6InNlcnZpY2Vfcm9sZSIsImlhdCI6MTY4OTUzODQ5NCwiZXhwIjoyMDA1MTE0NDk0fQ.2BoniwHN3ooSeb8Ci4LzruwUZ_P-x1F9U3BYubdsbUU", options);
        }

        protected async Task GenerateItems(DateTime Today)
        {
            Meal[] days = new Meal[4];
            var request = await client
                .From<supabaseDate>()
                .Select("*")
                .Order(x => x.Date, Postgrest.Constants.Ordering.Descending)
                .Get();
            for (int i = 0; i <= 3; i++)
            {
                var test = request.Models[i].MealID;
                var MealRequest = await client
                    .From<supabaseMeal>()
                    .Select("*")
                    .Where(x => x.MealID == test)
                    .Get();
                var MealIngredientRequest = await client
                    .From<supabaseMealIngrediant>()
                    .Select("*")
                    .Where(x => x.MealID == test)
                    .Get();

                days[i] = new Meal();

                days[i].MealItem = MealRequest.Model;
                days[i].Ingredients = new List<ingredient>();

                foreach (var ingredientID in MealIngredientRequest.Models)
                {
                    var IngredientRequest = await client
                        .From<supabaseIngredient>()
                        .Select("*")
                        .Where(x => x.IngredientID == ingredientID.IngredientID)
                        .Get();
                    await days[i].AddIngredient(client, IngredientRequest.Model);
                }
            }



            List<Meal> meals = new List<Meal>();
            var MealItems = await client
                .From<supabaseMeal>()
                .Select("*")
                .Get();
            foreach (var item in MealItems.Models)
            {
                Meal temp = new Meal();
                temp.MealItem = item;
                var MealIngredientItems = await client
                    .From<supabaseMealIngrediant>()
                    .Select("*")
                    .Where(x => x.MealID == item.MealID)
                    .Get();
                foreach (var id in MealIngredientItems.Models)
                {
                    var IngredientItem = await client
                        .From<supabaseIngredient>()
                        .Select("*")
                        .Where(x => x.IngredientID == id.IngredientID)
                        .Get();
                    await temp.AddIngredient(client, IngredientItem.Model);
                }
                meals.Add(temp);
            }
            meals = RandomizeList(meals);


            int redMeatCount = 0;
            int whiteMeatCount = 0;
            int vegetableCount = 0;
            int pastaCount = 0;
            int fishCount = 0;

            foreach (Meal item in days)
            {
                bool rm = false;
                bool wm = false;
                bool vg = false;
                bool pa = false;
                bool fs = false;
                foreach (var item2 in item.Ingredients)
                {
                    switch (item2.Attribute)
                    {
                        case "Red Meat":
                            if (rm == false)
                            {
                                redMeatCount++;
                                rm = true;
                            }
                            break;
                        case "White Meat":
                            if (wm == false)
                            {
                                whiteMeatCount++;
                                wm = true;
                            }
                            break;
                        case "Vegetable":
                            if (vg == false)
                            {
                                vegetableCount++;
                                vg = true;
                            }
                            break;
                        case "Pasta":
                            if (pa == false)
                            {
                                pastaCount++;
                                pa = true;
                            }
                            break;
                        case "Fish":
                            if (fs == false)
                            {
                                fishCount++;
                                fs = true;
                            }
                            break;
                    }
                }
            }



            Meal finalMeal = new Meal();
            var count = 0;
            int failCount = 0;
            const int failMax = 1;
            do
            {
                failCount = 0;
                var tempMeal = meals[count];
                if (days.Contains(tempMeal)) 
                { 
                    failCount+= failMax; 
                }
                if (Today.DayOfWeek == DayOfWeek.Sunday && !(tempMeal.MealItem.MealID == 2 || tempMeal.MealItem.MealID == 26 || tempMeal.MealItem.MealID == 27 || tempMeal.MealItem.MealID == 28 || tempMeal.MealItem.MealID == 29 || tempMeal.MealItem.MealID == 30))
                {
                    failCount += failMax;
                }
                //if (Today.DayOfWeek != DayOfWeek.Sunday && (tempMeal.MealItem.MealID == 2 || tempMeal.MealItem.MealID == 26 || tempMeal.MealItem.MealID == 27 || tempMeal.MealItem.MealID == 28 || tempMeal.MealItem.MealID == 29 || tempMeal.MealItem.MealID == 30))
                //{
                //    failCount += failMax;
                //}


                bool rm = false;
                bool wm = false;
                bool vg = false;
                bool pa = false;
                bool fs = false;
                foreach (var item in tempMeal.Ingredients)
                {

                    if (item.Attribute == "Red Meat" && redMeatCount >= 2)
                    {
                        if (rm == false)
                        {
                            failCount++;
                            rm = true;
                        }
                    }
                    if (item.Attribute == "White Meat" && whiteMeatCount >= 2) 
                    {
                        if (wm == false)
                        {
                            failCount++;
                            wm = true;
                        }
                    }
                    //if (item.Attribute == "Vegetable" && vegetableCount <= 3) 
                    //{
                    //    if (vg == false)
                    //    {
                    //        failCount++;
                    //        vg = true;
                    //    }
                    //}
                    if (item.Attribute == "Pasta" && pastaCount >= 1)
                    {
                        if (pa == false)
                        {
                            failCount++;
                            pa = true;
                        }
                    }
                    if (item.Attribute == "Fish" && fishCount >= 1) 
                    {
                        if (fs == false)
                        {
                            failCount++;
                            fs = true;
                        }
                    }
                }
                count++;
            }
            while (failCount >= failMax);

            if (failCount < failMax)
            {
                finalMeal = meals[count - 1];
            }
            string date = Today.ToString("yyyy-MM-dd");
            supabaseDate supabaseDate = new supabaseDate()
            {
                Date = date,
                MealID = finalMeal.MealItem.MealID,
            };
            await client
                .From<supabaseDate>()
                .Upsert(supabaseDate);
        }

        protected async void Calendar1_SelectionChanged(object sender, EventArgs e)
        {
            bool attempt = false;
            do
            {
                List<supabaseDate> items = new List<supabaseDate>();
                string test = Calendar1.SelectedDate.Date.ToString("yyyy-MM-dd");
                var request = await client
                    .From<supabaseDate>()
                    .Select("*")
                    .Where(x => x.Date == test)
                    .Get();
                if (request.Models.Count != 0)
                {
                    items = request.Models;
                    foreach (var item in items)
                    {
                        var mealRequest = await client
                            .From<supabaseMeal>()
                            .Select("*")
                            .Where(x => x.MealID == item.MealID)
                            .Get();
                        MealItem.Text = mealRequest.Model.Name;
                    }
                    attempt = true;
                }
                else
                {
                    await GenerateItems(Calendar1.SelectedDate.Date);
                }
            }
            while (attempt == false);
        }

        [Table("Date")]
        public class supabaseDate : BaseModel
        {
            [PrimaryKey("DateItem")]
            public string Date { set; get; }

            [Column("MealID")]
            public long MealID { set; get; }
        }

        [Table("Attribute")]
        public class supabaseAttribute : BaseModel
        {
            [PrimaryKey("id")]
            public long AttributeID { set; get; }

            [Column("Name")]
            public string Name { set; get; }
        }

        [Table("Ingredient")]
        public class supabaseIngredient : BaseModel
        {
            [PrimaryKey("id")]
            public long IngredientID { set; get; }

            [Column("Name")]
            public string Name { set; get; }

            [Column("Attribute")]
            public long AttributeID { set; get; }
        }

        [Table("Meal")]
        public class supabaseMeal : BaseModel
        {
            [PrimaryKey("mealID")]
            public long MealID { set; get; }

            [Column("Name")]
            public string Name { set; get; }

            [Column("Ease")]
            public short Ease { set; get; }
        }

        [Table("MealIngredient")]
        public class supabaseMealIngrediant : BaseModel
        {
            [PrimaryKey("Meal")]
            public long MealID { set; get; }

            [PrimaryKey("Ingredient")]
            public long IngredientID { set; get; }
        }

        public class Meal
        {
            public supabaseMeal MealItem;
            public List<ingredient> Ingredients;

            public async Task AddIngredient(Client client, supabaseIngredient ingredientItem)
            {
                if (Ingredients == null)
                {
                    Ingredients = new List<ingredient>();
                }
                ingredient temp = new ingredient();
                await temp.Set(client, ingredientItem);
                Ingredients.Add(temp);
            }
        }
        public class ingredient
        {
            public string Ingredient;
            public string Attribute;

            public async Task Set(Client client, supabaseIngredient ingredient)
            {
                Ingredient = ingredient.Name;
                var AttributeID = ingredient.AttributeID;
                var request = await client
                    .From<supabaseAttribute>()
                    .Select("*")
                    .Where(x => x.AttributeID == AttributeID)
                    .Get();
                Attribute = request.Model.Name;
            }
        }

        protected async void Button1_Click(object sender, EventArgs e)
        {
            for (DateTime i = DateTime.Today; 
                i != DateTime.Today.AddDays(7); 
                i = i.AddDays(1))
            {
                string test = i.Date.ToString("yyyy-MM-dd");
                var request = await client
                    .From<supabaseDate>()
                    .Select("*")
                    .Where(x => x.Date == test)
                    .Get();
                if (request.Model == null)
                {
                    await GenerateItems(i);
                }
            }
        }

        protected async void Button2_Click(object sender, EventArgs e)
        {
            for (DateTime i = DateTime.Today.Date; i != DateTime.Today.Date.AddDays(28); i = i.AddDays(1))
            {
                string test = i.Date.ToString("yyyy-MM-dd");
                var request = await client
                    .From<supabaseDate>()
                    .Select("*")
                    .Where(x => x.Date == test)
                    .Get();
                if (request.Model == null)
                {
                    await GenerateItems(i);
                }
            }
        }
    }
}