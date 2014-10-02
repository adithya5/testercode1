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
using MixERP.Net.Common.Models.Core;
using MixERP.Net.Common.Models.Transactions;
using MixERP.Net.DBFactory;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixERP.Net.Core.Modules.Sales.Data.Helpers
{
    public static class Return
    {
        public static long PostTransaction(long transactionMasterId, DateTime valueDate, int officeId, int userId, long loginId, int storeId, string partyCode, int priceTypeId, string referenceNumber, string statementReference, Collection<StockMasterDetailModel> details, Collection<AttachmentModel> attachments)
        {
            string detail = ParameterHelper.CreateStockMasterDetailParameter(details);
            string attachment = ParameterHelper.CreateAttachmentModelParameter(attachments);

            string sql = string.Format("SELECT * FROM transactions.post_sales_return(@TransactionMasterId, @OfficeId, @UserId, @LoginId, @ValueDate, @StoreId, @PartyCode, @PriceTypeId, @ReferenceNumber, @StatementReference, ARRAY[{0}], ARRAY[{1}]);", detail, attachment);

            using (NpgsqlCommand command = new NpgsqlCommand(sql))
            {
                command.Parameters.AddWithValue("@TransactionMasterId", transactionMasterId);
                command.Parameters.AddWithValue("@OfficeId", officeId);
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@LoginId", loginId);
                command.Parameters.AddWithValue("@ValueDate", valueDate);
                command.Parameters.AddWithValue("@StoreId", storeId);
                command.Parameters.AddWithValue("@PartyCode", partyCode);
                command.Parameters.AddWithValue("@PriceTypeId", priceTypeId);
                command.Parameters.AddWithValue("@ReferenceNumber", referenceNumber);
                command.Parameters.AddWithValue("@StatementReference", statementReference);

                command.Parameters.AddRange(ParameterHelper.AddStockMasterDetailParameter(details).ToArray());
                command.Parameters.AddRange(ParameterHelper.AddAttachmentParameter(attachments).ToArray());

                long tranId = Conversion.TryCastLong(DbOperations.GetScalarValue(command));
                MixERP.Net.TransactionGovernor.Autoverification.Autoverify.PassTransactionMasterId(tranId);
                return tranId;
            }
        }
    }
}