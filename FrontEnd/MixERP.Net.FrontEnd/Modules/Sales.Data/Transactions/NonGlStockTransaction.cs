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
using MixERP.Net.Common.PostgresHelper;
using MixERP.Net.DBFactory;
using Npgsql;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;

namespace MixERP.Net.Core.Modules.Sales.Data.Transactions
{
    internal static class NonGlStockTransaction
    {
        internal static long Add(string book, DateTime valueDate, int officeId, int userId, long logOnId, string referenceNumber, string statementReference, StockMasterModel stockMaster, Collection<StockMasterDetailModel> details, Collection<long> transactionIdCollection, Collection<AttachmentModel> attachments, bool nonTaxable)
        {
            if (stockMaster == null)
            {
                return 0;
            }

            if (details == null)
            {
                return 0;
            }

            if (details.Count.Equals(0))
            {
                return 0;
            }

            string tranIds = ParameterHelper.CreateBigintArrayParameter(transactionIdCollection, "bigint", "@TranId");
            string detail = StockMasterDetailHelper.CreateStockMasterDetailParameter(details);
            string attachment = AttachmentHelper.CreateAttachmentModelParameter(attachments);

            string sql = string.Format(CultureInfo.InvariantCulture, "SELECT * FROM transactions.post_non_gl_transaction(@Book, @OfficeId, @UserId, @LoginId, @ValueDate, @ReferenceNumber, @StatementReference, @PartyCode, @PriceTypeId, @NonTaxable,@SalesPersonId, @ShipperId, @ShippingAddressCode, @StoreId, ARRAY[{0}]::bigint[], ARRAY[{1}], ARRAY[{2}]);", tranIds, detail, attachment);

            using (NpgsqlCommand command = new NpgsqlCommand(sql))
            {
                command.Parameters.AddWithValue("@Book", book);
                command.Parameters.AddWithValue("@OfficeId", officeId);
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@LoginId", logOnId);
                command.Parameters.AddWithValue("@ValueDate", valueDate);
                command.Parameters.AddWithValue("@ReferenceNumber", referenceNumber);
                command.Parameters.AddWithValue("@StatementReference", statementReference);
                command.Parameters.AddWithValue("@PartyCode", stockMaster.PartyCode);

                if (stockMaster.PriceTypeId.Equals(0))
                {
                    command.Parameters.AddWithValue("@PriceTypeId", DBNull.Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@PriceTypeId", stockMaster.PriceTypeId);
                }

                command.Parameters.AddWithValue("@NonTaxable", nonTaxable);
                command.Parameters.AddWithValue("@SalesPersonId", stockMaster.SalespersonId);
                command.Parameters.AddWithValue("@ShipperId", stockMaster.ShipperId);
                command.Parameters.AddWithValue("@ShippingAddressCode", stockMaster.ShippingAddressCode);
                command.Parameters.AddWithValue("@StoreId", stockMaster.StoreId);

                command.Parameters.AddRange(ParameterHelper.AddBigintArrayParameter(transactionIdCollection, "@TranId").ToArray());
                command.Parameters.AddRange(StockMasterDetailHelper.AddStockMasterDetailParameter(details).ToArray());
                command.Parameters.AddRange(AttachmentHelper.AddAttachmentParameter(attachments).ToArray());

                long tranId = Conversion.TryCastLong(DbOperation.GetScalarValue(command));
                return tranId;
            }
        }
    }
}