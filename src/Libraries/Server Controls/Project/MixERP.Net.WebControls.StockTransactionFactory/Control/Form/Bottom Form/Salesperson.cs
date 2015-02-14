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

using System.Web.UI.HtmlControls;
using MixERP.Net.Common.Helpers;
using MixERP.Net.WebControls.StockTransactionFactory.Resources;

namespace MixERP.Net.WebControls.StockTransactionFactory
{
    public partial class StockTransactionForm
    {
        private static void AddSalespersonField(HtmlGenericControl container)
        {
            using (HtmlGenericControl salespersonDiv = HtmlControlHelper.GetField())
            {
                salespersonDiv.ID = "SalespersonDiv";

                using (HtmlGenericControl label = HtmlControlHelper.GetLabel(Titles.Salesperson, "SalesPersonSelect"))
                {
                    salespersonDiv.Controls.Add(label);
                }

                using (HtmlSelect salesPersonSelect = new HtmlSelect())
                {
                    salesPersonSelect.ID = "SalesPersonSelect";
                    salespersonDiv.Controls.Add(salesPersonSelect);
                }


                container.Controls.Add(salespersonDiv);
            }
        }
    }
}