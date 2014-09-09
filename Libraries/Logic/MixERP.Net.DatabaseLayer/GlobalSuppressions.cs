/********************************************************************************
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

// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.
//
// To add a suppression to this file, right-click the message in the
// Code Analysis results, point to "Suppress Message", and click
// "In Suppression File".
// You do not need to add suppressions to this file manually.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Scope = "member", Target = "MixERP.Net.DatabaseLayer.Helpers.FormHelper.#InsertRecord(System.String,System.String,System.Collections.Generic.List`1<System.Collections.Generic.KeyValuePair`2<System.String,System.String>>,System.String)")]
[assembly: SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Scope = "member", Target = "MixERP.Net.DatabaseLayer.Helpers.FormHelper.#UpdateRecord(System.String,System.String,System.Collections.Generic.List`1<System.Collections.Generic.KeyValuePair`2<System.String,System.String>>,System.String,System.String,System.String)")]
[assembly: SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Scope = "member", Target = "MixERP.Net.DatabaseLayer.Helpers.FormHelper.#GetView(System.String,System.String,System.String,System.Int32,System.Int32)")]
[assembly: SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Scope = "member", Target = "MixERP.Net.DatabaseLayer.Helpers.FormHelper.#GetTable(System.String,System.String)")]
[assembly: SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Scope = "member", Target = "MixERP.Net.DatabaseLayer.Helpers.FormHelper.#GetTable(System.String,System.String,System.String,System.String)")]
[assembly: SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Scope = "member", Target = "MixERP.Net.DatabaseLayer.Helpers.FormHelper.#GetTotalRecords(System.String,System.String)")]
[assembly: SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Scope = "member", Target = "MixERP.Net.DatabaseLayer.Helpers.FormHelper.#DeleteRecord(System.String,System.String,System.String,System.String)")]
[assembly: SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times", Scope = "member", Target = "MixERP.Net.DatabaseLayer.Helpers.FormHelper.#UpdateRecord(System.String,System.String,System.Collections.Generic.List`1<System.Collections.Generic.KeyValuePair`2<System.String,System.String>>,System.String,System.String,System.String)")]
[assembly: SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times", Scope = "member", Target = "MixERP.Net.DatabaseLayer.Helpers.FormHelper.#InsertRecord(System.String,System.String,System.Collections.Generic.List`1<System.Collections.Generic.KeyValuePair`2<System.String,System.String>>,System.String)")]
[assembly: SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Scope = "member", Target = "MixERP.Net.DatabaseLayer.Helpers.ReportHelper.#GetDataTable(System.String,System.Collections.Generic.List`1<System.Collections.Generic.KeyValuePair`2<System.String,System.String>>)")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "ip", Scope = "member", Target = "MixERP.Net.DatabaseLayer.Security.User.#SignIn(System.Int32,System.String,System.String,System.String,System.String,System.String)")]
[assembly: SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times", Scope = "member", Target = "MixERP.Net.DatabaseLayer.Helpers.FormHelper.#UpdateRecord(System.String,System.String,Collection`1<System.Collections.Generic.KeyValuePair`2<System.String,System.String>>,System.String,System.String,System.String)")]