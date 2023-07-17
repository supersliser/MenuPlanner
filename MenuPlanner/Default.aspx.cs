using Postgrest.Attributes;
using Postgrest.Models;
using Supabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MenuPlanner
{
    public partial class _Default : Page
    {
        public Client client;

        protected void Page_Load(object sender, EventArgs e)
        {
            var options = new Supabase.SupabaseOptions
            {
                AutoConnectRealtime = true
            };
            client = new Client(@"https://avmnzaqsnpbhciybmtrw.supabase.co", @"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImF2bW56YXFzbnBiaGNpeWJtdHJ3Iiwicm9sZSI6InNlcnZpY2Vfcm9sZSIsImlhdCI6MTY4OTUzODQ5NCwiZXhwIjoyMDA1MTE0NDk0fQ.2BoniwHN3ooSeb8Ci4LzruwUZ_P-x1F9U3BYubdsbUU", options);
        }

        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {
            
        }
    }

    [Table("Date")]
    public class supabaseDate : BaseModel
    {
        [PrimaryKey("Date")]
        public DateTime Date { set; get; }

        [PrimaryKey("PersonID")]
        public long PersonID { set; get; }

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
        [PrimaryKey("id")]
        public long MealID { set; get; }

        [Column("Name")]
        public string Name { set; get; }

        [Column("Ease")]
        public short Ease { set; get; }
    }

    [Table("Meal/Ingredient")]
    public class supabaseMealIngredient : BaseModel
    {
        [PrimaryKey("MealID")]
        public long MealID { set; get; }

        [PrimaryKey("IngredientID")]
        public long IngredientID { set; get; }

        [Column("Optional")]
        public bool Optional { set; get; }
    }

    [Table("Person")]
    public class supabasePerson : BaseModel
    {

    }
}