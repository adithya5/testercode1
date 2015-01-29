﻿using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace MixERP.Net.WebControls.AttachmentFactory
{
    public sealed partial class Attachment
    {
        private void AddHeaderCell(TableHeaderRow row, string width, string text, string cssClass = "")
        {
            using (TableHeaderCell cell = new TableHeaderCell())
            {
                if (!string.IsNullOrWhiteSpace(width))
                {
                    cell.Style.Add("width", width);
                }

                if (!string.IsNullOrWhiteSpace(cssClass))
                {
                    cell.CssClass = cssClass;
                }

                using (HtmlGenericControl span = new HtmlGenericControl("span"))
                {
                    span.InnerText = text;
                    cell.Controls.Add(span);
                }

                row.Cells.Add(cell);
            }
        }

        private void CreateHeaderRow(Table table)
        {
            using (TableHeaderRow header = new TableHeaderRow())
            {
                header.TableSection = TableRowSection.TableHeader;

                this.AddHeaderCell(header, "340px", "Comment");
                this.AddHeaderCell(header, "50px", "Upload", "nopadding");
                this.AddHeaderCell(header, "240px", "Progress");
                this.AddHeaderCell(header, "", "File Path");

                table.Rows.Add(header);
            }
        }
    }
}