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

using MixERP.Net.Common;
using MixERP.Net.Common.Models.Transactions;
using MixERP.Net.FrontEnd.Base;
using MixERP.Net.WebControls.TransactionChecklist;
using System;

namespace MixERP.Net.Core.Modules.Sales.Confirmation
{
    public partial class Quotation : MixERPUserControl
    {
        public override void OnControlLoad(object sender, EventArgs e)
        {
            long transactionMasterId = Conversion.TryCastLong(this.Request["TranId"]);

            using (TransactionChecklistForm checklist = new TransactionChecklistForm())
            {
                checklist.ViewReportButtonText = Resources.Titles.ViewThisQuotation;
                checklist.EmailReportButtonText = Resources.Titles.EmailThisQuotation;
                checklist.Text = Resources.Titles.SalesQuotation;
                checklist.PartyEmailAddress = Data.Helpers.Parties.GetEmailAddress(TranBook.Sales, SubTranBook.Quotation, transactionMasterId);
                checklist.AttachmentBookName = "non-gl-transaction";
                checklist.OverridePath = "/Modules/Sales/Quotation.mix";
                checklist.DisplayViewReportButton = true;
                checklist.DisplayEmailReportButton = true;
                checklist.DisplayAttachmentButton = true;
                checklist.IsNonGlTransaction = true;
                checklist.ReportPath = "~/Modules/Sales/Reports/SalesQuotationReport.mix";
                checklist.ViewPath = "/Modules/Sales/Quotation.mix";
                checklist.AddNewPath = "/Modules/Sales/Entry/Quotation.mix";

                Placeholder1.Controls.Add(checklist);
            }

            base.OnControlLoad(sender, e);
        }
    }
}