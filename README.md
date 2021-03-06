# csharp-pivot
Pivot data with aggregation and output to HTML

## Example with one pivot

Say your data is a list of animals

<table>
   <tr class='header'>
      <th>Family</th>
      <th>Name</th>
      <th>Size</th>
      <th>Quantity</th>
   </tr>
   <tr class=''>
      <td class=''>Felidae</td>
      <td class=''>Cat</td>
      <td class=''>Small</td>
      <td class='align-right'>1</td>
   </tr>
   <tr class=''>
      <td class=''>Felidae</td>
      <td class=''>Lion</td>
      <td class=''>Tall</td>
      <td class='align-right'>1</td>
   </tr>
   <tr class=''>
      <td class=''>Felidae</td>
      <td class=''>Tiger</td>
      <td class=''>Tall</td>
      <td class='align-right'>1</td>
   </tr>
   <tr class=''>
      <td class=''>Canidae</td>
      <td class=''>Dog</td>
      <td class=''>Small</td>
      <td class='align-right'>1</td>
   </tr>
   <tr class=''>
      <td class=''>Canidae</td>
      <td class=''>Wolf</td>
      <td class=''>Tall</td>
      <td class='align-right'>1</td>
   </tr>
   <tr class=''>
      <td class=''>Canidae</td>
      <td class=''>Racoon</td>
      <td class=''>Small</td>
      <td class='align-right'>1</td>
   </tr>
   <tr class=''>
      <td class=''>Canidae</td>
      <td class=''>Fox</td>
      <td class=''>Small</td>
      <td class='align-right'>1</td>
   </tr>
</table>

1) First, represent your data as a `List` of `Row`'s

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
    
2) Initialize grid. <br>
In this example we are going to group by "Size"

       Grid grid = new Grid(originalData: animals,
           columnDefinitions: new ColumnDefinition[] { 
               new ColumnDefinition { HeaderName = "Family", Field = "Family", CssClass = "", Width = 100 },
               new ColumnDefinition { HeaderName = "Name", Field = "Name", CssClass = "", Width = 100 },
               new ColumnDefinition { HeaderName = "Size", Field = "Size", CssClass = "", Width = 100 },
               new ColumnDefinition { HeaderName = "Qty", Field = "Quantity", CssClass = "align-right", Width = 40 }
           },
           pivotColumns: new string[] { "Size" },
           aggregatedColumns: new string[] { "Quantity" });
        
3) Get html

       string html = grid.GetHtml();
    
4) Output:

<table>
   <tr class='header'>
      <th>Family</th>
      <th>Name</th>
      <th>Size</th>
      <th>Qty</th>
   </tr>
   <tr class='tr-group'>
      <td></td>
      <td></td>
      <td class=''>Small</td>
      <td class='align-right'>4</td>
   </tr>
   <tr class=''>
      <td class=''>Felidae</td>
      <td class=''>Cat</td>
      <td class=''>Small</td>
      <td class='align-right'>1</td>
   </tr>
   <tr class=''>
      <td class=''>Canidae</td>
      <td class=''>Dog</td>
      <td class=''>Small</td>
      <td class='align-right'>1</td>
   </tr>
   <tr class=''>
      <td class=''>Canidae</td>
      <td class=''>Racoon</td>
      <td class=''>Small</td>
      <td class='align-right'>1</td>
   </tr>
   <tr class=''>
      <td class=''>Canidae</td>
      <td class=''>Fox</td>
      <td class=''>Small</td>
      <td class='align-right'>1</td>
   </tr>
   <tr class='tr-group'>
      <td></td>
      <td></td>
      <td class=''>Tall</td>
      <td class='align-right'>3</td>
   </tr>
   <tr class=''>
      <td class=''>Felidae</td>
      <td class=''>Lion</td>
      <td class=''>Tall</td>
      <td class='align-right'>1</td>
   </tr>
   <tr class=''>
      <td class=''>Felidae</td>
      <td class=''>Tiger</td>
      <td class=''>Tall</td>
      <td class='align-right'>1</td>
   </tr>
   <tr class=''>
      <td class=''>Canidae</td>
      <td class=''>Wolf</td>
      <td class=''>Tall</td>
      <td class='align-right'>1</td>
   </tr>
</table>

## Example with multiple pivots

With the same source data, this:

       pivotColumns: new string[] { "Family", "Size" },
    
Will output this:

<table>
   <tr class='header'>
      <th>Family</th>
      <th>Name</th>
      <th>Size</th>
      <th>Qty</th>
   </tr>
   <tr class='tr-group'>
      <td class=''>Felidae</td>
      <td></td>
      <td></td>
      <td class='align-right'>3</td>
   </tr>
   <tr class='tr-group'>
      <td class=''>Felidae</td>
      <td></td>
      <td class=''>Small</td>
      <td class='align-right'>1</td>
   </tr>
   <tr class=''>
      <td class=''>Felidae</td>
      <td class=''>Cat</td>
      <td class=''>Small</td>
      <td class='align-right'>1</td>
   </tr>
   <tr class='tr-group'>
      <td class=''>Felidae</td>
      <td></td>
      <td class=''>Tall</td>
      <td class='align-right'>2</td>
   </tr>
   <tr class=''>
      <td class=''>Felidae</td>
      <td class=''>Lion</td>
      <td class=''>Tall</td>
      <td class='align-right'>1</td>
   </tr>
   <tr class=''>
      <td class=''>Felidae</td>
      <td class=''>Tiger</td>
      <td class=''>Tall</td>
      <td class='align-right'>1</td>
   </tr>
   <tr class='tr-group'>
      <td class=''>Canidae</td>
      <td></td>
      <td></td>
      <td class='align-right'>4</td>
   </tr>
   <tr class='tr-group'>
      <td class=''>Canidae</td>
      <td></td>
      <td class=''>Small</td>
      <td class='align-right'>3</td>
   </tr>
   <tr class=''>
      <td class=''>Canidae</td>
      <td class=''>Dog</td>
      <td class=''>Small</td>
      <td class='align-right'>1</td>
   </tr>
   <tr class=''>
      <td class=''>Canidae</td>
      <td class=''>Racoon</td>
      <td class=''>Small</td>
      <td class='align-right'>1</td>
   </tr>
   <tr class=''>
      <td class=''>Canidae</td>
      <td class=''>Fox</td>
      <td class=''>Small</td>
      <td class='align-right'>1</td>
   </tr>
   <tr class='tr-group'>
      <td class=''>Canidae</td>
      <td></td>
      <td class=''>Tall</td>
      <td class='align-right'>1</td>
   </tr>
   <tr class=''>
      <td class=''>Canidae</td>
      <td class=''>Wolf</td>
      <td class=''>Tall</td>
      <td class='align-right'>1</td>
   </tr>
</table>

## TO-DO:

- Format numbers
- Format dates
- Allow column grouping in header (eg. Column "Birth": Column "Day" + Column "Month" + Column "Year")
- Allow other types of aggregation: average, min, max
- Sorting
