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
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MixERP.Net.Common.Helpers;
using MixERP.Net.Core.Modules.Finance.Resources;
using MixERP.Net.FrontEnd.Base;
using MixERP.Net.FrontEnd.Controls;

namespace MixERP.Net.Core.Modules.Finance.Setup
{
    public partial class MerchantFeeSetup : MixERPUserControl
    {
        public override void OnControlLoad(object sender, EventArgs e)
        {
            using (Scrud scrud = new Scrud())
            {
                scrud.KeyColumn = "merchant_fee_setup_id";
                scrud.TableSchema = "core";
                scrud.Table = "merchant_fee_setup";
                scrud.ViewSchema = "core";
                scrud.View = "merchant_fee_setup";
                scrud.Text = Titles.MerchantFeeSetup;

                scrud.DisplayFields = GetDisplayFields();
                scrud.DisplayViews = GetDisplayViews();
                scrud.UseDisplayViewsAsParents = true;

                scrud.ResourceAssembly = Assembly.GetAssembly(typeof(PaymentCards));
                this.ScrudPlaceholder.Controls.Add(scrud);
            }
        }

        private static string GetDisplayFields()
        {
            List<string> displayFields = new List<string>();
            ScrudHelper.AddDisplayField(displayFields, "core.bank_accounts.account_id", ConfigurationHelper.GetDbParameter("BankAccountDisplayField"));
            ScrudHelper.AddDisplayField(displayFields, "core.payment_cards.payment_card_id", ConfigurationHelper.GetDbParameter("PaymentCardDisplayField"));
            return string.Join(",", displayFields);
        }

        private static string GetDisplayViews()
        {
            List<string> displayViews = new List<string>();
            ScrudHelper.AddDisplayView(displayViews, "core.bank_accounts.account_id", "core.merchant_account_selector_view");
            ScrudHelper.AddDisplayView(displayViews, "core.payment_cards.payment_card_id", "core.payment_cards");
            return string.Join(",", displayViews);
        }
    }
}