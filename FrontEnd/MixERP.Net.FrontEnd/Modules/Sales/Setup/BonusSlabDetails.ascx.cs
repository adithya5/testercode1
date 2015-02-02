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
using MixERP.Net.Core.Modules.Sales.Resources;
using MixERP.Net.FrontEnd.Base;
using MixERP.Net.WebControls.ScrudFactory;

namespace MixERP.Net.Core.Modules.Sales.Setup
{
    public partial class BonusSlabDetails : MixERPUserControl
    {
        public override void OnControlLoad(object sender, EventArgs e)
        {
            using (ScrudForm scrud = new ScrudForm())
            {
                scrud.KeyColumn = "bonus_slab_detail_id";
                scrud.TableSchema = "core";
                scrud.Table = "bonus_slab_details";
                scrud.ViewSchema = "core";
                scrud.View = "bonus_slab_detail_scrud_view";
                scrud.DisplayFields = GetDisplayFields();
                scrud.DisplayViews = GetDisplayViews();
                scrud.Text = Titles.BonusSlabDetails;
                scrud.ResourceAssembly = Assembly.GetAssembly(typeof(BonusSlabDetails));

                this.AddScrudCustomValidatorMessages();

                this.ScrudPlaceholder.Controls.Add(scrud);
            }
        }

        private static string GetDisplayFields()
        {
            List<string> displayFields = new List<string>();
            ScrudHelper.AddDisplayField(displayFields, "core.bonus_slabs.bonus_slab_id",
                ConfigurationHelper.GetDbParameter("BonusSlabDisplayField"));
            return string.Join(",", displayFields);
        }

        private static string GetDisplayViews()
        {
            List<string> displayViews = new List<string>();
            ScrudHelper.AddDisplayView(displayViews, "core.bonus_slabs.bonus_slab_id", "core.bonus_slab_selector_view");
            return string.Join(",", displayViews);
        }

        private void AddScrudCustomValidatorMessages()
        {
            string javascript = "var compareAmountErrorMessageLocalized='" + Warnings.CompareAmountErrorMessage + "';";
            Common.PageUtility.RegisterJavascript("BonusSlabDetails_CustomValidatorMessages", javascript, this.Page, true);
        }
    }
}