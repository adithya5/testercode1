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
using Npgsql;
using System;

namespace MixERP.Net.Core.Modules.Sales.Data.Transactions
{
    public static class Receipt
    {
        public static long PostTransaction(int userId, int officeId, long loginId, string partyCode, string currencyCode, decimal amount, decimal debitExchangeRate, decimal creditExchangeRate, string referenceNumber, string statementReference, int costCenterId, int cashRepositoryId, DateTime? postedDate, int bankAccountId, string bankInstrumentCode, string bankTransactionCode)
        {
            const string sql = "SELECT transactions.post_receipt_function(@UserId, @OfficeId, @LoginId, @PartyCode, @CurrencyCode, @Amount, @DebitExchangeRate, @CreditExchangeRate, @ReferenceNumber, @StatementReference, @CostCenterId, @CashRepositoryId, @PostedDate, @BankAccountId, @BankInstrumentCode, @BankTransactionCode); ";
            using (NpgsqlCommand command = new NpgsqlCommand(sql))
            {
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@OfficeId", officeId);
                command.Parameters.AddWithValue("@LoginId", loginId);
                command.Parameters.AddWithValue("@PartyCode", partyCode);
                command.Parameters.AddWithValue("@CurrencyCode", currencyCode);
                command.Parameters.AddWithValue("@Amount", amount);
                command.Parameters.AddWithValue("@DebitExchangeRate", debitExchangeRate);
                command.Parameters.AddWithValue("@CreditExchangeRate", creditExchangeRate);
                command.Parameters.AddWithValue("@ReferenceNumber", referenceNumber);
                command.Parameters.AddWithValue("@StatementReference", statementReference);
                command.Parameters.AddWithValue("@CostCenterId", costCenterId);

                if (cashRepositoryId.Equals(0))
                {
                    command.Parameters.AddWithValue("@CashRepositoryId", DBNull.Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@CashRepositoryId", cashRepositoryId);
                }

                if (bankAccountId.Equals(0))
                {
                    command.Parameters.AddWithValue("@BankAccountId", DBNull.Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@BankAccountId", bankAccountId);
                }

                if (postedDate == null)
                {
                    command.Parameters.AddWithValue("@PostedDate", DBNull.Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@PostedDate", postedDate);
                }

                command.Parameters.AddWithValue("@BankInstrumentCode", bankInstrumentCode);
                command.Parameters.AddWithValue("@BankTransactionCode", bankTransactionCode);

                return Conversion.TryCastLong(DBFactory.DbOperations.GetScalarValue(command));
            }
        }
    }
}