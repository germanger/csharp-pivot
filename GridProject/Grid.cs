using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GridProject
{
    class Grid
    {
        private List<Row> originalData;
        private List<Row> groupedData;
        private ColumnDefinition[] columnDefinitions;
        private string[] pivotColumns;
        Dictionary<string, List<object>> pivotValues;
        private string[] aggregatedColumns;

        public Grid(List<Row> originalData,
                    ColumnDefinition[] columnDefinitions,
                    string[] pivotColumns,
                    string[] aggregatedColumns)
        {
            this.originalData = originalData;
            this.columnDefinitions = columnDefinitions;
            this.pivotColumns = pivotColumns;
            this.aggregatedColumns = aggregatedColumns;
            this.groupedData = new List<Row>();
            this.pivotValues = new Dictionary<string, List<object>>();

            // Initialize pivotValues
            foreach (string column in pivotColumns)
            {
                pivotValues[column] = new List<object>();
            }

            // Get values for each pivot
            foreach (string column in pivotColumns)
            {
                foreach (var row in originalData)
                {
                    if (!row.Fields.ContainsKey(column))
                    {
                        throw new Exception("Column '" + column + "' doesn't exist");
                    }

                    // Value already saved
                    if (pivotValues[column].Contains(row.Fields[column]))
                    {
                        continue;
                    }

                    // Save value
                    pivotValues[column].Add(row.Fields[column]);
                }
            }

            // Create groups
            string firstPivotColumn = pivotValues.Keys.FirstOrDefault();

            foreach (object val in pivotValues[firstPivotColumn])
            {
                groupedData.Add(AsGroup(firstPivotColumn, val, new Dictionary<string, object>()));
            }

            // Calculate aggregated columns
            foreach (string column in aggregatedColumns)
            {
                foreach (var row in groupedData)
                {
                    row.Fields[column] = GetColumnSum(row, column);
                }
            }
        }

        public string GetHtml()
        {
            string html = "<table>\n";

            html = html + "   <tr class='header'>\n";

            // Header
            foreach (ColumnDefinition columnDefinition in columnDefinitions)
            {
                html = html + "      <th>" + columnDefinition.HeaderName + "</th>\n";
            }

            html = html + "   </tr>\n";

            // Data
            foreach (var row in groupedData)
            {
                html = html + RowAsHtml(row);
            }

            html = html + "</table>";

            return html;
        }

        private string RowAsHtml(Row row)
        {
            string html = "   <tr class='" + (row.Children.Any() ? "tr-group" : "") + "'>\n";

            foreach (ColumnDefinition columnDefinition in columnDefinitions)
            {
                if (row.Fields.ContainsKey(columnDefinition.Field))
                {
                    html = html + "      <td class='" + columnDefinition.CssClass + "'>" + row.Fields[columnDefinition.Field] + "</td>\n";
                }
                else
                {
                    html = html + "      <td></td>\n";
                }
            }

            html = html + "   </tr>\n";

            if (row.Children.Any())
            {
                foreach (var child in row.Children)
                {
                    html = html + RowAsHtml(child);
                }
            }

            return html;
        }

        private Row AsGroup(string key, object val, Dictionary<string, object> inheritedProperties)
        {
            var group = new Row();
            var children = new List<Row>();

            group.Fields[key] = val;
            group.Children = children;

            foreach (KeyValuePair<string, object> kvp in inheritedProperties)
            {
                group.Fields[kvp.Key] = kvp.Value;
            }

            // Last pivot column contains the actual data
            if (pivotValues.Keys.LastOrDefault().Equals(key))
            {
                foreach (var row in originalData)
                {
                    if (BelongsToGroup(row, group))
                    {
                        children.Add(row);
                    }
                }

                return group;
            }

            int nextKeyIndex = pivotValues.Keys.ToList().IndexOf(key) + 1;
            string nextKey = pivotValues.Keys.ToList()[nextKeyIndex];

            foreach (object val2 in pivotValues[nextKey])
            {
                children.Add(AsGroup(nextKey, val2, group.Fields));
            }

            return group;
        }

        private bool BelongsToGroup(Row row, Row group)
        {
            foreach (KeyValuePair<string, object> kvp in group.Fields)
            {
                if (row.Fields[kvp.Key] != kvp.Value)
                {
                    return false;
                }
            }

            return true;
        }

        private decimal GetColumnSum(Row row, string column)
        {
            // No children
            if (!row.Children.Any())
            {
                Type concreteType = (row.Fields[column]).GetType();

                return Convert.ToDecimal(Convert.ChangeType(row.Fields[column], concreteType));
            }

            decimal sum = 0;

            foreach (var child in row.Children)
            {
                sum = sum + GetColumnSum(child, column);
            }

            row.Fields[column] = sum;

            return sum;
        }
    }


}
