﻿using MixERP.Net.Common;
using MixERP.Net.Common.Helpers;

/********************************************************************************
Copyright (C) Binod Nepal, Mix Open Foundation (http://mixof.org).

This file is part of MixERP.

MixERP is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

MixERP is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with MixERP.  If not, see <http://www.gnu.org/licenses/>.
***********************************************************************************/

using System;
using System.Threading;
using System.Web;
using System.Web.UI.WebControls;

namespace MixERP.Net.WebControls.ReportEngine
{
    public partial class Report //: CompositeControl
    {
        public void ExcelImageButton_Click(object sender, EventArgs e)
        {
            //EnsureChildControls();
            string html = this.reportHidden.Value;
            if (!string.IsNullOrWhiteSpace(html))
            {
                this.Page.Response.ContentType = "application/force-download";
                this.Page.Response.AddHeader("content-disposition", "attachment; filename=" + this.reportTitleHidden.Value + ".xls");
                this.Page.Response.Charset = "";
                this.Page.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                this.Page.Response.ContentType = "application/vnd.ms-excel";
                this.Page.Response.Write(html);
                this.Page.Response.Flush();
                this.Page.Response.Close();
            }
        }

        public void WordImageButton_Click(object sender, EventArgs e)
        {
            //EnsureChildControls();
            string html = this.reportHidden.Value;
            if (!string.IsNullOrWhiteSpace(html))
            {
                this.Page.Response.ContentType = "application/force-download";
                this.Page.Response.AddHeader("content-disposition", "attachment; filename=" + this.reportTitleHidden.Value + ".doc");
                this.Page.Response.Charset = "";
                this.Page.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                this.Page.Response.ContentType = "application/vnd.ms-word";
                this.Page.Response.Write(html);
                this.Page.Response.Flush();
                this.Page.Response.Close();
            }
        }

        #region "GridView Events"

        private void GridView_DataBound(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;

            int arg = Conversion.TryCastInteger(grid.ID.Replace("GridView", ""));

            if (string.IsNullOrWhiteSpace(this.runningTotalFieldIndicesCollection[arg]))
            {
                return;
            }

            if (grid.FooterRow == null)
            {
                return;
            }

            grid.FooterRow.Visible = true;

            for (int i = 0; i < this.runningTotalTextColumnIndexCollection[arg]; i++)
            {
                grid.FooterRow.Cells[i].Visible = false;
            }

            grid.FooterRow.Cells[this.runningTotalTextColumnIndexCollection[arg]].ColumnSpan = this.runningTotalTextColumnIndexCollection[arg] + 1;
            grid.FooterRow.Cells[this.runningTotalTextColumnIndexCollection[arg]].Text = this.RunningTotalText;
            grid.FooterRow.Cells[this.runningTotalTextColumnIndexCollection[arg]].Style.Add("text-align", "right");
            grid.FooterRow.Cells[this.runningTotalTextColumnIndexCollection[arg]].Font.Bold = true;

            foreach (string field in this.runningTotalFieldIndicesCollection[arg].Split(','))
            {
                int index = Conversion.TryCastInteger(field.Trim());

                decimal total = 0;

                if (index > 0)
                {
                    foreach (GridViewRow row in grid.Rows)
                    {
                        if (row.RowType == DataControlRowType.DataRow)
                        {
                            total += Conversion.TryCastDecimal(row.Cells[index].Text);
                        }
                    }

                    grid.FooterRow.Cells[index].Text = string.Format(Thread.CurrentThread.CurrentCulture, "{0:N}", total);
                    grid.FooterRow.Cells[index].Font.Bold = true;
                }
            }
        }

        private void GridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
                {
                    string cellText = e.Row.Cells[i].Text;

                    cellText = LocalizationHelper.GetResourceString("ScrudResource", cellText, false);
                    e.Row.Cells[i].Text = cellText;
                    e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Left;
                }
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                GridView grid = (GridView)sender;
                int arg = Conversion.TryCastInteger(grid.ID.Replace("GridView", ""));

                //Apply formatting on decimal fields
                if (string.IsNullOrWhiteSpace(this.decimalFieldIndicesCollection[arg]))
                {
                    return;
                }

                string decimalFields = this.decimalFieldIndicesCollection[arg];
                foreach (string fieldIndex in decimalFields.Split(','))
                {
                    int index = Conversion.TryCastInteger(fieldIndex);
                    decimal value = Conversion.TryCastDecimal(e.Row.Cells[index].Text);
                    e.Row.Cells[index].Text = string.Format(Thread.CurrentThread.CurrentCulture, "{0:N}", value);
                }
            }
        }

        #endregion "GridView Events"
    }
}