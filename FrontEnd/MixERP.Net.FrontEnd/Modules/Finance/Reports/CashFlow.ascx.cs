﻿/********************************************************************************
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
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using MixERP.Net.Common;
using MixERP.Net.Common.Extensions;
using MixERP.Net.Common.Helpers;
using MixERP.Net.Common.Models.Core;
using MixERP.Net.Core.Modules.Finance.Resources;
using MixERP.Net.FrontEnd.Base;
using MixERP.Net.WebControls.Common;

namespace MixERP.Net.Core.Modules.Finance.Reports
{
    public partial class CashFlow : MixERPUserControl
    {
        public override void OnControlLoad(object sender, EventArgs e)
        {
            this.CreateHeader(this.Placeholder1);
            this.CreateForm(this.Placeholder1);
            this.CreateGrid(this.Placeholder1);
            this.BindGrid();

            base.OnControlLoad(sender, e);
        }

        private void CreateHeader(Control container)
        {
            using (HtmlGenericControl header = new HtmlGenericControl("h2"))
            {
                header.InnerText = Titles.StatementOfCashFlows;
                container.Controls.Add(header);
            }
        }

        #region GridView

        private GridView cashflowStatementGridView;


        private void CashflowStatementGridViewDataBound(object sender, EventArgs eventArgs)
        {
            if (this.cashflowStatementGridView.HeaderRow != null)
            {
                this.cashflowStatementGridView.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        private void CashflowStatementGridViewRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Text = string.Empty;
            }


            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                for (int i = 1; i < e.Row.Cells.Count - 1; i++)
                {
                    e.Row.Cells[i].Text = e.Row.Cells[i].Text.ToFormattedNumber("{0:N}");
                    e.Row.Cells[i].CssClass = "text right";
                }
            }
        }

        private void CreateGrid(Control container)
        {
            using (HtmlGenericControl gridPanel = new HtmlGenericControl("div"))
            {
                gridPanel.Attributes.Add("class", "auto-overflow-panel");

                this.cashflowStatementGridView = new GridView();
                this.cashflowStatementGridView.ID = "CashFlowStatementGridView";
                this.cashflowStatementGridView.GridLines = GridLines.None;
                this.cashflowStatementGridView.CssClass = "ui definition celled segment table nowrap";
                this.cashflowStatementGridView.DataBound += this.CashflowStatementGridViewDataBound;
                this.cashflowStatementGridView.RowDataBound += this.CashflowStatementGridViewRowDataBound;

                gridPanel.Controls.Add(this.cashflowStatementGridView);

                container.Controls.Add(gridPanel);
            }
        }
        #endregion

        #region Form

        private HtmlInputText factorInputText;
        private DateTextBox fromDateTextBox;
        private Button showButton;
        private DateTextBox toDateTextBox;


        private void AddFactorField(HtmlGenericControl container)
        {
            using (HtmlGenericControl field = HtmlControlHelper.GetField())
            {
                using (HtmlGenericControl label = HtmlControlHelper.GetLabel(Titles.Factor, "FactorInputText"))
                {
                    field.Controls.Add(label);
                }
                this.factorInputText = new HtmlInputText();
                this.factorInputText.ID = "FactorInputText";
                this.factorInputText.Attributes.Add("class", "small input integer");
                this.factorInputText.Value = "1000";

                field.Controls.Add(this.factorInputText);

                container.Controls.Add(field);
            }
        }

        private void AddFromDateTextBoxField(HtmlGenericControl container)
        {
            using (HtmlGenericControl field = HtmlControlHelper.GetField())
            {
                using (HtmlGenericControl label = HtmlControlHelper.GetLabel(Titles.From, "FromDateTextBox"))
                {
                    field.Controls.Add(label);
                }
                this.fromDateTextBox = new DateTextBox();
                this.fromDateTextBox.ID = "FromDateTextBox";
                this.fromDateTextBox.Mode = Frequency.FiscalYearStartDate;
                field.Controls.Add(this.fromDateTextBox);

                container.Controls.Add(field);
            }
        }

        private void AddPrintButton(HtmlGenericControl container)
        {
            using (HtmlInputButton printButton = new HtmlInputButton())
            {
                printButton.ID = "PrintButton";
                printButton.Attributes.Add("class", "ui orange button");

                printButton.Value = Titles.Print;

                container.Controls.Add(printButton);
            }
        }

        private void AddShowButton(HtmlGenericControl container)
        {
            this.showButton = new Button();
            this.showButton.ID = "ShowButton";
            this.showButton.Text = Titles.Show;
            this.showButton.CssClass = "ui positive button";
            this.showButton.Click += this.ShowButton_Click;

            container.Controls.Add(this.showButton);
        }

        private void AddToDateTextBoxField(HtmlGenericControl container)
        {
            using (HtmlGenericControl field = HtmlControlHelper.GetField())
            {
                using (HtmlGenericControl label = HtmlControlHelper.GetLabel(Titles.To, "ToDateTextBox"))
                {
                    field.Controls.Add(label);
                }

                this.toDateTextBox = new DateTextBox();
                this.toDateTextBox.ID = "ToDateTextBox";
                this.toDateTextBox.Mode = Frequency.FiscalYearEndDate;

                field.Controls.Add(this.toDateTextBox);

                container.Controls.Add(field);
            }
        }

        private void BindGrid()
        {
            DateTime from = Conversion.TryCastDate(this.fromDateTextBox.Text);
            DateTime to = Conversion.TryCastDate(this.toDateTextBox.Text);
            decimal factor = Conversion.TryCastDecimal(this.factorInputText.Value);

            int userId = SessionHelper.GetUserId();
            int officeId = SessionHelper.GetOfficeId();

            this.cashflowStatementGridView.DataSource = Data.Reports.CashFlow.GetDirectCashFlowStatement(from, to, userId, officeId, factor);
            this.cashflowStatementGridView.DataBind();
        }


        private void CreateForm(Control container)
        {
            using (HtmlGenericControl formSegment = new HtmlGenericControl("div"))
            {
                formSegment.Attributes.Add("class", "ui form segment");

                using (HtmlGenericControl fields = HtmlControlHelper.GetFields("inline fields"))
                {
                    this.AddFromDateTextBoxField(fields);
                    this.AddToDateTextBoxField(fields);
                    this.AddFactorField(fields);
                    this.AddShowButton(fields);
                    this.AddPrintButton(fields);
                    formSegment.Controls.Add(fields);
                }
                container.Controls.Add(formSegment);
            }
        }


        private void ShowButton_Click(object sender, EventArgs e)
        {
            this.BindGrid();
        }

        #endregion
    }
}