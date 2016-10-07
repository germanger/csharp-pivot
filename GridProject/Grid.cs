using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PivotDataProject
{
    class Grid
    {
        private List<Dictionary<string, object>> originalData;
        private List<Dictionary<string, object>> groupedData;
        private ColumnDefinition[] columnDefinitions;
        private string[] pivotColumns;
        Dictionary<string, List<object>> pivotValues;
        private string[] aggregatedColumns;

        public Grid(List<Dictionary<string, object>> originalData,
                    ColumnDefinition[] columnDefinitions,
                    string[] pivotColumns,
                    string[] aggregatedColumns)
        {
            this.originalData = originalData;
            this.columnDefinitions = columnDefinitions;
            this.pivotColumns = pivotColumns;
            this.aggregatedColumns = aggregatedColumns;
            this.groupedData = new List<Dictionary<string, object>>();
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
                    if (!row.ContainsKey(column))
                    {
                        throw new Exception("Column '" + column + "' doesn't exist");
                    }

                    // Value already saved
                    if (pivotValues[column].Contains(row[column]))
                    {
                        continue;
                    }

                    // Save value
                    pivotValues[column].Add(row[column]);
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
                    row[column] = GetColumnSum(row, column);
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

        private string RowAsHtml(Dictionary<string, object> row)
        {
            List<Dictionary<string, object>> children = new List<Dictionary<string, object>>();

            if (row.ContainsKey("Children"))
            {
                children = (List<Dictionary<string, object>>)row["Children"];
            }

            string html = "   <tr class='" + (children.Any() ? "tr-group" : "") + "'>\n";

            foreach (ColumnDefinition columnDefinition in columnDefinitions)
            {
                if (row.ContainsKey(columnDefinition.Field))
                {
                    html = html + "      <td class='" + columnDefinition.CssClass + "'>" + row[columnDefinition.Field] + "</td>\n";
                }
                else
                {
                    html = html + "      <td></td>\n";
                }
            }

            html = html + "   </tr>\n";

            if (children.Any())
            {
                foreach (var child in children)
                {
                    html = html + RowAsHtml(child);
                }
            }

            return html;
        }

        private Dictionary<string, object> AsGroup(string key, object val, Dictionary<string, object> inheritedProperties)
        {
            var node = new Dictionary<string, object>();
            var children = new List<Dictionary<string, object>>();

            node[key] = val;
            node["Children"] = children;

            foreach (KeyValuePair<string, object> kvp in inheritedProperties)
            {
                if (kvp.Key.Equals("Children"))
                {
                    continue;
                }

                node[kvp.Key] = kvp.Value;
            }

            // Last pivot column contains the actual data
            if (pivotValues.Keys.LastOrDefault().Equals(key))
            {
                foreach (var item in originalData)
                {
                    if (BelongsToGroup(item, node))
                    {
                        children.Add(item);
                    }
                }

                return node;
            }

            int nextKeyIndex = pivotValues.Keys.ToList().IndexOf(key) + 1;
            string nextKey = pivotValues.Keys.ToList()[nextKeyIndex];

            foreach (object val2 in pivotValues[nextKey])
            {
                children.Add(AsGroup(nextKey, val2, node));
            }

            return node;
        }

        private bool BelongsToGroup(Dictionary<string, object> item, Dictionary<string, object> group)
        {
            foreach (KeyValuePair<string, object> kvp in group)
            {
                if (kvp.Key.Equals("Children")) 
                {
                    continue;
                }

                if (item[kvp.Key] != kvp.Value)
                {
                    return false;
                }
            }

            return true;
        }

        private decimal GetColumnSum(Dictionary<string, object> row, string column)
        {
            var children = new List<Dictionary<string, object>>();

            if (row.ContainsKey("Children"))
            {
                children = (List<Dictionary<string, object>>)row["Children"];
            }

            // No children
            if (!children.Any())
            {
                Type concreteType = (row[column]).GetType();

                return Convert.ToDecimal(Convert.ChangeType(row[column], concreteType));
            }

            decimal sum = 0;

            foreach (var child in children)
            {
                sum = sum + GetColumnSum(child, column);
            }

            row[column] = sum;

            return sum;
        }
    }


}
