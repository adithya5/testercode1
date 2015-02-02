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

using MixERP.Net.Common.Helpers;
using MixERP.Net.Core.Modules.Inventory.Resources;
using MixERP.Net.FrontEnd.Base;
using MixERP.Net.WebControls.ScrudFactory;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace MixERP.Net.Core.Modules.Inventory.Setup
{
    public partial class CompoundItemDetails : MixERPUserControl
    {
        public override void OnControlLoad(object sender, EventArgs e)
        {
            using (ScrudForm scrud = new ScrudForm())
            {
                scrud.KeyColumn = "compound_item_detail_id";

                scrud.TableSchema = "core";
                scrud.Table = "compound_item_details";
                scrud.ViewSchema = "core";
                scrud.View = "compound_item_detail_scrud_view";

                scrud.DisplayFields = GetDisplayFields();
                scrud.DisplayViews = GetDisplayViews();

                scrud.Text = Titles.CompoundItemDetails;
                scrud.ResourceAssembly = Assembly.GetAssembly(typeof(CompoundItemDetails));

                this.ScrudPlaceholder.Controls.Add(scrud);
            }

            
        }

        private static string GetDisplayFields()
        {
            List<string> displayFields = new List<string>();
            ScrudHelper.AddDisplayField(displayFields, "core.compound_items.compound_item_id",
                ConfigurationHelper.GetDbParameter("CompoundItemDisplayField"));
            ScrudHelper.AddDisplayField(displayFields, "core.items.item_id",
                ConfigurationHelper.GetDbParameter("ItemDisplayField"));
            ScrudHelper.AddDisplayField(displayFields, "core.units.unit_id",
                ConfigurationHelper.GetDbParameter("UnitDisplayField"));
            return string.Join(",", displayFields);
        }

        private static string GetDisplayViews()
        {
            List<string> displayViews = new List<string>();
            ScrudHelper.AddDisplayView(displayViews, "core.compound_items.compound_item_id", "core.compound_item_selector_view");
            ScrudHelper.AddDisplayView(displayViews, "core.items.item_id", "core.item_selector_view");
            ScrudHelper.AddDisplayView(displayViews, "core.units.unit_id", "core.unit_selector_view");
            return string.Join(",", displayViews);
        }
    }
}