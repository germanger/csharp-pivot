using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GridProject
{
    class Program
    {
        static void Main(string[] args)
        {
            var animals = new List<Row> 
            {
                new Row 
                {
                    Fields = new Dictionary<string, object> 
                    {
                        { "Name", "Cat" },
                        { "Family", "Felidae" },
                        { "Size", "Small" },
                        { "Quantity", 1 },
                    }
                },
                new Row
                {
                    Fields = new Dictionary<string, object> 
                    {
                        { "Name", "Lion" },
                        { "Family", "Felidae" },
                        { "Size", "Tall" },
                        { "Quantity", 1 },
                    }
                },
                new Row
                {
                    Fields = new Dictionary<string, object> 
                    {
                        { "Name", "Tiger" },
                        { "Family", "Felidae" },
                        { "Size", "Tall" },
                        { "Quantity", 1 },
                    }
                },
                new Row
                {
                    Fields = new Dictionary<string, object> 
                    {
                        { "Name", "Dog" },
                        { "Family", "Canidae" },
                        { "Size", "Small" },
                        { "Quantity", 1 },
                    }
                },
                new Row
                {
                    Fields = new Dictionary<string, object> 
                    {
                        { "Name", "Wolf" },
                        { "Family", "Canidae" },
                        { "Size", "Tall" },
                        { "Quantity", 1 },
                    }
                },
                new Row
                {
                    Fields = new Dictionary<string, object> 
                    {
                        { "Name", "Racoon" },
                        { "Family", "Canidae" },
                        { "Size", "Small" },
                        { "Quantity", 1 },
                    }
                },
                new Row
                {
                    Fields = new Dictionary<string, object> 
                    {
                        { "Name", "Fox" },
                        { "Family", "Canidae" },
                        { "Size", "Small" },
                        { "Quantity", 1 },
                    }
                },
            };

            Grid grid = new Grid(originalData: animals,
                columnDefinitions: new ColumnDefinition[] { 
                    new ColumnDefinition { HeaderName = "Family", Field = "Family", CssClass = "", Width = 100 },
                    new ColumnDefinition { HeaderName = "Name", Field = "Name", CssClass = "", Width = 100 },
                    new ColumnDefinition { HeaderName = "Size", Field = "Size", CssClass = "", Width = 100 },
                    new ColumnDefinition { HeaderName = "Quant", Field = "Quantity", CssClass = "align-right", Width = 40 }
                },
                pivotColumns: new string[] { "Size", "Family" },
                aggregatedColumns: new string[] { "Quantity" });

            string html = grid.GetHtml();

            Console.WriteLine(html);
            Console.ReadLine();


        }


    }
}
