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
using System.Collections.Generic;
using System.Reflection;
using MixERP.Net.Common.Helpers;
using MixERP.Net.Core.Modules.BackOffice.Resources;
using MixERP.Net.FrontEnd.Base;
using MixERP.Net.FrontEnd.Controls;

namespace MixERP.Net.Core.Modules.BackOffice.Tax
{
    public partial class SalesTaxExempts : MixERPUserControl
    {
        public override void OnControlLoad(object sender, EventArgs e)
        {
            using (Scrud scrud = new Scrud())
            {
                scrud.KeyColumn = "sales_tax_exempt_id";
                scrud.TableSchema = "core";
                scrud.Table = "sales_tax_exempts";
                scrud.ViewSchema = "core";
                scrud.View = "sales_tax_exempt_scrud_view";
                scrud.Text = Titles.SalesTaxExempts;

                scrud.DisplayFields = GetDisplayFields();
                scrud.DisplayViews = GetDisplayViews();

                scrud.ResourceAssembly = Assembly.GetAssembly(typeof (SalesTaxExempts));

                this.AddScrudCustomValidatorErrorMessages();

                this.ScrudPlaceholder.Controls.Add(scrud);
            }
        }

        private void AddScrudCustomValidatorErrorMessages()
        {
            string javascript = JSUtility.GetVar("dateErrorMessageLocalized", Warnings.DateErrorMessage);
            javascript += JSUtility.GetVar("comparePriceErrorMessageLocalized", Warnings.ComparePriceErrorMessage);

            Common.PageUtility.RegisterJavascript("SalesTaxExempts_ScrudCustomValidatorErrorMessages", javascript, this.Page, true);
        }

        private static string GetDisplayFields()
        {
            List<string> displayFields = new List<string>();
            ScrudHelper.AddDisplayField(displayFields, "core.tax_master.tax_master_id", ConfigurationHelper.GetDbParameter("TaxMasterDisplayField"));
            ScrudHelper.AddDisplayField(displayFields, "core.tax_exempt_types.tax_exempt_type_id", ConfigurationHelper.GetDbParameter("TaxExemptTypeDisplayField"));
            ScrudHelper.AddDisplayField(displayFields, "core.sales_taxes.sales_tax_id", ConfigurationHelper.GetDbParameter("SalesTaxDisplayField"));
            ScrudHelper.AddDisplayField(displayFields, "office.stores.store_id", ConfigurationHelper.GetDbParameter("StoreDisplayField"));
            return string.Join(",", displayFields);
        }

        private static string GetDisplayViews()
        {
            List<string> displayViews = new List<string>();
            ScrudHelper.AddDisplayView(displayViews, "core.tax_master.tax_master_id", "core.tax_master_scrud_view");
            ScrudHelper.AddDisplayView(displayViews, "core.tax_exempt_types.tax_exempt_type_id", "core.tax_exempt_type_scrud_view");
            ScrudHelper.AddDisplayView(displayViews, "core.sales_taxes.sales_tax_id", "core.sales_tax_scrud_view");
            ScrudHelper.AddDisplayView(displayViews, "office.stores.store_id", "office.store_scrud_view");

            return string.Join(",", displayViews);
        }
    }
}